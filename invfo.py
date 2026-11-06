from __future__ import annotations

from typing import Any, Dict, List
import pyodbc

CONN_STR = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=localhost;"
    "Database=your_database;"
    "Trusted_Connection=yes;"
)


class InvFo:
    """Python equivalent of the C# InvFo dashboard backend."""

    def __init__(self, conn_str: str = CONN_STR):
        self.conn_str = conn_str

    def get_connection(self):
        return pyodbc.connect(self.conn_str)

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
            row = cn.cursor().execute(sql).fetchone()
            return {
                "AllCnt": row.AllCnt,
                "AssignedCnt": row.AssignedCnt,
                "FreeCnt": row.FreeCnt,
            }

    def load_project_kpis(self) -> Dict[str, int]:
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

    def load_top_categories(self) -> List[Dict[str, Any]]:
        sql = """
            SELECT ISNULL(NULLIF(LTRIM(RTRIM(Category)),''),'(nincs)') AS Category, COUNT(*) AS Cnt
            FROM dbo.DLI_Parts
            GROUP BY ISNULL(NULLIF(LTRIM(RTRIM(Category)),''),'(nincs)')
            ORDER BY COUNT(*) DESC
        """
        rows: List[Dict[str, Any]] = []
        with self.get_connection() as cn:
            for row in cn.cursor().execute(sql):
                rows.append({"Category": row.Category, "Cnt": row.Cnt})
        return rows

    def load_top_projects(self) -> List[Dict[str, Any]]:
        sql = """
            SELECT p.ProjectId, p.Name, COUNT(a.PartId) AS Cnt
            FROM dbo.DLI_Projects p
            LEFT JOIN dbo.DLI_PartAssignments a ON a.ProjectId = p.ProjectId AND a.IsActive = 1
            GROUP BY p.ProjectId, p.Name
            ORDER BY COUNT(a.PartId) DESC
        """
        rows: List[Dict[str, Any]] = []
        with self.get_connection() as cn:
            for row in cn.cursor().execute(sql):
                rows.append({"ProjectId": row.ProjectId, "Name": row.Name, "Cnt": row.Cnt})
        return rows


if __name__ == "__main__":
    service = InvFo()
    print(service.load_kpis())
