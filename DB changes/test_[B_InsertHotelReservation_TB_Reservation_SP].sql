USE [Gbshotels]
GO
/****** Object:  StoredProcedure [dbo].[B_InsertHotelReservation_TB_Reservation_SP]    Script Date: 21-11-2016 13:29:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

[B_InsertHotelReservation_TB_Reservation_SP]
1030044
,'TND84BHV'
,1
,1
,'100002'
,'100002'
,''
,'1'
,'89'
,'1'
,'abhishek'
,'bha'
,'bhalaniabhishek@gmail.com'
,'99899898'
,'1055'
,'1055'
,'True'
,'EUR'
,'100002'
,'0' --cre
,'UBB2td6mPgq0Dps8KBPByg=='
,'d+IbP70W+nM9JzilPC9IMptXLkfjfgAgWxfkIvR02r4='
,'g4UGIPQ3ZJjovtc3dmFldw=='
,'c4ACYQUcsX7clj/sf3WTDQ=='
,'1'
,'en'
,'192.168.1.100'
,''
,''--notes
,'0'
,'0'
,'Adresss'
,'3'
,'0'
,'2016-11-21'
,'2016-11-27'
,'5221'
,''
,'Mialn'
,''
,7
,''
ALTER Procedure [dbo].[B_InsertHotelReservation_TB_Reservation_SP]
(
@ReservationID  bigint=null,
@LogUserID bigint=null,
@PinCode varchar(100)=null,
@PartID varchar(100)=null,
@FirmID varchar(100)=null,
@HotelID Varchar(100)=null,
@ReservationDate datetime=null,
@StatusID varchar(100)=null,
@CountryID varchar(100)=null,
@VAT varchar(100)=null,
@SalutationTypeID varchar(100)=null,
@Name varchar(100)=null,
@Surname varchar(100)=null,
@Email varchar(100)=null,
@Phone varchar(100)=null,
@Amount varchar(100)=null,
@GeneralPromotionDiscountPercentage varchar(100)=null,
@PromotionDiscountPercentage varchar(100)=null,
@PayableAmount varchar(100)=null,
@ActualAmount varchar(100)=null,
@CurrencyID varchar(100)=null,
@CreditCardUsed varchar(100)=null,
@CCTypeID varchar(100)=null,
@CCFullName varchar(100)=null,
@CCNo varchar(100)=null,
@CCExpiration varchar(100)=null,
@CCCVC varchar(100)=null,
@ReservationOperationID varchar(100)=null,
@Active varchar(100)=null,
@CultureID varchar(100)=null,
@IPAddress varchar(100)=null,
@CancelDateTime datetime=null,
@CreateDateTime datetime=null,
@CreateUserID varchar(100)=null,
@OpUserID varchar(100)=null,
@UserSessionID varchar(100)=null,
@SpecialNote varchar(Max)=null,
@Address varchar(200)=null,
@ReservationMode varchar(50)=null,
@RoomCountRemaining varchar(50)=null,
@Checkindate varchar(50)=null,
@CheckoutDate varchar(50)=null,
@CheckoutDate1 varchar(50)=null,
@RoomID varchar(50)=null,
@HotelAccommodationTypeID varchar(50)=null,
@GuestFullName varchar(100)=null,
@PeopleCount varchar(50)=null,
@NightCount varchar(50)=null,
@HotelCancelPolicyID varchar(50)=null,
@PricePolicyTypeID varchar(50)=null,
@NonRefundable varchar(50)=null,
@SingleRate varchar(50)=null,
@DoubleRate  varchar(50)=null,
@BedOptionNo varchar(50)=null,
@EstimatedArrivalTime varchar(50)=null,
@TravellerTypeID varchar(50)=null,
@DateID varchar(50)=null,
@RoomPrice varchar(50)=null,
@ComissionRate int=null,
@ComissionAmount numeric(18, 2)=null
)
As
Begin
declare @Culture varchar(50)=null
declare @Currency varchar(50)=null
declare @CheckindateID varchar(50)=null
declare @CheckoutdateID varchar(50)=null

Select @CheckindateID=ID from  TB_Date where  Date=@Checkindate
Select @CheckoutdateID=ID from  TB_Date where  Date=@CheckoutDate

Select @Culture=ID from  BizTbl_Culture where  Code=@CultureID
Select @Currency=ID from  TB_Currency where  Code=@CurrencyID

Insert into TB_HotelReservation  (ReservationID,HotelID,HotelRoomID,HotelAccommodationTypeID,GuestFullName
,PeopleCount,CheckInDate,CheckOutDate,NightCount,HotelCancelPolicyID,PricePolicyTypeID,NonRefundable
,SingleRate,DoubleRate,Amount,PromotionDiscountPercentage,PayableAmount,CurrencyID,BedOptionNo
,EstimatedArrivalTime,TravellerTypeID,CancelDateTime,StatusID,ReservationOperationID,Active
,OpDateTime,OpUserID)
values
(@ReservationID,@HotelID,@RoomID,@HotelAccommodationTypeID,@GuestFullName,@PeopleCount,@Checkindate,@CheckoutDate1
,@NightCount,@HotelCancelPolicyID,@PricePolicyTypeID,@NonRefundable,@SingleRate,@DoubleRate,@Amount,
@PromotionDiscountPercentage,@PayableAmount,@Currency,@BedOptionNo,@EstimatedArrivalTime,
@TravellerTypeID,@CancelDateTime,@StatusID,@ReservationOperationID,@Active,GETDATE(),@OpUserID)

select  ID from TB_HotelReservation where ReservationID=@ReservationID and HotelID=@HotelID 
and HotelRoomID=@RoomID and PeopleCount=@PeopleCount and PricePolicyTypeID =@PricePolicyTypeID

Update TB_HotelAvailability set RoomCount=@RoomCountRemaining where DateID between @CheckindateID and @CheckoutdateID  and HotelRoomID=@RoomID

End
