   M S B A M L   `   `   `  ���� E  ACRMDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CRMDesktop.PagesCRMDesktop.Pages  'clr-namespace:CRMDesktop
CRMDesktop  !    CRMDesktop.Pages.Price_Page   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:CRMDesktop.Pages     +local1clr-namespace:CRMDesktop       N� Title$  
Price_Page��.����  �� Orientation$ Vertical=�.+�� -    �Name$ 
HeaderGrid��
���   �$��1,1,1,1q� � ColumnDefinitions �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � � RowDefinitions �� $��25 �.+��� 
�������$��1q�$��0��$��0��$��2���� -    ��Name$ 
TypePicker��$��0��$��2��.@��� $	��A/C���� $��Furnace���� $��
Water Tank���� $��Tankless���� 
�������$��1q�$��2��$��0��$��2���� -   $ BrandPicker��$��2��$��2��   CRMDesktop.StyledButton     CRMDesktop.StyledButton   Click$ 	onClicked��$
��View��$��4��$��2���� -   $��scroll��$��Visible=�$
��Auto=�.��� -   $ TSection��
������ $��1,1,1,1q� �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* �