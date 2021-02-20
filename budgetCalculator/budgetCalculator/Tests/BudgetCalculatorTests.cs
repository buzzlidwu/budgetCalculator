using System.Collections.Generic;
using budgetCalculator.Interface;
using budgetCalculator.Models;
using NSubstitute;
using NUnit.Framework;

namespace budgetCalculator.Tests
{
    public class BudgetCalculatorTests
    {
        private BudgetCalculator _budgetCalculator;
        private readonly IBudgetRepo _budgetRepo;

        public BudgetCalculatorTests(IBudgetRepo budgetRepo)
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
        }

        [SetUp]
        public void SetUp()
        {
            _budgetCalculator = new BudgetCalculator();
        }
        [Test]
        public void First_test()
        {
            var budgetList = new List<Budget>
            {
                new Budget()
                {
                    Amount = 31,
                    YearMonth = "202101"
                }
            };
            GivenBudgetList(budgetList);
        }

        private void GivenBudgetList(List<Budget> returnThis)
        {
            _budgetRepo.GetAll().Returns(returnThis);
        }
    }
}