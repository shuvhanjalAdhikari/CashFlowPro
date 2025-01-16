using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CashFlowPro.Model;

namespace CashFlowPro.Services
{
    public class DashboardService
    {
        public List<Transaction> LoadTransactions(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading or deserializing the file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
            return new List<Transaction>();
        }

        public List<Debt> LoadDebts(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
            }
            return new List<Debt>();
        }

        public decimal CalculateTotalIncome(List<Transaction> transactions) =>
            transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);

        public decimal CalculateTotalExpense(List<Transaction> transactions) =>
            transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);

        public decimal CalculateTotalDebt(List<Debt> debts) =>
            debts.Where(d => d.Status == "Pending").Sum(d => d.Amount);

        public List<Debt> FilterDebts(List<Debt> debts, DateTime? startDate, DateTime? endDate, string statusFilter)
        {
            return debts.Where(d =>
                (string.IsNullOrEmpty(statusFilter) || d.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase)) &&
                (d.DueDate >= startDate && d.DueDate <= endDate)
            ).ToList();
        }

        public List<Transaction> FilterTransactions(List<Transaction> transactions, DateTime? startDate, DateTime? endDate)
        {
            return transactions.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();
        }

        public List<ChartData> PrepareChartData(List<Transaction> transactions, List<Debt> debts)
        {
            return transactions
                .GroupBy(t => t.Date.Date)
                .Select(g => new ChartData
                {
                    Date = g.Key,
                    Income = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expense = g.Where(t => t.Type == "Expense").Sum(t => t.Amount),
                    Debt = debts.Where(d => d.DueDate?.Date == g.Key).Sum(d => d.Amount),
                    Balance = g.Where(t => t.Type == "Income").Sum(t => t.Amount) -
                             g.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                })
                .OrderBy(d => d.Date)
                .ToList();
        }
    }
}
