﻿/*
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
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
        private void ExportFn()
        {
            SqlConnection cnn;
            string connectionString = null;
            string sql = null;
            string data = null;
            int i = 0;
            int j = 0;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

//            connectionString = "data source=servername;initial catalog=databasename;user id=username;password=password;";
            connectionString = "data source=Home;initial catalog=eBayFetchSQL;user id=eBayFetch;password=123123;";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = "SELECT * FROM Product";
            SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
            DataSet ds = new DataSet();
            dscmd.Fill(ds);

            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                {
                    data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                    xlWorkSheet.Cells[i + 1, j + 1] = data;
                }
            }

            xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

        Log("Excel file created , you can find the file c:\\csharp.net-informations.xls");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            Log("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}

*/