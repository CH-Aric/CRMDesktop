   M S B A M L   `   `   `  ���� E  ACRMDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:CRMDesktop.Pages.CustomersCRMDesktop.Pages.Customers  'clr-namespace:CRMDesktop
CRMDesktop  +    %CRMDesktop.Pages.Customers.Quote_Page   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:CRMDesktop.Pages.Customers     .	MainCRMV2clr-namespace:CRMDesktop     X TXceed.Wpf.Toolkit, Version=3.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit    N� Title$  
Quote_Page��.���� -    ��Name$ scroll��$��Visible=�.���� .+�� -    �Name$ 
quoteStack��
���   � � ColumnDefinitions �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � � RowDefinitions �� $��30 ��� $��30 �.+��� 
�������$��0��$��0��$��2q��� 
�������$��0��$��1��$��2q��� 
�������$��0��$��2��$��2q��� 
�������$��0��$��3��$��2q��� 
�������$��0��$��4��$��2q��� 
�������$��0��$��5��$��2q��� -    ��Name$ 	nameLabel��$��NameHere��
���   �$��2q�&   Xceed.Wpf.Toolkit.DateTimePicker  -     Name$ contactLabel��$��0��$��3��$��2q��� $��	Salesman:��$��0��$��1��$��2q�$��Right=�$��Center=��� -    ��Name$ SalemanCombo��$��0��$��2���� -   $ 
phoneLabel��$��	PhoneHere��$��0��$��4��   CRMDesktop.StyledButton  -     Name$ StageButton��   CRMDesktop.StyledButton	   Click$	 onClickAdvance��$��Advance��$��0��$��5���� 
�������$��1��$��0��$��2q��� 
�������$��1��$��1��$��2q��� 
�������$��1��$��2��$��2q��� 
�������$��1��$��3��$��2q��� 
�������$��1��$��4��$��2q��� 
�������$��1��$��5��$��2q��� $��Quote Total:��$��1��$��0��$��2q�$��Right=��� -   $ 
QuoteTotal��$��1��$��1��$��2q��� -	   $ PriceGuidecombo��$��1��$��2��  $ 	 onClickAddPrefilledFieldsQ��$��Add PriceGuide Item��$��1��$��3��  $	 onClickAddFieldsQ��$��Add Blank Item��$��1��$��4��  $	 	onClicked��$��Save Changes��$��1��$��5���� -
   
 ��Name$	
 Tab��.@��� ���� $��Option 1��$��25��$	��Top=�.���� $	��200��.��� -   $ Option1�� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� ���� $��Option 2��$��25��$	��Top=�.���� $	��200��.��� -   $ Option2�� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� ���� $��Option 3��$��25��$	��Top=�.���� $	��200��.��� -   $ Option3�� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� ���� $��Option 4��$��25��$	��Top=�.���� $	��200��.��� -   $ Option4�� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� ���� $��Option 5��$��25��$	��Top=�.���� $	��200��.��� -   $ Option5�� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* �� -   $ bottomStack��
���   � �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � �� $��30 �.+��� 
�������$��0��$��0��$��2q��� 
�������$��0��$��1��$��2q��� 
�������$��0��$��2��$��2q��� 
�������$��0��$��3��$��2q��� 
�������$��0��$��4��$��2q��� 
�������$��0��$��5��$��2q�  -   $	 row��$	 onClickAddFields��$��	Add Field��  -   $	 sig��$	 onClickAddFields��$��Add Signature��$��0��$��1��  -   $	 fie��$	 onClickAddFields��$��Add Deposit Field��$��0��$��2��  -   $	 met��$	 onClickAddFields��$��Add Payment Method��$��0��$��3��  $	 onFileButton��$��View/Modify Files��$��0��$��4��  $	 	onClicked��$��Save Changes��$��0��$��5��� -   $ fileGrid��
���   � �� $	��10* �