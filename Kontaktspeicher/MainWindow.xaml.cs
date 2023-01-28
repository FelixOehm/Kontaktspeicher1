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
        string Savefile = @"D:\Projekte\Kontaktspeicher\Kontaktspeicher\txtdata\contacts.txt";

        ContactControl Controler = new ContactControl();

        List<Contact> Contacts = new List<Contact>();
        List<Button> ContactButton = new List<Button>();
        public int ClearCount;
        BinaryFormatter SaveFormater;

        public MainWindow()
        {
            InitializeComponent();
            
            SaveFormater = new BinaryFormatter();

            using (FileStream ContactStream = new FileStream(Savefile, FileMode.Open, FileAccess.Read))
            {
                Contacts = (List<Contact>)SaveFormater.Deserialize(ContactStream);
                for (int i = 0; i < Contacts.Count; i++)
                {
                    //generate new ContactButtons
                    Button BetweenButton = new Button();
                    BetweenButton.Content = Contacts[i].FirstName + " " + Contacts[i].LastName;
                    BetweenButton.Click += ContactButton_Click;

                    //Zuweisen des KontaktButtons der Liste
                    ContactButton.Add(BetweenButton);
                    ContactPanel.Children.Add(ContactButton[Contacts[i].ContactNumber]);
                }
            }
        }

        //SafeButton_Click
        private void SafeButton_Click(object sender, RoutedEventArgs e)
        {
            //Erzeugen eines intermediateContact
            Contact intermediateContact = new Contact();

            //Zuweisen der KontaktNummer
            intermediateContact.ContactNumber = Contacts.Count;

            //Zuweisen des Vornamens
            intermediateContact.FirstName = FirstnameTextBox.Text;

            //Zuweisen des Nachnamens
            intermediateContact.LastName = LastnameTextBox.Text;

            //Zuweisen des Geschlechts
            if(MRadioButton.IsChecked == true)
            {
                intermediateContact.Gender = 1;
            }
            else if(FRadioButton.IsChecked == true)
            {
                intermediateContact.Gender = 2;
            }
            else
            {
                intermediateContact.Gender = 0;
            }

            //Einfügen des Zwischenkontakts in die Kontaktliste
            Contacts.Add(intermediateContact);

            //SaveContact
            Controler.SaveContact(SaveFormater, Contacts, Savefile);


            //Erzeigen eines KontaktButtons
            Button zwischenButton = new Button();
            zwischenButton.Content = intermediateContact.FirstName + " " + intermediateContact.LastName;
            zwischenButton.Click += ContactButton_Click;

            //Zuweisen des KontaktButtons der Liste
            ContactButton.Add(zwischenButton);
            ContactPanel.Children.Add(ContactButton[intermediateContact.ContactNumber]);

            ClearCount = intermediateContact.ContactNumber;
            ClearButton.IsEnabled = true;
        }

        //Aktiviere SpeicherButton
        private void GenderRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SafeButton.IsEnabled = true;
        }

        //NewButton_Click
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FirstnameTextBox.Text = "";
            LastnameTextBox.Text = "";
            MRadioButton.IsChecked = false;
            FRadioButton.IsChecked = false;
            SafeButton.IsEnabled = false;
            ClearButton.IsEnabled = false;
        }

        //ContactButton_Click
        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < ContactButton.Count; i++)
            {
                if (sender == ContactButton[i])
                {
                    LoadContact(i);
                }
            }
        }

        //method LoadContact
        public void LoadContact(int index)
        {
            FirstnameTextBox.Text = Contacts[index].FirstName;
            LastnameTextBox.Text = Contacts[index].LastName;
            if(Contacts[index].Gender == 1)
            {
                MRadioButton.IsChecked = true;
                FRadioButton.IsChecked = false;
            }
            else if(Contacts[index].Gender == 2)
            {
                MRadioButton.IsChecked = false;
                FRadioButton.IsChecked = true;
            }
            else
            {
                MRadioButton.IsChecked = false;
                FRadioButton.IsChecked = false;
            }

            ClearCount = index;
            ClearButton.IsEnabled = true;
        }

        //clear Contact
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("deleteCount: " + ClearCount);
            ContactPanel.Children.Remove(ContactButton[ClearCount]);
            ContactButton.Remove(ContactButton[ClearCount]);
            Contacts.Remove(Contacts[ClearCount]);

            if (ClearCount > 0)
            {
                ClearCount--;
            }
            else
            {
                ClearButton.IsEnabled = false;
            }

            //SaveContact
            Controler.SaveContact(SaveFormater, Contacts, Savefile);
        }
    }
}
