  
ALTER Procedure  [dbo].[B_GetSearchHotelsResults_TB_Hotel_SP]
(
	@Culture nchar(3),
	@OrderBy nvarchar(100),
	@PagingSize int,
	@PageIndex int,	
	@HotelID bigint = NULL,
	@Name nvarchar(100) = NULL,
	@FirmID int = NULL,	
	@CountryID int = NULL,
	@CityID bigint = NULL,
	@RegionIDs nvarchar(MAX) = NULL,
	@Address nvarchar(100) = NULL,
	@HotelTypeIDs nvarchar(MAX) = NULL,
	@HotelClassIDs nvarchar(MAX) = NULL,
	@HotelChainIDs nvarchar(MAX) = NULL,
	@HotelAccommodationTypeIDs nvarchar(MAX) = NULL,	
	@HotelAttributeIDs nvarchar(MAX) = NULL,
	@StatusID int = NULL,
	@Active bit = NULL,
	@CheckInDate datetime = NULL,
	@CheckOutDate datetime = NULL,
	@RoomCount int = NULL,
	@GuestCount nvarchar(max) = NULL,
	@LowerUSDPrice decimal(8,2) = 0,
	@UpperUSDPrice decimal(8,2) = 0,
	@AgencyRate bit = 0,
	@Keyword nvarchar(200) = NULL,
	@SecretDeal bit = NULL, 
	@Genius bit = NULL,
	@UserID bigint = NULL,
	@UserCountryID int = NULL
	)
AS

DECLARE @SQL NVARCHAR(MAX)
DECLARE @SELECTSQL NVARCHAR(MAX)
DECLARE @JOINSQL NVARCHAR(MAX)
DECLARE @SELECTJOINSQL NVARCHAR(MAX)
DECLARE @FILTERSQL NVARCHAR(MAX)
DECLARE @TOTALCOUNTSQL NVARCHAR(MAX)
DECLARE @PeopleCount INT
DECLARE @AdultCount INT
DECLARE @ChildrenCount INT
DECLARE @SingleRate BIT = 0
DECLARE @DoubleRate BIT = 0
DECLARE @SearchParameterID BIGINT
DECLARE @StartDateID INT
DECLARE @EndDateID INT
DECLARE @CheckOutDateID INT
DECLARE @DayCount INT

declare @sqlquerySet1 nvarchar(max)
declare @sqlquerySet2 nvarchar(max)
declare @sqlquerySet3 nvarchar(max)
declare @sqlquerySet4 nvarchar(max)
declare @sqlquerySet5 nvarchar(max)
declare @sqlquerySet6 nvarchar(max)
declare @sqlquerySet7 nvarchar(max)
declare @sqlquerySet8 nvarchar(max)
declare @sqlquerySet9 nvarchar(max)

declare @Variables1 nvarchar(100)
declare @Variables2 nvarchar(100)
declare @Variables3 nvarchar(100)
declare @Variables4 nvarchar(100)
declare @Variables5 nvarchar(100)
declare @Variables6 nvarchar(100)
declare @Variables7 nvarchar(100)
declare @Variables8 nvarchar(100)
declare @Variables9 nvarchar(100)

SET DATEFIRST 1;

SET @sqlquerySet1 = N'(select @Variables1=Description_'+ @Culture+' from BizTbl_Message where Code=''Hotel'') '
set @sqlquerySet2 = N'(select @Variables2=Description_'+ @Culture+' from BizTbl_Message where Code=''ScoreFrom'') '
set @sqlquerySet3 = N'(select @Variables3=Description_'+ @Culture+' from BizTbl_Message where Code=''Reviews'') '
set @sqlquerySet4 = N'(select @Variables4=Description_'+ @Culture+' from BizTbl_Message where Code=''Averagepricepernight'')'
set @sqlquerySet5 = N'(select @Variables5=Description_'+ @Culture+' from BizTbl_Message where Code=''Description'') '
set @sqlquerySet6 = N'(select @Variables6=Description_'+ @Culture+' from BizTbl_Message where Code=''KmFrom'')'
set @sqlquerySet7 = N'(select @Variables7=Description_'+ @Culture+' from BizTbl_Message where Code=''No'')'
set @sqlquerySet8 = N'(select @Variables8=Description_'+ @Culture+' from BizTbl_Message where Code=''New'') '
set @sqlquerySet9 = N'(select @Variables8=Description_'+ @Culture+' from BizTbl_Message where Code=''ShowMap'') '

EXECUTE sp_executesql @sqlquerySet1,N'@Variables1 nvarchar(100) out', @Variables1 out
EXECUTE sp_executesql @sqlquerySet2,N'@Variables2 nvarchar(100) out', @Variables2 out
EXECUTE sp_executesql @sqlquerySet3,N'@Variables3 nvarchar(100) out', @Variables3 out
EXECUTE sp_executesql @sqlquerySet4,N'@Variables4 nvarchar(100) out', @Variables4 out
EXECUTE sp_executesql @sqlquerySet5,N'@Variables5 nvarchar(100) out', @Variables5 out
EXECUTE sp_executesql @sqlquerySet6,N'@Variables6 nvarchar(100) out', @Variables6 out
EXECUTE sp_executesql @sqlquerySet7,N'@Variables7 nvarchar(100) out', @Variables7 out
EXECUTE sp_executesql @sqlquerySet8,N'@Variables8 nvarchar(100) out', @Variables8 out
EXECUTE sp_executesql @sqlquerySet9,N'@Variables8 nvarchar(100) out', @Variables9 out

SET @SELECTSQL = 
'SELECT
#S# 
H.ID,
H.FirmID,
H.HotelTypeID,
H.HotelClassID,
H.HotelChainID,
H.HotelAccommodationTypeID,
H.CountryID,
H.RegionID,
H.MainRegionID,
H.CityID,
H.ClosestAirportID,
TC.Code AS CountryCode,
TC.Name_' + @Culture + ' AS CountryName, 
TC.Name_en As CountryRoutingName,
ISNULL(R.Name_' + @Culture + ', ISNULL(R.Name, R.NameASCII)) AS RegionName, 
ISNULL(MR.Name_' + @Culture + ', ISNULL(MR.Name, MR.NameASCII)) AS MainRegionName, 
ISNULL(CAC.Name_' + @Culture + ', ISNULL(CAC.Name, CAC.NameASCII)) AS CityName,
CAC.Name As CityRoutingName, 
ISNULL(CA.Name_' + @Culture + ', ISNULL(CA.Name, CA.NameASCII)) AS ClosestAirportName, 
ISNULL(CA.Name_' + @Culture + ', ISNULL(CA.Name, CA.NameASCII)) + '' ('' + CA.Code + ''), '' + ISNULL(CAP.Name_' + @Culture + ', ISNULL(CAP.Name, CAP.NameASCII)) AS ClosestAirportNameWithParentNameAndCode, 
H.ClosestAirportDistance,
R.ParentID AS ParentRegionID,
H.Name,
H.RoutingName As HotelRoutingName,
H.Description_' + @Culture + ' AS Description,
H.Address,
H.Phone,
H.Fax,
H.PostCode,
H.RoomCount,
H.WebAddress,
H.Email,
H.CheckinStart,
H.CheckinEnd,
H.CheckoutStart,
H.CheckoutEnd,
H.FloorCount,
H.BuiltYear,
H.RenovationYear,
H.HitCount,
H.Sort,
H.Latitude,
H.Longitude,
H.MapZoomIndex,
ISNULL(H.CreditCardNotRequired, 1) AS CreditCardNotRequired,
H.StatusID,
H.CurrencyID,
C.Symbol AS CurrencySymbol,
C.Code AS CurrencyCode,
H.ShowOffline,
H.IsPreferred,
H.IsSecret,
H.Active,
H.RoutingName,
H.CreateDateTime,
H.CreateUserID,
H.OpDateTime,
H.OpUserID,
H.IPAddress,
TF.Name AS FirmName,
TH.Name_' + @Culture + ' AS HotelTypeName, 
THC.Name_' + @Culture + ' AS HotelClassName, 
THC.Code AS HotelClassCode, 
THC.Sort AS HotelClassSort,
THCH.Name AS HotelChainName, 
THA.Name_' + @Culture + ' AS HotelAccommodationTypeName, 
TS.Name_' + @Culture + ' AS StatusName,
dbo.TBF_GetHotelReviewCount(H.ID) AS ReviewCount,
dbo.TBF_GetHotelMinimumRoomPriceWithoutDate(H.ID) As MinimumRoomPrice,
dbo.TBF_GetHotelReviewCountByHotel(H.ID) As AveragePoint,
(CASE WHEN H.ReviewPoint IS NOT NULL THEN H.ReviewPoint ELSE dbo.TBF_GetHotelAverageReviewPoint(H.ID) END) AS AverageReviewPoint,
TRE.Name_' + @Culture + ' AS ReviewEvaluationName,
MP.Name AS MainPhotoName,
ISNULL(H.IsValueDeal, 0) AS IsValueDeal,
H.LatestBookingDate,
H.LatestBookingCountryID,
ISNULL(LBC.Name_' + @Culture + ', '''') AS LatestBookingCountryName,
@Variables1 As Hotel,
@Variables2 As ScoreFrom,
@Variables3 As Reviews,
@Variables4 As Averagepricepernight,
@Variables5 As DescriptionText,
@Variables6 As KmFrom,
@Variables7 As No,
@Variables8 As New,
@Variables9 As ShowMap
'

SET @JOINSQL ='#E# FROM dbo.TB_Hotel (NOLOCK) H
INNER JOIN dbo.TB_TypeHotel (NOLOCK) TH ON TH.ID = H.HotelTypeID
INNER JOIN dbo.TB_Country (NOLOCK) TC ON TC.ID = H.CountryID
INNER JOIN dbo.TB_Region (NOLOCK) R ON R.ID = H.RegionID
LEFT JOIN dbo.TB_Region (NOLOCK) MR ON MR.ID = H.MainRegionID
LEFT JOIN dbo.TB_Region (NOLOCK) CAC ON CAC.ID = H.CityID
LEFT JOIN dbo.TB_Region (NOLOCK) CA ON CA.ID = H.ClosestAirportID
LEFT JOIN dbo.TB_Region (NOLOCK) CAP ON CAP.ID = CA.ParentID
LEFT JOIN dbo.TB_TypeHotelClass (NOLOCK) THC ON THC.ID = H.HotelClassID
LEFT JOIN dbo.TB_TypeHotelChain (NOLOCK) THCH ON THCH.ID = H.HotelChainID
LEFT JOIN dbo.TB_TypeHotelAccommodation (NOLOCK) THA ON THA.ID = H.HotelAccommodationTypeID
LEFT JOIN dbo.TB_Firm (NOLOCK) TF ON TF.ID = H.FirmID
LEFT JOIN dbo.TB_TypeStatus (NOLOCK) TS ON TS.ID = H.StatusID 
LEFT JOIN dbo.TB_TypeReviewEvaluation (NOLOCK) TRE ON ((CASE WHEN H.ReviewPoint IS NOT NULL THEN H.ReviewPoint ELSE dbo.TBF_GetHotelAverageReviewPoint(H.ID) END) * 2) BETWEEN TRE.StartPoint AND TRE.EndPoint
LEFT JOIN dbo.TB_Photo (NOLOCK) MP ON MP.RecordID = H.ID AND MP.PartID = 1 AND MP.MainPhoto = 1
LEFT JOIN dbo.TB_Country (NOLOCK) LBC ON LBC.ID = H.LatestBookingCountryID 
LEFT JOIN dbo.TB_Currency (NOLOCK) C ON C.ID = H.CurrencyID
'

SET @FILTERSQL = '
WHERE 
(@HotelID IS NULL OR H.ID = @HotelID) AND
(@Name IS NULL OR H.Name LIKE ''%'' + @Name + ''%'') AND
(@FirmID IS NULL OR H.FirmID = @FirmID) AND
(@CountryID IS NULL OR H.CountryID = @CountryID) AND
(@CityID IS NULL OR H.CityID = @CityID) AND
(@Address IS NULL OR H.Address LIKE ''%'' + @Address + ''%'') AND
(@RegionIDs IS NULL OR (H.RegionID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (H.MainRegionID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (R.ParentID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (R.SecondParentID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (H.ID IN (SELECT HR.HotelID FROM dbo.TB_HotelRegion HR (NOLOCK) WHERE HR.RegionID IN (SELECT Items FROM Split(@RegionIDs, '';''))))) AND
(@HotelTypeIDs IS NULL OR H.HotelTypeID IN (SELECT Items FROM Split(@HotelTypeIDs, '';''))) AND
(@HotelClassIDs IS NULL OR H.HotelClassID IN (SELECT Items FROM Split(@HotelClassIDs, '';''))) AND
(@HotelChainIDs IS NULL OR H.HotelChainID IN (SELECT Items FROM Split(@HotelChainIDs, '';''))) AND
(@HotelAccommodationTypeIDs IS NULL OR H.HotelAccommodationTypeID IN (SELECT Items FROM Split(@HotelAccommodationTypeIDs, '';''))) AND
(@HotelAttributeIDs IS NULL OR (SELECT COUNT(HA.HotelID) FROM dbo.TB_HotelAttribute HA (NOLOCK) WHERE HA.HotelID = H.ID AND HA.Active = 1 AND HA.AttributeID IN (SELECT Items FROM Split(@HotelAttributeIDs, '';'')) GROUP BY HA.HotelID HAVING COUNT(HA.AttributeID) = (SELECT COUNT(Items) FROM Split(@HotelAttributeIDs, '';''))) > 0) AND 
(@StatusID IS NULL OR H.StatusID = @StatusID) AND
(H.Active = 1) 
'

IF @CheckInDate IS NOT NULL
BEGIN

SELECT @SearchParameterID = ISNULL(MAX(ID), 0) + 1 FROM dbo.TB_HotelSearchParameter

INSERT INTO dbo.TB_HotelSearchParameter
(
ID,
Culture,
UserCountryID,
CountryID,
CityID,
RegionIDs,
RegionNames,
CheckInDate,
CheckOutDate,
RoomCount,
GuestCount,
LowerUSDPrice,
UpperUSDPrice,
[Date]
)
SELECT
@SearchParameterID,
@Culture,
@UserCountryID,
@CountryID,
@CityID,
@RegionIDs,
STUFF((SELECT '- ' + R.Name_en FROM dbo.TB_Region (NOLOCK) R WHERE R.ID IN(SELECT Items FROM dbo.Split(@RegionIDs, ';')) FOR XML PATH('')),1,1,''),
@CheckInDate,
@CheckOutDate,
@RoomCount,
@GuestCount,
@LowerUSDPrice,
@UpperUSDPrice,
GetDate()

SELECT @StartDateID = ID FROM dbo.TB_Date (NOLOCK) WHERE Date = @CheckInDate
SELECT @EndDateID = ID FROM dbo.TB_Date (NOLOCK) WHERE Date = DATEADD(day, -1, @CheckOutDate)
SELECT @CheckOutDateID = ID FROM dbo.TB_Date (NOLOCK) WHERE Date = @CheckOutDate

SET @DayCount = DATEDIFF(day, @CheckInDate, @CheckOutDate)
	
INSERT INTO dbo.TB_HotelSearch
(
SearchParameterID,
HotelID,
HotelRoomID,
MaxPeopleCount,
MaxChildrenCount,
ChildrenAllowed,
MinumumRoomRate,
MinumumRoomRateHistory,
TotalRoomRate,
TotalRoomRateHistory,
AvailableRoomCount,
AllocatedRoomCount
)
SELECT 
DISTINCT
@SearchParameterID,
H.ID,
HR.ID,
HR.MaxPeopleCount,
HR.MaxChildrenCount,
(CASE WHEN ((SELECT COUNT(ID) FROM dbo.TB_HotelAttribute (NOLOCK) HA WHERE HA.Active = 1 AND HA.HotelID = HR.HotelID AND HA.AttributeID = 31) = 0 ) THEN 1 ELSE 0 END),
0,
0,
0,
0,
dbo.TBF_GetHotelAvailableRoomCount(HR.ID, @StartDateID, @EndDateID),
0
FROM dbo.TB_Hotel (NOLOCK) H
INNER JOIN dbo.TB_HotelRoom (NOLOCK) HR ON HR.HotelID = H.ID AND HR.Active = 1
INNER JOIN dbo.TB_Region (NOLOCK) R ON R.ID = H.RegionID
WHERE 
(@HotelID IS NULL OR H.ID = @HotelID) AND
(@Name IS NULL OR H.Name LIKE '%' + @Name + '%') AND
(@FirmID IS NULL OR H.FirmID = @FirmID) AND
(@CountryID IS NULL OR H.CountryID = @CountryID) AND
(@CityID IS NULL OR H.CityID = @CityID) AND
(@Address IS NULL OR H.Address LIKE '%' + @Address + '%') AND
(@RegionIDs IS NULL OR (H.RegionID IN (SELECT Items FROM Split(@RegionIDs, ';'))) OR (H.MainRegionID IN (SELECT Items FROM Split(@RegionIDs, ';'))) OR (R.ParentID IN (SELECT Items FROM Split(@RegionIDs, ';'))) OR (R.SecondParentID IN (SELECT Items FROM Split(@RegionIDs, ';'))) OR (H.ID IN (SELECT HR.HotelID FROM dbo.TB_HotelRegion HR (NOLOCK) WHERE HR.RegionID IN (SELECT Items FROM Split(@RegionIDs, ';'))))) AND
(@HotelTypeIDs IS NULL OR H.HotelTypeID IN (SELECT Items FROM Split(@HotelTypeIDs, ';'))) AND
(@HotelClassIDs IS NULL OR H.HotelClassID IN (SELECT Items FROM Split(@HotelClassIDs, ';'))) AND
(@HotelChainIDs IS NULL OR H.HotelChainID IN (SELECT Items FROM Split(@HotelChainIDs, ';'))) AND
(@HotelAccommodationTypeIDs IS NULL OR H.HotelAccommodationTypeID IN (SELECT Items FROM Split(@HotelAccommodationTypeIDs, ';'))) AND
(@HotelAttributeIDs IS NULL OR (SELECT COUNT(HA.HotelID) FROM dbo.TB_HotelAttribute HA (NOLOCK) WHERE HA.HotelID = H.ID AND HA.Active = 1 AND HA.AttributeID IN (SELECT Items FROM Split(@HotelAttributeIDs, ';')) GROUP BY HA.HotelID HAVING COUNT(HA.AttributeID) = (SELECT COUNT(Items) FROM Split(@HotelAttributeIDs, ';'))) > 0) AND 
(@StatusID IS NULL OR H.StatusID = @StatusID) AND
(@Active IS NULL OR H.Active = @Active) AND
dbo.TBF_HasHotelRoomAvailable(HR.ID, @StartDateID, @EndDateID, @CheckOutDateID, @DayCount) = 1 AND
(@DayCount >= ISNULL((SELECT HMA.MinDayCount FROM dbo.TB_HotelMinumumAccommodation HMA (NOLOCK) WHERE HMA.HotelID = H.ID AND @CheckInDate BETWEEN HMA.StartDate AND HMA.EndDate),0))

DECLARE @RoomGuestCount VARCHAR(10)
DECLARE cur INSENSITIVE CURSOR FOR
	SELECT T.Items FROM dbo.Split(@GuestCount,';') T ORDER BY T.Items DESC
OPEN cur

FETCH NEXT FROM cur INTO @RoomGuestCount

	WHILE @@FETCH_STATUS = 0 
	BEGIN

		SET @AdultCount = (SELECT TOP 1 T.Items FROM dbo.Split(@RoomGuestCount,',') T ORDER BY T.Sort)
		SET @ChildrenCount = (SELECT TOP 1 T.Items FROM dbo.Split(@RoomGuestCount,',') T ORDER BY T.Sort DESC)
		SET @PeopleCount = @AdultCount + @ChildrenCount
		
		IF @AdultCount = 1
			SET @SingleRate = 1
		ELSE IF @AdultCount = 2
			SET @DoubleRate = 1
	
		UPDATE dbo.TB_HotelSearch SET
			MinumumRoomRate = ISNULL(dbo.TBF_GetHotelMinumumRoomRate(dbo.TB_HotelSearch.HotelID, @StartDateID, @EndDateID, @DayCount, @SingleRate, @DoubleRate, @AgencyRate, @SecretDeal, @Genius, @UserID, dbo.TB_HotelSearch.HotelRoomID),0),
			MinumumRoomRateHistory = ISNULL(dbo.TBF_GetHotelMinumumRoomRateHistory(dbo.TB_HotelSearch.HotelID, @StartDateID, @EndDateID, @DayCount, @SingleRate, @DoubleRate, @AgencyRate, @SecretDeal, @Genius, @UserID, dbo.TB_HotelSearch.HotelRoomID, 1, NULL, NULL),0)
		WHERE dbo.TB_HotelSearch.SearchParameterID = @SearchParameterID AND dbo.TB_HotelSearch.AvailableRoomCount > dbo.TB_HotelSearch.AllocatedRoomCount
		
		UPDATE dbo.TB_HotelSearch SET
			dbo.TB_HotelSearch.AllocatedRoomCount = dbo.TB_HotelSearch.AllocatedRoomCount + 1,
			dbo.TB_HotelSearch.TotalRoomRate = dbo.TB_HotelSearch.TotalRoomRate + dbo.TB_HotelSearch.MinumumRoomRate,
			dbo.TB_HotelSearch.TotalRoomRateHistory = dbo.TB_HotelSearch.TotalRoomRateHistory + dbo.TB_HotelSearch.MinumumRoomRateHistory			
		FROM (
			SELECT MIN(T2.HotelRoomID) AS HotelRoomID FROM(
			SELECT HS.HotelID, HS.HotelRoomID FROM dbo.TB_HotelSearch (NOLOCK) HS 
			INNER JOIN (SELECT HotelID, MIN(HOS.MinumumRoomRate) AS MinumumRoomRate FROM dbo.TB_HotelSearch (NOLOCK) HOS WHERE HOS.SearchParameterID = @SearchParameterID AND HOS.MinumumRoomRate > 0 AND (@ChildrenCount = 0 OR HOS.ChildrenAllowed = 1) AND (HOS.AvailableRoomCount > HOS.AllocatedRoomCount) AND (HOS.MaxPeopleCount >= @PeopleCount OR (HOS.MaxPeopleCount >= @AdultCount AND HOS.MaxChildrenCount >= @ChildrenCount)) AND ((@LowerUSDPrice + @UpperUSDPrice = 0) OR (HOS.MinumumRoomRate / @DayCount >=  @LowerUSDPrice AND (@UpperUSDPrice = 0 OR HOS.MinumumRoomRate / @DayCount <= @UpperUSDPrice))) GROUP BY HOS.HotelID) T ON T.HotelID = HS.HotelID AND T.MinumumRoomRate = HS.MinumumRoomRate
			WHERE HS.SearchParameterID = @SearchParameterID
			) T2 GROUP BY T2.HotelID		
		) T3	
		WHERE T3.HotelRoomID = dbo.TB_HotelSearch.HotelRoomID AND dbo.TB_HotelSearch.SearchParameterID = @SearchParameterID
	
		FETCH NEXT FROM cur INTO @RoomGuestCount
	END

CLOSE cur DEALLOCATE cur	

SET @SELECTSQL = @SELECTSQL + ', (SELECT SUM(TotalRoomRate) FROM dbo.TB_HotelSearch (NOLOCK) HS WHERE HS.SearchParameterID = @SearchParameterID AND HS.HotelID = H.ID AND HS.AllocatedRoomCount > 0) AS MinimumRoomRate'	
SET @SELECTSQL = @SELECTSQL + ', (SELECT SUM(TotalRoomRateHistory) FROM dbo.TB_HotelSearch (NOLOCK) HS WHERE HS.SearchParameterID = @SearchParameterID AND HS.HotelID = H.ID AND HS.AllocatedRoomCount > 0) AS MinimumRoomRateHistory'	
SET @SELECTSQL = @SELECTSQL + ', 0 AS DiscountPercentage'	
SET @SELECTSQL = @SELECTSQL + ', ISNULL((SELECT SUM(TotalRoomRate) FROM dbo.TB_HotelSearch (NOLOCK) HS WHERE HS.SearchParameterID = @SearchParameterID AND HS.HotelID = H.ID AND HS.AllocatedRoomCount > 0), 0) AS DiscountedRoomRate'

SET @FILTERSQL = @FILTERSQL + 
' AND ISNULL(dbo.TBF_GetHotelMinumumRoomRate(H.ID, @StartDateID, @EndDateID, @DayCount, @SingleRate, @DoubleRate, @AgencyRate, @SecretDeal, @Genius, @UserID, NULL), 0) > 0 AND
H.ID IN(
SELECT 
HS.HotelID 
FROM dbo.TB_HotelSearch (NOLOCK) HS
WHERE HS.SearchParameterID = @SearchParameterID
GROUP BY HS.HotelID 
HAVING SUM(HS.AllocatedRoomCount) >= @RoomCount)
'

END

IF @Keyword IS NOT NULL
BEGIN
SET @FILTERSQL = @FILTERSQL + 
' AND ((H.Name LIKE ''%'' + @Keyword + ''%'') OR (Lower(H.Name) LIKE ''%'' + Lower(@Keyword) + ''%'') OR (Upper(H.Name) LIKE ''%'' + Upper(@Keyword) + ''%'') OR (H.Address LIKE ''%'' + @Keyword + ''%'') OR (Lower(H.Address) LIKE ''%'' + Lower(@Keyword) + ''%'') OR (Upper(H.Address) LIKE ''%'' + Upper(@Keyword) + ''%''))  '
END

SET @SELECTJOINSQL = @SELECTSQL + @JOINSQL
SELECT @TOTALCOUNTSQL = REPLACE(@SELECTJOINSQL, SUBSTRING(@SELECTJOINSQL, CHARINDEX('#S#', @SELECTJOINSQL), CHARINDEX('#E#', @SELECTJOINSQL) -5), ' COUNT(*) ') + @FILTERSQL

SELECT @SQL = 
'WITH R AS (SELECT T.*, ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ') AS RowNumber FROM (' +
REPLACE(REPLACE(@SELECTJOINSQL, '#S#', ' '), '#E#', ' ') + @FILTERSQL

SET @SQL = @SQL +
')T) SELECT * FROM R WHERE RowNumber BETWEEN ' + CONVERT(NVARCHAR,(((@PageIndex - 1) * @PagingSize) + 1)) + ' AND ' + CONVERT(NVARCHAR,(@PageIndex * @PagingSize))
+'  OPTION  (RECOMPILE) ; '

--print @sql

EXECUTE sp_executesql @TOTALCOUNTSQL, N'@HotelID bigint, @Name nvarchar(100), @FirmID int, @CountryID int, @CityID bigint, @RegionIDs nvarchar(MAX), @Address nvarchar(100), @HotelTypeIDs nvarchar(MAX), @HotelClassIDs nvarchar(MAX), @HotelChainIDs nvarchar(MAX), @HotelAccommodationTypeIDs nvarchar(MAX), @HotelAttributeIDs nvarchar(MAX), @StatusID int, @Active bit, @CheckInDate datetime, @CheckOutDate datetime, @RoomCount int, @GuestCount nvarchar(max), @LowerUSDPrice decimal(8,2), @UpperUSDPrice decimal(8,2), @PeopleCount int, @AdultCount int, @ChildrenCount int, @StartDateID int, @EndDateID int, @SearchParameterID int, @DayCount int, @SingleRate bit, @DoubleRate bit, @AgencyRate bit, @Keyword nvarchar(200), @SecretDeal bit, @Genius bit, @UserID bigint, @UserCountryID int', @HotelID, @Name, @FirmID, @CountryID, @CityID, @RegionIDs, @Address, @HotelTypeIDs, @HotelClassIDs, @HotelChainIDs, @HotelAccommodationTypeIDs, @HotelAttributeIDs, @StatusID, @Active, @CheckInDate, @CheckOutDate, @RoomCount, @GuestCount, @LowerUSDPrice, @UpperUSDPrice, @PeopleCount, @AdultCount, @ChildrenCount, @StartDateID, @EndDateID, @SearchParameterID, @DayCount, @SingleRate, @DoubleRate, @AgencyRate, @Keyword, @SecretDeal, @Genius, @UserID, @UserCountryID
EXECUTE sp_executesql @SQL, N'@HotelID bigint, @Name nvarchar(100), @FirmID int, @CountryID int, @CityID bigint, @RegionIDs nvarchar(MAX), @Address nvarchar(100), @HotelTypeIDs nvarchar(MAX), @HotelClassIDs nvarchar(MAX), @HotelChainIDs nvarchar(MAX), @HotelAccommodationTypeIDs nvarchar(MAX), @HotelAttributeIDs nvarchar(MAX), @StatusID int, @Active bit, @CheckInDate datetime, @CheckOutDate datetime, @RoomCount int, @GuestCount nvarchar(max), @LowerUSDPrice decimal(8,2), @UpperUSDPrice decimal(8,2), @PeopleCount int, @AdultCount int, @ChildrenCount int, @StartDateID int, @EndDateID int, @SearchParameterID int, @DayCount int, @SingleRate bit, @DoubleRate bit, @AgencyRate bit, @Keyword nvarchar(200), @SecretDeal bit, @Genius bit, @UserID bigint, @UserCountryID int,@Variables1 nvarchar(100),@Variables2 nvarchar(100) out,@Variables3 nvarchar(100) out,@Variables4 nvarchar(100) out,@Variables5 nvarchar(100) out,
@Variables6 nvarchar(100) out,@Variables7 nvarchar(100) out,@Variables8 nvarchar(100) out,@Variables9 nvarchar(100) out', @HotelID, @Name, @FirmID, @CountryID, @CityID, @RegionIDs
, @Address, @HotelTypeIDs, @HotelClassIDs, @HotelChainIDs, @HotelAccommodationTypeIDs, @HotelAttributeIDs, @StatusID, @Active, @CheckInDate, @CheckOutDate, @RoomCount, @GuestCount, @LowerUSDPrice, @UpperUSDPrice, @PeopleCount, @AdultCount, @ChildrenCount, @StartDateID, @EndDateID, @SearchParameterID, @DayCount, @SingleRate, @DoubleRate, @AgencyRate, @Keyword, @SecretDeal, @Genius, @UserID, @UserCountryID,@Variables1,@Variables2 out,@Variables3 out,@Variables4 out,@Variables5 out,@Variables6 out,@Variables7 out,@Variables8 out,@Variables9 out
IF @CheckInDate IS NOT NULL AND @RoomCount > 1
	DELETE FROM dbo.TB_HotelSearch WHERE SearchParameterID = @SearchParameterID

 GO

[B_GetSearchHotelsResults_TB_Hotel_SP]
@Culture ='en',
@OrderBy  ='Sort',
@PagingSize =15,
@PageIndex =897,	
@CountryID = NULL,
@CityID  = NULL,
@RegionIDs   = '2988507',
@Address   = NULL,
@HotelTypeIDs   = NULL,
@HotelClassIDs   = NULL,
@HotelChainIDs  = NULL,
@HotelAccommodationTypeIDs   = NULL,	
@HotelAttributeIDs   = NULL,
@StatusID  = NULL,
@Active  = NULL,
@CheckInDate  = NULL,
@CheckOutDate  = NULL,
@RoomCount  = 1,
@GuestCount  = '1,0;', 
@SecretDeal   = NULL, 
@Genius  = NULL,
@UserID  = NULL,
@UserCountryID  = NULL,
@LowerUSDPrice  = 0,
@UpperUSDPrice   = 0 
	
