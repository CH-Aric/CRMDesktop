   M S B A M L   `   `   `  ���� E  ACRMDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CRMDesktop.PagesCRMDesktop.Pages  'clr-namespace:CRMDesktop
CRMDesktop  &     CRMDesktop.Pages.PunchAdmin_Page   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:CRMDesktop.Pages     /
crmdesktopclr-namespace:CRMDesktop       N� Title$  PunchAdmin_Page��.���� .@��� $��Hour Calculator��.���� .+�� -    �Name$ 
quoteStack��
���   � � ColumnDefinitions �� $
��132* ��� $	��96* ��� $	��36* ��� $
��132* ��� $
��131* ��� $
��132* ��� $
��131* � � RowDefinitions �� $��25 ��� $��25 �.+��� 
�������$��3��$��2q��� 
�������$��2q�$��3���� 
�������$��2q�$��4���� 
�������$��5��$��2q��� 
�������$��6��$��2q��� $��Agent To View:��$��Right=��� -    ��Name$ Agent��$��1��$��2���� $��View Location Punches:��$��3���� -    ��Name$ LocCheck��$��3��$��Right=�$��Center=�$	��0,6q��� $��Week Beginning:��$��4��(  "System.Windows.Controls.DatePicker  -     Name$ 	DayPicker��$��5��   CRMDesktop.StyledButton     CRMDesktop.StyledButton   Click$ onClick��$��6��$
��View���� 
�������$��2q�$��0��$��1���� 
�������$��2q�$��1��$��1��$��2���� 
�������$��2q�$��3��$��1���� 
�������$��2q�$��4��$��1���� 
�������$��2q�$��5��$��1���� 
�������$��2q�$��6��$��1���� -    ��Name$ HourDisplay��$��Total Hours:��$��1���� -   $ AgentDisplay��$��Viewing:��$��1��$��1��$��2���� -   $ WeekDisplay��$��Week of:��$��1��$��3���� $	��400��.��� -   $ BodyGrid��
���   � �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � �� $��25 �.+��� 
�������$��1q��� 
�������$��1q�$��1���� 
�������$��1q�$��2���� 
�������$��1q�$��3���� $
��Time���� $��Location��$��1���� $��Notes��$��2���� $��In/Out��$��3���� $��	Live Feed��.���� .+�� 
���   � �� $��7* ��� $��5* ��� $��7* ��� $��7* ��� $��7* ��� $	��14* � �� $��25 �.+��� 
�������$��2q��� 
�������$��1��$��2q��� 
�������$��2q�$��2���� 
�������$��2q�$��3���� 
�������$��4��$��2q��� 
�������$��5��$��2q��� $��Name:��$��0��$��Center=��� $��State:��$��1��$��Center=��� $��	Location:��$��2��$��Center=��� $��Time of Punch:��$��3��$��Center=��� $��Age of Punch:��$��4��$��Center=��� $��Notes:��$��5��$��Center=��� -	   	 ��Name$	 scroll��.��� -
   $ LiveBody��
���   � �� $��7* ��� $��5* ��� $��7* ��� $��7* ��� $��7* ��� $	��14* ��� $��Punch Approval��.���� .+�� 
���   � �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � �� $��25 ��� $��25 �.+��� 
�������$��2��$��2q��� 
�������$��2q�$��2���� 
�������$��2q�$��3���� 
�������$��4��$��2q��� 
�������$��5��$��2q��� $��Agent To View:��$��Right=��� -   $ Agent2��$��1���� $��Pending Only?��$��2���� -   $ Pending��$��2��$��Right=�$��Center=�$��4q�  -   $ 
DayPicker2��$��4���� $��Week Of��$��3��$��Right=�  $ onClickPend��$��5��$
��View���� 
�������$��1��$��1q��� 
�������$��1��$��1q�$��1��$��5���� $��Admin Note:��$��1��$��Right=��� -   
 ��Name$
 	AdminNote��$��1��$��1��$��5��$��2q��� -   $	 scroll3��.��� -   $ 
DetailBody��
���   � �� $��5* ��� $��5* ��� $��7* ��� $��7* ��� $��5* ��� $	��14* � �� $��25 �.+��� 
�������$��2q��� 
�������$��2q�$��1���� 
�������$��2q�$��2���� 
�������$��2q�$��3���� 
�������$��2q�$��4���� 
�������$��2q�$��5���� $��Approve��$��0���� $��
Admin Note��$��1���� $
��Time��$��2���� $��Location��$��3���� $��In/Out��$��4���� $
��Note��$��5���� $��	Tardiness��.����  �� Orientation$ Vertical=�.+�� 
���   � �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $��5* ��� $��5* � �� $��25 ��� $��25 �.+��� 
�������$��2q��� 
�������$��2q�$��1���� 
�������$��2q�$��2���� 
�������$��2q�$��3���� 
�������$��2q�$��4���� 
�������$��2q�$��5���� 
�������$��2q�$��6���� 
�������$��2q�$��6��$��1��$��1���� 
�������$��2q�$��1���� -   $ Agent3��  $ onClickTardy��$��1��$
��View���� -   $
 Sick��$��2��$��
Sick Days:���� -   $
 Late��$��3��$��
Late Days:��  -   $ 
DayPicker3��$��4��  $ onClickSick��$��5��$��New Sick��  $ onClickLate��$��6��$��New Late���� $��0��$��1��$��Admin Note:��$��Right=��� -   $
 SickNote��$��1��$��7��$��1���� .��� -   $ 	TardiGrid��
���   � �� $��3* ��� $��3* ��� $	��12* � �� $��25 �.+��� 
�������$��2q��� 
�������$��2q�$��1���� 
�������$��2q�$��2���� $��State���� $
��Date��$��1���� $
��Note��$��2��