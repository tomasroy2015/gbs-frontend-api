
delete from dbo.TB_HotelReservationRate
where  HotelReservationID in ( select HotelReservationID from dbo.TB_HotelReservation where OpUserID=180168)
delete from dbo.TB_HotelReservationRateHistory 
where  HotelReservationID in ( select HotelReservationID from dbo.TB_HotelReservation where OpUserID=180168)

delete from dbo.TB_ReservationStatusHistory where  OpUserID=180168


delete from dbo.TB_Reservation where CreateUserID=180168
delete from dbo.TB_HotelReservation where OpUserID=180168
delete from dbo.TB_ReservationHistory where  OpUserID=180168

delete from BizTbl_UserOperation where Userid=180168
delete  from BizTbl_User where id=180168