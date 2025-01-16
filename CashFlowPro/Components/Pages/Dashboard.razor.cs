using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;
using CashFlowPro.Model;
using CashFlowPro.Services;

namespace CashFlowPro.Components.Pages
{
    public partial class Dashboard
    {
        private readonly DashboardService dashboardService = new();

        private decimal totalIncome;
        private decimal totalExpense;
        private decimal totalDebt;
        private decimal availableBalance;
        private string selectedChartType = "line";
        private List<ChartSeries> chartSeries = new();
        private string[] chartLabels = Array.Empty<string>();
        private List<ChartData> chartData = new();

        private DateTime? startDateDebts = DateTime.Now.AddMonths(-1);
        private DateTime? endDateDebts = DateTime.Now;

        private DateTime? startDateTransactions = DateTime.Now.AddMonths(-1);
        private DateTime? endDateTransactions = DateTime.Now;

        private string debtStatusFilter = "";

        private List<Transaction> transactions = new();
        private List<Debt> debts = new();

        private List<Debt> filteredDebts = new();
        private List<Transaction> filteredTransactions = new();

        private Transaction? highlightedTransaction;
        private Debt? highlightedDebt;
        private List<Debt> top5HighestDebts; 
     private List<Debt> top5LowestDebts; 
     private List<Transaction> top5HighestTransactions; 
     private List<Transaction> top5LowestTransactions; 

        private string transactionsFilePath = @"C:\Users\ashuv\AppDevCW1\CashFlowPro\CashFlowPro\CashFlowPro\wwwroot\transaction.json";
        private string debtsFilePath = @"C:\Users\ashuv\AppDevCW1\CashFlowPro\CashFlowPro\CashFlowPro\wwwroot\debt.json";

        protected override void OnInitialized()
        {
            transactions = dashboardService.LoadTransactions(transactionsFilePath);
            debts = dashboardService.LoadDebts(debtsFilePath);

            CalculateTotals();
            CalculateAvailableBalance();
            FilterDebts();
            FilterTransactions();
            PrepareChartData();
        }

        private void CalculateTotals()
        {
            totalIncome = dashboardService.CalculateTotalIncome(transactions);
            totalExpense = dashboardService.CalculateTotalExpense(transactions);
            totalDebt = dashboardService.CalculateTotalDebt(debts);
        }

        private void CalculateAvailableBalance()
        {
            availableBalance = totalIncome + totalDebt - totalExpense;
        }

        private void FilterDebts()
        {
            filteredDebts = dashboardService.FilterDebts(debts, startDateDebts, endDateDebts, debtStatusFilter);
            StateHasChanged();
        }

        private void FilterTransactions()
        {
            filteredTransactions = dashboardService.FilterTransactions(transactions, startDateTransactions, endDateTransactions);
        }

        private void ClearDebtsFilters()
        {
            startDateDebts = null;
            endDateDebts = null;
            debtStatusFilter = "";
            FilterDebts();
        }

        private void ClearTransactionsFilters()
        {
            startDateTransactions = null;
            endDateTransactions = null;
            FilterTransactions();
        }

        private void ShowTop5HighestDebts() 
    { 
         top5HighestDebts = debts.OrderByDescending(d => d.Amount).Take(5).ToList(); 
        filteredDebts = top5HighestDebts; 
        StateHasChanged(); 
    } 

    private void ShowTop5LowestDebts() 
     { 
        top5LowestDebts = debts.OrderBy(d => d.Amount).Take(5).ToList(); 
        filteredDebts = top5LowestDebts; 
         StateHasChanged(); 
     } 

     private void ShowTop5HighestTransactions() 
     { 
         top5HighestTransactions = transactions.OrderByDescending(t => t.Amount).Take(5).ToList(); 
         filteredTransactions = top5HighestTransactions; 
         StateHasChanged(); 
     } 

     private void ShowTop5LowestTransactions() 
     { 
         top5LowestTransactions = transactions.OrderBy(t => t.Amount).Take(5).ToList(); 
         filteredTransactions = top5LowestTransactions; 
         StateHasChanged(); 
     } 


        private void PrepareChartData()
        {
            var groupedData = dashboardService.PrepareChartData(transactions, debts);
            chartData = groupedData;
            chartLabels = groupedData.Select(d => d.Date.ToString("MMM dd")).ToArray();

            chartSeries = new List<ChartSeries>
        {
            new ChartSeries { Name = "Income", Data = groupedData.Select(d => (double)d.Income).ToArray() },
            new ChartSeries { Name = "Expense", Data = groupedData.Select(d => (double)d.Expense).ToArray() },
            new ChartSeries { Name = "Debt", Data = groupedData.Select(d => (double)d.Debt).ToArray() },
            new ChartSeries { Name = "Balance", Data = groupedData.Select(d => (double)d.Balance).ToArray() }
        };
        }
}
}
