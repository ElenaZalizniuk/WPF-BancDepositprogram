using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Wpf_OperDay
{
    [Serializable]
    public class CustomID:I_ID
    {
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public uint Taxnumber { get; set; }
        public string Passportseries { get; set; }
        public int Passportnumber { get; set; }
        public string Issuedby { get; set; }
        public string Issuedwhen { get; set; }
        public string DateofBirth { get; set; }
        public string Adress { get; set; }

        public CustomID()
        {
        }
        public CustomID(int id, string lastname, string name,  string patronymic, uint taxnumber, 
            string passpser, int passpnumb, string issuedby, string issuedwhen, string dateofbirth)
        {
            ID = id;
            Lastname = lastname;
            Name = name;
            Patronymic = patronymic;
            Taxnumber = taxnumber;
            Passportseries = passpser;
            Passportnumber = passpnumb;
            Issuedby = issuedby;
            Issuedwhen = issuedwhen;
            DateofBirth = dateofbirth;
        }


        private static ObservableCollection<CustomID> customers;

        public static ObservableCollection<CustomID> Customers
        {
            get
            {
                if (customers != null)
                { return customers; }

                customers = new ObservableCollection<CustomID>();
                return customers;
            }
        }

        
    }
}
