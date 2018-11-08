using NUnit.Framework;
using SGBank.BLL;
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
    public class FreeAccountTests
    {
        [Test]
        public void CanLoadFreeAccountTestData()
        {
            AccountManager manager = AccountManagerFactory.Create();

            AccountLookupResponse response = manager.LookupAccount("12345");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("12345", response.Account.AccountNumber);
        }

        [TestCase("12345", "Free Account", 100, AccountType.Free, 250, false)]
        [TestCase("12345", "Free Account", 100, AccountType.Free, -100, false)]
        [TestCase("12345", "Free Account", 100, AccountType.Basic, 50, false)]
        [TestCase("12345", "Free Account", 100, AccountType.Free, 50, true)]
        public void FreeAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit depositVar = new FreeAccountDepositRule();
            Account accountVar = new Account();

            accountVar.AccountNumber = accountNumber;
            accountVar.Name = name;
            accountVar.Balance = balance;
            accountVar.Type = accountType;

            AccountDepositResponse responseVar = depositVar.Deposit(accountVar, amount);

            Assert.AreEqual(expectedResult, responseVar.Success);
        }

        [TestCase("12345", "Free Account", 200, AccountType.Free, 50, false)]
        [TestCase("12345", "Free Account", 200, AccountType.Free, -150, false)]
        [TestCase("12345", "Free Account", 200, AccountType.Basic, -50, false)]
        [TestCase("12345", "Free Account", 50, AccountType.Free, -75, false)]
        [TestCase("12345", "Free Account", 200, AccountType.Free, -50, true)]
        public void FreeAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IWithdraw withdrawVar = new FreeAccountWithdrawRule();
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
