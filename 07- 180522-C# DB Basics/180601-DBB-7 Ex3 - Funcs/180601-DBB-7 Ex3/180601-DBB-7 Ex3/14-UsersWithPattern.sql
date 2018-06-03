SELECT Username, IpAddress AS [IPAdress]
FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username