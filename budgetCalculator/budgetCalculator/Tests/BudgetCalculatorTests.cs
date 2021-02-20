using System;
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
        public void query_jan_month_should_return_jan_budget()
        {
            GivenBudgetList(new Budget
            {
                Amount = 31,
                YearMonth = "202101"
            });

            var startDate = new DateTime(2021, 01, 01);
            var endDate = new DateTime(2021, 01, 31);
            var budgetAmount = _budgetCalculator.Query(startDate, endDate);

            Assert.AreEqual(31, budgetAmount);
        }


        [Test]
        public void query_two_full_month_should_return_two_month_budget()
        {
            GivenBudgetList(new Budget
            {
                Amount = 31,
                YearMonth = "202101"
            }, new Budget
            {
                Amount = 28,
                YearMonth = "202102"
            });

            var startDate = new DateTime(2021, 01, 01);
            var endDate = new DateTime(2021, 02, 28);
            var budgetAmount = _budgetCalculator.Query(startDate, endDate);

            Assert.AreEqual(59, budgetAmount);
        }

        [Test]
        public void query_cross_month_should()
        {
            GivenBudgetList(new Budget
            {
                Amount = 31,
                YearMonth = "202101"
            }, new Budget
            {
                Amount = 28,
                YearMonth = "202102"
            });

            var startDate = new DateTime(2021, 01, 31);
            var endDate = new DateTime(2021, 02, 3);
            var budgetAmount = _budgetCalculator.Query(startDate, endDate);

            Assert.AreEqual(4, budgetAmount);
        }

        [Test]
        public void query_cross_year_month_should()
        {
            GivenBudgetList(new Budget()
            {
                Amount = 31,
                YearMonth = "202012"
            }, new Budget()
            {
                Amount = 62,
                YearMonth = "202101"
            });

            var startDate = new DateTime(2020, 12, 31);
            var endDate = new DateTime(2021, 01, 1);
            var budgetAmount = _budgetCalculator.Query(startDate, endDate);

            Assert.AreEqual(3, budgetAmount);
        }

        [Test]
        public void query_lack_of_data_should()
        {
            GivenBudgetList(new Budget()
            {
                Amount = 31,
                YearMonth = "202012"
            });

            var startDate = new DateTime(2020, 12, 31);
            var endDate = new DateTime(2021, 01, 1);
            var budgetAmount = _budgetCalculator.Query(startDate, endDate);

            Assert.AreEqual(1, budgetAmount);
        }


        [Test]
        public void query_cross_year_should()
        {
            GivenBudgetList(new Budget
            {
                Amount = 31,
                YearMonth = "202101"
            }, new Budget
            {
                Amount = 28,
                YearMonth = "202102"
            }, new Budget
            {
                Amount = 31,
                YearMonth = "202201"
            });

            var startDate = new DateTime(2021, 01, 1);
            var endDate = new DateTime(2022, 01, 31);
            var budgetAmount = _budgetCalculator.Query(startDate, endDate);

            Assert.AreEqual(90, budgetAmount);
        }

        [Test]
        public void should_be_zero_when_start_day_greater_than_end_day()
        {
            GivenBudgetList(new Budget
            {
                Amount = 31,
                YearMonth = "202101"
            }, new Budget
            {
                Amount = 28,
                YearMonth = "202102"
            });

            var startDate = new DateTime(2021, 01, 31);
            var endDate = new DateTime(2021, 01, 1);

            var budgetAmount = _budgetCalculator.Query(startDate, endDate);

            Assert.AreEqual(0, budgetAmount);
        }

        private void GivenBudgetList(params Budget[] budgetList)
        {
            _budgetRepo.GetAll().Returns(budgetList.ToList());
        }
    }
}
