   M S B A M L   `   `   `  ����   XmtSchool_TeachersApp1    +XmtSchool.TeachersAppViews.UserModifyWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localXmtSchoolTypes.Users     D @ModernWpf, Version=0.9.6.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.6.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   -   0  *ModernWpf.Controls.Primitives.WindowHelper    UseModernWindowStyle$
  True��$	��400��$	��480�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=� � Title �� *WindowTitle+.��)  #ModernWpf.Controls.SimpleStackPanel  $��12q�   Spacing$ 24Q�.+��� -   $��TitleTextBox��$��
Loading...��$��24(��  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��32 ��� $��32 � � ColumnDefinitions �� $��7* ��� $��4* �.+��� $��Username��$
��Left=�$��5,5,0,0q�$	��Top=�$��0���� -   $��Username_TextBox��$��1��$
��Left=�$��3,24,0,0q� �� TextWrapping$ NoWrap=�$	��Top=�$	��149��$��0���� $��Password��$
��Left=�$��5,1,0,0q�$	��Top=�$��1��G� -   $��Password_TextBox��$��2��$
��Left=�$��3,20,0,0q�$	��Top=�$	��149��$��1���� $��E-Mail��$
��Left=�$��5,3,0,0q�$	��Top=�$��2���� -   $��Email_TextBox��$��3��$
��Left=�$��3,24,0,0q�$ NoWrap=�$	��Top=�$	��149��$��2���� $��	User Type��$
��Left=�$��5,3,0,0q�$	��Top=�$��3���� -   $��UserType_ComboBox��$��4��$��32��$	��149��$��3,24,0,0q�$��3��$��1������ *UserTypesArray+�� $��2��$��0��$��3��
�������$	��2pxq� �� CornerRadius$ 16��$	��128��$	��128��$	��Top=�$��0,5,0,0q����� -   Avatar_Image��	 j� Stretch$
	 Fill=��� $��
Avatar Url��$��Center=�$��0,24,0,0q�$	��Top=�$��3��$��2��$��2���� -   $��AvatarString_TextBox��$��5��$��Center=�$��0,8,0,0q�$ NoWrap=�$��Center=�$	��149��$��2��$��2��$��2������ *SelectedUser.AvatarUrl+�� -	   $��Confirm_Button��$��6��
 �� 	IsDefault$

 True��$��Confirm��$��5��$��70��$��Center=�$��2��$��0,0,85,0q��� -
   $��Cancel_Button��$��7�� �� IsCancel$
 True��$��Cancel��$��5��$��75��$��Center=�$��2��$��85,0,0,0q�