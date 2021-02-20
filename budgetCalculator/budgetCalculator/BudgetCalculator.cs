using budgetCalculator.Interface;
using System;

namespace budgetCalculator
{
    public class BudgetCalculator
    {
        IBudgetRepo data_source;

        public BudgetCalculator(IBudgetRepo repo)
        {
            data_source = repo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            var data = data_source.GetAll();
            return 0;
        }
    }
}