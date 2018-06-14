--01Employee Address
SELECT TOP(5) e.EmployeeID, e.JobTitle, e.AddressID, a.AddressText
FROM Employees AS e
JOIN Addresses AS a ON a.AddressID = e.AddressID
ORDER BY e.AddressID

--02 Addresses with Towns
SELECT TOP(50) e.FirstName, e.LastName, t.Name, a.AddressText
FROM Employees AS e
JOIN Addresses AS a ON a.AddressID = e.AddressID
JOIN Towns AS t ON t.TownID = a.TownID
ORDER BY e.FirstName, e.LastName

--03 Sales Employee
SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name
FROM Employees AS e
JOIN Departments AS d ON (d.DepartmentID = e.DepartmentID AND d.Name = 'Sales')
ORDER BY e.EmployeeID

--04 Employee Departments
SELECT TOP(5) e.EmployeeID, e.FirstName, e.Salary, d.Name AS [DepartmentName]
FROM Employees AS e
JOIN Departments AS d ON (d.DepartmentID = e.DepartmentID AND e.Salary > 15000)
ORDER BY e.DepartmentID

--05 Employees Without Project
SELECT TOP(3) emp.EmployeeID, emp.FirstName
FROM Employees AS emp
LEFT JOIN (
	SELECT DISTINCT EmployeeID
	FROM EmployeesProjects
) AS e ON (e.EmployeeID = emp.EmployeeID)
WHERE e.EmployeeID IS NULL
ORDER BY emp.EmployeeID

--06 Employees Hired After
SELECT e.FirstName, e.LastName, e.HireDate, d.Name AS [DeptName]
FROM Employees AS e
JOIN Departments AS d ON (d.DepartmentID = e.DepartmentID 
                          AND e.HireDate > '1.1.1999' 
                          AND d.Name IN ('Sales', 'Finance'))
ORDER BY e.HireDate

--07 Employees with Project
SELECT TOP(5) e.EmployeeID, e.FirstName, p.Name AS [ProjectName]
FROM Employees AS e
JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
JOIN (
	SELECT * 
	FROM Projects AS p
	WHERE p.StartDate > '08.13.2002' AND p.EndDate IS NULL
) AS p ON p.ProjectID = ep.ProjectID
ORDER BY e.EmployeeID

--08 Employee 24
SELECT e.EmployeeID, e.FirstName, 
	  CASE 
			WHEN DATEPART(YEAR, p.StartDate) >= '2005' THEN NULL
			ELSE p.Name
	   END AS [ProjectName]
FROM Employees AS e
JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
JOIN Projects AS p ON p.ProjectID = ep.ProjectID
WHERE e.EmployeeID = 24

--09 Employee Manager
SELECT e.EmployeeID, e.FirstName, e.ManagerID, m.FirstName AS [ManagerName]
FROM Employees AS e
JOIN Employees AS m ON (m.EmployeeID = e.ManagerID AND e.ManagerID IN (3, 7))
ORDER BY e.EmployeeID


--10 Employee Summary
SELECT TOP(50)  e.EmployeeID, 
				CONCAT(e.FirstName,' ', e.LastName) AS [EmployeeName],
				CONCAT(m.FirstName,' ', m.LastName) AS [ManagerName],
				d.Name AS [DepartmentName]
FROM Employees AS e
JOIN Employees AS m ON m.EmployeeID = e.ManagerID
JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
ORDER BY e.EmployeeID

--11 Min Average Salary
SELECT MIN(a.AverageSalary)
FROM(
	   SELECT AVG(e.Salary) AS [AverageSalary]
	   FROM Employees AS e
	   GROUP BY e.DepartmentID
) AS [a]

--12 Highest Peaks in Bulgaria
SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation
FROM Peaks AS p
JOIN Mountains AS m ON p.MountainId = m.Id
JOIN MountainsCountries AS mc ON (mc.MountainId = p.MountainId AND mc.CountryCode = 'BG' AND p.Elevation > 2835)
ORDER BY p.Elevation DESC

--13 Count Mountain Ranges
SELECT c.CountryCode, cc.MountainRanges
FROM Countries AS c
JOIN(
	SELECT mc.CountryCode, COUNT(mc.MountainId) AS [MountainRanges]
	FROM MountainsCountries AS mc
	GROUP BY mc.CountryCode
	) AS cc 
ON (cc.CountryCode = c.CountryCode AND c.CountryName IN ('United States', 'Russia', 'Bulgaria'))

--14 Countries with or without Rivers
SELECT TOP(5) c.CountryName, rc.RiverName 
FROM Countries AS c
JOIN Continents AS co ON (co.ContinentCode = c.ContinentCode AND co.ContinentName = 'Africa')
LEFT JOIN(
	SELECT r.RiverName, cr.CountryCode
	FROM Rivers AS r
	JOIN CountriesRivers AS cr ON cr.RiverId = r.Id
) AS rc 
ON rc.CountryCode = c.CountryCode
ORDER BY c.CountryName

--15 Countinents and Currencies
WITH CTE_ContinentInfo(ContinentCode, CurrencyCode, CurrencyUsage) AS(
SELECT c.ContinentCode, c.CurrencyCode, COUNT(c.CurrencyCode)
FROM Countries AS c
GROUP BY c.ContinentCode, c.CurrencyCode
HAVING COUNT(CurrencyCode) > 1)


SELECT cc.ContinentCode, ci.CurrencyCode, ci.CurrencyUsage
FROM (
SELECT ContinentCode, MAX(CurrencyUsage) AS MaxCurrency 
FROM CTE_ContinentInfo
GROUP BY ContinentCode) AS cc
JOIN CTE_ContinentInfo AS ci ON (ci.ContinentCode = cc.ContinentCode AND ci.CurrencyUsage = cc.MaxCurrency)



--16 Countries without any Mountains
SELECT COUNT(*) AS [CountryCode]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
WHERE mc.MountainId IS NULL

--17 Highest Peak and Longest Rover by Country
SELECT TOP (5) c.CountryName, 
			   MAX(p.Elevation) AS [HighestPeakElevation],
			   MAX(r.Length) AS [LongestRiverLength]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Peaks AS p ON p.MountainId = mc.MountainId
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC, c.CountryName

--18 Highest Peak Name and Elevation By Country
WITH CTE_CountriesInfo (CountryName, PeakName, Elevation, Mountain) AS (
SELECT c.CountryName, p.PeakName, MAX(p.Elevation), m.MountainRange
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
LEFT JOIN Peaks AS p ON p.MountainId = mc.MountainId
GROUP BY c.CountryName, p.PeakName, m.MountainRange )

SELECT TOP (5) cn.CountryName, 
	   ISNULL(cci.PeakName, '(no highest peak)') AS [Highest Peak Name], 
	   ISNULL(cci.Elevation, 0) AS [Highest Peak Elevation],
	   ISNULL(cci.Mountain, '(no mountain)')
FROM(
SELECT CountryName, MAX(Elevation) AS MaxElevation
  FROM CTE_CountriesInfo
GROUP BY CountryName ) AS cn
LEFT JOIN CTE_CountriesInfo AS cci ON cci.CountryName = cn.CountryName AND cci.Elevation = cn.MaxElevation
ORDER BY cn.CountryName, cci.PeakName

SELECT * FROM Countries
SELECT * FROM Rivers
SELECT * FROM Continents
SELECT * FROM MountainsCountries



