from __future__ import annotations

from typing import Any, Dict, List, Optional
import pyodbc

CONN_STR = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=localhost;"
    "Database=your_database;"
    "Trusted_Connection=yes;"
)


class InvDetails:
    """Python equivalent of the C# InvDetails backend logic."""

    def __init__(self, conn_str: str = CONN_STR):
        self.conn_str = conn_str

    def get_connection(self):
        return pyodbc.connect(self.conn_str)

    def load_project(self, project_id: int) -> str:
        sql = "SELECT Name FROM dbo.DLI_Projects WHERE ProjectId = ?"
        with self.get_connection() as cn:
            row = cn.cursor().execute(sql, (project_id,)).fetchone()
            if row is None:
                return "Ismeretlen projekt"
            return f"Projekt név: {row[0]}"

    def bind_items(self, project_id: int, filter_text: Optional[str] = None) -> List[Dict[str, Any]]:
        sql = """
            SELECT p.PartId, p.Barcode, p.Name, p.Category, p.Location, a.AssignedAt
            FROM dbo.DLI_Parts p
            JOIN dbo.DLI_PartAssignments a ON a.PartId = p.PartId AND a.IsActive = 1
            WHERE a.ProjectId = ?
        """
        params: List[Any] = [project_id]

        if filter_text and filter_text.strip():
            sql += " AND (p.Name LIKE ? OR p.Barcode LIKE ? OR p.Category LIKE ? OR p.Location LIKE ?)"
            q = f"%{filter_text.strip()}%"
            params.extend([q, q, q, q])

        sql += " ORDER BY p.Category, p.Barcode"

        rows: List[Dict[str, Any]] = []
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for row in cursor.execute(sql, params):
                rows.append(
                    {
                        "PartId": row.PartId,
                        "Barcode": row.Barcode,
                        "Name": row.Name,
                        "Category": row.Category,
                        "Location": row.Location,
                        "AssignedAt": row.AssignedAt,
                    }
                )
        return rows

    def unassign_selected(self, project_id: int, part_ids: List[int]) -> None:
        if not part_ids:
            return
        placeholders = ", ".join("?" for _ in part_ids)
        sql = (
            "UPDATE dbo.DLI_PartAssignments SET IsActive=0 "
            "WHERE IsActive=1 AND PartId IN (" + placeholders + ") AND ProjectId = ?"
        )
        params = part_ids + [project_id]
        with self.get_connection() as cn:
            cn.cursor().execute(sql, params)
            cn.commit()


if __name__ == "__main__":
    service = InvDetails()
    print(service.bind_items(1))
