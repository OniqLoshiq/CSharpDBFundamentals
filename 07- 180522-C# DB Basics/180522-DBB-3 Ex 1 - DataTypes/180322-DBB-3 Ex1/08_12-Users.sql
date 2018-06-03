CREATE TABLE Users(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	--[Password] BINARY(96) NOT NULL,  -- To has the password value
	ProfilePicture VARBINARY CHECK(DATALENGTH(ProfilePicture) <= 921600),
	LastLoginTime DATE,
	IsDeleted VARCHAR(5) CHECK(IsDeleted = 'true' OR IsDeleted = 'false')
	-- IsDeleted BIT
	-- 0 is false
	-- 1 is true
)

INSERT INTO Users(Username, Password, ProfilePicture, LastLoginTime, IsDeleted)
VALUES
('Dokka', '12345', NULL, '2018-05-27', 'false'),
('Mokka', '123445', NULL, '2018-05-27', 'false'),
('Jmokka', '1444345', NULL, '2018-05-27', 'false'),
('Rokka', '123345', NULL, '2018-05-27', 'false'),
('Tokka', '123545', NULL, '2018-05-27', 'false')

--('Dokka', '12345', NULL, '2018-05-27', 0) - inserted value with IsDeleted as BIT
--('Dokka', HASHBYTES('SHA1', '12345'), NULL, '2018-05-27', 0) - inserted value with Hashed Password
--('Dokka', '12345', NULL, CONVERT(datatime, '27-05-2018', 103), 'false') - inserted value with changed datetime format


ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC072B1F1711

ALTER TABLE Users
ADD CONSTRAINT PK_Users PRIMARY KEY (Id, Username)

ALTER TABLE Users
ADD CONSTRAINT MinPasswordLength CHECK (LEN(Password) >= 5)

ALTER TABLE Users
ADD DEFAULT GETDATE() FOR LastLoginTime


ALTER Table Users
DROP CONSTRAINT PK__Users__3214EC07E0464BC1

ALTER TABLE Users
ADD CONSTRAINT PK_Users PRIMARY KEY (Id)

ALTER Table Users
ADD CONSTRAINT CHK_UsernameLength CHECK (LEN(Username) >= 3)