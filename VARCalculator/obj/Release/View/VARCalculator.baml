   M S B A M L   `   `   `  ���� H  DVARCalculator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:VARCalculator.ViewModelVARCalculator.ViewModel      VARCalculator.MainWindow   H DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ;	ViewModel%clr-namespace:VARCalculator.ViewModel       � Title$  VAR Calculator��$	��350��$��650.711��(  "MahApps.Metro.Controls.MetroWindow   EnableDWMDropShadow .  � 
ResizeMode$ NoResize=�   WindowTransitionsEnabled .   � WindowStartupLocation$ CenterScreen=�.��� $��0.2,10.2,-1.2,0.8q� � ColumnDefinitions .+��� $
��Left=�$	��203��$��7,98,0,0q�$	��Top=�$	��629������ *VAROutputList+ �� View �� .P���  �� Width$	 100��$��Run Date�� �� DisplayMemberBinding �� 	 �� Path$	 RunDate�
 �� StringFormat$
 d���� $ 80��$��Count�� �� $	 InstrumentCount��� $ 90��$��
Start Date�� �� $	 	StartDate�$
 d���� $ 90��$��End Date�� �� $	 EndDate�$
 d���� $ 90��$��
Conf Level�� �� $	 	ConfLevel��� $ 90��$��Port Val�� �� $	 PortfolioValue��� $	 150��$	��VAR�� �� $		 VAR��� $
��Left=�$��415,52,0,218q�(  "MahApps.Metro.Controls.SplitButton  $��Right=�$��23��$��40��$��
0,52,537,0q� �� DisplayMemberPath$ 	ConfLevel��$	��Top=� �� SelectedIndex �� *SelectedConfLevel+���� *ConfLevelList+�� $��Confidence:��$
��Left=�$��	-2,52,0,0q�$	��Top=��� $��	Value($):��$
��Left=�$��
235,52,0,0q�$	��Top=��� $��	End Date:��$
��Left=�$��
202,10,0,0q�$	��Top=��� $��Start Date:��$
��Left=�$��	-1,10,0,0q�$	��Top=�(  "System.Windows.Controls.DatePicker  -   $��startDatePicker��$��25��$
��Left=�$��	68,11,0,0q�$	��Top=�$	��115��   SelectedDate �� *	StartDate+ �� Mode$ TwoWay=�   DisplayDate �� *DateTime.Now+  -     Name$ endDatePicker��$��25��$
��Left=�$��
263,10,0,0q�$	��Top=�$	��115�� �� *	EndDate+$ TwoWay=� �� *DateTime.Now+�� $��
Clear Grid��
�������$��400,52,157,232q����� *ClearGridClick+�� 
�������$��503,52,47,232q����� *CalculateLabel+���� *VARCalculateClick+�� $
��Left=�$��23��$��
294,52,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��85������ $	 PortfolioValue�� $
��Left=�$��	404,6,0,0q�$	��Top=�$	��195��$��21�� � Value �� *ProgressNumber+$ OneWay=�y��� *ProgressVisibility+�� $
��Left=�$��
398,26,0,0q�$	��Top=����� *CurrentStatus+  $��Right=�$��23��$��40��$��
0,52,417,0q�$ NoInstruments��$	��Top=� �� *SelectedNoInstruments+���� *NoInstrumentsList+�� $��Instruments:��$
��Left=�$��
118,52,0,0q�$	��Top=�$��75��