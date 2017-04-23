 
alter procedure [dbo].[B_InsertNewAccount_BizTbl_User_SP]
(
@Email varchar(100) =null,
@Password varchar(100) =null,
@UserName varchar(100) =null,
@varificationcode varchar(100) =null
)
as
begin

declare @RegisterId int
if not exists ( select ID from BizTbl_User where Email=@Email and Password=@Password and UserName=@UserName )
begin
insert into BizTbl_User(Email,Password,UserName,Name,Surname,Active,VerificationCode,StatusId) values(@Email,@Password,@UserName,@UserName,@UserName,0,@varificationcode,1)
end
select @RegisterId=ID from BizTbl_User where Email=@Email and Password=@Password and UserName=@UserName
select @RegisterId as ID
end

