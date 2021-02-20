using budgetCalculator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace budgetCalculator
{
    static class Helper
    {
        public static int DaysInMonth(this DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month);
        }
    }

    public class BudgetCalculator
    {
        IBudgetRepo dataSource;

        public BudgetCalculator(IBudgetRepo repo)
        {
            dataSource = repo;
        }

        class BudgetData
        {
            public DateTime DT { get; set; }
            public int Amount { get; set; }
        }

        class MonthData
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int DayInMonth { get; set; }
            public int Amount { get; set; }
            public int RealDay { get; set; }
        }

        Dictionary<string, int> dataDict = new Dictionary<string, int>();


        private decimal GetAmountInMonth(DateTime dt, int days)
        {
            var key = dt.ToString("yyyyMM");
            var amount = dataDict[key];
            var daysInMonth = dt.DaysInMonth();
            return amount * days / daysInMonth;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (start > end)
                return 0;

            foreach (var data in dataSource.GetAll())
                dataDict.Add(data.YearMonth, data.Amount);

            if (start.Year == end.Year && start.Month == end.Month)
            {
                return GetAmountInMonth(start, (end - start).Days + 1);
            }
            else
            {
                decimal amount = 0;

                amount += GetAmountInMonth(start, start.DaysInMonth() - start.Day + 1);
                amount += GetAmountInMonth(end, end.Day);

                var monthStart = start.AddDays(-start.Day + 1).AddMonths(1);
                var monthEnd = end.AddDays(end.DaysInMonth() - end.Day).AddMonths(-1);

                for (var m = monthStart; m <= monthEnd; m = m.AddMonths(1))
                    amount += GetAmountInMonth(m, m.DaysInMonth());

                return amount;
            }
        }
    }
}