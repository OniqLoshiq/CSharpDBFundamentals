CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(30) NOT NULL,
	DailyRate DECIMAL(10,2),
	WeeklyRate DECIMAL(10,2),
	MontlyRate DECIMAL(10,2),
	WeekendRate DECIMAL(10,2)
)

CREATE TABLE Cars(
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber VARCHAR(10) UNIQUE NOT NULL,
	Manufacturer NVARCHAR(30) NOT NULL,
	Model NVARCHAR(30) NOT NULL,
	CarYear INT NOT NULL CHECK(LEN(CarYear) = 4),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors INT NOT NULL,
	Picture VARBINARY(MAX),
	Condition NVARCHAR(30),
	Available BIT NOT NULL
)

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Title VARCHAR(30) NOT NULL,
	Notes NVARCHAR(255)
)

CREATE TABLE Customers(
	Id INT PRIMARY KEY IDENTITY,
	DriverLicenceNumber INT NOT NULL UNIQUE,
	FullName NVARCHAR(30) NOT NULL,
	[Address] NVARCHAR(50) NOT NULL,
	City NVARCHAR(30) NOT NULL,
	ZIPCode INT NOT NULL,
	Notes NVARCHAR(255)
)

CREATE TABLE RentalOrders(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	CarId INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
	TankLevel DECIMAL(10,2) NOT NULL,
	KilometrageStart DECIMAL(15,2) NOT NULL,
	KilometrageEnd DECIMAL(15,2),
	TotalKilometrage DECIMAL(15,2),
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	TotalDays INT NOT NULL,
	RateApplied DECIMAL(10,2),
	TaxRate DECIMAL(10,2),
	OrederStatus BIT NOT NULL,
	Notes NVARCHAR(255)
)

INSERT INTO Categories(CategoryName, DailyRate, WeeklyRate, MontlyRate, WeekendRate)
VALUES
('Category1', NULL, NULL, NULL, NULL),
('Category2', NULL, NULL, NULL, NULL),
('Category3', NULL, NULL, NULL, NULL)

INSERT INTO Cars(PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available)
VALUES
('CA 3333 PB', 'Volkswagen', 'VW Passat', '2017', 1, 5, NULL, NULL, 1),
('PB 3333 PB', 'Volkswagen', 'VW Golf IV', '2015', 2, 3, NULL, 'brand new', 0),
('B 3333 PB', 'Volkswagen', 'VW Touareg', '2014', 1, 5, NULL, NULL, 1)

INSERT INTO Employees(FirstName, LastName, Title, Notes)
VALUES
('Dokka', 'Bokka', 'Manager', NULL),
('Zmokka', 'Rokka', 'Sales manager', 'skatavka'),
('Oniq', 'Loshiq', 'Consultant', NULL)

INSERT INTO Customers(DriverLicenceNumber, FullName, Address, City, ZIPCode, Notes)
VALUES
(12315523, 'Customer One', 'Some adress', 'Sofia', '1618', NULL),
(33123123, 'Customer Two', 'Some adress 1', 'Plovdiv', '4000', NULL),
(99999999, 'Customer Three', 'Some adress 2', 'Varna', '1628', NULL)

INSERT INTO RentalOrders(EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, 
KilometrageEnd, TotalKilometrage, StartDate, EndDate, TotalDays, RateApplied, TaxRate, OrederStatus, Notes)
VALUES
(1, 1, 1, 45.15, 16000, NULL, NULL, '2018-05-22', '2018-02-27', 5, NULL, NULL, 0, NULL),
(3, 3,3, 25.15, 4000, NULL, NULL, '2018-05-22', '2018-02-27', 5, NULL, NULL, 0, NULL),
(2, 2, 2, 65.15, 55000, NULL, NULL, '2018-05-22', '2018-02-27', 5, NULL, NULL, 0, NULL)