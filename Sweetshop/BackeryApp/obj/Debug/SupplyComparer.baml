   M S B A M L   `   `   `  ����  
BackeryApp    BackeryApp.SupplyComparer   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $
��Auto��5   6   .��5      � -   5          �Name$  contentGrid��5       � ColumnDefinitions 5	   
   �� 5
      $	��160 �6   �� 5      $��* �6   �� 5      $	��140 �6   .+�5   
   �� $��13(�6Q   $	��130��6_   ���#777$��Center=�6}   $
��Left=�6�   $��10,0,0,0q�6�   ��6   �� *Name+   
formatConv �� 	Converter# [  6�   � 5   
   -   5         �Name$ CostPerBasicUnitPanel��5      $��2��63   $��Center=�6C   $��Right=�6^   $��26��6z   $��0,0,10,0q�6�   .+�5      �� -   5         ��Name$ unitCostText��5      $��12(�6r   ���#777$��Center=�6�   $��Center=�6�   $��2��6�   ��6.   �� *Cost.Amount+  
amountConv# [ 6�   �� 5      -   5         ��Name$ unitCostBox��5        regularFontSize#��[ 6�   ���#555$��25��6�   $��54��6�   $��Center=�6�   $��10,0,0,0q�6   �� SelectedIndex 6,   �� *	Cost.Unit+  currencyConv# [ ��6y   �� *UnitCost.AllUnits+6  5      � 5   
   -   5        $  
middleGrid��5      $��1��6#   .+�5      �� -   5         ��Name$ barRect��5      $��1��6)   $��24��66   $
��Left=�6B   $��Center=�6]   ���#2BA9B9 � Triggers 5      :� 5      	 :� RoutedEvent$	 Rectangle.Loaded��6#   .[�5      �� .��5      �� .��5   "   W� 
 W� From$
 0X�62   	 W� To$
 1000X�6;    g� Duration$ 0:0:2D�6E    �� TargetProperty$ (Rectangle.Width)�6V   5      5      5      5       �� 5!      -   5        $��differenceInfo��5!      $��14(�6@   $
��Left=�6N   $��Center=�6i   $��5 0 0 0q�6�   ��6.   �� 6�   5"      5#      5$      