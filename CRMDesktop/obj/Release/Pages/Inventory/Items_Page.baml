   M S B A M L   `   `   `  ���� E  ACRMDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:CRMDesktop.Pages.InventoryCRMDesktop.Pages.Inventory  'clr-namespace:CRMDesktop
CRMDesktop  +    %CRMDesktop.Pages.Inventory.Items_Page   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:CRMDesktop.Pages.Inventory     .	MainCRMV2clr-namespace:CRMDesktop       N� Title$  
Items_Page��.���� .+�� -    �Name$ TSection��
���   � � ColumnDefinitions �� $	��50* ��� $	��25* ��� $	��25* � � RowDefinitions �� $	��10* �.+��� 
�������$��1q�$��0��$��0��$��2���� -    ��Name$ AuditPicker�� � ToolTip$ Audit Selection��$��0��$��2���� 
�������$��1q�$��1��$��0��   CRMDesktop.StyledButton     CRMDesktop.StyledButton   Click$ onClickedViewAudit��$
��View��$��2���� .��� -   $ BodyGrid��
���   � �� $	��50* ��� $	��25* ��� $	��25* � �� $��50 �.+��� 
�������$��1q�$��0��$��0���� 
�������$��1q�$��1��$��0���� 
�������$��1q�$��2��$��0���� -    ��Name$ Label1��$
��Item��$��0��$��0��
���   ��� -   $ Label2��$��Count��$��1��$��0��
���   ��� -   $ Label3��$��Value��$��2��$��0��
���   �