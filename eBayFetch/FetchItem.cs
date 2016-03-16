using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using System.Net;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using eBay.Service.Util;
using Samples.Helper;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Data.Objects;




namespace eBayFetch
{

    public partial class MainWindow 
    {
        private void GetItemInfoFn( string ItemString )
        {
            try
            {
                Context.CallRetry = GetCallRetry();

                GetItemCall apicall = new GetItemCall(GetContext());
                apicall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
                fetchedItem = apicall.GetItem(ItemString);

                Log("Now updating ItemID = " + fetchedItem.ItemID.ToString() + ", SellerID = " + fetchedItem.Seller.UserID.ToString() + ", Title = " + fetchedItem.Title.ToString());

                var newSearch = new S2ItemInfo
                {
                    
                    ItemID = fetchedItem.ItemID.ToString(),
                    Title = fetchedItem.Title.ToString(),
                    SellerID = fetchedItem.Seller.UserID.ToString(),
                    FetchTime = DateTime.Now,
                    StartTime = fetchedItem.ListingDetails.StartTime.ToLocalTime(),
                    EndTime = fetchedItem.ListingDetails.EndTime.ToLocalTime(),
                    Price = fetchedItem.SellingStatus.CurrentPrice.Value.ToString(),
                    BuyItNowPrice = fetchedItem.BuyItNowPrice.Value.ToString(),
                    BidCount = fetchedItem.SellingStatus.BidCount.ToString(),
                    PrivateListing = fetchedItem.PrivateListing.ToString(),
                    Quantity = fetchedItem.Quantity.ToString(),
                    QuantitySold = fetchedItem.SellingStatus.QuantitySold.ToString(),
                    PrimaryCategory = fetchedItem.PrimaryCategory.CategoryName.ToString(),
                    PrimaryCategoryID = fetchedItem.PrimaryCategory.CategoryID.ToString(),
                    IsInPanelData = 0
                };

                eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
                dbContext.AddToS2ItemInfo(newSearch);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

        }




        private void GetItemFn()
        {
            
            
                Context.CallRetry = GetCallRetry();

                GetItemCall apicall = new GetItemCall(GetContext());
                apicall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
                fetchedItem = apicall.GetItem(TxtItemTerm.Text);
                string[] ListItemDetail = new string[500];

                if (fetchedItem.SecondaryCategory != null)
                {
                    ListItemDetail[6] = fetchedItem.SecondaryCategory.CategoryName;
                    ListItemDetail[7] = fetchedItem.SecondaryCategory.CategoryID;
                }


                if (fetchedItem.SellingStatus.HighBidder != null) ListItemDetail[11] = fetchedItem.SellingStatus.HighBidder.UserID;

                if (fetchedItem.BestOfferDetails != null)
                {
                    ListItemDetail[15] = fetchedItem.BestOfferDetails.BestOfferCount.ToString();
                    ListItemDetail[16] = fetchedItem.BestOfferDetails.BestOfferEnabled.ToString();
                }

                if (fetchedItem.ProductListingDetails != null)
                    ListItemDetail[17] = fetchedItem.ProductListingDetails.ProductID;

                ListItemDetail[18] = "";
                /*
                                if (fetchedItem.PictureDetails != null)
                                {
                                    StringCollection PictureURLs = fetchedItem.PictureDetails.PictureURL;
                                    string PictureURL = "";
                                    for (int i = 0; PictureURLs != null && i < PictureURLs.Count; i++)
                                    {
                                        PictureURL += PictureURLs[i] + "@@";
                                    }
                                    ListItemDetail[18] = PictureURL;
                                }
                 */
                ListItemDetail[19] = fetchedItem.Site.ToString();

                if (fetchedItem.PayPalEmailAddress != null)
                    ListItemDetail[20] = fetchedItem.PayPalEmailAddress.ToString();

                if (fetchedItem.ApplicationData != null)
                    ListItemDetail[21] = fetchedItem.ApplicationData.ToString();

                if (fetchedItem.BusinessSellerDetails != null)
                {
                    ListItemDetail[25] = fetchedItem.BusinessSellerDetails.Address.AddressID.ToString();
                    ListItemDetail[26] = fetchedItem.BusinessSellerDetails.Address.AddressOwner.ToString();
                    ListItemDetail[27] = fetchedItem.BusinessSellerDetails.Address.AddressRecordType.ToString();
                    ListItemDetail[28] = fetchedItem.BusinessSellerDetails.Address.AddressStatus.ToString();
                    ListItemDetail[29] = fetchedItem.BusinessSellerDetails.Address.AddressUsage.ToString();
                    ListItemDetail[30] = fetchedItem.BusinessSellerDetails.Address.CityName.ToString();
                    ListItemDetail[31] = fetchedItem.BusinessSellerDetails.Address.Country.ToString();
                    ListItemDetail[32] = fetchedItem.BusinessSellerDetails.Address.CountryName.ToString();
                    ListItemDetail[33] = fetchedItem.BusinessSellerDetails.Address.County.ToString();
                    ListItemDetail[34] = fetchedItem.BusinessSellerDetails.Address.ExternalAddressID.ToString();
                    ListItemDetail[35] = fetchedItem.BusinessSellerDetails.Address.FirstName.ToString();
                    ListItemDetail[36] = fetchedItem.BusinessSellerDetails.Address.LastName.ToString();
                    ListItemDetail[37] = fetchedItem.BusinessSellerDetails.Address.Name.ToString();
                    ListItemDetail[38] = fetchedItem.BusinessSellerDetails.Address.InternationalName.ToString();
                    ListItemDetail[39] = fetchedItem.BusinessSellerDetails.Address.InternationalStateAndCity.ToString();
                    ListItemDetail[40] = fetchedItem.BusinessSellerDetails.Address.InternationalStreet.ToString();
                    ListItemDetail[41] = fetchedItem.BusinessSellerDetails.Address.Phone.ToString();
                    ListItemDetail[42] = fetchedItem.BusinessSellerDetails.Address.PhoneCountryCode.ToString();
                    ListItemDetail[43] = fetchedItem.BusinessSellerDetails.Address.PhoneCountryPrefix.ToString();
                    ListItemDetail[44] = fetchedItem.BusinessSellerDetails.Address.PhoneLocalNumber.ToString();
                    ListItemDetail[45] = fetchedItem.BusinessSellerDetails.Address.PostalCode.ToString();
                    ListItemDetail[46] = fetchedItem.BusinessSellerDetails.Address.StateOrProvince.ToString();
                    ListItemDetail[47] = fetchedItem.BusinessSellerDetails.Address.Street.ToString();
                    ListItemDetail[48] = fetchedItem.BusinessSellerDetails.Address.Street1.ToString();
                    ListItemDetail[49] = fetchedItem.BusinessSellerDetails.Address.Street2.ToString();
                    ListItemDetail[50] = fetchedItem.BusinessSellerDetails.Email.ToString();
                    ListItemDetail[51] = fetchedItem.BusinessSellerDetails.Fax.ToString();
                    ListItemDetail[52] = fetchedItem.BusinessSellerDetails.LegalInvoice.ToString();
                    ListItemDetail[53] = fetchedItem.BusinessSellerDetails.TermsAndConditions.ToString();
                    ListItemDetail[54] = fetchedItem.BusinessSellerDetails.TradeRegistrationNumber.ToString();
                    ListItemDetail[55] = fetchedItem.BusinessSellerDetails.VATDetails.BusinessSeller.ToString();
                    ListItemDetail[56] = fetchedItem.BusinessSellerDetails.VATDetails.RestrictedToBusiness.ToString();
                    ListItemDetail[57] = fetchedItem.BusinessSellerDetails.VATDetails.VATID.ToString();
                    ListItemDetail[58] = fetchedItem.BusinessSellerDetails.VATDetails.VATPercent.ToString();
                    ListItemDetail[59] = fetchedItem.BusinessSellerDetails.VATDetails.VATSite.ToString();
                }

                if (fetchedItem.BuyerGuaranteePrice != null)
                {
                    ListItemDetail[60] = fetchedItem.BuyerGuaranteePrice.currencyID.ToString();
                    ListItemDetail[61] = fetchedItem.BuyerGuaranteePrice.Value.ToString();
                }

                ListItemDetail[62] = fetchedItem.BuyerProtection.ToString();
                ListItemDetail[63] = fetchedItem.BuyerResponsibleForShipping.ToString();


                if (fetchedItem.BuyItNowPrice != null)
                {
                    ListItemDetail[64] = fetchedItem.BuyItNowPrice.currencyID.ToString();
                    ListItemDetail[65] = fetchedItem.BuyItNowPrice.Value.ToString();
                }

                ListItemDetail[66] = fetchedItem.CategoryBasedAttributesPrefill.ToString();

                ListItemDetail[67] = fetchedItem.CategoryMappingAllowed.ToString();


                if (fetchedItem.Charity != null)
                {
                    ListItemDetail[68] = fetchedItem.Charity.CharityID.ToString();
                }
                if (fetchedItem.ClassifiedAdPayPerLeadFee != null)
                {
                    ListItemDetail[69] = fetchedItem.ClassifiedAdPayPerLeadFee.currencyID.ToString();
                    ListItemDetail[70] = fetchedItem.ClassifiedAdPayPerLeadFee.Value.ToString();
                }

                if (fetchedItem.ConditionDisplayName != null)
                {
                    ListItemDetail[71] = fetchedItem.ConditionDisplayName.ToString();

                }
                ListItemDetail[72] = fetchedItem.ConditionID.ToString();
                ListItemDetail[73] = fetchedItem.Country.ToString();
                if (fetchedItem.CrossBorderTrade != null)
                    ListItemDetail[74] = fetchedItem.CrossBorderTrade.ToString();
                if (fetchedItem.CrossPromotion != null)
                    ListItemDetail[75] = fetchedItem.CrossPromotion.ItemID.ToString();
                ListItemDetail[76] = fetchedItem.Currency.ToString();
                if (fetchedItem.Description != null)
                    //na                    ListItemDetail[77] = fetchedItem.Description.ToString();
                    ListItemDetail[78] = fetchedItem.DescriptionReviseMode.ToString();
                ListItemDetail[79] = fetchedItem.DisableBuyerRequirements.ToString();
                ListItemDetail[80] = fetchedItem.DispatchTimeMax.ToString();
                if (fetchedItem.Distance != null)
                    ListItemDetail[81] = fetchedItem.Distance.DistanceUnit.ToString();
                if (fetchedItem.eBayNotes != null)
                    ListItemDetail[82] = fetchedItem.eBayNotes.ToString();
                if (fetchedItem.ExternalProductID != null)
                    ListItemDetail[83] = fetchedItem.ExternalProductID.Value.ToString();
                if (fetchedItem.FreeAddedCategory != null)
                {
                    ListItemDetail[84] = fetchedItem.FreeAddedCategory.AutoPayEnabled.ToString();
                    ListItemDetail[85] = fetchedItem.FreeAddedCategory.B2BVATEnabled.ToString();
                    ListItemDetail[86] = fetchedItem.FreeAddedCategory.BestOfferEnabled.ToString();
                    ListItemDetail[87] = fetchedItem.FreeAddedCategory.CatalogEnabled.ToString();
                    ListItemDetail[88] = fetchedItem.FreeAddedCategory.CategoryID.ToString();
                    ListItemDetail[89] = fetchedItem.FreeAddedCategory.CategoryLevel.ToString();
                    ListItemDetail[90] = fetchedItem.FreeAddedCategory.CategoryName.ToString();
                    ListItemDetail[91] = fetchedItem.FreeAddedCategory.CategoryParentID.ToString();
                    ListItemDetail[92] = fetchedItem.FreeAddedCategory.CategoryParentName.ToString();
                    ListItemDetail[93] = fetchedItem.FreeAddedCategory.CharacteristicsSets.ToString();
                    ListItemDetail[94] = fetchedItem.FreeAddedCategory.Expired.ToString();
                    ListItemDetail[95] = fetchedItem.FreeAddedCategory.IntlAutosFixedCat.ToString();
                    ListItemDetail[96] = fetchedItem.FreeAddedCategory.Keywords.ToString();
                    ListItemDetail[97] = fetchedItem.FreeAddedCategory.LeafCategory.ToString();
                    ListItemDetail[98] = fetchedItem.FreeAddedCategory.LSD.ToString();
                    ListItemDetail[99] = fetchedItem.FreeAddedCategory.NumOfItems.ToString();
                    ListItemDetail[100] = fetchedItem.FreeAddedCategory.ORPA.ToString();
                    ListItemDetail[101] = fetchedItem.FreeAddedCategory.ORRA.ToString();
                    ListItemDetail[102] = fetchedItem.FreeAddedCategory.ProductFinderIDs.ToString();
                    ListItemDetail[103] = fetchedItem.FreeAddedCategory.ProductSearchPageAvailable.ToString();
                    ListItemDetail[104] = fetchedItem.FreeAddedCategory.SellerGuaranteeEligible.ToString();
                    ListItemDetail[105] = fetchedItem.FreeAddedCategory.Virtual.ToString();
                }
                ListItemDetail[106] = fetchedItem.GetItFast.ToString();
                ListItemDetail[107] = fetchedItem.GiftIcon.ToString();
                if (fetchedItem.GiftServices != null)
                    ListItemDetail[108] = fetchedItem.GiftServices.ToString();
                if (fetchedItem.GroupCategoryID != null)
                    ListItemDetail[109] = fetchedItem.GroupCategoryID.ToString();
                ListItemDetail[110] = fetchedItem.HitCount.ToString();
                ListItemDetail[111] = fetchedItem.HitCounter.ToString();
                ListItemDetail[112] = fetchedItem.IntegratedMerchantCreditCardEnabled.ToString();
                ListItemDetail[113] = fetchedItem.InventoryTrackingMethod.ToString();
                if (fetchedItem.ItemCompatibilityList != null)
                {
                    ListItemDetail[115] = fetchedItem.ItemCompatibilityList.Compatibility.ToString();
                }
                ListItemDetail[114] = fetchedItem.ItemCompatibilityCount.ToString();
                if (fetchedItem.ItemPolicyViolation != null)
                {
                    ListItemDetail[116] = fetchedItem.ItemPolicyViolation.PolicyID.ToString();
                    ListItemDetail[117] = fetchedItem.ItemPolicyViolation.PolicyText.ToString();
                }
                if (fetchedItem.ItemSpecifics != null)
                    ListItemDetail[118] = fetchedItem.ItemSpecifics.ToString();

                ListItemDetail[119] = fetchedItem.LeadCount.ToString();

                ListItemDetail[120] = fetchedItem.LimitedWarrantyEligible.ToString();
                if (fetchedItem.ListingCheckoutRedirectPreference != null)
                {
                    ListItemDetail[121] = fetchedItem.ListingCheckoutRedirectPreference.ProStoresStoreName.ToString();
                    ListItemDetail[122] = fetchedItem.ListingCheckoutRedirectPreference.SellerThirdPartyUsername.ToString();
                }

                if (fetchedItem.ListingDesigner != null)
                {
                    ListItemDetail[123] = fetchedItem.ListingDesigner.LayoutID.ToString();
                    ListItemDetail[124] = fetchedItem.ListingDesigner.ThemeID.ToString();
                    ListItemDetail[125] = fetchedItem.ListingDesigner.OptimalPictureSize.ToString();
                }

                ListItemDetail[126] = fetchedItem.ListingDetails.Adult.ToString();
                if (fetchedItem.ListingDetails.BestOfferAutoAcceptPrice != null)
                    ListItemDetail[127] = fetchedItem.ListingDetails.BestOfferAutoAcceptPrice.Value.ToString();
                ListItemDetail[128] = fetchedItem.ListingDetails.BindingAuction.ToString();
                ListItemDetail[129] = fetchedItem.ListingDetails.BuyItNowAvailable.ToString();
                ListItemDetail[130] = fetchedItem.ListingDetails.CheckoutEnabled.ToString();
                if (fetchedItem.ListingDetails.ConvertedBuyItNowPrice != null)
                    ListItemDetail[131] = fetchedItem.ListingDetails.ConvertedBuyItNowPrice.Value.ToString();
                if (fetchedItem.ListingDetails.ConvertedReservePrice != null)
                    ListItemDetail[132] = fetchedItem.ListingDetails.ConvertedReservePrice.Value.ToString();
                if (fetchedItem.ListingDetails.ConvertedStartPrice != null)
                    ListItemDetail[133] = fetchedItem.ListingDetails.ConvertedStartPrice.Value.ToString();
                if (fetchedItem.ListingDetails.EndTime != null)
                    ListItemDetail[134] = fetchedItem.ListingDetails.EndTime.ToString();
                ListItemDetail[135] = fetchedItem.ListingDetails.HasPublicMessages.ToString();
                ListItemDetail[136] = fetchedItem.ListingDetails.HasReservePrice.ToString();
                ListItemDetail[137] = fetchedItem.ListingDetails.HasUnansweredQuestions.ToString();
                if (fetchedItem.ListingDetails.LocalListingDistance != null)
                    ListItemDetail[138] = fetchedItem.ListingDetails.LocalListingDistance.ToString();
                if (fetchedItem.ListingDetails.MinimumBestOfferMessage != null)
                    ListItemDetail[139] = fetchedItem.ListingDetails.MinimumBestOfferMessage.ToString();
                if (fetchedItem.ListingDetails.MinimumBestOfferPrice != null)
                    ListItemDetail[140] = fetchedItem.ListingDetails.MinimumBestOfferPrice.Value.ToString();
                ListItemDetail[141] = fetchedItem.ListingDetails.PayPerLeadEnabled.ToString();
                if (fetchedItem.ListingDetails.RelistedItemID != null)
                    ListItemDetail[142] = fetchedItem.ListingDetails.RelistedItemID.ToString();
                if (fetchedItem.ListingDetails.SecondChanceOriginalItemID != null)
                    ListItemDetail[143] = fetchedItem.ListingDetails.SecondChanceOriginalItemID.ToString();
                ListItemDetail[144] = fetchedItem.ListingDetails.SellerBusinessType.ToString();
                if (fetchedItem.ListingDetails.StartTime != null)
                    ListItemDetail[145] = fetchedItem.ListingDetails.StartTime.ToString();
                if (fetchedItem.ListingDetails.TCROriginalItemID != null)
                    ListItemDetail[146] = fetchedItem.ListingDetails.TCROriginalItemID.ToString();
                if (fetchedItem.ListingDetails.ViewItemURL != null)
                    ListItemDetail[147] = fetchedItem.ListingDetails.ViewItemURL.ToString();




                if (fetchedItem.ListingDuration != null)
                    ListItemDetail[148] = fetchedItem.ListingDuration.ToString();
                if (fetchedItem.ListingEnhancement != null)
                    ListItemDetail[149] = fetchedItem.ListingEnhancement.ToString();

                ListItemDetail[150] = fetchedItem.ListingSubtype2.ToString();
                ListItemDetail[151] = fetchedItem.ListingType.ToString();
                ListItemDetail[152] = fetchedItem.LocalListing.ToString();
                if (fetchedItem.Location != null) ListItemDetail[153] = fetchedItem.Location.ToString();
                //                     ListItemDetail[154] = fetchedItem.LocationDefaulted.ToString();
                ListItemDetail[155] = fetchedItem.LotSize.ToString();
                ListItemDetail[156] = fetchedItem.MechanicalCheckAccepted.ToString();
                ListItemDetail[157] = fetchedItem.MotorsGermanySearchable.ToString();
                ListItemDetail[158] = fetchedItem.NewLeadCount.ToString();
                if (fetchedItem.PartnerCode != null) ListItemDetail[159] = fetchedItem.PartnerCode.ToString();
                if (fetchedItem.PartnerName != null) ListItemDetail[160] = fetchedItem.PartnerName.ToString();
                if (fetchedItem.PaymentAllowedSite != null) ListItemDetail[161] = fetchedItem.PaymentAllowedSite.ToString();

                if (fetchedItem.PaymentDetails != null) ListItemDetail[162] = fetchedItem.PaymentDetails.DaysToFullPayment.ToString();
                if (fetchedItem.PaymentDetails != null) ListItemDetail[163] = fetchedItem.PaymentDetails.HoursToDeposit.ToString();
                if (fetchedItem.PaymentMethods != null) ListItemDetail[164] = fetchedItem.PaymentMethods.ToString();
                if (fetchedItem.PayPalEmailAddress != null) ListItemDetail[165] = fetchedItem.PayPalEmailAddress.ToString();
                if (fetchedItem.PostalCode != null) ListItemDetail[166] = fetchedItem.PostalCode.ToString();
                ListItemDetail[167] = fetchedItem.PrivateListing.ToString();
                if (fetchedItem.PrivateNotes != null) ListItemDetail[168] = fetchedItem.PrivateNotes.ToString();
                if (fetchedItem.ProductListingDetails != null) ListItemDetail[169] = fetchedItem.ProductListingDetails.ToString();
                ListItemDetail[170] = fetchedItem.ProxyItem.ToString();
                ListItemDetail[171] = fetchedItem.Quantity.ToString();
                ListItemDetail[172] = fetchedItem.QuantityAvailable.ToString();
                ListItemDetail[173] = fetchedItem.QuantitySpecified.ToString();
                ListItemDetail[174] = fetchedItem.QuestionCount.ToString();
                if (fetchedItem.RegionID != null) ListItemDetail[175] = fetchedItem.RegionID.ToString();
                ListItemDetail[176] = fetchedItem.Relisted.ToString();
                ListItemDetail[177] = fetchedItem.RelistLink.ToString();
                if (fetchedItem.ReservePrice != null) ListItemDetail[178] = fetchedItem.ReservePrice.Value.ToString();
                if (fetchedItem.ReservePrice != null) ListItemDetail[179] = fetchedItem.ReservePrice.currencyID.ToString();

                if (fetchedItem.ReturnPolicy != null)
                {
                    ListItemDetail[180] = fetchedItem.ReturnPolicy.Refund.ToString();
                    ListItemDetail[181] = fetchedItem.ReturnPolicy.RefundOption.ToString();
                    ListItemDetail[182] = fetchedItem.ReturnPolicy.ReturnsAccepted.ToString();
                    ListItemDetail[183] = fetchedItem.ReturnPolicy.ReturnsWithin.ToString();
                    ListItemDetail[184] = fetchedItem.ReturnPolicy.ShippingCostPaidBy.ToString();
                    ListItemDetail[185] = fetchedItem.ReturnPolicy.WarrantyDuration.ToString();
                    ListItemDetail[186] = fetchedItem.ReturnPolicy.WarrantyOffered.ToString();
                    ListItemDetail[187] = fetchedItem.ReturnPolicy.WarrantyType.ToString();
                }

                if (fetchedItem.ScheduleTime != null) ListItemDetail[188] = fetchedItem.ScheduleTime.ToString();

                if (fetchedItem.SearchDetails != null)
                {
                    ListItemDetail[189] = fetchedItem.SearchDetails.Picture.ToString();
                    ListItemDetail[190] = fetchedItem.SearchDetails.RecentListing.ToString();
                    ListItemDetail[191] = fetchedItem.SearchDetails.BuyItNowEnabled.ToString();
                }

                if (fetchedItem.Seller != null) ListItemDetail[192] = fetchedItem.Seller.AboutMePage.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[193] = fetchedItem.Seller.BillingEmail.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[194] = fetchedItem.Seller.CharityAffiliations.CharityID.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[195] = fetchedItem.Seller.eBayGoodStanding.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[196] = fetchedItem.Seller.eBayWikiReadOnly.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[197] = fetchedItem.Seller.EIASToken.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[198] = fetchedItem.Seller.Email.ToString();
                if (fetchedItem.Seller != null) ListItemDetail[199] = fetchedItem.Seller.EnterpriseSeller.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[200] = fetchedItem.Seller.FeedbackPrivate.ToString();
                if (fetchedItem.Seller != null) ListItemDetail[201] = fetchedItem.Seller.FeedbackRatingStar.ToString();
                if (fetchedItem.Seller != null) ListItemDetail[202] = fetchedItem.Seller.FeedbackScore.ToString();
                if (fetchedItem.Seller != null) ListItemDetail[203] = fetchedItem.Seller.IDVerified.ToString();
                if (fetchedItem.Seller != null) ListItemDetail[204] = fetchedItem.Seller.MotorsDealer.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[205] = fetchedItem.Seller.NewUser.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[206] = fetchedItem.Seller.PayPalAccountLevel.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[207] = fetchedItem.Seller.PayPalAccountStatus.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[208] = fetchedItem.Seller.PayPalAccountType.ToString();
                if (fetchedItem.Seller != null) ListItemDetail[209] = fetchedItem.Seller.PositiveFeedbackPercent.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[210] = fetchedItem.Seller.QualifiesForSelling.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[211] = fetchedItem.Seller.RegistrationAddress.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[212] = fetchedItem.Seller.RegistrationDate.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[213] = fetchedItem.Seller.SellerPaymentMethod.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[214] = fetchedItem.Seller.TUVLevel.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[215] = fetchedItem.Seller.UniqueNegativeFeedbackCount.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[216] = fetchedItem.Seller.UniqueNeutralFeedbackCount.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[217] = fetchedItem.Seller.UniquePositiveFeedbackCount.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[218] = fetchedItem.Seller.UserAnonymized.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[219] = fetchedItem.Seller.UserIDChanged.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[220] = fetchedItem.Seller.UserSubscription.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[221] = fetchedItem.Seller.VATID.ToString();
                //                    if (fetchedItem.Seller != null) ListItemDetail[222] = fetchedItem.Seller.VATStatus.ToString();

                /*                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[223] = fetchedItem.SellerContactDetails.AddressID.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[224] = fetchedItem.SellerContactDetails.AddressOwner.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[225] = fetchedItem.SellerContactDetails.AddressRecordType.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[226] = fetchedItem.SellerContactDetails.AddressStatus.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[227] = fetchedItem.SellerContactDetails.AddressUsage.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[228] = fetchedItem.SellerContactDetails.CityName.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[229] = fetchedItem.SellerContactDetails.CompanyName.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[230] = fetchedItem.SellerContactDetails.Country.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[231] = fetchedItem.SellerContactDetails.CountryName.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[232] = fetchedItem.SellerContactDetails.County.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[233] = fetchedItem.SellerContactDetails.ExternalAddressID.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[234] = fetchedItem.SellerContactDetails.FirstName.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[235] = fetchedItem.SellerContactDetails.LastName.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[236] = fetchedItem.SellerContactDetails.Name.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[237] = fetchedItem.SellerContactDetails.InternationalName.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[238] = fetchedItem.SellerContactDetails.InternationalStateAndCity.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[239] = fetchedItem.SellerContactDetails.InternationalStreet.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[240] = fetchedItem.SellerContactDetails.Phone.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[241] = fetchedItem.SellerContactDetails.PhoneCountryCode.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[242] = fetchedItem.SellerContactDetails.PhoneCountryPrefix.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[243] = fetchedItem.SellerContactDetails.PhoneLocalNumber.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[244] = fetchedItem.SellerContactDetails.PostalCode.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[245] = fetchedItem.SellerContactDetails.StateOrProvince.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[246] = fetchedItem.SellerContactDetails.Street.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[247] = fetchedItem.SellerContactDetails.Street1.ToString();
                                    if (fetchedItem.SellerContactDetails != null) ListItemDetail[248] = fetchedItem.SellerContactDetails.Street2.ToString();
                 */
                //                    if (fetchedItem.SellerInventoryID != null) ListItemDetail[249] = fetchedItem.SellerInventoryID.ToString();
                //                    if (fetchedItem.SellerVacationNote != null) ListItemDetail[250] = fetchedItem.SellerVacationNote.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[251] = fetchedItem.SellingStatus.AdminEnded.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[252] = fetchedItem.SellingStatus.BidCount.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[253] = fetchedItem.SellingStatus.BidderCount.ToString();
                //                    if (fetchedItem.SellingStatus != null) ListItemDetail[254] = fetchedItem.SellingStatus.BidIncrement.Value.ToString();
                //                    if (fetchedItem.SellingStatus != null) ListItemDetail[255] = fetchedItem.SellingStatus.BidIncrement.currencyID.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[256] = fetchedItem.SellingStatus.ConvertedCurrentPrice.Value.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[257] = fetchedItem.SellingStatus.CurrentPrice.Value.ToString();
                //na                    if (fetchedItem.SellingStatus != null) ListItemDetail[258] = fetchedItem.SellingStatus.FinalValueFee.Value.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[259] = fetchedItem.SellingStatus.HighBidder.UserID.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[260] = fetchedItem.SellingStatus.LeadCount.ToString();
                //                    if (fetchedItem.SellingStatus != null) ListItemDetail[261] = fetchedItem.SellingStatus.ListingStatus.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[262] = fetchedItem.SellingStatus.MinimumToBid.ToString();
                //                    if (fetchedItem.SellingStatus != null) ListItemDetail[263] = fetchedItem.SellingStatus.PromotionalSaleDetails.EndTime.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[264] = fetchedItem.SellingStatus.QuantitySold.ToString();
                if (fetchedItem.SellingStatus != null) ListItemDetail[265] = fetchedItem.SellingStatus.ReserveMet.ToString();
                //                    if (fetchedItem.SellingStatus != null) ListItemDetail[266] = fetchedItem.SellingStatus.SecondChanceEligible.ToString();


                //                    if (fetchedItem.ShippingDetails != null) ListItemDetail[267] = fetchedItem.ShippingDetails.AllowPaymentEdit.ToString();
                //                    if (fetchedItem.ShippingDetails != null) ListItemDetail[268] = fetchedItem.ShippingDetails.ApplyShippingDiscount.ToString();
                //                    if (fetchedItem.ShippingDetails != null) ListItemDetail[269] = fetchedItem.ShippingDetails.CalculatedShippingDiscount.DiscountName.ToString();
                //                    if (fetchedItem.ShippingDetails != null) ListItemDetail[270] = fetchedItem.ShippingDetails.CalculatedShippingDiscount.DiscountProfile.ToString();
                //                    if (fetchedItem.ShippingDetails != null) ListItemDetail[271] = fetchedItem.ShippingDetails.InsuranceFee.Value.ToString();
                if (fetchedItem.ShipToLocations != null) ListItemDetail[272] = fetchedItem.ShipToLocations.ToString();
                ListItemDetail[273] = fetchedItem.Site.ToString();
                if (fetchedItem.SKU != null) ListItemDetail[274] = fetchedItem.SKU.ToString();
                ListItemDetail[275] = fetchedItem.SkypeEnabled.ToString();
                if (fetchedItem.SkypeID != null) ListItemDetail[276] = fetchedItem.SkypeID.ToString();
                //                    if (fetchedItem.StartPrice != null) ListItemDetail[277] = fetchedItem.StartPrice.Value.ToString();
                //                    if (fetchedItem.StartPrice != null) ListItemDetail[278] = fetchedItem.StartPrice.currencyID.ToString();
                //                    if (fetchedItem.Storefront != null) ListItemDetail[279] = fetchedItem.Storefront.StoreName.ToString();
                //                    if (fetchedItem.Storefront != null) ListItemDetail[280] = fetchedItem.Storefront.StoreCategoryID.ToString();
                //                    if (fetchedItem.Storefront != null) ListItemDetail[281] = fetchedItem.Storefront.StoreCategory2ID.ToString();
                //                    if (fetchedItem.SubTitle != null) ListItemDetail[282] = fetchedItem.SubTitle.ToString();
                //                    if (fetchedItem.TaxCategory != null) ListItemDetail[283] = fetchedItem.TaxCategory.ToString();
                ListItemDetail[284] = fetchedItem.ThirdPartyCheckout.ToString();
                ListItemDetail[285] = fetchedItem.ThirdPartyCheckoutIntegration.ToString();
                ListItemDetail[286] = fetchedItem.TimeLeft.ToString();
                ListItemDetail[287] = fetchedItem.TotalQuestionCount.ToString();
                ListItemDetail[288] = fetchedItem.UpdateReturnPolicy.ToString();
                ListItemDetail[289] = fetchedItem.UpdateSellerInfo.ToString();
                ListItemDetail[290] = fetchedItem.UseTaxTable.ToString();
                //                    if (fetchedItem.UUID != null) ListItemDetail[291] = fetchedItem.UUID.ToString();
                if (fetchedItem.Variations != null) ListItemDetail[292] = fetchedItem.Variations.ToString();
                //                    if (fetchedItem.VATDetails != null) ListItemDetail[293] = fetchedItem.VATDetails.VATID.ToString();
                ListItemDetail[294] = fetchedItem.WatchCount.ToString();
                ListItemDetail[295] = fetchedItem.WatchCountSpecified.ToString();


                if (fetchedItem.ApplyBuyerProtection != null)
                {
                    ListItemDetail[296] = fetchedItem.ApplyBuyerProtection.BuyerProtectionSource.ToString();
                    ListItemDetail[297] = fetchedItem.ApplyBuyerProtection.BuyerProtectionStatus.ToString();
                }

                //ListItemDetail[298] = fetchedItem.ApplyShippingDiscount.ToString();


                ListItemDetail[299] = fetchedItem.AutoPay.ToString();


                if (fetchedItem.BestOfferDetails != null)
                {
                    ListItemDetail[300] = fetchedItem.BestOfferDetails.BestOffer.currencyID.ToString();
                    ListItemDetail[301] = fetchedItem.BestOfferDetails.BestOffer.Value.ToString();
                    ListItemDetail[302] = fetchedItem.BestOfferDetails.BestOfferCount.ToString();
                    ListItemDetail[303] = fetchedItem.BestOfferDetails.BestOfferEnabled.ToString();
                    ListItemDetail[304] = fetchedItem.BestOfferDetails.BestOfferStatus.ToString();
                    ListItemDetail[305] = fetchedItem.BestOfferDetails.BestOfferType.ToString();
                }

                ListItemDetail[306] = fetchedItem.BestOfferEnabled.ToString();


                if (fetchedItem.BiddingDetails != null)
                {
                    ListItemDetail[307] = fetchedItem.BiddingDetails.BidAssistant.ToString();
                    ListItemDetail[308] = fetchedItem.BiddingDetails.ConvertedMaxBid.currencyID.ToString();
                    ListItemDetail[309] = fetchedItem.BiddingDetails.ConvertedMaxBid.Value.ToString();
                    ListItemDetail[310] = fetchedItem.BiddingDetails.MaxBid.currencyID.ToString();
                    ListItemDetail[311] = fetchedItem.BiddingDetails.MaxBid.Value.ToString();
                    ListItemDetail[312] = fetchedItem.BiddingDetails.QuantityBid.ToString();
                    ListItemDetail[313] = fetchedItem.BiddingDetails.QuantityWon.ToString();
                    ListItemDetail[314] = fetchedItem.BiddingDetails.Winning.ToString();
                    ListItemDetail[315] = fetchedItem.BidGroupItem.ToString();
                }



            

        }

        private static CallRetry GetCallRetry()
        {
            CallRetry retry = new CallRetry();
            retry.DelayTime = 1000;		// 1 second
            retry.MaximumRetries = 2;
            retry.TriggerHttpStatusCodes = new Int32Collection();
            retry.TriggerHttpStatusCodes.Add(502);
            retry.TriggerHttpStatusCodes.Add(404);
            return retry;
        }



        public class ItemSpecificBean
        {
            public string setCarAttr1 { get; set; }
            public string setCarAttr2 { get; set; }
            public string setCarAttr3 { get; set; }
            public string setCarAttr4 { get; set; }
            public string setCarAttr5 { get; set; }
            public string setCarAttr6 { get; set; }
            public string setCarAttr7 { get; set; }
            public string setCarAttr8 { get; set; }
            public string setCarAttr9 { get; set; }
            public string setCarAttr10 { get; set; }
            public string setCarAttr11 { get; set; }
            public string setCarAttr12 { get; set; }
            public string setCarAttr13 { get; set; }
            public string setCarAttr14 { get; set; }
            public string setCarAttr15 { get; set; }
            public string setCarAttr16 { get; set; }
            public string setCarAttr17 { get; set; }
            public string setCarAttr18 { get; set; }
            public string setCarAttr19 { get; set; }
            public string setCarAttr20 { get; set; }
            public string setCarAttr21 { get; set; }
            public string setCarAttr22 { get; set; }
            public string setCarAttr23 { get; set; }
            public string setCarAttr24 { get; set; }
            public string setCarAttr25 { get; set; }
            public string setCarAttr26 { get; set; }
            public string setCarAttr27 { get; set; }
            public string setCarAttr28 { get; set; }
            public string setCarAttr29 { get; set; }
            public string setCarAttr30 { get; set; }
            public string setCarAttr31 { get; set; }
            public string setCarAttr32 { get; set; }
            public string setCarAttr33 { get; set; }
            public string setCarAttr34 { get; set; }
            public string setCarAttr35 { get; set; }
            public string setCarAttr36 { get; set; }
            public string setCarAttr37 { get; set; }
            public string setCarAttr38 { get; set; }
            public string setCarAttr39 { get; set; }
            public string setCarAttr40 { get; set; }
            public string setCarAttr41 { get; set; }
            public string setCarAttr42 { get; set; }
            public string setCarAttr43 { get; set; }
            public string setCarAttr44 { get; set; }
            public string setCarAttr45 { get; set; }
            public string setCarAttr46 { get; set; }
            public string setCarAttr47 { get; set; }
            public string setCarAttr48 { get; set; }
            public string setCarAttr49 { get; set; }
            public string setCarAttr50 { get; set; }
            public string setCarAttr51 { get; set; }
            public string setCarAttr52 { get; set; }
            public string setCarAttr53 { get; set; }
            public string setCarAttr54 { get; set; }
            public string setCarAttr55 { get; set; }
            public string setCarAttr56 { get; set; }
            public string setCarAttr57 { get; set; }
            public string setCarAttr58 { get; set; }
            public string setCarAttr59 { get; set; }
            public string setCarAttr60 { get; set; }
            public string setCarAttr61 { get; set; }
            public string setCarAttr62 { get; set; }
            public string setCarAttr63 { get; set; }
            public string setCarAttr64 { get; set; }
            public string setCarAttr65 { get; set; }
            public string setCarAttr66 { get; set; }
            public string setCarAttr67 { get; set; }
            public string setCarAttr68 { get; set; }
            public string setCarAttr69 { get; set; }
            public string setCarAttr70 { get; set; }
            public string setCarAttr71 { get; set; }
            public string setCarAttr72 { get; set; }
            public string setCarAttr73 { get; set; }
            public string setCarAttr74 { get; set; }
            public string setCarAttr75 { get; set; }
            public string setCarAttr76 { get; set; }
            public string setCarAttr77 { get; set; }
            public string setCarAttr78 { get; set; }
            public string setCarAttr79 { get; set; }
            public string setCarAttr80 { get; set; }
            public string setCarAttr81 { get; set; }
            public string setCarAttr82 { get; set; }
            public string setCarAttr83 { get; set; }
            public string setCarAttr84 { get; set; }
            public string setCarAttr85 { get; set; }
            public string setCarAttr86 { get; set; }
            public string setCarAttr87 { get; set; }
            public string setCarAttr88 { get; set; }
            public string setCarAttr89 { get; set; }
            public string setCarAttr90 { get; set; }
            public string setCarAttr91 { get; set; }
            public string setCarAttr92 { get; set; }
            public string setCarAttr93 { get; set; }
            public string setCarAttr94 { get; set; }
            public string setCarAttr95 { get; set; }
            public string setCarAttr96 { get; set; }
            public string setCarAttr97 { get; set; }
            public string setCarAttr98 { get; set; }
            public string setCarAttr99 { get; set; }
            public string setCarAttr100 { get; set; } 
        }

        private void getCarAttr(AttributeSetType attrSet, ItemType item)
        {
            // create a ItemSpecificBean object for storing the attribute data
            // or you can store data to a Collection 
            ItemSpecificBean carEntry = new ItemSpecificBean();

            AttributeType[] attrTypeArray = attrSet.Attribute.ToArray();
            for (int attrIndex = 0; attrIndex < attrTypeArray.Length; attrIndex++)
            {
                AttributeType theAttr = attrTypeArray[attrIndex];

                Log("the Attribute IDs are: " + theAttr.attributeID.ToString());
                /*
                if (theAttr.attributeID == 39) carEntry.setCarAttr1 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 41) carEntry.setCarAttr2 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 214) carEntry.setCarAttr3 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10239) carEntry.setCarAttr4 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10242) carEntry.setCarAttr5 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10243) carEntry.setCarAttr6 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10244) carEntry.setCarAttr7 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 26738) carEntry.setCarAttr8 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 39705) carEntry.setCarAttr9 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10240) carEntry.setCarAttr10 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10241) carEntry.setCarAttr11 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10245) carEntry.setCarAttr12 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10246) carEntry.setCarAttr13 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10247) carEntry.setCarAttr14 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10248) carEntry.setCarAttr15 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10446) carEntry.setCarAttr16 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 18) carEntry.setCarAttr17 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 213) carEntry.setCarAttr18 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 85) carEntry.setCarAttr19 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 50968) carEntry.setCarAttr20 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25436) carEntry.setCarAttr21 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10718) carEntry.setCarAttr22 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10719) carEntry.setCarAttr23 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10720) carEntry.setCarAttr24 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 56739) carEntry.setCarAttr25 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 56740) carEntry.setCarAttr26 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 38) carEntry.setCarAttr27 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 50027) carEntry.setCarAttr28 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 50028) carEntry.setCarAttr29 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 50029) carEntry.setCarAttr30 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10733) carEntry.setCarAttr31 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 50030) carEntry.setCarAttr32 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10734) carEntry.setCarAttr33 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 50031) carEntry.setCarAttr34 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 33711) carEntry.setCarAttr35 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25839) carEntry.setCarAttr36 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25840) carEntry.setCarAttr37 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25841) carEntry.setCarAttr38 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25842) carEntry.setCarAttr39 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25843) carEntry.setCarAttr40 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 50037) carEntry.setCarAttr41 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25846) carEntry.setCarAttr42 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25852) carEntry.setCarAttr43 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 10236) carEntry.setCarAttr44 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25918) carEntry.setCarAttr45 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 25919) carEntry.setCarAttr46 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 26590) carEntry.setCarAttr47 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 26589) carEntry.setCarAttr48 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 26591) carEntry.setCarAttr49 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 31183) carEntry.setCarAttr50 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 27773) carEntry.setCarAttr51 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                if (theAttr.attributeID == 26739) carEntry.setCarAttr52 = theAttr.Value.ToArray()[0].ValueLiteral.ToString();
                */
            }
            /*
            Log("AttID: 39      -    " + carEntry.setCarAttr1);
            Log("AttID: 41      -    " + carEntry.setCarAttr2);
            Log("AttID: 214      -    " + carEntry.setCarAttr3);
            Log("AttID: 10239      -    " + carEntry.setCarAttr4);
            Log("AttID: 10242      -    " + carEntry.setCarAttr5);
            Log("AttID: 10243      -    " + carEntry.setCarAttr6);
            Log("AttID: 10244      -    " + carEntry.setCarAttr7);
            Log("AttID: 26738      -    " + carEntry.setCarAttr8);
            Log("AttID: 39705      -    " + carEntry.setCarAttr9);
            Log("AttID: 10240      -    " + carEntry.setCarAttr10);
            Log("AttID: 10241      -    " + carEntry.setCarAttr11);
            Log("AttID: 10245      -    " + carEntry.setCarAttr12);
            Log("AttID: 10246      -    " + carEntry.setCarAttr13);
            Log("AttID: 10247      -    " + carEntry.setCarAttr14);
            Log("AttID: 10248      -    " + carEntry.setCarAttr15);
            Log("AttID: 10446      -    " + carEntry.setCarAttr16);
            Log("AttID: 18      -    " + carEntry.setCarAttr17);
            Log("AttID: 213      -    " + carEntry.setCarAttr18);
            Log("AttID: 85      -    " + carEntry.setCarAttr19);
            Log("AttID: 50968      -    " + carEntry.setCarAttr20);
            Log("AttID: 25436      -    " + carEntry.setCarAttr21);
            Log("AttID: 10718      -    " + carEntry.setCarAttr22);
            Log("AttID: 10719      -    " + carEntry.setCarAttr23);
            Log("AttID: 10720      -    " + carEntry.setCarAttr24);
            Log("AttID: 56739      -    " + carEntry.setCarAttr25);
            Log("AttID: 56740      -    " + carEntry.setCarAttr26);
            Log("AttID: 38      -    " + carEntry.setCarAttr27);
            Log("AttID: 50027      -    " + carEntry.setCarAttr28);
            Log("AttID: 50028      -    " + carEntry.setCarAttr29);
            Log("AttID: 50029      -    " + carEntry.setCarAttr30);
            Log("AttID: 10733      -    " + carEntry.setCarAttr31);
            Log("AttID: 50030      -    " + carEntry.setCarAttr32);
            Log("AttID: 10734      -    " + carEntry.setCarAttr33);
            Log("AttID: 50031      -    " + carEntry.setCarAttr34);
            Log("AttID: 33711      -    " + carEntry.setCarAttr35);
            Log("AttID: 25839      -    " + carEntry.setCarAttr36);
            Log("AttID: 25840      -    " + carEntry.setCarAttr37);
            Log("AttID: 25841      -    " + carEntry.setCarAttr38);
            Log("AttID: 25842      -    " + carEntry.setCarAttr39);
            Log("AttID: 25843      -    " + carEntry.setCarAttr40);
            Log("AttID: 50037      -    " + carEntry.setCarAttr41);
            Log("AttID: 25846      -    " + carEntry.setCarAttr42);
            Log("AttID: 25852      -    " + carEntry.setCarAttr43);
            Log("AttID: 10236      -    " + carEntry.setCarAttr44);
            Log("AttID: 25918      -    " + carEntry.setCarAttr45);
            Log("AttID: 25919      -    " + carEntry.setCarAttr46);
            Log("AttID: 26590      -    " + carEntry.setCarAttr47);
            Log("AttID: 26589      -    " + carEntry.setCarAttr48);
            Log("AttID: 26591      -    " + carEntry.setCarAttr49);
            Log("AttID: 31183      -    " + carEntry.setCarAttr50);
            Log("AttID: 27773      -    " + carEntry.setCarAttr51);
            Log("AttID: 26739      -    " + carEntry.setCarAttr52);
            */




        }
    
    }
}
