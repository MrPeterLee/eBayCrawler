Imports System
Imports System.Globalization
Imports eBay.Service.Core.Soap
Imports eBay.Service.Core.Sdk
Imports eBay.Service.Call
Imports eBay.Service.Util

Public Class FormGetItemRecommendations
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents BtnGetCategories As System.Windows.Forms.Button
    Friend WithEvents TxtSecondaryCategory As System.Windows.Forms.TextBox
    Friend WithEvents LblSecondaryCategory As System.Windows.Forms.Label
    Friend WithEvents TxtBuyItNowPrice As System.Windows.Forms.TextBox
    Friend WithEvents LblBuyItNowPrice As System.Windows.Forms.Label
    Friend WithEvents TxtPrimaryCategory As System.Windows.Forms.TextBox
    Friend WithEvents LblPrimaryCategory As System.Windows.Forms.Label
    Friend WithEvents TxtStartPrice As System.Windows.Forms.TextBox
    Friend WithEvents LblStartPrice As System.Windows.Forms.Label
    Friend WithEvents TxtReservePrice As System.Windows.Forms.TextBox
    Friend WithEvents LblReservePrice As System.Windows.Forms.Label
    Friend WithEvents TxtTitle As System.Windows.Forms.TextBox
    Friend WithEvents LblTitle As System.Windows.Forms.Label
    Friend WithEvents TabSettings As System.Windows.Forms.TabControl
    Friend WithEvents TabListingAnalyzer As System.Windows.Forms.TabPage
    Friend WithEvents GrpListingTips As System.Windows.Forms.GroupBox
    Friend WithEvents LstTips As System.Windows.Forms.ListView
    Friend WithEvents BtnListingAnalyzer As System.Windows.Forms.Button
    Friend WithEvents CboListingFlow As System.Windows.Forms.ComboBox
    Friend WithEvents LblListingFlow As System.Windows.Forms.Label
    Friend WithEvents TabProductPricing As System.Windows.Forms.TabPage
    Friend WithEvents GrpResult As System.Windows.Forms.GroupBox
    Friend WithEvents TxtAverageStartPrice As System.Windows.Forms.TextBox
    Friend WithEvents TxtAverageSoldPrice As System.Windows.Forms.TextBox
    Friend WithEvents LblAverageStartPrice As System.Windows.Forms.Label
    Friend WithEvents LblAverageSoldPrice As System.Windows.Forms.Label
    Friend WithEvents TxtCatalogTitle As System.Windows.Forms.TextBox
    Friend WithEvents LblCatalogTitle As System.Windows.Forms.Label
    Friend WithEvents TxtExternalProductID As System.Windows.Forms.TextBox
    Friend WithEvents BtnProductPricing As System.Windows.Forms.Button
    Friend WithEvents LblExternalProductID As System.Windows.Forms.Label
    Friend WithEvents TabSuggestedAttributes As System.Windows.Forms.TabPage
    Friend WithEvents GrpSuggestedProducts As System.Windows.Forms.GroupBox
    Friend WithEvents lstProduct As System.Windows.Forms.ListView
    Friend WithEvents GrpSuggestedAttr As System.Windows.Forms.GroupBox
    Friend WithEvents LstAttr As System.Windows.Forms.ListView
    Friend WithEvents TxtQuery As System.Windows.Forms.TextBox
    Friend WithEvents LblQuery As System.Windows.Forms.Label
    Friend WithEvents BtnSuggestedAttr As System.Windows.Forms.Button
    Friend WithEvents ClmID As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmPriority As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmMessage As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmFieldID As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmFieldTip As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmFieldText As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmTitle As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmInfoID As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmAvgStartPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmAvgSoldPrice As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmCSID As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmVersion As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClmNoOfAttr As System.Windows.Forms.ColumnHeader
    Friend WithEvents TxtItemID As System.Windows.Forms.TextBox
    Friend WithEvents LblItemID As System.Windows.Forms.Label
    Friend WithEvents ClmHelpURL As System.Windows.Forms.ColumnHeader
    Friend WithEvents BtnGetItem As System.Windows.Forms.Button
    Friend WithEvents lblGetItem As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.BtnGetCategories = New System.Windows.Forms.Button()
        Me.TxtSecondaryCategory = New System.Windows.Forms.TextBox()
        Me.LblSecondaryCategory = New System.Windows.Forms.Label()
        Me.TxtBuyItNowPrice = New System.Windows.Forms.TextBox()
        Me.LblBuyItNowPrice = New System.Windows.Forms.Label()
        Me.TxtPrimaryCategory = New System.Windows.Forms.TextBox()
        Me.LblPrimaryCategory = New System.Windows.Forms.Label()
        Me.TxtStartPrice = New System.Windows.Forms.TextBox()
        Me.LblStartPrice = New System.Windows.Forms.Label()
        Me.TxtReservePrice = New System.Windows.Forms.TextBox()
        Me.LblReservePrice = New System.Windows.Forms.Label()
        Me.TxtTitle = New System.Windows.Forms.TextBox()
        Me.LblTitle = New System.Windows.Forms.Label()
        Me.TabSettings = New System.Windows.Forms.TabControl()
        Me.TabListingAnalyzer = New System.Windows.Forms.TabPage()
        Me.TxtItemID = New System.Windows.Forms.TextBox()
        Me.LblItemID = New System.Windows.Forms.Label()
        Me.GrpListingTips = New System.Windows.Forms.GroupBox()
        Me.LstTips = New System.Windows.Forms.ListView()
        Me.ClmID = New System.Windows.Forms.ColumnHeader()
        Me.ClmFieldID = New System.Windows.Forms.ColumnHeader()
        Me.ClmPriority = New System.Windows.Forms.ColumnHeader()
        Me.ClmMessage = New System.Windows.Forms.ColumnHeader()
        Me.ClmFieldTip = New System.Windows.Forms.ColumnHeader()
        Me.ClmFieldText = New System.Windows.Forms.ColumnHeader()
        Me.ClmHelpURL = New System.Windows.Forms.ColumnHeader()
        Me.BtnListingAnalyzer = New System.Windows.Forms.Button()
        Me.CboListingFlow = New System.Windows.Forms.ComboBox()
        Me.LblListingFlow = New System.Windows.Forms.Label()
        Me.TabProductPricing = New System.Windows.Forms.TabPage()
        Me.GrpResult = New System.Windows.Forms.GroupBox()
        Me.TxtAverageStartPrice = New System.Windows.Forms.TextBox()
        Me.TxtAverageSoldPrice = New System.Windows.Forms.TextBox()
        Me.LblAverageStartPrice = New System.Windows.Forms.Label()
        Me.LblAverageSoldPrice = New System.Windows.Forms.Label()
        Me.TxtCatalogTitle = New System.Windows.Forms.TextBox()
        Me.LblCatalogTitle = New System.Windows.Forms.Label()
        Me.TxtExternalProductID = New System.Windows.Forms.TextBox()
        Me.BtnProductPricing = New System.Windows.Forms.Button()
        Me.LblExternalProductID = New System.Windows.Forms.Label()
        Me.TabSuggestedAttributes = New System.Windows.Forms.TabPage()
        Me.GrpSuggestedProducts = New System.Windows.Forms.GroupBox()
        Me.lstProduct = New System.Windows.Forms.ListView()
        Me.ClmTitle = New System.Windows.Forms.ColumnHeader()
        Me.ClmInfoID = New System.Windows.Forms.ColumnHeader()
        Me.ClmAvgStartPrice = New System.Windows.Forms.ColumnHeader()
        Me.ClmAvgSoldPrice = New System.Windows.Forms.ColumnHeader()
        Me.GrpSuggestedAttr = New System.Windows.Forms.GroupBox()
        Me.LstAttr = New System.Windows.Forms.ListView()
        Me.ClmCSID = New System.Windows.Forms.ColumnHeader()
        Me.ClmVersion = New System.Windows.Forms.ColumnHeader()
        Me.ClmNoOfAttr = New System.Windows.Forms.ColumnHeader()
        Me.TxtQuery = New System.Windows.Forms.TextBox()
        Me.LblQuery = New System.Windows.Forms.Label()
        Me.BtnSuggestedAttr = New System.Windows.Forms.Button()
        Me.lblGetItem = New System.Windows.Forms.Label()
        Me.BtnGetItem = New System.Windows.Forms.Button()
        Me.TabSettings.SuspendLayout()
        Me.TabListingAnalyzer.SuspendLayout()
        Me.GrpListingTips.SuspendLayout()
        Me.TabProductPricing.SuspendLayout()
        Me.GrpResult.SuspendLayout()
        Me.TabSuggestedAttributes.SuspendLayout()
        Me.GrpSuggestedProducts.SuspendLayout()
        Me.GrpSuggestedAttr.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnGetCategories
        '
        Me.BtnGetCategories.Location = New System.Drawing.Point(248, 88)
        Me.BtnGetCategories.Name = "BtnGetCategories"
        Me.BtnGetCategories.Size = New System.Drawing.Size(96, 23)
        Me.BtnGetCategories.TabIndex = 100
        Me.BtnGetCategories.Text = "GetCategories ..."
        '
        'TxtSecondaryCategory
        '
        Me.TxtSecondaryCategory.Location = New System.Drawing.Point(160, 120)
        Me.TxtSecondaryCategory.Name = "TxtSecondaryCategory"
        Me.TxtSecondaryCategory.Size = New System.Drawing.Size(72, 20)
        Me.TxtSecondaryCategory.TabIndex = 99
        Me.TxtSecondaryCategory.Text = ""
        '
        'LblSecondaryCategory
        '
        Me.LblSecondaryCategory.Location = New System.Drawing.Point(32, 120)
        Me.LblSecondaryCategory.Name = "LblSecondaryCategory"
        Me.LblSecondaryCategory.Size = New System.Drawing.Size(112, 23)
        Me.LblSecondaryCategory.TabIndex = 98
        Me.LblSecondaryCategory.Text = "Secondary Category:"
        '
        'TxtBuyItNowPrice
        '
        Me.TxtBuyItNowPrice.Location = New System.Drawing.Point(504, 120)
        Me.TxtBuyItNowPrice.Name = "TxtBuyItNowPrice"
        Me.TxtBuyItNowPrice.Size = New System.Drawing.Size(72, 20)
        Me.TxtBuyItNowPrice.TabIndex = 97
        Me.TxtBuyItNowPrice.Text = "3000"
        '
        'LblBuyItNowPrice
        '
        Me.LblBuyItNowPrice.Location = New System.Drawing.Point(384, 120)
        Me.LblBuyItNowPrice.Name = "LblBuyItNowPrice"
        Me.LblBuyItNowPrice.Size = New System.Drawing.Size(96, 23)
        Me.LblBuyItNowPrice.TabIndex = 96
        Me.LblBuyItNowPrice.Text = "Buy It Now Price:"
        '
        'TxtPrimaryCategory
        '
        Me.TxtPrimaryCategory.Location = New System.Drawing.Point(160, 88)
        Me.TxtPrimaryCategory.Name = "TxtPrimaryCategory"
        Me.TxtPrimaryCategory.Size = New System.Drawing.Size(72, 20)
        Me.TxtPrimaryCategory.TabIndex = 95
        Me.TxtPrimaryCategory.Text = "80208"
        '
        'LblPrimaryCategory
        '
        Me.LblPrimaryCategory.Location = New System.Drawing.Point(32, 88)
        Me.LblPrimaryCategory.Name = "LblPrimaryCategory"
        Me.LblPrimaryCategory.Size = New System.Drawing.Size(112, 23)
        Me.LblPrimaryCategory.TabIndex = 94
        Me.LblPrimaryCategory.Text = "Primary Category:"
        '
        'TxtStartPrice
        '
        Me.TxtStartPrice.Location = New System.Drawing.Point(504, 56)
        Me.TxtStartPrice.Name = "TxtStartPrice"
        Me.TxtStartPrice.Size = New System.Drawing.Size(72, 20)
        Me.TxtStartPrice.TabIndex = 93
        Me.TxtStartPrice.Text = "1"
        '
        'LblStartPrice
        '
        Me.LblStartPrice.Location = New System.Drawing.Point(384, 56)
        Me.LblStartPrice.Name = "LblStartPrice"
        Me.LblStartPrice.Size = New System.Drawing.Size(96, 23)
        Me.LblStartPrice.TabIndex = 92
        Me.LblStartPrice.Text = "Start Price:"
        '
        'TxtReservePrice
        '
        Me.TxtReservePrice.Location = New System.Drawing.Point(504, 96)
        Me.TxtReservePrice.Name = "TxtReservePrice"
        Me.TxtReservePrice.Size = New System.Drawing.Size(72, 20)
        Me.TxtReservePrice.TabIndex = 91
        Me.TxtReservePrice.Text = "1000"
        '
        'LblReservePrice
        '
        Me.LblReservePrice.Location = New System.Drawing.Point(384, 88)
        Me.LblReservePrice.Name = "LblReservePrice"
        Me.LblReservePrice.Size = New System.Drawing.Size(96, 23)
        Me.LblReservePrice.TabIndex = 90
        Me.LblReservePrice.Text = "Reserve Price:"
        '
        'TxtTitle
        '
        Me.TxtTitle.Location = New System.Drawing.Point(72, 56)
        Me.TxtTitle.Name = "TxtTitle"
        Me.TxtTitle.Size = New System.Drawing.Size(208, 20)
        Me.TxtTitle.TabIndex = 89
        Me.TxtTitle.Text = "DELL new"
        '
        'LblTitle
        '
        Me.LblTitle.Location = New System.Drawing.Point(32, 32)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(32, 23)
        Me.LblTitle.TabIndex = 88
        Me.LblTitle.Text = "Title:"
        '
        'TabSettings
        '
        Me.TabSettings.Controls.AddRange(New System.Windows.Forms.Control() {Me.TabListingAnalyzer, Me.TabProductPricing, Me.TabSuggestedAttributes})
        Me.TabSettings.Location = New System.Drawing.Point(32, 160)
        Me.TabSettings.Name = "TabSettings"
        Me.TabSettings.SelectedIndex = 0
        Me.TabSettings.Size = New System.Drawing.Size(560, 372)
        Me.TabSettings.TabIndex = 87
        '
        'TabListingAnalyzer
        '
        Me.TabListingAnalyzer.Controls.AddRange(New System.Windows.Forms.Control() {Me.TxtItemID, Me.LblItemID, Me.GrpListingTips, Me.BtnListingAnalyzer, Me.CboListingFlow, Me.LblListingFlow})
        Me.TabListingAnalyzer.Location = New System.Drawing.Point(4, 22)
        Me.TabListingAnalyzer.Name = "TabListingAnalyzer"
        Me.TabListingAnalyzer.Size = New System.Drawing.Size(552, 346)
        Me.TabListingAnalyzer.TabIndex = 0
        Me.TabListingAnalyzer.Text = "Listing Analyzer"
        '
        'TxtItemID
        '
        Me.TxtItemID.Location = New System.Drawing.Point(400, 16)
        Me.TxtItemID.Name = "TxtItemID"
        Me.TxtItemID.Size = New System.Drawing.Size(144, 20)
        Me.TxtItemID.TabIndex = 87
        Me.TxtItemID.Text = ""
        Me.TxtItemID.Visible = False
        '
        'LblItemID
        '
        Me.LblItemID.Location = New System.Drawing.Point(336, 16)
        Me.LblItemID.Name = "LblItemID"
        Me.LblItemID.Size = New System.Drawing.Size(48, 23)
        Me.LblItemID.TabIndex = 86
        Me.LblItemID.Text = "Item ID:"
        Me.LblItemID.Visible = False
        '
        'GrpListingTips
        '
        Me.GrpListingTips.Controls.AddRange(New System.Windows.Forms.Control() {Me.LstTips})
        Me.GrpListingTips.Location = New System.Drawing.Point(16, 96)
        Me.GrpListingTips.Name = "GrpListingTips"
        Me.GrpListingTips.Size = New System.Drawing.Size(520, 216)
        Me.GrpListingTips.TabIndex = 65
        Me.GrpListingTips.TabStop = False
        Me.GrpListingTips.Text = "Listing Tips"
        '
        'LstTips
        '
        Me.LstTips.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ClmID, Me.ClmFieldID, Me.ClmPriority, Me.ClmMessage, Me.ClmFieldTip, Me.ClmFieldText, Me.ClmHelpURL})
        Me.LstTips.GridLines = True
        Me.LstTips.Location = New System.Drawing.Point(16, 24)
        Me.LstTips.Name = "LstTips"
        Me.LstTips.Size = New System.Drawing.Size(496, 176)
        Me.LstTips.TabIndex = 13
        Me.LstTips.View = System.Windows.Forms.View.Details
        '
        'ClmID
        '
        Me.ClmID.Text = "ID"
        Me.ClmID.Width = 40
        '
        'ClmFieldID
        '
        Me.ClmFieldID.Text = "Field ID"
        Me.ClmFieldID.Width = 53
        '
        'ClmPriority
        '
        Me.ClmPriority.Text = "Priority"
        Me.ClmPriority.Width = 50
        '
        'ClmMessage
        '
        Me.ClmMessage.Text = "Message"
        Me.ClmMessage.Width = 100
        '
        'ClmFieldTip
        '
        Me.ClmFieldTip.Text = "Field Tip"
        Me.ClmFieldTip.Width = 86
        '
        'ClmFieldText
        '
        Me.ClmFieldText.Text = "Field Text"
        '
        'ClmHelpURL
        '
        Me.ClmHelpURL.Text = "Help URL"
        Me.ClmHelpURL.Width = 95
        '
        'BtnListingAnalyzer
        '
        Me.BtnListingAnalyzer.Location = New System.Drawing.Point(112, 56)
        Me.BtnListingAnalyzer.Name = "BtnListingAnalyzer"
        Me.BtnListingAnalyzer.Size = New System.Drawing.Size(120, 24)
        Me.BtnListingAnalyzer.TabIndex = 64
        Me.BtnListingAnalyzer.Text = "Run Listing Analyzer"
        '
        'CboListingFlow
        '
        Me.CboListingFlow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboListingFlow.Location = New System.Drawing.Point(104, 16)
        Me.CboListingFlow.Name = "CboListingFlow"
        Me.CboListingFlow.Size = New System.Drawing.Size(216, 21)
        Me.CboListingFlow.TabIndex = 58
        '
        'LblListingFlow
        '
        Me.LblListingFlow.Location = New System.Drawing.Point(16, 16)
        Me.LblListingFlow.Name = "LblListingFlow"
        Me.LblListingFlow.Size = New System.Drawing.Size(72, 24)
        Me.LblListingFlow.TabIndex = 0
        Me.LblListingFlow.Text = "Listing Flow:"
        '
        'TabProductPricing
        '
        Me.TabProductPricing.Controls.AddRange(New System.Windows.Forms.Control() {Me.GrpResult, Me.TxtExternalProductID, Me.BtnProductPricing, Me.LblExternalProductID})
        Me.TabProductPricing.Location = New System.Drawing.Point(4, 22)
        Me.TabProductPricing.Name = "TabProductPricing"
        Me.TabProductPricing.Size = New System.Drawing.Size(552, 346)
        Me.TabProductPricing.TabIndex = 3
        Me.TabProductPricing.Text = "Product Pricing"
        Me.TabProductPricing.Visible = False
        '
        'GrpResult
        '
        Me.GrpResult.Controls.AddRange(New System.Windows.Forms.Control() {Me.TxtAverageStartPrice, Me.TxtAverageSoldPrice, Me.LblAverageStartPrice, Me.LblAverageSoldPrice, Me.TxtCatalogTitle, Me.LblCatalogTitle})
        Me.GrpResult.Location = New System.Drawing.Point(16, 112)
        Me.GrpResult.Name = "GrpResult"
        Me.GrpResult.Size = New System.Drawing.Size(520, 224)
        Me.GrpResult.TabIndex = 67
        Me.GrpResult.TabStop = False
        Me.GrpResult.Text = "Result"
        '
        'TxtAverageStartPrice
        '
        Me.TxtAverageStartPrice.Location = New System.Drawing.Point(192, 68)
        Me.TxtAverageStartPrice.Name = "TxtAverageStartPrice"
        Me.TxtAverageStartPrice.Size = New System.Drawing.Size(256, 20)
        Me.TxtAverageStartPrice.TabIndex = 23
        Me.TxtAverageStartPrice.Text = ""
        '
        'TxtAverageSoldPrice
        '
        Me.TxtAverageSoldPrice.Location = New System.Drawing.Point(192, 104)
        Me.TxtAverageSoldPrice.Name = "TxtAverageSoldPrice"
        Me.TxtAverageSoldPrice.Size = New System.Drawing.Size(256, 20)
        Me.TxtAverageSoldPrice.TabIndex = 65
        Me.TxtAverageSoldPrice.Text = ""
        '
        'LblAverageStartPrice
        '
        Me.LblAverageStartPrice.Location = New System.Drawing.Point(80, 68)
        Me.LblAverageStartPrice.Name = "LblAverageStartPrice"
        Me.LblAverageStartPrice.Size = New System.Drawing.Size(200, 16)
        Me.LblAverageStartPrice.TabIndex = 24
        Me.LblAverageStartPrice.Text = "Average Start Price:"
        '
        'LblAverageSoldPrice
        '
        Me.LblAverageSoldPrice.Location = New System.Drawing.Point(80, 104)
        Me.LblAverageSoldPrice.Name = "LblAverageSoldPrice"
        Me.LblAverageSoldPrice.Size = New System.Drawing.Size(200, 16)
        Me.LblAverageSoldPrice.TabIndex = 66
        Me.LblAverageSoldPrice.Text = "Average Sold Price:"
        '
        'TxtCatalogTitle
        '
        Me.TxtCatalogTitle.Location = New System.Drawing.Point(192, 32)
        Me.TxtCatalogTitle.Name = "TxtCatalogTitle"
        Me.TxtCatalogTitle.Size = New System.Drawing.Size(256, 20)
        Me.TxtCatalogTitle.TabIndex = 2
        Me.TxtCatalogTitle.Text = ""
        '
        'LblCatalogTitle
        '
        Me.LblCatalogTitle.Location = New System.Drawing.Point(80, 32)
        Me.LblCatalogTitle.Name = "LblCatalogTitle"
        Me.LblCatalogTitle.Size = New System.Drawing.Size(192, 16)
        Me.LblCatalogTitle.TabIndex = 22
        Me.LblCatalogTitle.Text = "Catalog Title:"
        '
        'TxtExternalProductID
        '
        Me.TxtExternalProductID.Location = New System.Drawing.Point(216, 16)
        Me.TxtExternalProductID.Name = "TxtExternalProductID"
        Me.TxtExternalProductID.Size = New System.Drawing.Size(208, 20)
        Me.TxtExternalProductID.TabIndex = 64
        Me.TxtExternalProductID.Text = "79328:2:1431:561576419:57669919:391bc1eb4871c4a4e930a52fca6eccfd:1:1:1:1348602270" & _
        ""
        '
        'BtnProductPricing
        '
        Me.BtnProductPricing.Location = New System.Drawing.Point(168, 56)
        Me.BtnProductPricing.Name = "BtnProductPricing"
        Me.BtnProductPricing.Size = New System.Drawing.Size(168, 23)
        Me.BtnProductPricing.TabIndex = 63
        Me.BtnProductPricing.Text = "Run Product Pricing Engine"
        '
        'LblExternalProductID
        '
        Me.LblExternalProductID.Location = New System.Drawing.Point(104, 16)
        Me.LblExternalProductID.Name = "LblExternalProductID"
        Me.LblExternalProductID.Size = New System.Drawing.Size(112, 16)
        Me.LblExternalProductID.TabIndex = 20
        Me.LblExternalProductID.Text = "External Product ID:"
        '
        'TabSuggestedAttributes
        '
        Me.TabSuggestedAttributes.Controls.AddRange(New System.Windows.Forms.Control() {Me.GrpSuggestedProducts, Me.GrpSuggestedAttr, Me.TxtQuery, Me.LblQuery, Me.BtnSuggestedAttr})
        Me.TabSuggestedAttributes.Location = New System.Drawing.Point(4, 22)
        Me.TabSuggestedAttributes.Name = "TabSuggestedAttributes"
        Me.TabSuggestedAttributes.Size = New System.Drawing.Size(552, 346)
        Me.TabSuggestedAttributes.TabIndex = 1
        Me.TabSuggestedAttributes.Text = "Suggested Attributes"
        Me.TabSuggestedAttributes.Visible = False
        '
        'GrpSuggestedProducts
        '
        Me.GrpSuggestedProducts.Controls.AddRange(New System.Windows.Forms.Control() {Me.lstProduct})
        Me.GrpSuggestedProducts.Location = New System.Drawing.Point(280, 88)
        Me.GrpSuggestedProducts.Name = "GrpSuggestedProducts"
        Me.GrpSuggestedProducts.Size = New System.Drawing.Size(256, 240)
        Me.GrpSuggestedProducts.TabIndex = 75
        Me.GrpSuggestedProducts.TabStop = False
        Me.GrpSuggestedProducts.Text = "Suggested Products"
        '
        'lstProduct
        '
        Me.lstProduct.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ClmTitle, Me.ClmInfoID, Me.ClmAvgStartPrice, Me.ClmAvgSoldPrice})
        Me.lstProduct.GridLines = True
        Me.lstProduct.Location = New System.Drawing.Point(16, 24)
        Me.lstProduct.Name = "lstProduct"
        Me.lstProduct.Size = New System.Drawing.Size(224, 200)
        Me.lstProduct.TabIndex = 15
        Me.lstProduct.View = System.Windows.Forms.View.Details
        '
        'ClmTitle
        '
        Me.ClmTitle.Text = "Title"
        Me.ClmTitle.Width = 40
        '
        'ClmInfoID
        '
        Me.ClmInfoID.Text = "Info ID"
        Me.ClmInfoID.Width = 50
        '
        'ClmAvgStartPrice
        '
        Me.ClmAvgStartPrice.Text = "Avg Start Price"
        Me.ClmAvgStartPrice.Width = 70
        '
        'ClmAvgSoldPrice
        '
        Me.ClmAvgSoldPrice.Text = "Avg Sold Price"
        Me.ClmAvgSoldPrice.Width = 70
        '
        'GrpSuggestedAttr
        '
        Me.GrpSuggestedAttr.Controls.AddRange(New System.Windows.Forms.Control() {Me.LstAttr})
        Me.GrpSuggestedAttr.Location = New System.Drawing.Point(16, 88)
        Me.GrpSuggestedAttr.Name = "GrpSuggestedAttr"
        Me.GrpSuggestedAttr.Size = New System.Drawing.Size(256, 240)
        Me.GrpSuggestedAttr.TabIndex = 74
        Me.GrpSuggestedAttr.TabStop = False
        Me.GrpSuggestedAttr.Text = "Suggested Attributes   "
        '
        'LstAttr
        '
        Me.LstAttr.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ClmCSID, Me.ClmVersion, Me.ClmNoOfAttr})
        Me.LstAttr.GridLines = True
        Me.LstAttr.Location = New System.Drawing.Point(16, 24)
        Me.LstAttr.Name = "LstAttr"
        Me.LstAttr.Size = New System.Drawing.Size(224, 200)
        Me.LstAttr.TabIndex = 16
        Me.LstAttr.View = System.Windows.Forms.View.Details
        '
        'ClmCSID
        '
        Me.ClmCSID.Text = "CSID"
        '
        'ClmVersion
        '
        Me.ClmVersion.Text = "Version"
        Me.ClmVersion.Width = 50
        '
        'ClmNoOfAttr
        '
        Me.ClmNoOfAttr.Text = "# of Attributes"
        Me.ClmNoOfAttr.Width = 110
        '
        'TxtQuery
        '
        Me.TxtQuery.Location = New System.Drawing.Point(192, 16)
        Me.TxtQuery.Name = "TxtQuery"
        Me.TxtQuery.Size = New System.Drawing.Size(178, 20)
        Me.TxtQuery.TabIndex = 73
        Me.TxtQuery.Text = "DELL Inspiron"
        '
        'LblQuery
        '
        Me.LblQuery.Location = New System.Drawing.Point(136, 16)
        Me.LblQuery.Name = "LblQuery"
        Me.LblQuery.Size = New System.Drawing.Size(48, 23)
        Me.LblQuery.TabIndex = 72
        Me.LblQuery.Text = "Query:"
        '
        'BtnSuggestedAttr
        '
        Me.BtnSuggestedAttr.Location = New System.Drawing.Point(184, 56)
        Me.BtnSuggestedAttr.Name = "BtnSuggestedAttr"
        Me.BtnSuggestedAttr.Size = New System.Drawing.Size(184, 24)
        Me.BtnSuggestedAttr.TabIndex = 70
        Me.BtnSuggestedAttr.Text = "Run Suggested Attributes Engine"
        '
        'lblGetItem
        '
        Me.lblGetItem.Location = New System.Drawing.Point(112, 8)
        Me.lblGetItem.Name = "lblGetItem"
        Me.lblGetItem.Size = New System.Drawing.Size(200, 23)
        Me.lblGetItem.TabIndex = 101
        Me.lblGetItem.Text = "Use GetItem to get some item data:"
        '
        'BtnGetItem
        '
        Me.BtnGetItem.Location = New System.Drawing.Point(328, 8)
        Me.BtnGetItem.Name = "BtnGetItem"
        Me.BtnGetItem.Size = New System.Drawing.Size(96, 23)
        Me.BtnGetItem.TabIndex = 102
        Me.BtnGetItem.Text = "GetItem"
        '
        'FormGetItemRecommendations
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(624, 553)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.BtnGetItem, Me.lblGetItem, Me.BtnGetCategories, Me.TxtSecondaryCategory, Me.LblSecondaryCategory, Me.TxtBuyItNowPrice, Me.LblBuyItNowPrice, Me.TxtPrimaryCategory, Me.LblPrimaryCategory, Me.TxtStartPrice, Me.LblStartPrice, Me.TxtReservePrice, Me.LblReservePrice, Me.TxtTitle, Me.LblTitle, Me.TabSettings})
        Me.Name = "FormGetItemRecommendations"
        Me.Text = "FormGetRecommendations"
        Me.TabSettings.ResumeLayout(False)
        Me.TabListingAnalyzer.ResumeLayout(False)
        Me.GrpListingTips.ResumeLayout(False)
        Me.TabProductPricing.ResumeLayout(False)
        Me.GrpResult.ResumeLayout(False)
        Me.TabSuggestedAttributes.ResumeLayout(False)
        Me.GrpSuggestedProducts.ResumeLayout(False)
        Me.GrpSuggestedAttr.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Context As ApiContext
    Private fetchedItem As ItemType

    Private Sub BtnListingAnalyzer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnListingAnalyzer.Click
        Try

            Dim ApiCall As GetItemRecommendationsCall = New GetItemRecommendationsCall(Context)
            Dim req As GetRecommendationsRequestContainerType = New GetRecommendationsRequestContainerType()
            Dim reqc As GetRecommendationsRequestContainerTypeCollection = New GetRecommendationsRequestContainerTypeCollection()
            reqc.Add(req)
            req.RecommendationEngine = New RecommendationEngineCodeTypeCollection()
            req.RecommendationEngine.Add(RecommendationEngineCodeType.ListingAnalyzer)
			req.ListingFlow = [Enum].Parse(GetType(ListingFlowCodeType), CboListingFlow.SelectedItem.ToString())
            Dim item As ItemType = FillItem(RecommendationEngineCodeType.ListingAnalyzer)
            req.Item = item
            If req.ListingFlow = ListingFlowCodeType.AddItem Then
                req.Item.ItemID = Nothing
            ElseIf Not (fetchedItem Is Nothing) Then
                req.Item.ItemID = fetchedItem.ItemID
            End If
            ' Make API call
            ApiCall.GetItemRecommendations(reqc)
            Dim resps As GetRecommendationsResponseContainerTypeCollection = ApiCall.ApiResponse.GetRecommendationsResponseContainer
            Dim listingAnalyzerRecommendations As ListingAnalyzerRecommendationsType = resps(0).ListingAnalyzerRecommendations
            LstTips.Items.Clear()
            If Not (listingAnalyzerRecommendations Is Nothing) Then
                Dim tips As ListingTipTypeCollection = listingAnalyzerRecommendations.ListingTipArray
                If Not tips Is Nothing Then
                    Dim tip As ListingTipType
                    For Each tip In tips
                        Dim listparams(8) As String
                        listparams(0) = tip.ListingTipID
                        listparams(1) = tip.Field.ListingTipFieldID
                        listparams(2) = tip.Priority.ToString()
                        listparams(3) = tip.Message.LongMessage
                        listparams(4) = StripCDATA(tip.Field.FieldTip)
                        listparams(5) = StripCDATA(tip.Field.CurrentFieldText)
                        listparams(6) = StripCDATA(tip.Message.HelpURLPath)

                        Dim vi As ListViewItem = New ListViewItem(listparams)
                        LstTips.Items.Add(vi)
                    Next tip
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub

    Private Sub BtnProductPricing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnProductPricing.Click
        Try

            TxtAverageSoldPrice.Text = ""
            TxtAverageStartPrice.Text = ""
            TxtCatalogTitle.Text = ""

            Dim ApiCall As GetItemRecommendationsCall = New GetItemRecommendationsCall(Context)
            Dim req As GetRecommendationsRequestContainerType = New GetRecommendationsRequestContainerType()
            req.RecommendationEngine = New RecommendationEngineCodeTypeCollection()
            req.RecommendationEngine.Add(RecommendationEngineCodeType.ProductPricing)
            req.ListingFlow = ListingFlowCodeType.AddItem
            Dim item As ItemType = FillItem(RecommendationEngineCodeType.ProductPricing)
            req.Item = item
            Dim reqc As GetRecommendationsRequestContainerTypeCollection = New GetRecommendationsRequestContainerTypeCollection()
            reqc.Add(req)
            ' Make API call
            ApiCall.GetItemRecommendations(reqc)
            Dim resps As GetRecommendationsResponseContainerTypeCollection = ApiCall.ApiResponse.GetRecommendationsResponseContainer
            Dim pricingRecommendations As PricingRecommendationsType = resps(0).PricingRecommendations
            If pricingRecommendations Is Nothing Then
                Return
            End If
            If pricingRecommendations.ProductInfo Is Nothing Then
                Return
            End If

            TxtCatalogTitle.Text = pricingRecommendations.ProductInfo.Title
            Dim avgSoldPrice As AmountType = pricingRecommendations.ProductInfo.AverageSoldPrice
            If Not avgSoldPrice Is Nothing Then
                TxtAverageSoldPrice.Text = avgSoldPrice.Value.ToString
            End If
            Dim avgStartPrice As AmountType = pricingRecommendations.ProductInfo.AverageStartPrice
            If Not avgStartPrice Is Nothing Then
                TxtAverageStartPrice.Text = avgStartPrice.Value.ToString
            End If

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub
 
    Private Sub BtnSuggestedAttr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSuggestedAttr.Click
        Try

            Dim ApiCall As GetItemRecommendationsCall = New GetItemRecommendationsCall(Context)
            Dim req As GetRecommendationsRequestContainerType = New GetRecommendationsRequestContainerType()
            req.RecommendationEngine = New RecommendationEngineCodeTypeCollection()
            req.RecommendationEngine.Add(RecommendationEngineCodeType.ProductPricing)
            req.ListingFlow = ListingFlowCodeType.AddItem
            Dim item As ItemType = FillItem(RecommendationEngineCodeType.SuggestedAttributes)
            req.Item = item
            If (TxtQuery.Text.Length > 0) Then
                req.Query = TxtQuery.Text
            End If
            Dim reqc As GetRecommendationsRequestContainerTypeCollection = New GetRecommendationsRequestContainerTypeCollection()
            reqc.Add(req)
            ' Make API call
            ApiCall.GetItemRecommendations(reqc)
            Dim resps As GetRecommendationsResponseContainerTypeCollection = ApiCall.ApiResponse.GetRecommendationsResponseContainer
            Dim attrs As AttributeRecommendationsType = resps(0).AttributeRecommendations
            Dim attrSets As AttributeSetTypeCollection = attrs.AttributeSetArray

            If Not attrSets Is Nothing Then
                Dim attribute As AttributeSetType
                For Each attribute In attrSets
                    If Not attribute Is Nothing Then
                        Dim listparams(3) As String
                        listparams(0) = attribute.attributeSetID.ToString()
                        listparams(1) = attribute.attributeSetVersion
                        listparams(2) = attribute.Attribute.Count.ToString()

                        Dim vi As ListViewItem = New ListViewItem(listparams)
                        LstAttr.Items.Add(vi)
                    End If
                Next attribute
            End If

            Dim products As ProductInfoTypeCollection = resps(0).ProductRecommendations
            If Not products Is Nothing Then
                Dim product As ProductInfoType
                For Each product In products
                    If Not product Is Nothing Then
                        Dim listparams(4) As String
                        listparams(0) = product.Title
                        listparams(1) = product.productInfoID

                        If Not product.AverageStartPrice Is Nothing Then
                            listparams(2) = product.AverageStartPrice.Value.ToString()
                        End If

                        If Not product.AverageSoldPrice Is Nothing Then
                            listparams(3) = product.AverageSoldPrice.Value.ToString()
                        End If

                        Dim vi As ListViewItem = New ListViewItem(listparams)
                        lstProduct.Items.Add(vi)
                    End If
                Next product
            End If

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Function FillItem(ByVal engine As RecommendationEngineCodeType) As ItemType
        Dim item As ItemType = New ItemType()
        If TxtTitle.Text.Length > 0 Then
            item.Title = TxtTitle.Text
        End If

        Dim currencyID As CurrencyCodeType = CurrencyUtility.GetDefaultCurrencyCodeType(Context.Site)

        item.Currency = currencyID
        If (TxtStartPrice.Text.Length > 0) Then
            item.StartPrice = New AmountType()
            item.StartPrice.Value = Convert.ToDouble(TxtStartPrice.Text)
            item.StartPrice.currencyID = currencyID
        End If


        If (TxtReservePrice.Text.Length > 0) Then
            item.ReservePrice = New AmountType()
            item.ReservePrice.Value = Convert.ToDouble(TxtReservePrice.Text)
            item.ReservePrice.currencyID = currencyID
        End If

        If (TxtBuyItNowPrice.Text.Length > 0) Then
            item.BuyItNowPrice = New AmountType()
            item.BuyItNowPrice.Value = Convert.ToDouble(TxtBuyItNowPrice.Text)
            item.BuyItNowPrice.currencyID = currencyID
        End If


        If (TxtPrimaryCategory.Text.Length > 0) Then
            item.PrimaryCategory = New CategoryType()
            item.PrimaryCategory.CategoryID = TxtPrimaryCategory.Text
        End If


        If (TxtSecondaryCategory.Text.Length > 0) Then
            item.SecondaryCategory = New CategoryType()
            item.SecondaryCategory.CategoryID = TxtSecondaryCategory.Text
        End If


        If (TxtItemID.Visible) Then
            item.ItemID = TxtItemID.Text
        End If

        If engine = RecommendationEngineCodeType.ProductPricing Then
            Dim s As String = TxtExternalProductID.Text
            If s.Length > 0 Then
                Dim pld As ProductListingDetailsType = New ProductListingDetailsType()
                pld.ProductID = s
                item.ProductListingDetails = pld
            End If
        End If

        Return item

    End Function

    Private Function StripCDATA(ByVal cdataString As String) As String
        Dim CDATA_START As String = "<![CDATA["
        Dim CDATA_END As String = "]]>"

        If cdataString Is Nothing Then
            Return ""
        End If
        Dim index1 As Integer = cdataString.IndexOf(CDATA_START)
        If index1 = -1 Then
            Return cdataString
        End If
        Dim index2 As Integer = cdataString.IndexOf(CDATA_END)
        If index2 = -1 Then
            Return cdataString.Substring(CDATA_START.Length)
        Else
            Return cdataString.Substring(CDATA_START.Length, (cdataString.Length - CDATA_START.Length - CDATA_END.Length))
        End If

    End Function

    Private Sub BtnGetCategories_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGetCategories.Click
        Dim form As FormGetCategories = New FormGetCategories()
        form.Context = Context
        form.ShowDialog()
    End Sub

    Private Sub BtnGetItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGetItem.Click
        Dim form As FormGetItem = New FormGetItem()
        form.Context = Context
        form.ShowDialog()
        fetchedItem = form.getItem()
        If Not (fetchedItem Is Nothing) Then
            TxtTitle.Text = fetchedItem.Title
            TxtPrimaryCategory.Text = fetchedItem.PrimaryCategory.CategoryID
            Dim secondaryCategory As CategoryType = fetchedItem.SecondaryCategory
            If secondaryCategory Is Nothing Then
                TxtSecondaryCategory.Text = ""
            Else
                TxtSecondaryCategory.Text = secondaryCategory.CategoryID
            End If
            TxtStartPrice.Text = fetchedItem.SellingStatus.CurrentPrice.Value.ToString()
            Dim amt As AmountType = fetchedItem.ReservePrice
            If amt Is Nothing Then
                TxtReservePrice.Text = ""
            Else
                TxtReservePrice.Text = amt.Value.ToString()
            End If
            amt = fetchedItem.BuyItNowPrice
            If amt Is Nothing Then
                TxtBuyItNowPrice.Text = ""
            Else
                TxtBuyItNowPrice.Text = amt.Value.ToString()
            End If
            TxtItemID.Text = fetchedItem.ItemID
        End If
    End Sub

    Private Sub FormGetRecommendations_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim enums As Type = GetType(ListingFlowCodeType)
        Dim item As String
        For Each item In [Enum].GetNames(enums)
            If (item <> "CustomCode") Then
                CboListingFlow.Items.Add(item)
            End If
        Next item

        CboListingFlow.SelectedIndex = 0
    End Sub

    Private Sub CboListingFlow_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboListingFlow.SelectedIndexChanged
        If CboListingFlow.Text.Equals("AddItem") Then
            LblItemID.Visible = False
            TxtItemID.Visible = False
        Else
            LblItemID.Visible = True
            TxtItemID.Visible = True
        End If
    End Sub

    Private Sub TxtTitle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtTitle.TextChanged

    End Sub

    Private Sub LstTips_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstTips.SelectedIndexChanged

    End Sub
End Class

