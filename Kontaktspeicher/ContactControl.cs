using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kontaktspeicher
{
    internal class ContactControl
    {
        //method SaveContact
        public void SaveContact(BinaryFormatter _formatter, List<Contact> _contact, string _savefile)
        {
            using (FileStream KontaktStream = new FileStream(_savefile, FileMode.Open, FileAccess.Write))
            {
                _formatter.Serialize(KontaktStream, _contact);
            }
        }
    }
}
