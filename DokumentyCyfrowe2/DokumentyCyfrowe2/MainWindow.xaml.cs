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
using System.Data;

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
        static string msgError;
        static int numErrors;
        bool isLoading = false;
        public MainWindow()
        {
            InitializeComponent();

            LoadEmpty();
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

        private void ZapiszButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = dokument.rodzina_wnioskodawcy.czlonek_rodziny;
                Array.Resize(ref obj, 0);
                dokument.rodzina_wnioskodawcy.czlonek_rodziny = obj;
                        
                foreach (czlonek_rodziny_type a in czlonkowie)
                {
                    if (a.st_pokrewienstwa != "WNIOSKODAWCA")
                    {
                        obj = dokument.rodzina_wnioskodawcy.czlonek_rodziny;
                        Array.Resize(ref obj, obj.Length + 1);
                        obj[obj.Length - 1] = a;

                        dokument.rodzina_wnioskodawcy.czlonek_rodziny = obj;
                    }
                }

                var memoryStream = new MemoryStream();

                XmlWriterSettings wsettings = new XmlWriterSettings();
                wsettings.Indent = true;
                wsettings.IndentChars = ("\t");
                wsettings.OmitXmlDeclaration = true;

                var xmlWriter = XmlWriter.Create(memoryStream, wsettings);

                XmlSerializer serializer = new XmlSerializer(dokument.GetType());
                serializer.Serialize(xmlWriter, dokument);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;

                memoryStream.Position = 0;

                var xmlReader = XmlReader.Create(memoryStream, settings);

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                xmlDoc.Schemas.Add(null, "..\\..\\XMLSchema\\DC1.xsd");

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                xmlDoc.Validate(eventHandler);

                if (numErrors > 0)
                    throw new Exception(msgError);

                xmlWriter.Flush();

                // save file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    //seralizacja dokument w formie memorystream do xml
                    FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate);

                    memoryStream.Position = 0;

                    memoryStream.CopyTo(stream);

                    stream.Close();
                }
                memoryStream.Close();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Probujesz zapisac pusty dokument. Wypelnij pola.", "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(msgError, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                numErrors = 0;
                msgError = "";
            }

        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            //TODO 
            if (!e.Exception.Message.Contains("pouczenie"))
            {
                msgError = msgError + "\r\n" + e.Message + " " + e.Exception.LineNumber;
                numErrors++;
            }
        }

        private void LoadEmpty()
        {
            isLoading = true;
            foreach (wydzial_type w in Enum.GetValues(typeof(wydzial_type)))
                Wydzial.Items.Add(w);

            Rokakademicki.Items.Add("2015/2016");
            Rokakademicki.Items.Add("2016/2017");
            Rokakademicki.Items.Add("2017/2018");
            Rokakademicki.Items.Add("2018/2019");
            Rokakademicki.Items.Add("2019/2020");
            Rokakademicki.Items.Add("2020/2021");

            for (int a = 1; a < 8; a++)
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


            czlonkowie.Add(new czlonek_rodziny_type { status_zatrudnienia = "POLITECHNIKA GDANSKA", st_pokrewienstwa = "WNIOSKODAWCA" });
            rodzinaGrid.ItemsSource = czlonkowie;
            isLoading = false;
        }

        private void Load()
        {
            isLoading = true;
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
                if (dokument.wnioskodawca.miejsce_dla_dziecka)
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
                    //TODO wyszarz pola malzonka
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
            isLoading = false;
        }

        private void kodpocztextChangedEventHandler(object sender, TextChangedEventArgs args) => dokument.wnioskodawca.adres.kod_pocztowy = kodpocz.Text;

        private void Wydzial_SelectionChanged(object sender, SelectionChangedEventArgs e) => dokument.wnioskodawca.wydzial = (wydzial_type)Wydzial.SelectedItem;

        private void Album_TextChanged(object sender, TextChangedEventArgs e) => dokument.wnioskodawca.nr_albumu = Album.Text;

        private void Rokakademicki_SelectionChanged(object sender, SelectionChangedEventArgs e) => dokument.wnioskodawca.na_rok_akademicki = Rokakademicki.SelectedItem.ToString();

        private void Imie_TextChanged(object sender, TextChangedEventArgs e) => dokument.wnioskodawca.imie = Imie.Text;

        private void Nazwisko_TextChanged(object sender, TextChangedEventArgs e) => dokument.wnioskodawca.nazwisko = Nazwisko.Text;

        private void przewidywanysemestr_SelectionChanged(object sender, SelectionChangedEventArgs e) => dokument.wnioskodawca.przewidywany_sem_studiow = przewidywanysemestr.SelectedItem.ToString();

        private void kierunek_SelectionChanged(object sender, SelectionChangedEventArgs e) => dokument.wnioskodawca.kierunek = (kierunek_type)kierunek.SelectedItem;

        private void email_TextChanged(object sender, TextChangedEventArgs e) => dokument.wnioskodawca.email = email.Text;

        private void telefon_TextChanged(object sender, TextChangedEventArgs e) => dokument.wnioskodawca.telefon = telefon.Text;

        private void ulica_TextChanged(object sender, TextChangedEventArgs e) => dokument.wnioskodawca.adres.ulica = ulica.Text;

        private void nr_TextChanged(object sender, TextChangedEventArgs e) => dokument.wnioskodawca.adres.nr = nr.Text;

        private void miejscowosc_TextChanged(object sender, TextChangedEventArgs e)
        {
            dokument.wnioskodawca.adres.miejscowosc = miejscowosc.Text;
        }

        private void Prio1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dokument.wnioskodawca.nr_ds_priorytet1 = Prio1.SelectedItem.ToString();
        }

        private void Prio2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dokument.wnioskodawca.nr_ds_priorytet2 = Prio2.SelectedItem.ToString();
        }

        private void MalzImieNazw_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(malzonekTAK.IsChecked == true)
                dokument.wnioskodawca.malzonek.imie_i_nazwisko = MalzImieNazw.Text;
        }

        private void WspolmazonekJest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (malzonekTAK.IsChecked == true)
                dokument.wnioskodawca.malzonek.status = (status_malzonka_type)WspolmazonekJest.SelectedItem;
        }

        private void pracadlaPG_TextChanged(object sender, TextChangedEventArgs e)
        {
            dokument.wnioskodawca.praca_na_rzecz_uczelni = pracadlaPG.Text;
        }

        private void datazlozeniawniosku_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dokument != null)
                dokument.data_zlozenia = (System.DateTime)datazlozeniawniosku.SelectedDate;
        }

        private void st1_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.wnioskodawca.rodzaj_studiow = rodzaj_studiow_type.Istopnia;
        }

        private void st2_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.wnioskodawca.rodzaj_studiow = rodzaj_studiow_type.IIstopnia;
        }

        private void malzonekNIE_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.wnioskodawca.malzonek = null;
        }

        private void dochUZTAK_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)                
                dokument.rodzina_wnioskodawcy.dochod_uzyskany = true;
        }

        private void dzieckoTAK_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.wnioskodawca.miejsce_dla_dziecka = true;
        }

        private void dzieckoNIE_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.wnioskodawca.miejsce_dla_dziecka = false;
        }

        private void dochUZNIE_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.rodzina_wnioskodawcy.dochod_uzyskany = false;
        }

        private void dochUTTAK_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.rodzina_wnioskodawcy.dochod_utracony = true;
        }

        private void dochUTNIE_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.rodzina_wnioskodawcy.dochod_utracony = false;
        } 

        private void rodzinaGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            //TODO remove this function
        }
    }
}
