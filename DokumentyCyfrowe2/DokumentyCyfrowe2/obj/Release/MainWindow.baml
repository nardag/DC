   M S B A M L   `   `   `  ���� L  HDokumentyCyfrowe2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:DokumentyCyfrowe2DokumentyCyfrowe2  "    DokumentyCyfrowe2.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:DokumentyCyfrowe2       � Title$4  .Wniosek o przyznanie miejsca w domu studenckim��$
��1000��$
��1100��.���� $
��Auto=�.��� $��0,0,0,0q� � ColumnDefinitions �� $
��169* ��� $
��925* �.+��� $��INFORMACJE O WNIOSKODAWCY��$��Center=�$��174,10,534,0q�$	��Top=�$	��217��$��1���� ���#FF0000$��Wydział��$
��Left=�$��	31,57,0,0q�$	��Top=��� ���#FF0000$��Imię��$
��Left=�$��	31,83,0,0q�$	��Top=��� ���#FF0000$��	Nr albumu��$
��Left=�$��
151,57,0,0q�$	��Top=�$��1���� ���#FF0000$��Na rok akademicki��$
��Left=�$��
367,57,0,0q�$	��Top=�$��1���� ���#FF0000$��Nazwisko��$
��Left=�$��
151,91,0,0q�$	��Top=�$��1���� $
��Left=�$��23��$��
221,60,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��120��$��1���� -    ��Name$ Album��$
��Left=�$��23��$��
221,60,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��120��$��1���� -   $
 Imie��$
��Left=�$��23��$��
126,86,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��180�� J� RenderTransformOrigin$ 0.5,0.5&�$��2���� -   $ Nazwisko��$
��Left=�$��23��$��
221,91,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��380��$��1���� ���#FF0000$#��Przewidywany semestr studiów��$
��Left=�$��
31,123,0,0q�$	��Top=�$	��182��$��2���� $��Rodzaj studiów��$
��Left=�$��110,123,0,0q�$	��Top=�$��91��$��1��	� -    	�Name$	 st1�� 	� 	GroupName$ rodzajst��$��	I stopnia��$
��Left=�$��221,129,0,0q�$	��Top=� d� 	IsChecked$
 trueY�$��1��	� -   $	 st2��$ rodzajst��$��
II stopnia��$
��Left=�$��221,149,0,0q�$	��Top=�$��1���� ���#FF0000$��Kierunek��$
��Left=�$��315,123,0,0q�$	��Top=�$��62��$��1���� -    ��Name$ kierunek��$
��Left=�$��391,129,0,0q�$	��Top=�$	��210��$��1���� -   $ przewidywanysemestr��$
��Left=�$��
49,126,0,0q�$	��Top=�$��56��$��1���� -   $ Rokakademicki��$
��Left=�$��
479,60,0,0q�$	��Top=�$	��113��$��1���� -	   $ Wydzial��$
��Left=�$��
126,57,0,0q�$	��Top=�$	��180��$��2���� ���#FF0000$��E-mail��$
��Left=�$��
31,174,0,0q�$	��Top=�$��51���� -
   $ email��$
��Left=�$��23��$��
82,178,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��344��$ 0.5,0.5&�$��2���� ���#FF0000$��Tel. kontaktowy
��$
��Left=�$��272,175,0,0q�$	��Top=�$��95��$��26��$��1���� -   $ telefon��$
��Left=�$��23��$��372,176,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��229��$ 0.5,0.5&�$��1���� $+��%Adres zameldowania [korespondencyjny]��$
��Left=�$��
31,215,0,0q�$	��Top=�$	��230��$��2���� ���#FF0000$��Ulica��$
��Left=�$��
31,241,0,0q�$	��Top=�$��51���� -   $ ulica��$
��Left=�$��23��$��
82,241,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��256��$ 0.5,0.5&�$��2���� ���#FF0000$��Nr��$
��Left=�$��174,240,0,0q�$	��Top=�$��27��$ 	5.173,0.5&�$��1���� -   $ nr��$
��Left=�$��23��$��216,241,0,0q�$
 Wrap=�$�� ��$	��Top=�$��52��$ 0.5,0.5&�$��1���� ���#FF0000$��Kod pocztowy��$
��Left=�$��
31,267,0,0q�$	��Top=�$��88���� -   $ kodpocz��$
��Left=�$��23��$��119,271,0,0q�$
 Wrap=�$�� ��$	��Top=�$��69��$ 0.5,0.5&�$��2���� ���#FF0000$��Miejscowość��$
��Left=�$��
28,270,0,0q�$	��Top=�$��77��$��1���� -   $ miejscowosc��$
��Left=�$��23��$��112,271,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��158��$ 0.5,0.5&�$��1���� $��Priorytet 1��$
��Left=�$��306,238,0,0q�$	��Top=�$��71��$��1���� -   $ Prio1��$
��Left=�$��391,240,0,0q�$	��Top=�$��42��$��1���� $��Priorytet 2��$
��Left=�$��445,238,0,0q�$	��Top=�$��71��$��1���� -   $ Prio2��$
��Left=�$��521,240,0,0q�$	��Top=�$��42��$��1���� $%��Ubiegam się o miejsce w DS nr:��$
��Left=�$��306,209,0,0q�$	��Top=�$	��230��$��1���� $2��,Ubiegam się o miejsce dla współmałżonka��$
��Left=�$��
31,326,0,0q�$	��Top=�$	��243��$��2��	� -   $ malzonekTAK��$ malzonek��$	��tak��$
��Left=�$��101,332,0,0q�$	��Top=�$��1��	� -   $ malzonekNIE��$ malzonek��$	��nie��$
��Left=�$��151,332,0,0q�$	��Top=�$
 trueY�$��1���� $(��"Imię  i nazwisko wpółmałżonka��$
��Left=�$��
31,357,0,0q�$	��Top=�$	��182��$��2���� -   $ MalzImieNazw��$
��Left=�$��23��$��
44,357,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��157��$ 0.5,0.5&�$��1���� $��Współmałżonek jest��$
��Left=�$��
31,392,0,0q�$	��Top=�$	��122���� $
��Left=�$��153,392,0,0q�$	��Top=�$	��217��$��2���� $3��-Ubiegam się o miejsce normatywne dla dziecka��$
��Left=�$��
31,423,0,0q�$	��Top=�$	��261��$��2��	� -   $ 
dzieckoTAK��$ dziecko��$	��tak��$
��Left=�$��133,429,0,0q�$	��Top=�$��1��	� -   $ 
dzieckoNIE��$ dziecko��$	��nie��$
��Left=�$��183,429,0,0q�$	��Top=�$
 trueY�$��1���� -   $ 
pracadlaPG��$
��Left=�$��87��$��
55,480,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��371��$ 0.5,0.5&�$��2���� $��Praca na rzecz uczelni��$
��Left=�$��
63,454,0,0q�$	��Top=�$	��243��$��2���� $��INFORMACJE O RODZINIE��$��Center=�$��183,604,549,0q�$	��Top=�$	��193��$��1���� $$��Wnioskuję od dochód uzyskany��$��Center=�$��36,572,881,0q�$	��Top=�$	��177��$��2��	� -   $ 	dochUZTAK��$ dochuzyskany��$	��tak��$
��Left=�$��
88,578,0,0q�$	��Top=�$ 0.631,-1.776&�$��1��	� -   $ 	dochUZNIE��$ dochuzyskany��$	��nie��$
��Left=�$��151,578,0,0q�$	��Top=�$
 trueY�$��1���� $$��Wnioskuję od dochód utracony��$��Center=�$��230,572,518,0q�$	��Top=�$	��177��$��1��	� -   $ 	dochUTTAK��$ dochutracony��$	��tak��$
��Left=�$��445,578,0,0q�$	��Top=�$��1��	� -   $ 	dochUTNIE��$ dochutracony��$	��nie��$
��Left=�$��501,578,0,0q�$	��Top=�$
 trueY�$��1���� -   	 ��Name$	 	pouczenie��$��Center=�$��646,91,23,0q�$	��Top=�$	��256��$	��401��$��1���� -   $ WspolmazonekJest��$
��Left=�$��153,392,0,0q�$	��Top=�$	��217��$��2���� -   $��Otworz plik��$
��Left=�$��665.2,666,0,0q�$	��Top=�$��75��$��1���� -   $��Zapisz plik��$
��Left=�$��772.2,666,0,0q�$	��Top=�$��75��$��1���� $
��Data��$
��Left=�$��
423,10,0,0q�$	��Top=�$ 0.367,-1.297&�$��1��(  "System.Windows.Controls.DatePicker  -    
  Name$
 datazlozeniawniosku��$
��Left=�$��
481,12,0,0q�$	��Top=�$	��164��$��1��&   System.Windows.Controls.DataGrid  -!     Name$ rodzinaGrid��   AutoGenerateColumns . $
��Left=�$	��240��$��
36,650,0,0q�$	��Top=�$	��717��$��2���� -"   $	 pouczenie_Copy��$��
POUCZENIE:��$��Center=�$��646,53,23,0q�$	��Top=�$	��256��$��26��$��1���� ���#FF0000$��Województwo��$
��Left=�$��267,310,0,0q�$	��Top=�$	��100��$��1���� -#   $ wojewodztwo��$
��Left=�$��372,310,0,0q�$	��Top=�$	��210��$��1���� ���#FF0000$��Powiat��$
��Left=�$��267,354,0,0q�$	��Top=�$	��100��$��1���� ���#FF0000$��Gmina��$
��Left=�$��267,392,0,0q�$	��Top=�$	��100��$��1���� -$   $ powiat��$
��Left=�$��23��$��378,354,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��158��$ 0.5,0.5&�$��1���� -%   $ gmina��$
��Left=�$��23��$��378,395,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��158��$ 0.5,0.5&�$��1���� -&   $	 pouczenie_Copy1�����#FF0000$,��&CZERWONE POLA MUSZĄ BYĆ WYPEŁNIONE!��$��Center=�$��646.2,560,22.6,0q�$	��Top=�$	��256��$��38��$��1��