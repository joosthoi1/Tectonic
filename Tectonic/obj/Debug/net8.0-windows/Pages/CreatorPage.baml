   M S B A M L   `   `   `  ����G  CPuzzleSolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:PuzzleSolverPuzzleSolver  5      A%clr-namespace:PuzzleSolver.ViewModelsPuzzleSolver.ViewModels  5      A%clr-namespace:PuzzleSolver.ConvertersPuzzleSolver.Converters  5      ;"clr-namespace:PuzzleSolver.HelpersPuzzleSolver.Helpers  5	          PuzzleSolver.CreatorPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:PuzzleSolver     5      <
viewModels%clr-namespace:PuzzleSolver.ViewModels     5      6conv%clr-namespace:PuzzleSolver.Converters     5      6helpers"clr-namespace:PuzzleSolver.Helpers     5	        N� Title$  CreatorPage��5       � 	Resources 5      6   0PuzzleSolver.Converters.BorderThicknessConverter   BorderThicknessConverter7   1PuzzleSolver.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter>   8PuzzleSolver.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter1  +PuzzleSolver.Converters.GroupColorConverter  GroupColorConverter��%t   &	        &	      &	 &     &	 9       5   
   6(     5   
   6)     5   
   60     5   
   6#   5       � DataContext 5      +   %PuzzleSolver.ViewModels.MainViewModel  5   
   .��5      ]� $
��True��6   .+�5   
   �� $	��Top=�6/   $��Center=�6D   ��6   �� *Board.Title+6D   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Bottom=�6.   $��Center=�6F   .+�5      �� $
��Save��6   $��40��6$   $��5q�60   $
��10,0q�6;   ��5      �� *WriteGroupsCommand+5       (� 5#   
    (� Stretch$ Uniform=�6    (� StretchDirection$
 Both=�6$   .��5$      �� ��6   �� *Board.Cells+��5%      �� 5&      .R�5'      A� ,   &PuzzleSolver.Helpers.UniformGridHelper   BoundColumns 6&   �� *DataContext.Board.X+ �� RelativeSource ��  �� AncestorType ItemsControl5(      ��5+      �� 5,      .R�5-      �� -   5        
�������
������ 	 � ToolTip$>	 8Click and drag the cell to copy the group to other cells��5-   M   $��45��51      $��55��52      $��0q�53   !   ��54      `� 55   "   
 `� 	Converter#
 [  6/   .1�56   &   �� �� 57   &    �� Path$ DataContext.Board�6.    6G   ��  Page�� 58   &   $ GroupChanged�6.   59   #   ._�5;      � $
��Auto��6#   $
��Auto��60   $��0q�6>    � RowDefinitions 5<   "   �� 5=   &   $
��Auto �64   �� 5>   &   .+�5B   "   �� ��6/   �� *SmallNumbers+y�5C   +   �� *HasSmallNumbers+ �� 	Converter# [ ��5D   &   �� 5E   *   .R�5F   .   � $��Center=�68   5G   +   ��5I   &   �� 5J   *   .R�5K   .   �� $��10(�6I   $��2q�6W   ��68   �� 6W   5L   +   5N   #   �� 5Q   "   .+�5R   &   �� -   5        $��1��5R   .   $��15(�6V   $
��Bold"�6d   $��20��6v   $��Center=�5S   *   $��Center=�6G   $��1��6b   ��5R   ;   �� *	BigNumber+y�5T   *   �� *HasSmallNumbers+# [ �� 5U   &   -   5        $��1��5U   .   $��10(�6w   $
��Bold"�6�   $��20��6�   $��Center=�5V   *   $��Center=�6G   $��2��6b   ��5U   ;   �� *Group+ �� UpdateSourceTrigger$ PropertyChanged=�y�5W   *   �� *HasSmallNumbers+# [ .��6�   ��5X   *   `� 5Y   .   #
 [ 6;   .1�5Z   2   �� $ DataContext.Board.Colors�6:    6Z   ��  Page�� 5[   2   $ Group�6:   5\   /   .��5]   >   5^   '   5_   #   5`      5a      5b      5d      5e      5f      5g      