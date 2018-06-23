--17 Employee's Load
CREATE FUNCTION udf_GetReportsCount(@employeeId INT, @statusId INT)
RETURNS INT 
AS
BEGIN
	DECLARE @count INT = (SELECT COUNT(*)
						  FROM Reports AS r
						  WHERE r.EmployeeId = @employeeId AND r.StatusId = @statusId)
	RETURN @count
END

--18 Assign Employee
CREATE PROCEDURE usp_AssignEmployeeToReport(@employeeId INT, @reportId INT)
AS
BEGIN
	BEGIN TRANSACTION
	UPDATE Reports
	SET EmployeeId = @employeeId
	WHERE Id = @reportId

	DECLARE @employeeDepartment INT = (SELECT e.DepartmentId
									  FROM Employees AS e
									  WHERE e.Id = @employeeId)

	DECLARE @reportDepartment INT = (SELECT c.DepartmentId
									  FROM Reports AS r
									  JOIN Categories AS c ON c.Id = r.CategoryId
									  WHERE r.Id = @reportId)

	IF(@employeeDepartment <> @reportDepartment)
	BEGIN
		ROLLBACK;
		RAISERROR('Employee doesn''t belong to the appropriate department!',16,1);
	END

	COMMIT
END

--19 Close Reports
CREATE TRIGGER tr_CloseReport ON Reports FOR UPDATE
AS
BEGIN
	UPDATE Reports
	SET StatusId = (SELECT Id FROM [Status] WHERE Label = 'completed')
	WHERE Id IN (SELECT Id 
				 FROM inserted
			     WHERE Id IN (SELECT Id 
							  FROM deleted
						      WHERE CloseDate IS NULL)
			              AND CloseDate IS NOT NULL)
END

--20 Categories Revision
--BONUS
SELECT c.Name, COUNT(*) AS [Reports Number],
	   CASE
			WHEN SUM(CASE WHEN r.StatusId = 2 THEN 1 ELSE 0 END) >
			SUM(CASE WHEN r.StatusId = 1 THEN 1 ELSE 0 END) THEN 'in progress'
			WHEN SUM(CASE WHEN r.StatusId = 2 THEN 1 ELSE 0 END) <
			SUM(CASE WHEN r.StatusId = 1 THEN 1 ELSE 0 END) THEN 'waiting'
			ELSE 'equal'
	   END AS [Main Status]
FROM Categories AS c
JOIN Reports AS r ON r.CategoryId = c.Id
JOIN [Status] AS s ON s.Id = r.StatusId
WHERE s.Label IN ('waiting', 'in progress')
GROUP BY c.Name
ORDER BY c.Name, [Reports Number], [Main Status]
