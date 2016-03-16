/*
© 2011-2013 eBay Inc., All Rights Reserved
Licensed under CDDL 1.0 - http://opensource.org/licenses/cddl1.php
*/ 

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
using eBay.Service.Core.Soap;

namespace Itemspecifics
{
    class Program
    {
        static void Main(string[] args)
        {
            //create the context
            ApiContext context = new ApiContext();

            //set the User token
            context.ApiCredential.eBayToken = "AgAAAA**AQAAAA**aAAAAA**NrJZUw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6AEk4CnC5KKpAidj6x9nY+seQ**6vQAAA**AAMAAA**L+28EHJN9l7AdyHMfejhnd01A1NY1CdcMK/P7ikHJqCJfM6gytEhG4VZ4UqJVHomv1suY4El+AZyO8mGLqN+A1roP2cfGlYfu6qap3IMfmmIoJdIwRmYjxa++obRGXPNZMmI2sD9tSvxOpAeg0zAmpfUBQC1dBDK/+bQxP/l7IXbPh8ruKgXbfKQl0W3pzocgnMau9BclVQsNnlN+mGpBMXgU8nTM2L/pbwy9b/Fi7+3m5xE1ytnIeMYOM33tn7643BnD+smVuKnA3qUy/k6LZlPWAbMJbnKfyQLoz52ZIgdpbs6Y4Br6nM4lPAWlWY9rIgStNDAmaIlfg6pl4gmBHaBXZb4vPZxyQLEYyEuaZDBRA9oYnCpELibXfsxeYk94Yczc/gvkdgDFVRt7CoRy0WSQvU9stU3oXRO8Yo1eiOVT00bwrcyQqjomEgP4C5vfmlXWRzun88gMxLX47atTLpqrxBswRAT80Uu9Jm9fbISjcOlLgH/jlQeaDaIsBDR7B1Lb3BiIQK4w6bYEdIlDv9J8o9hq/g9E+or7s6BCBPYr+NSpk2FY2W3+OQ8WC4FVjBo3T1KJAy1fmI4g7UsHTqLm3AARJYNVGB5h5Qn8+aNuhdT899TaW4Pu48owKZze8KqBtEVBo6mymWFfuAK2TwnnsyC95XdqPnQSqqsqzE96+ijDVEe0EJVRQUhidGSIwIBr7hK4bFqry5qrqvDS18e90i/Wxjvs7dRrwurcMyrnYIWgeD8XwRAoAJ3k1v2";

            //set the server url
            context.SoapApiServerUrl = "https://api.ebay.com/wsapi";

            //enable logging
            context.ApiLogManager = new ApiLogManager();
            context.ApiLogManager.ApiLoggerList.Add(new FileLogger("log.txt", true, true, true));
            context.ApiLogManager.EnableLogging = true;

            //set the version
            context.Version = "817";
            context.Site = SiteCodeType.US;
            GetCategoriesCall gc = new GetCategoriesCall(context);
            gc.ViewAllNodes = true;
            gc.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            gc.CategoryParent = new StringCollection();

            //For the sake of example, only one parent category is being added. Being a collection
            //this can take more than one parent categoryIDs
            gc.CategoryParent.Add("6447");

            gc.Execute();
            if (gc.ApiResponse.Ack == AckCodeType.Success)
            {
                //Store the version number so that you can compare it the next time you make a call
                //and verify if the category hierarchy has been updated.
                Console.WriteLine(gc.ApiResponse.Ack + " : " + gc.ApiResponse.CategoryVersion);
                CategoryTypeCollection cats = gc.ApiResponse.CategoryArray;

                //GetCategorySpecifics call
                GetCategorySpecificsCall gcs = new GetCategorySpecificsCall(context);
                gcs.CategoryIDList = new StringCollection();


                foreach (CategoryType category in cats)
                {
                    //Get ItemSpecific data for leaf categories
                    if (category.LeafCategory == true)
                    {
                        //initialize the category list
                        gcs.CategoryIDList.Add(category.CategoryID);
                    }

                }
                gcs.Execute();

                if (gcs.ApiResponse.Ack == AckCodeType.Success)
                {
                    //iterate through each recommendation
                    if (gcs.RecommendationList != null)
                    {
                        Console.WriteLine("CategoryID" + "\t" + "Name" + "\t" + "Value" + "\t" + "MinValues" + "\t"
                         + "MaxValues" + "\t" + "SelectionMode" + "\t" + "VariationSpecifics"
                         + "\t" + "ParentName" + "\t" + "ParentValue");

                        foreach (RecommendationsType recommendations in gcs.RecommendationList)
                        {
                            string catID = recommendations.CategoryID;
                            if (recommendations.NameRecommendation.Count == 0)
                            {
                                //Category does not have any recommendations, so just write the input info
                                Console.WriteLine(catID + ": Category does not have any recommendations");
                            }
                            foreach (NameRecommendationType recommendation in recommendations.NameRecommendation)
                            {
                                string name = recommendation.Name;
                                string parent = "";


                                RecommendationValidationRulesType validations = recommendation.ValidationRules;
                                //If VariationSpecifics is not returned in the response, .NET sets it to the default value
                                //Hence set to disabled and set it only if it is actually returned in the response
                                string variationsEnabled = "";
                                if (validations.VariationSpecificsSpecified)
                                {
                                    variationsEnabled = validations.VariationSpecifics.ToString();
                                }
                                string validation = validations.MinValues + "\t" + validations.MaxValues + "\t"
                                    + validations.SelectionMode + "\t" + variationsEnabled;

                                //there are no values for this recommendation, so write it out at this point
                                if (recommendation.ValueRecommendation.Count == 0)
                                {
                                    //Category does not have any recommendations, so just write the input info
                                    Console.WriteLine(catID + "\t" + name + "\t" + validation);
                                }
                                foreach (ValueRecommendationType value in recommendation.ValueRecommendation)
                                {
                                    if (value.ValidationRules != null)
                                    {
                                        parent = value.ValidationRules.Relationship[0].ParentName + "\t" + value.ValidationRules.Relationship[0].ParentValue + "\t";
                                    }
                                    Console.WriteLine(catID + "\t" + name + "\t" + value.Value + "\t" + validation + "\t" + parent);

                                }
                            }
                        }



                    }


                }
            }



            Console.ReadLine();
        }
    }

}

