 
SET QUOTED_IDENTIFIER ON
GO

ALTER Procedure [dbo].[B_GetHotelByID_TB_Hotel_SP]  
@Culture Varchar(50)=null,  
@HotelID Varchar(50)=null  
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
declare @sqlquerySet7 nvarchar(max)  
declare @sqlquerySet8 nvarchar(max)  
declare @sqlquerySet9 nvarchar(max)  
declare @sqlquerySet10 nvarchar(max) 
declare @sqlquerySet11 nvarchar(max) 
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
declare @Variables10 nvarchar(100)  
declare @Variables11 nvarchar(100)  
declare @Variables12 nvarchar(100)  
set @sqlquerySet = N'(select @Variables=Description_'+ @Culture+' from BizTbl_Message where Code=''Superb'') '  
set @sqlquerySet1 = N'(select @Variables1=Description_'+ @Culture+' from BizTbl_Message where Code=''Hotel'') '  
set @sqlquerySet2 = N'(select @Variables2=Description_'+ @Culture+' from BizTbl_Message where Code=''ScoreFrom'') '  
set @sqlquerySet3 = N'(select @Variables3=Description_'+ @Culture+' from BizTbl_Message where Code=''Reviews'') '  
set @sqlquerySet4 = N'(select @Variables4=Description_'+ @Culture+' from BizTbl_Message where Code=''Description'') '  
set @sqlquerySet5 = N'(select @Variables5=Description_'+ @Culture+' from BizTbl_Message where Code=''VeryGood'') '  
set @sqlquerySet6 = N'(select @Variables6=Description_'+ @Culture+' from BizTbl_Message where Code=''New'') '  
set @sqlquerySet7 = N'(select @Variables7=Description_'+ @Culture+' from BizTbl_Message where Code=''Policiesof'') '  
set @sqlquerySet8 = N'(select @Variables8=Description_'+ @Culture+' from BizTbl_Message where Code=''Facilitiesof'') '  
set @sqlquerySet9 = N'(select @Variables9=Description_'+ @Culture+' from BizTbl_Message where Code=''ShowMap'') '  
set @sqlquerySet10 = N'(select @Variables10=Description_'+ @Culture+' from BizTbl_Message where Code=''RatingBasedOnReview'') '
set @sqlquerySet11 = N'(select @Variables11=Description_'+ @Culture+' from BizTbl_Message where Code=''WishListAdded'') '
set @sqlquerySet12 = N'(select @Variables12=count(*) from TB_WishLists where HotelID='''+ @HotelID+''') '
  
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

EXECUTE sp_executesql @sqlquerySet10,N'@Variables10 nvarchar(100) out', @Variables10 out  
EXECUTE sp_executesql @sqlquerySet11,N'@Variables11 nvarchar(100) out', @Variables11 out  
EXECUTE sp_executesql @sqlquerySet12,N'@Variables12 nvarchar(100) out', @Variables12 out  
  
  
Set @SqlQuery='Select distinct  
H.ID,
R.ID as RegionID,   
H.Description_'+ @Culture+' As Description,  
H.Name As RoutingName,  
  
ISNULL(R.Name_' + @Culture + ', ISNULL(R.Name, R.NameASCII)) AS ClosestAirportName,   
H.ClosestAirportID,  
ISNULL(CA.Name_' + @Culture + ', ISNULL(CA.Name, CA.NameASCII)) + '' ('' + CA.Code + ''), '' + ISNULL(CAP.Name_' + @Culture + ', ISNULL(CAP.Name, CAP.NameASCII)) AS ClosestAirportNameWithParentNameAndCode,  
H.ClosestAirportDistance,  
THC.code As HotelClass,  
THC.Name_' + @Culture + ' As HotelStar,  
P.Name As MainPhotoName,  
ISNULL(RC.Name_' + @Culture + ', ISNULL(RC.Name, RC.NameASCII)) AS CityName,  
ISNULL(RC.Name_en, ISNULL(RC.Name, RC.NameASCII)) AS CityName_en, 
H.CityID,  
H.CountryID,  
TC.Name_'+@Culture+' as CountryName, 
TC.Name_en as CountryName_en,  
TC.ID,  
H.Address,  
H.IsPreferred,  
H.Phone,  
H.Fax,  
h.Email,  
H.PostCode,  
H.CreditCardNotRequired,  
H.FirmID,  
BC.Code As HotelCultureCode,  
H.CurrencyID,  
C.Symbol AS CurrencySymbol,  
dbo.TBF_GetHotelReviewCount(H.ID) AS ReviewCount,  
dbo.TBF_GetHotelMinimumRoomPriceWithoutDate(H.ID) As MinimumRoomPrice,  
dbo.TBF_GetHotelReviewCountByHotel(H.ID) As AveragePoint,  
(CASE WHEN H.ReviewPoint IS NOT NULL THEN H.ReviewPoint ELSE dbo.TBF_GetHotelAverageReviewPoint(H.ID) END) AS AverageReviewPoint,
TRE.Name_' + @Culture + ' AS ReviewEvaluationName,
@Variables As Superb,  
@Variables1 As Hotel,  
@Variables2 As ScoreFrom,  
@Variables3 As Reviews,  
@Variables4 As DescriptionText,  
@Variables5 As VeryGood,  
@Variables6 As New,  
@Variables7 As Policiesof,  
@Variables8 As Facilitiesof,  
@Variables9 As ShowMap  ,
@Variables10 As RatingBasedOnReview,  
@Variables11 As WishListAdded,  
@Variables12 As WishListCount   
from TB_Hotel H   
left Join BizTbl_Culture BC on BC.ID=H.CultureID  
left Join TB_Region R on R.ID=H.ClosestAirportID  
left Join TB_TypeHotelClass THC on THC.ID=H.HotelClassID  
left Join TB_Photo P on P.RecordID=H.ID AND P.PartID = 1 AND P.MainPhoto = 1  
left Join TB_Region RC on RC.ID=H.CityID   
left join TB_Country TC   
on H.CountryID = TC.ID  
  
left Join TB_Currency C   
ON C.ID = H.CurrencyID  
  
LEFT JOIN dbo.TB_Region (NOLOCK) CA ON CA.ID = H.ClosestAirportID  
  
LEFT JOIN dbo.TB_Region (NOLOCK) CAP ON CAP.ID = CA.ParentID   LEFT JOIN dbo.TB_TypeReviewEvaluation (NOLOCK) TRE ON ((CASE WHEN H.ReviewPoint IS NOT NULL THEN H.ReviewPoint ELSE dbo.TBF_GetHotelAverageReviewPoint(H.ID) END) * 2) BETWEEN TRE.StartPoint AND TRE.EndPoint
Where H.Active=''true'' and H.ID='+@HotelID+'  
order by H.id asc  
'  
EXECUTE  sp_executesql @SqlQuery,  
N'@Variables nvarchar(100) out,@Variables1 nvarchar(100) out,@Variables2 nvarchar(100) out,@Variables3 nvarchar(100) out,  
 @Variables4 nvarchar(100) out,@Variables5 nvarchar(100) out,@Variables6 nvarchar(100) out,@Variables7 nvarchar(100) out,
 @Variables8 nvarchar(100) out,@Variables9 nvarchar(100) out,@Variables10 nvarchar(100) out,@Variables11 nvarchar(100) out,
 @Variables12 nvarchar(100) out',  
  @Variables out,@Variables1 out,@Variables2 out,@Variables3 out,@Variables4 out,@Variables5 out,@Variables6 out,
  @Variables7 out,@Variables8 out,@Variables9 out,@Variables10 out,@Variables11 out,@Variables12 out 
end 
GO