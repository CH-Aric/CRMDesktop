   M S B A M L   `   `   `  ���� E  ACRMDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CRMDesktop.PagesCRMDesktop.Pages  'clr-namespace:CRMDesktop
CRMDesktop  !    CRMDesktop.Pages.Punch_Page   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:CRMDesktop.Pages     /
crmdesktopclr-namespace:CRMDesktop       N� Title$  
Punch_Page��.���� .+�� 
���   � � ColumnDefinitions �� $��7* ��� $��7* ��� $	��20* ��� $	��10* � � RowDefinitions �� $��25 �.+��� 
�������$��1��$��1q��� 
�������$��2��$��1q��� 
�������$��3��$��1q�   CRMDesktop.StyledButton  -     Name$ PunchButton��   CRMDesktop.StyledButton   Click$ onclick��$��Perform Punch��  $ onClickStateless��$��Perform Location Punch��$��1���� -    ��Name$ 	TextEntry��$��2��$
��Note��$��Center=��� -    ��Name$ 
ClockState��$ ��Current State: Clocked Out��$��3��$��Center=�
���   ��� -    ��Name$ scroll��.��� -    �Name$ logGrid��
���   � �� $	��10* ��� $��5* ��� $��5* � �� $��25 �.+��� 
�������$��0��$��1q��� 
�������$��1��$��1q��� 
�������$��2��$��1q��� $��Notes:��$��0��$��Center=�
���   ��� $
��Time��$��1��$��Center=�
���   ��� $��Punch State��$��2��$��Center=�
���   �