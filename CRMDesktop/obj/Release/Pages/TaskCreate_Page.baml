   M S B A M L   `   `   `  ���� E  ACRMDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CRMDesktop.PagesCRMDesktop.Pages  'clr-namespace:CRMDesktop
CRMDesktop  &     CRMDesktop.Pages.TaskCreate_Page   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:CRMDesktop.Pages     .	maincrmv2clr-namespace:CRMDesktop       N� Title$  TaskCreate_Page��.���� -    ��Name$ scroll��$��Visible=�.��� -    �Name$ 	gridStack��
���   � � ColumnDefinitions �� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* ��� $	��10* � � RowDefinitions �� $��25 ��� $��25 �.+��� 
�������$��0��$��0��$��2q��� 
�������$��0��$��1��$��2q��� 
�������$��0��$��2��$��2q��� 
�������$��0��$��3��$��2q��� 
�������$��0��$��4��$��2q��� 
�������$��0��$��5��$��2q��� $��0��$��0��$��	Template:��$��Right=��� -    ��Name$ templatePicker��$��0��$��1��$��2q�
�������   CRMDesktop.StyledButton     CRMDesktop.StyledButton   Click$ onClickedLoadTemplate��$��Load From Template��$��0��$��2���� $��0��$��3��$��Save Task As:��$��Right=��� -    ��Name$ taskName��$�� �� � ToolTip$ SaveAs��$��1,2,3,2q�$��0��$��4��
�������  $ onClickedCreate��$��Create Task��$��0��$��5���� 
�������$��1��$��0��$��2q��� 
�������$��1��$��1��$��2q��� 
�������$��1��$��2��$��2q��� 
�������$��1��$��3��$��2q��� 
�������$��1��$��4��$��2q��� 
�������$��1��$��5��$��2q��� $��1��$��0��$��Save Template As:��$��Right=��� -   $ templateName��$�� ��$ SaveAs��$��2q�$��1��$��1��
�������  $ onClickedSaveTemplate��$��Save as Template��$��1��$��4���� $��1��$��2��$��Template Description:��$��Right=��� -   $ 	descField��$�� ��$ SaveAs��$��2q�$��1��$��3��
�������  $ onClickAddFields��$��
Add Fields��$��1��$��5��