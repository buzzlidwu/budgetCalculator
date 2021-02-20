using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace budgetCalculator
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

    public class BudgetRepo : IBudgetRepo
    {
        public List<Budget> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }

    public class BudgetCalculator
    {
    }
}