   M S B A M L   `   `   `  ����F  BTextEncrypt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TextEncryptTextEncrypt  5   	       TextEncrypt.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:TextEncrypt     5   	     � Title$  TextEncrypt��5   	   $	��450��6   $	��800��6*   
���iii� � 
ResizeMode$ NoResize=�5	      .��5   
   � ��5   
   �� 5       �� ImageSource$ 
app_bg.jpg��6    j� Stretch$ UniformToFill=�62    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   .+�5"   
   �� -   5         ��Name$ encrypt_txtbox��5"      
���   �
�������$��1��6P   $��1��6`   $��4��6m   $��4��6�   
������� �� 	MaxLength$	 300��6�   $��0,0,0,25q�6�   6�   �� 5#   
   -   5         ��Name$ status��5#      
���   �
�������$��25��6I   $��4��6U   $��0,45,0,0q�6b   $��4��6t   $$��300 Макс. символов��6�   $��13(�6�   $��1��6�   �� 5$   
   -   5        	 ��Name$
	 time��5$      
���   �
����� �$��25��6J   $��0��6V   $�� ��6d   $��13(�6o   $��0,45,0,0q�6}   $��4��6�   $��1��6�   6�   �� 5%   
   -   5        
 ��Name$
 encrypt_button��5%      $��1��6)   $��5��69   $��35��6F   $�� ��6R   6p   �� 5&   
   -   5        $
 decrypt_button��5&      $��3��6)   $��5��69   $��35��6F   $�� ��6R   6_   5'      5(      