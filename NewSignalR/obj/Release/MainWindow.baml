   M S B A M L   `   `   `  ���� E  ANewSignalR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:NewSignalR
NewSignalR      NewSignalR.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:NewSignalR       � SizeToContent$  WidthAndHeight=� � Title$  RTLS Application for CPALS��$	��550��$	��910��.��� $��0,0,1,1q� � RowDefinitions �� $	��110 ��� $	��400 �.+�� $��0��$��0,0,0,2.857q�.+��� $��Server��$
��Left=�$��10,10,803,0q�$	��Top=�$��87��$��26���� $��UserName��$
��Left=�$��10,41,803,0q�$	��Top=�$��87��$��26���� $��UserPassword��$
��Left=�$��10,72,803,0q�$	��Top=�$��87��$��26���� -    ��Name$ 	txtServer��$
��Left=�$��26��$��102,10,485,0q� �� TextWrapping$
 Wrap=�$��TextBox��$	��Top=�$	��313���� -   $ txtUserName��$
��Left=�$��26��$��102,41,485,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��313���� -   $ txtPassword��$
��Left=�$��26��$��102,72,485,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��313���� -    ��Name$ btnCheck��$��CheckID��$
��Left=�$��
434,10,0,0q�$	��Top=�$��70��$��88���� $��Tag ID��$
��Left=�$��	534,6,0,0q�$	��Top=�$��55��$��26���� -   $ txtTagID��$
��Left=�$��88��$��
589,10,0,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��114���� -   $ 	btnDebbug��$��Debbug��$
��Left=�$��
800,10,0,0q�$	��Top=�$��80��$��26��� $��1��$��0,5,0,0q�.+��� .@��� $��Position��.��� .+��� $��ObjectID��$
��Left=�$��	10,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� $��Position��$
��Left=�$��
105,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� $)��#Movement distinction velocity (m/s)��$
��Left=�$��	194,7,0,0q�$	��Top=�$	��201��$��26��$��Italic$��� -   $ txtMovementVelocity��$
��Left=�$��26��$��
400,10,0,0q�$	��Top=�$��65��$	��1.0���� -   $ btnStart��$��Start��$
��Left=�$��
499,10,0,0q�$	��Top=�$��80�� J� RenderTransformOrigin$ -0.162,0.264&�$��26���� -	   $ btnStop��$
��Stop��$
��Left=�$��
600,10,0,0q�$	��Top=�$��80��$��26���� -
   $ btnSave��$
��Save��$
��Left=�$��
701,10,0,0q�$	��Top=�$��80��$��26���� -   $ 
BtnRefresh��$��Refresh��$
��Left=�$��
800,10,0,0q�$	��Top=�$��80��$��26���� -    ��Name$ 	cmbTagID1��$
��Left=�$��	10,80,0,0q�$	��Top=�$��87��$��22���� -   $ 	cmbTagID2��$
��Left=�$��
10,174,0,0q�$	��Top=�$��87��$��22���� -   $ 	cmbTagID3��$
��Left=�$��
10,268,0,0q�$	��Top=�$��87��$��22���� -    ��Name$ lbxPosition1��   NewSignalR.ListBoxBehavior	   ScrollOnNewItem	 . $��Visible=�$
��Left=�$��89��$��
105,81,0,0q�$	��Top=�$	��775������ *positionList1+���� .R�� 
 � ColumnDefinitions
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2�� �� ContentStringFormat$ yyyy-MM-dd HH:mm:ss��$��30������ *	Timestamp+�� $��3��$��50������ *X+�� $��4��$��50������ *Y+�� $��5��$��30������ *Zone+�� $��6��$��50������ *
ZoneName+�� $��7��$��80������ *	Longitude+�� $��8��$��80������ *
Latitude+�� -   $ lbxPosition2��	 . $��Visible=�$
��Left=�$��89��$��105,175,0,0q�$	��Top=�$	��775������ *positionList2+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	Timestamp+�� $��3��$��50������ *X+�� $��4��$��50������ *Y+�� $��5��$��30������ *Zone+�� $��6��$��50������ *
ZoneName+�� $��7��$��80������ *	Longitude+�� $��8��$��80������ *
Latitude+�� -   $ lbxPosition3��	 . $��Visible=�$
��Left=�$��89��$��105,268,0,0q�$	��Top=�$	��775������ *positionList3+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	Timestamp+�� $��3��$��50������ *X+�� $��4��$��50������ *Y+�� $��5��$��30������ *Zone+�� $��6��$��50������ *
ZoneName+�� $��7��$��80������ *	Longitude+�� $��8��$��80������ *
Latitude+�� $��Real-time Movement��.��� .+��� $��ObjectID��$
��Left=�$��	10,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� $'��!Distance, Velocity, Movement Type��$
��Left=�$��
105,47,0,0q�$	��Top=�$	��220��$��26��$
��Bold"��� $��Movement��$
��Left=�$��
499,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� -    ��Name$ lbObjectID1��$
��Left=�$��5,81,0,0q�$	��Top=�$��87��$��26��$��Center=��� -   $ lbObjectID2��$
��Left=�$��	5,175,0,0q�$	��Top=�$��87��$��26��$��Center=��� -   $ lbObjectID3��$
��Left=�$��	5,268,0,0q�$	��Top=�$��87��$��26��$��Center=��� -   $ btnStartInMovement��$��Start��$
��Left=�$��
499,10,0,0q�$	��Top=�$��80��$ -0.162,0.264&�$��26���� -   $ btnStopInMovement��$
��Stop��$
��Left=�$��
600,10,0,0q�$	��Top=�$��80��$��26���� -   $ btnSaveInMovement��$
��Save��$
��Left=�$��
701,10,0,0q�$	��Top=�$��80��$��26���� -   $ BtnRefreshInMovement��$��Refresh��$
��Left=�$��
800,10,0,0q�$	��Top=�$��80��$��26���� -   $ lbxMovementType1��$
��Left=�	 . $��Visible=�$��89��$��
105,81,0,0q�$	��Top=�$	��381������ *movementType1+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$��50������ *Zone+�� $��3��$��30������ *
ZoneName+�� $��4��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��5��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��6��$��30������ *MovementTime+�� $��7��$��30������ *
Distance+�� $��8��$��30������ *
Velocity+�� $��9��$��30������ *Type+�� -   $ lbxMovementType2��$
��Left=�	 . $��Visible=�$��89��$��105,175,0,0q�$	��Top=�$	��381������ *movementType2+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$��50������ *Zone+�� $��3��$��30������ *
ZoneName+�� $��4��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��5��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��6��$��30������ *MovementTime+�� $��7��$��30������ *
Distance+�� $��8��$��30������ *
Velocity+�� $��9��$��30������ *Type+�� -   $ lbxMovementType3��$
��Left=�	 . $��Visible=�$��89��$��105,268,0,0q�$	��Top=�$	��381������ *movementType3+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$��50������ *Zone+�� $��3��$��30������ *
ZoneName+�� $��4��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��5��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��6��$��30������ *MovementTime+�� $��7��$��30������ *
Distance+�� $��8��$��30������ *
Velocity+�� $��9��$��30������ *Type+�� -   $ lbxMovement1��	 . $��Visible=�$
��Left=�$��89��$��
499,81,0,0q�$	��Top=�$	��381������ *movementList1+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��3��$��50������ *Type+�� $��4��$��30������ *Zone+�� $��5��$��30������ *
Distance+�� -   $ lbxMovement2��	 . $��Visible=�$
��Left=�$��89��$��499,175,0,0q�$	��Top=�$	��381������ *movementList2+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��3��$��50������ *Type+�� $��4��$��30������ *Zone+�� $��5��$��30������ *
Distance+�� -   $ lbxMovement3��	 . $��Visible=�$
��Left=�$��89��$��499,268,0,0q�$	��Top=�$	��381������ *movementList3+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��3��$��50������ *Type+�� $��4��$��30������ *Zone+�� $��5��$��30������ *
Distance+�� $��Redefined Movement��.��� .+��� $��ObjectID��$
��Left=�$��	10,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� $'��!Distance, Velocity, Movement Type��$
��Left=�$��
105,47,0,0q�$	��Top=�$	��220��$��26��$
��Bold"��� $��Movement��$
��Left=�$��
499,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� -   $ lbObjectIDinRM1��$
��Left=�$��5,81,0,0q�$	��Top=�$��87��$��26��$��Center=��� -    $ lbObjectIDinRM2��$
��Left=�$��	5,175,0,0q�$	��Top=�$��87��$��26��$��Center=��� -!   $ lbObjectIDinRM3��$
��Left=�$��	5,268,0,0q�$	��Top=�$��87��$��26��$��Center=��� $?��9Percentage(%) for criteria velocity to re-define movement��$
��Left=�$��10,7,0,0q�$	��Top=�$	��315��$��26��$��Italic$��� -"   $ txtPercentage��$
��Left=�$��26��$��
325,10,0,0q�$	��Top=�$��55��$
��80.0�� �� 	IsEnabled . �� -#   $# btnAnalyzeInRedefinedMovement��$��Analyze��$
��Left=�$��
400,10,0,0q�$	��Top=�$��80��$ -0.162,0.264&�$��26���� -$   $! btnStartInRedefinedMovement��$��Start��$
��Left=�$��
499,10,0,0q�$	��Top=�$��80��$ -0.162,0.264&�$��26���� -%   $  btnStopInRedefinedMovement��$
��Stop��$
��Left=�$��
600,10,0,0q�$	��Top=�$��80��$��26���� -&   $  btnSaveInRedefinedMovement��$
��Save��$
��Left=�$��
701,10,0,0q�$	��Top=�$��80��$��26���� -'   $# BtnRefreshInRedefinedMovement��$��Refresh��$
��Left=�$��
800,10,0,0q�$	��Top=�$��80��$��26���� -(   $ lbxRedefinedMovementType1��	 . $��Visible=�$
��Left=�$��89��$��
105,81,0,0q�$	��Top=�$	��381������ *redefinedMovementType1+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$��50������ *Zone+�� $��3��$��30������ *
ZoneName+�� $��4��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��5��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��6��$��30������ *MovementTime+�� $��7��$��30������ *
Distance+�� $��8��$��30������ *
Velocity+�� $��9��$��30������ *RedefinedType+�� -)   $ lbxRedefinedMovementType2��	 . $��Visible=�$
��Left=�$��89��$��105,175,0,0q�$	��Top=�$	��381������ *redefinedMovementType2+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$��50������ *Zone+�� $��3��$��30������ *
ZoneName+�� $��4��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��5��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��6��$��30������ *MovementTime+�� $��7��$��30������ *
Distance+�� $��8��$��30������ *
Velocity+�� $��9��$��30������ *RedefinedType+�� -*   $ lbxRedefinedMovementType3��	 . $��Visible=�$
��Left=�$��89��$��105,268,0,0q�$	��Top=�$	��381������ *redefinedMovementType3+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$��50������ *Zone+�� $��3��$��30������ *
ZoneName+�� $��4��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��5��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��6��$��30������ *MovementTime+�� $��7��$��30������ *
Distance+�� $��8��$��30������ *
Velocity+�� $��9��$��30������ *RedefinedType+�� -+   $ lbxRedefinedMovement1��$
��Left=�	 . $��Visible=�$��89��$��
499,81,0,0q�$	��Top=�$	��381������ *redefinedMovement1+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��3��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��4��$��30������ *MovementTime+�� $��5��$��30������ *
Distance+�� $��6��$��30������ *
Velocity+�� $��7��$��30������ *Type+�� -,   $ lbxRedefinedMovement2��$
��Left=�	 . $��Visible=�$��89��$��499,175,0,0q�$	��Top=�$	��381������ *redefinedMovement2+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��3��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��4��$��30������ *MovementTime+�� $��5��$��30������ *
Distance+�� $��6��$��30������ *
Velocity+�� $��7��$��30������ *Type+�� --   $ lbxRedefinedMovement3��$
��Left=�	 . $��Visible=�$��89��$��499,268,0,0q�$	��Top=�$	��381������ *redefinedMovement3+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	StartTime+�� $��3��$ yyyy-MM-dd HH:mm:ss��$��30������ *	EndTime+�� $��4��$��30������ *MovementTime+�� $��5��$��30������ *
Distance+�� $��6��$��30������ *
Velocity+�� $��7��$��30������ *Type+�� $��Visualization��.��� .+��� ����  �� Path$ EllipseNodes� � 	Resources  �� DataType   NewSignalR.EllipseNode1   NewSignalR.EllipseNode2   NewSignalR.EllipseNode3��%   (��       *, +)(�� P     *, +)(�� �     *, +)�� "  .R�A� ���� *	FillColor+���� *Height+���� *Width+�� "  .R�A� ���� *	FillColor+���� *Height+���� *Width+�� "  .R�A� ���� *	FillColor+���� *Height+���� *Width+���� .R���  ��Name$ myCanvas��$
��Left=�$	��Top=�$	��340��$��10,10,10,10q�$	��870�����	#FFE9E9E9����  �� 
TargetType" ��.���  �� Property �@��Left �� Value �� $
 Left���  �@��Top �� $	 Top��� $��Visualization(GPS)��.��� .+��� ���� $ EllipseNodes� ��%   (��       *, +)(�� P     *, +)(�� �     *, +)�� "  .R�A� ���� *	FillColor+���� *Height+���� *Width+�� "  .R�A� ���� *	FillColor+���� *Height+���� *Width+�� "  .R�A� ���� *	FillColor+���� *Height+���� *Width+���� .R��� $ myCanvas��$
��Left=�$	��Top=�$	��340��$��10,10,10,10q�$	��870�����	#FFE9E9E9���� " ��.���  �@��Left �� $ LeftGPS���  �@��Top �� $ TopGPS��� $��	Collision��.��� .+��� $��ObjectID��$
��Left=�$��	10,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� $��
Collisions��$
��Left=�$��
105,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� $��Distance��$
��Left=�$��
600,47,0,0q�$	��Top=�$��87��$��26��$
��Bold"��� $*��$Collisions distinction distances (m)��$
��Left=�$��	203,7,0,0q�$	��Top=�$	��192��$��26��$��Italic$��� -.   $ txtMovementVelocity1��$
��Left=�$��26��$��
400,10,0,0q�$	��Top=�$��65��$	��1.0���� -/   $ btnStartInCollision��$��Start��$
��Left=�$��
499,10,0,0q�$	��Top=�$��80��$ -0.162,0.264&�$��26���� -0   $ btnStopInCollision��$
��Stop��$
��Left=�$��
600,10,0,0q�$	��Top=�$��80��$��26���� -1   $ btnSaveInCollision��$
��Save��$
��Left=�$��
701,10,0,0q�$	��Top=�$��80��$��26���� -2   $ BtnRefreshInCollision��$��Refresh��$
��Left=�$��
800,10,0,0q�$	��Top=�$��80��$��26���� -3   $ cmbinCollisionTagID1��$
��Left=�$��	10,80,0,0q�$	��Top=�$��87��$��22���� -4   $ cmbinCollisionTagID2��$
��Left=�$��
10,174,0,0q�$	��Top=�$��87��$��22���� -5   $ cmbinCollisionTagID3��$
��Left=�$��
10,268,0,0q�$	��Top=�$��87��$��22���� -6   $ listbox1inCollision��	 . $��Visible=�$
��Left=�$��89��$��
105,81,0,0q�$	��Top=�$	��474������ *collisionList1+���� .R�� 
 �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��30������ *Index+�� $��1��$��30������ *
ObjectId+�� $��2��$ yyyy-MM-dd HH:mm:ss��$��30������ *	Timestamp+�� $��3��$��50������ *X+�� $��4��$��50������ *Y+�� $��5��$��30������ *Zone+�� $��6��$��80������ *	Longitude+�� $��7��$��80������ *
Latitude+�� $��8��$��30������ *
Velocity+�� $��Distance (historical)��.��� .+��� -7   $ btnDistance��$��Distance��$
��Left=�$��	403,7,0,0q�$	��Top=�$��74��$��25���� -8   $ cmbObjectForDistance1��$
��Left=�$��	10,51,0,0q�$	��Top=�$��87��$��22��$��2���� -9   $ txtObjectForDistance1��$
��Left=�" �� VerticalScrollBarVisibility$ Visible=�$��91��$��
111,51,0,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��455��$��2���� -:   $ cmbObjectForDistance2��$
��Left=�$��
10,153,0,0q�$	��Top=�$��87��$��22���� -;   $ txtObjectForDistance2��$
��Left=�$ Visible=�$��91��$��111,154,0,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��455���� -<   $ cmbObjectForDistance3��$
��Left=�$��
10,257,0,0q�$	��Top=�$��87��$��22���� -=   $ txtObjectForDistance3��$
��Left=�$ Visible=�$��91��$��111,257,0,0q�$
 Wrap=�$��TextBox��$	��Top=�$	��455���� $��max age (min)��$
��Left=�$��	197,5,0,0q�$	��Top=�$��87��$��26���� ->   $ 
txtmax_age��$
��Left=�$��23��$��	289,9,0,0q�$
 Wrap=�$	��Top=�$��87�� �� TextDecorations Z� �� -?   $ cmbAggregation��$
��Left=�$��	111,9,0,0q�$	��Top=�$��69��$ 0.048,-0.138&�$��22���� $��Aggregation��$
��Left=�$��24,7,0,0q�$	��Top=�$��87��$��27���� -@   $ btnSaveDistance��$
��Save��$
��Left=�$��	492,7,0,0q�$	��Top=�$��74��$��25���� -A   $ BtnRefreshRESTful��$��Refresh��$
��Left=�$��	581,7,0,0q�$	��Top=�$��74��$��25��