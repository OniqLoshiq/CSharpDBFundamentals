--17 Find My Ride
CREATE FUNCTION udf_CheckForVehicle(@townName NVARCHAR(50), @seatsNumber INT)
RETURNS VARCHAR(50) AS 
BEGIN
	DECLARE @result NVARCHAR(100);

	SET @result = (SELECT TOP (1) CONCAT(o.Name,' - ', m.Model)
			   FROM Towns AS t
			   JOIN Offices AS o ON o.TownId = t.Id
			   JOIN Vehicles AS v ON v.OfficeId = o.Id
			   JOIN Models AS m ON m.Id = v.ModelId
			   WHERE t.Name = @townName AND m.Seats = @seatsNumber
			   ORDER BY o.Name
	)
	IF(@result IS NULL)
	BEGIN
		SET @result = 'NO SUCH VEHICLE FOUND';
	END

	RETURN @result

END

--18 Move a Vehicle
CREATE PROCEDURE usp_MoveVehicle(@vehicleId INT, @officeId INT) AS 
BEGIN
	BEGIN TRANSACTION
		UPDATE Vehicles
		SET OfficeId = @officeId
		WHERE Id = @vehicleId

		DECLARE @takenParkingPlaces INT = (SELECT COUNT(*)
										   FROM Vehicles AS v
										   JOIN Offices As o ON o.Id = v.OfficeId
										   WHERE o.Id = @officeId
										   )
		DECLARE @maxParkingPlaces INT = (SELECT o.ParkingPlaces
										   FROM Offices As o
										   WHERE o.Id = @officeId
										   )

		IF(@takenParkingPlaces > @maxParkingPlaces)
		BEGIN
			ROLLBACK;
			RAISERROR('Not enough room in this office!', 16,1);
		END

		COMMIT
END

--19 Move the Tally
CREATE TRIGGER tr_UpdateMileage ON Orders FOR UPDATE
AS
BEGIN
	UPDATE Vehicles 
	SET Mileage += (SELECT TotalMileage
					FROM inserted
					WHERE TotalMileage IS NOT NULL)
END