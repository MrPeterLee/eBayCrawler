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
        private void FetchUserFn(string userTerm)
        {
            try
            {
                GetUserCall apicall = new GetUserCall(GetContext());

                apicall.UserID = userTerm;
                UserType user = apicall.GetUser();

                var newUser = new S3UserDetail
                {
                    UserID = user.UserID.ToString(),
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

                eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
                dbContext.AddToS3UserDetail(newUser);
                dbContext.SaveChanges();

                Log("Fetched User Info for ID = " + user.UserID.ToString());

            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }



    }
}
