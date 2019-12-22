using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cashflowio.Web.Models
{
    public class DashboardViewModel
    {
        public SelectList Years { get; set; }
        public int SelectedYear { get; set; }
        public List<IncomeSourceViewModel> Income { get; set; } = new List<IncomeSourceViewModel>();
        public List<MoneyAccountViewModel> MoneyAccounts { get; set; } = new List<MoneyAccountViewModel>();
        public List<ExpenseCategoryViewModel> ExpenseCategories { get; set; } = new List<ExpenseCategoryViewModel>();
    }

    public class IncomeSourceViewModel
    {
        public string Name { get; set; }
        public double TotalAmount { get; set; }
        public List<IncomeConceptViewModel> Concepts { get; set; } = new List<IncomeConceptViewModel>();
    }

    public class IncomeConceptViewModel
    {
        public string Description { get; set; }
        public double Amount { get; set; }
    }

    public class MoneyAccountViewModel
    {
        public string Name { get; set; }
        public double Balance { get; set; }
        public List<MoneyAccountConceptViewModel> Concepts { get; set; } = new List<MoneyAccountConceptViewModel>();
    }

    public class MoneyAccountConceptViewModel
    {
        public string Icon { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
    }

    public class ExpenseCategoryViewModel
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public List<ExpenseConceptViewModel> Concepts { get; set; } = new List<ExpenseConceptViewModel>();
    }

    public class ExpenseConceptViewModel
    {
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}