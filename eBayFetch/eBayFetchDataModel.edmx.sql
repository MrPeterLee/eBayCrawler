
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/25/2014 23:58:44
-- Generated from EDMX file: D:\Profile\Desktop\2011 - C# eBay Fetch\eBayFetch\eBayFetchDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [eBayFetchSQL];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[S1SearchTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[S1SearchTable];
GO
IF OBJECT_ID(N'[dbo].[S2ItemInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[S2ItemInfo];
GO
IF OBJECT_ID(N'[dbo].[S3UserDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[S3UserDetail];
GO
IF OBJECT_ID(N'[dbo].[S4PanelData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[S4PanelData];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'S1SearchTable'
CREATE TABLE [dbo].[S1SearchTable] (
    [SearchID] int IDENTITY(1,1) NOT NULL,
    [ItemID] nvarchar(max)  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [FetchTime] datetime  NOT NULL,
    [EndTimeDum] int  NULL,
    [IsInPanelData] int  NULL,
    [Category] nvarchar(max)  NULL,
    [CategoryID] nvarchar(max)  NULL
);
GO

-- Creating table 'S2ItemInfo'
CREATE TABLE [dbo].[S2ItemInfo] (
    [SearchID] int IDENTITY(1,1) NOT NULL,
    [ItemID] nvarchar(max)  NOT NULL,
    [SellerID] nvarchar(max)  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [FetchTime] datetime  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Price] nvarchar(max)  NOT NULL,
    [PrivateListing] nvarchar(max)  NOT NULL,
    [BuyItNowPrice] nvarchar(max)  NULL,
    [BidCount] nvarchar(max)  NOT NULL,
    [Quantity] nvarchar(max)  NOT NULL,
    [QuantitySold] nvarchar(max)  NOT NULL,
    [PrimaryCategory] nvarchar(max)  NOT NULL,
    [PrimaryCategoryID] nvarchar(max)  NOT NULL,
    [IsInPanelData] int  NULL
);
GO

-- Creating table 'S3UserDetail'
CREATE TABLE [dbo].[S3UserDetail] (
    [TableIndex] int IDENTITY(1,1) NOT NULL,
    [UserID] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NULL,
    [FeedBackScore] nvarchar(max)  NULL,
    [SellerRegDate] datetime  NULL,
    [SellerLevel] nvarchar(max)  NULL,
    [Site] nvarchar(max)  NULL,
    [SellerStar] nvarchar(max)  NULL,
    [SellerStore] nvarchar(max)  NULL,
    [UserIsNewReg] nvarchar(max)  NULL,
    [SellerVerified] nvarchar(max)  NULL,
    [SellerIDChanged] nvarchar(max)  NULL,
    [MotorsDealer] nvarchar(max)  NULL
);
GO

-- Creating table 'S4PanelData'
CREATE TABLE [dbo].[S4PanelData] (
    [SearchID] int IDENTITY(1,1) NOT NULL,
    [ItemID] varchar(max)  NOT NULL,
    [SellerID] varchar(50)  NULL,
    [Title] varchar(max)  NULL,
    [FetchTime] datetime  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [Price] varchar(max)  NULL,
    [PrivateListing] varchar(max)  NULL,
    [BuyItNowPrice] varchar(max)  NULL,
    [BidCount] varchar(max)  NULL,
    [Quantity] varchar(max)  NULL,
    [QuantitySold] varchar(max)  NULL,
    [PrimaryCategory] varchar(max)  NULL,
    [PrimaryCategoryID] varchar(max)  NULL,
    [UserInfoFetched] int  NULL,
    [Email] varchar(max)  NULL,
    [FeedBackScore] varchar(max)  NULL,
    [SellerRegDate] datetime  NULL,
    [SellerLevel] varchar(max)  NULL,
    [Site] varchar(max)  NULL,
    [SellerStar] varchar(max)  NULL,
    [SellerStore] varchar(max)  NULL,
    [UserIsNewReg] varchar(max)  NULL,
    [SellerVerified] varchar(max)  NULL,
    [SellerIDChanged] varchar(max)  NULL,
    [Ones] int  NULL,
    [MotorsDealer] nvarchar(max)  NULL,
    [SecondaryCategoryCategoryName] nvarchar(max)  NULL,
    [SecondaryCategoryCategoryID] nvarchar(max)  NULL,
    [SellingStatusHighBidderUserID] nvarchar(max)  NULL,
    [BestOfferDetailsBestOfferCount] nvarchar(max)  NULL,
    [BestOfferDetailsBestOfferEnabled] nvarchar(max)  NULL,
    [ProductListingDetailsProductID] nvarchar(max)  NULL,
    [PictureDetailsPictureURL] nvarchar(max)  NULL,
    [PayPalEmailAddress] nvarchar(max)  NULL,
    [ApplicationData] nvarchar(max)  NULL,
    [PicNumber] int  NULL,
    [BuyerProtection] nvarchar(max)  NULL,
    [BuyerResponsibleForShipping] nvarchar(max)  NULL,
    [BuyItNowPricecurrencyID] nvarchar(max)  NULL,
    [BuyItNowPriceValue] nvarchar(max)  NULL,
    [BuyerGuaranteePricecurrencyID] nvarchar(max)  NULL,
    [BuyerGuaranteePriceValue] nvarchar(max)  NULL,
    [CategoryBasedAttributesPrefill] nvarchar(max)  NULL,
    [CategoryMappingAllowed] nvarchar(max)  NULL,
    [CharityCharityID] nvarchar(max)  NULL,
    [ClassifiedAdPayPerLeadFeecurrencyID] nvarchar(max)  NULL,
    [ClassifiedAdPayPerLeadFeeValue] nvarchar(max)  NULL,
    [ConditionDisplayName] nvarchar(max)  NULL,
    [ConditionID] nvarchar(max)  NULL,
    [Country] nvarchar(max)  NULL,
    [CrossBorderTrade] nvarchar(max)  NULL,
    [CrossPromotionItemID] nvarchar(max)  NULL,
    [Currency] nvarchar(max)  NULL,
    [DescriptionReviseMode] nvarchar(max)  NULL,
    [DisableBuyerRequirements] nvarchar(max)  NULL,
    [DispatchTimeMax] nvarchar(max)  NULL,
    [DistanceDistanceUnit] nvarchar(max)  NULL,
    [eBayNotes] nvarchar(max)  NULL,
    [FreeAddedCategoryAutoPayEnabled] nvarchar(max)  NULL,
    [FreeAddedCategoryB2BVATEnabled] nvarchar(max)  NULL,
    [FreeAddedCategoryBestOfferEnabled] nvarchar(max)  NULL,
    [FreeAddedCategoryCatalogEnabled] nvarchar(max)  NULL,
    [FreeAddedCategoryCategoryID] nvarchar(max)  NULL,
    [FreeAddedCategoryCategoryLevel] nvarchar(max)  NULL,
    [FreeAddedCategoryCategoryName] nvarchar(max)  NULL,
    [FreeAddedCategoryCategoryParentID] nvarchar(max)  NULL,
    [FreeAddedCategoryCategoryParentName] nvarchar(max)  NULL,
    [FreeAddedCategoryCharacteristicsSets] nvarchar(max)  NULL,
    [FreeAddedCategoryExpired] nvarchar(max)  NULL,
    [FreeAddedCategoryIntlAutosFixedCat] nvarchar(max)  NULL,
    [FreeAddedCategoryKeywords] nvarchar(max)  NULL,
    [FreeAddedCategoryLeafCategory] nvarchar(max)  NULL,
    [FreeAddedCategoryLSD] nvarchar(max)  NULL,
    [FreeAddedCategoryNumOfItems] nvarchar(max)  NULL,
    [FreeAddedCategoryORPA] nvarchar(max)  NULL,
    [FreeAddedCategoryORRA] nvarchar(max)  NULL,
    [FreeAddedCategoryProductFinderIDs] nvarchar(max)  NULL,
    [FreeAddedCategoryProductSearchPageAvailable] nvarchar(max)  NULL,
    [FreeAddedCategorySellerGuaranteeEligible] nvarchar(max)  NULL,
    [FreeAddedCategoryVirtual] nvarchar(max)  NULL,
    [GetItFast] nvarchar(max)  NULL,
    [GiftIcon] nvarchar(max)  NULL,
    [GiftServices] nvarchar(max)  NULL,
    [GroupCategoryID] nvarchar(max)  NULL,
    [HitCount] nvarchar(max)  NULL,
    [HitCounter] nvarchar(max)  NULL,
    [IntegratedMerchantCreditCardEnabled] nvarchar(max)  NULL,
    [InventoryTrackingMethod] nvarchar(max)  NULL,
    [ItemCompatibilityListCompatibility] nvarchar(max)  NULL,
    [ItemCompatibilityCount] nvarchar(max)  NULL,
    [ItemPolicyViolationPolicyID] nvarchar(max)  NULL,
    [ItemPolicyViolationPolicyText] nvarchar(max)  NULL,
    [ItemSpecifics] nvarchar(max)  NULL,
    [LeadCount] nvarchar(max)  NULL,
    [LimitedWarrantyEligible] nvarchar(max)  NULL,
    [ListingCheckoutRedirectPreferenceProStoresStoreName] nvarchar(max)  NULL,
    [ListingCheckoutRedirectPreferenceSellerThirdPartyUsername] nvarchar(max)  NULL,
    [ListingDesignerLayoutID] nvarchar(max)  NULL,
    [ListingDesignerThemeID] nvarchar(max)  NULL,
    [ListingDesignerOptimalPictureSize] nvarchar(max)  NULL,
    [ListingDetailsAdult] nvarchar(max)  NULL,
    [ListingDetailsBestOfferAutoAcceptPriceValue] nvarchar(max)  NULL,
    [ListingDetailsBindingAuction] nvarchar(max)  NULL,
    [ListingDetailsBuyItNowAvailable] nvarchar(max)  NULL,
    [ListingDetailsCheckoutEnabled] nvarchar(max)  NULL,
    [ListingDetailsConvertedBuyItNowPriceValue] nvarchar(max)  NULL,
    [ListingDetailsConvertedReservePriceValue] nvarchar(max)  NULL,
    [ListingDetailsConvertedStartPriceValue] nvarchar(max)  NULL,
    [ListingDetailsEndTime] nvarchar(max)  NULL,
    [ListingDetailsHasPublicMessages] nvarchar(max)  NULL,
    [ListingDetailsHasReservePrice] nvarchar(max)  NULL,
    [ListingDetailsHasUnansweredQuestions] nvarchar(max)  NULL,
    [ListingDetailsLocalListingDistance] nvarchar(max)  NULL,
    [ListingDetailsMinimumBestOfferMessage] nvarchar(max)  NULL,
    [ListingDetailsMinimumBestOfferPriceValue] nvarchar(max)  NULL,
    [ListingDetailsPayPerLeadEnabled] nvarchar(max)  NULL,
    [ListingDetailsRelistedItemID] nvarchar(max)  NULL,
    [ListingDetailsSecondChanceOriginalItemID] nvarchar(max)  NULL,
    [ListingDetailsSellerBusinessType] nvarchar(max)  NULL,
    [ListingDetailsStartTime] nvarchar(max)  NULL,
    [ListingDetailsTCROriginalItemID] nvarchar(max)  NULL,
    [ListingDetailsViewItemURL] nvarchar(max)  NULL,
    [ListingDuration] nvarchar(max)  NULL,
    [ListingEnhancement] nvarchar(max)  NULL,
    [ListingSubtype2] nvarchar(max)  NULL,
    [ListingType] nvarchar(max)  NULL,
    [LocalListing] nvarchar(max)  NULL,
    [LocationDefaulted] nvarchar(max)  NULL,
    [LotSize] nvarchar(max)  NULL,
    [MechanicalCheckAccepted] nvarchar(max)  NULL,
    [MotorsGermanySearchable] nvarchar(max)  NULL,
    [NewLeadCount] nvarchar(max)  NULL,
    [PartnerCode] nvarchar(max)  NULL,
    [PartnerName] nvarchar(max)  NULL,
    [PaymentAllowedSite] nvarchar(max)  NULL,
    [PaymentDetailsDaysToFullPayment] nvarchar(max)  NULL,
    [PaymentDetailsHoursToDeposit] nvarchar(max)  NULL,
    [PaymentMethods] nvarchar(max)  NULL,
    [PostalCode] nvarchar(max)  NULL,
    [PrivateNotes] nvarchar(max)  NULL,
    [ProductListingDetails] nvarchar(max)  NULL,
    [ProxyItem] nvarchar(max)  NULL,
    [QuantityAvailable] nvarchar(max)  NULL,
    [QuantitySpecified] nvarchar(max)  NULL,
    [QuestionCount] nvarchar(max)  NULL,
    [RegionID] nvarchar(max)  NULL,
    [Relisted] nvarchar(max)  NULL,
    [RelistLink] nvarchar(max)  NULL,
    [ReservePriceValue] nvarchar(max)  NULL,
    [ReservePricecurrencyID] nvarchar(max)  NULL,
    [ReturnPolicyRefund] nvarchar(max)  NULL,
    [ReturnPolicyRefundOption] nvarchar(max)  NULL,
    [ReturnPolicyReturnsAccepted] nvarchar(max)  NULL,
    [ReturnPolicyReturnsWithin] nvarchar(max)  NULL,
    [ReturnPolicyShippingCostPaidBy] nvarchar(max)  NULL,
    [ReturnPolicyWarrantyDuration] nvarchar(max)  NULL,
    [ReturnPolicyWarrantyOffered] nvarchar(max)  NULL,
    [ReturnPolicyWarrantyType] nvarchar(max)  NULL,
    [ScheduleTime] nvarchar(max)  NULL,
    [SearchDetailsPicture] nvarchar(max)  NULL,
    [SearchDetailsRecentListing] nvarchar(max)  NULL,
    [SearchDetailsBuyItNowEnabled] nvarchar(max)  NULL,
    [SellerAboutMePage] nvarchar(max)  NULL,
    [SellerBillingEmail] nvarchar(max)  NULL,
    [SellerCharityAffiliationsCharityID] nvarchar(max)  NULL,
    [SellereBayGoodStanding] nvarchar(max)  NULL,
    [SellereBayWikiReadOnly] nvarchar(max)  NULL,
    [SellerEIASToken] nvarchar(max)  NULL,
    [SellerEmail] nvarchar(max)  NULL,
    [SellerEnterpriseSeller] nvarchar(max)  NULL,
    [SellerFeedbackPrivate] nvarchar(max)  NULL,
    [SellerFeedbackRatingStar] nvarchar(max)  NULL,
    [SellerFeedbackScore] nvarchar(max)  NULL,
    [SellerIDVerified] nvarchar(max)  NULL,
    [SellerMotorsDealer] nvarchar(max)  NULL,
    [SellerNewUser] nvarchar(max)  NULL,
    [SellerPayPalAccountLevel] nvarchar(max)  NULL,
    [SellerPayPalAccountStatus] nvarchar(max)  NULL,
    [SellerPayPalAccountType] nvarchar(max)  NULL,
    [SellerPositiveFeedbackPercent] nvarchar(max)  NULL,
    [SellerQualifiesForSelling] nvarchar(max)  NULL,
    [SellerRegistrationAddress] nvarchar(max)  NULL,
    [SellerRegistrationDate] nvarchar(max)  NULL,
    [SellerSellerPaymentMethod] nvarchar(max)  NULL,
    [SellerTUVLevel] nvarchar(max)  NULL,
    [SellerUniqueNegativeFeedbackCount] nvarchar(max)  NULL,
    [SellerUniqueNeutralFeedbackCount] nvarchar(max)  NULL,
    [SellerUniquePositiveFeedbackCount] nvarchar(max)  NULL,
    [SellerUserAnonymized] nvarchar(max)  NULL,
    [SellerUserIDChanged] nvarchar(max)  NULL,
    [SellerUserSubscription] nvarchar(max)  NULL,
    [SellerVATID] nvarchar(max)  NULL,
    [SellerVATStatus] nvarchar(max)  NULL,
    [SellerContactDetailsAddressID] nvarchar(max)  NULL,
    [SellerContactDetailsAddressOwner] nvarchar(max)  NULL,
    [SellerContactDetailsAddressRecordType] nvarchar(max)  NULL,
    [SellerContactDetailsAddressStatus] nvarchar(max)  NULL,
    [SellerContactDetailsAddressUsage] nvarchar(max)  NULL,
    [SellerContactDetailsCityName] nvarchar(max)  NULL,
    [SellerContactDetailsCompanyName] nvarchar(max)  NULL,
    [SellerContactDetailsCountry] nvarchar(max)  NULL,
    [SellerContactDetailsCountryName] nvarchar(max)  NULL,
    [SellerContactDetailsCounty] nvarchar(max)  NULL,
    [SellerContactDetailsExternalAddressID] nvarchar(max)  NULL,
    [SellerContactDetailsFirstName] nvarchar(max)  NULL,
    [SellerContactDetailsLastName] nvarchar(max)  NULL,
    [SellerContactDetailsName] nvarchar(max)  NULL,
    [SellerContactDetailsInternationalName] nvarchar(max)  NULL,
    [SellerContactDetailsInternationalStateAndCity] nvarchar(max)  NULL,
    [SellerContactDetailsInternationalStreet] nvarchar(max)  NULL,
    [SellerContactDetailsPhone] nvarchar(max)  NULL,
    [SellerContactDetailsPhoneCountryCode] nvarchar(max)  NULL,
    [SellerContactDetailsPhoneCountryPrefix] nvarchar(max)  NULL,
    [SellerContactDetailsPhoneLocalNumber] nvarchar(max)  NULL,
    [SellerContactDetailsPostalCode] nvarchar(max)  NULL,
    [SellerContactDetailsStateOrProvince] nvarchar(max)  NULL,
    [SellerContactDetailsStreet] nvarchar(max)  NULL,
    [SellerContactDetailsStreet1] nvarchar(max)  NULL,
    [SellerContactDetailsStreet2] nvarchar(max)  NULL,
    [SellerInventoryID] nvarchar(max)  NULL,
    [SellerVacationNote] nvarchar(max)  NULL,
    [SellingStatusAdminEnded] nvarchar(max)  NULL,
    [SellingStatusBidCount] nvarchar(max)  NULL,
    [SellingStatusBidderCount] nvarchar(max)  NULL,
    [SellingStatusBidIncrementValue] nvarchar(max)  NULL,
    [SellingStatusBidIncrementcurrencyID] nvarchar(max)  NULL,
    [SellingStatusConvertedCurrentPriceValue] nvarchar(max)  NULL,
    [SellingStatusCurrentPriceValue] nvarchar(max)  NULL,
    [SellingStatusFinalValueFeeValue] nvarchar(max)  NULL,
    [SellingStatusLeadCount] nvarchar(max)  NULL,
    [SellingStatusListingStatus] nvarchar(max)  NULL,
    [SellingStatusMinimumToBid] nvarchar(max)  NULL,
    [SellingStatusPromotionalSaleDetailsEndTime] nvarchar(max)  NULL,
    [SellingStatusQuantitySold] nvarchar(max)  NULL,
    [SellingStatusReserveMet] nvarchar(max)  NULL,
    [SellingStatusSecondChanceEligible] nvarchar(max)  NULL,
    [ShippingDetailsAllowPaymentEdit] nvarchar(max)  NULL,
    [ShippingDetailsApplyShippingDiscount] nvarchar(max)  NULL,
    [ShippingDetailsCalculatedShippingDiscountDiscountName] nvarchar(max)  NULL,
    [ShippingDetailsCalculatedShippingDiscountDiscountProfile] nvarchar(max)  NULL,
    [ShippingDetailsInsuranceFeeValue] nvarchar(max)  NULL,
    [ShipToLocations] nvarchar(max)  NULL,
    [SKU] nvarchar(max)  NULL,
    [SkypeEnabled] nvarchar(max)  NULL,
    [SkypeID] nvarchar(max)  NULL,
    [StartPriceValue] nvarchar(max)  NULL,
    [StartPricecurrencyID] nvarchar(max)  NULL,
    [StorefrontStoreName] nvarchar(max)  NULL,
    [StorefrontStoreCategoryID] nvarchar(max)  NULL,
    [StorefrontStoreCategory2ID] nvarchar(max)  NULL,
    [SubTitle] nvarchar(max)  NULL,
    [TaxCategory] nvarchar(max)  NULL,
    [ThirdPartyCheckout] nvarchar(max)  NULL,
    [ThirdPartyCheckoutIntegration] nvarchar(max)  NULL,
    [TimeLeft] nvarchar(max)  NULL,
    [TotalQuestionCount] nvarchar(max)  NULL,
    [UpdateReturnPolicy] nvarchar(max)  NULL,
    [UpdateSellerInfo] nvarchar(max)  NULL,
    [UseTaxTable] nvarchar(max)  NULL,
    [UUID] nvarchar(max)  NULL,
    [Variations] nvarchar(max)  NULL,
    [VATDetailsVATID] nvarchar(max)  NULL,
    [WatchCount] nvarchar(max)  NULL,
    [WatchCountSpecified] nvarchar(max)  NULL,
    [ApplyBuyerProtectionBuyerProtectionSource] nvarchar(max)  NULL,
    [ApplyBuyerProtectionBuyerProtectionStatus] nvarchar(max)  NULL,
    [ApplyShippingDiscount] nvarchar(max)  NULL,
    [AutoPay] nvarchar(max)  NULL,
    [BestOfferDetailsBestOffercurrencyID] nvarchar(max)  NULL,
    [BestOfferDetailsBestOfferValue] nvarchar(max)  NULL,
    [BestOfferDetailsBestOfferStatus] nvarchar(max)  NULL,
    [BestOfferDetailsBestOfferType] nvarchar(max)  NULL,
    [BestOfferEnabled] nvarchar(max)  NULL,
    [BiddingDetailsBidAssistant] nvarchar(max)  NULL,
    [BiddingDetailsConvertedMaxBidcurrencyID] nvarchar(max)  NULL,
    [BiddingDetailsConvertedMaxBidValue] nvarchar(max)  NULL,
    [BiddingDetailsMaxBidcurrencyID] nvarchar(max)  NULL,
    [BiddingDetailsMaxBidValue] nvarchar(max)  NULL,
    [BiddingDetailsQuantityBid] nvarchar(max)  NULL,
    [BiddingDetailsQuantityWon] nvarchar(max)  NULL,
    [BiddingDetailsWinning] nvarchar(max)  NULL,
    [BidGroupItem] nvarchar(max)  NULL,
    [ItemSpecific1] nvarchar(max)  NULL,
    [ItemSpecific2] nvarchar(max)  NULL,
    [ItemSpecific3] nvarchar(max)  NULL,
    [ItemSpecific4] nvarchar(max)  NULL,
    [ItemSpecific5] nvarchar(max)  NULL,
    [ItemSpecific6] nvarchar(max)  NULL,
    [ItemSpecific7] nvarchar(max)  NULL,
    [ItemSpecific8] nvarchar(max)  NULL,
    [ItemSpecific9] nvarchar(max)  NULL,
    [ItemSpecific10] nvarchar(max)  NULL,
    [ItemSpecific11] nvarchar(max)  NULL,
    [ItemSpecific12] nvarchar(max)  NULL,
    [ItemSpecific13] nvarchar(max)  NULL,
    [ItemSpecific14] nvarchar(max)  NULL,
    [ItemSpecific15] nvarchar(max)  NULL,
    [ItemSpecific16] nvarchar(max)  NULL,
    [ItemSpecific17] nvarchar(max)  NULL,
    [ItemSpecific18] nvarchar(max)  NULL,
    [ItemSpecific19] nvarchar(max)  NULL,
    [ItemSpecific20] nvarchar(max)  NULL,
    [ItemSpecific21] nvarchar(max)  NULL,
    [ItemSpecific22] nvarchar(max)  NULL,
    [ItemSpecific23] nvarchar(max)  NULL,
    [ItemSpecific24] nvarchar(max)  NULL,
    [ItemSpecific25] nvarchar(max)  NULL,
    [ItemSpecific26] nvarchar(max)  NULL,
    [ItemSpecific27] nvarchar(max)  NULL,
    [ItemSpecific28] nvarchar(max)  NULL,
    [ItemSpecific29] nvarchar(max)  NULL,
    [ItemSpecific30] nvarchar(max)  NULL,
    [ItemSpecific31] nvarchar(max)  NULL,
    [ItemSpecific32] nvarchar(max)  NULL,
    [ItemSpecific33] nvarchar(max)  NULL,
    [ItemSpecific34] nvarchar(max)  NULL,
    [ItemSpecific35] nvarchar(max)  NULL,
    [ItemSpecific36] nvarchar(max)  NULL,
    [ItemSpecific37] nvarchar(max)  NULL,
    [ItemSpecific38] nvarchar(max)  NULL,
    [ItemSpecific39] nvarchar(max)  NULL,
    [ItemSpecific40] nvarchar(max)  NULL,
    [ItemSpecific41] nvarchar(max)  NULL,
    [ItemSpecific42] nvarchar(max)  NULL,
    [ItemSpecific43] nvarchar(max)  NULL,
    [ItemSpecific44] nvarchar(max)  NULL,
    [ItemSpecific45] nvarchar(max)  NULL,
    [ItemSpecific46] nvarchar(max)  NULL,
    [ItemSpecific47] nvarchar(max)  NULL,
    [ItemSpecific48] nvarchar(max)  NULL,
    [ItemSpecific49] nvarchar(max)  NULL,
    [ItemSpecific50] nvarchar(max)  NULL,
    [ItemSpecific51] nvarchar(max)  NULL,
    [ItemSpecific52] nvarchar(max)  NULL,
    [ItemSpecific53] nvarchar(max)  NULL,
    [ItemSpecific54] nvarchar(max)  NULL,
    [ItemSpecific55] nvarchar(max)  NULL,
    [ItemSpecific56] nvarchar(max)  NULL,
    [ItemSpecific57] nvarchar(max)  NULL,
    [ItemSpecific58] nvarchar(max)  NULL,
    [ItemSpecific59] nvarchar(max)  NULL,
    [ItemSpecific60] nvarchar(max)  NULL,
    [ItemSpecific61] nvarchar(max)  NULL,
    [ItemSpecific62] nvarchar(max)  NULL,
    [ItemSpecific63] nvarchar(max)  NULL,
    [ItemSpecific64] nvarchar(max)  NULL,
    [ItemSpecific65] nvarchar(max)  NULL,
    [ItemSpecific66] nvarchar(max)  NULL,
    [ItemSpecific67] nvarchar(max)  NULL,
    [ItemSpecific68] nvarchar(max)  NULL,
    [ItemSpecific69] nvarchar(max)  NULL,
    [ItemSpecific70] nvarchar(max)  NULL,
    [ItemSpecific71] nvarchar(max)  NULL,
    [ItemSpecific72] nvarchar(max)  NULL,
    [ItemSpecific73] nvarchar(max)  NULL,
    [ItemSpecific74] nvarchar(max)  NULL,
    [ItemSpecific75] nvarchar(max)  NULL,
    [ItemSpecific76] nvarchar(max)  NULL,
    [ItemSpecific77] nvarchar(max)  NULL,
    [ItemSpecific78] nvarchar(max)  NULL,
    [ItemSpecific79] nvarchar(max)  NULL,
    [ItemSpecific80] nvarchar(max)  NULL,
    [ItemSpecific81] nvarchar(max)  NULL,
    [ItemSpecific82] nvarchar(max)  NULL,
    [ItemSpecific83] nvarchar(max)  NULL,
    [ItemSpecific84] nvarchar(max)  NULL,
    [ItemSpecific85] nvarchar(max)  NULL,
    [ItemSpecific86] nvarchar(max)  NULL,
    [ItemSpecific87] nvarchar(max)  NULL,
    [ItemSpecific88] nvarchar(max)  NULL,
    [ItemSpecific89] nvarchar(max)  NULL,
    [ItemSpecific90] nvarchar(max)  NULL,
    [ItemSpecific91] nvarchar(max)  NULL,
    [ItemSpecific92] nvarchar(max)  NULL,
    [ItemSpecific93] nvarchar(max)  NULL,
    [ItemSpecific94] nvarchar(max)  NULL,
    [ItemSpecific95] nvarchar(max)  NULL,
    [ItemSpecific96] nvarchar(max)  NULL,
    [ItemSpecific97] nvarchar(max)  NULL,
    [ItemSpecific98] nvarchar(max)  NULL,
    [ItemSpecific99] nvarchar(max)  NULL,
    [ItemSpecific100] nvarchar(max)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [SearchID] in table 'S1SearchTable'
ALTER TABLE [dbo].[S1SearchTable]
ADD CONSTRAINT [PK_S1SearchTable]
    PRIMARY KEY CLUSTERED ([SearchID] ASC);
GO

-- Creating primary key on [SearchID] in table 'S2ItemInfo'
ALTER TABLE [dbo].[S2ItemInfo]
ADD CONSTRAINT [PK_S2ItemInfo]
    PRIMARY KEY CLUSTERED ([SearchID] ASC);
GO

-- Creating primary key on [TableIndex] in table 'S3UserDetail'
ALTER TABLE [dbo].[S3UserDetail]
ADD CONSTRAINT [PK_S3UserDetail]
    PRIMARY KEY CLUSTERED ([TableIndex] ASC);
GO

-- Creating primary key on [SearchID] in table 'S4PanelData'
ALTER TABLE [dbo].[S4PanelData]
ADD CONSTRAINT [PK_S4PanelData]
    PRIMARY KEY CLUSTERED ([SearchID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------