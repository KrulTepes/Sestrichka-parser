   M S B A M L   `   `   `  ���� Z  VСестричка-парсер, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Сестричка_парсерСестричка_парсер  0    *Сестричка_парсер.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ?local-clr-namespace:Сестричка_парсер     U QMaterialDesignThemes.Wpf, Version=2.6.0.325, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$%  Сестричка-парсер��$	��600��$	��896��.���  � RowDefinitions �� $
��auto ��� $��* ��� $
��auto �.+�� $��0�� �� �� .+���  �� Orientation$ 
Horizontal=�$	��Top=�$
��Left=�$��0��.+��� -   $2��,Загрузить из интеренета��$��3q��� -   $��Справка��$��3q��� -    ��Name$ processingTB��$��Page processing: ��$��13(� �� Padding$ 3q��� $��ФИО��$��13(�$��20,0,0,0q�$ 3q��� -    ��Name$ fioTB��$�� ��$	��200���� $ Vertical=�$��1��.+��  � ColumnDefinitions �� $
��auto ��� $��* �.+��� $��Адрес: ���� -   $ linkTB��$���~https://pnzgu.ru/apply/list/faculty/352690757/speciality/843/edu_level/2/edu_form/1/edu_base/1/sort_field/sum/sort_type/desc/p��$��3q�$��1���  �� �� �� �� .+��� $ 
Horizontal=�$��0��.+��� $"��Кол-во страниц: ���� -   $ 	endPageTB��$��0��$��40��$��3q��� $ 
Horizontal=�$��1��.+��� $/��)Кол-во бюджетных мест: ���� -   $ countPublicPlacesTB��$��0��$��40��$��3q��� $ 
Horizontal=�$��2��.+��� $)��#Кол-во особых мест: ���� -   $ countSpecialPlacesTB��$��0��$��40��$��3q��� $ 
Horizontal=�$��3��.+��� $+��%Кол-во целевых мест: ���� -	   $ countTargetPlacesTB��$��0��$��40��$��3q��� -
    ��Name$ listAbiturient��$��1�� �� View �� .P��� $��*��	 �� Width$	 20��
 �� DisplayMemberBinding
 �� *obj+�� $	��New��$	 50��
 �� *New+�� $	��№��
 �� *Num+�� $��ФИО��
 �� *FIO+�� $��Общие баллы��
 �� *
AllBalls+�� $!��Оригинал/Копия��
 �� *Doc+�� $��Особые квоты��
 �� *SpecialRights+�� $+��%Целевое направление��
 �� *TargetDirection+�� $ 
Horizontal=�$��Bottom=�$��Right=�$��2��.+��� -   $2��,Сохранить шаблот поиска��$��3q��� -   $0��*Сбросить шаблот поиска��$��3q��� -   $��Загрузить��$��3q��� -   $��Сравнить��$��3q��� -   $��Сохранить��$��3q�