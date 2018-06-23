--05 Showroom
SELECT m.Manufacturer, m.Model
FROM Models AS m
ORDER BY m.Manufacturer, m.Id DESC

--06 Y Generation
SELECT c.FirstName, c.LastName
FROM Clients AS c
WHERE DATEPART(YEAR,c.BirthDate) BETWEEN 1977 AND 1994
ORDER BY c.FirstName, c.LastName, c.Id

--07 Spacious Office
SELECT t.Name AS [TownName], o.Name AS [OfficeName], o.ParkingPlaces
FROM Offices AS o
JOIN Towns AS t ON t.Id = o.TownId
WHERE o.ParkingPlaces > 25
ORDER BY TownName, o.Id

--08 Available Vehicles
SELECT m.Model, m.Seats, v.Mileage
FROM Models AS m
JOIN Vehicles AS v ON v.ModelId = m.Id
WHERE v.Id NOT IN (
	 SELECT o.VehicleId
	 FROM Orders AS o
	 WHere o.ReturnDate IS NULL
)
ORDER BY v.Mileage, m.Seats DESC, m.Id

--09 Offices per Town
SELECT t.Name AS [TownName], COUNT(*) AS [OfficesNumber]
FROM Towns AS t
JOIN Offices AS o ON o.TownId = t.Id
GROUP BY t.Name
ORDER BY OfficesNumber DESC, TownName

--10 Buyers best choice
SELECT m.Manufacturer, m.Model, COUNT(v.Id) AS [TimesOrdered]
FROM Orders AS o
JOIN Vehicles AS v ON v.Id = o.VehicleId
RIGHT JOIN Models AS m ON m.Id = v.ModelId
GROUP BY m.Model, m.Manufacturer
ORDER BY TimesOrdered DESC, Manufacturer DESC, Model

--11 Kinda Person
SELECT Names, Class
FROM
	(SELECT	CONCAT(c.FirstName,' ', c.LastName) AS Names,
			m.Class,
			RANK() OVER(PARTITION BY CONCAT(c.FirstName,' ', c.LastName) ORDER BY COUNT(m.Class) DESC) AS Ranking
	FROM Clients AS c
	JOIN Orders As o ON o.ClientId = c.Id
	JOIN Vehicles AS v ON v.Id = o.VehicleId
	JOIN Models AS m ON m.Id = v.ModelId
	GROUP BY CONCAT(c.FirstName,' ', c.LastName), m.Class) AS s
WHERE Ranking = 1
ORDER BY Names, Class

--12 Age Groups Revenue
SELECT nt.AgeGroup, SUM(Bill) AS Revenue, AVG(TotalMileage) AS AverageMileage
FROM(
 SELECT c.Id,
 	   CASE
 			WHEN DATEPART(YEAR,c.BirthDate) BETWEEN 1970 AND 1979 THEN '70''s'
 			WHEN DATEPART(YEAR,c.BirthDate) BETWEEN 1980 AND 1989 THEN '80''s'
 			WHEN DATEPART(YEAR,c.BirthDate) BETWEEN 1990 AND 1999 THEN '90''s'
 			ELSE 'Others'
 	   END AS AgeGroup,
	   o.Bill,
	   o.TotalMileage
 FROM Clients AS c
 JOIN Orders AS o ON o.ClientId = c.Id) AS nt
 GROUP BY nt.AgeGroup
 ORDER BY AgeGroup

 --13 Consumption in Mind
 SELECT m.Manufacturer, nt.AverageConsumption
 FROM Models AS m
 JOIN (
	   SELECT TOP(7) m.Model, AVG(m.Consumption) AS AverageConsumption, COUNT(m.Model) AS TimesOrdered
	   FROM Orders AS o
	   JOIN Vehicles AS v ON v.Id = o.VehicleId
	   JOIN Models AS m ON m.Id = v.ModelId
	   GROUP BY m.Model
	   ORDER BY COUNT(*) DESC) AS nt ON nt.Model = m.Model
WHERE nt.AverageConsumption BETWEEN 5 AND 15
ORDER BY m.Manufacturer, AverageConsumption

--14 Debt Hunter
SELECT nt.FullName, nt.Email, nt.Bill, nt.Town 
FROM (
	SELECT CONCAT(c.FirstName, ' ', c.LastName) AS FullName, c.Email, o.Bill, t.Name AS Town, RANK() OVER(PARTITION BY t.Name ORDER BY o.Bill DESC) AS Ranking
	FROM Orders AS o
	JOIN Clients AS c ON c.Id = o.ClientId
	JOIN Towns AS t ON t.Id = o.TownId
	WHERE c.CardValidity < o.CollectionDate
	GROUP BY t.Name, c.Email, CONCAT(c.FirstName, ' ', c.LastName), o.Bill) AS nt
WHERE Ranking IN(1,2) AND Bill IS NOT NULL
ORDER BY nt.Town, nt.Bill, nt.FullName

--15 Town Statistics
WITH CTE_TownsClients AS (
SELECT t.Name, c.Gender, t.Id
FROM Towns AS t
JOIN Orders AS o ON o.TownId = t.Id
JOIN Clients AS c ON c.Id = o.ClientId
)

SELECT tc.Name, 
	   CONVERT(INT,(CAST(tc2.Males AS FLOAT) / COUNT(*)) * 100) AS MalePercent, 
	   CONVERT(INT,(CAST(tc1.Females AS FLOAT) / COUNT(*)) * 100) AS FemalePercent
FROM CTE_TownsClients AS tc 
LEFT JOIN ( SELECT Name, COUNT(*) AS Females
		    FROM CTE_TownsClients
			WHERE Gender = 'F'
			GROUP BY Name
) AS tc1 ON tc1.Name = tc.Name
LEFT JOIN ( SELECT Name, COUNT(*) AS Males
		    FROM CTE_TownsClients
			WHERE Gender = 'M'
			GROUP BY Name
) AS tc2 ON tc2.Name = tc.Name
GROUP BY tc.Name, Males, Females, tc.Id
ORDER BY tc.Name, tc.Id

--16 Home Sweet Home
WITH CTE_Ranks AS (
SELECT ReturnOfficeId, OfficeId, Id, Manufacturer, Model
FROM(
	SELECT DENSE_RANK() OVER(PARTITION BY v.Id ORDER BY o.CollectionDate DESC) AS LatestRentCars,
		   o.ReturnOfficeId,
		   v.OfficeId,
		   v.Id,
		   m.Manufacturer,
		   m.Model
	FROM Orders AS o
	RIGHT JOIN Vehicles AS v ON v.Id = o.VehicleId
	JOIN Models AS m ON m.Id = v.ModelId) AS RankedByDateDesc
WHERE LatestRentCars = 1 )

SELECT CONCAT(Manufacturer,' - ', Model) AS Vehicle,
	   Location =
		   CASE
			WHEN (
					SELECT COUNT(*)
					FROM Orders AS o
					WHERE o.VehicleId = CTE_Ranks.Id
					) = 0
			THEN 'home'
			WHEN (ReturnOfficeId IS NULL ) THEN 'on a rent'
			WHEN OfficeId <> ReturnOfficeId THEN (
												   SELECT CONCAT(t.Name,' - ',o.Name)
												   FROM Towns AS t
												   JOIN Offices AS o ON o.TownId = t.Id
												   WHERE o.Id = ReturnOfficeId
												   )
				

		   END
FROM CTE_Ranks
ORDER BY Vehicle, Id