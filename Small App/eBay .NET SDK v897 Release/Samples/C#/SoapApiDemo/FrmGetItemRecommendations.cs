#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using eBay.Service.Util;

namespace SoapLibraryDemo
{
	/// <summary>
	/// Summary description for FrmGetItemRecommendations.
	/// </summary>
	public class FrmGetItemRecommendations : System.Windows.Forms.Form
	{
		public ApiContext Context;
		private System.Windows.Forms.TabControl TabSettings;
		private System.Windows.Forms.TabPage TabListingAnalyzer;
		private System.Windows.Forms.TabPage TabProductPricing;
		private System.Windows.Forms.Label LblAverageStartPrice;
		private System.Windows.Forms.TextBox TxtCatalogTitle;
		private System.Windows.Forms.Label LblCatalogTitle;
		internal System.Windows.Forms.Button BtnProductPricing;
		private System.Windows.Forms.TextBox TxtAverageSoldPrice;
		private System.Windows.Forms.Label LblAverageSoldPrice;
		private System.Windows.Forms.TabPage TabSuggestedAttributes;
		private System.Windows.Forms.Label LblListingFlow;
		private System.Windows.Forms.ComboBox CboListingFlow;
		internal System.Windows.Forms.Button BtnListingAnalyzer;
		private System.Windows.Forms.GroupBox GrpListingTips;
		private System.Windows.Forms.Button BtnSuggestedAttr;
		private System.Windows.Forms.Label LblQuery;
		private System.Windows.Forms.TextBox TxtQuery;
		private System.Windows.Forms.GroupBox GrpResult;
		private System.Windows.Forms.GroupBox GrpSuggestedAttr;
		private System.Windows.Forms.GroupBox GrpSuggestedProducts;
		private System.Windows.Forms.Label LblTitle;
		private System.Windows.Forms.Label LblReservePrice;
		private System.Windows.Forms.Label LblStartPrice;
		private System.Windows.Forms.Label LblPrimaryCategory;
		private System.Windows.Forms.Label LblSecondaryCategory;
		private System.Windows.Forms.Label LblBuyItNowPrice;
		private System.Windows.Forms.ColumnHeader ClmID;
		private System.Windows.Forms.ColumnHeader ClmPriority;
		private System.Windows.Forms.ColumnHeader ClmMessage;
		private System.Windows.Forms.ColumnHeader ClmFieldID;
		private System.Windows.Forms.ColumnHeader ClmFieldTip;
		private System.Windows.Forms.ColumnHeader ClmFieldText;
		private System.Windows.Forms.ListView LstTips;
		private System.Windows.Forms.ListView lstProduct;
		private System.Windows.Forms.ColumnHeader ClmTitle;
		private System.Windows.Forms.ColumnHeader ClmInfoID;
		private System.Windows.Forms.ColumnHeader ClmAvgStartPrice;
		private System.Windows.Forms.ColumnHeader ClmAvgSoldPrice;
		private System.Windows.Forms.ListView LstAttr;
		private System.Windows.Forms.ColumnHeader ClmCSID;
		private System.Windows.Forms.ColumnHeader ClmVersion;
		private System.Windows.Forms.ColumnHeader ClmNoOfAttr;
		private System.Windows.Forms.TextBox TxtTitle;
		private System.Windows.Forms.TextBox TxtReservePrice;
		private System.Windows.Forms.TextBox TxtStartPrice;
		private System.Windows.Forms.TextBox TxtPrimaryCategory;
		private System.Windows.Forms.TextBox TxtSecondaryCategory;
		private System.Windows.Forms.TextBox TxtBuyItNowPrice;
		private System.Windows.Forms.Label LblExternalProductID;
		private System.Windows.Forms.TextBox TxtExternalProductID;
		private System.Windows.Forms.TextBox TxtAverageStartPrice;
		private System.Windows.Forms.Button BtnGetCategories;
		private System.Windows.Forms.TextBox TxtItemID;
		private System.Windows.Forms.Label LblItemID;
		private System.Windows.Forms.ColumnHeader ClmHelpURL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button BtnGetItem;
		private ItemType fetchedItem;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrmGetItemRecommendations()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.BtnGetItem.Click += new System.EventHandler(this.BtnGetItem_Click);

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.TabSettings = new System.Windows.Forms.TabControl();
			this.TabListingAnalyzer = new System.Windows.Forms.TabPage();
			this.TxtItemID = new System.Windows.Forms.TextBox();
			this.LblItemID = new System.Windows.Forms.Label();
			this.GrpListingTips = new System.Windows.Forms.GroupBox();
			this.LstTips = new System.Windows.Forms.ListView();
			this.ClmID = new System.Windows.Forms.ColumnHeader();
			this.ClmPriority = new System.Windows.Forms.ColumnHeader();
			this.ClmMessage = new System.Windows.Forms.ColumnHeader();
			this.ClmFieldID = new System.Windows.Forms.ColumnHeader();
			this.ClmFieldTip = new System.Windows.Forms.ColumnHeader();
			this.ClmFieldText = new System.Windows.Forms.ColumnHeader();
			this.ClmHelpURL = new System.Windows.Forms.ColumnHeader();
			this.BtnListingAnalyzer = new System.Windows.Forms.Button();
			this.CboListingFlow = new System.Windows.Forms.ComboBox();
			this.LblListingFlow = new System.Windows.Forms.Label();
			this.TabProductPricing = new System.Windows.Forms.TabPage();
			this.GrpResult = new System.Windows.Forms.GroupBox();
			this.TxtAverageStartPrice = new System.Windows.Forms.TextBox();
			this.TxtAverageSoldPrice = new System.Windows.Forms.TextBox();
			this.LblAverageStartPrice = new System.Windows.Forms.Label();
			this.LblAverageSoldPrice = new System.Windows.Forms.Label();
			this.TxtCatalogTitle = new System.Windows.Forms.TextBox();
			this.LblCatalogTitle = new System.Windows.Forms.Label();
			this.TxtExternalProductID = new System.Windows.Forms.TextBox();
			this.BtnProductPricing = new System.Windows.Forms.Button();
			this.LblExternalProductID = new System.Windows.Forms.Label();
			this.TabSuggestedAttributes = new System.Windows.Forms.TabPage();
			this.GrpSuggestedProducts = new System.Windows.Forms.GroupBox();
			this.lstProduct = new System.Windows.Forms.ListView();
			this.ClmTitle = new System.Windows.Forms.ColumnHeader();
			this.ClmInfoID = new System.Windows.Forms.ColumnHeader();
			this.ClmAvgStartPrice = new System.Windows.Forms.ColumnHeader();
			this.ClmAvgSoldPrice = new System.Windows.Forms.ColumnHeader();
			this.GrpSuggestedAttr = new System.Windows.Forms.GroupBox();
			this.LstAttr = new System.Windows.Forms.ListView();
			this.ClmCSID = new System.Windows.Forms.ColumnHeader();
			this.ClmVersion = new System.Windows.Forms.ColumnHeader();
			this.ClmNoOfAttr = new System.Windows.Forms.ColumnHeader();
			this.TxtQuery = new System.Windows.Forms.TextBox();
			this.LblQuery = new System.Windows.Forms.Label();
			this.BtnSuggestedAttr = new System.Windows.Forms.Button();
			this.TxtTitle = new System.Windows.Forms.TextBox();
			this.LblTitle = new System.Windows.Forms.Label();
			this.TxtReservePrice = new System.Windows.Forms.TextBox();
			this.LblReservePrice = new System.Windows.Forms.Label();
			this.TxtStartPrice = new System.Windows.Forms.TextBox();
			this.LblStartPrice = new System.Windows.Forms.Label();
			this.TxtPrimaryCategory = new System.Windows.Forms.TextBox();
			this.LblPrimaryCategory = new System.Windows.Forms.Label();
			this.TxtSecondaryCategory = new System.Windows.Forms.TextBox();
			this.LblSecondaryCategory = new System.Windows.Forms.Label();
			this.TxtBuyItNowPrice = new System.Windows.Forms.TextBox();
			this.LblBuyItNowPrice = new System.Windows.Forms.Label();
			this.BtnGetCategories = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.BtnGetItem = new System.Windows.Forms.Button();
			this.TabSettings.SuspendLayout();
			this.TabListingAnalyzer.SuspendLayout();
			this.GrpListingTips.SuspendLayout();
			this.TabProductPricing.SuspendLayout();
			this.GrpResult.SuspendLayout();
			this.TabSuggestedAttributes.SuspendLayout();
			this.GrpSuggestedProducts.SuspendLayout();
			this.GrpSuggestedAttr.SuspendLayout();
			this.SuspendLayout();
			// 
			// TabSettings
			// 
			this.TabSettings.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.TabListingAnalyzer,
																					  this.TabProductPricing,
																					  this.TabSuggestedAttributes});
			this.TabSettings.Location = new System.Drawing.Point(40, 168);
			this.TabSettings.Name = "TabSettings";
			this.TabSettings.SelectedIndex = 0;
			this.TabSettings.Size = new System.Drawing.Size(560, 372);
			this.TabSettings.TabIndex = 41;
			// 
			// TabListingAnalyzer
			// 
			this.TabListingAnalyzer.Controls.AddRange(new System.Windows.Forms.Control[] {
																							 this.TxtItemID,
																							 this.LblItemID,
																							 this.GrpListingTips,
																							 this.BtnListingAnalyzer,
																							 this.CboListingFlow,
																							 this.LblListingFlow});
			this.TabListingAnalyzer.Location = new System.Drawing.Point(4, 22);
			this.TabListingAnalyzer.Name = "TabListingAnalyzer";
			this.TabListingAnalyzer.Size = new System.Drawing.Size(552, 346);
			this.TabListingAnalyzer.TabIndex = 0;
			this.TabListingAnalyzer.Text = "Listing Analyzer";
			// 
			// TxtItemID
			// 
			this.TxtItemID.Location = new System.Drawing.Point(384, 16);
			this.TxtItemID.Name = "TxtItemID";
			this.TxtItemID.Size = new System.Drawing.Size(144, 20);
			this.TxtItemID.TabIndex = 85;
			this.TxtItemID.Text = "";
			this.TxtItemID.Visible = false;
			// 
			// LblItemID
			// 
			this.LblItemID.Location = new System.Drawing.Point(320, 16);
			this.LblItemID.Name = "LblItemID";
			this.LblItemID.Size = new System.Drawing.Size(48, 23);
			this.LblItemID.TabIndex = 84;
			this.LblItemID.Text = "Item ID:";
			this.LblItemID.Visible = false;
			// 
			// GrpListingTips
			// 
			this.GrpListingTips.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.LstTips});
			this.GrpListingTips.Location = new System.Drawing.Point(16, 96);
			this.GrpListingTips.Name = "GrpListingTips";
			this.GrpListingTips.Size = new System.Drawing.Size(520, 216);
			this.GrpListingTips.TabIndex = 65;
			this.GrpListingTips.TabStop = false;
			this.GrpListingTips.Text = "Listing Tips";
			// 
			// LstTips
			// 
			this.LstTips.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.ClmID,
																					  this.ClmFieldID,
																					  this.ClmPriority,
																					  this.ClmMessage,
																					  this.ClmFieldTip,
																					  this.ClmFieldText,
																					  this.ClmHelpURL});
			this.LstTips.GridLines = true;
			this.LstTips.Location = new System.Drawing.Point(16, 24);
			this.LstTips.Name = "LstTips";
			this.LstTips.Size = new System.Drawing.Size(496, 176);
			this.LstTips.TabIndex = 13;
			this.LstTips.View = System.Windows.Forms.View.Details;
			this.LstTips.SelectedIndexChanged += new System.EventHandler(this.LstTips_SelectedIndexChanged);
			// 
			// ClmID
			// 
			this.ClmID.Text = "ID";
			this.ClmID.Width = 40;
			// 
			// ClmPriority
			// 
			this.ClmPriority.Text = "Priority";
			this.ClmPriority.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ClmPriority.Width = 50;
			// 
			// ClmMessage
			// 
			this.ClmMessage.Text = "Message";
			this.ClmMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ClmMessage.Width = 100;
			// 
			// ClmFieldID
			// 
			this.ClmFieldID.Text = "Field ID";
			this.ClmFieldID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ClmFieldID.Width = 53;
			// 
			// ClmFieldTip
			// 
			this.ClmFieldTip.Text = "Field Tip";
			this.ClmFieldTip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ClmFieldTip.Width = 86;
			// 
			// ClmFieldText
			// 
			this.ClmFieldText.Text = "Field Text";
			this.ClmFieldText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// ClmHelpURL
			// 
			this.ClmHelpURL.Text = "Help URL";
			this.ClmHelpURL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ClmHelpURL.Width = 95;
			// 
			// BtnListingAnalyzer
			// 
			this.BtnListingAnalyzer.Location = new System.Drawing.Point(120, 56);
			this.BtnListingAnalyzer.Name = "BtnListingAnalyzer";
			this.BtnListingAnalyzer.Size = new System.Drawing.Size(176, 23);
			this.BtnListingAnalyzer.TabIndex = 64;
			this.BtnListingAnalyzer.Text = "Run Listing Analyzer Engine";
			this.BtnListingAnalyzer.Click += new System.EventHandler(this.BtnListingAnalyzer_Click);
			// 
			// CboListingFlow
			// 
			this.CboListingFlow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CboListingFlow.Location = new System.Drawing.Point(112, 16);
			this.CboListingFlow.Name = "CboListingFlow";
			this.CboListingFlow.Size = new System.Drawing.Size(176, 21);
			this.CboListingFlow.TabIndex = 58;
			this.CboListingFlow.SelectedIndexChanged += new System.EventHandler(this.CboListingFlow_SelectedIndexChanged);
			// 
			// LblListingFlow
			// 
			this.LblListingFlow.Location = new System.Drawing.Point(24, 16);
			this.LblListingFlow.Name = "LblListingFlow";
			this.LblListingFlow.Size = new System.Drawing.Size(72, 23);
			this.LblListingFlow.TabIndex = 0;
			this.LblListingFlow.Text = "Listing Flow:";
			// 
			// TabProductPricing
			// 
			this.TabProductPricing.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.GrpResult,
																							this.TxtExternalProductID,
																							this.BtnProductPricing,
																							this.LblExternalProductID});
			this.TabProductPricing.Location = new System.Drawing.Point(4, 22);
			this.TabProductPricing.Name = "TabProductPricing";
			this.TabProductPricing.Size = new System.Drawing.Size(552, 346);
			this.TabProductPricing.TabIndex = 3;
			this.TabProductPricing.Text = "Product Pricing";
			this.TabProductPricing.Visible = false;
			// 
			// GrpResult
			// 
			this.GrpResult.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.TxtAverageStartPrice,
																					this.TxtAverageSoldPrice,
																					this.LblAverageStartPrice,
																					this.LblAverageSoldPrice,
																					this.TxtCatalogTitle,
																					this.LblCatalogTitle});
			this.GrpResult.Location = new System.Drawing.Point(16, 112);
			this.GrpResult.Name = "GrpResult";
			this.GrpResult.Size = new System.Drawing.Size(520, 224);
			this.GrpResult.TabIndex = 67;
			this.GrpResult.TabStop = false;
			this.GrpResult.Text = "Result";
			// 
			// TxtAverageStartPrice
			// 
			this.TxtAverageStartPrice.Location = new System.Drawing.Point(192, 68);
			this.TxtAverageStartPrice.Name = "TxtAverageStartPrice";
			this.TxtAverageStartPrice.Size = new System.Drawing.Size(256, 20);
			this.TxtAverageStartPrice.TabIndex = 23;
			this.TxtAverageStartPrice.Text = "";
			// 
			// TxtAverageSoldPrice
			// 
			this.TxtAverageSoldPrice.Location = new System.Drawing.Point(192, 104);
			this.TxtAverageSoldPrice.Name = "TxtAverageSoldPrice";
			this.TxtAverageSoldPrice.Size = new System.Drawing.Size(256, 20);
			this.TxtAverageSoldPrice.TabIndex = 65;
			this.TxtAverageSoldPrice.Text = "";
			// 
			// LblAverageStartPrice
			// 
			this.LblAverageStartPrice.Location = new System.Drawing.Point(80, 68);
			this.LblAverageStartPrice.Name = "LblAverageStartPrice";
			this.LblAverageStartPrice.Size = new System.Drawing.Size(200, 16);
			this.LblAverageStartPrice.TabIndex = 24;
			this.LblAverageStartPrice.Text = "Average Start Price:";
			// 
			// LblAverageSoldPrice
			// 
			this.LblAverageSoldPrice.Location = new System.Drawing.Point(80, 104);
			this.LblAverageSoldPrice.Name = "LblAverageSoldPrice";
			this.LblAverageSoldPrice.Size = new System.Drawing.Size(200, 16);
			this.LblAverageSoldPrice.TabIndex = 66;
			this.LblAverageSoldPrice.Text = "Average Sold Price:";
			// 
			// TxtCatalogTitle
			// 
			this.TxtCatalogTitle.Location = new System.Drawing.Point(192, 32);
			this.TxtCatalogTitle.Name = "TxtCatalogTitle";
			this.TxtCatalogTitle.Size = new System.Drawing.Size(256, 20);
			this.TxtCatalogTitle.TabIndex = 2;
			this.TxtCatalogTitle.Text = "";
			// 
			// LblCatalogTitle
			// 
			this.LblCatalogTitle.Location = new System.Drawing.Point(80, 32);
			this.LblCatalogTitle.Name = "LblCatalogTitle";
			this.LblCatalogTitle.Size = new System.Drawing.Size(192, 16);
			this.LblCatalogTitle.TabIndex = 22;
			this.LblCatalogTitle.Text = "Catalog Title:";
			// 
			// TxtExternalProductID
			// 
			this.TxtExternalProductID.Location = new System.Drawing.Point(216, 16);
			this.TxtExternalProductID.Name = "TxtExternalProductID";
			this.TxtExternalProductID.Size = new System.Drawing.Size(208, 20);
			this.TxtExternalProductID.TabIndex = 64;
			this.TxtExternalProductID.Text = "79328:2:1431:561576419:57669919:391bc1eb4871c4a4e930a52fca6eccfd:1:1:1:1348602270" +
				"";
			// 
			// BtnProductPricing
			// 
			this.BtnProductPricing.Location = new System.Drawing.Point(168, 56);
			this.BtnProductPricing.Name = "BtnProductPricing";
			this.BtnProductPricing.Size = new System.Drawing.Size(168, 23);
			this.BtnProductPricing.TabIndex = 63;
			this.BtnProductPricing.Text = "Run Product Pricing Engine";
			this.BtnProductPricing.Click += new System.EventHandler(this.BtnProductPricing_Click);
			// 
			// LblExternalProductID
			// 
			this.LblExternalProductID.Location = new System.Drawing.Point(104, 16);
			this.LblExternalProductID.Name = "LblExternalProductID";
			this.LblExternalProductID.Size = new System.Drawing.Size(112, 16);
			this.LblExternalProductID.TabIndex = 20;
			this.LblExternalProductID.Text = "External Product ID:";
			// 
			// TabSuggestedAttributes
			// 
			this.TabSuggestedAttributes.Controls.AddRange(new System.Windows.Forms.Control[] {
																								 this.GrpSuggestedProducts,
																								 this.GrpSuggestedAttr,
																								 this.TxtQuery,
																								 this.LblQuery,
																								 this.BtnSuggestedAttr});
			this.TabSuggestedAttributes.Location = new System.Drawing.Point(4, 22);
			this.TabSuggestedAttributes.Name = "TabSuggestedAttributes";
			this.TabSuggestedAttributes.Size = new System.Drawing.Size(552, 346);
			this.TabSuggestedAttributes.TabIndex = 1;
			this.TabSuggestedAttributes.Text = "Suggested Attributes";
			this.TabSuggestedAttributes.Visible = false;
			// 
			// GrpSuggestedProducts
			// 
			this.GrpSuggestedProducts.Controls.AddRange(new System.Windows.Forms.Control[] {
																							   this.lstProduct});
			this.GrpSuggestedProducts.Location = new System.Drawing.Point(280, 88);
			this.GrpSuggestedProducts.Name = "GrpSuggestedProducts";
			this.GrpSuggestedProducts.Size = new System.Drawing.Size(256, 240);
			this.GrpSuggestedProducts.TabIndex = 75;
			this.GrpSuggestedProducts.TabStop = false;
			this.GrpSuggestedProducts.Text = "Suggested Products";
			// 
			// lstProduct
			// 
			this.lstProduct.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						 this.ClmTitle,
																						 this.ClmInfoID,
																						 this.ClmAvgStartPrice,
																						 this.ClmAvgSoldPrice});
			this.lstProduct.GridLines = true;
			this.lstProduct.Location = new System.Drawing.Point(16, 24);
			this.lstProduct.Name = "lstProduct";
			this.lstProduct.Size = new System.Drawing.Size(224, 200);
			this.lstProduct.TabIndex = 15;
			this.lstProduct.View = System.Windows.Forms.View.Details;
			// 
			// ClmTitle
			// 
			this.ClmTitle.Text = "Title";
			this.ClmTitle.Width = 40;
			// 
			// ClmInfoID
			// 
			this.ClmInfoID.Text = "Info ID";
			this.ClmInfoID.Width = 50;
			// 
			// ClmAvgStartPrice
			// 
			this.ClmAvgStartPrice.Text = "Avg Start Price";
			this.ClmAvgStartPrice.Width = 70;
			// 
			// ClmAvgSoldPrice
			// 
			this.ClmAvgSoldPrice.Text = "Avg Sold Price";
			this.ClmAvgSoldPrice.Width = 70;
			// 
			// GrpSuggestedAttr
			// 
			this.GrpSuggestedAttr.Controls.AddRange(new System.Windows.Forms.Control[] {
																						   this.LstAttr});
			this.GrpSuggestedAttr.Location = new System.Drawing.Point(16, 88);
			this.GrpSuggestedAttr.Name = "GrpSuggestedAttr";
			this.GrpSuggestedAttr.Size = new System.Drawing.Size(256, 240);
			this.GrpSuggestedAttr.TabIndex = 74;
			this.GrpSuggestedAttr.TabStop = false;
			this.GrpSuggestedAttr.Text = "Suggested Attributes   ";
			// 
			// LstAttr
			// 
			this.LstAttr.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.ClmCSID,
																					  this.ClmVersion,
																					  this.ClmNoOfAttr});
			this.LstAttr.GridLines = true;
			this.LstAttr.Location = new System.Drawing.Point(16, 24);
			this.LstAttr.Name = "LstAttr";
			this.LstAttr.Size = new System.Drawing.Size(224, 200);
			this.LstAttr.TabIndex = 16;
			this.LstAttr.View = System.Windows.Forms.View.Details;
			// 
			// ClmCSID
			// 
			this.ClmCSID.Text = "CSID";
			// 
			// ClmVersion
			// 
			this.ClmVersion.Text = "Version";
			this.ClmVersion.Width = 50;
			// 
			// ClmNoOfAttr
			// 
			this.ClmNoOfAttr.Text = "# of Attributes";
			this.ClmNoOfAttr.Width = 110;
			// 
			// TxtQuery
			// 
			this.TxtQuery.Location = new System.Drawing.Point(192, 16);
			this.TxtQuery.Name = "TxtQuery";
			this.TxtQuery.Size = new System.Drawing.Size(178, 20);
			this.TxtQuery.TabIndex = 73;
			this.TxtQuery.Text = "DELL Inspiron";
			// 
			// LblQuery
			// 
			this.LblQuery.Location = new System.Drawing.Point(136, 16);
			this.LblQuery.Name = "LblQuery";
			this.LblQuery.Size = new System.Drawing.Size(48, 23);
			this.LblQuery.TabIndex = 72;
			this.LblQuery.Text = "Query:";
			// 
			// BtnSuggestedAttr
			// 
			this.BtnSuggestedAttr.Location = new System.Drawing.Point(184, 56);
			this.BtnSuggestedAttr.Name = "BtnSuggestedAttr";
			this.BtnSuggestedAttr.Size = new System.Drawing.Size(184, 24);
			this.BtnSuggestedAttr.TabIndex = 70;
			this.BtnSuggestedAttr.Text = "Run Suggested Attributes Engine";
			this.BtnSuggestedAttr.Click += new System.EventHandler(this.BtnSuggestedAttr_Click);
			// 
			// TxtTitle
			// 
			this.TxtTitle.Location = new System.Drawing.Point(80, 40);
			this.TxtTitle.Name = "TxtTitle";
			this.TxtTitle.Size = new System.Drawing.Size(208, 20);
			this.TxtTitle.TabIndex = 75;
			this.TxtTitle.Text = "DELL new";
			this.TxtTitle.TextChanged += new System.EventHandler(this.TxtTitle_TextChanged);
			// 
			// LblTitle
			// 
			this.LblTitle.Location = new System.Drawing.Point(40, 39);
			this.LblTitle.Name = "LblTitle";
			this.LblTitle.Size = new System.Drawing.Size(32, 23);
			this.LblTitle.TabIndex = 74;
			this.LblTitle.Text = "Title:";
			// 
			// TxtReservePrice
			// 
			this.TxtReservePrice.Location = new System.Drawing.Point(512, 73);
			this.TxtReservePrice.Name = "TxtReservePrice";
			this.TxtReservePrice.Size = new System.Drawing.Size(72, 20);
			this.TxtReservePrice.TabIndex = 77;
			this.TxtReservePrice.Text = "1000";
			// 
			// LblReservePrice
			// 
			this.LblReservePrice.Location = new System.Drawing.Point(392, 72);
			this.LblReservePrice.Name = "LblReservePrice";
			this.LblReservePrice.Size = new System.Drawing.Size(96, 23);
			this.LblReservePrice.TabIndex = 76;
			this.LblReservePrice.Text = "Reserve Price:";
			// 
			// TxtStartPrice
			// 
			this.TxtStartPrice.Location = new System.Drawing.Point(512, 40);
			this.TxtStartPrice.Name = "TxtStartPrice";
			this.TxtStartPrice.Size = new System.Drawing.Size(72, 20);
			this.TxtStartPrice.TabIndex = 79;
			this.TxtStartPrice.Text = "1";
			// 
			// LblStartPrice
			// 
			this.LblStartPrice.Location = new System.Drawing.Point(392, 39);
			this.LblStartPrice.Name = "LblStartPrice";
			this.LblStartPrice.Size = new System.Drawing.Size(96, 23);
			this.LblStartPrice.TabIndex = 78;
			this.LblStartPrice.Text = "Start Price:";
			// 
			// TxtPrimaryCategory
			// 
			this.TxtPrimaryCategory.Location = new System.Drawing.Point(168, 72);
			this.TxtPrimaryCategory.Name = "TxtPrimaryCategory";
			this.TxtPrimaryCategory.Size = new System.Drawing.Size(72, 20);
			this.TxtPrimaryCategory.TabIndex = 81;
			this.TxtPrimaryCategory.Text = "80208";
			// 
			// LblPrimaryCategory
			// 
			this.LblPrimaryCategory.Location = new System.Drawing.Point(40, 72);
			this.LblPrimaryCategory.Name = "LblPrimaryCategory";
			this.LblPrimaryCategory.Size = new System.Drawing.Size(112, 23);
			this.LblPrimaryCategory.TabIndex = 80;
			this.LblPrimaryCategory.Text = "Primary Category:";
			// 
			// TxtSecondaryCategory
			// 
			this.TxtSecondaryCategory.Location = new System.Drawing.Point(168, 104);
			this.TxtSecondaryCategory.Name = "TxtSecondaryCategory";
			this.TxtSecondaryCategory.Size = new System.Drawing.Size(72, 20);
			this.TxtSecondaryCategory.TabIndex = 85;
			this.TxtSecondaryCategory.Text = "";
			// 
			// LblSecondaryCategory
			// 
			this.LblSecondaryCategory.Location = new System.Drawing.Point(40, 104);
			this.LblSecondaryCategory.Name = "LblSecondaryCategory";
			this.LblSecondaryCategory.Size = new System.Drawing.Size(112, 23);
			this.LblSecondaryCategory.TabIndex = 84;
			this.LblSecondaryCategory.Text = "Secondary Category:";
			// 
			// TxtBuyItNowPrice
			// 
			this.TxtBuyItNowPrice.Location = new System.Drawing.Point(512, 104);
			this.TxtBuyItNowPrice.Name = "TxtBuyItNowPrice";
			this.TxtBuyItNowPrice.Size = new System.Drawing.Size(72, 20);
			this.TxtBuyItNowPrice.TabIndex = 83;
			this.TxtBuyItNowPrice.Text = "3000";
			// 
			// LblBuyItNowPrice
			// 
			this.LblBuyItNowPrice.Location = new System.Drawing.Point(392, 103);
			this.LblBuyItNowPrice.Name = "LblBuyItNowPrice";
			this.LblBuyItNowPrice.Size = new System.Drawing.Size(96, 23);
			this.LblBuyItNowPrice.TabIndex = 82;
			this.LblBuyItNowPrice.Text = "Buy It Now Price:";
			// 
			// BtnGetCategories
			// 
			this.BtnGetCategories.Location = new System.Drawing.Point(256, 72);
			this.BtnGetCategories.Name = "BtnGetCategories";
			this.BtnGetCategories.Size = new System.Drawing.Size(96, 23);
			this.BtnGetCategories.TabIndex = 86;
			this.BtnGetCategories.Text = "GetCategories ...";
			this.BtnGetCategories.Click += new System.EventHandler(this.BtnGetCategories_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(136, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 23);
			this.label1.TabIndex = 87;
			this.label1.Text = "Use GetItem to get some item data:";
			// 
			// BtnGetItem
			// 
			this.BtnGetItem.Location = new System.Drawing.Point(368, 8);
			this.BtnGetItem.Name = "BtnGetItem";
			this.BtnGetItem.Size = new System.Drawing.Size(96, 23);
			this.BtnGetItem.TabIndex = 88;
			this.BtnGetItem.Text = "GetItem";
			// 
			// FrmGetItemRecommendations
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 553);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.BtnGetItem,
																		  this.label1,
																		  this.BtnGetCategories,
																		  this.TxtSecondaryCategory,
																		  this.LblSecondaryCategory,
																		  this.TxtBuyItNowPrice,
																		  this.LblBuyItNowPrice,
																		  this.TxtPrimaryCategory,
																		  this.LblPrimaryCategory,
																		  this.TxtStartPrice,
																		  this.LblStartPrice,
																		  this.TxtReservePrice,
																		  this.LblReservePrice,
																		  this.TxtTitle,
																		  this.LblTitle,
																		  this.TabSettings});
			this.Name = "FrmGetItemRecommendations";
			this.Text = "FrmGetItemRecommendations";
			this.Load += new System.EventHandler(this.FrmGetItemRecommendations_Load);
			this.TabSettings.ResumeLayout(false);
			this.TabListingAnalyzer.ResumeLayout(false);
			this.GrpListingTips.ResumeLayout(false);
			this.TabProductPricing.ResumeLayout(false);
			this.GrpResult.ResumeLayout(false);
			this.TabSuggestedAttributes.ResumeLayout(false);
			this.GrpSuggestedProducts.ResumeLayout(false);
			this.GrpSuggestedAttr.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FrmGetItemRecommendations_Load(object sender, System.EventArgs e)
		{
			string[] enums = Enum.GetNames(typeof(ListingFlowCodeType));
			foreach (string item in enums)
			{
				if (item != "CustomCode")
					CboListingFlow.Items.Add(item);
			}

			CboListingFlow.SelectedIndex = 0;
			
		}

		private ItemType FillItem(RecommendationEngineCodeType engine)
		{
			ItemType item = new ItemType();
			if (TxtTitle.Text.Length > 0)
				item.Title = TxtTitle.Text;

			CurrencyCodeType currencyID = CurrencyUtility.GetDefaultCurrencyCodeType(Context.Site);

			item.Currency = currencyID;
			if (TxtStartPrice.Text.Length > 0)
			{
				item.StartPrice = new AmountType();
				item.StartPrice.Value = Convert.ToDouble(TxtStartPrice.Text);
				item.StartPrice.currencyID = currencyID;
			}

			if (TxtReservePrice.Text.Length > 0)
			{
				item.ReservePrice = new AmountType();
				item.ReservePrice.Value = Convert.ToDouble(TxtReservePrice.Text);
				item.ReservePrice.currencyID = currencyID;
			};

			if (TxtBuyItNowPrice.Text.Length > 0)
			{
				item.BuyItNowPrice = new AmountType();
				item.BuyItNowPrice.Value = Convert.ToDouble(TxtBuyItNowPrice.Text);
				item.BuyItNowPrice.currencyID = currencyID;
			}

			if (TxtPrimaryCategory.Text.Length > 0)
			{
				item.PrimaryCategory = new CategoryType();
				item.PrimaryCategory.CategoryID = TxtPrimaryCategory.Text;
			}

			if (TxtSecondaryCategory.Text.Length > 0)
			{
				item.SecondaryCategory = new CategoryType();
				item.SecondaryCategory.CategoryID= TxtSecondaryCategory.Text;
			}

			if (TxtItemID.Visible) 
			{
				item.ItemID = TxtItemID.Text;
			}
			if( engine == RecommendationEngineCodeType.ProductPricing )
			{
				string s = TxtExternalProductID.Text;
				if (s.Length > 0) 
				{
					ProductListingDetailsType pld = new ProductListingDetailsType();
					pld.ProductID = s;
					item.ProductListingDetails = pld;
				}
			}

			return item;
		}

		private void BtnListingAnalyzer_Click(object sender, System.EventArgs e)
		{
			try
			{
				GetItemRecommendationsCall apicall = new GetItemRecommendationsCall(Context);
				GetRecommendationsRequestContainerType req = new GetRecommendationsRequestContainerType();
				GetRecommendationsRequestContainerTypeCollection reqc = new GetRecommendationsRequestContainerTypeCollection();
				reqc.Add(req);
				req.RecommendationEngine = new RecommendationEngineCodeTypeCollection();
				req.RecommendationEngine.Add(RecommendationEngineCodeType.ListingAnalyzer);
				req.ListingFlow = 
					(ListingFlowCodeType) Enum.Parse(typeof(ListingFlowCodeType), CboListingFlow.SelectedItem.ToString())   ;
				
				ItemType item = FillItem(RecommendationEngineCodeType.ListingAnalyzer);
				req.Item = item;
				if(req.ListingFlow == ListingFlowCodeType.AddItem) 
				{
					req.Item.ItemID = null;
				} 
				else if(fetchedItem != null) 
				{
					req.Item.ItemID = fetchedItem.ItemID;
				}
				// Make API call
				apicall.GetItemRecommendations(reqc);
				GetRecommendationsResponseContainerTypeCollection resps = apicall.ApiResponse.GetRecommendationsResponseContainer;
				ListingAnalyzerRecommendationsType listingAnalyzerRecommendations = resps[0].ListingAnalyzerRecommendations;
				LstTips.Items.Clear();
				if(listingAnalyzerRecommendations != null) 
				{
					ListingTipTypeCollection tips = listingAnalyzerRecommendations.ListingTipArray;
					if(tips != null) 
					{
						foreach (ListingTipType tip in tips)
						{
							string[] listparams = new string[8];
							listparams[0] = tip.ListingTipID;
							listparams[1] = tip.Field.ListingTipFieldID;
							listparams[2] = tip.Priority.ToString();
							listparams[3] = tip.Message.LongMessage;
							listparams[4] = StripCDATA(tip.Field.FieldTip);
							listparams[5] = StripCDATA(tip.Field.CurrentFieldText);
							listparams[6] = StripCDATA(tip.Message.HelpURLPath);

							ListViewItem vi = new ListViewItem(listparams);
							LstTips.Items.Add(vi);
						}				
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnProductPricing_Click(object sender, System.EventArgs e)
		{
			try
			{
				if( TxtExternalProductID.Text.Length == 0 ) 
				{
					throw new Exception("Please specify the catalog product ID.");
				}
				TxtAverageSoldPrice.Text = "";
				TxtAverageStartPrice.Text = "";
				TxtCatalogTitle.Text = "";

				ItemType item = FillItem(RecommendationEngineCodeType.ProductPricing);
				GetItemRecommendationsCall apicall = new GetItemRecommendationsCall(Context);
				GetRecommendationsRequestContainerType req = new GetRecommendationsRequestContainerType();

				req.Item = item;
				req.Item.LookupAttributeArray = null;
				req.ListingFlow = ListingFlowCodeType.AddItem;
				req.RecommendationEngine = new RecommendationEngineCodeTypeCollection();
				req.RecommendationEngine.Add(RecommendationEngineCodeType.ProductPricing);
				GetRecommendationsRequestContainerTypeCollection reqc = new GetRecommendationsRequestContainerTypeCollection();
				reqc.Add(req);

				// Make API call
				apicall.GetItemRecommendations(reqc);
				GetRecommendationsResponseContainerTypeCollection resps = apicall.ApiResponse.GetRecommendationsResponseContainer;
				PricingRecommendationsType pricingRecoms = resps[0].PricingRecommendations;

				if (pricingRecoms != null && pricingRecoms.ProductInfo != null)
				{					
					TxtCatalogTitle.Text = pricingRecoms.ProductInfo.Title;
					AmountType avgSoldPrice = pricingRecoms.ProductInfo.AverageSoldPrice;
					TxtAverageSoldPrice.Text = (avgSoldPrice != null)?avgSoldPrice.Value.ToString():"";
					AmountType avgStartPrice = pricingRecoms.ProductInfo.AverageStartPrice;
					TxtAverageStartPrice.Text = (avgStartPrice != null)?avgStartPrice.Value.ToString():"";
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void BtnSuggestedAttr_Click(object sender, System.EventArgs e)
		{
			try
			{
				ItemType item = FillItem(RecommendationEngineCodeType.SuggestedAttributes);
				GetItemRecommendationsCall apicall = new GetItemRecommendationsCall(Context);
				GetRecommendationsRequestContainerType req = new GetRecommendationsRequestContainerType();

				req.Item = item;
				req.Item.LookupAttributeArray = null;
				req.ListingFlow = ListingFlowCodeType.AddItem;
				req.RecommendationEngine = new RecommendationEngineCodeTypeCollection();
				req.RecommendationEngine.Add(RecommendationEngineCodeType.SuggestedAttributes);
				if(TxtQuery.Text.Length > 0) 
				{
					req.Query = TxtQuery.Text;
				}

				GetRecommendationsRequestContainerTypeCollection reqc = new GetRecommendationsRequestContainerTypeCollection();
				reqc.Add(req);

				// Make API call
				apicall.GetItemRecommendations(reqc);
				GetRecommendationsResponseContainerTypeCollection resps = apicall.ApiResponse.GetRecommendationsResponseContainer;
				AttributeRecommendationsType attributes = resps[0].AttributeRecommendations;
				AttributeSetTypeCollection attrSets = null;
				if(attributes != null) 
				{
					attrSets = attributes.AttributeSetArray;
				}
				if (attrSets != null)
				{
					foreach (AttributeSetType attribute in attrSets)
					{
						if(attribute != null) 
						{
							string[] listparams = new string[3];
							listparams[0] = attribute.attributeSetID.ToString();
							listparams[1] = attribute.attributeSetVersion;
							listparams[2] =  attribute.Attribute.Count.ToString();

							ListViewItem vi = new ListViewItem(listparams);
							LstAttr.Items.Add(vi);
						}
					}	
				}
			
				ProductInfoTypeCollection products = resps[0].ProductRecommendations;
				if (products != null)
				{
					foreach (ProductInfoType product in products)
					{
						if(product != null) 
						{
							string[] listparams = new string[4];
							listparams[0] = product.Title;
							listparams[1] = product.productInfoID;
							if (product.AverageStartPrice != null) 
							{
								listparams[2] =  product.AverageStartPrice.Value.ToString();
							}
							if (product.AverageSoldPrice != null) 
							{
								listparams[3] =  product.AverageSoldPrice.Value.ToString();;
							}
							ListViewItem vi = new ListViewItem(listparams);
							lstProduct.Items.Add(vi);
						}
					}
				}
				

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnGetCategories_Click(object sender, System.EventArgs e)
		{
			FrmGetCategories form = new FrmGetCategories();
			form.Context = Context;
			form.ShowDialog();
		}

		private void BtnGetItem_Click(object sender, System.EventArgs e)
		{
			FrmGetItem form = new FrmGetItem();
			form.Context = Context;
			form.ShowDialog();
			fetchedItem = form.getItem();
			if(fetchedItem != null) 
			{
				TxtTitle.Text = fetchedItem.Title;
				TxtPrimaryCategory.Text = fetchedItem.PrimaryCategory.CategoryID;
				CategoryType secondaryCategory = fetchedItem.SecondaryCategory; 
				TxtSecondaryCategory.Text = (secondaryCategory != null)?secondaryCategory.CategoryID:"";
				TxtStartPrice.Text = fetchedItem.SellingStatus.CurrentPrice.Value.ToString();
				AmountType amt = fetchedItem.ReservePrice;
				TxtReservePrice.Text = (amt == null)?"":amt.Value.ToString();
				amt = fetchedItem.BuyItNowPrice;
				TxtBuyItNowPrice.Text = (amt == null)?"":amt.Value.ToString();
				TxtItemID.Text = fetchedItem.ItemID;
			}

		}

		private void CboListingFlow_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (CboListingFlow.Text.Equals("AddItem"))
			{
				LblItemID.Visible = false;
				TxtItemID.Visible = false;
			} 
			else 
			{
				LblItemID.Visible = true;
				TxtItemID.Visible = true;
			}
		}

		private void LstTips_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
		private string StripCDATA(string cdataString) 
		{
			string CDATA_START = "<![CDATA[";
			string CDATA_END = "]]>";
	  
			if(cdataString == null || cdataString.Length == 0) 
			{
				return "";
			}
			int index1 = cdataString.IndexOf(CDATA_START);
			if(index1 == -1) 
			{
				return cdataString;
			}
			int index2 = cdataString.IndexOf(CDATA_END);
			if(index2 == -1) 
			{
				return cdataString.Substring(CDATA_START.Length);
			} 
			else 
			{
				return cdataString.Substring(CDATA_START.Length, (cdataString.Length - CDATA_START.Length - CDATA_END.Length));
			}
		}

		private void TxtTitle_TextChanged(object sender, System.EventArgs e)
		{
		
		}

	}
}
