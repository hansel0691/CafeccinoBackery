   M S B A M L   `   `   `  ����  
BackeryApp!    BackeryApp.IndexProductions   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5       �� 
TargetType �� Property �� Value   ProductionCost  ProductionProfits��%W  '��       &	  �     &	 �     �� 5	   
   " ��6   .�5
      ��  � ��$ Stretch=�6;   �� 5       � �� �#3336+   -   5        5      �� 5   
   .R�5      �� $��Center=�6   ��65   �� *Cost+5      �� 5   
   .R�5      �� $��Center=�6   $��Center=�64   ��6Q   �� *	Profits+5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��9* �6   �� 5      $
��161* �6    � RowDefinitions 5   
   �� 5      $��63 �6   �� 5      $��40 �6   �� 5      $
��467* �6   �� 5      $��56 �6   .+�5!   
   �� $��Producciones��6   ���#3399ff  headerFontSize#��[ 6=   $��SemiBold"�6h   $
��Left=�6~   $	��Top=�6�   $��5,20,0,0q�6�   $��1��6�   �� 5#   
   -   5         ��Name$ SearchProductionText��5#      $��Filtrar Producciones...��60   $��14(�6O   ���#aaa$��26��6o   $	��300��6{    � ToolTip$< 6Introduzca un nombre para filtrar en la base de datos.��6�   $��1��6�   $��1��6�   $
��Left=�6�   $��Center=�6   $��5 0 0 0q�6"  �� 5$   
   -   5         ��Name$ listView��5$      ���#aaa$��	5,0,30,21q�69   $��2��6L   $	 �� IsSynchronizedWithCurrentItem$
	 TrueY�6}   
 �� SelectionMode$
 Single=�6�   $��1��6�   ��6Y   �� *Productions+ �� View 5%      �� 5&       �� AllowsColumnReorder .  �� ColumnHeaderToolTip$# Producciones en base de datos��66   .P�5'      �� $��Identificador��6%    �� Width$	 150��6<    �� DisplayMemberBinding 6H   �� *Name+�� 5(      $��Descripción��6%   $	 200��6:    6F   �� *Description+  
stringConv �� 	Converter# [ �� 5)      $��Costo Total��6%   $	 180��6:    �� CellTemplate# [  6F   �� 5*      $��	Ganancias��6%   $	 170��68   # [ 6D   5+      5-      � 5.   
   $��3��6   $��2��6   $��0��60   .+�5/      �� -   5         ��Name$ NewProductionButton��5/      $��	_Nueva...��62   $��20��6F   $��70��6S   $��Bottom=�6^   $
��Left=�6y   $��	50,0,0,15q�6�   �� 50      -   5        $ DelProductionButton��50      $��	_Eliminar��62   $��20��6F   $��70��6R   $��2��6]   $��Bottom=�6j   $��Right=�6�   $��
0,0,210,15q�6�   �� 51      -   5        $ EditProductionButton��51      $��E_ditar��63   $��20��6E   $��70��6Q   $��2��6\   $��Bottom=�6i   $��Right=�6�   $��
0,0,130,15q�6�   �� 52      -   5        $ CancelButton��52      $��_Atrás��6+   $��20��6<   $��70��6H   $��2��6S   $��Bottom=�6`   $��Right=�6{   $��	0,0,50,15q�6�   53      54      55      