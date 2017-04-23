alter Procedure [dbo].[B_GetRecentlyViewedHotels_TB_HotelSearchHistory_SP]
(@CultureID varchar(70),
@UserID varchar(100))
as
begin

declare @RecentHotels Nvarchar(max)

declare @sqlquerySet1 nvarchar(max)
declare @sqlquerySet2 nvarchar(max)
declare @sqlquerySet3 nvarchar(max)

declare @Variables1 nvarchar(100)
declare @Variables2 nvarchar(100)
declare @Variables3 nvarchar(100)


SET @sqlquerySet1 = N'(select @Variables1=Description_'+ @CultureID+' from BizTbl_Message where Code=''Hotel'')'
set @sqlquerySet2 = N'(select @Variables2=Description_'+ @CultureID+' from BizTbl_Message where Code=''ScoreFrom'')'
set @sqlquerySet3 = N'(select @Variables3=Description_'+ @CultureID+' from BizTbl_Message where Code=''Reviews'')'

EXECUTE sp_executesql @sqlquerySet1,N'@Variables1 nvarchar(100) out', @Variables1 out
EXECUTE sp_executesql @sqlquerySet2,N'@Variables2 nvarchar(100) out', @Variables2 out
EXECUTE sp_executesql @sqlquerySet3,N'@Variables3 nvarchar(100) out', @Variables3 out
Set @RecentHotels='Select 
H.ID,
HS.ID as SearchID,
H.FirmID,
H.Name,
H.IsPreferred,
H.RoutingName,
ISNULL(R.Name_' + @CultureID + ', ISNULL(R.Name, R.NameASCII)) AS ClosestAirportName,
ISNULL(RC.Name_' + @CultureID + ', ISNULL(RC.Name, RC.NameASCII)) AS CityName,
H.ClosestAirportDistance,
H.Description_'+ @CultureID +' as Description,
H.Address,
C.Name_'+ @CultureID +' as CountryName,
RC.Name As CityRoutingName,
C.Name_en As CountryRoutingName,
C.Code AS CountryCode,
dbo.TBF_GetHotelReviewCount(H.ID) AS ReviewCount,
dbo.TBF_GetHotelMinimumRoomPriceWithoutDate(H.ID) As MinimumRoomPrice,
dbo.TBF_GetHotelReviewCountByHotel(H.ID) As AveragePoint,
TRE.Name_' + @CultureID + ' AS ReviewEvaluationName,
P.Name As MainPhotoName,
THC.code As HotelClass,
@Variables1 as Hotel,
@Variables2 as ScoreFrom,
@Variables3 as Reviews,
H.RegionID
From 
TB_Hotel H Join
TB_HotelSearchHistory HS
on
HS.HotelID=H.ID
left Join TB_Region R 
on R.ID=H.ClosestAirportID
left Join TB_Region RC 
on RC.ID=H.CityID
join TB_Country C
on 
H.CountryID=C.ID
left Join TB_TypeHotelClass THC 
on THC.ID=H.HotelClassID
left Join TB_Photo P
on P.RecordID=H.ID AND P.PartID = 1 AND P.MainPhoto = 1
LEFT JOIN dbo.TB_TypeReviewEvaluation (NOLOCK) TRE ON ((CASE WHEN H.ReviewPoint IS NOT NULL THEN H.ReviewPoint ELSE dbo.TBF_GetHotelAverageReviewPoint(H.ID) END) * 2) BETWEEN TRE.StartPoint AND TRE.EndPoint
where HS.UserID='''+ @UserID +''' and HS.Status=1
ORDER BY HS.UpdatedDate DESC' 
Execute sp_executesql @RecentHotels,
N'@Variables1 nvarchar(100) out,@Variables2 nvarchar(100) out,@Variables3 nvarchar(100) out',
@Variables1 out,@Variables2 out,@Variables3 out
end

