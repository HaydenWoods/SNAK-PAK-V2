   M S B A M L   `   `   `  ����B  >SNAKPAK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:SNAKPAKSNAKPAK  5   	       SNAKPAK.Preferences   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:SNAKPAK     5   	     � Title$  Preferences��5   	   $	��450��6   $	��800��6*    � 	Resources 5	         LeftBarButtons �� 
TargetType �� Property �� Value �� 
TargetType J� SnapsToDevicePixels��%�  &	        �� 5
   
   " ��6'   .�5      ��  � ��
 �   �6+   �� 5       � ��$ 16(�6)   �� 5       � ��
 ���� 6+   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��5      #��z��5      #��z��5      # z 5      #��z��5      ��. #��z��5      5      5      5      5      .��5       �  � ColumnDefinitions 5!   
   �� 5"      $	��200 �6   6-   �� 5#      $��* �6   6+   .+�5&   
   ��  �� Padding$ 0q�6   $��0��6   ._�5'      �� .+�5(      �� $��General��6   $��50��6+   #��[  67   6a   �� 5)      $��Advanced��6   $��50��6,   #��[  68   6b   �� 5*      $��Commands��6   $��50��6,   #��[  68   6b   5+      5,      �� 5.   
   $ 10q�6   $��1��6   ._�5/      �� .+�50      �� 	 �� Orientation$	 
Horizontal=�6   $��0,0,0,0q�66   .+�51      �� $��LeftToRight=�6    $��Snap To Grid��6<   $	��100��6P   
 �� Padding$
 0,0,10,0q�6]   �� 52      -   5        $��
SnapToGrid��52      $��LeftToRight=�61    d� 	IsChecked$
 TrueY�6M   53      �� 55      $	 
Horizontal=�6   $��0,10,0,0q�66   .+�56      �� $��Snap Amount��6    $
��Left=�63   $	��100��6N   $
 0,0,10,0q�6Z   �� 57      -   5        $��
SnapAmount��57       � Maximum$ 50Q�6/    � Minimum$ 10Q�6<   $	��200��6I   $��Center=�6U    �� TickPlacement$ BottomRight=�6p    �� TickFrequency$ 10Q�6�    �� IsSnapToTickEnabled . 6�   58      59      5:      5;      5<      