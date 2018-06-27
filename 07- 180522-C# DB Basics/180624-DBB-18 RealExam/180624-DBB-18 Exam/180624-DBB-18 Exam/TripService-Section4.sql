--18 Available Room
CREATE FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @roomId INT = ISNULL((SELECT nt.Id
								  FROM (
										SELECT TOP(1) r.Id, (SUM(h.Baserate + r.Price)) AS RoomPrice
										FROM Rooms AS r
										JOIN Hotels AS h ON h.Id = r.HotelId
										JOIN Trips AS t ON t.RoomId = r.Id
										WHERE r.HotelId = @HotelId 
											 AND r.Beds >=  @People
											 AND  t.RoomId NOT IN (SELECT t1.Id 
																   FROM Trips AS t1
																   WHERE t1.RoomId = t.RoomId 
																		 AND @Date BETWEEN t1.ArrivalDate AND t1.ReturnDate)
										GROUP BY r.Id
										ORDER BY RoomPrice DESC) AS nt
											 ), 0)
	IF(@roomId = 0)
	BEGIN
		RETURN 'No rooms available'
	END

	DECLARE @roomType VARCHAR(50) = (SELECT r.Type
									 FROM Rooms AS r
									 WHERE r.Id = @roomId)
	
	DECLARE @roomBeds INT = (SELECT r.Beds
							 FROM Rooms AS r
							 WHERE r.Id = @roomId)

	DECLARE @totalPrice DECIMAL(14,2) = (SELECT SUM(h.Baserate + r.Price) * @People
										 FROM Rooms AS r
										 JOIN Hotels AS h ON h.Id = r.HotelId
										 WHERE r.Id = @roomId AND h.Id = @HotelId)

	RETURN CONCAT('Room ', @roomId,': ', @roomType, ' (', @roomBeds, ' beds) - $', @totalPrice)
END

--19 Switch Room
CREATE PROCEDURE usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @previousHotelId INT = (SELECT r.HotelId
								   FROM Trips AS t
								   JOIN Rooms AS r ON r.Id = t.RoomId
								   WHERE t.Id = @TripId)

	DECLARE @newHotelId INT = (SELECT r.HotelId
								   FROM Rooms AS r
								   WHERE r.Id = @TargetRoomId)

	UPDATE Trips
	SET RoomId = @TargetRoomId
	WHERE Id = @TripId
	
	IF(@previousHotelId <> @newHotelId)
	BEGIN
		ROLLBACK;
		RAISERROR('Target room is in another hotel!',16,1)
	END

	DECLARE @roomBeds INT = (SELECT r.Beds
								   FROM Rooms AS r
								   WHERE r.Id = @TargetRoomId)

	DECLARE @countTripAccounts INT = (SELECT COUNT(*)
								   FROM AccountsTrips AS act
								   WHERE act.TripId = @TripId)

	IF(@roomBeds < @countTripAccounts)
	BEGIN
		ROLLBACK;
		RAISERROR('Not enough beds in target room!',16,2)
	END

	COMMIT

END

--20 Cancel Trip
CREATE TRIGGER tr_DeleteTrips ON Trips INSTEAD OF DELETE 
AS
BEGIN
	UPDATE Trips
	SET CancelDate = GETDATE()
	WHERE Id IN (SELECT Id FROM deleted WHERE CancelDate IS NULL)
END