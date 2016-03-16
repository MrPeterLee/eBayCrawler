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
        private void LoadDebug()
        {

            // Fix up comboboxes in Search Tab
            //CboSiteFilter.Items.Add("NoFilter");
            //CboItemFilter.Items.Add("NoFilter");
            //CboSearchFilter.Items.Add("NoFilter");
            //CboSortBy.Items.Add("DefaultSort");
            if (debug == 1)
            {
                /*
                string[] enums = Enum.GetNames(typeof(SiteIDFilterCodeType));
                Log("SiteIDFilterCodeType");
                foreach (string item in enums)
                {
                    if (item != "CustomCode" & debug == 1)
                        Log("<ribbon:RibbonGalleryItem Content=" + item + "/>");
                }

                enums = Enum.GetNames(typeof(ItemTypeFilterCodeType));
                Log("ItemTypeFilterCodeType");
                foreach (string item in enums)
                {
                    if (item != "CustomCode" & debug == 1)
                        Log("<ribbon:RibbonGalleryItem Content=" + item + "/>");
                }

                enums = Enum.GetNames(typeof(CategoryListingsOrderCodeType));
                Log("CategoryListingsFilterCodeType");
                foreach (string item in enums)
                {
                    if (item != "CustomCode" & debug == 1)
                        Log("<ribbon:RibbonGalleryItem Content=" + item + "/>");
                }


                enums = Enum.GetNames(typeof(CategoryListingsSearchCodeType));
                Log("CategoryListingSearchFilterCodeType");
                foreach (string item in enums)
                {
                    if (item != "CustomCode" & debug == 1)
                        Log("<ribbon:RibbonGalleryItem Content=" + item + "/>");
                }

                //CboItemFilter.Items.CurrentPosition = 0;
                //  CboSearchType.SelectedIndex = 0;
                //    CboSiteFilter.SelectedIndex = 0;
                //     CboSort.SelectedIndex = 0;
                */
            }
        }

        public void GetEnvironmentInfo()
        {
            // Fully qualified path of the current directory
            
            Log("CurrentDirectory: " + Environment.CurrentDirectory);
            // Gets the NetBIOS name of this local computer

            Log("MachineName: "+ Environment.MachineName);
            // Version number of the OS

            Log("OSVersion: " + Environment.OSVersion.ToString());
            // Fully qualified path of the system directory

            Log("SystemDirectory: " + Environment.SystemDirectory);
            // Network domain name associated with the current user

            Log("UserDomainName: " + Environment.UserDomainName);
            // Whether the current process is running in user interactive mode

            Log("UserInteractive: "+ Environment.UserInteractive);
            // User name of the person who started the current thread

            Log("UserName: " + Environment.UserName);
            // Major, minor, build, and revision numbers of the CLR

            Log("CLRVersion: " + Environment.Version.ToString());
            // Amount of physical memory mapped to the process context

            Log("WorkingSet: " + Environment.WorkingSet);
            // Returns values of Environment variables enclosed in %%

            Log("ExpandEnvironmentVariables: " + 
                Environment.ExpandEnvironmentVariables("System drive: " + 
                "%SystemDrive% System root: %SystemRoot%"));
            // Array of string containing the names of the logical drives

            Log("LogicalDrives: " + String.Join(", ", 
                              Environment.GetLogicalDrives()));
            LabelTxt.Content = "Version Number :" +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            // Display version number
            Log("Program successfully initialized.");

            Log("eBay Fetch automates the process to fetch eBay data, which is done in 3 stages.");
            Log("    Stage 1: Search for auction listings base on predefined categories.");
            Log("    Stage 2: Fetch item details from search result.");
            Log("    Stage 3: Fetch sellers' and listings' information for ended listings.");


        }

    }
}
