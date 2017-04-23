begin tran

exec [B_InsertHotelReservation_TB_Reservation_SP]
@ReservationID=1030044
,@LogUserID=null
,@PinCode =  'TND84BHV'
,@PartID=1
,@FirmID='100002'
,@HotelID= '100002'
,@StatusID='1'
,@CountryID='89'
,@SalutationTypeID='1'
,@Name='abhishek'
,@Surname='bha'
,@Email='bhalaniabhishek@gmail.com'
,@Phone='99899898'
,@Amount='1055'
,@PayableAmount ='1055'
,@ActualAmount='True'
,@CurrencyID='EUR'
,@CreditCardUsed='100002'
,@CCTypeID='0' --cre
,@CCFullName='UBB2td6mPgq0Dps8KBPByg=='
,@CCNo='d+IbP70W+nM9JzilPC9IMptXLkfjfgAgWxfkIvR02r4='
,@CCExpiration='g4UGIPQ3ZJjovtc3dmFldw=='
,@CCCVC='c4ACYQUcsX7clj/sf3WTDQ=='
,@Active='1'
,@CultureID='en'
,@IPAddress='192.168.1.100'
,@OpUserID=null
,@SpecialNote=''--notes
,@GeneralPromotionDiscountPercentage='0'
,@PromotionDiscountPercentage='0'
,@Address='Adresss'
,@ReservationMode='3'
,@RoomCountRemaining='0'
,@Checkindate='2016-11-21'
,@CheckoutDate='2016-11-27'
,@RoomID='5221'
,@HotelAccommodationTypeID='' --req
,@GuestFullName='Mialn' 
,@PeopleCount='' --req
,@NightCount=7
,@PricePolicyTypeID='' --req
,@SingleRate=''--req
,@DoubleRate=''--req
,@BedOptionNo='1'
,@TravellerTypeID='1'
,@EstimatedArrivalTime='00:00'
,@CheckoutDate1='2016-11-28'
,@RoomPrice='1055'
,@NonRefundable='0'
,@ReservationOperationID='1'
,@HotelCancelPolicyID='' --req
,@ComissionRate=15.0
,@ComissionAmount=158.25
rollback


--DailyRoomPrices: 20161121-155;20161122-155;20161123-155;20161124-155;20161125-145;20161126-145;20161127-145

select * from TB_HotelReservation

