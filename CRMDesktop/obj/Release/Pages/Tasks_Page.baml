   M S B A M L   `   `   `  ���� E  ACRMDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CRMDesktop.PagesCRMDesktop.Pages  'clr-namespace:CRMDesktop
CRMDesktop  !    CRMDesktop.Pages.Tasks_Page   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:CRMDesktop.Pages     +local1clr-namespace:CRMDesktop       N� Title$  
Tasks_Page��.���� .+�� 
���   � � ColumnDefinitions �� $	��10* ��� $	��40* ��� $	��30* ��� $	��25* � � RowDefinitions �� $��25 ��� $��25 �.+��� 
�������$��1q�$��0��$��0���� 
�������$��1q�$��1��$��0���� 
�������$��1q�$��2��$��0���� 
�������$��1q�$��3��$��0���� 
�������$��1q�$��0��$��1��$��3���� 
�������$��1q�$��3��$��1��d� -   $��0��$��0���� -    ��Name$ agentPicker�� � ToolTip$ Agent��$��1��$��0��   CRMDesktop.StyledButton     CRMDesktop.StyledButton   Click$ onClickedAssign��$��Transfer��$��2��$��0��  $ onClickedCreate��$��Create��$��3��$��0���� -    ��Name$ SearchEntry��$ 
Search For��$��0��$��1��$��3��  $ onClickedSearch��$��Search��$��3��$��1��� -    �Name$ TSection��
���   � �� $	��85* ��� $	��15* �