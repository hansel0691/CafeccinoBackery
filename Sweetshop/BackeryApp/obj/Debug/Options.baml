   M S B A M L   `   `   `  ����  
BackeryApp    BackeryApp.Options   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Options��5   	   $	��365��6   $	��650��6&    � WindowStyle$ 
ToolWindow=�62    � 
ResizeMode$ NoResize=�6K   ���#f0f0f0.��5      �  � ColumnDefinitions 5   
   �� 5      $��0.306* �6   �� 5      $��0.694* �6   .+�5
   
   ��  �� SelectionMode$ Single=�6   ���#999$��10,20,20,20q�6>   .@�5      �� $��Primer Módulo��6   �� 5      $��Segundo Módulo��6   �� 5      $��Tercer Módulo��6   �� 5      $��Cuarto Módulo��6   5      �� 5   
   $��1��6   $��Moneda��6#   $��	0,20,10,0q�63   $	��Top=�6F   $	��100��6^   .��5      � $��0 10 0 0q�6   .+�5      � .+�5      �� $��Ratio de Cambio:��6    $��14(�68   ���#212121$	��Top=�6\   $
��Left=�6t   $��10 3 0 0q�6�   �� 5      -   5         ��Name$ currencyRatio��5      $	��Top=�6|   $��12(�6�    � ToolTip$ Valor de 1 CUC en CUP.��6�   $��24��6�   $��Center=�6�   $	��100��6�   $
��Left=�6�   $��10 0 0 0q�6  ��65   �� *CurrencyRatio+   definedConv �� 	Converter# [  6  5      5      5      �� 5   
   -   5         ��Name$ 	cancelBtn��5      $��Cancelar��6$   $��1��67   $��Right=�6G   $��	0,0,10,20q�6c   $��Bottom=�6v   $��88��6�   �� 5   
   -   5        $ okBtn��5      $��Ok��6    $��1��6-   $��
0,0,110,20q�6=   $��Bottom=�6Q   $��Right=�6l   $��88��6�   5      5      