   M S B A M L   `   `   `  ����G  CPuzzleSolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:PuzzleSolverPuzzleSolver  5      A%clr-namespace:PuzzleSolver.ViewModelsPuzzleSolver.ViewModels  5      A%clr-namespace:PuzzleSolver.ConvertersPuzzleSolver.Converters  5      ;"clr-namespace:PuzzleSolver.HelpersPuzzleSolver.Helpers  5	          PuzzleSolver.SolverPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:PuzzleSolver     5      <
viewModels%clr-namespace:PuzzleSolver.ViewModels     5      6conv%clr-namespace:PuzzleSolver.Converters     5      6helpers"clr-namespace:PuzzleSolver.Helpers     5	        N� Title$  
SolverPage��5       � 	Resources 5      6   0PuzzleSolver.Converters.BorderThicknessConverter   BorderThicknessConverter7   1PuzzleSolver.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter>   8PuzzleSolver.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter1   +PuzzleSolver.Converters.FullBorderConverter  FullBorderConverter��%t   &	        &	      &	 &     &	 9       5   
   6(     5   
   6)     5   
   60     5   
   6#   5       � DataContext 5      +   %PuzzleSolver.ViewModels.MainViewModel  5   
   .��5      ]� $
��True��6   .+�5   
   �� $	��Top=�6   $��Center=�6*   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��5q�6O   .+�5      �� $	��←��6   $��5q�6N   $��16(�6Y   ��6%   �� *PreviousStateCommand+6Y   �� 5      $	��→��6   $��5q�6J   $��16(�6U   ��6%   �� *NextStateCommand+6U   5      �� 5      $��Center=�65   ��6   �� *Board.Title+65   5       �� 5#   
   $ 
Horizontal=�6   $��Bottom=�6.   $��Center=�6F   .+�5$      �� $��	Next Step��6   $��40��6)   $��5q�65   $
��10,0q�6@   ��5%      �� *CheckNextCommand+�� 5&      $��Until complete��6   $��40��6.   $��5q�6:   $
��10,0q�6E   ��5'      �� *CheckAllCommand+5(      (� 5+   
    (� Stretch$ Uniform=�6    (� StretchDirection$
 Both=�6$   .��5,      �� ��6   �� *Board.Cells+��5-      �� 5.      .R�5/      A� ,   &PuzzleSolver.Helpers.UniformGridHelper   BoundColumns 6&   �� *DataContext.Board.X+ �� RelativeSource ��  �� AncestorType ItemsControl50      ��53      �� 54      .R�55      � 
������ $��0q�68   $��55��6C   $��45��6N   .+�59      �� ��5:   "   `� 5;   &   	 `� 	Converter#	 [ 63   
 `� ConverterParameter$
 LeftThickness��6d   .1�5<   *   ��  �� Path$ .�62   �� 5=   *   $ DataContext.Board�62    6K   ��  ItemsControl5>   '   ��5@   "   `� 5A   &   #	 [ 63   $
 	LeftBrush��6d   .1�5B   *   �� $ .�62   �� 5C   *   $ DataContext.Board�62    6K   ��  ItemsControl5D   '   5F      �� 5I      ��5J   "   `� 5K   &   #	 [ 63   $
 TopThickness��6d   .1�5L   *   �� $ .�62   �� 5M   *   $ DataContext.Board�62    6K   ��  ItemsControl5N   '   ��5P   "   `� 5Q   &   #	 [ 63   $
 TopBrush��6d   .1�5R   *   �� $ .�62   �� 5S   *   $ DataContext.Board�62    6K   ��  ItemsControl5T   '   5V      �� 5Y      ��5Z   "   `� 5[   &   #	 [ 63   $
 RightThickness��6d   .1�5\   *   �� $ .�62   �� 5]   *   $ DataContext.Board�62    6K   ��  ItemsControl5^   '   ��5`   "   `� 5a   &   #	 [ 63   $
 
RightBrush��6d   .1�5b   *   �� $ .�62   �� 5c   *   $ DataContext.Board�62    6K   ��  ItemsControl5d   '   5f      �� 5i      ��5j   "   `� 5k   &   #	 [ 63   $
 BottomThickness��6d   .1�5l   *   �� $ .�62   �� 5m   *   $ DataContext.Board�62    6K   ��  ItemsControl5n   '   ��5p   "   `� 5q   &   #	 [ 63   $
 BottomBrush��6d   .1�5r   *   �� $ .�62   �� 5s   *   $ DataContext.Board�62    6K   ��  ItemsControl5t   '   5v      � 5y      $
��Auto��6#   $
��Auto��60   $��0q�6>    � RowDefinitions 5{   "   �� 5|   &   $
��Auto �64   �� 5}   &   .+�5�   "   �� ��6/   �� *SmallNumbers+y�5�   /   �� *HasSmallNumbers+ �� 	Converter# [ ��5�   &   �� 5�   *   .R�5�   .   � $��Center=�68   5�   +   ��5�   &   �� 5�   *   .R�5�   .   �� $��10(�6I   $��2q�6W   ��68   �� 6W   5�   +   5�   #   �� 5�   "   $��1��6,   $��28(�6T   $
��Bold"�6b   $��Center=�5�   ,   $��Center=�6I   ��5�   9   �� *	BigNumber+y�5�   ,   �� *HasSmallNumbers+# [ 5�      5�      5�      5�      5�      5�      5�      