from __future__ import annotations

from typing import Any, Dict, Iterable, List, Optional
import pyodbc

CONN_STR = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=localhost;"
    "Database=your_database;"
    "Trusted_Connection=yes;"
)


class Inv:
    """Python equivalent of the C# Inv backend logic."""

    def __init__(self, conn_str: str = CONN_STR):
        self.conn_str = conn_str
        self.bin_ids: set[int] = set()

    def get_connection(self):
        return pyodbc.connect(self.conn_str)

    def bind_projects(self) -> List[Dict[str, Any]]:
        sql = "SELECT ProjectId, Name FROM DLI_Projects WHERE IsActive = 1 ORDER BY Name"
        rows: List[Dict[str, Any]] = []
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for project_id, name in cursor.execute(sql):
                rows.append({"ProjectId": project_id, "Name": name})
        return rows

    def bind_categories(self) -> List[str]:
        sql = (
            "SELECT DISTINCT Category FROM DLI_Parts "
            "WHERE Category IS NOT NULL AND LTRIM(RTRIM(Category)) <> '' "
            "ORDER BY Category"
        )
        rows: List[str] = []
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for (category,) in cursor.execute(sql):
                rows.append(category)
        return rows

    def get_parts(
        self,
        search: Optional[str] = None,
        category: Optional[str] = None,
        status: Optional[str] = None,
        project_id: Optional[int] = None,
    ) -> List[Dict[str, Any]]:
        sql = """
            SELECT p.PartId, p.Barcode, p.Name, p.Category, p.Location, p.CreatedAt,
                   a.ProjectId, pr.Name AS ProjectName
            FROM DLI_Parts p
            LEFT JOIN DLI_PartAssignments a ON a.PartId = p.PartId AND a.IsActive = 1
            LEFT JOIN DLI_Projects pr ON pr.ProjectId = a.ProjectId
            WHERE 1=1
        """
        params: List[Any] = []

        if search and search.strip():
            sql += " AND (p.Name LIKE ? OR p.Barcode LIKE ?)"
            q = f"%{search.strip()}%"
            params.extend([q, q])

        if category:
            sql += " AND p.Category = ?"
            params.append(category)

        if status:
            if status == "free":
                sql += " AND a.ProjectId IS NULL"
            elif status == "assigned":
                sql += " AND a.ProjectId IS NOT NULL"

        if project_id is not None:
            sql += " AND a.ProjectId = ?"
            params.append(project_id)

        sql += " ORDER BY p.Category, p.Barcode"

        results: List[Dict[str, Any]] = []
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for row in cursor.execute(sql, params):
                results.append(
                    {
                        "PartId": row.PartId,
                        "Barcode": row.Barcode,
                        "Name": row.Name,
                        "Category": row.Category,
                        "Location": row.Location,
                        "CreatedAt": row.CreatedAt,
                        "ProjectId": row.ProjectId,
                        "ProjectName": row.ProjectName,
                    }
                )
        return results

    def get_parts_by_ids(self, ids: Iterable[int]) -> List[Dict[str, Any]]:
        ids = list(ids)
        if not ids:
            return []

        placeholders = ", ".join("?" for _ in ids)
        sql = f"""
            SELECT p.PartId, p.Barcode, p.Name, p.Category, p.Location,
                   a.ProjectId, pr.Name AS ProjectName
            FROM DLI_Parts p
            LEFT JOIN DLI_PartAssignments a ON a.PartId = p.PartId AND a.IsActive = 1
            LEFT JOIN DLI_Projects pr ON pr.ProjectId = a.ProjectId
            WHERE p.PartId IN ({placeholders})
        """

        results: List[Dict[str, Any]] = []
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for row in cursor.execute(sql, ids):
                results.append(
                    {
                        "PartId": row.PartId,
                        "Barcode": row.Barcode,
                        "Name": row.Name,
                        "Category": row.Category,
                        "Location": row.Location,
                        "ProjectId": row.ProjectId,
                        "ProjectName": row.ProjectName,
                    }
                )
        return results

    def load_kpis(self) -> Dict[str, int]:
        sql = """
            SELECT
              (SELECT COUNT(*) FROM dbo.DLI_Parts) AS AllCnt,
              (SELECT COUNT(*) FROM dbo.DLI_PartAssignments WHERE IsActive=1) AS AssignedCnt,
              (SELECT COUNT(*) FROM dbo.DLI_Parts p WHERE NOT EXISTS(
                   SELECT 1 FROM dbo.DLI_PartAssignments a WHERE a.PartId=p.PartId AND a.IsActive=1
              )) AS FreeCnt
        """
        with self.get_connection() as cn:
            cursor = cn.cursor()
            row = cursor.execute(sql).fetchone()
            return {
                "AllCnt": row.AllCnt,
                "AssignedCnt": row.AssignedCnt,
                "FreeCnt": row.FreeCnt,
            }

    def build_groups(self, parts: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
        grouped: Dict[str, List[Dict[str, Any]]] = {}
        for part in parts:
            key = str(part.get("Barcode") or "").strip()
            grouped.setdefault(key, []).append(part)

        result: List[Dict[str, Any]] = []
        for key, group in sorted(
            grouped.items(), key=lambda item: (-len(item[1]), item[0])
        ):
            sample_name = ""
            for part in group:
                name = str(part.get("Name") or "").strip()
                if name:
                    sample_name = name
                    break
            result.append(
                {
                    "Barcode": key,
                    "Count": len(group),
                    "SampleName": sample_name,
                    "KeySafe": self.make_safe_key(key),
                }
            )
        return result

    @staticmethod
    def make_safe_key(value: str) -> str:
        if not value:
            return "empty"
        chars = []
        for ch in value:
            ok = ("a" <= ch <= "z") or ("A" <= ch <= "Z") or ("0" <= ch <= "9") or ch in ("-", "_")
            chars.append(ch if ok else "_")
        return "".join(chars)

    def add_part_to_db(self, barcode: str, name: str, category: str, location: str) -> int:
        sql = """
            INSERT INTO DLI_Parts(Barcode, Name, Category, Location, CreatedAt)
            VALUES(?, ?, ?, ?, SYSDATETIME());
            SELECT CAST(SCOPE_IDENTITY() AS int);
        """
        with self.get_connection() as cn:
            cursor = cn.cursor()
            row = cursor.execute(
                sql,
                (
                    barcode or None,
                    name or None,
                    category or None,
                    location or None,
                ),
            )
            inserted_id = row.fetchone()[0]
            cn.commit()
            return int(inserted_id)

    def assign_parts_to_project(self, part_ids: Iterable[int], project_id: int) -> None:
        ids = list(dict.fromkeys(part_ids))
        if not ids:
            return
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for part_id in ids:
                cursor.execute(
                    "UPDATE DLI_PartAssignments SET IsActive=0 WHERE PartId=? AND IsActive=1",
                    (part_id,),
                )
                cursor.execute(
                    "INSERT INTO DLI_PartAssignments(PartId, ProjectId, AssignedAt, IsActive) VALUES(?, ?, SYSDATETIME(), 1)",
                    (part_id, project_id),
                )
            cn.commit()

    def unassign_parts(self, part_ids: Iterable[int]) -> None:
        ids = list(part_ids)
        if not ids:
            return
        placeholders = ", ".join("?" for _ in ids)
        sql = f"UPDATE DLI_PartAssignments SET IsActive=0 WHERE PartId IN ({placeholders}) AND IsActive=1"
        with self.get_connection() as cn:
            cn.cursor().execute(sql, ids)
            cn.commit()

    def delete_parts(self, part_ids: Iterable[int]) -> None:
        ids = list(part_ids)
        if not ids:
            return
        placeholders = ", ".join("?" for _ in ids)
        with self.get_connection() as cn:
            cursor = cn.cursor()
            cursor.execute(f"DELETE FROM DLI_PartAssignments WHERE PartId IN ({placeholders})", ids)
            cursor.execute(f"DELETE FROM DLI_Parts WHERE PartId IN ({placeholders})", ids)
            cn.commit()

    def add_new_parts_from_rows(self, rows: List[Dict[str, Any]]) -> int:
        inserted = 0
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for row in rows:
                name = str(row.get("Name") or "").strip()
                if not name:
                    continue
                barcode = str(row.get("Barcode") or "").strip()
                category = str(row.get("Category") or "").strip()
                location = str(row.get("Location") or "").strip()
                repeat_count = int(row.get("Number") or 1)
                for _ in range(max(repeat_count, 1)):
                    cursor.execute(
                        "INSERT INTO DLI_Parts(Barcode, Name, Category, Location, CreatedAt) VALUES(?, ?, ?, ?, SYSDATETIME())",
                        (
                            barcode or None,
                            name,
                            category or None,
                            location or None,
                        ),
                    )
                    inserted += 1
            cn.commit()
        return inserted


if __name__ == "__main__":
    service = Inv()
    print(service.bind_projects())
