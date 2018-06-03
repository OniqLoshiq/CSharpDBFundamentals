SELECT FirstName,LastName FROM Employees WHERE NOT (DepartmentID = 4)
--SELECT FirstName, LastName FROM Employees WHERE NOT (DepartmentID = (SELECT DepartmentID FROM Departments WHERE Name = 'Marketing'))
