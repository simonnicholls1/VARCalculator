   M S B A M L   `   `   `  ����H  DVARCalculator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:VARCalculator.ViewModelVARCalculator.ViewModel  5   	       VARCalculator.MainWindow   5      H DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ;	ViewModel%clr-namespace:VARCalculator.ViewModel     5   	     � Title$  VAR Calculator��5   	   $	��350��6    $��650.711��6-   (  "MahApps.Metro.Controls.MetroWindow   EnableDWMDropShadow .  � 
ResizeMode$ NoResize=�5	   	      WindowTransitionsEnabled .   � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��0.2,10.2,-1.2,0.8q�6    � ColumnDefinitions 5   
   .+�5   
   �� $
��Left=�6;   $	��203��6V   $��7,98,0,0q�6c   $	��Top=�6u   $	��629��6�   ��6   �� *VAROutputList+ �� View 5      �� 5      .P�5      ��  �� Width$	 100��6%   $��Run Date��61    �� DisplayMemberBinding 6C   �� 	 �� Path$	 RunDate�
 �� StringFormat$
 d���� 5      $ 80��6%   $��Count��60    6?   �� $	 InstrumentCount��� 5      $ 90��6%   $��
Start Date��60    6D   �� $	 	StartDate�$
 d���� 5      $ 90��6%   $��End Date��60    6B   �� $	 EndDate�$
 d���� 5      $ 90��6%   $��
Conf Level��60    6D   �� $	 	ConfLevel��� 5      $ 90��6%   $��Port Val��60    6B   �� $	 PortfolioValue��� 5      $	 150��6%   $	��VAR��61    6>   �� $		 VAR�5      5      �� 5   
   $
��Left=�6   $��415,52,0,218q�6+   (  "MahApps.Metro.Controls.SplitButton  5    
   $��Right=�5!   	   $��23��6%   $��40��61   $��
0,52,537,0q�6<    �� DisplayMemberPath$ 	ConfLevel��5$   	   $	��Top=�5%   	    �� SelectedIndex 5"   	   �� *SelectedConfLevel+��5#   	   �� *ConfLevelList+5%   	   �� 5&   
   $��Confidence:��6   $
��Left=�6&   $��	-2,52,0,0q�6A   $	��Top=�6T   �� 5'   
   $��	Value($):��6   $
��Left=�6$   $��
235,52,0,0q�6?   $	��Top=�6S   �� 5(   
   $��	End Date:��6   $
��Left=�6$   $��
202,10,0,0q�6?   $	��Top=�6S   �� 5)   
   $��Start Date:��6   $
��Left=�6&   $��	-1,10,0,0q�6A   $	��Top=�6T   (  "System.Windows.Controls.DatePicker  5*   
   -   5        $��startDatePicker��5*   O   $��25��6   $
��Left=�6!   $��	68,11,0,0q�6<   $	��Top=�6f   $	��115��5+   
      SelectedDate 6   �� *	StartDate+ �� Mode$ TwoWay=�   DisplayDate 6F   �� *DateTime.Now+  5,   
   -   5          Name$ endDatePicker��5,   P   $��25��6   $
��Left=�6!   $��
263,10,0,0q�6<   $	��Top=�6g   $	��115��5-   
    6   �� *	EndDate+$ TwoWay=� 6D   �� *DateTime.Now+�� 5.   
   $��
Clear Grid��6   
�������$��400,52,157,232q�6`   ��6=   �� *ClearGridClick+6`   �� 5/   
   
�������$��503,52,47,232q�6q   ��6   �� *CalculateLabel+��6K   �� *VARCalculateClick+6q   �� 50   
   $
��Left=�6   $��23��6-   $��
294,52,0,0q�69    �� TextWrapping$
 Wrap=�6r   $	��Top=�6�   $��85��6�   ��6M   �� $	 PortfolioValue�6�   � 51   
   $
��Left=�6   $��	404,6,0,0q�61   $	��Top=�6D   $	��195��6\   $��21��6h    � Value 6u   �� *ProgressNumber+$ OneWay=�y�6�   �� *ProgressVisibility+�� 52   
   $
��Left=�62   $��
398,26,0,0q�6M   $	��Top=�6a   ��6   �� *CurrentStatus+6a     53   
   $��Right=�54   
   $��23��6&   $��40��62   $��
0,52,417,0q�6=   $ NoInstruments��57   
   $	��Top=�58   
    55   
   �� *SelectedNoInstruments+��56   
   �� *NoInstrumentsList+58   
   �� 59   
   $��Instruments:��6   $
��Left=�6'   $��
118,52,0,0q�6B   $	��Top=�6V   $��75��6n   5;      5=      