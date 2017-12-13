using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace Wpf_OperDay
{
    [Serializable]
    public class Deposit:Account
    {
        public int NumberContr { get; set; }
        public DateTime DataContr { get; set; }
        public DateTime DataEnd { get; set; }
        public double SumContr { get; set; }
        public double BalPerc { get; set; }
        public float Rate { get; set; }
        public int Deadline { get; set; }
        public Deposit()
        {           
        }

        public Deposit(int ID, int contr, int bal, int key, int tail, double balance,

            int numbcontr,
            DateTime datacontr,
            DateTime dataend,
            double sumcontr,
            double balperc,
            float rate,
            int deadline) : base(ID, contr, bal, key, tail, balance)
        {
            NumberContr = numbcontr;
            Deadline = deadline;
            DataContr = datacontr;
            DataEnd = dataend;
            SumContr = sumcontr;
            BalPerc = balperc;
            Rate = rate;

        }

        public Deposit(int ID,
            int numbcontr,
            DateTime datacontr,
            DateTime dataend,
            double sumcontr,
            float rate,
            int deadline)
        {
            NumberContr = numbcontr;
            Deadline = deadline;
            DataContr = datacontr;
            DataEnd = dataend;
            SumContr = sumcontr;
            Rate = rate;

        }

        private static ObservableCollection<Deposit> deposit;
        public static ObservableCollection<Deposit> AllDeposit
        {
            get
            {
                if (deposit != null)
                    return deposit;
                deposit = new ObservableCollection<Deposit>();
                return deposit;
            }
        }
    }
}
