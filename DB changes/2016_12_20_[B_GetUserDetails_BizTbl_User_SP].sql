USE [Gbshotels]
GO
/****** Object:  StoredProcedure [dbo].[B_GetUserDetails_BizTbl_User_SP]    Script Date: 20-12-2016 18:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
[B_GetUserDetails_BizTbl_User_SP] '180191'
ALTER Procedure [dbo].[B_GetUserDetails_BizTbl_User_SP]
(@UserID varchar(70))
as
begin

Declare @SqlQuery nvarchar(MAX)  
  
declare @sqlquerySet nvarchar(max)  
declare @sqlquerySet1 nvarchar(max)  
declare @sqlquerySet2 nvarchar(max) 
declare @Variables nvarchar(100)  
declare @Variables1 nvarchar(100)  
declare @Variables2 nvarchar(100)   
set @sqlquerySet = N'(select  @Variables=  COUNT(DISTINCT CountryID) from TB_Reservation TR where UserID='+@UserID+') '  
set @sqlquerySet1 = N'(select @Variables1=  COUNT(DISTINCT City) from TB_Reservation TR where UserID='+@UserID+') '  
set @sqlquerySet2 = N'(select @Variables2= COUNT(*) from TB_Reservation TR where UserID='+@UserID+') ' 

EXECUTE sp_executesql @sqlquerySet,N'@Variables nvarchar(100) out', @Variables out  
EXECUTE sp_executesql @sqlquerySet1,N'@Variables1 nvarchar(100) out', @Variables1 out  
EXECUTE sp_executesql @sqlquerySet2,N'@Variables2 nvarchar(100) out', @Variables2 out  
Set @SqlQuery='select 
ID,
Name,
Address,
Email,
Phone,
UserName,
@Variables As CountryCount,  
@Variables1 As CityCount,  
@Variables2 As TripCount, 
RIGHT(CONVERT(VARCHAR(11), getdate(), 106), 9) AS CreatedDate, 
VerificationCode, 
Genius,
case when Userphoto is Null then ''37c658304ec04310263ebfe3280f0894304a18c4.png'' else Userphoto  
end as UserPhoto from BizTbl_User where ID='+ @UserID+'' 


EXECUTE  sp_executesql @SqlQuery,  
N'@Variables nvarchar(100) out,@Variables1 nvarchar(100) out,@Variables2 nvarchar(100) out',  
  @Variables out,@Variables1 out,@Variables2 out

--select COUNT(CountryID) As CountryCount from TB_Reservation TR where UserID=@UserID
--select COUNT(City) As CityCount from  TB_Reservation TR where UserID=@UserID
--select COUNT(*) As TripCount from TB_Reservation TR where UserID=@UserID
end