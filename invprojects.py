from __future__ import annotations

from typing import Any, Dict, List, Optional
import pyodbc

CONN_STR = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=localhost;"
    "Database=your_database;"
    "Trusted_Connection=yes;"
)


class InvProjects:
    """Python equivalent of the C# InvProjects backend logic."""

    def __init__(self, conn_str: str = CONN_STR):
        self.conn_str = conn_str

    def get_connection(self):
        return pyodbc.connect(self.conn_str)

    def bind(self, filter_text: Optional[str] = None) -> List[Dict[str, Any]]:
        sql = "SELECT ProjectId, Name, Code, [User], IsActive, Place, SecUser FROM dbo.DLI_Projects"
        params: List[Any] = []

        if filter_text and filter_text.strip():
            sql += " WHERE Name LIKE ? OR Code LIKE ?"
            q = f"%{filter_text.strip()}%"
            params.extend([q, q])

        sql += " ORDER BY Name"

        rows: List[Dict[str, Any]] = []
        with self.get_connection() as cn:
            cursor = cn.cursor()
            for row in cursor.execute(sql, params):
                rows.append(
                    {
                        "ProjectId": row.ProjectId,
                        "Name": row.Name,
                        "Code": row.Code,
                        "User": row.User,
                        "IsActive": bool(row.IsActive),
                        "Place": row.Place,
                        "SecUser": row.SecUser,
                    }
                )
        return rows

    def add_project(
        self,
        name: str,
        code: Optional[str] = None,
        user: Optional[str] = None,
        place: Optional[str] = None,
        sec_user: Optional[str] = None,
    ) -> None:
        if not name or not name.strip():
            raise ValueError("Adj nevet.")

        sql = (
            "INSERT INTO dbo.DLI_Projects(Name, Code, IsActive, CreatedAt, [User], Place, SecUser) "
            "VALUES(?, ?, 1, SYSDATETIME(), ?, ?, ?)"
        )
        with self.get_connection() as cn:
            cn.cursor().execute(
                sql,
                (
                    name.strip(),
                    code.strip() if code and code.strip() else None,
                    user.strip() if user and user.strip() else None,
                    place.strip() if place and place.strip() else None,
                    sec_user.strip() if sec_user and sec_user.strip() else None,
                ),
            )
            cn.commit()

    def update_project(
        self,
        project_id: int,
        name: str,
        code: Optional[str],
        user: Optional[str],
        place: Optional[str],
        sec_user: Optional[str],
        is_active: bool,
    ) -> None:
        if not name or not name.strip():
            raise ValueError("Név kötelező.")

        sql = (
            "UPDATE dbo.DLI_Projects SET Name=?, Code=?, IsActive=?, [User]=?, Place=?, SecUser=? "
            "WHERE ProjectId=?"
        )
        with self.get_connection() as cn:
            cn.cursor().execute(
                sql,
                (
                    name.strip(),
                    code.strip() if code and code.strip() else None,
                    1 if is_active else 0,
                    user.strip() if user and user.strip() else None,
                    place.strip() if place and place.strip() else None,
                    sec_user.strip() if sec_user and sec_user.strip() else None,
                    project_id,
                ),
            )
            cn.commit()

    def delete_project(self, project_id: int) -> None:
        with self.get_connection() as cn:
            cursor = cn.cursor()
            cursor.execute("DELETE FROM DLI_PartAssignments WHERE ProjectId = ?", (project_id,))
            cursor.execute("DELETE FROM DLI_Projects WHERE ProjectId = ?", (project_id,))
            cn.commit()

    def load_kpis(self) -> Dict[str, int]:
        sql = """
            SELECT
              (SELECT COUNT(*) FROM DLI_Projects) AS AllCnt,
              (SELECT COUNT(*) FROM DLI_Projects WHERE IsActive=1) AS ActiveCnt,
              (SELECT COUNT(*) FROM DLI_Projects WHERE IsActive=0) AS InactiveCnt
        """
        with self.get_connection() as cn:
            row = cn.cursor().execute(sql).fetchone()
            return {
                "AllCnt": row.AllCnt,
                "ActiveCnt": row.ActiveCnt,
                "InactiveCnt": row.InactiveCnt,
            }


if __name__ == "__main__":
    service = InvProjects()
    print(service.bind())
