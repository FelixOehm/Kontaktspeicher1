using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics.Contracts;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace Kontaktspeicher
{
    internal class ContactControl
    {
        /*method LoadContact
        internal void LoadContact(List<Contact> _contacts, int index)
        {
            FirstnameTextBox.Text = _contacts[index].FirstName;
            LastnameTextBox.Text = _contacts[index].LastName;
            switch (_contacts[index].Gender)
            {
                case 1:
                    MRadioButton.IsChecked = true;
                    FRadioButton.IsChecked = false;
                    break;
                case 2:
                    MRadioButton.IsChecked = false;
                    FRadioButton.IsChecked = true;
                    break;
                default:
                    MRadioButton.IsChecked = false;
                    FRadioButton.IsChecked = false;
                    break;

            }
        }*/
        //method SaveContact
        internal void SaveContact(BinaryFormatter _formatter, List<Contact> _contacts, string _savefile)
        {
            using (FileStream KontaktStream = new FileStream(_savefile, FileMode.Open, FileAccess.Write))
            {
                _formatter.Serialize(KontaktStream, _contacts);
                Console.WriteLine(_savefile);
            }
        }

        internal string CheckFileContent(string _savefile)
        {
            string content;

            using (StreamReader checkstream = new StreamReader(_savefile))
            {
                content = checkstream.ReadToEnd();
            }
            return content;
        }
    }
}
