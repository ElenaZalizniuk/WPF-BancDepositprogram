using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Wpf_OperDay
{
    [Serializable]
    public class Account : CustomID
    {
        public int Bal{get;set;}
        public int Key { get; set; }
        public int Tail { get; set; }
        public int Contr { get; set; }
        public double Balance { get; set; }

        public Account()
        { }
        public Account(int ID, int contr, int bal, int key, int tail, double balance)
        {
            Contr = contr;
            Bal = bal;
            Key = key;
            Tail = tail;
            Balance = balance;
        }
        private static ObservableCollection<Account> account;
        public static ObservableCollection<Account> AllAccount
        {
            get
            {
                if (account != null)
                    return account;
                account = new ObservableCollection<Account>();
                return account;
            }
        }
    }
}
