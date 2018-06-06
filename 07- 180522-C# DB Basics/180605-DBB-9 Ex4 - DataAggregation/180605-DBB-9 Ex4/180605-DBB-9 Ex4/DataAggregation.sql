SELECT * FROM WizzardDeposits

--01 Records' Count
SELECT COUNT(*)  AS [Count]
FROM WizzardDeposits 

--02 Longest Magic Wand
SELECT MAX(w.MagicWandSize) AS [LongestMagicWand]
FROM WizzardDeposits AS w

--03 Longest Magic Wand per Deposit Groups
SELECT w.DepositGroup, MAX(w.MagicWandSize) AS [LongestMagicWand]
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup

--04 Smallest Deposit Group per Magic Wand Size
SELECT TOP(2) w.DepositGroup
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup
ORDER BY  AVG(w.MagicWandSize)

--05 Deposits Sum
SELECT w.DepositGroup, SUM(w.DepositAmount) AS TotalSum
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup

--06 Deposits Sum for Ollivander Family
SELECT w.DepositGroup, SUM(w.DepositAmount) AS TotalSum
FROM WizzardDeposits AS w
WHERE w.MagicWandCreator = 'Ollivander Family'
GROUP BY w.DepositGroup

--07 Deposits Filter
SELECT w.DepositGroup, SUM(w.DepositAmount) AS TotalSum
FROM WizzardDeposits AS w
WHERE w.MagicWandCreator = 'Ollivander Family'
GROUP BY w.DepositGroup
HAVING SUM(w.DepositAmount) < 150000
ORDER BY TotalSum DESC

--08 Deposit Charge
SELECT w.DepositGroup, w.MagicWandCreator, MIN(w.DepositCharge) AS [MinDepositCharge]
FROM WizzardDeposits AS w
GROUP BY w.DepositGroup, w.MagicWandCreator
ORDER BY w.MagicWandCreator, w.DepositGroup

--09 Age Groups
SELECT ageGroups.AgeGroup, COUNT(*) AS [WizardCount]
	FROM ( 
	SELECT
		CASE
			WHEN w.Age <= 10 THEN '[0-10]'
			WHEN w.Age <= 20 THEN '[11-20]'
			WHEN w.Age <= 30 THEN '[21-30]'
			WHEN w.Age <= 40 THEN '[31-40]'
			WHEN w.Age <= 50 THEN '[41-50]'
			WHEN w.Age <= 60 THEN '[51-60]'
			ELSE '[61+]'
		END AS [AgeGroup]	
	FROM WizzardDeposits AS w
	) AS ageGroups
GROUP BY ageGroups.AgeGroup

--10 First Letter
SELECT firstNames.FirstLetter
FROM (
	SELECT LEFT(w.FirstName,1) AS [FirstLetter]
	FROM WizzardDeposits AS w
	WHERE w.DepositGroup = 'Troll Chest'
	) AS firstNames
GROUP BY firstNames.FirstLetter

--11 Average Interest
SELECT w.DepositGroup, w.IsDepositExpired, AVG(w.DepositInterest) AS [AverageInterest]
FROM WizzardDeposits AS w
WHERE w.DepositStartDate > '01/01/1985'
GROUP BY w.DepositGroup, w.IsDepositExpired
ORDER BY w.DepositGroup DESC, w.IsDepositExpired

--12 Rich Wizard, Poor Wizard
--SELECT SUM(Difftable.Difference) AS [SumDifference]
-- FROM (
--SELECT Host.DepositAmount - (SELECT DepositAmount FROM WizzardDeposits WHERE Id = Host.Id + 1) AS [Difference]
--FROM WizzardDeposits AS Host
--) AS Difftable

SELECT SUM(w1.Diff) AS [SumDifference]  FROM (
SELECT w.DepositAmount - LEAD(DepositAmount, 1, NULL) OVER (ORDER BY Id) AS [Diff]
FROM WizzardDeposits AS w
) AS w1

-- USE SoftUni
--13 Departments Total Salaries
SELECT e.DepartmentID, SUM(e.Salary) AS TotalSalary
FROM Employees AS e
GROUP BY e.DepartmentID

--14 Employees Minimum Salaries
SELECT e.DepartmentID, MIN(e.Salary) AS MinimumSalary
FROM Employees AS e
GROUP BY e.DepartmentID
HAVING e.DepartmentID IN (2,5,7)

--15 Employees Average Salaries
SELECT *
INTO EmployeesAverageSalary
FROM Employees AS e
WHERE e.Salary > 30000 

DELETE FROM EmployeesAverageSalary
WHERE ManagerID = 42

UPDATE EmployeesAverageSalary
SET Salary += 5000
WHERE DepartmentID = 1


SELECT n.DepartmentID, AVG(n.Salary) AS AverageSalary
FROM EmployeesAverageSalary AS n
GROUP BY n.DepartmentID

--16 Employess Maximum Salaries
SELECT e.DepartmentID, MAX(e.Salary) AS [MaxSalary]
FROM Employees AS e
GROUP BY e.DepartmentID
HAVING MAX(e.Salary) NOT BETWEEN 30000 AND 70000

--17 Employees Count Salaries
SELECT COUNT(*) AS [Count]
FROM Employees AS e
WHERE e.ManagerID IS NULL

--18 3rd Highest Salary
SELECT s.DepartmentID, s.Salary AS [ThirdHighestSalary]
FROM (
SELECT e.DepartmentID, e.Salary, DENSE_RANK() OVER(PARTITION BY e.DepartmentId ORDER BY e.Salary DESC) AS [SalaryRank]
FROM Employees AS e
GROUP BY e.DepartmentID, e.Salary
) AS s
WHERE [SalaryRank] = 3


--19 Salaray Challenge
SELECT TOP(10) e1.FirstName, e1.LastName, e1.DepartmentID
FROM Employees As e1
WHERE e1.Salary > 
	(SELECT AVG(e2.Salary)
	FROM Employees AS e2
	WHERE e1.DepartmentID = e2.DepartmentID
	GROUP BY e2.DepartmentID
	)
