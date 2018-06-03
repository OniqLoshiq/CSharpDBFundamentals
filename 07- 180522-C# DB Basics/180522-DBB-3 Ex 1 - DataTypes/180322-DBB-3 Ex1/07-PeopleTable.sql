CREATE TABLE People(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARBINARY CHECK(DATALENGTH(Picture) <= 2097152),
	Height DECIMAL(10, 2),
	[Weight] DECIMAL(10, 2),
	Gender CHAR(1) CHECK (Gender = 'm' OR Gender = 'f') NOT NULL,
	Birthdate DATE NOT NULL,
	Biography TEXT
)

INSERT INTO People(Name, Picture, Height, Weight, Gender, Birthdate, Biography)
VALUES
('Bot1', NULL, 1.75, 75, 'm', '1998-07-18', 'some text' ),
('Bot2', NULL, 1.75, 75, 'f', '1997-09-1', 'some random text' ),
('Bot3', NULL, 1.75, 75, 'f', '1996-04-13', 'some text random' ),
('Bot4', NULL, 1.75, 75, 'm', '1993-03-14', 'random some text' ),
('Bot5', NULL, 1.75, 75, 'm', '1992-08-6', 'random text' )