 
alter Procedure [dbo].[B_GetPopularHotel_TBHotel_SP]
@Culture Varchar(50)
As
Begin
Declare @SqlQuery nvarchar(MAX)

declare @sqlquerySet nvarchar(max)
declare @sqlquerySet1 nvarchar(max)
declare @sqlquerySet2 nvarchar(max)
declare @sqlquerySet3 nvarchar(max)
declare @sqlquerySet4 nvarchar(max)
declare @sqlquerySet5 nvarchar(max)
declare @sqlquerySet6 nvarchar(max)
declare @Variables nvarchar(100)
declare @Variables1 nvarchar(100)
declare @Variables2 nvarchar(100)
declare @Variables3 nvarchar(100)
declare @Variables4 nvarchar(100)
declare @Variables5 nvarchar(100)
declare @Variables6 nvarchar(100)
set @sqlquerySet = N'(select @Variables=Description_'+ @Culture+' from BizTbl_Message where Code=''Superb'') '
set @sqlquerySet1 = N'(select @Variables1=Description_'+ @Culture+' from BizTbl_Message where Code=''Hotel'') '
set @sqlquerySet2 = N'(select @Variables2=Description_'+ @Culture+' from BizTbl_Message where Code=''ScoreFrom'') '
set @sqlquerySet3 = N'(select @Variables3=Description_'+ @Culture+' from BizTbl_Message where Code=''Reviews'') '
set @sqlquerySet4 = N'(select @Variables4=Description_'+ @Culture+' from BizTbl_Message where Code=''Description'') '
set @sqlquerySet5 = N'(select @Variables5=Description_'+ @Culture+' from BizTbl_Message where Code=''VeryGood'') '
set @sqlquerySet6 = N'(select @Variables6=Description_'+ @Culture+' from BizTbl_Message where Code=''New'') '

EXECUTE sp_executesql @sqlquerySet,N'@Variables nvarchar(100) out', @Variables out
EXECUTE sp_executesql @sqlquerySet1,N'@Variables1 nvarchar(100) out', @Variables1 out
EXECUTE sp_executesql @sqlquerySet2,N'@Variables2 nvarchar(100) out', @Variables2 out
EXECUTE sp_executesql @sqlquerySet3,N'@Variables3 nvarchar(100) out', @Variables3 out
EXECUTE sp_executesql @sqlquerySet4,N'@Variables4 nvarchar(100) out', @Variables4 out
EXECUTE sp_executesql @sqlquerySet5,N'@Variables5 nvarchar(100) out', @Variables5 out
EXECUTE sp_executesql @sqlquerySet6,N'@Variables6 nvarchar(100) out', @Variables6 out


Set @SqlQuery='Select distinct TOP 8
H.ID, 
H.Description_'+ @Culture+' As Description,
H.Name As RoutingName,
H.RoutingName as HotelRoutingName,
ISNULL(R.Name_' + @Culture + ', ISNULL(R.Name, R.NameASCII)) AS ClosestAirportName, 

H.ClosestAirportDistance,
THC.code As HotelClass,
P.Name As MainPhotoName,
ISNULL(RC.Name_' + @Culture + ', ISNULL(RC.Name, RC.NameASCII)) AS CityName,
RC.Name As CityRoutingName,
H.CurrencyID,
H.IsPreferred,
C.Symbol AS CurrencySymbol,
C.Code AS CurrencyCode,
TC.Code AS CountryCode,
TC.Name_' + @Culture + ' AS CountryName, 
TC.Name_en As CountryRoutingName,
dbo.TBF_GetHotelReviewCount(H.ID) AS ReviewCount,
dbo.TBF_GetHotelReviewCountByHotel(H.ID) As AveragePoint,
dbo.TBF_GetHotelMinimumRoomPriceWithoutDate(H.ID) As RoomPrice,
@Variables As Superb,
@Variables1 As Hotel,
@Variables2 As ScoreFrom,
@Variables3 As Reviews,
@Variables4 As DescriptionText,
@Variables5 As VeryGood,
@Variables6 As New,
H.RegionID

from TB_Hotel H 
INNER JOIN dbo.TB_Country (NOLOCK) TC ON TC.ID = H.CountryID
left Join TB_Region R 
on R.ID=H.ClosestAirportID
left Join TB_TypeHotelClass THC 
on THC.ID=H.HotelClassID
left Join TB_Photo P
on P.RecordID=H.ID AND P.PartID = 1 AND P.MainPhoto = 1
left Join TB_Region RC 
on RC.ID=H.CityID
left Join TB_Currency C 
ON C.ID = H.CurrencyID
Where H.Active=''true'' and  P.id is not null  and H.ID not in(100003,102002)
order by ReviewCount DESC
'
EXECUTE  sp_executesql @SqlQuery,
N'@Variables nvarchar(100) out,@Variables1 nvarchar(100) out,@Variables2 nvarchar(100) out,@Variables3 nvarchar(100) out,@Variables4 nvarchar(100) out,@Variables5 nvarchar(100) out,@Variables6 nvarchar(100) out',@Variables out,@Variables1 out,@Variables2 out,@Variables3 out,@Variables4 out,@Variables5 out,@Variables6 out
end

