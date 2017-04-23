 
ALTER PROC [dbo].[TB_SP_GetHotels]
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
	@HotelTypeID int = NULL,
	@HotelClassID int = NULL,
	@HotelChainID int = NULL,
	@HotelAccommodationTypeID int = NULL,	
	@StatusID int = NULL,
	@IsMainPageDisplay bit = NULL,
	@Active bit = NULL,
	@HotelIDs nvarchar(MAX)= NULL
)
AS

DECLARE @SQL NVARCHAR(MAX)
DECLARE @FILTERSQL NVARCHAR(MAX)
DECLARE @TOTALCOUNTSQL NVARCHAR(MAX)


DECLARE @HIDs NVARCHAR(MAX)
set @HIDs=''
 if (@HotelIDs is not null)
 Begin 
  set @HIDs = ' AND (@HotelIDs IS NULL OR (H.ID IN (SELECT Items FROM Split(@HotelIDs, '','')))) '
 end 

SET @SQL = 
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
ISNULL(ISNULL(R.HasCityTax, TC.HasCityTax), 0) AS CityTaxApplied,
H.ClosestAirportID,
TC.Code AS CountryCode,
TC.Name_' + @Culture + ' AS CountryName, 
ISNULL(TC.VAT,0) AS VAT,
ISNULL(R.Name_' + @Culture + ', ISNULL(R.Name, R.NameASCII)) AS RegionName, 
ISNULL(MR.Name_' + @Culture + ', ISNULL(MR.Name, MR.NameASCII)) AS MainRegionName, 
ISNULL(CAC.Name_' + @Culture + ', ISNULL(CAC.Name, CAC.NameASCII)) AS CityName, 
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
H.StatusID,
H.IsSecret,
H.IsPreferred,
H.ShowOffline,
H.ChannelManagerID,
H.CurrencyID,
C.Symbol AS CurrencySymbol,
H.CreditCardNotRequired,
H.IsMainPageDisplay,
H.MainPageDisplaySort,
H.AvailabilityRateUpdate,
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
THA.Description_' + @Culture + ' AS HotelAccommodationTypeDescription,
TS.Name_' + @Culture + ' AS StatusName,
dbo.TBF_GetHotelReviewCount(H.ID) AS ReviewCount,
(CASE WHEN H.ReviewPoint IS NOT NULL THEN H.ReviewPoint ELSE dbo.TBF_GetHotelAverageReviewPoint(H.ID) END) AS AverageReviewPoint,
TRE.Name_' + @Culture + ' AS ReviewEvaluationName,
ISNULL(MP.Name,'''') AS MainPhotoName,
H.LatestBookingDate,
H.LatestBookingCountryID,
ISNULL(LBC.Name_' + @Culture + ', '''') AS LatestBookingCountryName  
#E# FROM dbo.TB_Hotel (NOLOCK) H
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
LEFT JOIN dbo.TB_TypeReviewEvaluation (NOLOCK) TRE ON ((CASE WHEN H.ReviewPoint IS NOT NULL THEN H.ReviewPoint ELSE dbo.TBF_GetHotelAverageReviewPoint(H.ID) END) *2) BETWEEN TRE.StartPoint AND TRE.EndPoint
LEFT JOIN dbo.TB_Currency (NOLOCK) C ON C.ID = H.CurrencyID
LEFT JOIN dbo.TB_Photo (NOLOCK) MP ON MP.RecordID = H.ID AND MP.PartID = 1 AND MP.MainPhoto = 1 
LEFT JOIN dbo.TB_Country (NOLOCK) LBC ON LBC.ID = H.LatestBookingCountryID '

SET @FILTERSQL = '
WHERE 
(@HotelID IS NULL OR H.ID = @HotelID) AND
(@Name IS NULL OR H.Name LIKE ''%'' + @Name + ''%'') AND
(@FirmID IS NULL OR H.FirmID = @FirmID) AND
(@CountryID IS NULL OR H.CountryID = @CountryID) AND
(@CityID IS NULL OR H.CityID = @CityID) AND
(@Address IS NULL OR H.Address LIKE ''%'' + @Address + ''%'') AND
(@RegionIDs IS NULL OR (H.RegionID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (H.MainRegionID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (R.ParentID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (R.SecondParentID IN (SELECT Items FROM Split(@RegionIDs, '';''))) OR (H.ID IN (SELECT HR.HotelID FROM dbo.TB_HotelRegion HR (NOLOCK) WHERE HR.RegionID IN (SELECT Items FROM Split(@RegionIDs, '';''))))) AND
(@HotelTypeID IS NULL OR H.HotelTypeID = @HotelTypeID) AND
(@HotelClassID IS NULL OR H.HotelClassID = @HotelClassID) AND
(@HotelChainID IS NULL OR H.HotelChainID = @HotelChainID) AND
(@HotelAccommodationTypeID IS NULL OR H.HotelAccommodationTypeID = @HotelAccommodationTypeID) AND
(@StatusID IS NULL OR H.StatusID = @StatusID) AND
(@IsMainPageDisplay IS NULL OR H.IsMainPageDisplay = @IsMainPageDisplay) AND
(@Active IS NULL OR H.Active = @Active)
' 
 + @HIDs
 SELECT @TOTALCOUNTSQL = REPLACE(@SQL, SUBSTRING(@SQL, CHARINDEX('#S#', @SQL), CHARINDEX('#E#', @SQL) -5), ' COUNT(*) ') + @FILTERSQL

 print '@TOTALCOUNTSQL ' + cast( Len( @TOTALCOUNTSQL)  as varchar)
SELECT @SQL = 
'WITH R AS (SELECT T.*, ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ') AS RowNumber FROM (' +
REPLACE(REPLACE(@SQL + @FILTERSQL, '#S#', ' '), '#E#', ' ')

SET @SQL = @SQL + 
')T) SELECT * FROM R WHERE RowNumber BETWEEN ' + CONVERT(NVARCHAR,(((@PageIndex - 1) * @PagingSize) + 1)) + ' AND ' + CONVERT(NVARCHAR,(@PageIndex * @PagingSize))

--print @SQL

 print '@@SQL ' + cast( Len( @SQL)  as varchar)

EXECUTE sp_executesql @TOTALCOUNTSQL, N'@HotelID bigint, @Name nvarchar(100), @FirmID int, @CountryID int, @CityID bigint, @RegionIDs nvarchar(MAX), @Address nvarchar(100), @HotelTypeID int, @HotelClassID int, @HotelChainID int, @HotelAccommodationTypeID int, @StatusID int, @IsMainPageDisplay bit, @Active bit,@HotelIDs nvarchar(MAX)', @HotelID, @Name, @FirmID, @CountryID, @CityID, @RegionIDs, @Address, @HotelTypeID, @HotelClassID, @HotelChainID, @HotelAccommodationTypeID, @StatusID, @IsMainPageDisplay, @Active,@HotelIDs 
EXECUTE sp_executesql @SQL, N'@HotelID bigint, @Name nvarchar(100), @FirmID int, @CountryID int, @CityID bigint, @RegionIDs nvarchar(MAX), @Address nvarchar(100), @HotelTypeID int, @HotelClassID int, @HotelChainID int, @HotelAccommodationTypeID int, @StatusID int, @IsMainPageDisplay bit, @Active bit,@HotelIDs nvarchar(MAX)', @HotelID, @Name, @FirmID, @CountryID, @CityID, @RegionIDs, @Address, @HotelTypeID, @HotelClassID, @HotelChainID, @HotelAccommodationTypeID, @StatusID, @IsMainPageDisplay, @Active,@HotelIDs 

GO
 
 exec [TB_SP_GetHotels]
 @OrderBy='ID',
 @Culture='en',
 @regionIds='2988507',
 @PagingSize=1121212121
 ,@PageIndex=1,
 @Active = 1
, @HotelIDs ='100002'
