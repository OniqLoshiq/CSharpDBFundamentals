--01 Employees with Salary Above 35000
CREATE PROC usp_GetEmployeesSalaryAbove35000
AS 
	SELECT e.FirstName, e.LastName
	FROM Employees AS e
	WHERE e.Salary > 35000

--02 Employees with Salary Above Number
CREATE PROC usp_GetEmployeesSalaryAboveNumber (@salary DECIMAL (18,4))
AS
	SELECT e.FirstName, e.LastName
	FROM Employees AS e
	WHERE e.Salary >= @salary

--03 Town Names Starting With
CREATE PROC usp_GetTownsStartingWith (@startString VARCHAR(10))
AS
	SELECT t.Name AS [Town]
	FROM Towns AS t
	WHERE t.Name LIKE CONCAT(@startString, '%')

--04 Employees from Town
CREATE PROC usp_GetEmployeesFromTown (@townName VARCHAR(20))
AS
	SELECT e.FirstName, e.LastName
	FROM Employees AS e
	JOIN Addresses AS a ON a.AddressID = e.AddressID
	JOIN Towns AS t ON (t.TownID = a.TownID AND t.Name = @townName)

--05 Salary Level Function
CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @salaryType VARCHAR(10);
	
	IF(@salary < 30000) SET @salaryType = 'Low'
	ELSE IF (@salary <= 50000) SET @salaryType = 'Average'
	ELSE SET @salaryType = 'High'

	RETURN @salaryType
END

--06 Employees by Salary Level
CREATE PROC usp_EmployeesBySalaryLevel (@salaryLevel VARCHAR(10))
AS
	SELECT e.FirstName, e.LastName
	FROM Employees AS e
	WHERE dbo.ufn_GetSalaryLevel(e.Salary) = @salaryLevel

--07 Define Function
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(50), @word VARCHAR(50))
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT = 1;
	DECLARE @count INT = 1;

	WHILE @count <= LEN(@word)
	BEGIN
		IF(CHARINDEX(SUBSTRING(@word, @count, 1), @setOfLetters) = 0)
		BEGIN
			SET @result = 0;
			BREAK;
		END
		SET @count += 1;
	END

	RETURN @result
END

--08 Delete Employees and Departments
CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
BEGIN
	ALTER TABLE Departments
	ALTER COLUMN ManagerID INT NULL

	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerID IN (
							SELECT EmployeeID FROM Employees
							WHERE DepartmentID = @departmentId)

	UPDATE Departments
	SET ManagerID = NULL
	WHERE ManagerID IN (
							SELECT EmployeeID FROM Employees
							WHERE DepartmentID = @departmentId)

	DELETE FROM EmployeesProjects
	WHERE EmployeeID IN (
							SELECT EmployeeID FROM Employees
							WHERE DepartmentID = @departmentId)

	DELETE FROM Employees
	WHERE DepartmentID = @departmentId

	DELETE FROM Departments
	WHERE DepartmentID = @departmentId
	
	SELECT COUNT(*) 
	FROM Employees AS e	
	WHERE e.DepartmentID = @departmentId
	
END

--09 Find Full Name
CREATE PROC usp_GetHoldersFullName 
AS
SELECT CONCAT(ah.FirstName, ' ', ah.LastName) AS [Full Name]
FROM AccountHolders AS ah

--10 People with Balance Higher Than
CREATE PROC usp_GetHoldersWithBalanceHigherThan (@minimumMoney DECIMAL(15,2))
AS
SELECT ah.FirstName, ah.LastName
FROM AccountHolders AS ah
JOIN (
	  SELECT a.AccountHolderId, SUM(a.Balance) AS [Total Balance]
	  FROM Accounts AS a
	  GROUP BY a.AccountHolderId
	) AS ab ON ab.AccountHolderId = ah.Id
WHERE ab.[Total Balance] > @minimumMoney
ORDER BY ah.LastName, ah.FirstName

--11 Future Value Function
CREATE FUNCTION ufn_CalculateFutureValue(@sum DECIMAL(18,2), @yIR FLOAT, @nYears INT)
RETURNS DECIMAL(18,4)
BEGIN
	DECLARE @result DECIMAL(18,4);
	SET @result = @sum * (POWER((1 + @yIR), @nYears))

	RETURN @result
END

--12 Calculating Interest
CREATE PROC usp_CalculateFutureValueForAccount(@accountId INT, @yIR FLOAT)
AS
BEGIN
	SELECT ah.Id AS [Account Id],
		   ah.FirstName,
		   ah.LastName,
		   a.Balance AS [Current Balance],
		   dbo.ufn_CalculateFutureValue(a.Balance, @yIR, 5) AS [Balance in 5 years]
	FROM AccountHolders AS ah
	JOIN Accounts AS a ON a.AccountHolderId = ah.Id
	WHERE a.Id = @accountId
END

--13 Scalar Function: Cash in User Games Odd Rows
CREATE FUNCTION ufn_CashInUsersGames(@gameName VARCHAR(50))
RETURNS TABLE
AS
RETURN (
	SELECT SUM(t.Cash) AS [SumCash]
	FROM (
		SELECT ug.Cash AS Cash, ROW_NUMBER() OVER (ORDER BY ug.Cash DESC) AS [RowNumber]
		FROM UsersGames AS ug
		JOIN Games AS g ON (g.Id = ug.GameId AND g.Name = @gameName)
	) AS t
	WHERE t.RowNumber % 2 = 1
)


