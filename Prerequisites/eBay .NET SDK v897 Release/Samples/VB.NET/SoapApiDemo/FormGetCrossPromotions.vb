Imports System
Imports System.Globalization
Imports eBay.Service.Core.Soap
Imports eBay.Service.Core.Sdk
Imports eBay.Service.Call
Imports eBay.Service.Util



Public Class FormGetCrossPromotions
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

		Friend WithEvents BtnGetCrossPromotions As System.Windows.Forms.Button 

		Friend WithEvents TxtItemId As System.Windows.Forms.TextBox 

		Friend WithEvents CboMethod As System.Windows.Forms.ComboBox 

		Friend WithEvents LblMethod As System.Windows.Forms.Label 

		Friend WithEvents CboViewMode As System.Windows.Forms.ComboBox 

		Friend WithEvents LblViewMode As System.Windows.Forms.Label 

		Friend WithEvents LblPromotions As System.Windows.Forms.Label 

		Friend WithEvents ClmPriceType As System.Windows.Forms.ColumnHeader 

		Friend WithEvents ClmListingType As System.Windows.Forms.ColumnHeader 



		<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()


			Me.GrpResult = new System.Windows.Forms.GroupBox()

			Me.LblPromotions = new System.Windows.Forms.Label()

			Me.LstCrossPromotions = new System.Windows.Forms.ListView()

			Me.ClmItemId = new System.Windows.Forms.ColumnHeader()

			Me.ClmTitle = new System.Windows.Forms.ColumnHeader()

			Me.ClmPrice = new System.Windows.Forms.ColumnHeader()

			Me.ClmPriceType = new System.Windows.Forms.ColumnHeader()

			Me.ClmListingType = new System.Windows.Forms.ColumnHeader()

			Me.BtnGetCrossPromotions = new System.Windows.Forms.Button()

			Me.LblCategory = new System.Windows.Forms.Label()

			Me.TxtItemId = new System.Windows.Forms.TextBox()

			Me.CboMethod = new System.Windows.Forms.ComboBox()

			Me.LblMethod = new System.Windows.Forms.Label()

			Me.CboViewMode = new System.Windows.Forms.ComboBox()

			Me.LblViewMode = new System.Windows.Forms.Label()

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

        Me.LstCrossPromotions.Anchor = ((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right

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

        ' BtnGetCrossPromotions

        ' 

        Me.BtnGetCrossPromotions.Location = New System.Drawing.Point(152, 88)

        Me.BtnGetCrossPromotions.Name = "BtnGetCrossPromotions"

        Me.BtnGetCrossPromotions.Size = New System.Drawing.Size(128, 23)

        Me.BtnGetCrossPromotions.TabIndex = 23

        Me.BtnGetCrossPromotions.Text = "GetCrossPromotions"

        'Me.BtnGetCrossPromotions.Click += new System.EventHandler(Me.BtnGetCrossPromotions_Click)

        ' 

        ' LblCategory

        ' 

        Me.LblCategory.Location = New System.Drawing.Point(120, 8)

        Me.LblCategory.Name = "LblCategory"

        Me.LblCategory.Size = New System.Drawing.Size(80, 23)

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

        Me.CboMethod.Location = New System.Drawing.Point(200, 32)

        Me.CboMethod.Name = "CboMethod"

        Me.CboMethod.Size = New System.Drawing.Size(136, 21)

        Me.CboMethod.TabIndex = 87

        ' 

        ' LblMethod

        ' 

        Me.LblMethod.Location = New System.Drawing.Point(120, 32)

        Me.LblMethod.Name = "LblMethod"

        Me.LblMethod.Size = New System.Drawing.Size(80, 18)

        Me.LblMethod.TabIndex = 86

        Me.LblMethod.Text = "Method:"

        ' 

        ' CboViewMode

        ' 

        Me.CboViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList

        Me.CboViewMode.Location = New System.Drawing.Point(200, 56)

        Me.CboViewMode.Name = "CboViewMode"

        Me.CboViewMode.Size = New System.Drawing.Size(136, 21)

        Me.CboViewMode.TabIndex = 89

        ' 

        ' LblViewMode

        ' 

        Me.LblViewMode.Location = New System.Drawing.Point(120, 56)

        Me.LblViewMode.Name = "LblViewMode"

        Me.LblViewMode.Size = New System.Drawing.Size(80, 18)

        Me.LblViewMode.TabIndex = 88

        Me.LblViewMode.Text = "View Mode:"

        ' 

        ' FrmGetCrossPromotions

        ' 

			Me.AutoScaleBaseSize = new System.Drawing.Size(5, 13)

			Me.ClientSize = new System.Drawing.Size(472, 437)

			Me.Controls.Add(Me.CboViewMode)

			Me.Controls.Add(Me.LblViewMode)

			Me.Controls.Add(Me.CboMethod)

			Me.Controls.Add(Me.LblMethod)

			Me.Controls.Add(Me.LblCategory)

			Me.Controls.Add(Me.TxtItemId)

			Me.Controls.Add(Me.GrpResult)

			Me.Controls.Add(Me.BtnGetCrossPromotions)

			Me.Name = "FrmGetCrossPromotions"

			Me.Text = "GetCrossPromotions"

			'Me.Load += new System.EventHandler(Me.FrmGetCrossPromotions_Load)

			Me.GrpResult.ResumeLayout(false)

			Me.ResumeLayout(false)

   End Sub

#End Region


    Public Context As ApiContext


		Private Sub  FrmGetCrossPromotions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load


			Dim s() As String = [Enum].GetNames(GetType(PromotionMethodCodeType))
			Dim item As String

			For Each item in s
				If item <> "CustomCode" Then
					CboMethod.Items.Add(item)
				End If
			Next item

			CboMethod.SelectedIndex = 0


			s = [Enum].GetNames(GetType(TradingRoleCodeType))

			For Each item in s
				If item <> "CustomCode" Then
					CboViewMode.Items.Add(item)
				End If
			Next item

			CboViewMode.SelectedIndex = 0

		End Sub


		Private Sub  BtnGetCrossPromotions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGetCrossPromotions.Click

			Try

				LstCrossPromotions.Items.Clear()

				Dim apicall As GetCrossPromotionsCall = new GetCrossPromotionsCall(Context)

				apicall.PromotionViewMode = [Enum].Parse(GetType(TradingRoleCodeType), CboViewMode.SelectedItem.ToString())

				Dim promotions As CrossPromotionsType = apicall.GetCrossPromotions(TxtItemId.Text, [Enum].Parse(GetType(PromotionMethodCodeType), CboMethod.SelectedItem.ToString()))
				Dim promo As PromotedItemType
				For Each promo In promotions.PromotedItem

					Dim listparams(5) As String

                listparams(0) = promo.ItemID

                listparams(1) = promo.Title

                listparams(3) = promo.ListingType.ToString()

					Dim vi As ListViewItem= new ListViewItem(listparams)

					LstCrossPromotions.Items.Add(vi)
				Next promo

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

End Class

