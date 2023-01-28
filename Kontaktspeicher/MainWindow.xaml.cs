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

    public partial class MainWindow : Window
    {
        List<Contact> contact = new List<Contact>();
        List<Button> contactButton = new List<Button>();
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
                using (FileStream ContactStream = new FileStream(@"D:\Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\kontakte.txt", FileMode.Open, FileAccess.Read))
                {
                    contact = (List<Contact>)saveFormater.Deserialize(ContactStream);
                    for (int i = 0; i < contact.Count; i++)
                    {
                        //generate new ContactButtons
                        Button BetweenButton = new Button();
                        BetweenButton.Content = contact[i].firstName + " " + contact[i].lastName;
                        BetweenButton.Click += ContactButton_Click;

                        //Zuweisen des KontaktButtons der Liste
                        contactButton.Add(BetweenButton);
                        ContactPanel.Children.Add(contactButton[contact[i].contactNumber]);
                    }
                }
            }
        }

        //save contact
        private void SafeButton_Click(object sender, RoutedEventArgs e)
        {
            //Erzeugen eines intermediateContact
            Contact intermediateContact = new Contact();

            //Zuweisen der KontaktNummer
            intermediateContact.contactNumber = contact.Count;

            //Zuweisen des Vornamens
            intermediateContact.firstName = FirstnameTextBox.Text;

            //Zuweisen des Nachnamens
            intermediateContact.lastName = LastnameTextBox.Text;

            //Zuweisen des Geschlechts
            if(MRadioButton.IsChecked == true)
            {
                intermediateContact.gender = 1;
            }
            else if(FRadioButton.IsChecked == true)
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
            contactButton.Add(zwischenButton);
            ContactPanel.Children.Add(contactButton[intermediateContact.contactNumber]);

            deleteCount = intermediateContact.contactNumber;
            ClearButton.IsEnabled = true;
        }

        //Aktiviere SpeicherButton
        private void GenderRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SafeButton.IsEnabled = true;
        }

       //new contact
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FirstnameTextBox.Text = "";
            LastnameTextBox.Text = "";
            MRadioButton.IsChecked = false;
            FRadioButton.IsChecked = false;
            SafeButton.IsEnabled = false;
            ClearButton.IsEnabled = false;
        }

        //load contact from ContactButton
        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < contactButton.Count; i++)
            {
                if (sender == contactButton[i])
                {
                    LoadContact(i);
                }
            }
        }

        //LoadContact methode
        public void LoadContact(int index)
        {
            FirstnameTextBox.Text = contact[index].firstName;
            LastnameTextBox.Text = contact[index].lastName;
            if(contact[index].gender == 1)
            {
                MRadioButton.IsChecked = true;
                FRadioButton.IsChecked = false;
            }
            else if(contact[index].gender == 2)
            {
                MRadioButton.IsChecked = false;
                FRadioButton.IsChecked = true;
            }
            else
            {
                MRadioButton.IsChecked = false;
                FRadioButton.IsChecked = false;
            }

            deleteCount = index;
            ClearButton.IsEnabled = true;
        }

        //Kontakte löschen
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("deleteCount: " + deleteCount);
            ContactPanel.Children.Remove(contactButton[deleteCount]);
            contactButton.Remove(contactButton[deleteCount]);
            contact.Remove(contact[deleteCount]);

            if (deleteCount > 0)
            {
                deleteCount--;
            }
            else
            {
                ClearButton.IsEnabled = false;
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
    public class Contact
    {
        public int contactNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public byte gender { get; set; }
    }

    
}
