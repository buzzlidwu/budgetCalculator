using budgetCalculator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

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

            monthDetail.Add(new MonthDetail()
            {
                Year = start.Year,
                Month = start.Month,
                TotalDays = MathStartDateMonthDays(start)
            });

            for (int i = 1; i < monthRange; i++)
            {
                var currentMonth = start.AddMonths(i);
                monthDetail.Add(new MonthDetail()
                {
                    Year = currentMonth.Year,
                    Month = currentMonth.Month,
                    TotalDays = MathStartDateMonthDays(currentMonth)
                });
            }

            monthDetail.Add(new MonthDetail()
            {
                Year = end.Year,
                Month = end.Month,
                TotalDays = end.Day
            });

            var budgets = data_source.GetAll();
            var total = 0;
            foreach (var month in monthDetail)
            {
                var currentMonthsBudget = budgets.Find(budget =>
                 {
                     var time = Convert.ToDateTime(budget.YearMonth);
                     return time.Year == month.Year && time.Month == month.Month;
                 });

                //if(
            }


            //int nStart = Convert.ToInt16(start.ToString("yyyyMM"));
            //int nEnd = Convert.ToInt16(end.ToString("yyyyMM"));
            //data_source.GetAll().Where()
            //foreach (var d in data_source.GetAll())
            //{
            //    BudgetData bd = new BudgetData() {
            //        DT = DateTime.ParseExact(d.YearMonth, "yyyyMM", null),
            //        Amount = d.Amount };
            //    dataList.Add(bd);

            //    //DateTime.DaysInMonth
            //}
            //var diff = (end - start).TotalDays();

            return 0;
        }

        private static int getMonthRange(DateTime start, DateTime end)
        {
            int monthRange;
            if (start.Year == end.Year)
            {
                monthRange = end.Month - start.Month - 1;
            }
            else
            {
                var startMonth = 12 - start.Month;
                monthRange = startMonth + end.Month - 1;
            }

            return monthRange;
        }

        private int MathStartDateMonthDays(DateTime start)
        {
            var dataList = new List<BudgetData>();
            var startDateMonthDays = DateTime.DaysInMonth(start.Year, start.Month);
            return startDateMonthDays - start.Day + 1;
        }
    }

    public class MonthDetail
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalDays { get; set; }
    }
}