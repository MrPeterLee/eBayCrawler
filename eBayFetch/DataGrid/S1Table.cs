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

        private void DeleteS1Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            var query = from p in dbContext.S1SearchTable
                        //            where p.ProductName.Contains("Test")
                        select p;
            foreach (S1SearchTable p in query)
                dbContext.DeleteObject(p);
            dbContext.SaveChanges();
        }

        private void DelDuplicateS1Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S1SearchTable> listings = dbContext.S1SearchTable;

            var query = from listing in dbContext.S1SearchTable
                        group listing by listing.ItemID into g
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
                Log("Now deleting Search ID = " + rowNum.ToString() + ", in S1TableResult");
                DelS1Row(rowNum);
            }
        }

        private void DelS1Row(int IndexID)
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S1SearchTable> listings = dbContext.S1SearchTable;

            Log("The current IndexID is = " + IndexID.ToString());

            var query = from listing in dbContext.S1SearchTable
                        where listing.SearchID == IndexID
                        select listing;

            foreach (var listin in query)
                dbContext.DeleteObject(listin);
            dbContext.SaveChanges();
        }

        private void reloadPreDataGrid()
        {
            ObjectQuery<S1SearchTable> listings = dataEntities.S1SearchTable;

            var query =
            from listing in listings
            orderby listing.SearchID
            select new
            {
                listing.SearchID,
                listing.ItemID,
                listing.FetchTime,
                listing.IsInPanelData,
                listing.Title,
                listing.CategoryID
            };
            GridPreData.ItemsSource = query.ToList();
        }
    }
}
