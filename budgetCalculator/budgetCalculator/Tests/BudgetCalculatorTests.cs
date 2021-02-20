using System;
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
        public void query_jan_month_should_return_jan_budget()
        {
            GivenBudgetList(new Budget()
            {
                Amount = 31,
                YearMonth = "202101"
            });

            var endDate = new DateTime(2021, 01, 01);
            var startDate = new DateTime(2021, 01, 31);
            var budgetAmount = _budgetCalculator.Query(endDate, startDate);

            Assert.AreEqual(31, budgetAmount);
        }


        [Test]
        public void query_two_full_month_should_return_two_month_budget()
        {
            GivenBudgetList(new Budget()
            {
                Amount = 31,
                YearMonth = "202101"
            }, new Budget()
            {
                Amount = 28,
                YearMonth = "202102"
            });

            var endDate = new DateTime(2021, 01, 01);
            var startDate = new DateTime(2021, 01, 31);
            var budgetAmount = _budgetCalculator.Query(endDate, startDate);

            Assert.AreEqual(59, budgetAmount);
        }

        [Test]
        public void query_cross_month_should()
        {
            GivenBudgetList(new Budget()
            {
                Amount = 31,
                YearMonth = "202101"
            }, new Budget()
            {
                Amount = 28,
                YearMonth = "202102"
            });

            var endDate = new DateTime(2021, 01, 31);
            var startDate = new DateTime(2021, 01, 3);
            var budgetAmount = _budgetCalculator.Query(endDate, startDate);

            Assert.AreEqual(4, budgetAmount);
        }

        private void GivenBudgetList(params Budget[] budgetList)
        {
            _budgetRepo.GetAll().Returns(budgetList.ToList());
        }
    }
}