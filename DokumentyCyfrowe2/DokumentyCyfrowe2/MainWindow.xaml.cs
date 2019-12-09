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
using System.Reflection;

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
        static MemoryStream memoryStream;
        static bool isNew = true;
        static string po = "Wnoszę o przyznanie mi prawa do zamieszkania w Domach Studenckich PG." +
"\n    Oświadczam, iż zapoznałem się z Regulaminem przyznawania prawa do zamieszkania w Domach Studenckich Politechniki Gdańskiej." +
"\n    Oświadczam, że gospodarstwo domowe nie osiąga dochodów ze źródeł innych niż wskazane." +
"\n    Uprzedzony o odpowiedzialności karnej za przestępstwo wyłudzenia nienależnych świadczeń finansowych(art. 286 KK) oświadczam, że wykazane" +
" dane są kompletne i zgodne ze stanem faktycznym." +
"\n    Oświadczam, iż uprzedzono mnie, że w przypadku, gdy okaże się, że otrzywąłem/ łam prawo do zamieszkania w Domu Studenckim PG na podstawie" +
" nieprawdziwych danych, będą wyciągnięte wobec mnie konsekwencje dyscyplinarne, do wydalenia z Uczelni włącznie, niezależnie od skutków cywilnoprawnych.";

        public MainWindow()
        {
            InitializeComponent();

            LoadEmpty();
        }

        private void OtworzButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;                
                //serializacja xml -> obiekt
                XmlSerializer serializer = new XmlSerializer(typeof(typ_wniosku));
                FileStream fs = new FileStream(fileName, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);

                dokument = (typ_wniosku)serializer.Deserialize(reader);

                fs.Close();

                Load(fileName);
                isNew = false;
            }
        }

        private void ZapiszButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isNew)
                {
                    dokument.pouczenie = po;
                    if (dokument.wnioskodawca.imie != null &&
                    dokument.wnioskodawca.nazwisko != null &&
                    dokument.wnioskodawca.wydzial != null &&
                    dokument.wnioskodawca.na_rok_akademicki != null &&
                    dokument.wnioskodawca.nr_albumu != null)
                    {
                        czlonek_rodziny_type a = new czlonek_rodziny_type();
                        a.imie = dokument.wnioskodawca.imie;
                        a.nazwisko = dokument.wnioskodawca.nazwisko;
                        var s = rodzinaGrid.Items;
                        czlonek_rodziny_type z = (czlonek_rodziny_type)s[0];
                        a.wiek = z.wiek;
                        a.st_pokrewienstwa = "WNIOSKODAWCA";
                        a.status_zatrudnienia = "POLITECHNIKA GDAŃSKA";

                        if (a.wiek == null)
                        {
                            msgError = "Podaj wiek wnioskodawcy w tabelce";
                            throw new System.InvalidOperationException("Podaj wiek wnioskodawcy w tabelce");
                        }

                        var obj = dokument.rodzina_wnioskodawcy.czlonek_rodziny;
                        Array.Resize(ref obj, 1);
                        obj[obj.Length - 1] = a;
                        dokument.rodzina_wnioskodawcy.czlonek_rodziny = obj;
                    }
                    else
                        throw new NullReferenceException();
                }
                else
                {
                    var obj = dokument.rodzina_wnioskodawcy.czlonek_rodziny;
                    Array.Resize(ref obj, 0);
                    dokument.rodzina_wnioskodawcy.czlonek_rodziny = obj;

                    foreach (czlonek_rodziny_type a in czlonkowie)
                    {
                        //if (a.st_pokrewienstwa != "WNIOSKODAWCA")
                        {
                            obj = dokument.rodzina_wnioskodawcy.czlonek_rodziny;
                            Array.Resize(ref obj, obj.Length + 1);
                            obj[obj.Length - 1] = a;

                            dokument.rodzina_wnioskodawcy.czlonek_rodziny = obj;
                        }
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
                //xmlDoc.Schemas.Add(null, "..\\..\\XMLSchema\\DC1.xsd");


                //////
                string xsdString = xsdlib.Resource.DC1;
                XmlSchema schema;
                using(StringReader xsdReader = new StringReader(xsdString))
                {
                    schema = XmlSchema.Read(xsdReader, null);
                }
                //////

                xmlDoc.Schemas.Add(schema);



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
                MessageBox.Show("Probujesz niepełny pusty dokument. Wypelnij wszystkie czerwone pola.", "Brakuje danych", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(msgError, "Wprowadzono błędne dane", MessageBoxButton.OK, MessageBoxImage.Warning);
                numErrors = 0;
                msgError = "";
            }

        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (!e.Exception.Message.Contains("pouczenie"))
            {
                var exception = (XmlSchemaValidationException)e.Exception;
                System.Xml.XmlElement sourceObject = (System.Xml.XmlElement)exception.SourceObject;

                msgError += "Niepoprawny format " + sourceObject.Name + "\r\n";
                numErrors++;
            }
        }

        private void LoadEmpty()
        {

            pouczenie.Content = new TextBlock() { Text = po, TextWrapping = TextWrapping.Wrap };

            dokument = new typ_wniosku();
            dokument.pouczenie = pouczenie.Content.ToString();
            dokument.wnioskodawca = new wnioskodawca_type();
            dokument.wnioskodawca.rodzaj_studiow = rodzaj_studiow_type.Istopnia;
            dokument.wnioskodawca.miejsce_dla_dziecka = true;
            dokument.rodzina_wnioskodawcy = new rodzina_wnioskodawcy_type();
            dokument.rodzina_wnioskodawcy.dochod_uzyskany = false;
            dokument.rodzina_wnioskodawcy.dochod_utracony = false;
            dokument.wnioskodawca.adres = new adres_type();
            dokument.wnioskodawca.nr_ds_priorytet1 = "1";
            dokument.wnioskodawca.nr_ds_priorytet2 = "1";

            isLoading = true;
            foreach (wydzial_type w in Enum.GetValues(typeof(wydzial_type)))
                Wydzial.Items.Add(w);
            foreach (wojewodztwo_type w in Enum.GetValues(typeof(wojewodztwo_type)))
                wojewodztwo.Items.Add(w);

            Rokakademicki.Items.Add("2015/2016");
            Rokakademicki.Items.Add("2016/2017");
            Rokakademicki.Items.Add("2017/2018");
            Rokakademicki.Items.Add("2018/2019");
            Rokakademicki.Items.Add("2019/2020");
            Rokakademicki.Items.Add("2020/2021");

            for (int a = 1; a < 8; a++)
                przewidywanysemestr.Items.Add(a.ToString());

            var type = typeof(kierunek_type);
            foreach (kierunek_type k in Enum.GetValues(typeof(kierunek_type)))
            {
                var memInfo = type.GetMember(k.ToString());
                string value;
                if (memInfo[0].CustomAttributes.Count() != 0)
                {
                    var attributes = memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute), false);
                    //if (attributes.Length != 0)
                    value = ((XmlEnumAttribute)attributes[0]).Name;
                }
                else
                    value = memInfo[0].Name;
                //    value = k.ToString();

                kierunek.Items.Add(value);
            }


            type = typeof(status_malzonka_type);
            foreach (status_malzonka_type k in Enum.GetValues(typeof(status_malzonka_type)))
            {
                var memInfo = type.GetMember(k.ToString());
                string value;
                var attributes = memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute), false);
                if (attributes.Length != 0)
                    value = ((XmlEnumAttribute)attributes[0]).Name;
                else
                    value = k.ToString();

                WspolmazonekJest.Items.Add(value);
            }

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

        private void Load(string filename)
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

            wojewodztwo.SelectedItem = dokument.wnioskodawca.wojewodztwo;

            Imie.Text = dokument.wnioskodawca.imie;
            Nazwisko.Text = dokument.wnioskodawca.nazwisko;

            przewidywanysemestr.SelectedItem = dokument.wnioskodawca.przewidywany_sem_studiow;
            if (dokument.wnioskodawca.rodzaj_studiow.ToString() == "Istopnia")
                st1.IsChecked = true;
            else
                st2.IsChecked = true;






            var type = typeof(kierunek_type);
            foreach (kierunek_type k in Enum.GetValues(typeof(kierunek_type)))
            {
                var memInfo = type.GetMember(k.ToString());
                string value;
                if (memInfo[0].CustomAttributes.Count() != 0)
                {
                    var attributes = memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute), false);
                    value = ((XmlEnumAttribute)attributes[0]).Name;
                }
                else
                    value = memInfo[0].Name;

                if (value == dokument.wnioskodawca.kierunek.ToString())
                {
                    //dokument.wnioskodawca.kierunek = k;
                    kierunek.SelectedItem = value;

                }
            }





            email.Text = dokument.wnioskodawca.email;
            telefon.Text = dokument.wnioskodawca.telefon;

            ulica.Text = dokument.wnioskodawca.adres.ulica;
            nr.Text = dokument.wnioskodawca.adres.nr;
            kodpocz.Text = dokument.wnioskodawca.adres.kod_pocztowy;
            miejscowosc.Text = dokument.wnioskodawca.adres.miejscowosc;
            powiat.Text = dokument.wnioskodawca.powiat;
            gmina.Text = dokument.wnioskodawca.gmina;


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
            if (dokument.rodzina_wnioskodawcy.czlonek_rodziny[0].wiek == null)
                czlonkowie.First<czlonek_rodziny_type>().wiek = rnd.Next(18, 27).ToString();
            else
                czlonkowie.First<czlonek_rodziny_type>().wiek = dokument.rodzina_wnioskodawcy.czlonek_rodziny[0].wiek;
            czlonkowie.First<czlonek_rodziny_type>().imie = dokument.wnioskodawca.imie;
            czlonkowie.First<czlonek_rodziny_type>().nazwisko = dokument.wnioskodawca.nazwisko;

            rodzinaGrid.ItemsSource = czlonkowie;




            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;

                //////
                string xsdString = xsdlib.Resource.DC1;
                XmlSchema schema;
                using (StringReader xsdReader = new StringReader(xsdString))
                {
                    schema = XmlSchema.Read(xsdReader, null);
                }
                //////

                settings.Schemas.Add(schema);


                //settings.Schemas.Add(null, "..\\..\\XMLSchema\\DC1.xsd");


                var xmlReader = XmlReader.Create(filename, settings);
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                xmlDoc.Validate(eventHandler);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("pouczenie"))
                    MessageBox.Show("Plik nieprawidłowo sformatowany", "Błącz wczytania pliku", MessageBoxButton.OK, MessageBoxImage.Warning);
            }




            isLoading = false;
        }



        private void Wydzial_SelectionChanged(object sender, SelectionChangedEventArgs e) => dokument.wnioskodawca.wydzial = (wydzial_type)Wydzial.SelectedItem;

        private void Rokakademicki_SelectionChanged(object sender, SelectionChangedEventArgs e) => dokument.wnioskodawca.na_rok_akademicki = Rokakademicki.SelectedItem.ToString();

        private void przewidywanysemestr_SelectionChanged(object sender, SelectionChangedEventArgs e) => dokument.wnioskodawca.przewidywany_sem_studiow = przewidywanysemestr.SelectedItem.ToString();

        private void kierunek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = typeof(kierunek_type);
            foreach (kierunek_type k in Enum.GetValues(typeof(kierunek_type)))
            {
                var memInfo = type.GetMember(k.ToString());
                string value;
                if (memInfo[0].CustomAttributes.Count() != 0)
                {
                    var attributes = memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute), false);
                    value = ((XmlEnumAttribute)attributes[0]).Name;
                }
                else
                    value = memInfo[0].Name;

                if (value == kierunek.SelectedItem.ToString())
                {
                    dokument.wnioskodawca.kierunek = k;
                }
            }
        }

        private void Prio1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                dokument.wnioskodawca.nr_ds_priorytet1 = Prio1.SelectedItem.ToString();
        }

        private void Prio2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dokument.wnioskodawca.nr_ds_priorytet2 = Prio2.SelectedItem.ToString();
        }



        private void WspolmazonekJest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (malzonekTAK.IsChecked == true)
            {
                WspolmazonekJest.SelectedItem.ToString().Replace(" ", string.Empty);

                dokument.wnioskodawca.malzonek.status = (status_malzonka_type)WspolmazonekJest.SelectedItem;
            }
        }



        private void datazlozeniawniosku_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dokument != null)
                dokument.data_zlozenia = (System.DateTime)datazlozeniawniosku.SelectedDate;
        }

        private void st1_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false && dokument != null)
                dokument.wnioskodawca.rodzaj_studiow = rodzaj_studiow_type.Istopnia;
        }

        private void st2_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.wnioskodawca.rodzaj_studiow = rodzaj_studiow_type.IIstopnia;
        }

        private void malzonekNIE_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false && dokument != null)
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
            if (isLoading == false && dokument != null)
                dokument.wnioskodawca.miejsce_dla_dziecka = false;
        }

        private void dochUZNIE_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false && dokument != null)
                dokument.rodzina_wnioskodawcy.dochod_uzyskany = false;
        }

        private void dochUTTAK_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false)
                dokument.rodzina_wnioskodawcy.dochod_utracony = true;
        }

        private void dochUTNIE_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoading == false && dokument != null)
                dokument.rodzina_wnioskodawcy.dochod_utracony = false;
        }

        private void malzonekTAK_Checked(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.malzonek = new malzonek_type();
        }

        private void Imie_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.imie = Imie.Text;
        }

        private void Album_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.nr_albumu = Album.Text;
        }

        private void Nazwisko_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.nazwisko = Nazwisko.Text;
        }

        private void email_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.email = email.Text;
        }

        private void telefon_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.telefon = telefon.Text;
        }

        private void ulica_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.adres.ulica = ulica.Text;
        }

        private void nr_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.adres.nr = nr.Text;
        }

        private void kodpocz_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.adres.kod_pocztowy = kodpocz.Text;
        }

        private void miejscowosc_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.adres.miejscowosc = miejscowosc.Text;

        }

        private void MalzImieNazw_LostFocus(object sender, RoutedEventArgs e)
        {
            if (malzonekTAK.IsChecked == true)
                dokument.wnioskodawca.malzonek.imie_i_nazwisko = MalzImieNazw.Text;
        }

        private void pracadlaPG_LostFocus_1(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.praca_na_rzecz_uczelni = pracadlaPG.Text;

        }

        private void wojewodztwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dokument.wnioskodawca.wojewodztwo = (wojewodztwo_type)wojewodztwo.SelectedItem;
        }

        private void powiat_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.powiat = powiat.Text;

        }

        private void gmina_LostFocus(object sender, RoutedEventArgs e)
        {
            dokument.wnioskodawca.gmina = gmina.Text;

        }

        private void data_urodzenia_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan span = datazlozeniawniosku.SelectedDate.Value - data_urodzenia.SelectedDate.Value;
            
            DateTime zeroTime = new DateTime(1, 1, 1);

            // Because we start at year 1 for the Gregorian
            // calendar, we must subtract a year here.
            int years = (zeroTime + span).Year - 1;
            czlonkowie[0].wiek = years.ToString();
            rodzinaGrid.ItemsSource = null;
            rodzinaGrid.ItemsSource = czlonkowie;
        }
    }
}
