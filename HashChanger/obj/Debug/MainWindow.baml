   M S B A M L   `   `   `  ����F  BHashChanger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:HashChangerHashChanger  5   	       HashChanger.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:HashChanger     5   	     � Title$  
MainWindow��5   	   $	��650��6   $	��500��6)   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $��5* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5        $��0��5      
�������$��1q�61    �� CornerRadius$ 5��6E    �� Padding$ 10q�6V   ���#F0F0F0 J� 	AllowDrop . ._�5      �� $��Drag Drop a File Here��6   $��Center=�65   $��Center=�6R   5      �� 5   
   -   5        $��FilePathTextBox��5      $��1��6    �� 
IsReadOnly . $��5q�6H   � 5    
   -   5        $��ProgressBar��5    #   $��2��6   $��20��66   $��5q�6B   �� 5#   
   -   5        $��3��5#      $��Modify Hash��6   $��5q�6M   �� 5&   
   $��4��6   $��Original Hash:��6!   $��5q�67   �� 5'   
   -   5        $��OriginalHashTextBox��5'      $��5��6    . $��5q�6L   �� 5(   
   $��6��6   $��Modified Hash:��6!   $��5q�67   �� 5)   
   -   5        $��ModifiedHashTextBox��5)      $��7��6    . $��5q�6L   �� 5-   
   -   5        $��StatusLabel��5-      $��8��6   $��Ready��60   $��Center=�6@   $��5q�6]   5.      5/      