   M S B A M L   `   `   `  ���� D  @eBayFetch, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:eBayFetch	eBayFetch      eBayFetch.SplashScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     )localclr-namespace:eBayFetch       � Title$  SplashScreen��$	��236��$	��414�� � WindowStyle$
 None=� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � ShowInTaskbar .   � Topmost .  � 	Resources    showStoryBoard �� TargetProperty �� 
TargetName<  6System.Windows.Media.Animation.EasingThicknessKeyFrame	 p� KeyTime
 p� Value  HideStoryBoard��%  &	        &	 �     �� .��r� $ (FrameworkElement.Margin)�$ 
txtLoading��.	�  $	 0:0:0��$
 0,0,0,0q�  $	 0:0:1��$
 0,0,0,70q��� .��r� $ (FrameworkElement.Margin)�$ 
txtLoading��.	�  $	 0:0:0��$
 0,0,0,70q�  $	 0:0:1��$
 	0,0,0,150q�.���  � RowDefinitions �� $��1* ��� $��0.618* �����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ Transparent�� � Offset$ 0Q�� $ 	MintCream��$ 1Q�� $ 	LightCyan��$
 0.72Q�.+��� -   $��LabelSplashScreenTitle��$��0��$��30(�$��Center=�$��Center=�$��eBay Fetch 2010��
���   �$��Snap ITC)�$��Normal"��� -   $��LabelSplashScreenSubtitle��$��0��$��20(�$��Center=�$��Bottom=�$��
Loading...��
���   �$��Tahoma)�$��Normal"��� -   $��LabelLoadingInfo��$��1��$��Bottom=�����    eBayFetch.MessageListener   Instance �� Source# Z  �� Path$ Message�