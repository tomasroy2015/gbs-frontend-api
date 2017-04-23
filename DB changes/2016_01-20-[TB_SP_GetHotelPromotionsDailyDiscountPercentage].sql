USE [Gbshotels]
GO
/****** Object:  StoredProcedure [dbo].[TB_SP_GetHotelPromotionsDailyDiscountPercentage]    Script Date: 20-01-2017 13:29:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[TB_SP_GetHotelPromotionsDailyDiscountPercentage]
(
	@Culture nchar(3),
	@OrderBy nvarchar(100) = NULL,
	@HotelPromotionIDs varchar(MAX) = NULL,
	@HotelIDs varchar(MAX) = NULL,
	@HotelRoomID int = NULL,
	@PricePolicyTypeID int = NULL,
	@PromotionID bigint = NULL,
	@PromotionType varchar(50) = NULL,
	@Date datetime = NULL,
	@AccommodationStartDate datetime = NULL,
	@AccommodationEndDate datetime = NULL,
	@AccommodationDate datetime = NULL,
	@DayCount int = NULL,
	@SecretDeal bit = NULL,
	@Genius bit = NULL,
	@UserID bigint = NULL,
	@Active bit = NULL
)
AS

DECLARE @SQL NVARCHAR(4000)

SET DATEFIRST 1;

IF @DayCount IS NULL
	SET @DayCount = DATEDIFF(DAY, @AccommodationStartDate, @AccommodationEndDate) + 1

 ;WITH CTE AS (

SELECT 
HP.ID AS HotelPromotionID,  
HP.HotelID,
HP.PromotionID,
P.Type AS PromotionType,
P.Sort AS PromotionSort,
HP.StartDate,
HP.EndDate,
HP.AccommodationStartDate,
HP.AccommodationEndDate,
HP.DayCount,
TPP.ID,
D.[Date],
HPR.HotelRoomID,
TPP.ID AS PricePolicyTypeID,
(SELECT ISNULL([dbo].[TBF_GetHotelPromotionDiscountPercentage] (HP.HotelID, HPR.HotelRoomID, TPP.ID, HP.PromotionID, NULL, @Date, @AccommodationStartDate, @AccommodationEndDate, D.[Date], @DayCount, @SecretDeal, @Genius, @UserID, HP.Active), 0)) AS DiscountPercentage,
HP.Region,
--HP.DiscountPercentage,
HP.DayID,
HP.PricePolicyID,
HP.LastMinuteMargin,
HP.EarlyBookerMargin,
HP.BookingDate,
HP.SecretDeal,
HP.ValidForAllRoomTypes,
HP.Active,
HP.CreateDateTime,
HP.CreateUserID,
HP.OpDateTime,
HP.OpUserID
 , RN = ROW_NUMBER()OVER(PARTITION BY HotelPromotionID,HotelRoomID ORDER BY HotelPromotionID,HotelRoomID)

FROM dbo.TB_HotelPromotion HP (NOLOCK) 
INNER JOIN dbo.TB_Promotion P (NOLOCK) ON P.ID = HP.PromotionID
INNER JOIN dbo.TB_HotelPromotionRoom (NOLOCK) HPR ON HPR.HotelPromotionID = HP.ID
INNER JOIN dbo.TB_Date (NOLOCK) D ON D.[Date] BETWEEN @AccommodationStartDate AND @AccommodationEndDate
INNER JOIN dbo.TB_TypePricePolicy TPP (NOLOCK) ON TPP.Active = 1
WHERE 
HP.HasDiscount = 1 AND
(@HotelIDs IS NULL OR HP.HotelID IN (SELECT Items FROM dbo.Split(@HotelIDs, ','))) AND
(@HotelPromotionIDs IS NULL OR HP.ID IN (SELECT Items FROM dbo.Split(@HotelPromotionIDs, ','))) AND
(@HotelRoomID IS NULL OR @HotelRoomID IN(SELECT HPR.HotelRoomID FROM dbo.TB_HotelPromotionRoom (NOLOCK) HPR WHERE HPR.HotelPromotionID = HP.ID)) AND
(@PricePolicyTypeID IS NULL OR @PricePolicyTypeID IN (SELECT Items FROM dbo.Split(HP.PricePolicyID, ';'))) AND
(@PromotionID IS NULL OR HP.PromotionID = @PromotionID) AND
(@PromotionType IS NULL OR P.[Type] = @PromotionType) AND
(@Date IS NULL OR @Date BETWEEN HP.StartDate AND HP.EndDate) AND
((@AccommodationStartDate IS NULL AND @AccommodationEndDate IS NULL) OR @AccommodationStartDate BETWEEN HP.AccommodationStartDate AND HP.AccommodationEndDate OR @AccommodationEndDate BETWEEN HP.AccommodationStartDate AND HP.AccommodationEndDate) AND
(@AccommodationDate IS NULL OR ((@AccommodationDate BETWEEN HP.AccommodationStartDate AND HP.AccommodationEndDate) AND (DATEPART(dw, @AccommodationDate) IN (SELECT Items FROM dbo.Split(HP.DayID, ';'))))) AND
(@DayCount IS NULL OR HP.DayCount <= @DayCount) AND
(HP.LastMinuteMargin IS NULL OR @Date IS NULL OR DATEADD(HOUR, HP.LastMinuteMargin, @Date) >= @AccommodationStartDate) AND 
(HP.EarlyBookerMargin IS NULL OR @Date IS NULL OR DATEADD(DAY, HP.EarlyBookerMargin, @Date) >= @AccommodationStartDate) AND 
(HP.BookingDate IS NULL OR CONVERT(DATE, getdate()) = HP.BookingDate) AND
(@SecretDeal IS NULL OR HP.SecretDeal = @SecretDeal) AND
(@Genius IS NULL OR P.[Type] <> 'Genius' OR (P.[Type] = 'Genius' AND @Genius = 1)) AND
(@Active IS NULL OR HP.Active = @Active)

 )
 SELECT * FROM CTE
 where RN=1
GO


--[TB_SP_GetHotelPromotionsDailyDiscountPercentage]
--@Culture='en',
--@OrderBy='ValidForAllRoomTypes DESC, PromotionSort',
--@HotelIDs='100002',
--@AccommodationStartDate='2017-01-20',
--@AccommodationEndDate='2017-01-26'