using System;
using System.Collections.Generic;
using budgetCalculator.Interface;

namespace budgetCalculator
{
    public class BudgetCalculator
    {
        IBudgetRepo data_source;

        public BudgetCalculator(IBudgetRepo repo)
        {
            data_source = repo;
        }

        class BudgetData
        {
            public DateTime DT { get; set; }
            public int Amount { get; set; }
        }

        public int Query(DateTime start, DateTime end)
        {
            var monthDetail = new List<MonthDetail>();
            var monthRange = getMonthRange(start, end);
       
            for (int i = 1; i <= monthRange; i++)
            {
                DateTime currentMonthDate;
                if (i == 1)
                    currentMonthDate = start;
                else if (i == monthRange)
                    currentMonthDate = end;
                else
                    currentMonthDate = start.AddMonths(i);

                var detail = new MonthDetail
                {
                    Year = currentMonthDate.Year,
                    Month = currentMonthDate.Month,
                    OuterDays = GetOuterDays(i, monthRange, currentMonthDate),
                    TotalDays = DateTime.DaysInMonth(currentMonthDate.Year, currentMonthDate.Month)
                };
                monthDetail.Add(detail);
            }

            var budgets = data_source.GetAll();
            int total = 0;
            foreach (var month in monthDetail)
            {
                var currentMonthsBudget = budgets.Find(budget =>
                {
                    var time = DateTime.ParseExact($"{budget.YearMonth}01", "yyyyMM01", null);
                    return time.Year == month.Year && time.Month == month.Month;
                });

                if (currentMonthsBudget != null)
                {
                    if (month.OuterDays == 0)
                        total += currentMonthsBudget.Amount;
                    else
                        total += currentMonthsBudget.Amount -
                                 month.OuterDays * (currentMonthsBudget.Amount / month.TotalDays);
                }
            }

            return total;
        }

        private int GetOuterDays(int idx, int monthRange, DateTime currentMonth)
        {
            if (idx == 1) return MathStartOuterDays(currentMonth);
            return idx == monthRange ? MathEndOuterDays(currentMonth) : DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
        }

        private int getMonthRange(DateTime start, DateTime end)
        {
            int monthRange;
            if (start.Year == end.Year)
            {
                monthRange = end.Month - start.Month + 1;
            }
            else
            {
                var startMonth = 12 - start.Month + 1;
                monthRange = startMonth + end.Month;
            }

            return monthRange;
        }

        private int MathStartOuterDays(DateTime date)
        {
            return date.Day - 1;
        }
        private int MathEndOuterDays(DateTime date)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return daysInMonth - date.Day;
        }
    }

    public class MonthDetail
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public string YearMonth => $"{Year}{Month:00}";
        public int TotalDays { get; set; }
        public int OuterDays { get; set; }
    }
}