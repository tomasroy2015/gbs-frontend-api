alter procedure [dbo].[B_GetHotels_TB_Hotel_SP]
(
@city varchar(50) =null,
@Cultureid Varchar(50) =null
)
As
Begin
Declare @SqlQuery nvarchar(MAX)
Declare @sqlQuery1 nvarchar(MAX)

declare @sqlquerySet nvarchar(max)
declare @sqlquerySet1 nvarchar(max)
declare @sqlquerySet2 nvarchar(max)
declare @sqlquerySet3 nvarchar(max)
declare @sqlquerySet4 nvarchar(max)
declare @sqlquerySet5 nvarchar(max)
declare @sqlquerySet6 nvarchar(max)
declare @sqlquerySet7 nvarchar(max)
declare @sqlquerySet8 nvarchar(max)
declare @sqlquerySet9 nvarchar(max)
declare @sqlquerySet12 nvarchar(max)

declare @Variables nvarchar(100)
declare @Variables1 nvarchar(100)
declare @Variables2 nvarchar(100)
declare @Variables3 nvarchar(100)
declare @Variables4 nvarchar(100)
declare @Variables5 nvarchar(100)
declare @Variables6 nvarchar(100)
declare @Variables7 nvarchar(100)
declare @Variables8 nvarchar(100)
declare @Variables9 nvarchar(100)
declare @Variables12 nvarchar(100)

set @sqlquerySet = N'(select @Variables=Description_'+ @Cultureid+' from BizTbl_Message where Code=''Superb'') '
set @sqlquerySet1 = N'(select @Variables1=Description_'+ @Cultureid+' from BizTbl_Message where Code=''Hotel'') '
set @sqlquerySet2 = N'(select @Variables2=Description_'+ @Cultureid+' from BizTbl_Message where Code=''ScoreFrom'') '
set @sqlquerySet3 = N'(select @Variables3=Description_'+ @Cultureid+' from BizTbl_Message where Code=''Reviews'') '
set @sqlquerySet4 = N'(select @Variables4=Description_'+ @Cultureid+' from BizTbl_Message where Code=''Description'') '
set @sqlquerySet5 = N'(select @Variables5=Description_'+ @Cultureid+' from BizTbl_Message where Code=''VeryGood'') '
set @sqlquerySet6 = N'(select @Variables6=Description_'+ @Cultureid+' from BizTbl_Message where Code=''New'') '
set @sqlquerySet7 = N'(select @Variables7=Description_'+ @Cultureid+' from BizTbl_Message where Code=''No'')'
set @sqlquerySet8 = N'(select @Variables8=Description_'+ @Cultureid+' from BizTbl_Message where Code=''Averagepricepernight'')'
set @sqlquerySet9 = N'(select @Variables9=Description_'+ @Cultureid+' from BizTbl_Message where Code=''KmFrom'')'
set @sqlquerySet12 = N'( select top 1 @Variables12 = HR1.ID from TB_Hotel H join TB_HotelRoom HR on HR.HotelID=H.FirmID join TB_HotelRate HR1 on HR1.HotelRoomID=HR.ID where HR1.RoomPrice!=0.00 and H.CityID='+@city+' group by H.FirmID,HR.HotelID,HR1.HotelRoomID,HR1.ID)'

EXECUTE sp_executesql @sqlquerySet,N'@Variables nvarchar(100) out', @Variables out
EXECUTE sp_executesql @sqlquerySet1,N'@Variables1 nvarchar(100) out', @Variables1 out
EXECUTE sp_executesql @sqlquerySet2,N'@Variables2 nvarchar(100) out', @Variables2 out
EXECUTE sp_executesql @sqlquerySet3,N'@Variables3 nvarchar(100) out', @Variables3 out
EXECUTE sp_executesql @sqlquerySet4,N'@Variables4 nvarchar(100) out', @Variables4 out
EXECUTE sp_executesql @sqlquerySet5,N'@Variables5 nvarchar(100) out', @Variables5 out
EXECUTE sp_executesql @sqlquerySet6,N'@Variables6 nvarchar(100) out', @Variables6 out
EXECUTE sp_executesql @sqlquerySet7,N'@Variables7 nvarchar(100) out', @Variables7 out
EXECUTE sp_executesql @sqlquerySet8,N'@Variables8 nvarchar(100) out', @Variables8 out
EXECUTE sp_executesql @sqlquerySet9,N'@Variables9 nvarchar(100) out', @Variables9 out
EXECUTE sp_executesql @sqlquerySet12,N'@Variables12 nvarchar(100) out', @Variables12 out

Set @SqlQuery='Select distinct
H.ID, 
H.Description_'+ @Cultureid+' As Description,

H.Name As RoutingName,

ISNULL(R.Name_' + @Cultureid + ', ISNULL(R.Name, R.NameASCII)) AS ClosestAirportName, 

H.ClosestAirportDistance,
THC.code As HotelClass,
P.Name As MainPhotoName,
ISNULL(RC.Name_' + @Cultureid + ', ISNULL(RC.Name, RC.NameASCII)) AS CityName,
RC.Name As CityRoutingName,
H.CurrencyID,
H.IsPreferred,
C.Symbol AS CurrencySymbol,
C.Code As CurrencyCode,
dbo.TBF_GetHotelReviewCount(H.ID) AS ReviewCount,
@Variables As Superb,
@Variables1 As Hotel,
@Variables2 As ScoreFrom,
@Variables3 As Reviews,
@Variables4 As DescriptionText,
@Variables5 As VeryGood,
@Variables6 As New,
@Variables7 As No,
@Variables8 As Averagepricepernight,
@Variables9 As KmFrom,
dbo.TBF_GetHotelMinimumRoomPriceWithoutDate(H.ID) As RoomPrice,

@Variables12 As RoomId,
Co.Code AS CountryCode,
Co.Name_en As CountryRoutingName,
H.RoutingName As HotelRoutingName,
H.RegionID
from TB_Hotel H 
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
join TB_Country Co
on 
H.CountryID=Co.ID


Where H.Active=''true'' and H.StatusID=2 and CityID = '+@city+'  

order by H.id asc 
'

EXECUTE  sp_executesql @SqlQuery,N'@Variables nvarchar(100) out,@Variables1 nvarchar(100) out,@Variables2 nvarchar(100) out,@Variables3 nvarchar(100) out, @Variables4 nvarchar(100) out,@Variables5 nvarchar(100) out,@Variables6 nvarchar(100) out,@Variables7 nvarchar(100) out,@Variables8 nvarchar(100) out,@Variables9 nvarchar(100) out,@Variables12 nvarchar(100) out',@Variables out,@Variables1 out,@Variables2 out,@Variables3 out,@Variables4 out,@Variables5 out,@Variables6 out, @Variables7 out,@Variables8 out,@Variables9 out ,@Variables12 out
end

