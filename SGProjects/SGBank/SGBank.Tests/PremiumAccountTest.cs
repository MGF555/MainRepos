using NUnit.Framework;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Tests
{
    [TestFixture]
    public class PremiumAccountTest
    {
        [TestCase("00000", "Premium Account", 200, AccountType.Premium, 200, true)]
        [TestCase("00000", "Premium Account", 200, AccountType.Free, 500, false)]
        [TestCase("00000", "Premium Account", 200, AccountType.Premium, -200, false)]
        [TestCase("00000", "Premium Account", 200, AccountType.Premium, 200, true)]
        public void PremiumAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit depositVar = new NoLimitDepositRule();
            Account accountVar = new Account();

            accountVar.AccountNumber = accountNumber;
            accountVar.Name = name;
            accountVar.Balance = balance;
            accountVar.Type = accountType;

            AccountDepositResponse responseVar = depositVar.Deposit(accountVar, amount);

            Assert.AreEqual(expectedResult, responseVar.Success);
        }
        [TestCase("00000", "Premium Account", 500, AccountType.Premium, -1100, -600, false)]
        [TestCase("00000", "Premium Account", 100, AccountType.Free, -100, 100, false)]
        [TestCase("00000", "Premium Account", 100, AccountType.Basic, 100, 100, false)]
        [TestCase("00000", "Premium Account", 150, AccountType.Premium, -50, 100, true)]
        [TestCase("00000", "Premium Account", 100, AccountType.Premium, -150, -60, true)]


        public void PremiumAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdraw withdrawVar = new PremiumAccountWithdrawRule();
            Account accountVar = new Account();

            accountVar.AccountNumber = accountNumber;
            accountVar.Name = name;
            accountVar.Balance = balance;
            accountVar.Type = accountType;

            AccountWithdrawResponse responseVar = withdrawVar.Withdraw(accountVar, amount);

            Assert.AreEqual(expectedResult, responseVar.Success);
        }


    }
}
