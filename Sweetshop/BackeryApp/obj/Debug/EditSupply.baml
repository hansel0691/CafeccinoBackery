   M S B A M L   `   `   `  ����  
BackeryApp    BackeryApp.EditSupply   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � Title$  Insumo��5       � WindowStyle$ 
ToolWindow=�6   $	��250��66   $	��550��6C    � 
ResizeMode$ NoResize=�6O    � Topmost .  � WindowStartupLocation$ CenterScreen=�5      .��5	      � -   5         �Name$ content��5	       � RowDefinitions 5
   
   �� 5      $��50 �6   6*   �� 5      $��70 �6   6*   �� 5      6   �� 5      $��40 �6   6*    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      6!   .+�5   
   �� -   5         ��Name$ 	nameLabel��5      $��_Nombre:��6#   $��14(�66   $��0��6D   $��0��6T   $��Center=�6a   $��Right=�6|   $��
0,10,10,10q�6�   	 �� Target	 6�   �� 
 �� ElementName$
 nameText���� 5   
   -   5         ��Name$ nameText��5       �� StaysOpenOnEdit .  �� IsTextSearchEnabled .  �� 
IsEditable . $��14(�6�   ���#212121$��26��6�   $	��380��6�    � ToolTip$ Nombre del insumo.��6�   $��1��6  $��0��6  $��Center=�6  $��Center=�6@  $��0,12,150,12q�6[  ��6<   �� *SuppliesName+ �� Text 6�   �� *Name+6[  �� 5   
   -   5         ��Name$ 
frameImage��5      ���#fff$	��100��67   $	��100��6D   ���#eee$��1q�6c   $> 8Seleccione la imagen del formato dando click sobre ella.��6w   $��0��6�   $��2��6�   $��1��6�   $��Right=�6�   $	��Top=�6  $��	0,10,20,0q�6   � Cursor$
 Hand��60  ._�5      �� -   5         ��Name$	 img��5       J� BitmapEffect 5      F� 5       F� Color$ Black��6)    F� 	Direction$	 320Q�67    F� ShadowDepth$ 5Q�6H    F� Softness$ 1Q�6X    F� Opacity$	 0.5Q�6e   5      �� 5   
   -   5        $ formatLabel��5      $��Formato:��6%   $��14(�68   $��0��6F   $��1��6V   $��Center=�6c   $��Right=�6~   $��
0,20,10,20q�6�   �� 5!   
   -   5        $ format_amountLabel��5!      $��	C_antidad��6,   $��13(�6@   $��1��6O   $��1��6_   $	��Top=�6l   $
��Left=�6�   	 6�   �� $
 format_amountText���� 5"   
   -   5         ��Name$ format_amountText��5"      ���#212121$��12(�6�   $+ %cantidad del insumo según el formato��6�   $��1��6�   $��1��6�   $��73��6�   $��40��6�   $��80��6   $��25��6  $��24��6  $��Center=�6*  $��Center=�6L  $
��Left=�6g  $��	6,28,0,16q�6�  ��6-   �� *MeasurementAmount+   
amountConv �� 	Converter# [  6�  �� 5#   
   -	   5        $ format_measurementList��5#      $��64��6�   $��25��6�   $��1��6�   $��1��6�   $��Center=�6�   $��Center=�6
  $
��Left=�6%  $��	80,12,0,0q�6@   �� SelectedValue 63   �� *MeasurementUnit+  measurementConv# [ ��6�   �� *Amount.AllUnits+6@  �� 5%   
   -
   5        $ format_costLabel��5%      $��C_osto��6*   $��13(�6;   $��1��6J   $��1��6Z   $	��Top=�6g   $
��Left=�6   $��	170 0 0 0q�6�   	 6�   �� $
 format_costText���� 5&   
   -   5        $ format_costText��5&      $* $Precio del insumo según el formato.��6�   ���#212121$��12(�6�   $��73��6  $��40��6  $��80��6"  $��25��60  $��24��6?  $��1��6L  $��1��6\  $��Center=�6j  $��Center=�6�  $
��Left=�6�  $��
170,12,0,0q�6�  ��6+   �� *
CostAmount+# [  {�6n   �� *
IsTemplate+  inverseConv# [ 6�  �� 5'   
   -   5        $ format_costList��5'      $��10(�6�   $��1��6�   $��1��6  $��64��6  $��25��6  $��Center=�6+  $��Center=�6M  $
��Left=�6h  $��
244,12,0,0q�6�   �� SelectedIndex 6,   �� *
CostUnit+  currencyConv# [ ��6x   �� *Cost.AllUnits+{�6�   �� *
IsTemplate+# [ 6�  �� 5)   
   -   5        $ descriptionLabel��5)      $��_Descripción:��6*   $��14(�6C   $��0��6R   $��2��6b   $	��Top=�6o   $��Right=�6�   $��	0,10,10,0q�6�   	 6�   �� $
 detailsText���� 5*   
   -   5        $ detailsText��5*      $��14(�6E   ���#212121 �� TextWrapping$
 Wrap=�6h   $) #Describa el insumo en esta casilla.��6|   " �� VerticalScrollBarVisibility$
 Auto=�6�   $��1��6�   $��2��6�   $��
0 10 20 10q�6�   ��6'   �� *Description+6�   �� 5.   
   $��0 1 0 0q�6   ���#bbb$��3��6>   $��2��6K   ���#ddd6_   �� 50   
   -   5          ��Name$  cancelButton��50      $��	_Cancelar��6'   $��3��6;   $��1��6H   $��20��6X   $��70��6d   $��Bottom=�6o   $��Right=�6�   $��	0,0,20,10q�6�   �� 51   
   -   5        $  
SaveButton��51      $�� ��6%   $��3��60   $��1��6=   $��20��6M   $��70��6Y   $��Bottom=�6d   $��Right=�6   $��
0,0,100,10q�6�   52      53      