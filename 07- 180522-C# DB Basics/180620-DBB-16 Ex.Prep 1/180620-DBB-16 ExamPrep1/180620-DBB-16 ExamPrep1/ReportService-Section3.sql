--05 Users by Age
SELECT u.Username, u.Age
FROM Users AS u
ORDER BY u.Age, u.Username DESC

--06 Unassigned Reports
SELECT r.Description, r.OpenDate
FROM Reports AS r
WHERE r.EmployeeId IS NULL
ORDER BY r.OpenDate, r.Description

--07 Employees & Reports
SELECT e.FirstName, e.LastName, r.Description, FORMAT(r.OpenDate, 'yyyy-MM-dd') AS [OpenDate]
FROM Employees AS e
JOIN Reports AS r ON r.EmployeeId = e.Id
ORDER BY e.Id, r.OpenDate, r.Id

--08 Most reported Category
SELECT c.Name AS [CategoryName], COUNT(c.Name) AS [ReportsNumber]
FROM Categories AS c
JOIN Reports AS r ON r.CategoryId = c.Id
GROUP BY c.Name
ORDER BY [ReportsNumber] DESC, [CategoryName]

--09 Employees in Category
SELECT c.Name AS [CategoryName], COUNT(e.Id) AS [EmployeesNumber]
FROM Categories AS c
JOIN Departments AS d ON d.Id = c.DepartmentId
JOIN Employees AS e ON e.DepartmentId = d.Id
GROUP BY c.Name

--10 Users per Employee
SELECT CONCAT(e.FirstName,' ',e.LastName) AS [Name], COUNT(DISTINCT r.UserId) AS [Users Number]
FROM Employees AS e
LEFT JOIN Reports AS r ON r.EmployeeId = e.Id
GROUP BY  CONCAT(e.FirstName,' ',e.LastName)
ORDER BY [Users Number] DESC, [Name]

--11 Emergency Patrol
SELECT r.OpenDate, r.Description, u.Email AS [Reporter Email]
FROM Reports AS r
JOIN Users AS u ON u.Id = R.UserId
JOIN Categories AS c ON c.Id = r.CategoryId
JOIN Departments AS d ON d.Id = c.DepartmentId
WHERE r.CloseDate IS NULL
	  AND LEN(r.Description) > 20 
	  AND r.Description LIKE '%str%'
	  AND d.Name IN ('Infrastructure', 'Emergency', 'Roads Maintenance')
ORDER BY r.OpenDate, [Reporter Email], r.Id

--12 Birthday Report
SELECT DISTINCT c.Name AS [Category Name]
FROM Reports AS r
JOIN Users AS u ON u.Id = r.UserId
JOIN Categories AS c ON c.Id = r.CategoryId
WHERE FORMAT(r.OpenDate,'MM-dd') = FORMAT(u.BirthDate,'MM-dd')
ORDER BY [Category Name]

--13 Numbers Coincidence
SELECT DISTINCT u.Username
FROM Users AS u
JOIN Reports AS r ON r.UserId = u.Id
JOIN Categories AS c ON c.Id = r.CategoryId
WHERE (u.Username LIKE '[0-9]%'
	  AND LEFT(u.Username,1) = CAST(c.Id AS VARCHAR(2)))
	  OR (u.Username LIKE '%[0-9]'
	  AND RIGHT(u.Username,1) = CAST(c.Id AS VARCHAR(2)))
ORDER BY u.Username

--14 Open/Close Statistics
SELECT CONCAT(e.FirstName,' ', e.LastName) AS [Name], CONCAT(ISNULL(cr.[Closed Reports], 0), '/',ISNULL(oc.[Open Reports],0)) AS [Closed Open Reports]
FROM Employees AS e
JOIN (SELECT r.EmployeeId, COUNT(*) AS [Open Reports]
	  FROM Reports AS r
	  WHERE DATEPART(YEAR, r.OpenDate) = 2016
	  GROUP BY r.EmployeeId
		) AS oc ON oc.EmployeeId = e.Id
LEFT JOIN (SELECT r.EmployeeId, COUNT(*) AS [Closed Reports]
	  FROM Reports AS r
	  WHERE DATEPART(YEAR, r.CloseDate) = 2016 
	  GROUP BY r.EmployeeId
		) AS cr ON cr.EmployeeId = e.Id
ORDER BY [Name], e.Id

--15 Average Closing Time
SELECT dd.[Department Name], ISNULL(CAST(dd.[Average Duration] AS VARCHAR), 'no info') AS [Average Duration]
FROM (SELECT d.Name AS [Department Name], AVG(DATEDIFF(DAY, r.OpenDate, r.CloseDate)) AS [Average Duration]
	  FROM Departments AS d
	  JOIN Categories AS c ON c.DepartmentId = d.Id
	  JOIN Reports AS r ON r.CategoryId = c.Id
	  GROUP BY d.Name) AS dd
ORDER BY dd.[Department Name]

--16 Favorite Categories
WITH CTE_ReportsDistribution AS (
SELECT d.Name AS [Department Name], c.Name AS [Category Name], COUNT(*) AS [ReportsCount]
FROM Departments AS d
JOIN Categories AS c ON c.DepartmentId = d.Id
JOIN Reports AS r ON r.CategoryId = c.Id
GROUP BY d.Name, c.Name
)

SELECT rd1.[Department Name], rd1.[Category Name], ROUND(((CAST(rd1.ReportsCount AS FLOAT) / CAST(rd2.TotalReports AS FLOAT)) * 100),0) AS [Percentage]
FROM CTE_ReportsDistribution AS rd1
JOIN (
	  SELECT [Department Name], SUM(ReportsCount) AS [TotalReports]
	  FROM CTE_ReportsDistribution AS rd
	  GROUP BY rd.[Department Name]
) AS rd2 ON rd2.[Department Name] = rd1.[Department Name]
ORDER BY rd1.[Department Name], rd1.[Category Name], [Percentage]

