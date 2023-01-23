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
        List<Kontakt> contact = new List<Kontakt>();
        List<Button> kontaktButton = new List<Button>();
        public int deleteCount { get; set; }
        BinaryFormatter saveFormater;

        public MainWindow()
        {
            InitializeComponent();

            string fileContent;

            using (StreamReader checkstream = new StreamReader(@"D:\Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\kontakte.txt"))
            {
                fileContent = checkstream.ReadToEnd();
            }

            saveFormater = new BinaryFormatter();
            if (fileContent != "")
            {
                using (FileStream kontaktStream = new FileStream(@"D:\Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\kontakte.txt", FileMode.Open, FileAccess.Read))
                {
                    contact = (List<Kontakt>)saveFormater.Deserialize(kontaktStream);
                    for (int i = 0; i < contact.Count; i++)
                    {
                        //Erzeigen eines KontaktButtons
                        Button zwischenButton = new Button();
                        zwischenButton.Content = contact[i].firstName + " " + contact[i].lastName;
                        zwischenButton.Click += ContactButton_Click;

                        //Zuweisen des KontaktButtons der Liste
                        kontaktButton.Add(zwischenButton);
                        kontaktPanel.Children.Add(kontaktButton[contact[i].contactNumber]);
                    }
                }
            }
        }

        //save kontakt
        private void SafeButton_Click(object sender, RoutedEventArgs e)
        {
            //Erzeugen eines intermediateContact
            Kontakt intermediateContact = new Kontakt();

            //Zuweisen der KontaktNummer
            intermediateContact.contactNumber = contact.Count;

            //Zuweisen des Vornamens
            intermediateContact.firstName = vornameTextBox.Text;

            //Zuweisen des Nachnamens
            intermediateContact.lastName = nachnameTextBox.Text;

            //Zuweisen des Geschlechts
            if(mRadioButton.IsChecked == true)
            {
                intermediateContact.gender = 1;
            }
            else if(wRadioButton.IsChecked == true)
            {
                intermediateContact.gender = 2;
            }
            else
            {
                intermediateContact.gender = 0;
            }

            //Einfügen des Zwischenkontakts in die Kontaktliste
            contact.Add(intermediateContact);

            //save contact data

            SaveContact();
            

            //Erzeigen eines KontaktButtons
            Button zwischenButton = new Button();
            zwischenButton.Content = intermediateContact.firstName + " " + intermediateContact.lastName;
            zwischenButton.Click += ContactButton_Click;

            //Zuweisen des KontaktButtons der Liste
            kontaktButton.Add(zwischenButton);
            kontaktPanel.Children.Add(kontaktButton[intermediateContact.contactNumber]);

            deleteCount = intermediateContact.contactNumber;
            deleteButton.IsEnabled = true;
        }

        //Aktiviere SpeicherButton
        private void GeschlechtRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            safeButton.IsEnabled = true;
        }

       //new contact
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            vornameTextBox.Text = "";
            nachnameTextBox.Text = "";
            mRadioButton.IsChecked = false;
            wRadioButton.IsChecked = false;
            safeButton.IsEnabled = false;
            deleteButton.IsEnabled = false;
        }

        //load contact from ContactButton
        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < kontaktButton.Count; i++)
            {
                if (sender == kontaktButton[i])
                {
                    LoadContact(i);
                }
            }
        }

        //LoadContact methode
        public void LoadContact(int index)
        {
            vornameTextBox.Text = contact[index].firstName;
            nachnameTextBox.Text = contact[index].lastName;
            if(contact[index].gender == 1)
            {
                mRadioButton.IsChecked = true;
                wRadioButton.IsChecked = false;
            }
            else if(contact[index].gender == 2)
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
            Console.WriteLine("deleteCount: " + deleteCount);
            kontaktPanel.Children.Remove(kontaktButton[deleteCount]);
            kontaktButton.Remove(kontaktButton[deleteCount]);
            contact.Remove(contact[deleteCount]);

            if (deleteCount > 0)
            {
                deleteCount--;
            }
            else
            {
                deleteButton.IsEnabled = false;
            }

            //save contact data

            SaveContact();
        }

        //save contact method
        public void SaveContact()
        {
            using (FileStream kontaktStreamz = new FileStream(@"D:\Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\kontakte.txt", FileMode.Open, FileAccess.Write))
            {
                saveFormater.Serialize(kontaktStreamz, contact);
            }
        }
    }

    //Kontakt Klasse
    [Serializable]
    public class Kontakt
    {
        public int contactNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public byte gender { get; set; }
    }

    
}
