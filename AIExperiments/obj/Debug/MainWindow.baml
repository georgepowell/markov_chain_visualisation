   M S B A M L   `   `   `  ����  AIExperiments    AIExperiments.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$  AI experiments for London��5      $	��570��5   	   $	��840��5   	    � 
ResizeMode$ NoResize=�5   	   .��5      �  � RowDefinitions 5	   
   �� 5
      $
��522* �6   �� 5      $	��19* �6    � ColumnDefinitions 5   
   �� 5      $	��200 �6   �� 5      $��1* �6   .+�5   
   �� -   5         ��Name$ canvas��5      $��1��5      ���	#11000000$��2��5   (   �� 5   
   -   5         ��Name$ patternCombo��5      $
��Left=�6)   $��	10,10,0,0q�6D   $	��Top=�6W   $	��180��6o   .@�5      �� 
 � Tag$ R��6   .��6#   Random6+   �� 5      $ R��6   .��6#   
Sequential6/   �� 5      $ R��6    �� 
IsSelected . .��65   Bible6<   �� 5      $ R��6   .��6#   Shakespeare60   �� 5      $ R��6   .��6#   HTML6)   5      �� 5   
   -   5         ��Name$ checkboxTrim��5      $��trim connections��6m   $
��Left=�6�   $��	10,37,0,0q�6�   $	��Top=�6�   $	��180��6�   �� 5    
   -   5        	 ��Name$	 btnStartStop��5       $��Start��6B   $
��Left=�6R   $��
10,474,0,0q�6m   $	��Top=�6�   $��48��6�   $��38��6�   
 J� RenderTransformOrigin$
 
0.6,-0.826&�6�   �� 5"   
   -   5        $	 btnStep��5"      $
��Step��68   $
��Left=�6G   $��
76,474,0,0q�6b   $	��Top=�6v   $��46��6�   $��38��6�   �� 5$   
   -   5        $	 btnReset��5$      $��Reset��6:   $
��Left=�6J   $��144,474,0,0q�6e   $	��Top=�6z   $��46��6�   $��38��6�   �� 5&   
   -   5         ��Name$ txtSoFar��5&      $��0��6&   $��2��66   $��Right=�6J   $��1��6f   $��Center=�6s   $��NoWrap=�6�   $��0,2,0,1q�6�   5(      5)      