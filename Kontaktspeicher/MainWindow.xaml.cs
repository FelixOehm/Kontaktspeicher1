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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Kontaktspeicher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Kontakt> kontakte = new List<Kontakt>();
        List<Button> kontaktButton = new List<Button>();
        public int deleteCount { get; set; }
        BinaryFormatter saveFormater;

        public MainWindow()
        {
            InitializeComponent();

            string fileContent;

            using (StreamReader checkstream = new StreamReader(@"C:\Users\felix oehm\Desktop\C# Referenzen\WPF Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\kontakte.txt"))
            {
                fileContent = checkstream.ReadToEnd();
            }

            saveFormater = new BinaryFormatter();
            if (fileContent != "")
            {
                using (FileStream kontaktStream = new FileStream(@"C:\Users\felix oehm\Desktop\C# Referenzen\WPF Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\kontakte.txt", FileMode.Open, FileAccess.Read))
                {
                    kontakte = (List<Kontakt>)saveFormater.Deserialize(kontaktStream);
                    for (int i = 0; i < kontakte.Count; i++)
                    {
                        //Erzeigen eines KontaktButtons
                        Button zwischenButton = new Button();
                        zwischenButton.Content = kontakte[i].vorname + " " + kontakte[i].nachname;
                        zwischenButton.Click += KontaktButton_Click;

                        //Zuweisen des KontaktButtons der Liste
                        kontaktButton.Add(zwischenButton);
                        kontaktPanel.Children.Add(kontaktButton[kontakte[i].kontaktNummer]);
                    }
                }
            }
        }

        //Kontakt Speichern
        private void SafeButton_Click(object sender, RoutedEventArgs e)
        {
            //Erzeugen eines Zwischenkontakts
            Kontakt zwischenkontakt = new Kontakt();

            //Zuweisen der KontaktNummer
            zwischenkontakt.kontaktNummer = kontakte.Count;

            //Zuweisen des Vornamens
            zwischenkontakt.vorname = vornameTextBox.Text;

            //Zuweisen des Nachnamens
            zwischenkontakt.nachname = nachnameTextBox.Text;

            //Zuweisen des Geschlechts
            if(mRadioButton.IsChecked == true)
            {
                zwischenkontakt.geschlecht = 1;
            }
            else if(wRadioButton.IsChecked == true)
            {
                zwischenkontakt.geschlecht = 2;
            }
            else
            {
                zwischenkontakt.geschlecht = 0;
            }

            //Einfügen des Zwischenkontakts in die Kontaktliste
            kontakte.Add(zwischenkontakt);

            //Speicherung der Kontakttaten
            
            using (FileStream kontaktStreamz = new FileStream(@"C:\Users\felix oehm\Desktop\C# Referenzen\WPF Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\kontakte.txt", FileMode.Open, FileAccess.Write))
            {
            saveFormater.Serialize(kontaktStreamz, kontakte);
            }
            

            //Erzeigen eines KontaktButtons
            Button zwischenButton = new Button();
            zwischenButton.Content = zwischenkontakt.vorname + " " + zwischenkontakt.nachname;
            zwischenButton.Click += KontaktButton_Click;

            //Zuweisen des KontaktButtons der Liste
            kontaktButton.Add(zwischenButton);
            kontaktPanel.Children.Add(kontaktButton[zwischenkontakt.kontaktNummer]);

            deleteCount = zwischenkontakt.kontaktNummer;
            deleteButton.IsEnabled = true;
        }

        //Aktiviere SpeicherButton
        private void GeschlechtRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            safeButton.IsEnabled = true;
        }

       //Neuer Kontakt
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            vornameTextBox.Text = "";
            nachnameTextBox.Text = "";
            mRadioButton.IsChecked = false;
            wRadioButton.IsChecked = false;
            safeButton.IsEnabled = false;
            deleteButton.IsEnabled = false;
        }

        //Laden eines Kontaktes aus einem KontaktButton
        private void KontaktButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < kontaktButton.Count; i++)
            {
                if (sender == kontaktButton[i])
                {
                    LoadKontakt(i);
                }
            }
        }

        //LoadKontakt methode
        public void LoadKontakt(int index)
        {
            vornameTextBox.Text = kontakte[index].vorname;
            nachnameTextBox.Text = kontakte[index].nachname;
            if(kontakte[index].geschlecht == 1)
            {
                mRadioButton.IsChecked = true;
                wRadioButton.IsChecked = false;
            }
            else if(kontakte[index].geschlecht == 2)
            {
                mRadioButton.IsChecked = false;
                wRadioButton.IsChecked = true;
            }
            else
            {
                mRadioButton.IsChecked = false;
                wRadioButton.IsChecked = false;
            }

            deleteCount = index;
            deleteButton.IsEnabled = true;
        }

        //Kontakte löschen
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(deleteCount);
            kontaktPanel.Children.Remove(kontaktButton[deleteCount]);
            kontaktButton.Remove(kontaktButton[deleteCount]);
            kontakte.Remove(kontakte[deleteCount]);

            if (deleteCount == 0)
            {
                deleteButton.IsEnabled = false;
            }
        }
    }

    //Kontakt Klasse
    [Serializable]
    public class Kontakt
    {
        public int kontaktNummer { get; set; }
        public string vorname { get; set; }
        public string nachname { get; set; }
        public byte geschlecht { get; set; }
    }

    
}
