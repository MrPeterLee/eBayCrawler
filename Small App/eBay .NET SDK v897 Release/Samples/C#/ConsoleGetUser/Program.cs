using System;
using System.Configuration;
using System.Collections.Generic;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using System.IO;


namespace ConsoleGetUser
{
    /// <summary>
    /// A simple item adding sample,
    /// show basic flow to list an item to eBay Site using eBay SDK.
    /// </summary>
    class Program
    {
        private static ApiContext apiContext = null;

        static void Main(string[] args)
        {

            try
            {
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("+ Welcome to stupid Peter eBay App    +");
                Console.WriteLine("+ - This app gets payment details     +");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine();
                Console.WriteLine();

                //Initialize eBay ApiContext object
                ApiContext apiContext = GetApiContext();

                // Input an item ID
                Console.WriteLine("Enter an Item ID (i.e. 400816089628): ");
                string ItemString = Console.ReadLine();

                if (ItemString.Length<11)
                {
                    Console.WriteLine("The Item ID seems to be invalid. The ID is replaced by 400816089628");
                    ItemString = "400816089628";
                }
            
           /*
                //Create Call object and execute the Call
                GetUserCall apiCall = new GetUserCall(apiContext);

                apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
                apiCall.Execute();
                Console.WriteLine("Begin to call eBay API, please wait ...");

                Console.WriteLine("End to call eBay API, show call result ...");
                Console.WriteLine();

                //Handle the result returned
                Console.WriteLine("UserID: " +apiCall.User.UserID.ToString());
                Console.WriteLine("EIAS Token is: " +apiCall.User.EIASToken.ToString());
                Console.WriteLine();

                if (apiCall.User.eBayGoodStanding == true)
                    Console.WriteLine("User has good eBay standing");

                Console.WriteLine("Rating Star color: " +apiCall.User.FeedbackRatingStar.ToString());
                Console.WriteLine("Feedback score: " + apiCall.User.FeedbackScore.ToString());
                Console.WriteLine();

                Console.WriteLine("Total count of Negative Feedback: " + apiCall.User.UniqueNegativeFeedbackCount.ToString());
                Console.WriteLine("Total count of Neutral Feedback: " + apiCall.User.UniqueNeutralFeedbackCount.ToString());
                Console.WriteLine("Total count of Positive Feedback: " + apiCall.User.UniquePositiveFeedbackCount.ToString());
                Console.WriteLine();
*/
                // Get Item
                Console.WriteLine("Now connecting to eBay...");

                string addContent = "";

                // create a file to write to
                string pathItem = @"c:\temp\testApp.txt";
                // This text is added only once to the file. 
                
                if (!File.Exists(pathItem))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(pathItem))
                    {
                        sw.WriteLine("This file is used to test individual ebay call to see if they work properly...");
                    }
                }
               
                try {
                    apiContext.CallRetry = GetCallRetry(); 
                    GetItemCall apicall = new GetItemCall(GetContext()); 
                    apicall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll); 
                    apicall.IncludeItemSpecifics = true;

                    ItemType fetchedItem = apicall.GetItem(ItemString);

                    var payMethods = fetchedItem.PaymentMethods.ToArray();

                    Console.WriteLine();
                    Console.WriteLine("Display details of payment methods: ");
                    for (int i = 0; payMethods != null & i < fetchedItem.PaymentMethods.Count; i++)
                    {
                        Console.WriteLine("    Payment Method " + i + " : " + payMethods[i].ToString());
                        using (StreamWriter sw = File.AppendText(pathItem)) sw.WriteLine(" + .Payment Method " + i + " : " + payMethods[i].ToString());
                    }

                    Console.WriteLine();
                    Console.WriteLine("Display details of deposit amount: "); 
                    Console.WriteLine("    Deposit Amount: $" + fetchedItem.PaymentDetails.DepositAmount.Value.ToString());
                    using (StreamWriter sw = File.AppendText(pathItem)) sw.WriteLine(" + .Deposit Amount: $" + fetchedItem.PaymentDetails.DepositAmount.Value.ToString());

                    Console.WriteLine();
                    Console.WriteLine("Display details of hours to payment: "); 
                    Console.WriteLine("    Hours to payment: " + fetchedItem.PaymentDetails.HoursToDeposit.ToString() + "hours");
                    using (StreamWriter sw = File.AppendText(pathItem)) sw.WriteLine(" + .Hours to payment: " + fetchedItem.PaymentDetails.HoursToDeposit.ToString() + " hours");

                    Console.WriteLine();
                    Console.WriteLine(@"You can check downloaded data from c:\temp\testApp.txt");

                }
                catch (Exception ex) {
                    using (StreamWriter sw = File.AppendText(pathItem))
                    {
                        Console.WriteLine(ex.Message);
                        sw.WriteLine(ex.Message); 
                    }
                        
                }


            
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get user data : " + ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Done! Press any key to close the program.");
            Console.ReadKey();

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

        /// <summary>
        /// Populate eBay SDK ApiContext object with data from application configuration file
        /// </summary>
        /// <returns>ApiContext object</returns>
        static ApiContext GetApiContext()
        {
            //apiContext is a singleton,
            //to avoid duplicate configuration reading
            if (apiContext != null)
            {
                return apiContext;
            }
            else
            {
                apiContext = new ApiContext();

                //set Api Server Url
                apiContext.SoapApiServerUrl =
                    ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
                //set Api Token to access eBay Api Server
                ApiCredential apiCredential = new ApiCredential();
                apiCredential.eBayToken =
                    ConfigurationManager.AppSettings["UserAccount.ApiToken"];
                apiContext.ApiCredential = apiCredential;
                //set eBay Site target to US
                apiContext.Site = SiteCodeType.US;

                //set Api logging
                apiContext.ApiLogManager = new ApiLogManager();
                apiContext.ApiLogManager.ApiLoggerList.Add(
                    new FileLogger("listing_log.txt", true, true, true)
                    );
                apiContext.ApiLogManager.EnableLogging = true;


                return apiContext;
            }
        }


        static ApiContext GetContext()
        {
            try
            {
                string accD, accA, accC, accT;

                ApiContext context = new ApiContext();

                // Set the server side urls fro sandbox environment
                context.SoapApiServerUrl = "https://api.ebay.com/wsapi";
                context.SignInUrl = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn";
                context.EPSServerUrl = "https://api.ebay.com/ws/api.dll";

                // Set the server side urls fro production environment
                //context.SoapApiServerUrl = "https://api.ebay.com/wsapi";
                //context.SignInUrl = "https://api.ebay.com/ws/api.dll";
                //context.EPSServerUrl = "https://api.ebay.com/ws/api.dll";

                //set the logging
                string logFile = "LogFile.txt";
                context.ApiLogManager = new ApiLogManager();
                context.ApiLogManager.ApiLoggerList.Add(new FileLogger(logFile, true, true, true));
                context.ApiLogManager.EnableLogging = false;
                context.Site = eBay.Service.Core.Soap.SiteCodeType.US;

                // Credentials : Add various accounts in this section

                    accD = "4897b3fe-78e7-4d2d-b5ee-7b4466e1af77";
                    accA = "Universi-8583-4e4a-a5fe-b0120156122c";
                    accC = "599249a6-40fb-4a03-a483-2361135b66c9";
                    accT = "AgAAAA**AQAAAA**aAAAAA**GcJvVA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AHkIehDJiBogSdj6x9nY+seQ**pJUCAA**AAMAAA**qcToNIbOsclQi/WQoJLwOuCWHGCUdn6zHBxNhl1eSyG9MacNjWIjH+jUElVzX8mqTAvJGsDp9cmhta6wKvb7VkjD8DoWktnDWkQZaV7y7iAq1g02ngfMOxTuxMaZfXmlZ5AZS+hqOMK8JZInhhyGDCKH4JsJFaMxANS67gmmaJaSoSvCYTH93DnGaHar1SsNyB88rPn3Knzp7d3ictFUZTIhMs5sni3DELP3ax2SM32CwvuSoyC0c7MR7xRTeCYEXARfjMf7lbdUiSdoNReIzTbjOeF0PjXoP5f0As5zfDTZSqc+15PbeBNf6eMZAS+SGWhpCeB0hY15+q0kiyIyyPulqNkEHctoqs0pGMO/ZOMzh+0K94DILCMFOKspXsiA3P7MHXMOCAvDGLdQptS/d2YoD+obOOT+mfsXZL4fosJw33aTqDdC4gS3DGv9+W1hmx6raxZmSbVZzoUT01LJ/lz2jP/DIa52D3x4571i40ZKgCq03CYiinT/fokrpvOT1fkRwJ63e0kmsTmrUIQfVgnxCVmIuliOo5I7RUifKcGvmaIKwpqZUKXuWeecvkkP1mzN+h5fKC917kH/gCYvnLX04JaMlkrt7Q0Mcb5aIcY6tItAbQoURWTMxPcmMzZ8yILmKYZ3ttTfpzozx9UBJaH8q7LDf9FL4Oh/OIf6slHsPwNvpW/r7Gu8uLQUlkirdzpxwwOilVRepifqpRgWZKgjwL9F8+dhRJXcn1MGQcxedTfgamWukTFXVLXooXO0";
                

                context.ApiCredential.ApiAccount.Developer = accD;
                context.ApiCredential.ApiAccount.Application = accA;
                context.ApiCredential.ApiAccount.Certificate = accC;
                context.ApiCredential.eBayToken = accT;

                return context;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
