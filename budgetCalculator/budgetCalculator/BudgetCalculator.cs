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
            public DateTime dateTime { get; set; }
            public int Amount { get; set; }
        }

        public decimal Query(DateTime start, DateTime end)
        {
            List<string> strs = new List<string>();
            strs.Add(start.ToString("YYYYMM"));
            strs.Add(end.ToString("YYYYMM"));

            data_source.GetAll().Where(x => strs.Contains(x.YearMonth));
            return 0;
        }
    }
}