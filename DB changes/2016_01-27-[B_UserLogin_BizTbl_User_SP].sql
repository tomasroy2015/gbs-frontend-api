alter procedure [dbo].[B_UserLogin_BizTbl_User_SP]
(
@Emailid varchar(100) =null,
@Password varchar(100) =null
)
as
begin
select UserName,ID from BizTbl_User where (Email=@Emailid OR UserName=@Emailid) and Password=@Password and Active=1
end
GO