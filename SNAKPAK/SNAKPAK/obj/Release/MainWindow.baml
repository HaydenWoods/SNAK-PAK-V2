   M S B A M L   `   `   `  ���� B  >SNAKPAK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:SNAKPAKSNAKPAK      SNAKPAK.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:SNAKPAK     -     � Title$  SNAKPAK��$	��500��$
��1000��$	��500��$
��1000�� � 	Resources    FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  TransparentStyle �� 
TargetType �� 
TargetType  TabItem.Static.Background �� EndPoint	 �� 
StartPoint
 � Color � Offset  TabItem.Static.Border �� Color   TabItem.MouseOver.Background  TabItem.MouseOver.Border  TabItem.Disabled.Background  TabItem.Disabled.Border  TabItem.Selected.Border 	 TabItem.Selected.Background 
 TabItemStyle1 �Name ��Name J� Opacity ��Name �� RelativeSource �� AncestorType �� Triggers ^� 
Conditions �� Binding �� Value �� 
TargetName��%�  &	        &	 N     &	 �     &	 �     &	 �     &	 P    &	 c    &	 v    &	 �    &		 �    &	
 �    7   7  7  7  7 	 7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  �� .���  � �� �� .R��� $��2q� . #��� ��$��1��$	 1 2R��� " ��.���  � �� �� " ��.R��� 
������ ._��� �� $	 0,1&�$		 0,0&�.��� $
 #F0F0F0��$	 0.0Q�� $
 #E5E5E5��$	 1.0Q��� $ #ACACAC���� $	 0,1&�$		 0,0&�.��� $
 #ECF4FC��$	 0.0Q�� $
 #DCECFC��$	 1.0Q��� $ #7EB4EA���� $ #F0F0F0���� $ #D9D9D9���� $ #ACACAC���� $ #FFFFFF���� " ��.���  �@�FocusVisualStyle8   ��  � ��
 �   ���  � ��8  ��  � ��8  ��  � ��$ 0q���  � ��$ 6,2,6,2q���  � ��$ Stretch=���  � ��$ Stretch=���  � �� �� " ��.R�� $ templateRoot�� . .+��� $ 
mainBorder��#��z��$��1,1,1,0q�#��z��$��0q�._��� $ innerBorder��8�� $��1,1,1,0q�8�� $��-1q�$ 0Q��� $ contentPresenter��$��Header��|�.  #��z����. # z ���� *HorizontalContentAlignment+ �� " ������ *VerticalContentAlignment+ �� " �� ^�  ��  �� *IsMouseOver+ �� *Self+ true��  �� *TabStripPlacement+ �� " �� Left.0��� $ 
mainBorder��8   � ���� $ 
mainBorder��8   � ���� $ innerBorder�� � ��$ 1,1,0,1q��� $ 
mainBorder�� � ��$ 1,1,0,1q�^�  ��  �� *IsMouseOver+ �� *Self+ true��  �� *TabStripPlacement+ �� " ��
 Bottom.0��� $ 
mainBorder��8   � ���� $ 
mainBorder��8   � ���� $ innerBorder�� � ��$ 1,0,1,1q��� $ 
mainBorder�� � ��$ 1,0,1,1q�^�  ��  �� *IsMouseOver+ �� *Self+ true��  �� *TabStripPlacement+ �� " ��	 Right.0��� $ 
mainBorder��8 	  � ���� $ 
mainBorder��8 
  � ���� $ innerBorder�� � ��$ 0,1,1,1q��� $ 
mainBorder�� � ��$ 0,1,1,1q�^�  ��  �� *IsMouseOver+ �� *Self+ true��  �� *TabStripPlacement+ �� " �� Top.0��� $ 
mainBorder��8   � ���� $ 
mainBorder��8   � ���� $ innerBorder�� � ��$ 1,1,1,0q��� $ 
mainBorder�� � ��$ 1,1,1,0q�^�  ��  �� *	IsEnabled+ �� *Self+	 false��  �� *TabStripPlacement+ �� " �� Left.0��� $ contentPresenter�� �@J�Opacity$
 0.56Q��� $ 
mainBorder��8   � ���� $ 
mainBorder��8   � ���� $ innerBorder�� � ��$ 1,1,0,1q��� $ 
mainBorder�� � ��$ 1,1,0,1q�^�  ��  �� *	IsEnabled+ �� *Self+	 false��  �� *TabStripPlacement+ �� " ��
 Bottom.0��� $ contentPresenter�� �@J�Opacity$
 0.56Q��� $ 
mainBorder��8   � ���� $ 
mainBorder��8   � ���� $ innerBorder�� � ��$ 1,0,1,1q��� $ 
mainBorder�� � ��$ 1,0,1,1q�^�  ��  �� *	IsEnabled+ �� *Self+	 false��  �� *TabStripPlacement+ �� " ��	 Right.0��� $ contentPresenter�� �@J�Opacity$
 0.56Q��� $ 
mainBorder��8   � ���� $ 
mainBorder��8   � ���� $ innerBorder�� � ��$ 0,1,1,1q��� $ 
mainBorder�� � ��$ 0,1,1,1q�^�  ��  �� *	IsEnabled+ �� *Self+	 false��  �� *TabStripPlacement+ �� " �� Top.0��� $ contentPresenter�� �@J�Opacity$
 0.56Q��� $ 
mainBorder��8   � ���� $ 
mainBorder��8   � ���� $ innerBorder�� � ��$ 1,1,1,0q��� $ 
mainBorder�� � ��$ 1,1,1,0q�^�  ��  �� *
IsSelected+ �� *Self+	 false��  �� *TabStripPlacement+ �� " �� Left.0��� $ innerBorder�� � ��$ 1,1,0,1q��� $ 
mainBorder�� � ��$ 1,1,0,1q�^�  ��  �� *
IsSelected+ �� *Self+ true��  �� *TabStripPlacement+ �� " �� Left.0���  �@K�ZIndex$ 1����  � ��$ 
-2,-2,0,-2q��� $ innerBorder�� �@J�Opacity$ 1Q��� $ innerBorder�� � ��$ 1,1,0,1q��� $ 
mainBorder�� � ��$ 1,1,0,1q�^�  ��  �� *
IsSelected+ �� *Self+	 false��  �� *TabStripPlacement+ �� " ��
 Bottom.0��� $ innerBorder�� � ��$ 1,0,1,1q��� $ 
mainBorder�� � ��$ 1,0,1,1q�^�  ��  �� *
IsSelected+ �� *Self+ true��  �� *TabStripPlacement+ �� " ��
 Bottom.0���  �@K�ZIndex$ 1����  � ��$ 
-2,0,-2,-2q��� $ innerBorder�� �@J�Opacity$ 1Q��� $ innerBorder�� � ��$ 1,0,1,1q��� $ 
mainBorder�� � ��$ 1,0,1,1q�^�  ��  �� *
IsSelected+ �� *Self+	 false��  �� *TabStripPlacement+ �� " ��	 Right.0��� $ innerBorder�� � ��$ 0,1,1,1q��� $ 
mainBorder�� � ��$ 0,1,1,1q�^�  ��  �� *
IsSelected+ �� *Self+ true��  �� *TabStripPlacement+ �� " ��	 Right.0���  �@K�ZIndex$ 1����  � ��$ 
0,-2,-2,-2q��� $ innerBorder�� �@J�Opacity$ 1Q��� $ innerBorder�� � ��$ 0,1,1,1q��� $ 
mainBorder�� � ��$ 0,1,1,1q�^�  ��  �� *
IsSelected+ �� *Self+	 false��  �� *TabStripPlacement+ �� " �� Top.0��� $ innerBorder�� � ��$ 1,1,1,0q��� $ 
mainBorder�� � ��$ 1,1,1,0q�^�  ��  �� *
IsSelected+ �� *Self+ true��  �� *TabStripPlacement+ �� " �� Top.0���  �@K�ZIndex$ 1����  � ��$ 
-2,-2,-2,0q��� $ innerBorder�� �@J�Opacity$ 1Q��� $ innerBorder�� � ��$ 1,1,1,0q��� $ 
mainBorder�� � ��$ 1,1,1,0q�.��� .+��� 
�������.@��� $��Network��.���  � ColumnDefinitions �� $��7* ��� $��3* �$	��300��$	��600��.+�� -   $��computerGrid��$��0��$��0,0,0,0q�.+��� -   $��computerCanvas�����#c4aee8� -   $��rightBar��$��1�� � RowDefinitions �� $��40 ��� $��* �.+���  �� Padding$ 10,10,10,10q�._��  �� $��50 ��� $��* �.+��� $��Search�� �� Padding$ 0,0,10,0q�$��0���� $��20��$��1���� $ 10,10,10,10q�$��1��._��� -   $��output���� $��Preferences��.���� $ 10q�._��� .+���  �� Orientation$ 
Horizontal=�$��0,0,0,0q�.+��� $��LeftToRight=�$��Snap To Grid��$	��100��$ 0,0,10,0q��� -   $��
SnapToGrid��$��LeftToRight=� d� 	IsChecked$
 TrueY��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Snap Amount��$
��Left=�$	��100��$ 0,0,10,0q��� -   $��
SnapAmount�� � Maximum$ 50Q� � Minimum$ 10Q�$	��200��$��Center=�  �� TickPlacement$  BottomRight=�! �� TickFrequency$! 10Q�" �� IsSnapToTickEnabled" . �� $��Advanced��