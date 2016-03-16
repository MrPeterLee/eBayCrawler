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
using System.IO;

namespace eBayFetch
{

    public partial class MainWindow
    {
        List<string> defaultSearches = new List<string>();

        public void AutoFetchTimer_Tick1(object o1, EventArgs e)    // does search every 1000 seconds
        {
            TxtAutoFetchCountDown.Content = "Countdown to Next Fetch: " + AutoFetchTimerSecs.ToString() + " Secs";

            if (AutoFetchTimerSecs <= 0)
            {
                AutoFetchTimerSecs = Convert.ToInt32(TxtAutoFetchInterval.Text) * 60;

                // Start to do Searches
                addDefaultSearch();                     // Add categories IDs to default search list

                if (defaultSearches.Count > 0)
                {                    
                    GeteBayTime();

                    Log("Fetched eBay official system time.");

                    Log("***********************************************************************");
                    Log("  AutoFetch - Identify ended listings and fetch item, seller and bidder info");
                    Log("***********************************************************************");
                    // Start to compare Endtime and CurrentTime in S1Table 
                    eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
                    ObjectQuery<S1SearchTable> listings = dbContext.S1SearchTable;

                    // Identify those ended listings
                    var query = from listing in dbContext.S1SearchTable
                                where (EntityFunctions.AddHours(listing.FetchTime, 5) < DateTime.Now & listing.IsInPanelData == 0)  // add 5 hours in fetchtime
                                select listing;

                    bool successFetched;
                    foreach (var listin in query)
                    {
                        successFetched = false;

                        Log("ItemID " + listin.ItemID + " has Fetchtime plus 5 hours Less Than CurrentTime, proceed to Panel Database, and begin to modify isPanel dummy");

                        // Start to fetch item-information on the ended listings
                        successFetched = GetItemInfoFnS4(listin.ItemID);
                        //  DelS2Row(listin.SearchID);

                        if (successFetched == true)
                            ModifyIsPanelS1(listin.SearchID);
                    }

                    // Start to fetch Seller-information on the ended listings

                    int fetCount = CalSumFetchedUsers();
                    int obsCount = CalSumRows();
                    for (int i = fetCount; i < obsCount; i++)
                    {
                        QueryUpdateUserID();
                    }



                    // To refresh all data tables
                    reloadPreDataGrid();
                    reloadItemDataGrid();
                    reloadUserDataGrid();
                    reloadPanelDataGrid();

                    //DeleteS1Data();                         // clear S1 Table
                }
                else
                {                    
                    Log(" ERROR - PLEASE SPECIFY A SEARCH CATEGORY ID ");
                }

                // Switch to a different account
                //SwitchAccountFn();                        // Switch to different accounts
                //GetAPICallsNumber();                      // Refresh request count
            }
            else
            {
                AutoFetchTimerSecs = AutoFetchTimerSecs - 1;
            }
        }

        private void Stage1Fn()
        {
            Log("***********************************************************************");
            Log("    Stage 1: Conduct Sequential Listing Search base on Category IDs");
            Log("***********************************************************************");

            foreach (string defaultSearch in defaultSearches)
            {
                TxtSearchTerm.Text = defaultSearch;
                Log("Current search category is " + defaultSearch);

                GetCategoryListings(defaultSearch);
                DelDuplicateS1Data();
            }
            DelDuplicateS1Data();
        }
        
        private void Stage2Fn()
        {
            Log("***********************************************************************");
            Log("          Stage 2: Fetch Item Detail for All Search Results");
            Log("***********************************************************************");

            // Start to get Item detail for each of the search results
            List<string> ItemList = QuerySearchItemID();
            foreach (string iteList in ItemList)
            {
                Log("Search Listings Obtained for ItemID:" + iteList);
                GetItemInfoFn(iteList);     // duplicate entries are added now
            }

            // Remove duplicate entries in S2ListingData
            DelBeginDuplicateS2Data();    // Delete One duplicate entry which is fetched at beginning & isInPanel == 0  
            DelBeginDuplicateS2Data();    // Delete One duplicate entry which is fetched at beginning & isInPanel == 0  
            DelBeginDuplicateS2Data();    // Delete One duplicate entry which is fetched at beginning & isInPanel == 0  
            DelBeginDuplicateS2Data();    // Delete One duplicate entry which is fetched at beginning & isInPanel == 0  
            DelBeginDuplicateS2Data();    // Delete One duplicate entry which is fetched at beginning & isInPanel == 0  
            DelBeginDuplicateS2Data();    // Delete One duplicate entry which is fetched at beginning & isInPanel == 0  
            DelBeginDuplicateS2Data();    // Delete One duplicate entry which is fetched at beginning & isInPanel == 0  
            DelDuplicateS2Data();      //    Delete One duplicate entry which is fetched recently & isInPanel == 0 
        }


        private void ModifyIsPanelS1(int IndexID)
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S1SearchTable> listings = dbContext.S1SearchTable;

            Log("The current IndexID is = " + IndexID.ToString());

            var query = (from listing in dbContext.S1SearchTable
                         where listing.SearchID == IndexID
                         select listing).First();

            query.IsInPanelData = 1;
            dbContext.SaveChanges();
        }

        private void ModifyIsPanelS2(int IndexID)
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S2ItemInfo> listings = dbContext.S2ItemInfo;

            Log("The current IndexID is = " + IndexID.ToString());

            var query = (from listing in dbContext.S2ItemInfo
                         where listing.SearchID == IndexID
                         select listing).First();

            query.IsInPanelData = 1;
            dbContext.SaveChanges();
        }

        private int CalSumRows()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S4PanelData> listings = dbContext.S4PanelData;
            var query = from listing in dbContext.S4PanelData
                        select listing;
            var SumRows = query.Sum(User => User.Ones);
            Log("the Panel Data Obs Number is = " + SumRows.ToString());
            int SumRow = Convert.ToInt32(SumRows);
            return SumRow;
        }

        private int CalSumFetchedUsers()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S4PanelData> listings = dbContext.S4PanelData;
            var query = from listing in dbContext.S4PanelData
                        select listing;
            var SumFetchedUsers = query.Sum(User => User.UserInfoFetched);
            Log("the UserInfoFetched Column Sum is = " + SumFetchedUsers.ToString());
            int SumFetchedUser = Convert.ToInt32(SumFetchedUsers);
            return SumFetchedUser;

        }

        private void getBidderHistory(string ItemString)
        {

            // create a file to write to
            string path = @BidderHistoryFile.Text;
            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("ItemID, BidderID, FeedbackScore, CurrencyID, MaxBidValue, TimeBid, TotalBids, ConvertPrice, ConvertPriceCurrency, Quantity, UserAboutMePage");
                }
            }


            Context.CallRetry = GetCallRetry();
            GetAllBiddersCall oGetAllBiddersCall = new GetAllBiddersCall(GetContext());
            // enable the compression feature
            oGetAllBiddersCall.EnableCompression = true;
            // replace xxx with your ItemID
            oGetAllBiddersCall.ItemID = ItemString;
            oGetAllBiddersCall.IncludeBiddingSummary = true;
            // CallMode is a very critical setting
            // set ViewAll to return all bidders for an ended or active listing
            // set EndedListing to return all non-winning bidders for an ended listing
            // set SecondChanceEligibleEndedListing to return all non-winning bidders
            //   for an ended listing who have not yet received a Second Chance Offer.
            // EndedListing and SecondChanceEligibleEndedListing can only be used by the seller
            oGetAllBiddersCall.CallMode = GetAllBiddersModeCodeType.ViewAll;
            // 
			try
			{
				OfferTypeCollection oOffers = oGetAllBiddersCall.GetAllBidders(oGetAllBiddersCall.ItemID, oGetAllBiddersCall.CallMode);
				// output some of the data
				foreach (OfferType oOffer in oOffers)
				{
					Log("Bidder is " + oOffer.User.UserID);
					Log("Bidders feedback score is " + oOffer.User.FeedbackScore.ToString());
                    Log("The MaxBid is " + oOffer.MaxBid.currencyID.ToString() + " " + oOffer.MaxBid.Value.ToString());
                    Log("The bid was made on " + oOffer.TimeBid.ToLongDateString() + " at " + oOffer.TimeBid.ToLongTimeString());
                    Log("");
					// the data that is accessible through the Offer object
					// can be found at the following URL:
					// http://developer.ebay.com/DevZone/SOAP/docs/Reference/eBay/io_GetAllBidders.html#Response

                    // This text is always added, making the file longer over time 
                    // if it is not deleted. 
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(ItemString + "," +
                                    oOffer.User.UserID.ToString() + "," + 
                                    oOffer.User.FeedbackScore.ToString() + "," +
                                    oOffer.MaxBid.currencyID.ToString() + "," +
                                    oOffer.MaxBid.Value.ToString() + "," +
                                    oOffer.TimeBid.ToString() + "," +
                                    oOffer.User.BiddingSummary.TotalBids.ToString() + "," +
                                    oOffer.ConvertedPrice.Value.ToString() + "," +
                                    oOffer.ConvertedPrice.currencyID.ToString()  + "," +
                                    oOffer.Quantity.ToString()   + "," +
                                    oOffer.User.AboutMePage.ToString()  );
                    }




                    #region Bids History
                    // Get item specifics
                    Log("Now Getting bid history...");
                    Log("The number of itemspecifics is: " + oOffer.User.BiddingSummary.TotalBids.ToString());

                    ItemBidDetailsTypeCollection attrSets = oOffer.User.BiddingSummary.ItemBidDetails;
                    createBidsFile("ItemID-+|+-BidCount-+|+-LastBidTime-+|+-SellerID-+|+-UserID", "BidsHistory");
                    foreach (ItemBidDetailsType attribute in attrSets)
                    {
                        string addNewContent = ItemString;
                        Log("Now getting individual bid details...");
                        addNewContent = addNewContent + "-+|+-" + attribute.BidCount.ToString();
                        addNewContent = addNewContent + "-+|+-" + attribute.LastBidTime.ToString();
                        addNewContent = addNewContent + "-+|+-" + attribute.SellerID.ToString();
                        addNewContent = addNewContent + "-+|+-" + oOffer.User.UserID.ToString();

                        writeToBidsFile(addNewContent, "BidsHistory");
                        
                    }

                    #endregion
				}



                Log("Bidder History has been successfully fetched");
			}
            catch (ApiException oApiEx)
            {
                // process exception ... pass to caller, implement retry logic here or in caller, whatever you want to do
                Log(oApiEx.Message);
                return;
            }
            catch (SdkException oSdkEx)
            {
                // process exception ... pass to caller, implement retry logic here or in caller, whatever you want to do
                Log(oSdkEx.Message);
                return;
            }
            catch (Exception oEx)
            {
                // process exception ... pass to caller, implement retry logic here or in caller, whatever you want to do
                Log(oEx.Message);
                return;
            }
        }

        private void createFile(string content, string fileName)
        {
            // create a file to write to
            pathItem = FetchedListings.Text + fileName + ".txt";
            // This text is added only once to the file. 
            if (!File.Exists(pathItem))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(pathItem))
                {
                    sw.WriteLine(content);
                }
            }
        }

        private void writeToFile(string content, string fileName)
        {
            // create a file to write to
            pathItem = FetchedListings.Text + fileName + ".txt";
            // This text is added only once to the file. 
            using (StreamWriter sw = File.AppendText(pathItem))
            {
                sw.WriteLine(content);
            }
        }

        private void createSpecsFile(string content, string fileName)
        {
            // create a file to write to
            pathItem = ItemSpecifics.Text + fileName + ".txt";
            // This text is added only once to the file. 
            if (!File.Exists(pathItem))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(pathItem))
                {
                    sw.WriteLine(content);
                }
            }
        }

        private void writeToSpecsFile(string content, string fileName)
        {
            // create a file to write to
            pathItem = ItemSpecifics.Text + fileName + ".txt";
            // This text is added only once to the file. 
            using (StreamWriter sw = File.AppendText(pathItem))
            {
                sw.WriteLine(content);
            }
        }

        private void createBidsFile(string content, string fileName)
        {
            // create a file to write to
            pathItem = ItemBids.Text + fileName + ".txt";
            // This text is added only once to the file. 
            if (!File.Exists(pathItem))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(pathItem))
                {
                    sw.WriteLine(content);
                }
            }
        }

        private void writeToBidsFile(string content, string fileName)
        {
            // create a file to write to
            pathItem = ItemBids.Text + fileName + ".txt";
            // This text is added only once to the file. 
            using (StreamWriter sw = File.AppendText(pathItem))
            {
                sw.WriteLine(content);
            }
        }

        private bool GetItemInfoFnS4(string ItemString)
        {
            try
            {
                Log("in sub fn ItemID String received is:" + ItemString);
                Context.CallRetry = GetCallRetry();

                GetItemCall apicall = new GetItemCall(GetContext());
                apicall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
                apicall.IncludeItemSpecifics = true;
                // apicall.IncludeWatchCount = true; // you must be the seller to retreive the watch count
                fetchedItem = apicall.GetItem(ItemString);

                Log("Now updating ItemID in PanelData = " + fetchedItem.ItemID.ToString() + ", SellerID = " + fetchedItem.Seller.UserID.ToString() + ", Title = " + fetchedItem.Title.ToString());

                pictureNumber = 0;
                if (fetchedItem.PictureDetails != null)
                {
                    StringCollection PictureURLs = fetchedItem.PictureDetails.PictureURL;
                    for (int i = 0; PictureURLs != null && i < PictureURLs.Count; i++)
                    {
                        pictureNumber += 1;
                    }
                }


                string[] ListItemDetail = new string[500];

                #region Info Contents

                // Update variables
                int fileNumber = 1;

                if (fetchedItem != null) { createFile("ItemID-+|+-pictureNumber", "pictureNumber"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + pictureNumber.ToString(), "pictureNumber"); fileNumber++; } 




if (fetchedItem != null) { createFile("ItemID-+|+-ApplicationData", "ApplicationData"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ApplicationData, "ApplicationData"); fileNumber++; } 

if (fetchedItem.ApplyBuyerProtection != null) { createFile("ItemID-+|+-ApplyBuyerProtectionBuyerProtectionSource", "ApplyBuyerProtectionBuyerProtectionSource"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ApplyBuyerProtection.BuyerProtectionSource, "ApplyBuyerProtectionBuyerProtectionSource"); fileNumber++; } 
if (fetchedItem.ApplyBuyerProtection != null) { createFile("ItemID-+|+-ApplyBuyerProtectionBuyerProtectionStatus", "ApplyBuyerProtectionBuyerProtectionStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ApplyBuyerProtection.BuyerProtectionStatus, "ApplyBuyerProtectionBuyerProtectionStatus"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-AttributeSetArray", "AttributeSetArray"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.AttributeSetArray, "AttributeSetArray"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-AutoPay", "AutoPay"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.AutoPay, "AutoPay"); fileNumber++; } 

if (fetchedItem.BestOfferDetails != null) { createFile("ItemID-+|+-BestOfferDetailsBestOfferCount", "BestOfferDetailsBestOfferCount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BestOfferDetails.BestOfferCount, "BestOfferDetailsBestOfferCount"); fileNumber++; } 
if (fetchedItem.BestOfferDetails != null) { createFile("ItemID-+|+-BestOfferDetailsBestOfferEnabled", "BestOfferDetailsBestOfferEnabled"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BestOfferDetails.BestOfferEnabled, "BestOfferDetailsBestOfferEnabled"); fileNumber++; } 
if (fetchedItem.BestOfferDetails != null) { createFile("ItemID-+|+-BestOfferDetailsNewBestOffer", "BestOfferDetailsNewBestOffer"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BestOfferDetails.NewBestOffer, "BestOfferDetailsNewBestOffer"); fileNumber++; } 


if (fetchedItem.BusinessSellerDetails != null) { createFile("ItemID-+|+-BusinessSellerDetailsAdditionalContactInformation", "BusinessSellerDetailsAdditionalContactInformation"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BusinessSellerDetails.AdditionalContactInformation, "BusinessSellerDetailsAdditionalContactInformation"); fileNumber++; } 




if (fetchedItem.BusinessSellerDetails != null) { createFile("ItemID-+|+-BusinessSellerDetailsEmail", "BusinessSellerDetailsEmail"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BusinessSellerDetails.Email, "BusinessSellerDetailsEmail"); fileNumber++; } 
if (fetchedItem.BusinessSellerDetails != null) { createFile("ItemID-+|+-BusinessSellerDetailsFax", "BusinessSellerDetailsFax"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BusinessSellerDetails.Fax, "BusinessSellerDetailsFax"); fileNumber++; } 
if (fetchedItem.BusinessSellerDetails != null) { createFile("ItemID-+|+-BusinessSellerDetailsLegalInvoice", "BusinessSellerDetailsLegalInvoice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BusinessSellerDetails.LegalInvoice, "BusinessSellerDetailsLegalInvoice"); fileNumber++; } 
if (fetchedItem.BusinessSellerDetails != null) { createFile("ItemID-+|+-BusinessSellerDetailsTermsAndConditions", "BusinessSellerDetailsTermsAndConditions"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BusinessSellerDetails.TermsAndConditions, "BusinessSellerDetailsTermsAndConditions"); fileNumber++; } 
if (fetchedItem.BusinessSellerDetails != null) { createFile("ItemID-+|+-BusinessSellerDetailsTradeRegistrationNumber", "BusinessSellerDetailsTradeRegistrationNumber"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BusinessSellerDetails.TradeRegistrationNumber, "BusinessSellerDetailsTradeRegistrationNumber"); fileNumber++; } 








if (fetchedItem != null) { createFile("ItemID-+|+-BuyerGuaranteePrice", "BuyerGuaranteePrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyerGuaranteePrice.Value.ToString(), "BuyerGuaranteePrice"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-BuyerProtection", "BuyerProtection"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyerProtection, "BuyerProtection"); fileNumber++; } 

if (fetchedItem.BuyerRequirementDetails != null) { createFile("ItemID-+|+-BuyerRequirementDetailsLinkedPayPalAccount", "BuyerRequirementDetailsLinkedPayPalAccount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyerRequirementDetails.LinkedPayPalAccount, "BuyerRequirementDetailsLinkedPayPalAccount"); fileNumber++; } 












if (fetchedItem.BuyerRequirementDetails != null) { createFile("ItemID-+|+-BuyerRequirementDetailsMinimumFeedbackScore", "BuyerRequirementDetailsMinimumFeedbackScore"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyerRequirementDetails.MinimumFeedbackScore, "BuyerRequirementDetailsMinimumFeedbackScore"); fileNumber++; } 
if (fetchedItem.BuyerRequirementDetails != null) { createFile("ItemID-+|+-BuyerRequirementDetailsShipToRegistrationCountry", "BuyerRequirementDetailsShipToRegistrationCountry"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyerRequirementDetails.ShipToRegistrationCountry, "BuyerRequirementDetailsShipToRegistrationCountry"); fileNumber++; } 




if (fetchedItem.BuyerRequirementDetails != null) { createFile("ItemID-+|+-BuyerRequirementDetailsZeroFeedbackScore", "BuyerRequirementDetailsZeroFeedbackScore"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyerRequirementDetails.ZeroFeedbackScore, "BuyerRequirementDetailsZeroFeedbackScore"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-BuyerResponsibleForShipping", "BuyerResponsibleForShipping"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyerResponsibleForShipping, "BuyerResponsibleForShipping"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-BuyItNowPrice", "BuyItNowPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.BuyItNowPrice.Value.ToString(), "BuyItNowPrice"); fileNumber++; } 

if (fetchedItem.Charity != null) { createFile("ItemID-+|+-CharityCharityID", "CharityCharityID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Charity.CharityID, "CharityCharityID"); fileNumber++; } 
if (fetchedItem.Charity != null) { createFile("ItemID-+|+-CharityCharityName", "CharityCharityName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Charity.CharityName, "CharityCharityName"); fileNumber++; } 
if (fetchedItem.Charity != null) { createFile("ItemID-+|+-CharityCharityNumber", "CharityCharityNumber"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Charity.CharityNumber, "CharityCharityNumber"); fileNumber++; } 
if (fetchedItem.Charity != null) { createFile("ItemID-+|+-CharityDonationPercent", "CharityDonationPercent"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Charity.DonationPercent, "CharityDonationPercent"); fileNumber++; } 
if (fetchedItem.Charity != null) { createFile("ItemID-+|+-CharityLogoURL", "CharityLogoURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Charity.LogoURL, "CharityLogoURL"); fileNumber++; } 
if (fetchedItem.Charity != null) { createFile("ItemID-+|+-CharityMission", "CharityMission"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Charity.Mission, "CharityMission"); fileNumber++; } 
if (fetchedItem.Charity != null) { createFile("ItemID-+|+-CharityStatus", "CharityStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Charity.Status, "CharityStatus"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-ConditionDescription", "ConditionDescription"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ConditionDescription, "ConditionDescription"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-ConditionDisplayName", "ConditionDisplayName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ConditionDisplayName, "ConditionDisplayName"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-ConditionID", "ConditionID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ConditionID, "ConditionID"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-Country", "Country"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Country, "Country"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-CrossBorderTrade", "CrossBorderTrade"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.CrossBorderTrade, "CrossBorderTrade"); fileNumber++; } 


if (fetchedItem.CrossPromotion != null) { createFile("ItemID-+|+-CrossPromotionItemID", "CrossPromotionItemID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.CrossPromotion.ItemID, "CrossPromotionItemID"); fileNumber++; } 
if (fetchedItem.CrossPromotion != null) { createFile("ItemID-+|+-CrossPromotionPrimaryScheme", "CrossPromotionPrimaryScheme"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.CrossPromotion.PrimaryScheme, "CrossPromotionPrimaryScheme"); fileNumber++; } 
if (fetchedItem.CrossPromotion != null) { createFile("ItemID-+|+-CrossPromotionPromotedItem", "CrossPromotionPromotedItem"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.CrossPromotion.PromotedItem, "CrossPromotionPromotedItem"); fileNumber++; } 

if (fetchedItem.CrossPromotion != null) { createFile("ItemID-+|+-CrossPromotionPromotionMethod", "CrossPromotionPromotionMethod"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.CrossPromotion.PromotionMethod, "CrossPromotionPromotionMethod"); fileNumber++; } 
if (fetchedItem.CrossPromotion != null) { createFile("ItemID-+|+-CrossPromotionSellerID", "CrossPromotionSellerID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.CrossPromotion.SellerID, "CrossPromotionSellerID"); fileNumber++; } 
if (fetchedItem.CrossPromotion != null) { createFile("ItemID-+|+-CrossPromotionShippingDiscount", "CrossPromotionShippingDiscount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.CrossPromotion.ShippingDiscount, "CrossPromotionShippingDiscount"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-Currency", "Currency"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Currency, "Currency"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-DisableBuyerRequirements", "DisableBuyerRequirements"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DisableBuyerRequirements, "DisableBuyerRequirements"); fileNumber++; } 

if (fetchedItem.DiscountPriceInfo != null) { createFile("ItemID-+|+-DiscountPriceInfoMadeForOutletComparisonPrice", "DiscountPriceInfoMadeForOutletComparisonPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DiscountPriceInfo.MadeForOutletComparisonPrice, "DiscountPriceInfoMadeForOutletComparisonPrice"); fileNumber++; } 
if (fetchedItem.DiscountPriceInfo != null) { createFile("ItemID-+|+-DiscountPriceInfoMinimumAdvertisedPrice", "DiscountPriceInfoMinimumAdvertisedPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DiscountPriceInfo.MinimumAdvertisedPrice, "DiscountPriceInfoMinimumAdvertisedPrice"); fileNumber++; } 
if (fetchedItem.DiscountPriceInfo != null) { createFile("ItemID-+|+-DiscountPriceInfoMinimumAdvertisedPriceExposure", "DiscountPriceInfoMinimumAdvertisedPriceExposure"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DiscountPriceInfo.MinimumAdvertisedPriceExposure, "DiscountPriceInfoMinimumAdvertisedPriceExposure"); fileNumber++; } 
if (fetchedItem.DiscountPriceInfo != null) { createFile("ItemID-+|+-DiscountPriceInfoOriginalRetailPrice", "DiscountPriceInfoOriginalRetailPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DiscountPriceInfo.OriginalRetailPrice, "DiscountPriceInfoOriginalRetailPrice"); fileNumber++; } 
if (fetchedItem.DiscountPriceInfo != null) { createFile("ItemID-+|+-DiscountPriceInfoPricingTreatment", "DiscountPriceInfoPricingTreatment"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DiscountPriceInfo.PricingTreatment, "DiscountPriceInfoPricingTreatment"); fileNumber++; } 
if (fetchedItem.DiscountPriceInfo != null) { createFile("ItemID-+|+-DiscountPriceInfoSoldOffeBay", "DiscountPriceInfoSoldOffeBay"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DiscountPriceInfo.SoldOffeBay, "DiscountPriceInfoSoldOffeBay"); fileNumber++; } 
if (fetchedItem.DiscountPriceInfo != null) { createFile("ItemID-+|+-DiscountPriceInfoSoldOneBay", "DiscountPriceInfoSoldOneBay"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DiscountPriceInfo.SoldOneBay, "DiscountPriceInfoSoldOneBay"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-DispatchTimeMax", "DispatchTimeMax"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.DispatchTimeMax, "DispatchTimeMax"); fileNumber++; } 


if (fetchedItem.ExtendedSellerContactDetails != null) { createFile("ItemID-+|+-ExtendedSellerContactDetailsClassifiedAdContactByEmailEnabled", "ExtendedSellerContactDetailsClassifiedAdContactByEmailEnabled"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ExtendedSellerContactDetails.ClassifiedAdContactByEmailEnabled, "ExtendedSellerContactDetailsClassifiedAdContactByEmailEnabled"); fileNumber++; } 













if (fetchedItem.FreeAddedCategory != null) { createFile("ItemID-+|+-FreeAddedCategoryCategoryID", "FreeAddedCategoryCategoryID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.FreeAddedCategory.CategoryID, "FreeAddedCategoryCategoryID"); fileNumber++; } 
if (fetchedItem.FreeAddedCategory != null) { createFile("ItemID-+|+-FreeAddedCategoryCategoryName", "FreeAddedCategoryCategoryName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.FreeAddedCategory.CategoryName, "FreeAddedCategoryCategoryName"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-GetItFast", "GetItFast"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.GetItFast, "GetItFast"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-GiftIcon", "GiftIcon"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.GiftIcon, "GiftIcon"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-GiftServices", "GiftServices"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.GiftServices, "GiftServices"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-HideFromSearch", "HideFromSearch"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.HideFromSearch, "HideFromSearch"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-HitCount", "HitCount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.HitCount.ToString(), "HitCount"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-HitCounter", "HitCounter"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.HitCounter, "HitCounter"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-IntegratedMerchantCreditCardEnabled", "IntegratedMerchantCreditCardEnabled"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.IntegratedMerchantCreditCardEnabled, "IntegratedMerchantCreditCardEnabled"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-InventoryTrackingMethod", "InventoryTrackingMethod"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.InventoryTrackingMethod, "InventoryTrackingMethod"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-IsIntermediatedShippingEligible", "IsIntermediatedShippingEligible"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.IsIntermediatedShippingEligible, "IsIntermediatedShippingEligible"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-ItemCompatibilityCount", "ItemCompatibilityCount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ItemCompatibilityCount, "ItemCompatibilityCount"); fileNumber++; } 













if (fetchedItem != null) { createFile("ItemID-+|+-ItemID", "ItemID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ItemID, "ItemID"); fileNumber++; } 

if (fetchedItem.ItemPolicyViolation != null) { createFile("ItemID-+|+-ItemPolicyViolationPolicyID", "ItemPolicyViolationPolicyID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ItemPolicyViolation.PolicyID, "ItemPolicyViolationPolicyID"); fileNumber++; } 
if (fetchedItem.ItemPolicyViolation != null) { createFile("ItemID-+|+-ItemPolicyViolationPolicyText", "ItemPolicyViolationPolicyText"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ItemPolicyViolation.PolicyText, "ItemPolicyViolationPolicyText"); fileNumber++; } 











if (fetchedItem.ListingCheckoutRedirectPreference != null) { createFile("ItemID-+|+-ListingCheckoutRedirectPreferenceProStoresStoreName", "ListingCheckoutRedirectPreferenceProStoresStoreName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingCheckoutRedirectPreference.ProStoresStoreName, "ListingCheckoutRedirectPreferenceProStoresStoreName"); fileNumber++; } 
if (fetchedItem.ListingCheckoutRedirectPreference != null) { createFile("ItemID-+|+-ListingCheckoutRedirectPreferenceSellerThirdPartyUsername", "ListingCheckoutRedirectPreferenceSellerThirdPartyUsername"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingCheckoutRedirectPreference.SellerThirdPartyUsername, "ListingCheckoutRedirectPreferenceSellerThirdPartyUsername"); fileNumber++; } 


if (fetchedItem.ListingDesigner != null) { createFile("ItemID-+|+-ListingDesignerLayoutID", "ListingDesignerLayoutID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDesigner.LayoutID, "ListingDesignerLayoutID"); fileNumber++; } 
if (fetchedItem.ListingDesigner != null) { createFile("ItemID-+|+-ListingDesignerOptimalPictureSize", "ListingDesignerOptimalPictureSize"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDesigner.OptimalPictureSize, "ListingDesignerOptimalPictureSize"); fileNumber++; } 
if (fetchedItem.ListingDesigner != null) { createFile("ItemID-+|+-ListingDesignerThemeID", "ListingDesignerThemeID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDesigner.ThemeID, "ListingDesignerThemeID"); fileNumber++; } 


if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsAdult", "ListingDetailsAdult"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.Adult, "ListingDetailsAdult"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsBestOfferAutoAcceptPrice", "ListingDetailsBestOfferAutoAcceptPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.BestOfferAutoAcceptPrice, "ListingDetailsBestOfferAutoAcceptPrice"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsBindingAuction", "ListingDetailsBindingAuction"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.BindingAuction, "ListingDetailsBindingAuction"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsBuyItNowAvailable", "ListingDetailsBuyItNowAvailable"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.BuyItNowAvailable, "ListingDetailsBuyItNowAvailable"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsCheckoutEnabled", "ListingDetailsCheckoutEnabled"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.CheckoutEnabled, "ListingDetailsCheckoutEnabled"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsConvertedBuyItNowPrice", "ListingDetailsConvertedBuyItNowPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.ConvertedBuyItNowPrice.Value.ToString(), "ListingDetailsConvertedBuyItNowPrice"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsConvertedReservePrice", "ListingDetailsConvertedReservePrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.ConvertedReservePrice, "ListingDetailsConvertedReservePrice"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsConvertedStartPrice", "ListingDetailsConvertedStartPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.ConvertedStartPrice.Value.ToString(), "ListingDetailsConvertedStartPrice"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsEndingReason", "ListingDetailsEndingReason"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.EndingReason, "ListingDetailsEndingReason"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsEndTime", "ListingDetailsEndTime"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.EndTime, "ListingDetailsEndTime"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsHasPublicMessages", "ListingDetailsHasPublicMessages"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.HasPublicMessages, "ListingDetailsHasPublicMessages"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsHasReservePrice", "ListingDetailsHasReservePrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.HasReservePrice, "ListingDetailsHasReservePrice"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsHasUnansweredQuestions", "ListingDetailsHasUnansweredQuestions"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.HasUnansweredQuestions, "ListingDetailsHasUnansweredQuestions"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsMinimumBestOfferPrice", "ListingDetailsMinimumBestOfferPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.MinimumBestOfferPrice, "ListingDetailsMinimumBestOfferPrice"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsRelistedItemID", "ListingDetailsRelistedItemID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.RelistedItemID, "ListingDetailsRelistedItemID"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsSecondChanceOriginalItemID", "ListingDetailsSecondChanceOriginalItemID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.SecondChanceOriginalItemID, "ListingDetailsSecondChanceOriginalItemID"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsStartTime", "ListingDetailsStartTime"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.StartTime, "ListingDetailsStartTime"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsTCROriginalItemID", "ListingDetailsTCROriginalItemID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.TCROriginalItemID, "ListingDetailsTCROriginalItemID"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsViewItemURL", "ListingDetailsViewItemURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.ViewItemURL, "ListingDetailsViewItemURL"); fileNumber++; } 
if (fetchedItem.ListingDetails != null) { createFile("ItemID-+|+-ListingDetailsViewItemURLForNaturalSearch", "ListingDetailsViewItemURLForNaturalSearch"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDetails.ViewItemURLForNaturalSearch, "ListingDetailsViewItemURLForNaturalSearch"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-ListingDuration", "ListingDuration"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingDuration, "ListingDuration"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-ListingEnhancement", "ListingEnhancement"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingEnhancement, "ListingEnhancement"); fileNumber++; } 


if (fetchedItem != null) { createFile("ItemID-+|+-ListingType", "ListingType"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ListingType, "ListingType"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-Location", "Location"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Location, "Location"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-LocationDefaulted", "LocationDefaulted"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.LocationDefaulted, "LocationDefaulted"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-LotSize", "LotSize"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.LotSize, "LotSize"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-MechanicalCheckAccepted", "MechanicalCheckAccepted"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.MechanicalCheckAccepted, "MechanicalCheckAccepted"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-MotorsGermanySearchable", "MotorsGermanySearchable"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.MotorsGermanySearchable, "MotorsGermanySearchable"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-OutOfStockControl", "OutOfStockControl"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.OutOfStockControl, "OutOfStockControl"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-PaymentAllowedSite", "PaymentAllowedSite"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentAllowedSite, "PaymentAllowedSite"); fileNumber++; } 


if (fetchedItem.PaymentDetails != null) { createFile("ItemID-+|+-PaymentDetailsDaysToFullPayment", "PaymentDetailsDaysToFullPayment"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentDetails.DaysToFullPayment, "PaymentDetailsDaysToFullPayment"); fileNumber++; } 
if (fetchedItem.PaymentDetails != null) { createFile("ItemID-+|+-PaymentDetailsDepositAmount", "PaymentDetailsDepositAmount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentDetails.DepositAmount, "PaymentDetailsDepositAmount"); fileNumber++; } 
if (fetchedItem.PaymentDetails != null) { createFile("ItemID-+|+-PaymentDetailsDepositType", "PaymentDetailsDepositType"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentDetails.DepositType, "PaymentDetailsDepositType"); fileNumber++; } 
if (fetchedItem.PaymentDetails != null) { createFile("ItemID-+|+-PaymentDetailsHoursToDeposit", "PaymentDetailsHoursToDeposit"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentDetails.HoursToDeposit, "PaymentDetailsHoursToDeposit"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-PaymentMethods", "PaymentMethods"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentMethods, "PaymentMethods"); fileNumber++; }

// Save individual items of Paymentmethods
if (fetchedItem != null) 
{
    var payMethods = fetchedItem.PaymentMethods.ToArray();
    for (int i = 0; payMethods != null & i < fetchedItem.PaymentMethods.Count; i++)
    {
        createFile("ItemID-+|+-PayMethod" + i, "PayMethod" + i);
        writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + payMethods[i].ToString(), "PayMethod" + i);
        fileNumber++; 
    }    
} 

// Save deposit amount
if (fetchedItem.PaymentDetails != null) {
    createFile("ItemID-+|+-PayDepositAmount", "PayDepositAmount");
    writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentDetails.DepositAmount.Value.ToString(), "PayDepositAmount"); 
    fileNumber++; 
}

// Save hours to deposit
if (fetchedItem.PaymentDetails != null)
{
    createFile("ItemID-+|+-PayHoursToDeposit", "PayHoursToDeposit");
    writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PaymentDetails.HoursToDeposit.ToString(), "PayHoursToDeposit");
    fileNumber++;
}

if (fetchedItem != null) { createFile("ItemID-+|+-PayPalEmailAddress", "PayPalEmailAddress"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PayPalEmailAddress, "PayPalEmailAddress"); fileNumber++; } 

if (fetchedItem.PickupInStoreDetails != null) { createFile("ItemID-+|+-PickupInStoreDetailsEligibleForPickupInStore", "PickupInStoreDetailsEligibleForPickupInStore"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PickupInStoreDetails.EligibleForPickupInStore, "PickupInStoreDetailsEligibleForPickupInStore"); fileNumber++; } 


if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsExternalPictureURL", "PictureDetailsExternalPictureURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.ExternalPictureURL, "PictureDetailsExternalPictureURL"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsGalleryDuration", "PictureDetailsGalleryDuration"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.GalleryDuration, "PictureDetailsGalleryDuration"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsGalleryErrorInfo", "PictureDetailsGalleryErrorInfo"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.GalleryErrorInfo, "PictureDetailsGalleryErrorInfo"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsGalleryStatus", "PictureDetailsGalleryStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.GalleryStatus, "PictureDetailsGalleryStatus"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsGalleryType", "PictureDetailsGalleryType"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.GalleryType, "PictureDetailsGalleryType"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsGalleryURL", "PictureDetailsGalleryURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.GalleryURL, "PictureDetailsGalleryURL"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsPhotoDisplay", "PictureDetailsPhotoDisplay"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.PhotoDisplay, "PictureDetailsPhotoDisplay"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsPictureSource", "PictureDetailsPictureSource"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.PictureSource, "PictureDetailsPictureSource"); fileNumber++; } 
if (fetchedItem.PictureDetails != null) { createFile("ItemID-+|+-PictureDetailsPictureURL", "PictureDetailsPictureURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PictureDetails.PictureURL, "PictureDetailsPictureURL"); fileNumber++; } 


if (fetchedItem != null) { createFile("ItemID-+|+-PostalCode", "PostalCode"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PostalCode, "PostalCode"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-PostCheckoutExperienceEnabled", "PostCheckoutExperienceEnabled"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PostCheckoutExperienceEnabled, "PostCheckoutExperienceEnabled"); fileNumber++; } 

if (fetchedItem.PrimaryCategory != null) { createFile("ItemID-+|+-PrimaryCategoryCategoryID", "PrimaryCategoryCategoryID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PrimaryCategory.CategoryID, "PrimaryCategoryCategoryID"); fileNumber++; } 
if (fetchedItem.PrimaryCategory != null) { createFile("ItemID-+|+-PrimaryCategoryCategoryName", "PrimaryCategoryCategoryName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PrimaryCategory.CategoryName, "PrimaryCategoryCategoryName"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-PrivateListing", "PrivateListing"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.PrivateListing, "PrivateListing"); fileNumber++; } 





if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsCopyright", "ProductListingDetailsCopyright"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.Copyright, "ProductListingDetailsCopyright"); fileNumber++; } 

if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsEAN", "ProductListingDetailsEAN"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.EAN, "ProductListingDetailsEAN"); fileNumber++; } 
if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsIncludePrefilledItemInformation", "ProductListingDetailsIncludePrefilledItemInformation"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.IncludePrefilledItemInformation, "ProductListingDetailsIncludePrefilledItemInformation"); fileNumber++; } 
if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsIncludeStockPhotoURL", "ProductListingDetailsIncludeStockPhotoURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.IncludeStockPhotoURL, "ProductListingDetailsIncludeStockPhotoURL"); fileNumber++; } 
if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsISBN", "ProductListingDetailsISBN"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.ISBN, "ProductListingDetailsISBN"); fileNumber++; } 
if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsProductID", "ProductListingDetailsProductID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.ProductID, "ProductListingDetailsProductID"); fileNumber++; } 
if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsStockPhotoURL", "ProductListingDetailsStockPhotoURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.StockPhotoURL, "ProductListingDetailsStockPhotoURL"); fileNumber++; } 
if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsUPC", "ProductListingDetailsUPC"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.UPC, "ProductListingDetailsUPC"); fileNumber++; } 
if (fetchedItem.ProductListingDetails != null) { createFile("ItemID-+|+-ProductListingDetailsUseStockPhotoURLAsGallery", "ProductListingDetailsUseStockPhotoURLAsGallery"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProductListingDetails.UseStockPhotoURLAsGallery, "ProductListingDetailsUseStockPhotoURLAsGallery"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-ProxyItem", "ProxyItem"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ProxyItem, "ProxyItem"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-Quantity", "Quantity"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Quantity, "Quantity"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-QuantityAvailableHint", "QuantityAvailableHint"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.QuantityAvailableHint, "QuantityAvailableHint"); fileNumber++; } 

if (fetchedItem.QuantityInfo != null) { createFile("ItemID-+|+-QuantityInfoMinimumRemnantSet", "QuantityInfoMinimumRemnantSet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.QuantityInfo.MinimumRemnantSet, "QuantityInfoMinimumRemnantSet"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-QuantityThreshold", "QuantityThreshold"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.QuantityThreshold, "QuantityThreshold"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-ReasonHideFromSearch", "ReasonHideFromSearch"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReasonHideFromSearch, "ReasonHideFromSearch"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-RelistParentID", "RelistParentID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.RelistParentID, "RelistParentID"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-ReservePrice", "ReservePrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReservePrice, "ReservePrice"); fileNumber++; } 

if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyDescription", "ReturnPolicyDescription"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.Description, "ReturnPolicyDescription"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyEAN", "ReturnPolicyEAN"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.EAN, "ReturnPolicyEAN"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyRefund", "ReturnPolicyRefund"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.Refund, "ReturnPolicyRefund"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyRefundOption", "ReturnPolicyRefundOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.RefundOption, "ReturnPolicyRefundOption"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyRestockingFeeValue", "ReturnPolicyRestockingFeeValue"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.RestockingFeeValue, "ReturnPolicyRestockingFeeValue"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyRestockingFeeValueOption", "ReturnPolicyRestockingFeeValueOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.RestockingFeeValueOption, "ReturnPolicyRestockingFeeValueOption"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyReturnsAccepted", "ReturnPolicyReturnsAccepted"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.ReturnsAccepted, "ReturnPolicyReturnsAccepted"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyReturnsAcceptedOption", "ReturnPolicyReturnsAcceptedOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.ReturnsAcceptedOption, "ReturnPolicyReturnsAcceptedOption"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyReturnsWithin", "ReturnPolicyReturnsWithin"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.ReturnsWithin, "ReturnPolicyReturnsWithin"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyReturnsWithinOption", "ReturnPolicyReturnsWithinOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.ReturnsWithinOption, "ReturnPolicyReturnsWithinOption"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyShippingCostPaidBy", "ReturnPolicyShippingCostPaidBy"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.ShippingCostPaidBy, "ReturnPolicyShippingCostPaidBy"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyShippingCostPaidByOption", "ReturnPolicyShippingCostPaidByOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.ShippingCostPaidByOption, "ReturnPolicyShippingCostPaidByOption"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyWarrantyDuration", "ReturnPolicyWarrantyDuration"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.WarrantyDuration, "ReturnPolicyWarrantyDuration"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyWarrantyDurationOption", "ReturnPolicyWarrantyDurationOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.WarrantyDurationOption, "ReturnPolicyWarrantyDurationOption"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyWarrantyOffered", "ReturnPolicyWarrantyOffered"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.WarrantyOffered, "ReturnPolicyWarrantyOffered"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyWarrantyOfferedOption", "ReturnPolicyWarrantyOfferedOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.WarrantyOfferedOption, "ReturnPolicyWarrantyOfferedOption"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyWarrantyType", "ReturnPolicyWarrantyType"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.WarrantyType, "ReturnPolicyWarrantyType"); fileNumber++; } 
if (fetchedItem.ReturnPolicy != null) { createFile("ItemID-+|+-ReturnPolicyWarrantyTypeOption", "ReturnPolicyWarrantyTypeOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReturnPolicy.WarrantyTypeOption, "ReturnPolicyWarrantyTypeOption"); fileNumber++; } 


if (fetchedItem.ReviseStatus != null) { createFile("ItemID-+|+-ReviseStatusBuyItNowAdded", "ReviseStatusBuyItNowAdded"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReviseStatus.BuyItNowAdded, "ReviseStatusBuyItNowAdded"); fileNumber++; } 
if (fetchedItem.ReviseStatus != null) { createFile("ItemID-+|+-ReviseStatusBuyItNowLowered", "ReviseStatusBuyItNowLowered"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReviseStatus.BuyItNowLowered, "ReviseStatusBuyItNowLowered"); fileNumber++; } 
if (fetchedItem.ReviseStatus != null) { createFile("ItemID-+|+-ReviseStatusItemRevised", "ReviseStatusItemRevised"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReviseStatus.ItemRevised, "ReviseStatusItemRevised"); fileNumber++; } 
if (fetchedItem.ReviseStatus != null) { createFile("ItemID-+|+-ReviseStatusReserveLowered", "ReviseStatusReserveLowered"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReviseStatus.ReserveLowered, "ReviseStatusReserveLowered"); fileNumber++; } 
if (fetchedItem.ReviseStatus != null) { createFile("ItemID-+|+-ReviseStatusReserveRemoved", "ReviseStatusReserveRemoved"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ReviseStatus.ReserveRemoved, "ReviseStatusReserveRemoved"); fileNumber++; } 


if (fetchedItem.SecondaryCategory != null) { createFile("ItemID-+|+-SecondaryCategoryCategoryID", "SecondaryCategoryCategoryID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SecondaryCategory.CategoryID, "SecondaryCategoryCategoryID"); fileNumber++; } 
if (fetchedItem.SecondaryCategory != null) { createFile("ItemID-+|+-SecondaryCategoryCategoryName", "SecondaryCategoryCategoryName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SecondaryCategory.CategoryName, "SecondaryCategoryCategoryName"); fileNumber++; } 


if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerAboutMePage", "SellerAboutMePage"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.AboutMePage, "SellerAboutMePage"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellereBayGoodStanding", "SellereBayGoodStanding"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.eBayGoodStanding, "SellereBayGoodStanding"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerEmail", "SellerEmail"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.Email, "SellerEmail"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerFeedbackPrivate", "SellerFeedbackPrivate"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.FeedbackPrivate, "SellerFeedbackPrivate"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerFeedbackRatingStar", "SellerFeedbackRatingStar"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.FeedbackRatingStar, "SellerFeedbackRatingStar"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerFeedbackScore", "SellerFeedbackScore"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.FeedbackScore, "SellerFeedbackScore"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerIDVerified", "SellerIDVerified"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.IDVerified, "SellerIDVerified"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerMotorsDealer", "SellerMotorsDealer"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.MotorsDealer, "SellerMotorsDealer"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerNewUser", "SellerNewUser"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.NewUser, "SellerNewUser"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerPositiveFeedbackPercent", "SellerPositiveFeedbackPercent"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.PositiveFeedbackPercent, "SellerPositiveFeedbackPercent"); fileNumber++; } 

if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressCityName", "SellerRegistrationAddressCityName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.CityName, "SellerRegistrationAddressCityName"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressCountry", "SellerRegistrationAddressCountry"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.Country, "SellerRegistrationAddressCountry"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressCountryName", "SellerRegistrationAddressCountryName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.CountryName, "SellerRegistrationAddressCountryName"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressFirstName", "SellerRegistrationAddressFirstName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.FirstName, "SellerRegistrationAddressFirstName"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressLastName", "SellerRegistrationAddressLastName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.LastName, "SellerRegistrationAddressLastName"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressName", "SellerRegistrationAddressName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.Name, "SellerRegistrationAddressName"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressPhone", "SellerRegistrationAddressPhone"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.Phone, "SellerRegistrationAddressPhone"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressPostalCode", "SellerRegistrationAddressPostalCode"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.PostalCode, "SellerRegistrationAddressPostalCode"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressStreet", "SellerRegistrationAddressStreet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.Street, "SellerRegistrationAddressStreet"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressStreet", "SellerRegistrationAddressStreet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.Street, "SellerRegistrationAddressStreet"); fileNumber++; } 
if (fetchedItem.Seller.RegistrationAddress != null) { createFile("ItemID-+|+-SellerRegistrationAddressStreet", "SellerRegistrationAddressStreet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationAddress.Street, "SellerRegistrationAddressStreet"); fileNumber++; } 

if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerRegistrationDate", "SellerRegistrationDate"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.RegistrationDate, "SellerRegistrationDate"); fileNumber++; } 

if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoAllowPaymentEdit", "SellerSellerInfoAllowPaymentEdit"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.AllowPaymentEdit, "SellerSellerInfoAllowPaymentEdit"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoCheckoutEnabled", "SellerSellerInfoCheckoutEnabled"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.CheckoutEnabled, "SellerSellerInfoCheckoutEnabled"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoCIPBankAccountStored", "SellerSellerInfoCIPBankAccountStored"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.CIPBankAccountStored, "SellerSellerInfoCIPBankAccountStored"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoGoodStanding", "SellerSellerInfoGoodStanding"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.GoodStanding, "SellerSellerInfoGoodStanding"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoMerchandizingPref", "SellerSellerInfoMerchandizingPref"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.MerchandizingPref, "SellerSellerInfoMerchandizingPref"); fileNumber++; } 

if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoSafePaymentExempt", "SellerSellerInfoSafePaymentExempt"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.SafePaymentExempt, "SellerSellerInfoSafePaymentExempt"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoSellerBusinessType", "SellerSellerInfoSellerBusinessType"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.SellerBusinessType, "SellerSellerInfoSellerBusinessType"); fileNumber++; } 

if (fetchedItem.Seller.SellerInfo.SellereBayPaymentProcessConsent != null) { createFile("ItemID-+|+-SellerSellerInfoSellereBayPaymentProcessConsentPayoutMethod", "SellerSellerInfoSellereBayPaymentProcessConsentPayoutMethod"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.SellereBayPaymentProcessConsent.PayoutMethod, "SellerSellerInfoSellereBayPaymentProcessConsentPayoutMethod"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo.SellereBayPaymentProcessConsent != null) { createFile("ItemID-+|+-SellerSellerInfoSellereBayPaymentProcessConsentPayoutMethodSet", "SellerSellerInfoSellereBayPaymentProcessConsentPayoutMethodSet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.SellereBayPaymentProcessConsent.PayoutMethodSet, "SellerSellerInfoSellereBayPaymentProcessConsentPayoutMethodSet"); fileNumber++; } 









if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoSellereBayPaymentProcessStatus", "SellerSellerInfoSellereBayPaymentProcessStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.SellereBayPaymentProcessStatus, "SellerSellerInfoSellereBayPaymentProcessStatus"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoSellerLevel", "SellerSellerInfoSellerLevel"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.SellerLevel, "SellerSellerInfoSellerLevel"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoStoreOwner", "SellerSellerInfoStoreOwner"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.StoreOwner, "SellerSellerInfoStoreOwner"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoStoreURL", "SellerSellerInfoStoreURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.StoreURL, "SellerSellerInfoStoreURL"); fileNumber++; } 
if (fetchedItem.Seller.SellerInfo != null) { createFile("ItemID-+|+-SellerSellerInfoTopRatedSeller", "SellerSellerInfoTopRatedSeller"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.SellerInfo.TopRatedSeller, "SellerSellerInfoTopRatedSeller"); fileNumber++; } 

if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerSite", "SellerSite"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.Site, "SellerSite"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerStatus", "SellerStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.Status, "SellerStatus"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerUserID", "SellerUserID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.UserID, "SellerUserID"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerUserIDChanged", "SellerUserIDChanged"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.UserIDChanged, "SellerUserIDChanged"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerUserIDLastChanged", "SellerUserIDLastChanged"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.UserIDLastChanged, "SellerUserIDLastChanged"); fileNumber++; } 
if (fetchedItem.Seller != null) { createFile("ItemID-+|+-SellerVATStatus", "SellerVATStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Seller.VATStatus, "SellerVATStatus"); fileNumber++; } 


if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsCompanyName", "SellerContactDetailsCompanyName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.CompanyName, "SellerContactDetailsCompanyName"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsCounty", "SellerContactDetailsCounty"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.County, "SellerContactDetailsCounty"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsFirstName", "SellerContactDetailsFirstName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.FirstName, "SellerContactDetailsFirstName"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsLastName", "SellerContactDetailsLastName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.LastName, "SellerContactDetailsLastName"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneAreaOrCityCode", "SellerContactDetailsPhoneAreaOrCityCode"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneAreaOrCityCode, "SellerContactDetailsPhoneAreaOrCityCode"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneCountryCode", "SellerContactDetailsPhoneCountryCode"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneCountryCode, "SellerContactDetailsPhoneCountryCode"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneCountryPrefix", "SellerContactDetailsPhoneCountryPrefix"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneCountryPrefix, "SellerContactDetailsPhoneCountryPrefix"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneLocalNumber", "SellerContactDetailsPhoneLocalNumber"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneLocalNumber, "SellerContactDetailsPhoneLocalNumber"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneAreaOrCityCode", "SellerContactDetailsPhoneAreaOrCityCode"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneAreaOrCityCode, "SellerContactDetailsPhoneAreaOrCityCode"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneCountryCode", "SellerContactDetailsPhoneCountryCode"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneCountryCode, "SellerContactDetailsPhoneCountryCode"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneCountryPrefix", "SellerContactDetailsPhoneCountryPrefix"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneCountryPrefix, "SellerContactDetailsPhoneCountryPrefix"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsPhoneLocalNumber", "SellerContactDetailsPhoneLocalNumber"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.PhoneLocalNumber, "SellerContactDetailsPhoneLocalNumber"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsStreet", "SellerContactDetailsStreet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.Street, "SellerContactDetailsStreet"); fileNumber++; } 
if (fetchedItem.SellerContactDetails != null) { createFile("ItemID-+|+-SellerContactDetailsStreet", "SellerContactDetailsStreet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerContactDetails.Street, "SellerContactDetailsStreet"); fileNumber++; } 















if (fetchedItem != null) { createFile("ItemID-+|+-SellerProvidedTitle", "SellerProvidedTitle"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerProvidedTitle, "SellerProvidedTitle"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-SellerVacationNote", "SellerVacationNote"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellerVacationNote, "SellerVacationNote"); fileNumber++; } 

if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusAdminEnded", "SellingStatusAdminEnded"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.AdminEnded, "SellingStatusAdminEnded"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusBidCount", "SellingStatusBidCount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.BidCount, "SellingStatusBidCount"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusBidIncrement", "SellingStatusBidIncrement"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.BidIncrement.Value.ToString(), "SellingStatusBidIncrement"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusConvertedCurrentPrice", "SellingStatusConvertedCurrentPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.ConvertedCurrentPrice.Value.ToString(), "SellingStatusConvertedCurrentPrice"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusCurrentPrice", "SellingStatusCurrentPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.CurrentPrice.Value.ToString(), "SellingStatusCurrentPrice"); fileNumber++; } 

if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderAboutMePage", "SellingStatusHighBidderAboutMePage"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.AboutMePage, "SellingStatusHighBidderAboutMePage"); fileNumber++; } 








if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBiddereBayGoodStanding", "SellingStatusHighBiddereBayGoodStanding"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.eBayGoodStanding, "SellingStatusHighBiddereBayGoodStanding"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderEmail", "SellingStatusHighBidderEmail"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.Email, "SellingStatusHighBidderEmail"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderFeedbackPrivate", "SellingStatusHighBidderFeedbackPrivate"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.FeedbackPrivate, "SellingStatusHighBidderFeedbackPrivate"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderFeedbackRatingStar", "SellingStatusHighBidderFeedbackRatingStar"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.FeedbackRatingStar, "SellingStatusHighBidderFeedbackRatingStar"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderFeedbackScore", "SellingStatusHighBidderFeedbackScore"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.FeedbackScore, "SellingStatusHighBidderFeedbackScore"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderIDVerified", "SellingStatusHighBidderIDVerified"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.IDVerified, "SellingStatusHighBidderIDVerified"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderNewUser", "SellingStatusHighBidderNewUser"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.NewUser, "SellingStatusHighBidderNewUser"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderPositiveFeedbackPercent", "SellingStatusHighBidderPositiveFeedbackPercent"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.PositiveFeedbackPercent, "SellingStatusHighBidderPositiveFeedbackPercent"); fileNumber++; } 













if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderRegistrationDate", "SellingStatusHighBidderRegistrationDate"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.RegistrationDate, "SellingStatusHighBidderRegistrationDate"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderSite", "SellingStatusHighBidderSite"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.Site, "SellingStatusHighBidderSite"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderStatus", "SellingStatusHighBidderStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.Status, "SellingStatusHighBidderStatus"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderUserAnonymized", "SellingStatusHighBidderUserAnonymized"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.UserAnonymized, "SellingStatusHighBidderUserAnonymized"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderUserID", "SellingStatusHighBidderUserID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.UserID, "SellingStatusHighBidderUserID"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderUserIDChanged", "SellingStatusHighBidderUserIDChanged"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.UserIDChanged, "SellingStatusHighBidderUserIDChanged"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderUserIDLastChanged", "SellingStatusHighBidderUserIDLastChanged"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.UserIDLastChanged, "SellingStatusHighBidderUserIDLastChanged"); fileNumber++; } 
if (fetchedItem.SellingStatus.HighBidder != null) { createFile("ItemID-+|+-SellingStatusHighBidderVATStatus", "SellingStatusHighBidderVATStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.HighBidder.VATStatus, "SellingStatusHighBidderVATStatus"); fileNumber++; } 

if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusLeadCount", "SellingStatusLeadCount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.LeadCount, "SellingStatusLeadCount"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusListingStatus", "SellingStatusListingStatus"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.ListingStatus, "SellingStatusListingStatus"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusMinimumToBid", "SellingStatusMinimumToBid"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.MinimumToBid.Value.ToString(), "SellingStatusMinimumToBid"); fileNumber++; } 

if (fetchedItem.SellingStatus.PromotionalSaleDetails != null) { createFile("ItemID-+|+-SellingStatusPromotionalSaleDetailsEndTime", "SellingStatusPromotionalSaleDetailsEndTime"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.PromotionalSaleDetails.EndTime, "SellingStatusPromotionalSaleDetailsEndTime"); fileNumber++; } 
if (fetchedItem.SellingStatus.PromotionalSaleDetails != null) { createFile("ItemID-+|+-SellingStatusPromotionalSaleDetailsOriginalPrice", "SellingStatusPromotionalSaleDetailsOriginalPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.PromotionalSaleDetails.OriginalPrice, "SellingStatusPromotionalSaleDetailsOriginalPrice"); fileNumber++; } 
if (fetchedItem.SellingStatus.PromotionalSaleDetails != null) { createFile("ItemID-+|+-SellingStatusPromotionalSaleDetailsStartTime", "SellingStatusPromotionalSaleDetailsStartTime"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.PromotionalSaleDetails.StartTime, "SellingStatusPromotionalSaleDetailsStartTime"); fileNumber++; } 

if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusQuantitySold", "SellingStatusQuantitySold"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.QuantitySold, "SellingStatusQuantitySold"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusQuantitySoldByPickupInStore", "SellingStatusQuantitySoldByPickupInStore"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.QuantitySoldByPickupInStore, "SellingStatusQuantitySoldByPickupInStore"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusReserveMet", "SellingStatusReserveMet"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.ReserveMet, "SellingStatusReserveMet"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusSecondChanceEligible", "SellingStatusSecondChanceEligible"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.SecondChanceEligible, "SellingStatusSecondChanceEligible"); fileNumber++; } 
if (fetchedItem.SellingStatus != null) { createFile("ItemID-+|+-SellingStatusSoldAsBin", "SellingStatusSoldAsBin"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SellingStatus.SoldAsBin, "SellingStatusSoldAsBin"); fileNumber++; } 


if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsAllowPaymentEdit", "ShippingDetailsAllowPaymentEdit"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.AllowPaymentEdit, "ShippingDetailsAllowPaymentEdit"); fileNumber++; } 

if (fetchedItem.ShippingDetails.CalculatedShippingDiscount != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingDiscountDiscountName", "ShippingDetailsCalculatedShippingDiscountDiscountName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingDiscount.DiscountName, "ShippingDetailsCalculatedShippingDiscountDiscountName"); fileNumber++; } 









if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRateInternationalPackagingHandlingCosts", "ShippingDetailsCalculatedShippingRateInternationalPackagingHandlingCosts"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.InternationalPackagingHandlingCosts, "ShippingDetailsCalculatedShippingRateInternationalPackagingHandlingCosts"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRateOriginatingPostalCode", "ShippingDetailsCalculatedShippingRateOriginatingPostalCode"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.OriginatingPostalCode, "ShippingDetailsCalculatedShippingRateOriginatingPostalCode"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRatePackageDepth", "ShippingDetailsCalculatedShippingRatePackageDepth"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.PackageDepth, "ShippingDetailsCalculatedShippingRatePackageDepth"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRatePackageLength", "ShippingDetailsCalculatedShippingRatePackageLength"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.PackageLength, "ShippingDetailsCalculatedShippingRatePackageLength"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRatePackageWidth", "ShippingDetailsCalculatedShippingRatePackageWidth"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.PackageWidth, "ShippingDetailsCalculatedShippingRatePackageWidth"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRatePackagingHandlingCosts", "ShippingDetailsCalculatedShippingRatePackagingHandlingCosts"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.PackagingHandlingCosts, "ShippingDetailsCalculatedShippingRatePackagingHandlingCosts"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRateShippingIrregular", "ShippingDetailsCalculatedShippingRateShippingIrregular"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.ShippingIrregular, "ShippingDetailsCalculatedShippingRateShippingIrregular"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRateShippingPackage", "ShippingDetailsCalculatedShippingRateShippingPackage"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.ShippingPackage, "ShippingDetailsCalculatedShippingRateShippingPackage"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRateWeightMajor", "ShippingDetailsCalculatedShippingRateWeightMajor"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.WeightMajor, "ShippingDetailsCalculatedShippingRateWeightMajor"); fileNumber++; } 
if (fetchedItem.ShippingDetails.CalculatedShippingRate != null) { createFile("ItemID-+|+-ShippingDetailsCalculatedShippingRateWeightMinor", "ShippingDetailsCalculatedShippingRateWeightMinor"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CalculatedShippingRate.WeightMinor, "ShippingDetailsCalculatedShippingRateWeightMinor"); fileNumber++; } 

if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsCODCost", "ShippingDetailsCODCost"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.CODCost, "ShippingDetailsCODCost"); fileNumber++; } 
if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsExcludeShipToLocation", "ShippingDetailsExcludeShipToLocation"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.ExcludeShipToLocation, "ShippingDetailsExcludeShipToLocation"); fileNumber++; } 


if (fetchedItem.ShippingDetails.FlatShippingDiscount != null) { createFile("ItemID-+|+-ShippingDetailsFlatShippingDiscountDiscountName", "ShippingDetailsFlatShippingDiscountDiscountName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.FlatShippingDiscount.DiscountName, "ShippingDetailsFlatShippingDiscountDiscountName"); fileNumber++; } 









if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsGetItFast", "ShippingDetailsGetItFast"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.GetItFast, "ShippingDetailsGetItFast"); fileNumber++; } 
if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsGlobalShipping", "ShippingDetailsGlobalShipping"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.GlobalShipping, "ShippingDetailsGlobalShipping"); fileNumber++; } 

if (fetchedItem.ShippingDetails.InsuranceDetails != null) { createFile("ItemID-+|+-ShippingDetailsInsuranceDetailsInsuranceFee", "ShippingDetailsInsuranceDetailsInsuranceFee"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InsuranceDetails.InsuranceFee, "ShippingDetailsInsuranceDetailsInsuranceFee"); fileNumber++; } 
if (fetchedItem.ShippingDetails.InsuranceDetails != null) { createFile("ItemID-+|+-ShippingDetailsInsuranceDetailsInsuranceOption", "ShippingDetailsInsuranceDetailsInsuranceOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InsuranceDetails.InsuranceOption, "ShippingDetailsInsuranceDetailsInsuranceOption"); fileNumber++; } 

if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsInsuranceFee", "ShippingDetailsInsuranceFee"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InsuranceFee, "ShippingDetailsInsuranceFee"); fileNumber++; } 
if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsInsuranceOption", "ShippingDetailsInsuranceOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InsuranceOption, "ShippingDetailsInsuranceOption"); fileNumber++; } 

if (fetchedItem.ShippingDetails.InternationalCalculatedShippingDiscount != null) { createFile("ItemID-+|+-ShippingDetailsInternationalCalculatedShippingDiscountDiscountName", "ShippingDetailsInternationalCalculatedShippingDiscountDiscountName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InternationalCalculatedShippingDiscount.DiscountName, "ShippingDetailsInternationalCalculatedShippingDiscountDiscountName"); fileNumber++; } 









if (fetchedItem.ShippingDetails.InternationalFlatShippingDiscount != null) { createFile("ItemID-+|+-ShippingDetailsInternationalFlatShippingDiscountDiscountName", "ShippingDetailsInternationalFlatShippingDiscountDiscountName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InternationalFlatShippingDiscount.DiscountName, "ShippingDetailsInternationalFlatShippingDiscountDiscountName"); fileNumber++; } 










if (fetchedItem.ShippingDetails.InternationalInsuranceDetails != null) { createFile("ItemID-+|+-ShippingDetailsInternationalInsuranceDetailsInsuranceFee", "ShippingDetailsInternationalInsuranceDetailsInsuranceFee"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InternationalInsuranceDetails.InsuranceFee, "ShippingDetailsInternationalInsuranceDetailsInsuranceFee"); fileNumber++; } 
if (fetchedItem.ShippingDetails.InternationalInsuranceDetails != null) { createFile("ItemID-+|+-ShippingDetailsInternationalInsuranceDetailsInsuranceOption", "ShippingDetailsInternationalInsuranceDetailsInsuranceOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InternationalInsuranceDetails.InsuranceOption, "ShippingDetailsInternationalInsuranceDetailsInsuranceOption"); fileNumber++; } 

if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsInternationalPromotionalShippingDiscount", "ShippingDetailsInternationalPromotionalShippingDiscount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InternationalPromotionalShippingDiscount, "ShippingDetailsInternationalPromotionalShippingDiscount"); fileNumber++; } 
if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsInternationalShippingDiscountProfileID", "ShippingDetailsInternationalShippingDiscountProfileID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.InternationalShippingDiscountProfileID, "ShippingDetailsInternationalShippingDiscountProfileID"); fileNumber++; } 









if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsPaymentInstructions", "ShippingDetailsPaymentInstructions"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.PaymentInstructions, "ShippingDetailsPaymentInstructions"); fileNumber++; } 
if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsPromotionalShippingDiscount", "ShippingDetailsPromotionalShippingDiscount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.PromotionalShippingDiscount, "ShippingDetailsPromotionalShippingDiscount"); fileNumber++; } 

if (fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails != null) { createFile("ItemID-+|+-ShippingDetailsPromotionalShippingDiscountDetailsDiscountName", "ShippingDetailsPromotionalShippingDiscountDetailsDiscountName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails.DiscountName, "ShippingDetailsPromotionalShippingDiscountDetailsDiscountName"); fileNumber++; } 
if (fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails != null) { createFile("ItemID-+|+-ShippingDetailsPromotionalShippingDiscountDetailsItemCount", "ShippingDetailsPromotionalShippingDiscountDetailsItemCount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails.ItemCount, "ShippingDetailsPromotionalShippingDiscountDetailsItemCount"); fileNumber++; } 
if (fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails != null) { createFile("ItemID-+|+-ShippingDetailsPromotionalShippingDiscountDetailsOrderAmount", "ShippingDetailsPromotionalShippingDiscountDetailsOrderAmount"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails.OrderAmount, "ShippingDetailsPromotionalShippingDiscountDetailsOrderAmount"); fileNumber++; } 
if (fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails != null) { createFile("ItemID-+|+-ShippingDetailsPromotionalShippingDiscountDetailsShippingCost", "ShippingDetailsPromotionalShippingDiscountDetailsShippingCost"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.PromotionalShippingDiscountDetails.ShippingCost, "ShippingDetailsPromotionalShippingDiscountDetailsShippingCost"); fileNumber++; } 


if (fetchedItem.ShippingDetails.RateTableDetails != null) { createFile("ItemID-+|+-ShippingDetailsRateTableDetailsDomesticRateTable", "ShippingDetailsRateTableDetailsDomesticRateTable"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.RateTableDetails.DomesticRateTable, "ShippingDetailsRateTableDetailsDomesticRateTable"); fileNumber++; } 
if (fetchedItem.ShippingDetails.RateTableDetails != null) { createFile("ItemID-+|+-ShippingDetailsRateTableDetailsInternationalRateTable", "ShippingDetailsRateTableDetailsInternationalRateTable"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.RateTableDetails.InternationalRateTable, "ShippingDetailsRateTableDetailsInternationalRateTable"); fileNumber++; } 


if (fetchedItem.ShippingDetails.SalesTax != null) { createFile("ItemID-+|+-ShippingDetailsSalesTaxSalesTaxPercent", "ShippingDetailsSalesTaxSalesTaxPercent"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.SalesTax.SalesTaxPercent, "ShippingDetailsSalesTaxSalesTaxPercent"); fileNumber++; } 
if (fetchedItem.ShippingDetails.SalesTax != null) { createFile("ItemID-+|+-ShippingDetailsSalesTaxSalesTaxState", "ShippingDetailsSalesTaxSalesTaxState"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.SalesTax.SalesTaxState, "ShippingDetailsSalesTaxSalesTaxState"); fileNumber++; } 
if (fetchedItem.ShippingDetails.SalesTax != null) { createFile("ItemID-+|+-ShippingDetailsSalesTaxShippingIncludedInTax", "ShippingDetailsSalesTaxShippingIncludedInTax"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.SalesTax.ShippingIncludedInTax, "ShippingDetailsSalesTaxShippingIncludedInTax"); fileNumber++; } 

if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsSellerExcludeShipToLocationsPreference", "ShippingDetailsSellerExcludeShipToLocationsPreference"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.SellerExcludeShipToLocationsPreference, "ShippingDetailsSellerExcludeShipToLocationsPreference"); fileNumber++; } 
if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsShippingDiscountProfileID", "ShippingDetailsShippingDiscountProfileID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.ShippingDiscountProfileID, "ShippingDetailsShippingDiscountProfileID"); fileNumber++; } 













if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsShippingType", "ShippingDetailsShippingType"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.ShippingType, "ShippingDetailsShippingType"); fileNumber++; } 








if (fetchedItem.ShippingDetails != null) { createFile("ItemID-+|+-ShippingDetailsThirdPartyCheckout", "ShippingDetailsThirdPartyCheckout"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingDetails.ThirdPartyCheckout, "ShippingDetailsThirdPartyCheckout"); fileNumber++; } 


if (fetchedItem.ShippingPackageDetails != null) { createFile("ItemID-+|+-ShippingPackageDetailsPackageDepth", "ShippingPackageDetailsPackageDepth"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingPackageDetails.PackageDepth, "ShippingPackageDetailsPackageDepth"); fileNumber++; } 
if (fetchedItem.ShippingPackageDetails != null) { createFile("ItemID-+|+-ShippingPackageDetailsPackageLength", "ShippingPackageDetailsPackageLength"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingPackageDetails.PackageLength, "ShippingPackageDetailsPackageLength"); fileNumber++; } 
if (fetchedItem.ShippingPackageDetails != null) { createFile("ItemID-+|+-ShippingPackageDetailsPackageWidth", "ShippingPackageDetailsPackageWidth"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingPackageDetails.PackageWidth, "ShippingPackageDetailsPackageWidth"); fileNumber++; } 
if (fetchedItem.ShippingPackageDetails != null) { createFile("ItemID-+|+-ShippingPackageDetailsShippingIrregular", "ShippingPackageDetailsShippingIrregular"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingPackageDetails.ShippingIrregular, "ShippingPackageDetailsShippingIrregular"); fileNumber++; } 
if (fetchedItem.ShippingPackageDetails != null) { createFile("ItemID-+|+-ShippingPackageDetailsShippingPackage", "ShippingPackageDetailsShippingPackage"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingPackageDetails.ShippingPackage, "ShippingPackageDetailsShippingPackage"); fileNumber++; } 
if (fetchedItem.ShippingPackageDetails != null) { createFile("ItemID-+|+-ShippingPackageDetailsWeightMajor", "ShippingPackageDetailsWeightMajor"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingPackageDetails.WeightMajor, "ShippingPackageDetailsWeightMajor"); fileNumber++; } 
if (fetchedItem.ShippingPackageDetails != null) { createFile("ItemID-+|+-ShippingPackageDetailsWeightMinor", "ShippingPackageDetailsWeightMinor"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingPackageDetails.WeightMinor, "ShippingPackageDetailsWeightMinor"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-ShippingTermsInDescription", "ShippingTermsInDescription"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShippingTermsInDescription, "ShippingTermsInDescription"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-ShipToLocations", "ShipToLocations"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.ShipToLocations, "ShipToLocations"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-Site", "Site"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Site, "Site"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-SKU", "SKU"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SKU, "SKU"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-SkypeContactOption", "SkypeContactOption"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SkypeContactOption, "SkypeContactOption"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-SkypeEnabled", "SkypeEnabled"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SkypeEnabled, "SkypeEnabled"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-SkypeID", "SkypeID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SkypeID, "SkypeID"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-StartPrice", "StartPrice"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.StartPrice.Value.ToString(), "StartPrice"); fileNumber++; } 

if (fetchedItem.Storefront != null) { createFile("ItemID-+|+-StorefrontStoreCategoryID", "StorefrontStoreCategoryID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Storefront.StoreCategoryID, "StorefrontStoreCategoryID"); fileNumber++; } 
if (fetchedItem.Storefront != null) { createFile("ItemID-+|+-StorefrontStoreCategoryName", "StorefrontStoreCategoryName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Storefront.StoreCategoryName, "StorefrontStoreCategoryName"); fileNumber++; } 
if (fetchedItem.Storefront != null) { createFile("ItemID-+|+-StorefrontStoreCategoryID", "StorefrontStoreCategoryID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Storefront.StoreCategoryID, "StorefrontStoreCategoryID"); fileNumber++; } 
if (fetchedItem.Storefront != null) { createFile("ItemID-+|+-StorefrontStoreCategoryName", "StorefrontStoreCategoryName"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Storefront.StoreCategoryName, "StorefrontStoreCategoryName"); fileNumber++; } 
if (fetchedItem.Storefront != null) { createFile("ItemID-+|+-StorefrontStoreURL", "StorefrontStoreURL"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Storefront.StoreURL, "StorefrontStoreURL"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-SubTitle", "SubTitle"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.SubTitle, "SubTitle"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-TaxCategory", "TaxCategory"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.TaxCategory, "TaxCategory"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-TimeLeft", "TimeLeft"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.TimeLeft, "TimeLeft"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-Title", "Title"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.Title, "Title"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-TopRatedListing", "TopRatedListing"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.TopRatedListing, "TopRatedListing"); fileNumber++; } 

if (fetchedItem.UnitInfo != null) { createFile("ItemID-+|+-UnitInfoUnitQuantity", "UnitInfoUnitQuantity"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.UnitInfo.UnitQuantity, "UnitInfoUnitQuantity"); fileNumber++; } 
if (fetchedItem.UnitInfo != null) { createFile("ItemID-+|+-UnitInfoUnitType", "UnitInfoUnitType"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.UnitInfo.UnitType, "UnitInfoUnitType"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-UUID", "UUID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.UUID, "UUID"); fileNumber++; } 












































































































if (fetchedItem.VATDetails != null) { createFile("ItemID-+|+-VATDetailsBusinessSeller", "VATDetailsBusinessSeller"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VATDetails.BusinessSeller, "VATDetailsBusinessSeller"); fileNumber++; } 
if (fetchedItem.VATDetails != null) { createFile("ItemID-+|+-VATDetailsRestrictedToBusiness", "VATDetailsRestrictedToBusiness"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VATDetails.RestrictedToBusiness, "VATDetailsRestrictedToBusiness"); fileNumber++; } 
if (fetchedItem.VATDetails != null) { createFile("ItemID-+|+-VATDetailsVATID", "VATDetailsVATID"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VATDetails.VATID, "VATDetailsVATID"); fileNumber++; } 
if (fetchedItem.VATDetails != null) { createFile("ItemID-+|+-VATDetailsVATPercent", "VATDetailsVATPercent"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VATDetails.VATPercent, "VATDetailsVATPercent"); fileNumber++; } 
if (fetchedItem.VATDetails != null) { createFile("ItemID-+|+-VATDetailsVATSite", "VATDetailsVATSite"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VATDetails.VATSite, "VATDetailsVATSite"); fileNumber++; } 

if (fetchedItem != null) { createFile("ItemID-+|+-VIN", "VIN"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VIN, "VIN"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-VINLink", "VINLink"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VINLink, "VINLink"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-VRM", "VRM"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VRM, "VRM"); fileNumber++; } 
if (fetchedItem != null) { createFile("ItemID-+|+-VRMLink", "VRMLink"); writeToFile(fetchedItem.ItemID.ToString() + "-+|+-" + fetchedItem.VRMLink, "VRMLink"); fileNumber++; } 











































    #endregion


                #region Item Specifics
                // Get item specifics
                Log("Now Getting Item Specifics...");
                Log("The number of itemspecifics is: " + fetchedItem.ItemSpecifics.Count.ToString());

                NameValueListTypeCollection attrSets = fetchedItem.ItemSpecifics;
                foreach (NameValueListType attribute in attrSets)
                {
                    string addNewContent = "";
                    Log("Now getting attribute details...");
                    {
                        if (attribute.Value.ToArray().Count()>1)
                        {
                            addNewContent = attribute.Value.ToArray()[0];
                            for (int i = 2; i <= attribute.Value.ToArray().Count(); i++ )
                            {
                                addNewContent = addNewContent + ";" + attribute.Value.ToArray()[i - 1];
                            }
                                
                        }
                        else
                        {
                            addNewContent = attribute.Value.ToArray()[0];


                        }

                        createSpecsFile("ItemID-+|+-" + attribute.Name.ToString(), attribute.Name.ToString());
                        writeToSpecsFile(fetchedItem.ItemID.ToString() + "-+|+-" + addNewContent, attribute.Name.ToString());
                    }
                }

                #endregion


                // Start to fetch bidding-history on the ended listings
                if (fetchedItem.SellingStatus.BidCount>0)
                {
                    getBidderHistory(fetchedItem.ItemID.ToString());
                }
                

                // THE SQL METHOD IS REPLACED TO SAVING AS A TEXT FILE
                //eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
                //dbContext.AddToS4PanelData(newSearch);
                //dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            return false;
        }

        private List<string> QuerySearchItemID()
        {
            // return ItemID only
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S1SearchTable> listings = dbContext.S1SearchTable;
            var query = from listing in dbContext.S1SearchTable
                        where (listing.EndTimeDum != 1 | listing.EndTimeDum == null)
                        select listing;

            GridPreData.ItemsSource = query.ToList();

            List<string> itemList = new List<string>();
            foreach (var listin in query)
            {
                itemList.Add(listin.ItemID);
            }

            return itemList;
        }
       
        private void QueryUpdateUserID()
        {
            
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S4PanelData> listings = dbContext.S4PanelData;
            var query = (from listing in dbContext.S4PanelData
                        where (listing.UserInfoFetched != 1 | listing.UserInfoFetched == null)
                        select listing).First();

            // userList.Add(listin.SellerID); ItemID = listin.ItemID ; UserID = listin.SellerID 
            Log("Now QueryUpdate ItemID =: " + query.ItemID + ", SellerID =" + query.SellerID);
            try
            {
                S4PanelData returnedResult = FetchUserFnS4(query.SellerID);

                query.UserInfoFetched = 1;
                query.Email = returnedResult.Email;
                query.FeedBackScore = returnedResult.FeedBackScore;
                query.SellerRegDate = returnedResult.SellerRegDate;
                query.SellerLevel = returnedResult.SellerLevel;
                query.Site = returnedResult.Site;
                query.SellerStar = returnedResult.SellerStar;
                query.SellerStore = returnedResult.SellerStore;
                query.UserIsNewReg = returnedResult.UserIsNewReg;
                query.SellerVerified = returnedResult.SellerVerified;
                query.SellerIDChanged = returnedResult.SellerIDChanged;

                dbContext.SaveChanges();

                Log("user info obtained for user id:" + query.SellerID.ToString() + ", with store details: " + query.SellerStore);

            }
            catch (Exception ex)
            {
                query.UserInfoFetched = 1;
                dbContext.SaveChanges();
                Log(ex.Message);
            }
        }


        private S4PanelData FetchUserFnS4(string userTerm)
        {
            try
            {
                GetUserCall apicall = new GetUserCall(GetContext());

                apicall.UserID = userTerm;
                UserType user = apicall.GetUser();

                var newUser = new S4PanelData
                {
                    Email = user.Email.ToString(),
                    FeedBackScore = user.FeedbackScore.ToString(),
                    SellerRegDate = user.RegistrationDate.ToLocalTime(),
                    SellerLevel = user.SellerInfo.SellerLevel.ToString(),
                    Site = user.Site.ToString(),
                    SellerStar = user.FeedbackRatingStar.ToString(),
                    SellerStore = user.SellerInfo.StoreURL,
                    UserIsNewReg = user.NewUser.ToString(),
                    SellerVerified = user.IDVerified.ToString(),
                    SellerIDChanged = user.UserIDLastChanged.ToString(),
                    MotorsDealer = user.MotorsDealer.ToString()
                };
                Log("Fetched User Infor for ID = " + user.UserID.ToString());
                return newUser;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return null;
            }
        }


        int requestCountNum;
        private void countRequest()
        {
            requestCountNum = 0;
            // Count number of entries in a day-interval in 3 databases: S1, S2 and S3
            requestCountNum += countS1Request();
            requestCountNum += countS2Request();
            requestCountNum += countS4Request();
            requestCountNum += countS4Request();    // each entry in S4 counts twice: one for item; one for user

            LabelTxt.Content = "Total requests sent today is: " + requestCountNum.ToString();
            
        }


        private int countS1Request()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S1SearchTable> listings = dbContext.S1SearchTable;
            var query = from listing in dbContext.S1SearchTable
                        where (listing.FetchTime >= DateTime.Today)
                         select listing;

            List<int> s1RequestNumList = new List<int>();
            Log(query.Count().ToString());
            foreach (var listin in query)
            {
                s1RequestNumList.Add(listin.SearchID);
            }

            return s1RequestNumList.Count();
        }

        private int countS2Request()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S2ItemInfo> listings = dbContext.S2ItemInfo;
            var query = from listing in dbContext.S2ItemInfo
                        where (listing.FetchTime >= DateTime.Today)
                        select listing;

            List<int> s2RequestNumList = new List<int>();
            Log(query.Count().ToString());
            foreach (var listin in query)
            {
                s2RequestNumList.Add(listin.SearchID);
            }

            return s2RequestNumList.Count();
        }

        private int countS4Request()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S4PanelData> listings = dbContext.S4PanelData;
            var query = from listing in dbContext.S4PanelData
                        where (listing.FetchTime >= DateTime.Today)
                        select listing;

            List<int> s4RequestNumList = new List<int>();
            Log(query.Count().ToString());
            foreach (var listin in query)
            {
                s4RequestNumList.Add(listin.SearchID);
            }

            return s4RequestNumList.Count();
        }

        public void addDefaultSearch()
        {

            defaultSearches.Clear();                // clear Search List

            // Add Car types if checked
            if (ChkAllCars.IsChecked == true)
            {
                foreach (int item in carCatArray)
                {
                    defaultSearches.Add(Convert.ToString(item));
                }
            }


            // Add iPad iPod iPhone iMac if checked
            if (ChkAllApples.IsChecked == true)
            {
                foreach (int item in appleCatArray)
                {
                    defaultSearches.Add(Convert.ToString(item));
                }
            }


            // Add Real Estate Categories if checked
            if (ChkAllRealEst.IsChecked == true)
            {
                foreach (int item in realEstateCatArray)
                {
                    defaultSearches.Add(Convert.ToString(item));
                }
            }

            // Add MotorBikes if checked
            if (ChkMotorBikes.IsChecked == true)
            {
                foreach (int item in motorBikesCatArray)
                {
                    defaultSearches.Add(Convert.ToString(item));
                }
            }

            // Add Softwares if checked
            if (ChkSoftwares.IsChecked == true)
            {
                foreach (int item in SoftwaresCatArray)
                {
                    defaultSearches.Add(Convert.ToString(item));
                }
            }

            // Add Boats if checked
            if (ChkBoats.IsChecked == true)
            {
                foreach (int item in BoatCatArray)
                {
                    defaultSearches.Add(Convert.ToString(item));
                }
            }

            // Add Giftcards if checked
            if (ChkGiftcards.IsChecked == true)
            {
                foreach (int item in GiftcardsCatArray)
                {
                    defaultSearches.Add(Convert.ToString(item));
                }
            }

            // If Search list is NIL, add SearchTerm
            if (TxtSearchTerm.Text.Length > 0 && defaultSearches.Count == 0)
            {
                defaultSearches.Add(TxtSearchTerm.Text);
            }
        }

        public void SearchTimer_Tick1(object o1, EventArgs e)    // does search every 1000 seconds
        {
            TxtSearchCountDown.Content = "Next Search Countdown: " + SearchTimerSecs.ToString() + " Secs";
            if (SearchTimerSecs <= 0)
            {
                SearchTimerSecs = Convert.ToInt32(TxtSearchInterval.Text) * 60;

                // Start to do Searches
                addDefaultSearch();                     // Add categories IDs to default search list

                if (defaultSearches.Count > 0)
                {

                    // System.Threading.Thread.Sleep(Convert.ToInt32(TxtFetchInterval.Text) * 1000 * 60);

                    Stage1Fn();

                    reloadPreDataGrid();

                }
                else
                {
                    Log(" ERROR - PLEASE SPECIFY A SEARCH CATEGORY ID ");
                }

            }
            else
            {
                SearchTimerSecs = SearchTimerSecs - 1;
            }

        }





    }
}
