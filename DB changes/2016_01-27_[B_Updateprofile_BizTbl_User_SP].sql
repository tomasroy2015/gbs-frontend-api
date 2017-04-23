 
alter procedure [dbo].[B_Updateprofile_BizTbl_User_SP]
(
@UserId varchar(100) = null,
@Email varchar(100)=null,
@UserName varchar(100) = null,
@Name varchar(100) = null,
@SurName varchar(100) = null,
@Phone varchar(100) = null,
@CountryID varchar(100) = null,
@Country varchar(max)=null,
@CityId bigint =null,
@City varchar(100) = null,
@Address varchar(100) = null,
@Postcode varchar(100) = null,
@filename varchar(max) = null
)
as
begin
set NoCount OFF
update BizTbl_User 
set UserName=@UserName,Email=@Email,Name=@Name,Surname=@SurName,Phone=@Phone,CountryID=@CountryID,Country=@Country,City=@City,
CityID=@CityId,Address=@Address,PostCode=@Postcode 
where ID=@UserId
end
GO


