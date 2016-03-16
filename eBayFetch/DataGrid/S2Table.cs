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

        private void DeleteS2Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            var query = from p in dbContext.S2ItemInfo
                        //            where p.ProductName.Contains("Test")
                        select p;
            foreach (S2ItemInfo p in query)
                dbContext.DeleteObject(p);
            dbContext.SaveChanges();
        }

        private void DelDuplicateS2Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S2ItemInfo> listings = dbContext.S2ItemInfo;

            var query = from listing in dbContext.S2ItemInfo
                        group listing by listing.ItemID into g
                        where g.Count() > 1
                        select new
                        {
                            DuplicateItemID = g.Key,
                            DupSearchID = g.Max(listing => listing.SearchID)       // delete 1 duplicate entry recently
                        };

            List<int> DupList = new List<int>();
            foreach (var listin in query)
            {
                DupList.Add(listin.DupSearchID);
            }

            foreach (int rowNum in DupList)
            {
                Log("Now deleting Search ID = " + rowNum.ToString() + ", in S2TableResult");
                DelS2Row(rowNum);
            }
        }

        private void DelBeginDuplicateS2Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S2ItemInfo> listings = dbContext.S2ItemInfo;

            var query = from listing in dbContext.S2ItemInfo
                        group listing by listing.ItemID into g
                        where g.Count() > 1
                        select new
                        {
                            DuplicateItemID = g.Key,
                            DupSearchID = g.Min(listing => listing.SearchID)       // delete 1 duplicate entry at beginning
                        };

            List<int> DupList = new List<int>();
            foreach (var listin in query)
            {
                DupList.Add(listin.DupSearchID);
            }

            foreach (int rowNum in DupList)
            {
                Log("Now deleting Search ID = " + rowNum.ToString() + ", in S2TableResult");
                DelS2Row(rowNum);
            }
        }

        private void DelS2Row(int IndexID)
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S2ItemInfo> listings = dbContext.S2ItemInfo;

            Log("The current IndexID is = " + IndexID.ToString());

            var query = from listing in dbContext.S2ItemInfo
                        where (listing.SearchID == IndexID & listing.IsInPanelData == 0)
                        select listing;

            foreach (var listin in query)
                dbContext.DeleteObject(listin);
            dbContext.SaveChanges();
        }


        private void reloadItemDataGrid()
        {
            ObjectQuery<S2ItemInfo> listings = dataEntities.S2ItemInfo;

            var query = from listing in listings
                        //  where product.Color == "Red"
                        orderby listing.SearchID
                        select new
                        {
                            listing.ItemID,
                            listing.Title,
                            listing.SellerID,
                            listing.IsInPanelData,
                            listing.FetchTime,
                            listing.StartTime,
                            listing.EndTime,
                            listing.Price,
                            listing.BuyItNowPrice,
                            listing.BidCount,
                            listing.PrivateListing,
                            listing.Quantity,
                            listing.QuantitySold,
                            listing.PrimaryCategory,
                            listing.PrimaryCategoryID
                        };
            GridItemData.ItemsSource = query.ToList();
        }
    }
}
