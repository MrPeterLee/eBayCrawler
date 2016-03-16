Imports System
Imports System.Globalization
Imports eBay.Service.Core.Soap
Imports eBay.Service.Core.Sdk
Imports eBay.Service.Call
Imports eBay.Service.Util



Public Class FormGetPromotionRules
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


    Friend WithEvents GrpResult As System.Windows.Forms.GroupBox

    Friend WithEvents ClmItemId As System.Windows.Forms.ColumnHeader

    Friend WithEvents ClmTitle As System.Windows.Forms.ColumnHeader

    Friend WithEvents ClmPrice As System.Windows.Forms.ColumnHeader

    Friend WithEvents LblCategory As System.Windows.Forms.Label

    Friend WithEvents LstCrossPromotions As System.Windows.Forms.ListView

    Friend WithEvents BtnGetPromotionRules As System.Windows.Forms.Button

    Friend WithEvents TxtItemId As System.Windows.Forms.TextBox

    Friend WithEvents CboMethod As System.Windows.Forms.ComboBox

    Friend WithEvents LblMethod As System.Windows.Forms.Label

    Friend WithEvents LblPromotions As System.Windows.Forms.Label

    Friend WithEvents ClmPriceType As System.Windows.Forms.ColumnHeader

    Friend WithEvents ClmListingType As System.Windows.Forms.ColumnHeader

    Friend WithEvents LblStoreCat As System.Windows.Forms.Label

    Friend WithEvents TxtStoreCat As System.Windows.Forms.TextBox



    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()



        Me.GrpResult = New System.Windows.Forms.GroupBox()

        Me.LblPromotions = New System.Windows.Forms.Label()

        Me.LstCrossPromotions = New System.Windows.Forms.ListView()

        Me.ClmItemId = New System.Windows.Forms.ColumnHeader()

        Me.ClmTitle = New System.Windows.Forms.ColumnHeader()

        Me.ClmPrice = New System.Windows.Forms.ColumnHeader()

        Me.ClmPriceType = New System.Windows.Forms.ColumnHeader()

        Me.ClmListingType = New System.Windows.Forms.ColumnHeader()

        Me.BtnGetPromotionRules = New System.Windows.Forms.Button()

        Me.LblCategory = New System.Windows.Forms.Label()

        Me.TxtItemId = New System.Windows.Forms.TextBox()

        Me.CboMethod = New System.Windows.Forms.ComboBox()

        Me.LblMethod = New System.Windows.Forms.Label()

        Me.LblStoreCat = New System.Windows.Forms.Label()

        Me.TxtStoreCat = New System.Windows.Forms.TextBox()

        Me.GrpResult.SuspendLayout()

        Me.SuspendLayout()

        ' 

        ' GrpResult

        ' 

        Me.GrpResult.Controls.Add(Me.LblPromotions)

        Me.GrpResult.Controls.Add(Me.LstCrossPromotions)

        Me.GrpResult.Location = New System.Drawing.Point(8, 128)

        Me.GrpResult.Name = "GrpResult"

        Me.GrpResult.Size = New System.Drawing.Size(456, 296)

        Me.GrpResult.TabIndex = 24

        Me.GrpResult.TabStop = False

        Me.GrpResult.Text = "Results"

        ' 

        ' LblPromotions

        ' 

        Me.LblPromotions.Location = New System.Drawing.Point(16, 24)

        Me.LblPromotions.Name = "LblPromotions"

        Me.LblPromotions.Size = New System.Drawing.Size(112, 23)

        Me.LblPromotions.TabIndex = 15

        Me.LblPromotions.Text = "Promotions:"

        ' 

        ' LstCrossPromotions

        ' 

        Me.LstCrossPromotions.Anchor = (((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)

        Me.LstCrossPromotions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ClmItemId, Me.ClmTitle, Me.ClmPrice, Me.ClmPriceType, Me.ClmListingType})

        Me.LstCrossPromotions.GridLines = True

        Me.LstCrossPromotions.Location = New System.Drawing.Point(8, 48)

        Me.LstCrossPromotions.Name = "LstCrossPromotions"

        Me.LstCrossPromotions.Size = New System.Drawing.Size(440, 232)

        Me.LstCrossPromotions.TabIndex = 4

        Me.LstCrossPromotions.View = System.Windows.Forms.View.Details

        ' 

        ' ClmItemId

        ' 

        Me.ClmItemId.Text = "ItemId"

        Me.ClmItemId.Width = 80

        ' 

        ' ClmTitle

        ' 

        Me.ClmTitle.Text = "Title"

        Me.ClmTitle.Width = 135

        ' 

        ' ClmPrice

        ' 

        Me.ClmPrice.Text = "Price"

        Me.ClmPrice.Width = 49

        ' 

        ' ClmPriceType

        ' 

        Me.ClmPriceType.Text = "Price Type"

        Me.ClmPriceType.Width = 70

        ' 

        ' ClmListingType

        ' 

        Me.ClmListingType.Text = "Listing Type"

        Me.ClmListingType.Width = 96

        ' 

        ' BtnGetPromotionRules

        ' 

        Me.BtnGetPromotionRules.Location = New System.Drawing.Point(152, 88)

        Me.BtnGetPromotionRules.Name = "BtnGetPromotionRules"

        Me.BtnGetPromotionRules.Size = New System.Drawing.Size(128, 23)

        Me.BtnGetPromotionRules.TabIndex = 23

        Me.BtnGetPromotionRules.Text = "GetPromotionRules"

        'Me.BtnGetPromotionRules.Click += New System.EventHandler(Me.BtnGetPromotionRules_Click)

        ' 

        ' LblCategory

        ' 

        Me.LblCategory.Location = New System.Drawing.Point(104, 8)

        Me.LblCategory.Name = "LblCategory"

        Me.LblCategory.Size = New System.Drawing.Size(96, 23)

        Me.LblCategory.TabIndex = 78

        Me.LblCategory.Text = "Item Id:"

        ' 

        ' TxtItemId

        ' 

        Me.TxtItemId.Location = New System.Drawing.Point(200, 8)

        Me.TxtItemId.Name = "TxtItemId"

        Me.TxtItemId.TabIndex = 77

        Me.TxtItemId.Text = ""

        ' 

        ' CboMethod

        ' 

        Me.CboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList

        Me.CboMethod.Location = New System.Drawing.Point(200, 56)

        Me.CboMethod.Name = "CboMethod"

        Me.CboMethod.Size = New System.Drawing.Size(136, 21)

        Me.CboMethod.TabIndex = 87

        ' 

        ' LblMethod

        ' 

        Me.LblMethod.Location = New System.Drawing.Point(104, 56)

        Me.LblMethod.Name = "LblMethod"

        Me.LblMethod.Size = New System.Drawing.Size(88, 18)

        Me.LblMethod.TabIndex = 86

        Me.LblMethod.Text = "Method:"

        ' 

        ' LblStoreCat

        ' 

        Me.LblStoreCat.Location = New System.Drawing.Point(104, 32)

        Me.LblStoreCat.Name = "LblStoreCat"

        Me.LblStoreCat.Size = New System.Drawing.Size(96, 23)

        Me.LblStoreCat.TabIndex = 89

        Me.LblStoreCat.Text = "Store Category:"

        ' 

        ' TxtStoreCat

        ' 

        Me.TxtStoreCat.Location = New System.Drawing.Point(200, 32)

        Me.TxtStoreCat.Name = "TxtStoreCat"

        Me.TxtStoreCat.TabIndex = 88

        Me.TxtStoreCat.Text = ""

        ' 

        ' FrmGetPromotionRules

        ' 

        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)

        Me.ClientSize = New System.Drawing.Size(472, 437)

        Me.Controls.Add(Me.LblStoreCat)

        Me.Controls.Add(Me.TxtStoreCat)

        Me.Controls.Add(Me.CboMethod)

        Me.Controls.Add(Me.LblMethod)

        Me.Controls.Add(Me.LblCategory)

        Me.Controls.Add(Me.TxtItemId)

        Me.Controls.Add(Me.GrpResult)

        Me.Controls.Add(Me.BtnGetPromotionRules)

        Me.Name = "FrmGetPromotionRules"

        Me.Text = "GetPromotionRules"

        'Me.Load += New System.EventHandler(Me.FrmGetPromotionRules_Load)

        Me.GrpResult.ResumeLayout(False)

        Me.ResumeLayout(False)
    End Sub

#End Region


    Public Context As ApiContext

    Private Sub FrmGetPromotionRules_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim s() As String = ([Enum]).GetNames(GetType(PromotionMethodCodeType))
        Dim item As String

        For Each item In s

            If item <> "CustomCode" Then

                CboMethod.Items.Add(item)

            End If
        Next item

        CboMethod.SelectedIndex = 0

    End Sub



    Private Sub BtnGetPromotionRules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGetPromotionRules.Click
        Try

            LstCrossPromotions.Items.Clear()

            Dim apicall As GetPromotionRulesCall = New GetPromotionRulesCall(Context)



            If TxtItemId.Text <> String.Empty Then

                apicall.GetPromotionRules(TxtItemId.Text, ([Enum]).Parse(GetType(PromotionMethodCodeType), CboMethod.SelectedItem.ToString()))

            Else

                apicall.GetPromotionRules(Convert.ToInt32(TxtStoreCat.Text), ([Enum]).Parse(GetType(PromotionMethodCodeType), CboMethod.SelectedItem.ToString()))

            End If

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub

End Class
