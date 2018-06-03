SELECT Peaks.PeakName, Rivers.RiverName, 
		LOWER(CONCAT(Peaks.PeakName, SUBSTRING(Rivers.RiverName, 2, LEN(Rivers.RiverName)))) AS [Mix]
FROM Peaks, Rivers
WHERE LEFT(Rivers.RiverName, 1) = RIGHT(Peaks.PeakName, 1)
ORDER BY [Mix]