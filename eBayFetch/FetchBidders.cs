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
        private int FetchBiddersFn(string TxtItemId)
        {
            try
            {
                GetAllBiddersCall apicall = new GetAllBiddersCall(GetContext());
                OfferTypeCollection bids = apicall.GetAllBidders(TxtItemId, (GetAllBiddersModeCodeType)Enum.Parse(typeof(GetAllBiddersModeCodeType), "ViewAll" ));
                
                foreach (OfferType offer in bids)
                {
                    string[] listparams = new string[6];
                    listparams[0] = offer.Action.ToString();
                    listparams[1] = offer.User.UserID;
                    listparams[2] = offer.Currency.ToString();
                    listparams[3] = offer.MaxBid.Value.ToString();
                    listparams[4] = offer.Quantity.ToString();
                    listparams[5] = offer.TimeBid.ToString();
                  
                }
                 
                int bidderNum = 0;
                foreach (OfferType offer in bids)
                {
                    bidderNum = bidderNum + 1;
                }
                Log("ItemID :" + TxtItemId + "has bidders : " + bidderNum.ToString());
                return bidderNum;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return -999;
            }
        }

   

    } 
}
