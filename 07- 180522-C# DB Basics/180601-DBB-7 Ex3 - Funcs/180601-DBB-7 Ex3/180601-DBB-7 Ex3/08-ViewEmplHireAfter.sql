CREATE VIEW v_EmployeesHiredAfter2000  AS
SELECT FirstName,LastName
FROM Employees
WHERE DATEPART(YEAR, HireDate) > 2000