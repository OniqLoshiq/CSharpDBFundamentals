CREATE DATABASE Minions

CREATE TABLE Minions(
	Id INT PRIMARY KEY NOT NULL,
	[Name] VARCHAR(30) NOT NULL,
	Age INT
)

CREATE TABLE Towns(
	Id INT PRIMARY KEY NOT NULL,
	[Name] VARCHAR(30) NOT NULL
)

ALTER TABLE Minions
ADD TownId INT FOREIGN KEY REFERENCES Towns(Id)

INSERT INTO Towns(Id, Name)
VALUES
(1, 'Sofia')

INSERT INTO Towns(Id, Name)
VALUES
(2, 'Plovdiv')

INSERT INTO Towns(Id, Name)
VALUES
(3, 'Varna')

INSERT INTO Minions(Id, Name, Age, TownId)
VALUES
(1, 'Kevin', 22, 1)

INSERT INTO Minions(Id, Name, Age, TownId)
VALUES
(2, 'Bob', 22, 3)

INSERT INTO Minions(Id, Name, Age, TownId)
VALUES
(3, 'Steward', NULL, 2)

TRUNCATE TABLE Minions

DROP TABLE Minions
DROP Table Towns