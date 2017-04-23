use [Gbshotels] 
 Go
alter Procedure [dbo].[B_GetHotelRoomDetails_TB_HotelRoom_SP]  
@Culture Varchar(50)=null,  
@HotelID Varchar(50)=null  
As  
Begin  
  
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
set @sqlquerySet = N'(select @Variables=Description_'+ @Culture+' from BizTbl_Message where Code=''Checkin'') '  
set @sqlquerySet1 = N'(select @Variables1=Description_'+ @Culture+' from BizTbl_Message where Code=''Checkout'') '  
set @sqlquerySet2 = N'(select @Variables2=Description_'+ @Culture+' from BizTbl_Message where Code=''RoomType'') '  
set @sqlquerySet3 = N'(select @Variables3=Description_'+ @Culture+' from BizTbl_Message where Code=''RoomPrice'') '  
set @sqlquerySet4 = N'(select @Variables4=Description_'+ @Culture+' from BizTbl_Message where Code=''RoomMaxPeopleCount'') '  
set @sqlquerySet5 = N'(select @Variables5=Description_'+ @Culture+' from BizTbl_Message where Code=''Quantity'') '  
set @sqlquerySet6 = N'(select @Variables6=Description_'+ @Culture+' from BizTbl_Message where Code=''UntilTime'') '  
set @sqlquerySet7 = N'(select @Variables7=Description_'+ @Culture+' from BizTbl_Message where Code=''FromTime'') '  
set @sqlquerySet8 = N'(select @Variables8=Description_'+ @Culture+' from BizTbl_Message where Code=''FREEcancellation'') '  
set @sqlquerySet9 = N'(select @Variables9=Description_'+ @Culture+' from BizTbl_Message where Code=''RoomFacilities'') '  
set @sqlquerySet10 = N'(select @Variables10=Description_'+ @Culture+' from BizTbl_Message where Code=''HotelCreditCard'') '  
set @sqlquerySet12 = N'(select @Variables11=Description_'+ @Culture+' from BizTbl_Message where Code=''CancelPolicy'') '  
  
  
  
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
EXECUTE sp_executesql @sqlquerySet12,N'@Variables11 nvarchar(100) out', @Variables11 out  
  
Declare @SqlQuery nvarchar(MAX)  
  
Set @SqlQuery='Select HR.ID,  
HR.HotelID,   
HR.Description_'+ @Culture+' As Description,  
HR.RoomTypeID,  
TR.Name_'+ @Culture+'  AS HotelTypeName,  
TS.Name_'+ @Culture+'  AS SmokingTypeName,  
HR.RoomCount,  
HR.RoomSize,  
HR.MaxPeopleCount,  
HR.MaxChildrenCount,  
HR.BabyCotCount,  
HR.ExtraBedCount,  
HR.SmokingTypeID,  
H.CheckinStart,  
H.CheckinEnd,  
H.CheckoutStart,  
H.CheckoutEnd ,  
@Variables as Checkin,  
@Variables1 as Checkout,  
@Variables2 as RoomTypeText,  
@Variables3 as RoomPriceText,  
@Variables4 as RoomMaxPeopleCount,  
@Variables5 as Quantity,  
@Variables6 as UntilTime,  
@Variables7 as FromTime,  
@Variables8 as FREEcancellation,  
@Variables9 as RoomFacilities,  
@Variables10 as HotelCreditCard,  
@Variables11 as CancelPolicy  
From 
TB_HotelRoom  HR 
left join TB_Hotel H on H.ID=HR.HotelID   
left join  TB_TypeRoom TR on HR.RoomTypeID=TR.ID  
left join TB_TypeSmoking TS on HR.SmokingTypeID=TS.ID  
 
Where HR.HotelID='+@HotelID+' and HR.Active=1 and TR.Active=1  
Order by HR.ID asc'  
  --print @SqlQuery
  --Where HR.HotelID='+@HotelID+' and HR.Active=1 and HR.Description_'+ @Culture+' is not null and TR.Active=1  
EXECUTE  sp_executesql @SqlQuery,  
   
N'@Variables nvarchar(100) out,@Variables1 nvarchar(100) out,@Variables2 nvarchar(100) out,@Variables3 nvarchar(100) out,@Variables4 nvarchar(100) out,@Variables5 nvarchar(100) out,@Variables6 nvarchar(100) out,
@Variables7 nvarchar(100) out,@Variables8 nvarchar(100) out,@Variables9 nvarchar(100) out,@Variables10 nvarchar(100) out,@Variables11 nvarchar(100) out',  
@Variables out,@Variables1 out,@Variables2 out,@Variables3 out,@Variables4 out,@Variables5 out,@Variables6 out,@Variables7 out,@Variables8 out,@Variables9 out,@Variables10 out,@Variables11 out     
  
declare @datetime datetime = getdate()

Exec [TB_SP_GetHotelAttributes] @HotelID,null,null,@datetime,null,@Culture,'AttributeTypeID,AttributeHeaderID'  
Exec [TB_SP_GetHotelCreditCards] @HotelID,@Culture,'ID'  
  
set @sqlquerySet11 = N'(select Description_'+ @Culture+' As CancelPolicyWarning from BizTbl_Message where Code=''CancelPolicyWarning'') '  
EXECUTE sp_executesql @sqlquerySet11  
  
Select 
H.ID as HotelID,
H.CheckinStart,  
H.CheckinEnd,  
H.CheckoutStart,  
H.CheckoutEnd ,  
@Variables as Checkin,  
@Variables1 as Checkout,  
@Variables2 as RoomTypeText,  
@Variables3 as RoomPriceText,  
@Variables4 as RoomMaxPeopleCount,  
@Variables5 as Quantity,  
@Variables6 as UntilTime,  
@Variables7 as FromTime,  
@Variables8 as FREEcancellation,  
@Variables9 as RoomFacilities,  
@Variables10 as HotelCreditCard,  
@Variables11 as CancelPolicy  
From 
TB_Hotel H  
Where H.ID =@HotelID 


End
GO
exec [B_GetHotelRoomDetails_TB_HotelRoom_SP]  'en',100002
exec [B_GetHotelRoomDetails_TB_HotelRoom_SP]  'en',100001
