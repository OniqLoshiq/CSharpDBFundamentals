--05 Bulgarian Cities
SELECT c.Id, c.Name
FROM Cities AS c
WHERE c.CountryCode = 'BG'
ORDER BY c.Name

--06 People Born After 1991
SELECT CONCAT(a.FirstName + ' ',ISNULL(a.MiddleName +' ', ''), a.LastName) AS [FullName],
	   YEAR(a.BirthDate) AS [BirthYear]
FROM Accounts AS a
WHERE YEAR(a.BirthDate) > 1991
ORDER BY BirthYear DESC, a.FirstName

--07 EEE-Mails
SELECT a.FirstName, a.LastName, FORMAT(a.BirthDate, 'MM-dd-yyyy') AS BirthDate, c.Name AS [HomeTown], a.Email
FROM Accounts AS a
JOIN Cities AS c ON c.Id = a.CityId
WHERE a.Email LIKE 'e%'
ORDER BY HomeTown DESC

--08 City Statistics
SELECT c.Name AS [City], ISNULL(COUNT(h.Id),0) AS [Hotels]
FROM Cities AS c
LEFT JOIN Hotels AS h ON h.CityId = c.Id
GROUP BY c.Name
ORDER BY Hotels DESC, City

--09 Expensive First-Class Rooms
SELECT r.Id, r.Price, h.Name AS [Hotel], c.Name AS [City]
FROM Rooms AS r
JOIN Hotels AS h ON h.Id = r.HotelId
JOIN Cities AS c ON c.Id = h.CityId
WHERE r.Type = 'First Class'
ORDER BY r.Price DESC, r.Id

--10 Longest and Shortest Trips
SELECT a.Id AS [AccountId], 
	   CONCAT(a.FirstName, ' ', a.LastName) AS [FullName],
	   MAX(DATEDIFF(DAY,t.ArrivalDate, t.ReturnDate)) AS [LongestTrip],
	   MIN(DATEDIFF(DAY,t.ArrivalDate, t.ReturnDate)) AS [ShortestTrip]
FROM Accounts AS a
JOIN AccountsTrips AS act ON act.AccountId = a.Id
JOIN Trips AS t ON t.Id = act.TripId
WHERE a.MiddleName IS NULL AND t.CancelDate IS NULL
GROUP BY a.Id, a.FirstName, a.LastName
ORDER BY LongestTrip DESC, AccountId

--11 Metropolis
SELECT TOP(5) c.Id, c.Name AS [City], c.CountryCode AS [Country], COUNT(*) AS [Accounts]
FROM Cities AS c
JOIN Accounts AS a ON a.CityId = c.Id
GROUP BY c.Id, c.Name, c.CountryCode
ORDER BY Accounts DESC

--12 Romantic Getaways
SELECT a.Id, a.Email, c.Name AS [City], COUNT(t.Id) AS [Trips]
FROM Accounts AS a
JOIN Cities AS c ON c.Id = a.CityId
JOIN AccountsTrips AS act ON act.AccountId = a.Id
JOIN Trips AS t ON t.Id = act.TripId
JOIN Rooms AS r ON r.Id = t.RoomId
JOIN Hotels AS h ON h.Id = r.HotelId
WHERE a.CityId = h.CityId
GROUP BY a.Id, a.Email, c.Name
ORDER BY Trips DESC, a.Id
 
--13 Lucrative Destinations
SELECT TOP(10) c.Id, c.Name, SUM(h.BaseRate + r.Price) AS [Total Revenue], COUNT(t.Id) AS [Trips]
FROM Cities AS c
JOIN Hotels AS h ON h.CityId = c.Id
JOIN Rooms AS r ON r.HotelId = h.Id
JOIN Trips As t ON t.RoomId = r.Id
WHERE YEAR(t.BookDate) = 2016
GROUP BY c.Id, c.Name
ORDER BY [Total Revenue] DESC, Trips DESC

--14 Trip Revenues
SELECT t.Id, h.Name, r.Type,
	CASE
		WHEN t.CancelDate IS NOT NULL THEN 0
		ELSE COUNT(at.AccountId) * (r.Price + h.BaseRate)
	END AS Revenue
FROM Trips t
JOIN Rooms r ON r.Id = t.RoomId
JOIN Hotels h ON h.Id = r.HotelId
JOIN AccountsTrips act ON act.TripId = t.Id
GROUP BY t.Id, h.Name, r.Type, t.CancelDate, r.Price, h.BaseRate
ORDER BY r.Type, t.Id



--15 Top Travelers
SELECT nt.AccountId, nt.Email, nt.CountryCode, nt.Trips
FROM(
	SELECT a.Id AS [AccountId], a.Email, c.CountryCode, COUNT(a.Id) AS [Trips], ROW_NUMBER() OVER(PARTITION BY c.CountryCode ORDER BY COUNT(a.Id) DESC) AS Ranking
	FROM Accounts AS a
	JOIN AccountsTrips AS act ON act.AccountId = a.Id
	JOIN Trips AS t ON t.Id = act.TripId
	JOIN Rooms AS r ON r.Id = t.RoomId
	JOIN Hotels AS h ON h.Id = r.HotelId
	JOIN Cities AS c ON c.Id = h.CityId
	GROUP BY c.CountryCode, a.Id, a.Email) AS nt
WHERE nt.Ranking = 1
ORDER BY nt.Trips DESC

--16 Luggage Fees
SELECT nt.Id, nt.Luggage, 
	   CASE 
	   WHEN nt.Luggage > 5 THEN ('$' + CAST((nt.Luggage * 5) AS varchar))
	   WHEN nt.Luggage > 0 THEN '$0'
	   END AS [Fee]
FROM(
	SELECT t.Id, SUM(act.Luggage) AS [Luggage]
	FROM Trips AS t
	JOIN AccountsTrips AS act ON act.TripId = t.Id
	JOIN Rooms AS r ON r.Id = t.RoomId
	JOIN Hotels AS h ON h.Id = r.HotelId
	WHERE act.Luggage > 0
	GROUP BY t.Id) AS nt
ORDER BY nt.Luggage DESC

--17 GDPR Violation
SELECT t.Id,
	   CONCAT(a.FirstName + ' ',ISNULL(a.MiddleName +' ', ''), a.LastName) AS [FullName],
	   c1.Name AS [From],
	   c2.Name AS [To],
	   CASE
		WHEN t.CancelDate IS NOT NULL THEN 'Canceled'
		ELSE CAST(DATEDIFF(DAY,t.ArrivalDate, t.ReturnDate) AS varchar) + ' days'
	   END AS [Duration]
FROM Trips AS t
JOIN AccountsTrips AS act ON act.TripId = t.Id
JOIN Accounts AS a ON a.Id = act.AccountId
JOIN Cities AS c1 ON c1.Id = a.CityId
JOIN Rooms AS r ON r.Id = t.RoomId
JOIN Hotels AS h ON h.Id = r.HotelId
JOIN Cities AS c2 ON c2.Id = h.CityId
ORDER BY FullName, t.Id