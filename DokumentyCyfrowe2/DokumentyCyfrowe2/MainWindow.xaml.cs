using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;
using Microsoft.Win32;

namespace DokumentyCyfrowe2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        public typ_wniosku dokument;
        string fileName = "";
        List<czlonek_rodziny_type> czlonkowie = new List<czlonek_rodziny_type>();

        public MainWindow()
        {
            InitializeComponent();
            //fileName = "C:\\Users\\me\\Documents\\DC\\DokumentyCyfrowe2\\DokumentyCyfrowe2\\SampleXml\\DC1_3.xml";
            
            //serializacja xml -> obiekt
            //XmlSerializer serializer = new XmlSerializer(typeof(typ_wniosku));
            //FileStream fs = new FileStream(fileName, FileMode.Open);
            //XmlReader reader = XmlReader.Create(fs);

            //dokument = (typ_wniosku)serializer.Deserialize(reader);

            //fs.Close();
            LoadEmpty();
           // Load();
        }

        private void OtworzButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                fileName = openFileDialog.FileName;

            //serializacja xml -> obiekt
            XmlSerializer serializer = new XmlSerializer(typeof(typ_wniosku));
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);
            
            dokument = (typ_wniosku)serializer.Deserialize(reader);
            
            fs.Close();

            Load();
        }
        private void LoadEmpty()
        {
            foreach (wydzial_type w in Enum.GetValues(typeof(wydzial_type)))
                Wydzial.Items.Add(w);

            Rokakademicki.Items.Add("2015/2016");
            Rokakademicki.Items.Add("2016/2017");
            Rokakademicki.Items.Add("2017/2018");
            Rokakademicki.Items.Add("2018/2019");
            Rokakademicki.Items.Add("2019/2020");
            Rokakademicki.Items.Add("2020/2021");

            for (int a = 1; a <8; a++)
                przewidywanysemestr.Items.Add(a.ToString());

            foreach (kierunek_type k in Enum.GetValues(typeof(kierunek_type)))
                kierunek.Items.Add(k);

            foreach (status_malzonka_type k in Enum.GetValues(typeof(status_malzonka_type)))
                WspolmazonekJest.Items.Add(k);

            for (int a = 1; a < 13; a++)
            {
                Prio1.Items.Add(a.ToString());
                Prio2.Items.Add(a.ToString());
            }

            datazlozeniawniosku.SelectedDate = DateTime.Today;
            //pouczenie.Content =  new TextBlock() { Text = dokument.pouczenie, TextWrapping = TextWrapping.Wrap };


            czlonkowie.Add(new czlonek_rodziny_type { status_zatrudnienia="POLITECHNIKA GDANSKA", st_pokrewienstwa="WNIOSKODAWCA"  });
            rodzinaGrid.ItemsSource = czlonkowie;
        }

        private void Load()
        {
            czlonkowie.Clear();
            czlonkowie.Add(new czlonek_rodziny_type { status_zatrudnienia = "POLITECHNIKA GDANSKA", st_pokrewienstwa = "WNIOSKODAWCA" });
            rodzinaGrid.ItemsSource = czlonkowie;

            pouczenie.Content = new TextBlock() { Text = dokument.pouczenie, TextWrapping = TextWrapping.Wrap };

            if (dokument.data_zlozeniaSpecified)
                datazlozeniawniosku.SelectedDate = dokument.data_zlozenia;

            Wydzial.SelectedItem = dokument.wnioskodawca.wydzial;
            Album.Text = dokument.wnioskodawca.nr_albumu;
            Rokakademicki.SelectedItem = dokument.wnioskodawca.na_rok_akademicki;

            Imie.Text = dokument.wnioskodawca.imie;
            Nazwisko.Text = dokument.wnioskodawca.nazwisko;

            przewidywanysemestr.SelectedItem = dokument.wnioskodawca.przewidywany_sem_studiow;
            if (dokument.wnioskodawca.rodzaj_studiow.ToString() == "Istopnia")
                st1.IsChecked = true;
            else
                st2.IsChecked = true;
            kierunek.SelectedItem = dokument.wnioskodawca.kierunek;

            email.Text = dokument.wnioskodawca.email;
            telefon.Text = dokument.wnioskodawca.telefon;

            ulica.Text = dokument.wnioskodawca.adres.ulica;
            nr.Text = dokument.wnioskodawca.adres.nr;
            kodpocz.Text = dokument.wnioskodawca.adres.kod_pocztowy;
            miejscowosc.Text = dokument.wnioskodawca.adres.miejscowosc;

            Prio1.SelectedItem = dokument.wnioskodawca.nr_ds_priorytet1;
            Prio2.SelectedItem = dokument.wnioskodawca.nr_ds_priorytet2;

            if (dokument.wnioskodawca.miejsce_dla_dzieckaSpecified)
            {
                if(dokument.wnioskodawca.miejsce_dla_dziecka)
                    dzieckoTAK.IsChecked = true;
                else
                    dzieckoNIE.IsChecked = true;
            }

            if (dokument.wnioskodawca.malzonek != null)
            {
                if (dokument.wnioskodawca.malzonek.miejsce_dlaSpecified)
                {
                    if (dokument.wnioskodawca.malzonek.miejsce_dla)
                        malzonekTAK.IsChecked = true;
                    else
                        malzonekNIE.IsChecked = true;
                    //wyszarz pola malzonka
                }

                MalzImieNazw.Text = dokument.wnioskodawca.malzonek.imie_i_nazwisko;
                WspolmazonekJest.SelectedItem = dokument.wnioskodawca.malzonek.status;
            }
            pracadlaPG.Text = dokument.wnioskodawca.praca_na_rzecz_uczelni;

            if (dokument.rodzina_wnioskodawcy.dochod_utraconySpecified)
            {
                if (dokument.rodzina_wnioskodawcy.dochod_utracony)
                    dochUTTAK.IsChecked = true;
                else
                    dochUTNIE.IsChecked = true;
            }


            rodzinaGrid.ItemsSource = null;

            foreach (czlonek_rodziny_type a in dokument.rodzina_wnioskodawcy.czlonek_rodziny)
            {
                if (a.st_pokrewienstwa != "WNIOSKODAWCA")
                {
                    czlonkowie.Add(new czlonek_rodziny_type
                    {
                        imie = a.imie,
                        nazwisko = a.nazwisko,
                        status_zatrudnienia = a.status_zatrudnienia,
                        st_pokrewienstwa = a.st_pokrewienstwa,
                        wiek = a.wiek
                    });
                }
            }
            Random rnd = new Random();
            czlonkowie.First<czlonek_rodziny_type>().wiek = rnd.Next(18, 27).ToString();
            czlonkowie.First<czlonek_rodziny_type>().imie = dokument.wnioskodawca.imie;
            czlonkowie.First<czlonek_rodziny_type>().nazwisko = dokument.wnioskodawca.nazwisko;
            
            rodzinaGrid.ItemsSource = czlonkowie;
        }
    }
}
