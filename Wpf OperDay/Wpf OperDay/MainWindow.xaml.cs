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
using System.IO;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Wpf_OperDay
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);
        string PathID = "AllID.xml";
        string PathAccount = "Account.xml";
        string PathDeposit = "Deposit.xml";
        /// <summary>
        /// field for serialization of the objects
        /// </summary>
        XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<CustomID>));
        XmlSerializer formaccount = new XmlSerializer(typeof(ObservableCollection<Account>));
        XmlSerializer formdepo = new XmlSerializer(typeof(ObservableCollection<Deposit>));

        object back = null;
        object backacc = null;
        object backdepo = null;

        public MainWindow()
        {
            InitializeComponent();
            tbTax.PreviewTextInput += TbTax_PreviewTextInput;
        }
        /// <summary>
        /// deserialization all info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.btLoadall_Click(sender, e);
            this.btLoadAcc_Click(sender, e);
            this.btLoadDep_Click(sender, e);
        }
        /// <summary>
        /// for inputting only digits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbTax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }
        /// <summary>
        /// button event to add new custom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbLastname.Text) || string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbTax.Text) ||
                    string.IsNullOrWhiteSpace(tbSer.Text) || string.IsNullOrWhiteSpace(tbNumb.Text) || string.IsNullOrWhiteSpace(tbissuedby.Text) ||
                    string.IsNullOrWhiteSpace(dpissuedwhen.Text) || string.IsNullOrWhiteSpace(tbAdr.Text) || string.IsNullOrWhiteSpace(dpBirstd.Text)
                    )
                {
                    MessageBox.Show("Введены не все данные");
                    return;
                }
                else
                {///create new custom with all needed properties
                    CustomID custom = new CustomID();
                    custom.ID = CustomID.Customers.Max(x => x.ID) + 1;//create next number ID
                    custom.Lastname = tbLastname.Text;
                    custom.Name = tbName.Text;
                    custom.Patronymic = tbPatr.Text;
                    var tax = uint.Parse(tbTax.Text);
                    
                    var check = CustomID.Customers.Any(x => x.Taxnumber == tax);//checking if consists Taxnumber
                    if (check)
                    {
                        MessageBox.Show("Клиент с таким ИНН уже существует!");
                        return;
                    }
                    else
                        custom.Taxnumber = tax;

                    custom.Passportseries = tbSer.Text;
                    custom.Passportnumber = int.Parse(tbNumb.Text);
                    custom.Issuedby = tbissuedby.Text;
                    custom.Issuedwhen = dpissuedwhen.Text;
                    custom.DateofBirth = dpBirstd.Text;
                    custom.Adress = tbAdr.Text;
                    CustomID.Customers.Add(custom);
                    tabHeadID.IsSelected = true;
                    MessageBox.Show("Проверьте введенные данные и сохраните!");
                }
            }
            catch (FormatException) { MessageBox.Show("Пожалуйста, проверьте правильность ввода данных!"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        /// <summary>
        /// button event to open new customers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOpenID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tabAddHeadID.IsSelected = true;
                btAdd_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// button`s event for serialization created objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(PathID, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, CustomID.Customers);

                    MessageBox.Show("Данные сохранены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// button`s event for deserialization data & loading to grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLoadall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(PathID, FileMode.OpenOrCreate))
                {
                    var newcustom = ((ObservableCollection<CustomID>)formatter.Deserialize(fs));

                    foreach (var i in newcustom)
                    {
                        CustomID.Customers.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        /// <summary>
        /// event button to open an account for the selected customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOpenAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var i = this.dgCustom.SelectedItem as CustomID;
                tabHeadOpenacc.IsSelected = true;
                if (i == null)
                {
                    MessageBox.Show("Введите ID и балансовый номер!");
                    return;
                }
                else
                {
                    tbIDac.Text = i.ID.ToString();
                    btAddAcc_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// button event for adding new account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbIDac.Text) || string.IsNullOrEmpty(tbBal.Text))
                {
                    MessageBox.Show("Введите ID и балансовый номер счета!");
                    return;
                }
               
                var check = CustomID.Customers.Any(x => x.ID == int.Parse(tbIDac.Text));//checking if consists ID
                if (!check)
                {
                    MessageBox.Show("Отсутствует клиент с таким ID!");
                    return;
                }
               
                else
                {
                    Account acc = new Account();
                    acc.ID = int.Parse(tbIDac.Text);
                    tbBalII.Text = tbBal.Text;
                    acc.Bal = int.Parse(tbBal.Text);
                    tbKey.Text = rnd.Next(0, 9).ToString();
                    acc.Key = int.Parse(tbKey.Text);

                    ///this conditions are needed for the first client:
                    ///
                    //var tail = rnd.Next(100000001, 100000002);
                    //foreach (var i in Account.AllAccount)
                    //{
                    //    while (i.Tail == tail)
                    //    { tail++; }
                    //}
                    //tbTail.Text = tail.ToString();

                    tbTail.Text = (Account.AllAccount.Max(x => x.Tail) + 1).ToString();
                    acc.Tail = int.Parse(tbTail.Text);
                    if(acc.Bal==2638)
                    { acc.Balance = 0; }
                    else
                    acc.Balance = double.Parse(tbSum.Text);
                    acc.Contr = int.Parse(txblDep.Text);
                    Account.AllAccount.Add(acc);

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// button event to serialization the accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream fa = new FileStream(PathAccount, FileMode.OpenOrCreate))
                {
                    formaccount.Serialize(fa, Account.AllAccount);

                    MessageBox.Show("Данные счетов сохранены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// button event to load all accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLoadAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream fa = new FileStream(PathAccount, FileMode.OpenOrCreate))
                {
                    var newaccount = ((ObservableCollection<Account>)formaccount.Deserialize(fa));

                    foreach (var i in newaccount)
                    {
                        Account.AllAccount.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// button event for adding new deposit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdddeposit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(tbIDdep.Text) || string.IsNullOrEmpty(tbSum.Text))
                {
                    MessageBox.Show("Введите ID и сумму договора!");
                    return;
                }
                var check = CustomID.Customers.Any(x => x.ID == int.Parse(tbIDdep.Text));//checking if consists ID
                if (!check)
                {
                    MessageBox.Show("Отсутствует клиент с таким ID!");
                    return;
                }
                else
                {
                    Deposit deposit = new Deposit();
                    {
                        deposit.ID = int.Parse(tbIDdep.Text);

                        ///this conditions are needed for the first client:
                        //var numb = rnd.Next(101, 102);
                        //foreach (var i in Deposit.AllDeposit)
                        //{
                        //    while (i.NumberContr == numb)
                        //    { numb++; }
                        //}

                        deposit.NumberContr = Deposit.AllDeposit.Max(x => x.ID) + 1;
                        tbNumbcontr.Text = deposit.NumberContr.ToString();
                        txblDep.Text = tbNumbcontr.Text;
                        
                        deposit.SumContr = double.Parse(tbSum.Text);
                        if (cb1m.IsSelected)
                        {
                            deposit.Rate = 11;
                            deposit.Deadline = 30;
                        }
                        else if (cb2m.IsSelected)
                        {
                            deposit.Rate = 12;
                            deposit.Deadline = 60;
                        }
                        else if (cb3m.IsSelected)
                        {
                            deposit.Rate = 13;
                            deposit.Deadline = 90;
                        }
                        deposit.DataContr = DateTime.Now;
                        tbDatacontr.Text = DateTime.Now.ToString();
                        deposit.DataContr = DateTime.Now;
                        deposit.DataEnd = deposit.DataContr.AddDays(deposit.Deadline);
                        tbDataend.Text = deposit.DataEnd.ToString();

                        tbIDac.Text = deposit.ID.ToString();
                        deposit.Balance = deposit.SumContr;
                        tbBal.Text = 2630.ToString();
                        btAddAcc_Click(sender, e);
                        deposit.Bal = int.Parse(tbBal.Text);
                        deposit.Key = int.Parse(tbKey.Text);
                        deposit.Tail = int.Parse(tbTail.Text);
                        

                        tbBal.Text = 2638.ToString();
                        btAddAcc_Click(sender, e);
                        deposit.BalPerc = 0;
                        deposit.Balance = 0;
                        deposit.Bal = int.Parse(tbBal.Text);
                        deposit.Key = int.Parse(tbKey.Text);
                        deposit.Tail = int.Parse(tbTail.Text);
                        deposit.Contr = int.Parse(txblDep.Text);
                    }
                   

                    Deposit.AllDeposit.Add(deposit);
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// button event to serialization all deposits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSavedeposit_Click(object sender, RoutedEventArgs e)
        {
            this.btSaveAcc_Click(sender, e);
            try
            {
                using (FileStream fd = new FileStream(PathDeposit, FileMode.OpenOrCreate))
                {
                    formdepo.Serialize(fd, Deposit.AllDeposit);

                    MessageBox.Show("Данные договоров сохранены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// button event to load all deposits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLoadDep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream fd = new FileStream(PathDeposit, FileMode.OpenOrCreate))
                {
                    var newdepo = ((ObservableCollection<Deposit>)formdepo.Deserialize(fd));

                    foreach (var i in newdepo)
                    {
                        Deposit.AllDeposit.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
       
        /// <summary>
        /// button event to open new deposit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOpenDepo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var i = this.dgCustom.SelectedItem as CustomID;
                tabHeadAddDepo.IsSelected = true;
                if (i == null)
                {
                    MessageBox.Show("Введите ID и сумму договора!");
                    return;
                }
                else
                {                   
                    tbIDdep.Text = i.ID.ToString();
                    btAdddeposit_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// button event for search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.back = dgCustom.ItemsSource;//save the source data

                //var search = CustomID.Customers.Where(x => (x.ID == int.Parse(tbSearch.Text)) ||
                //(x.Taxnumber == uint.Parse(tbSearch.Text)));
                //dgCustom.ItemsSource = search;

                var searchName = CustomID.Customers.Where(x => x.Lastname == tbSearch.Text);
                dgCustom.ItemsSource = searchName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// event to search the account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearchAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.backacc = dgAccount.ItemsSource;
                var search = Account.AllAccount.Where(x => x.ID == int.Parse(tbSearchAcc.Text));
                dgAccount.ItemsSource = search;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// event to search the deposit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearchDepo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.backdepo = dgDeposit.ItemsSource;
                var search = Deposit.AllDeposit.Where(x => (x.ID == int.Parse(tbSearchDepo.Text)));
                dgDeposit.ItemsSource = search;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// event to refresh data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dgCustom.ItemsSource = this.back as IEnumerable<CustomID>;//return the origanal data CustomIDcollection

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// event to refresh data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRefreshAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dgAccount.ItemsSource = this.backacc as IEnumerable<Account>;//return the origanal data Accountcollection
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// event to refresh data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRefreshDepo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dgDeposit.ItemsSource = this.backdepo as IEnumerable<Deposit>;//return the origanal data Depositcollection
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// about programm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }
        /// <summary>
        /// event menu all customers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
        private void MenuallID_Click(object sender, RoutedEventArgs e)
        {
            this.tabHeadID.IsSelected = true;
        }
        /// <summary>
        /// menu all accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuallAcc_Click(object sender, RoutedEventArgs e)
        {
            this.tabHeadAllAcc.IsSelected = true;
        }
        /// <summary>
        /// menu all deposits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuallDepo_Click(object sender, RoutedEventArgs e)
        {
            this.tabHeadAllDepo.IsSelected = true;
        }
        /// <summary>
        /// menu to close programm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// calculate interest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCalc_Click(object sender, RoutedEventArgs e)
        {        
            foreach (var i in Deposit.AllDeposit)
            {
                try
                {
                    if (i.DataEnd != DateTime.Today)
                    {
                        i.BalPerc += Math.Round((i.SumContr * i.Rate / 36000), 2);
                        MessageBox.Show("Начислено");
                    }
                    else
                        i.BalPerc += 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
