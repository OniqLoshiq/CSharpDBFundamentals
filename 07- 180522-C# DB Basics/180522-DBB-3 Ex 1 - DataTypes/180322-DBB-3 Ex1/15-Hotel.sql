CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Title NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(255)
)

INSERT INTO Employees(FirstName, LastName, Title, Notes)
VALUES
('Dokka', 'Mokka', 'Hotel Manager', 'Good experience in the field'),
('Bokka', 'Rokka', 'Receptionist', 'Working student'),
('Oniq', 'Loshiq', 'Head of Security', 'Military training')

CREATE TABLE Customers(
	AccountNumber INT PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	PhoneNumber VARCHAR(15) NOT NULL,
	EmergencyName NVARCHAR(30) NOT NULL,
	EmergencyNumber VARCHAR(15) NOT NULL,
	Notes NVARCHAR(255)
)

INSERT INTO Customers(AccountNumber, FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes)
VALUES
(0000001, 'Customer', 'One', '0888888888', 'His Wife', '08888888', NULL),
(0000002, 'Customer', 'Two', '099999999', 'Her Husband', '089999999', NULL),
(0000003, 'Customer', 'Three', '077777777', 'His Girlfriend', '087777777', NULL)


CREATE TABLE RoomStatus(
	RoomStatus NVARCHAR(30) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(255)
)

INSERT INTO RoomStatus(RoomStatus, Notes)
VALUES
('Occupied', 'The room is used by a customer'),
('Vacant', 'The room is ready and free to use'),
('Reserved', 'The room is booked')

CREATE TABLE RoomTypes(
	RoomType NVARCHAR(30) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(255)
)

INSERT INTO RoomTypes(RoomType, Notes)
VALUES
('President', 'Most expensive and luxury'),
('Lux', 'Middle top to low high class'),
('Normal', 'Middle class')

CREATE TABLE BedTypes(
	BedType NVARCHAR(30) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(255)
)

INSERT INTO BedTypes(BedType, Notes)
VALUES
('Kingsize', NULL),
('Double', NULL),
('Single', NULL)

CREATE TABLE Rooms(
	RoomNumber INT PRIMARY KEY NOT NULL,
	RoomType NVARCHAR(30) CONSTRAINT FK_Rooms_RoomTypes FOREIGN KEY REFERENCES RoomTypes(RoomType) NOT NULL,
	BedType NVARCHAR(30) CONSTRAINT FK_Rooms_BedTypes FOREIGN KEY REFERENCES BedTypes(BedType) NOT NULL,
	Rate DECIMAL(8,2) NOT NULL,
	RoomStatus NVARCHAR(30) CONSTRAINT FK_Rooms_RoomStatus FOREIGN KEY REFERENCES RoomStatus(RoomStatus) NOT NULL,
	Notes NVARCHAR(255)
)

INSERT INTO Rooms(RoomNumber, RoomType, BedType, Rate, RoomStatus, Notes)
VALUES
(1, 'President', 'Kingsize', 75.10, 'Occupied', NULL),
(11, 'Lux', 'Double', 55, 'Reserved', NULL),
(12, 'Normal', 'Single', 40.60, 'Vacant', NULL)

CREATE TABLE Payments(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT CONSTRAINT FK_Payments_EmployeesId FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	PaymentDate DATETIME DEFAULT GETDATE(),
	AccountNumber INT CONSTRAINT FK_Payments_CustomersAccNum FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	FirstDateOccupied DATE NOT NULL,
	LastDateOccupied DATE NOT NULL,
	TotalDays AS DATEDIFF(day,FirstDateOccupied, LastDateOccupied),
	AmountCharged DECIMAL(15,2) NOT NULL,
	TaxRate DECIMAL(5,2) NOT NULL,
	TaxAmount AS CONVERT(DECIMAL(15,2), AmountCharged * (TaxRate / 100)),
	PaymentTotal AS CONVERT(DECIMAL(15,2), AmountCharged * (1 + TaxRate / 100)),
	Notes NVARCHAR(255)
)

INSERT INTO Payments(EmployeeId, AccountNumber, FirstDateOccupied, LastDateOccupied, AmountCharged, TaxRate)
VALUES
(2, 0000001, '2018-05-22', '2018-05-27', 50, 10),
(2, 0000002, '2018-05-21', '2018-05-27', 60, 15),
(2, 0000003, '2018-05-20', '2018-05-27', 40, 20)

CREATE TABLE Occupancies(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT CONSTRAINT FK_Occupancies_EmployeesId FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	DateOccupied DATE,
	AccountNumber INT CONSTRAINT FK_Occupancies_CustomersAccNum FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	RoomNumber INT CONSTRAINT FK_Occupancies_RoomsNum FOREIGN KEY REFERENCES Rooms(RoomNumber) NOT NULL,
	RateApplied DECIMAL(15,2),
	PhoneCharge INT,
	Notes NVARCHAR(255)
)

INSERT INTO Occupancies(EmployeeId, AccountNumber, RoomNumber)
VALUES	   
(1, 0000001, 1),
(2, 0000002, 11),
(3, 0000003, 12)
