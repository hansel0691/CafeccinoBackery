   M S B A M L   `   `   `  ����E  ABackeryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:BackeryApp
BackeryApp  5   K   =#clr-namespace:BackeryApp.ClassUtilsBackeryApp.ClassUtils  5          BackeryApp.IndexTemplates   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      (locclr-namespace:BackeryApp     6K   :
ClassUtils#clr-namespace:BackeryApp.ClassUtils     5        � 	Resources  5       �� 
TargetType �� Property �� Value   producedUnits  templateCost 
 profit  profitPercentage��%U  '��       &	  �     &	 �     &	 J    &	 �    �� 5	   
   " ��6   .�5
      ��  � ��$ Stretch=�6;   �� 5       � �� �#3336+   -   5        5      �� 5   
   .R�5      �� $��Center=�6   ��65   �� *ProducedUnits+5      �� 5   
   .R�5      �� $��Center=�6   $��Center=�63   ��6Q   �� *Cost.Identifier+5      �� 5   
   .R�5      �� $��Center=�6   $��Center=�63   ��6Q   �� *Profit.Identifier+5      �� 5   
   .R�5      �� $��Center=�6   $��Center=�63   ��6Q   �� *
Percentage+5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��9* �6   �� 5      $
��161* �6    � RowDefinitions 5    
   �� 5!      $��63 �6   �� 5"      $��40 �6   �� 5#      $
��467* �6   �� 5$      $��56 �6   .+�5&   
   �� $��Fichas de Costo��6   ���#3399ff  headerFontSize#��[ 6@   $��SemiBold"�6k   $
��Left=�6�   $	��Top=�6�   $��5,20,0,0q�6�   $��1��6�   �� 5(   
   -   5         ��Name$ searchTempateText��5(      $ ��Filtrar Fichas de Costo...��6-   $��14(�6O   ���#aaa$��26��6o   $	��300��6{    � ToolTip$< 6Introduzca un nombre para filtrar en la base de datos.��6�   $��1��6�   $��1��6�   $
��Left=�6�   $��Center=�6   $��5 0 0 0q�6"  �� 5)   
   $��Ver...��6   $��14(�6"   $��Italic$�60   ���#555$��1��6U   $��1��6b   $��Right=�6r   $��Center=�6�   $��
10,0,200,0q�6�   �� 5*   
   -   5         ��Name$ selectedFilterBox��5*      $��14(�6.   	 �� SelectedIndex$	 0��6<   ���#666$	��140��6`   $��1��6l   $��1��6y   $��Right=�6�   $��Center=�6�   $��0 0 50 0q�6�   .@�5+      �� Todo6   �� 5,      Complementarias6)   �� 5-      
Terminadas6$   5.      �� 50   
   -   5        
 ��Name$
 listView��50      ���#aaa$��	5,6,50,15q�69   $��2��6L   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6Y    �� SelectionMode$ Single=�6~   $��1��6�    �� View 51      �� 52       �� AllowsColumnReorder .  �� ColumnHeaderToolTip$  Templates en base de datos��66   .P�53      �� $��Nombre��6%    �� Width$	 110��65    �� DisplayMemberBinding 6A   �� *Name+�� 54      $��Descripción��6%   $	 150��6:    6F   �� *Description+  
stringConv �� 	Converter# [ �� 55      $��Unidades Producidas��6%   $	 140��6B    �� CellTemplate# [  6N   �� 56      $��Costo Total��6%   $	 140��6:   # [ 6F   �� 57      $��Ganancia Neta��6%   $	 150��6<   # [ 6H   �� 58      $��% de Ganancia��6%   $	 150��6<   # [ 6H   59      5;      � 5<   
   $��3��6   $��2��6   .+�5=      �� -   5         ��Name$ goSuppliesIndex��5=      $��_Insumos��6.   $��20��6A   $��70��6N   $��Bottom=�6Y   $
��Left=�6t   $��	50,0,0,15q�6�   �� 5>      -   5        $ newTemplateButton��5>      $��Nueva...��60   $��20��6C   $��70��6P   $��Bottom=�6[   $
��Left=�6v   $��
130,0,0,15q�6�   �� 5?      -   5        $ delTemplateButton��5?      $��Eliminar��60   $��20��6C   $��70��6O   $��2��6Z   $��Bottom=�6g   $��Right=�6�   $��
0,0,210,15q�6�   �� 5@      -   5        $ 
editButton��5@      $��Editar��6)   $��20��6:   $��70��6F   $��2��6Q   $��Bottom=�6^   $��Right=�6y   $��
0,0,130,15q�6�   �� 5A      -	   5        $ cancelButton��5A      $��_Atrás��6+   $��20��6<   $��70��6H   $��2��6S   $��Bottom=�6`   $��Right=�6{   $��	0,0,50,15q�6�   5B      5C      5D      