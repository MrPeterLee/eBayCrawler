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

        private void DeleteS3Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            var query = from p in dbContext.S3UserDetail
                        //            where p.ProductName.Contains("Test")
                        select p;
            foreach (S3UserDetail p in query)
                dbContext.DeleteObject(p);
            dbContext.SaveChanges();
        }

        private void DelDuplicateS3Data()
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S3UserDetail> listings = dbContext.S3UserDetail;

            var query = from listing in dbContext.S3UserDetail
                        group listing by listing.UserID into g
                        where g.Count() > 1
                        select new
                        {
                            DuplicateItemID = g.Key,
                            DupSearchID = g.Min(listing => listing.TableIndex)
                        };

            List<int> DupList = new List<int>();
            foreach (var listin in query)
            {
                DupList.Add(listin.DupSearchID);
            }

            foreach (int rowNum in DupList)
            {
                Log("Now deleting Search ID = " + rowNum.ToString() + ", in S3TableResult");
                DelS3Row(rowNum);
            }
        }

        private void DelS3Row(int IndexID)
        {
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S3UserDetail> listings = dbContext.S3UserDetail;

            Log("The current IndexID is = " + IndexID.ToString());

            var query = from listing in dbContext.S3UserDetail
                        where listing.TableIndex == IndexID
                        select listing;

            foreach (var listin in query)
                dbContext.DeleteObject(listin);
            dbContext.SaveChanges();
        }

        private void reloadUserDataGrid()
        {
            ObjectQuery<S3UserDetail> listings = dataEntities.S3UserDetail;

            var query = from listing in listings
                        orderby listing.UserID
                        select new
                        {
                            listing.UserID,
                            listing.FeedBackScore,
                            listing.MotorsDealer,
                            listing.SellerStar,
                            listing.SellerRegDate,
                            listing.SellerLevel,
                            listing.Site,
                            listing.Email,
                            listing.UserIsNewReg,
                            listing.SellerVerified,
                            listing.SellerIDChanged,
                            listing.SellerStore
                        };
            GridUserData.ItemsSource = query.ToList();
        }
    }
}
