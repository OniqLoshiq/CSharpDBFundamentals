--01 One-To-One Relationship
CREATE TABLE Persons(
	PersonID INT NOT NULL,
	FirstName NVARCHAR(30) NOT NULL,
	Salary DECIMAL(15,2) NOT NULL,
	PassportID INT NOT NULL
)

CREATE TABLE Passports(
	PassportID INT NOT NULL,
	PassportNumber VARCHAR(50) NOT NULL
)

ALTER TABLE Passports
ADD CONSTRAINT PK_Passports
PRIMARY KEY (PassportID)

ALTER TABLE Persons
ADD CONSTRAINT PK_Persons
PRIMARY KEY (PersonID)

ALTER TABLE Persons
ADD CONSTRAINT FK_Persons_Passports
FOREIGN KEY (PassportID)
REFERENCES Passports(PassportID)

ALTER TABLE Persons
ADD CONSTRAINT UQ_PassportID
UNIQUE(PassportID)

INSERT INTO Passports
VALUES
(101, 'N34FG21B'),
(102, 'K65LO4R7'),
(103, 'ZE657QP2')

INSERT INTO Persons
VALUES
(1, 'Roberto', 43300, 102),
(2, 'Tom', 56100, 103),
(3, 'Yana', 60200, 101)


--02 One-To-Many Relationship
CREATE TABLE Manufacturers(
	ManufacturerID INT NOT NULL IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	EstablishedOn DATE NOT NULL

	CONSTRAINT PK_Manufacturers
	PRIMARY KEY (ManufacturerID)
)

INSERT INTO Manufacturers
VALUES
('BMW', '07/03/1916'),
('Tesla', '01/01/2003'),
('Lada', '01/05/1966')

CREATE TABLE Models(
	ModelID INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	ManufacturerID INT NOT NULL

	CONSTRAINT PK_Models
	PRIMARY KEY (ModelID)

	CONSTRAINT FK_Models_Manufacturers
	FOREIGN KEY (ManufacturerID)
	REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO Models
VALUES
(101, 'X1', 1),
(102, 'i6', 1),
(103, 'Model S', 2),
(104, 'Model X', 2),
(105, 'Model 3', 2),
(106, 'Nova', 3)


--03 Many-To-Many Relationship
CREATE TABLE Students(
	StudentID INT NOT NULL IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	
	CONSTRAINT PK_Students
	PRIMARY KEY (StudentID)
)

INSERT INTO Students
VALUES
('Mila'),
('Toni'),
('Ron')

CREATE TABLE Exams(
	ExamID INT NOT NULL IDENTITY(101,1),
	[Name] NVARCHAR(50) NOT NULL,
	
	CONSTRAINT PK_Exams
	PRIMARY KEY (ExamID)
)

INSERT INTO Exams
VALUES
('SpringMVC'),
('Neo4j'),
('Oracle 11g')

CREATE TABLE StudentsExams(
	StudentID INT NOT NULL,
	ExamID INT NOT NULL,

	CONSTRAINT PK_StudentsExams
	PRIMARY KEY(StudentID, ExamID),

	CONSTRAINT FK_StudentsExams_Students
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID),

	CONSTRAINT FK_StudentsExams_Exams
	FOREIGN KEY (ExamID)
	REFERENCES Exams(ExamID)
)

INSERT INTO StudentsExams
VALUES
(1, 101),
(1, 102),
(2, 101),
(3, 103),
(2, 102),
(2, 103)


--04 Self-Referencing
CREATE TABLE Teachers(
	TeacherID INT NOT NULL IDENTITY(101, 1),
	[Name] NVARCHAR(50) NOT NULL,
	ManagerID INT,

	CONSTRAINT PK_Teachers
	PRIMARY KEY (TeacherID),

	CONSTRAINT FK_ManagerID_TeacherID
	FOREIGN KEY (ManagerID)
	REFERENCES Teachers(TeacherID)
)

INSERT INTO Teachers
VALUES
('John', NULL),
('Maya', 106),
('Silvia', 106),
('Ted', 105),
('Mark', 101),
('Greta', 101)


--05 Online Store Database
CREATE TABLE Cities(
	CityID INT NOT NULL IDENTITY,
	[Name] VARCHAR(50) NOT NULL,

	CONSTRAINT PK_Cities
	PRIMARY KEY CLUSTERED (CityID)
)

CREATE TABLE Customers(
	CustomerID INT NOT NULL IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	Birthday DATE NOT NULL,
	CityID INT NOT NULL,

	CONSTRAINT PK_Customers
	PRIMARY KEY CLUSTERED (CustomerID),

	CONSTRAINT FK_Customers_Cities
	FOREIGN KEY (CityID)
	REFERENCES Cities(CityID)
)

CREATE TABLE Orders(
	OrderID INT NOT NULL IDENTITY,
	CustomerID INT NOT NULL,

	CONSTRAINT PK_Orders
	PRIMARY KEY CLUSTERED (OrderID),

	CONSTRAINT FK_Orders_Customers
	FOREIGN KEY (CustomerID)
	REFERENCES Customers(CustomerID)
)

CREATE TABLE ItemTypes(
	ItemTypeID INT NOT NULL IDENTITY,
	[Name] VARCHAR(50) NOT NULL,

	CONSTRAINT PK_ItemTypes
	PRIMARY KEY CLUSTERED (ItemTypeID)
)

CREATE TABLE Items(
	ItemID INT NOT NULL IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	ItemTypeID INT NOT NULL,

	CONSTRAINT PK_Items
	PRIMARY KEY CLUSTERED (ItemID),

	CONSTRAINT FK_Items_ItemTypes
	FOREIGN KEY (ItemTypeID)
	REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE OrderItems(
	OrderID INT NOT NULL,
	ItemID INT NOT NULL,

	CONSTRAINT PK_OrderItems
	PRIMARY KEY (OrderID, ItemID),

	CONSTRAINT FK_OrderItems_Orders
	FOREIGN KEY (OrderID)
	REFERENCES Orders (OrderID),

	CONSTRAINT FK_OrderItems_Items
	FOREIGN KEY (ItemID)
	REFERENCES Items (ItemID)
)


--06 University Database
CREATE TABLE Subjects(
	SubjectID INT NOT NULL IDENTITY,
	SubjectName VARCHAR(50) NOT NULL,

	CONSTRAINT PK_Subjects
	PRIMARY KEY CLUSTERED (SubjectID)
)

CREATE TABLE Majors(
	MajorID INT NOT NULL IDENTITY,
	[Name] VARCHAR(50) NOT NULL,

	CONSTRAINT PK_Majors
	PRIMARY KEY CLUSTERED (MajorID)
)

CREATE TABLE Students(
	StudentID INT NOT NULL IDENTITY,
	StudentNumber VARCHAR(50) NOT NULL,
	StudentName VARCHAR(50) NOT NULL,
	MajorID INT NOT NULL,

	CONSTRAINT PK_Students
	PRIMARY KEY CLUSTERED (StudentID),

	CONSTRAINT FK_Students_Majors
	FOREIGN KEY (MajorID)
	REFERENCES Majors(MajorID)
)

CREATE TABLE Agenda(
	StudentID INT NOT NULL,
	SubjectID INT NOT NULL,

	CONSTRAINT PK_Agenda
	PRIMARY KEY(StudentID, SubjectID),

	CONSTRAINT FK_Agenda_Students
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID),

	CONSTRAINT FK_Agenda_Subjects
	FOREIGN KEY (SubjectID)
	REFERENCES Subjects(SubjectID)
)

CREATE TABLE Payments(
	PaymentID INT NOT NULL IDENTITY,
	PaymentDate DATE NOT NULL,
	PaymentAccount INT NOT NULL,
	StudentID INT NOT NULL,

	CONSTRAINT PK_Payments
	PRIMARY KEY CLUSTERED (PaymentID),

	CONSTRAINT FK_Payments_Students
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID)
)


--09 Peaks int Rila
SELECT m.MountainRange, p.PeakName, p.Elevation 
FROM Mountains AS m
JOIN Peaks AS p
ON p.MountainId = m.Id
WHERE m.MountainRange = 'Rila'
ORDER BY p.Elevation DESC
