   M S B A M L   `   `   `  ����  
BackeryApp    BackeryApp.IndexSupplies   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5	       �� 
TargetType �� Property �� Value   formatAmount  
formatCost��%V  '��       &	  �     &	 �     �� 5
   
   " ��6   .�5      ��  � ��$ Stretch=�6;   �� 5       � �� �#3336+   -   5        5      �� 5   
   .R�5      �� $��Center=�6   ��65   �� *Amount+5      �� 5   
   .R�5      �� $��Center=�6   $��Center=�64   ��6Q   �� *Cost+5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $	��23* �6   �� 5      $
��402* �6    � RowDefinitions 5   
   �� 5      $��63 �6   �� 5      $��40 �6   �� 5      $
��464* �6   �� 5       $	��56* �6   .+�5"   
   �� $��Insumos��6   ���#3399ff  headerFontSize#��[ 68   $��SemiBold"�6c   $
��Left=�6y   $��Center=�6�   $��	4,19,0,18q�6�   $��1��6�   �� 5$   
   -   5         ��Name$ searchTempateText��5$      $��Filtrar Insumo...��6-   ���#aaa$��14(�6X   $��25��6f   $	��300��6r    � ToolTip$< 6Introduzca un nombre para filtrar en la base de datos.��6~   $��1��6�   $��1��6�   $
��Left=�6�   $��Center=�6�   $��Center=�6  $��5 0 0 0q�64  �� 5%   
   $��Ver...��6   $��14(�6"   $��Italic$�60   ���#555$��1��6U   $��1��6b   $��Right=�6r   $��Center=�6�   $��
10,0,200,0q�6�   �� 5&   
   -   5         ��Name$ selectedFilterBox��5&      $��14(�6.   ���#666$	��140��6w   $��1��6�   $��1��6�   $��Right=�6�   $��Center=�6�   $��0 0 50 0q�6�   	 �� SelectedIndex	 6<   �� *SelectedOption+.@�5'      �� Todo6   �� 5(      	Insumos6!   �� 5)      Complementarias6)   5*      �� 5,   
   -   5        
 ��Name$
 listView��5,      ���#aaa$��	5,6,50,15q�6Y   $��2��6l   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6y    �� SelectionMode$ Single=�6�   $��1��6�   ��68   �� *
Supplies+ �� View 5-      �� 5.       �� ColumnHeaderToolTip$ Insumos en base de datos.��6   .P�5/      �� $��Nombre��6%    �� Width$	 180��65    �� DisplayMemberBinding 6A   �� *Name+�� 50      $��Descripción��6%   $	 220��6:    6F   �� *Description+  
stringConv �� 	Converter# [ �� 51      $��Cantidad del Formato��6%   $	 150��6C    �� CellTemplate# [  6O   �� 52      $��Costo del Formato��6%   $	 150��6@   # [ 6L   53      55      � 56   
   $��3��6   $��2��6   .+�57      �� -   5         ��Name$ goTemplatesIndex��57      $��_Fichas de Costo��6/   $��20��6J   $��90��6W   $��Bottom=�6b   $
��Left=�6}   $��	50,0,0,15q�6�   �� 58      -   5        $ newSupplyButton��58      $��	_Nueva...��6.   $��20��6B   $��70��6O   $��Bottom=�6Z   $
��Left=�6u   $��
150,0,0,15q�6�   �� 59      -   5        $ delSupplyButton��59      $��	_Eliminar��6.   $��20��6B   $��70��6N   $��2��6Y   $��Bottom=�6f   $��Right=�6�   $��
0,0,210,15q�6�   �� 5:      -   5        $ editSupplyButton��5:      $��E_ditar��6/   $��20��6A   $��70��6M   $��2��6X   $��Bottom=�6e   $��Right=�6�   $��
0,0,130,15q�6�   �� 5;      -	   5        $ cancelButton��5;      $��_Atrás��6+   $��20��6<   $��70��6H   $��2��6S   $��Bottom=�6`   $��Right=�6{   $��	0,0,50,15q�6�   5<      5=      5>      