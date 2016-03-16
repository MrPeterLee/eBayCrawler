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

        private void DelDuplicateS4Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S4PanelData> listings = dbContext.S4PanelData;

            var query = from listing in dbContext.S4PanelData
                        group listing by listing.SearchID into g
                        where g.Count() > 1
                        select new
                        {
                            DuplicateItemID = g.Key,
                            DupSearchID = g.Min(listing => listing.SearchID)
                        };

            List<int> DupList = new List<int>();
            foreach (var listin in query)
            {
                DupList.Add(listin.DupSearchID);
            }

            foreach (int rowNum in DupList)
            {
                Log("Now deleting Search ID = " + rowNum.ToString() + ", in S4TableResult");
                DelS4Row(rowNum);
            }
        }

        private void DelS4Row(int IndexID)
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S4PanelData> listings = dbContext.S4PanelData;

            Log("The current IndexID is = " + IndexID.ToString());

            var query = from listing in dbContext.S4PanelData
                        where listing.SearchID == IndexID
                        select listing;

            foreach (var listin in query)
            {
                dbContext.DeleteObject(listin);
                Log(listin.SearchID.ToString() + " Search ID is being deleted.");
            }

            dbContext.SaveChanges();
        }

        private void reloadPanelDataGrid()
        {
            ObjectQuery<S4PanelData> listings = dataEntities.S4PanelData;

            var query = from listing in listings
                        orderby listing.ItemID
                        select new
                        {
                            listing.SearchID,
                            listing.ItemID,
                            listing.Title,
                            listing.SellerID,
                            listing.UserInfoFetched,
                            listing.FetchTime,
                            listing.StartTime,
                            listing.EndTime,
                            listing.Price,
                            listing.BuyItNowPrice,
                            listing.BidCount,
                            listing.SellingStatusBidderCount,
                            listing.PrivateListing,
                            listing.MotorsDealer,
                            listing.Quantity,
                            listing.QuantitySold,
                            listing.PrimaryCategory,
                            listing.PrimaryCategoryID,
                            listing.FeedBackScore,
                            listing.SellerStar,
                            listing.SellerFeedbackScore,
                            listing.SellerFeedbackRatingStar,
                            listing.SellerRegDate,
                            listing.SellerLevel,
                            listing.SellerPositiveFeedbackPercent,
                            listing.Site,
                            listing.PicNumber,
                            listing.UserIsNewReg,
                            listing.SellerVerified,
                            listing.SellerIDChanged,
                            listing.SellerStore,
                            listing.SellingStatusHighBidderUserID,
                            listing.BuyerProtection,
                            listing.BuyerResponsibleForShipping,
                            listing.BuyItNowPricecurrencyID,
                            listing.BuyerGuaranteePricecurrencyID,
                            listing.BuyerGuaranteePriceValue,
                            listing.CategoryBasedAttributesPrefill,
                            listing.CategoryMappingAllowed,
                            listing.CharityCharityID,
                            listing.ConditionID,
                            listing.Country,
                            listing.Currency,
                            listing.DescriptionReviseMode,
                            listing.DisableBuyerRequirements,
                            listing.DispatchTimeMax,
                            listing.GetItFast,
                            listing.GiftIcon,
                            listing.HitCount,
                            listing.HitCounter,
                            listing.IntegratedMerchantCreditCardEnabled,
                            listing.InventoryTrackingMethod,
                            listing.ItemCompatibilityCount,
                            listing.LeadCount,
                            listing.LimitedWarrantyEligible,
                            listing.ListingDesignerLayoutID,
                            listing.ListingDesignerThemeID,
                            listing.ListingDesignerOptimalPictureSize,
                            listing.ListingDetailsAdult,
                            listing.ListingDetailsBindingAuction,
                            listing.ListingDetailsBuyItNowAvailable,
                            listing.ListingDetailsCheckoutEnabled,
                            listing.ListingDetailsConvertedBuyItNowPriceValue,
                            listing.ListingDetailsConvertedStartPriceValue,
                            listing.ListingDetailsEndTime,
                            listing.ListingDetailsHasPublicMessages,
                            listing.ListingDetailsHasReservePrice,
                            listing.ListingDetailsHasUnansweredQuestions,
                            listing.ListingDetailsPayPerLeadEnabled,
                            listing.ListingDetailsSellerBusinessType,
                            listing.ListingDetailsStartTime,
                            listing.ListingDetailsViewItemURL,
                            listing.ListingDuration,
                            listing.ListingSubtype2,
                            listing.ListingType,
                            listing.LocalListing,
                            listing.LotSize,
                            listing.MechanicalCheckAccepted,
                            listing.MotorsGermanySearchable,
                            listing.NewLeadCount,
                            listing.PaymentDetailsDaysToFullPayment,
                            listing.PaymentDetailsHoursToDeposit,
                            listing.PostalCode,
                            listing.ProxyItem,
                            listing.QuantityAvailable,
                            listing.QuantitySpecified,
                            listing.QuestionCount,
                            listing.Relisted,
                            listing.RelistLink,
                            listing.ScheduleTime,
                            listing.SellerAboutMePage,
                            listing.SellerEnterpriseSeller,
                            listing.SellerIDVerified,
                            listing.SellerMotorsDealer,
                            listing.SellingStatusAdminEnded,
                            listing.SellingStatusBidCount,
                            listing.SellingStatusConvertedCurrentPriceValue,
                            listing.SellingStatusCurrentPriceValue,
                            listing.SellingStatusLeadCount,
                            listing.SellingStatusQuantitySold,
                            listing.SellingStatusReserveMet,
                            listing.SkypeEnabled,
                            listing.ThirdPartyCheckout,
                            listing.ThirdPartyCheckoutIntegration,
                            listing.TimeLeft,
                            listing.TotalQuestionCount,
                            listing.UpdateReturnPolicy,
                            listing.UpdateSellerInfo,
                            listing.UseTaxTable,
                            listing.WatchCount,
                            listing.WatchCountSpecified,
                            listing.ApplyShippingDiscount,
                            listing.AutoPay,
                            listing.BestOfferEnabled,
                            listing.ItemSpecific1,
                            listing.ItemSpecific2,
                            listing.ItemSpecific3,
                            listing.ItemSpecific4,
                            listing.ItemSpecific5,
                            listing.ItemSpecific6,
                            listing.ItemSpecific7,
                            listing.ItemSpecific8,
                            listing.ItemSpecific9,
                            listing.ItemSpecific10,
                            listing.ItemSpecific11,
                            listing.ItemSpecific12,
                            listing.ItemSpecific13,
                            listing.ItemSpecific14,
                            listing.ItemSpecific15,
                            listing.ItemSpecific16,
                            listing.ItemSpecific17,
                            listing.ItemSpecific18,
                            listing.ItemSpecific19,
                            listing.ItemSpecific20,
                            listing.ItemSpecific21,
                            listing.ItemSpecific22,
                            listing.ItemSpecific23,
                            listing.ItemSpecific24,
                            listing.ItemSpecific25,
                            listing.ItemSpecific26,
                            listing.ItemSpecific27,
                            listing.ItemSpecific28,
                            listing.ItemSpecific29,
                            listing.ItemSpecific30,
                            listing.ItemSpecific31,
                            listing.ItemSpecific32,
                            listing.ItemSpecific33,
                            listing.ItemSpecific34,
                            listing.ItemSpecific35,
                            listing.ItemSpecific36,
                            listing.ItemSpecific37,
                            listing.ItemSpecific38,
                            listing.ItemSpecific39,
                            listing.ItemSpecific40,
                            listing.ItemSpecific41,
                            listing.ItemSpecific42,
                            listing.ItemSpecific43,
                            listing.ItemSpecific44,
                            listing.ItemSpecific45,
                            listing.ItemSpecific46,
                            listing.ItemSpecific47,
                            listing.ItemSpecific48,
                            listing.ItemSpecific49,
                            listing.ItemSpecific50,
                            listing.ItemSpecific51,
                            listing.ItemSpecific52,
                            listing.ItemSpecific53,
                            listing.ItemSpecific54,
                            listing.ItemSpecific55,
                            listing.ItemSpecific56,
                            listing.ItemSpecific57,
                            listing.ItemSpecific58,
                            listing.ItemSpecific59,
                            listing.ItemSpecific60,
                            listing.ItemSpecific61,
                            listing.ItemSpecific62,
                            listing.ItemSpecific63,
                            listing.ItemSpecific64,
                            listing.ItemSpecific65,
                            listing.ItemSpecific66,
                            listing.ItemSpecific67,
                            listing.ItemSpecific68,
                            listing.ItemSpecific69,
                            listing.ItemSpecific70,
                            listing.ItemSpecific71,
                            listing.ItemSpecific72,
                            listing.ItemSpecific73,
                            listing.ItemSpecific74,
                            listing.ItemSpecific75,
                            listing.ItemSpecific76,
                            listing.ItemSpecific77,
                            listing.ItemSpecific78,
                            listing.ItemSpecific79,
                            listing.ItemSpecific80,
                            listing.ItemSpecific81,
                            listing.ItemSpecific82,
                            listing.ItemSpecific83,
                            listing.ItemSpecific84,
                            listing.ItemSpecific85,
                            listing.ItemSpecific86,
                            listing.ItemSpecific87,
                            listing.ItemSpecific88,
                            listing.ItemSpecific89,
                            listing.ItemSpecific90,
                            listing.ItemSpecific91,
                            listing.ItemSpecific92,
                            listing.ItemSpecific93,
                            listing.ItemSpecific94,
                            listing.ItemSpecific95,
                            listing.ItemSpecific96,
                            listing.ItemSpecific97,
                            listing.ItemSpecific98,
                            listing.ItemSpecific99,
                            listing.ItemSpecific100

                        };
            GridPanelData.ItemsSource = query.ToList();
        }

        private void DeleteS4Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            var query = from p in dbContext.S4PanelData
                        select p;
            foreach (S4PanelData p in query)
                dbContext.DeleteObject(p);
            dbContext.SaveChanges();
        }

        

    }
}
