using System.Collections.Generic;
using budgetCalculator.Models;

namespace budgetCalculator.Interface
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}