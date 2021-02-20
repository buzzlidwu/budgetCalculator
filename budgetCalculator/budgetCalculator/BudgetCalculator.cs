using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using budgetCalculator.Interface;
using budgetCalculator.Models;

namespace budgetCalculator
{
    public class BudgetCalculator
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetCalculator(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public int Query(DateTime start, DateTime end)
        {
            if (start > end) return 0;

            var allBudget = _budgetRepo.GetAll();

            if (allBudget == null) return 0;

            if (start.Year == end.Year && start.Month == end.Month) return GetFirstMonthAmount(start, allBudget);

            return GetFirstMonthAmount(start, allBudget) + GetMidMonthTotalAmount(start, end, allBudget) +
                GetLastMonthTotalAmount(end, allBudget);
        }

        private int GetLastMonthTotalAmount(DateTime end, IEnumerable<Budget> allBudget)
        {
            return CalculationBudgetByDays(GetBudgetAmountByYearAndMonth(end, allBudget), GetMonthDaysByTime(end),
                end.Day);
        }

        private int GetFirstMonthAmount(DateTime start, IEnumerable<Budget> allBudget)
        {
            var firstMonthTotalDays = GetMonthDaysByTime(start);
            var needCalculationDays = firstMonthTotalDays - start.Day + 1;

            return CalculationBudgetByDays(GetBudgetAmountByYearAndMonth(start, allBudget), firstMonthTotalDays,
                needCalculationDays);
        }

        private int GetMidMonthTotalAmount(DateTime start, DateTime end, IReadOnlyCollection<Budget> allBudget)
        {
            var amount = 0;
            var endTime = new DateTime(end.Year, end.Month, 1).AddSeconds(-1);

            for (var date = start.AddMonths(1); date <= endTime; date = date.AddMonths(1))
                amount += GetBudgetAmountByYearAndMonth(date, allBudget);

            return amount;
        }


        private int GetMonthDaysByTime(DateTime time)
        {
            return DateTime.DaysInMonth(time.Year, time.Month);
        }

        private int GetBudgetAmountByYearAndMonth(DateTime searchTime, IEnumerable<Budget> allBudget)
        {
            return allBudget.FirstOrDefault(budget =>
            {
                var budgetTime = DateTime.ParseExact(budget.YearMonth, "yyyyMM", CultureInfo.CurrentCulture);
                return searchTime.Year == budgetTime.Year && searchTime.Month == budgetTime.Month;
            })?.Amount ?? 0;
        }


        private int CalculationBudgetByDays(int amount, int monthTotalDays, int calculationDays)
        {
            return amount / monthTotalDays * calculationDays;
        }
    }
}