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

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Data.Objects;
using eBay.Services;
using eBay.Services.Finding;
using Slf;

using System.Xml;
using eBay.Service.Util;
using Samples.Helper;

namespace eBayFetch
{

    public partial class MainWindow
    {


        private void GeteBayTime()
        {
            try
            {
                GeteBayOfficialTimeCall apicall = new GeteBayOfficialTimeCall(GetContext());

                DateTime eBayTime = apicall.GeteBayOfficialTime();

                Log("Current eBay official data/time is: " + eBayTime.ToLongTimeString() + ", " + eBayTime.ToLongDateString());
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        private void GetCategoryListings(string CategoryTerm)
        {
            // Init log
            // This sample and the FindingKit use <a href="http://slf.codeplex.com/">Simple Logging Facade(SLF)</a>,
            // Here is a <a href="http://slf.codeplex.com/">good introdution</a> about SLF(for example, supported log levels, how to log to a file)
            
            LoggerService.SetLogger(new Slf.ConsoleLogger());
            ILogger logger = LoggerService.GetLogger();

            try
            {
                Log("Implementing GetCategoryListings()");

                ClientConfig config = new ClientConfig();
                // Initialize service end-point configuration
                config.EndPointAddress = "http://svcs.ebay.com/services/search/FindingService/v1";
                // set eBay developer account AppID

                config.ApplicationId = GetContext().ApiCredential.ApiAccount.Application;

                // Create a service client
                FindingServicePortTypeClient client = FindingServiceClientFactory.getServiceClient(config);

                // Create request object
                FindItemsByCategoryRequest request = new FindItemsByCategoryRequest();

                // Set request parameters
                //request.categoryId = new string[] { TxtRegion.Text };

                //string[] searchCatID = CategoryIDListBOx.Items.Cast<String>().ToArray();
                //int[] searchCatID = modArray.Select(int.Parse).ToArray();

                /*
                string[] searchCatID = new string[100];
                for (int i = 0; i < CategoryIDListBOx.Items.Count; i++ )
                {
                    searchCatID[i] = CategoryIDListBOx.Items[i].ToString();
                    Log("The " + i + 1 + "th Category Search ID is : " + searchCatID[i]);
                }
                */
                string[] searchCatID = new string[1];
                searchCatID[0] = CategoryTerm;

                request.categoryId = searchCatID;
                request.sortOrder = SortOrderType.EndTimeSoonest;
                request.sortOrderSpecified = true;

                // Some item filter I don't understand
                //request.itemFilter = new ItemFilter[]
                //{
                //    new ItemFilter()
                //    {
                //        name = ItemFilterType.EndTimeFrom,
                //        value = new string[]
                //        {
                //            DateTime.Now.Add(TimeSpan.FromHours(1).ToString("yyyy-MM-ddtHH:mm:ss"))
                //        }
                //    }
                //};


                PaginationInput pi = new PaginationInput();
                pi.entriesPerPage = int.MaxValue;
                pi.entriesPerPageSpecified = true;
                request.paginationInput = pi;

                // Call the service
                FindItemsByCategoryResponse response = client.findItemsByCategory(request);

                // Show output
                Log("Ack = " + response.ack);
                Log("Find " + response.searchResult.count + " items.");
                
                SearchItem[] fnditems = response.searchResult.item;


                        
                //ItemTypeCollection fnditems = apicall.GetCategoryListings(CategoryTerm);

                foreach (SearchItem fnditem in fnditems)
                {
                    Log("Now updating ItemID = " + fnditem.itemId.ToString() + ", Title = " + fnditem.title.ToString());
                    var newSearch = new S1SearchTable
                    {
                        ItemID = fnditem.itemId.ToString(),
                        Title = fnditem.title.ToString(),
                        FetchTime = DateTime.Now,
                        EndTimeDum = 0,
                        IsInPanelData = 0,
                        CategoryID = CategoryTerm
                    };

                    eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
                    dbContext.AddToS1SearchTable(newSearch);
                    dbContext.SaveChanges();
                }

                Log("Ack = " + response.ack);
                Log("Find " + response.searchResult.count + " items.");

                SearchItem[] items = response.searchResult.item;
                for (int i = 0; i < items.Length; i++)
                {
                    Log(items[i].title);
                    //logger.Info(items[i].title);
                }
                

                

                
                //Log("Category search has successfully completed, result count is: " + fnditems.Count.ToString());
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
                
}


        private void QuerySearchResult()
        {
            Log("Starting to reload Search output to:");
            Log("  ---- Recent search listings that have not yet obtained End Time Infomation.");
            eBayFetchSQLEntities dbContext = new eBayFetchSQLEntities();
            ObjectQuery<S1SearchTable> listings = dbContext.S1SearchTable;
            var query = from listing in dbContext.S1SearchTable
                        where (listing.EndTimeDum != 1 | listing.EndTimeDum == null)
                        select listing;
            GridPreData.ItemsSource = query.ToList();
        }





    }
}
