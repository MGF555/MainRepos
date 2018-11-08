using SGBank.Models;
using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Data
{
    public class FileAccountRepository : IAccountRepository
    {

        private List<Account> _account = new List<Account>();
        
        public void GetData()
        {
            FileInfo accountReader = new FileInfo(@".\Accounts.txt");

            if (!File.Exists(@".\Accounts.txt"))
            {
                File.Create(@".\Accounts.txt");
            }
            using (TextReader T = accountReader.OpenText())
            {
                string line = T.ReadLine();
                while ((line = T.ReadLine()) != null)
                {
                    line = line.Replace("\"", "");
                    string[] column = line.Split(',');
                    Account a = new Account();
                    a.AccountNumber = column[0];
                    a.Name = column[1];
                    a.Balance = decimal.Parse(column[2]);
                    switch (column[3])
                    {
                        case ("F"):
                            a.Type = AccountType.Free;
                            break;
                        case ("B"):
                            a.Type = AccountType.Basic;
                            break;
                        case ("P"):
                            a.Type = AccountType.Premium;
                            break;
                    }

                    _account.Add(a);
                }
            }
        }

        public Account LoadAccount(string accountNumber)
        {
            GetData();
            return _account.SingleOrDefault(x => x.AccountNumber == accountNumber);
        }

        public void SaveAccount(Account account)
        {
            string old = account.AccountNumber;

            if (File.Exists(@".\Accounts.txt"))
            {
                File.Delete(@".\Accounts.txt");
            }

            using (StreamWriter accountWriter = new StreamWriter(@".\Accounts.txt"))
            {
                accountWriter.WriteLine("AccountNumber,Name,Balance,Type");
                foreach (var y in _account)
                {
                    string z = null;
                    switch (y.Type)
                    {
                        case (AccountType.Free):
                            z = "F";
                            break;
                        case (AccountType.Basic):
                            z = "B";
                            break;
                        case (AccountType.Premium):
                            z = "P";
                            break;
                    }
                    accountWriter.WriteLine($"{y.AccountNumber},{y.Name},{y.Balance},{z}");
                }
            }
        }
    }
}
