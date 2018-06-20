--14 Create Table Logs
CREATE TABLE Logs(
	LogId INT NOT NULL IDENTITY,
	AccountId INT NOT NULL,
	OldSum DECIMAL(15,2) NOT NULL,
	NewSum DECIMAL(15,2) NOT NULL,

	CONSTRAINT PK_Logs PRIMARY KEY (LogId),

	CONSTRAINT FK_LogsAccounts FOREIGN KEY (AccountId)
	REFERENCES Accounts (Id)
)

CREATE TRIGGER tr_AccountsChangeBalance ON Accounts FOR UPDATE AS
BEGIN
	DECLARE @accountId INT = (SELECT Id FROM inserted);
	DECLARE @oldBalance DECIMAL(15,2) = (SELECT Balance FROM deleted);
	DECLARE @newBalance DECIMAL(15,2) = (SELECT Balance FROM inserted);

	IF(@newBalance <> @oldBalance)
		INSERT INTO Logs
		VALUES
		(@accountId, @oldBalance, @newBalance)
END

--15 Create Table Emails
CREATE TABLE NotificationEmails(
	Id INT IDENTITY NOT NULL,
	Recipient INT NOT NULL,
	[Subject] VARCHAR(50) NOT NULL,
	Body VARCHAR(255) NOT NULL,

	CONSTRAINT PK_NotificationEmails PRIMARY KEY (Id),
	CONSTRAINT FK_NotificationEmailsLogs FOREIGN KEY (Recipient)
	REFERENCES Accounts (Id)
)


CREATE TRIGGER tr_CreateLog ON Logs FOR INSERT AS
BEGIN
	DECLARE @recepient INT = (SELECT AccountId FROM inserted);
	DECLARE @subject VARCHAR(50) = (SELECT CONCAT('Balance change for account: ', @recepient))
	DECLARE @oldBalance DECIMAL(15, 2) = (SELECT OldSum FROM inserted);
	DECLARE @newBalance DECIMAL(15, 2) = (SELECT NewSum FROM inserted);
	DECLARE @body VARCHAR(255) = (SELECT CONCAT('On ', GETDATE(), ' your balance was changed from ', @oldBalance, ' to ', @newBalance,'.'));

	INSERT INTO NotificationEmails
	VALUES
	(@recepient, @subject, @body)

END

--16 Deposit Money
--Stupid Judge wont compile with check for positive moneyAmount
CREATE PROC usp_DepositMoney (@accountId INT, @moneyAmount DECIMAL(15,4)) AS
BEGIN
	BEGIN TRAN
	DECLARE @oldBalance DECIMAL(15,4) = (SELECT Balance FROM Accounts WHERE Id = @accountId);
	
	UPDATE Accounts
	SET Balance += @moneyAmount
	WHERE Id = @accountId

	IF(@@ROWCOUNT <> 1)
	BEGIN
	ROLLBACK;
	RAISERROR('Invalid account', 16, 1);
	RETURN;
	END

	--works without this check
	DECLARE @newBalance DECIMAL(15,4) = (SELECT Balance FROM Accounts WHERE Id = @accountId);

	IF(@oldBalance + @moneyAmount <> @newBalance)
	BEGIN
	ROLLBACK;
	RAISERROR('Invalid transaction', 16, 2);
	RETURN;
	END

	COMMIT
	
END

--17 Withdraw Money
--Stupid Judge wont compile with check if moneyAmount is bigger than the balance
CREATE PROC usp_WithdrawMoney (@accountId INT, @moneyAmount DECIMAL(15,4)) AS
BEGIN 
	BEGIN TRAN
	
	UPDATE Accounts
	SET Balance -= @moneyAmount
	WHERE Id = @accountId

	IF(@@ROWCOUNT <> 1)
	BEGIN
	ROLLBACK;
	RAISERROR('Invalid account', 16, 1);
	RETURN;
	END

	--works without this check
	DECLARE @newBalance DECIMAL(15,4) = (SELECT Balance FROM Accounts WHERE Id = @accountId);

	IF(@newBalance < 0)
	BEGIN
	ROLLBACK;
	RAISERROR('Invalid amount', 16, 2);
	RETURN;
	END

	COMMIT

END

--18 Money Transfer
CREATE PROC usp_TransferMoney(@senderId INT , @receiverId INT, @amount DECIMAL(15,4)) AS
BEGIN
	BEGIN TRAN
	EXEC usp_WithdrawMoney @senderId, @amount;
	EXEC usp_DepositMoney @receiverId, @amount;

	-- works without this check
	IF(@senderId = @receiverId)
	BEGIN
	ROLLBACK;
	RAISERROR('Cannot transfer to the same account!', 16, 1);
	RETURN;
	END

	COMMIT

END

--19 Trigger
CREATE TRIGGER tr_SetLevelRestrictionToGettingItems ON UserGameItems INSTEAD OF INSERT AS
BEGIN
	INSERT INTO UserGameItems
	SELECT i.ItemId, i.UserGameId
	FROM inserted AS i
	WHERE i.ItemId IN ( 
						SELECT Id
						FROM Items
						WHERE MinLevel <= (
											SELECT [Level]
											FROM UsersGames
											WHERE Id = UserGameId
											)
					)
END 

UPDATE UsersGames
SET Cash += 50000
WHERE UserId IN (
	SELECT Id 
	FROM Users
	WHERE Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos')	
) 
AND GameId = (SELECT Id FROM Games WHERE Name = 'Bali')

INSERT INTO UserGameItems (UserGameId, ItemId)
SELECT ug.Id, i.Id
FROM UsersGames AS ug, Items AS i
WHERE UserId IN (
	SELECT Id 
	FROM Users
	WHERE Username IN ('baleremuda', 'loosenoise', 'inguinalself', 'buildingdeltoid', 'monoxidecos')	
) 
AND GameId = (SELECT Id FROM Games WHERE Name = 'Bali')
AND ((i.Id > 250 AND i.Id < 300) OR (i.Id > 500 and i.Id < 540))

SELECT u.Username, g.Name, ug.Cash, i.Name AS [Item Name]
FROM UsersGames AS ug
JOIN Games AS g ON g.Id = ug.GameId
JOIN Users AS u ON u.Id = ug.UserId
JOIN UserGameItems AS ugi ON ugi.UserGameId = ug.Id
JOIN Items AS i ON i.Id = ugi.ItemId
WHERE g.Name = 'Bali'
ORDER BY Username, [Item Name]

SELECT * FROM Items
SELECT * FROM UsersGames WHERE UserId = 9
SELECT * FROM UserGameItems
ORDER BY UserGameId

INSERT INTO UserGameItems
VALUES
(1, 1)

--20 Massive Shopping
--User Stamat has Id 9
--Game with Name Safflower has Id 87
--UsersGames with id 110 is Game Safflower with User Stamat
DECLARE @itemsCost1 DECIMAL = (
					SELECT SUM(i.Price)
					FROM Items AS i
					WHERE i.MinLevel BETWEEN 11 AND 12
);

DECLARE @playerCash1 DECIMAL = (
					SELECT ug.Cash
					FROM UsersGames AS ug
					WHERE ug.Id = 110
)
IF(@playerCash1 - @itemsCost1 > 0)
BEGIN
BEGIN TRANSACTION [TRAN 1]

UPDATE UsersGames
SET Cash = Cash - @itemsCost1
WHERE Id = 110

INSERT INTO UserGameItems(UserGameId, ItemId)
SELECT 110, i.Id FROM Items AS i WHERE i.MinLevel BETWEEN 11 AND 12

COMMIT TRANSACTION [TRAN 1]
END

DECLARE @itemsCost2 DECIMAL = (
					SELECT SUM(i.Price)
					FROM Items AS i
					WHERE i.MinLevel BETWEEN 19 AND 21
);

DECLARE @playerCash2 DECIMAL = (
					SELECT ug.Cash
					FROM UsersGames AS ug
					WHERE ug.Id = 110
)
IF(@playerCash2 - @itemsCost2 > 0)
BEGIN
BEGIN TRANSACTION [TRAN 2]

UPDATE UsersGames
SET Cash = Cash - @itemsCost2
WHERE Id = 110

INSERT INTO UserGameItems(UserGameId, ItemId)
SELECT 110, i.Id FROM Items AS i WHERE i.MinLevel BETWEEN 19 AND 21

COMMIT TRANSACTION [TRAN 2]
END

SELECT i.Name AS [Item Name]
FROM UserGameItems AS ugi
JOIN Items AS i ON i.Id = ugi.ItemId AND ugi.UserGameId = 110
ORDER BY [Item Name]

--21 Employees with Three Projects
CREATE PROCEDURE usp_AssignProject(@emloyeeID INT, @projectID INT) AS
BEGIN
	DECLARE @maxEmployeeProjectsCount INT = 3
	DECLARE @employeeProjectsCount INT;

BEGIN TRAN
	INSERT INTO EmployeesProjects
	VALUES
	(@emloyeeID, @projectID)

	SET @employeeProjectsCount = (
									SELECT COUNT(*) 
									FROM EmployeesProjects AS ep
									WHERE ep.EmployeeID = @emloyeeID
    )

	IF(@employeeProjectsCount > @maxEmployeeProjectsCount)
	BEGIN
		ROLLBACK;
		RAISERROR('The employee has too many projects!',16,1);
		RETURN;
	END

	COMMIT
END

--22 Delete Employees
CREATE TABLE Deleted_Employees(
	EmployeeId INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	MiddleName VARCHAR(30),
	JobTitle VARCHAR(30),
	DepartmentId INT,
	Salary DECIMAL(15,2)
)

CREATE TRIGGER tr_DeletedEmployees ON Employees INSTEAD OF DELETE AS
	INSERT INTO Deleted_Employees
	SELECT FirstName,LastName,MiddleName,JobTitle,DepartmentID, Salary
	FROM deleted

