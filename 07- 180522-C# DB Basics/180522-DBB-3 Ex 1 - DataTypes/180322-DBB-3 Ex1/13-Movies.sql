CREATE TABLE Directors(
	Id INT PRIMARY KEY IDENTITY,
	DirectorName NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(255),
)

CREATE TABLE Genres(
	Id INT PRIMARY KEY IDENTITY,
	GenreName NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(255)
)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(255)
)

CREATE TABLE Movies(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(30) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL,
	CopyrightYear INT CHECK (LEN(CopyrightYear) = 4) NOT NULL,
	[Length] DECIMAL(10,2) NOT NULL,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Rating INT,
	Notes NVARCHAR(255)
)

INSERT INTO Directors (DirectorName, Notes)
VALUES
('Dokka', 'random notes'),
('Mokka Losh', NULL),
('Zokka Shin', 'random notes'),
('Bokka O', NULL),
('Rokka Mokka', 'random notes')

INSERT INTO Genres(GenreName, Notes)
VALUES
('Action', 'short description: asdasd'),
('Comedy', 'short description: aasdasdsdasd'),
('Triller', 'short description: asxazxzxdasd'),
('Fantasy', 'short description: asdertertasd'),
('Crime', 'short description: asertwadasd')

INSERT INTO Categories(CategoryName, Notes)
VALUES
('Adults +18', 'some notes'),
('Parent control +16', NULL),
('Parent control +14', NULL),
('Parent control +12', NULL),
('No restrictions', 'some random alabala notes')

INSERT INTO Movies(Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes)
VALUES
('Die Hard', 1, '1988', 132, 1, 3, 8, NULL),
('Die Hard 2', 2, '1990', 125, 3, 2, 7, NULL),
('Die Hard 3', 4, '1995', 131, 5, 1, 8, NULL),
('Die Hard 4', 3, '2007', 129, 4, 4, 7, NULL),
('Die Hard 5', 5, '2013', 101, 2, 5, 5, NULL)

