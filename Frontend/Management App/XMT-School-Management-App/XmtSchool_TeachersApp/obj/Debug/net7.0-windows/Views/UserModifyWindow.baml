   M S B A M L   `   `   `  ����  XmtSchool_TeachersApp1    +XmtSchool.TeachersAppViews.UserModifyWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localXmtSchoolTypes.Users     5   	   D @ModernWpf, Version=0.9.6.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.6.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   5   	   -   5        0  *ModernWpf.Controls.Primitives.WindowHelper    UseModernWindowStyle$
  True��5   	   $	��400��5
   '   $	��480��64    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title 5
   	   �� *WindowTitle+.��5      )  #ModernWpf.Controls.SimpleStackPanel  $��12q�6      Spacing$ 24Q�6&   .+�5   
   �� -   5        $��TitleTextBox��5      $��
Loading...��6(   $��24(�6:   � 5   
    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $��32 �6    �� 5      $��32 �6     � ColumnDefinitions 5      �� 5      $��7* �6#   �� 5      $��4* �6#   .+�5      �� $��Username��6   $
��Left=�6'   $��5,5,0,0q�6B   $	��Top=�6S   $��0��6k   �� 5       -   5        $��Username_TextBox��5       $��1��6.   $
��Left=�6;   $��3,24,0,0q�6V    �� TextWrapping$ NoWrap=�6h   $	��Top=�6~   $	��149��6�   $��0��6�   �� 5"      $��Password��6   $
��Left=�6'   $��5,1,0,0q�6B   $	��Top=�6S   $��1��6k   G� 5#      -   5        $��Password_TextBox��5#      $��2��62   $
��Left=�6?   $��3,20,0,0q�6Z   $	��Top=�6l   $	��149��6�   $��1��6�   �� 5%      $��E-Mail��6   $
��Left=�6%   $��5,3,0,0q�6@   $	��Top=�6Q   $��2��6i   �� 5&      -   5        $��Email_TextBox��5&      $��3��6+   $
��Left=�68   $��3,24,0,0q�6S   $ NoWrap=�6e   $	��Top=�6{   $	��149��6�   $��2��6�   �� 5(      $��	User Type��6   $
��Left=�6(   $��5,3,0,0q�6C   $	��Top=�6T   $��3��6l   �� 5)      -   5        $��UserType_ComboBox��5)      $��4��6W   $��32��6d   $	��149��6p   $��3,24,0,0q�6|   $��3��6�   $��1��6�   ��60   �� *UserTypesArray+6�   �� 5+      $��2��5,      $��0��5-      $��3��5.      
�������$	��2pxq�50       �� CornerRadius$ 16��51      $	��128��52      $	��128��53      $	��Top=�54      $��0,5,0,0q�55      ��56      �� 57      -   5        Avatar_Image��	 j� Stretch$
	 Fill=�59      5;      �� 5=      $��
Avatar Url��6   $��Center=�6)   $��0,24,0,0q�6F   $	��Top=�6X   $��3��6p   $��2��6�   $��2��6�   �� 5>      -   5        $��AvatarString_TextBox��5>      $��5��6Z   $��Center=�6g   $��0,8,0,0q�6�   $ NoWrap=�6�   $��Center=�6�   $	��149��6�   $��2��6�   $��2��6�   $��2��6�   ��62   �� *SelectedUser.AvatarUrl+6   �� 5@      -	   5        $��Confirm_Button��5@   3   $��6��6   
 �� 	IsDefault$

 True��6"   $��Confirm��6I   $��5��6[   $��70��6h   $��Center=�6s   $��2��6�   $��0,0,85,0q�6�   6�   �� 5A      -
   5        $��Cancel_Button��5A   2   $��7��6    �� IsCancel$
 True��6"   $��Cancel��6G   $��5��6X   $��75��6e   $��Center=�6p   $��2��6�   $��85,0,0,0q�6�   5B      5C      5D      