   M S B A M L   `   `   `  ����D  @eBayFetch, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:eBayFetch	eBayFetch  5   	       eBayFetch.SplashScreen   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=4.0.0.11019, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   )localclr-namespace:eBayFetch     5   	     � Title$  SplashScreen��5   	   $	��236��5   	   $	��414��6    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�6A    � ShowInTaskbar .   � Topmost .  � 	Resources 5	         showStoryBoard �� TargetProperty �� 
TargetName<  6System.Windows.Media.Animation.EasingThicknessKeyFrame	 p� KeyTime
 p� Value  HideStoryBoard��%�  &	        &	 �     �� 5
   
   .��5      r� $ (FrameworkElement.Margin)�6/   $ 
txtLoading��6e   .	�5        $	 0:0:0��6*   $
 0,0,0,0q�6:     5      $	 0:0:1��6*   $
 0,0,0,70q�6:   5      5      �� 5   
   .��5      r� $ (FrameworkElement.Margin)�6/   $ 
txtLoading��6e   .	�5        $	 0:0:0��6*   $
 0,0,0,70q�6:     5      $	 0:0:1��6*   $
 	0,0,0,150q�6:   5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��0.618* �6   ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5       �  � Color$ Transparent��6    � Offset$ 0Q�63   � 5!      $ 	MintCream��6   $ 1Q�61   � 5"      $ 	LightCyan��6   $
 0.72Q�61   5#      .+�5&   
   �� -   5        $��LabelSplashScreenTitle��5&      $��0��6   $��30(�6;   $��Center=�6I   $��Center=�6m   $��eBay Fetch 2010��6�   
���   �$��Snap ITC)�6�   $��Normal"�6�   6�   �� 5'   
   -   5        $��LabelSplashScreenSubtitle��5'      $��0��6   $��20(�6>   $��Center=�6L   $��Bottom=�6p   $��
Loading...��6�   
���   �$��Tahoma)�6�   $��Normal"�6�   6�   �� 5)   
   -   5        $��LabelLoadingInfo��5)      $��1��6   $��Bottom=�65   ��6P   ��    eBayFetch.MessageListener   Instance �� Source# Z  �� Path$ Message�6�   5+      5-      