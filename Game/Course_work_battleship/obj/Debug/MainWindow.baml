   M S B A M L   `   `   `  ����Q  MCourse_work_battleship, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Course_work_gameCourse_work_game  5   	   !    Course_work_game.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Course_work_game     5   	   -   5          � Title$&   Собери свой салат��5   	   $	��480��6#   $	��580��60   .��5	      � -   5         �Name$ gridMain��5	      .+�5
   
   �� -   5         ��Name$ lableWelcome��5
      $2��,Добро пожаловать в игру!��6&   $��Center=�6I   $��128,29,123.333,0q�6f   $	��Top=�6�   $��Bodoni Bd BT)�6�   $��18(�6�   $	��242��6�   ���	#FF3A8F49 J� RenderTransformOrigin$ 0.969,0.625&�6�   �� 5   
   -   5        $ lblName��5      $&�� Введите Ваше имя: ��6!   $��Center=�6>   $��168,65,176.333,0q�6[   $	��Top=�6u   $��Bodoni Bd BT)�6�   $��16(�6�   $	��149��6�   ���	#FF30448F6�   �� 5   
   -   5         ��Name$ textboxName��5      $��Center=�6'   $��37��6D   $��153,101,154.333,0q�6P    �� TextWrapping$
 Wrap=�6k   $	��Top=�6   $	��186��6�   $��Bodoni Bk BT)�6�   $��16(�6�   ���	#FF432B6D
������ $ 0.5,0.5&�6�   .��6  z�5      V� 5      .~�5      �� �� 5       �� AngleX$ 0.156Q�6$   �� 5      T� 5       T� X$
 0.33Q�6)   5      .��5   '   5      �� 5   
   -   5         ��Name$ buttonStart��5      $��Начать игру��6&   $��182,227,184.333,177.667q�6<   $��18(�6]   $��Bodoni Bd BT)�6k   ���	#FFCDEDD2���	#FFB4E4C56�   �� 5   
   -   5        $ 	textboxIP��5      $��Center=�6%   $��37��6B   $��153,177,154.333,0q�6N   $
 Wrap=�6i   $	��Top=�6}   $	��186��6�   $��Bodoni Bk BT)�6�   $��16(�6�   ���	#FF432B6D
������ $ 0.5,0.5&�6�   .��6  z�5      V� 5      .~�5      �� �� 5      $ 0.156Q�6$   �� 5      T� 5      $
 0.33Q�6)   5      .��5   '   5       �� 5!   
   -   5        $ lableIP��5!      $��IP сервера: ��6!   $��Center=�68   $��199,141,184.333,0q�6U   $	��Top=�6p   $��Bodoni Bd BT)�6�   $��16(�6�   $	��110��6�   ���	#FF30448F6�   �� 5"   
   -	   5        	 ��Name$
	 Img3��5"      $��Recources/apple.png��6   
 �� Left$	
 142��6;   
 �� Top$	 -78��6M   $��54��6^   $��52��6j   $��15,129,426.333,137.667q�6u   $ 0.5,0.5&�6�   z�5#      V� 5$      .~�5%      �� �� 5&      �� 5'       �� Angle$ -28.408Q�6&   T� 5(      5)      5+      �� 5,   
   -
   5        $
	 Img4��5,      $��Recources/carrot.png��6   $
 -8��6<   $	 -17��6M   $��42��6^   $��66��6j   $ 0.5,0.5&�6u   $��40,16,387.333,262.667q�6�   z�5-      V� 5.      .~�5/      �� �� 50      �� 51      $ -103.075Q�6&   T� 52      53      55      �� 56   
   -   5        $
	 Img6��56      $��Recources/carrot.png��6   $	
 114��6<   $	 -21��6N   $��42��6_   $��66��6k   $ 0.5,0.5&�6v   $��401,239,26.333,39.667q�6�   z�57      V� 58      .~�59      �� �� 5:      �� 5;      $ -47.139Q�6&   T� 5<      5=      5?      �� 5@   
   -   5        $
	 Img7��5@      $��Recources/carrot.png��6   $	
 199��6<   $	 -21��6N   $��42��6_   $��66��6k   $ 0.5,0.5&�6v   $��25,244,402.333,34.667q�6�   z�5A      V� 5B      .~�5C      �� �� 5D      �� 5E      $ -107.102Q�6&   T� 5F      5G      5I      �� 5J   
   -   5        $	 Img12��5J      $��Recources/strawberry.png��6   $ 40��6A   $��34��6Q   $��35��6]   $	
 176��6h   $ 0.5,0.5&�6z   $��382,190,76.333,96.667q�6�   z�5K      V� 5L      .~�5M      �� �� 5N      �� 5O      $ -83.471Q�6&   T� 5P      5Q      5S      �� 5T   
   -   5        $	 Img13��5T      $��Recources/strawberry.png��6   $ 42��6A   $��34��6Q   $��35��6]   $	
 138��6h   $ 0.5,0.5&�6z   $��389,85,69.333,201.667q�6�   z�5U      V� 5V      .~�5W      �� �� 5X      �� 5Y      $ -87.318Q�6&   T� 5Z      5[      5]      �� 5^   
   -   5        $	 Img14��5^      $��Recources/strawberry.png��6   $ 40��6A   $��34��6Q   $��35��6]   $
 92��6h   $ 0.5,0.5&�6y   $��65,185,393.333,101.667q�6�   z�5_      V� 5`      .~�5a      �� �� 5b      �� 5c      $ -24.694Q�6&   T� 5d      5e      5g      �� 5h   
   -   5        $	 Img15��5h      $��Recources/strawberry.png��6   $ 40��6A   $��34��6Q   $��35��6]   $
 56��6h   $ 0.5,0.5&�6y   $��46,80,412.333,206.667q�6�   z�5i      V� 5j      .~�5k      �� �� 5l      �� 5m      $ -9.586Q�6&   T� 5n      5o      5q      �� 5s   
   -   5        $	 Img16��5s      $��Recources/plum.png��6   $
 26��6;   $ 97��6L   $��58��6\   $��70��6h   $��408,125,15.333,137.667q�6s   $ 0.5,0.5&�6�   z�5t      V� 5u      .~�5v      �� �� 5w      �� 5x      $ 153.456Q�6&   T� 5y      5z      5|      �� 5}   
   -   5        $	 	Img6_Copy��5}      $��Recources/carrot.png��6#   $��42��6A   $��66��6M   $ 0.5,0.5&�6X   $��405,15,22.333,263.667q�6x   z�5~      V� 5      .~�5�      �� �� 5�      �� 5�      $ -38.529Q�6&   T� 5�      5�      5�      5�      5�      