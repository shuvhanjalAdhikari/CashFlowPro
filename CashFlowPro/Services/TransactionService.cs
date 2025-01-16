using Microsoft.JSInterop;
using System.Text.Json;
using CashFlowPro.Model;

namespace CashFlowPro.Services
{
    public class TransactionService
    {
        private static readonly string transactionsFilePath = Path.Combine(@"C:\Users\ashuv\AppDevCW1\CashFlowPro\CashFlowPro\CashFlowPro\wwwroot", "transaction.json");
        private List<Transaction> transactions = new();
        private decimal totalIncome;
        private decimal totalExpense;
        private decimal totalDebt;

        public decimal TotalIncome => totalIncome;
        public decimal TotalExpense => totalExpense;
        public decimal TotalDebt => totalDebt;

        public void LoadTransactions()
        {
            try
            {
                if (File.Exists(transactionsFilePath))
                {
                    var json = File.ReadAllText(transactionsFilePath);
                    transactions = JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }

        public void SaveTransactions()
        {
            try
            {
                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(transactionsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }

        public void CalculateTotals(List<Debt> debts)
        {
            totalIncome = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            totalExpense = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
            totalDebt = debts.Where(d => d.Status == "Pending").Sum(d => d.Amount);
        }

        public void AddTransaction(Transaction newTransaction, decimal availableBalance, List<Transaction> filteredTransactions, JSRuntime JS)
        {
            try
            {
                if (newTransaction.Type == "Expense" && newTransaction.Amount > availableBalance)
                {
                    JS.InvokeVoidAsync("alert", "Insufficient balance");
                    return;
                }

                transactions.Add(newTransaction);
                newTransaction = new Transaction();
                FilterTransactions(filteredTransactions);
                CalculateTotals(filteredTransactions);
                SaveTransactions();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding transaction: {ex.Message}");
            }
        }

        private void CalculateTotals(List<Transaction> filteredTransactions)
        {
            throw new NotImplementedException();
        }

        private void FilterTransactions(List<Transaction> filteredTransactions)
        {
            filteredTransactions = transactions.OrderBy(t => t.Date).ToList();
        }
    }
}
