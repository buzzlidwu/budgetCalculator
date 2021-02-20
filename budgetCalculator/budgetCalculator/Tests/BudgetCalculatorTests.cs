using System.Collections.Generic;
using System.Linq;
using budgetCalculator.Interface;
using budgetCalculator.Models;
using NSubstitute;
using NUnit.Framework;

namespace budgetCalculator.Tests
{
    public class BudgetCalculatorTests
    {
        private BudgetCalculator _budgetCalculator;
        private IBudgetRepo _budgetRepo;

        [SetUp]
        public void SetUp()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetCalculator = new BudgetCalculator(_budgetRepo);
        }
        [Test]
        public void First_test()
        {
            GivenBudgetList(new Budget()
            {
                Amount = 31,
                YearMonth = "202101"
            });

        }

        private void GivenBudgetList(params Budget[] returnThis)
        {
            _budgetRepo.GetAll().Returns(returnThis.ToList());
        }
    }
}