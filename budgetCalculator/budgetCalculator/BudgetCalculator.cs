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
            var dataList = new List<BudgetData>();
            foreach (var d in data_source.GetAll())
            {
                BudgetData bd = new BudgetData() {
                    DT = DateTime.ParseExact(d.YearMonth, "yyyyMM", null),
                    Amount = d.Amount };
                dataList.Add(bd);

                //DateTime.DaysInMonth
            }
            var diff = (end - start).TotalDays();

            return 0;
        }
    }
}