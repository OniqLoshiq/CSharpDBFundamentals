CREATE TABLE Users(
	Id INT IDENTITY NOT NULL,
	Username NVARCHAR(30) NOT NULL UNIQUE,
	[Password] NVARCHAR(50) NOT NULL,
	[Name] NVARCHAR(50),
	Gender CHAR(1),
	BirthDate DATE,
	Age INT,
	Email NVARCHAR(50) NOT NULL,

	CONSTRAINT PK_Users PRIMARY KEY (Id),
	CONSTRAINT ck_Gender CHECK (Gender = 'M' OR Gender = 'F')
)

CREATE TABLE Departments(
	Id INT IDENTITY NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,

	CONSTRAINT PK_Departments PRIMARY KEY (Id)
)

CREATE TABLE Employees(
	Id INT IDENTITY NOT NULL,
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	Gender CHAR(1),
	BirthDate DATE,
	Age INT,
	DepartmentId INT NOT NULL,

	CONSTRAINT PK_Employees PRIMARY KEY (Id),
	CONSTRAINT FK_EmployeesDepartments FOREIGN KEY (DepartmentId)
	REFERENCES Departments(Id),
	CONSTRAINT ck_EmployeesGender CHECK (Gender = 'M' OR Gender = 'F')
)

CREATE TABLE Categories(
	Id INT IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	DepartmentId INT NOT NULL,

	CONSTRAINT PK_Categories PRIMARY KEY (Id),
	CONSTRAINT FK_CategoriesDepartments FOREIGN KEY (DepartmentId)
	REFERENCES Departments(Id)
)

CREATE TABLE [Status](
	Id INT IDENTITY NOT NULL,
	[Label] VARCHAR(30) NOT NULL,

	CONSTRAINT PK_Status PRIMARY KEY (Id)
)

CREATE TABLE Reports(
	Id INT IDENTITY NOT NULL,
	CategoryId INT NOT NULL,
	StatusId INT NOT NULL,
	OpenDate DATETIME NOT NULL,
	CloseDate DATETIME,
	[Description] VARCHAR(200),
	UserId INT NOT NULL,
	EmployeeId INT,

	CONSTRAINT PK_Reports PRIMARY KEY (Id),
	CONSTRAINT FK_ReportsCategories FOREIGN KEY(CategoryId)
	REFERENCES Categories(Id),

	CONSTRAINT FK_ReportsStatus FOREIGN KEY(StatusID)
	REFERENCES Status(Id),

	CONSTRAINT FK_ReportsUsers FOREIGN KEY(UserId)
	REFERENCES Users(Id),

	CONSTRAINT FK_ReportsEmployees FOREIGN KEY(EmployeeId)
	REFERENCES Employees(Id)
)