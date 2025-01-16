using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using CashFlowPro.Model;

namespace CashFlowPro.Components.Pages
{
    public partial class Transactions
    {
        private static readonly string transactionsFilePath = Path.Combine(@"C:\Users\ashuv\AppDevCW1\CashFlowPro\CashFlowPro\CashFlowPro\wwwroot", "transaction.json");
        private static readonly string debtsFilePath = Path.Combine(@"C:\Users\ashuv\AppDevCW1\CashFlowPro\CashFlowPro\CashFlowPro\wwwroot", "debt.json");

        private List<string> tags = new List<string>
    {
        "Yearly", "Monthly", "Food", "Drinks", "Clothes", "Gadgets", "Miscellaneous", "Fuel", "Rent", "EMI", "Party"
    };

        private bool isEditingTags = false;
        private string customTagsInput = string.Empty;

        private void HandleTagChange(ChangeEventArgs e)
        {
            var selectedValue = e.Value?.ToString();
            if (selectedValue == "EditTags")
            {
                isEditingTags = true; // Show the input field for manual entry
            }
            else
            {
                newTransaction.Tags = selectedValue; // Assign selected predefined tag
                isEditingTags = false; // Hide input field
            }
        }

        private void AssignCustomTag()
        {
            if (!string.IsNullOrWhiteSpace(customTagsInput))
            {
                newTransaction.Tags = customTagsInput.Trim(); // Assign the custom tag directly
            }

            // Clear the input field and exit edit mode
            customTagsInput = string.Empty;
            isEditingTags = false;
        }

        private List<Transaction> transactions = new();
        private List<Debt> debts = new();
        private List<Transaction> filteredTransactions = new();
        private Transaction newTransaction = new();
        private DateTime? fromDate;
        private DateTime? toDate;
        private string filterTitle;
        private string filterType;
        private string filterTags;
        private bool isModalOpen = false;
        private string sortOrder = "Ascending";  // Default sorting order

        private decimal totalIncome;
        private decimal totalExpense;
        private decimal totalDebt;

        public decimal TotalIncome => totalIncome;
        public decimal TotalExpense => totalExpense;
        public decimal TotalDebt => totalDebt;
        public int TotalTransactions => transactions.Count + debts.Count;
        public decimal AvailableBalance => TotalIncome + TotalDebt - TotalExpense;

        // Load transactions on initialization
        protected override void OnInitialized()
        {
            LoadTransactions();
            LoadDebts();

            CalculateTotals();
            filteredTransactions = transactions; // Start with all transactions
        }

        // Load transactions from the JSON file
        private void LoadTransactions()
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

        // Save transactions to the JSON file
        private void SaveTransactions()
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

        private void CalculateTotals()
        {
            totalIncome = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            totalExpense = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
            totalDebt = debts.Where(d => d.Status == "Pending").Sum(d => d.Amount);
        }

        // Apply filters to transactions
        private void FilterTransactions()
        {
            filteredTransactions = transactions
                .Where(t =>
                    (!fromDate.HasValue || t.Date >= fromDate.Value) &&
                    (!toDate.HasValue || t.Date <= toDate.Value) &&
                    (string.IsNullOrWhiteSpace(filterTitle) || t.Title.Contains(filterTitle, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(filterType) || t.Type.Equals(filterType, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(filterTags) || t.Tags.Equals(filterTags, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();

            // Sort transactions based on selected sort order
            if (sortOrder == "Descending")
            {
                filteredTransactions = filteredTransactions.OrderByDescending(t => t.Date).ToList();
            }
            else
            {
                filteredTransactions = filteredTransactions.OrderBy(t => t.Date).ToList();
            }
        }

        // Clear all filters
        private void ClearFilters()
        {
            fromDate = null;
            toDate = null;
            filterTitle = null;
            filterType = null;
            filterTags = null;
            filteredTransactions = transactions;
        }

        // Add a new transaction
        private async void AddTransaction()
        {
            try
            {
                // Check if the transaction type is "Expense" and the amount exceeds the available balance
                if (newTransaction.Type == "Expense" && newTransaction.Amount > AvailableBalance)
                {
                    // Display an alert message when balance is insufficient
                    await JS.InvokeVoidAsync("alert", "Insufficient balance");
                    return; // Exit the method
                }

                transactions.Add(newTransaction);
                newTransaction = new Transaction();  // Clear the form
                FilterTransactions();  // Reapply filters

                CalculateTotals();
                CloseModal();  // Close modal
                SaveTransactions();  // Save transactions to file
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding transaction: {ex.Message}");
            }
        }

        private void LoadDebts()
        {
            try
            {
                if (File.Exists(debtsFilePath))
                {
                    var json = File.ReadAllText(debtsFilePath);
                    debts = JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading debts: {ex.Message}");
            }
        }

        // Modal control
        private void OpenModal() => isModalOpen = true;
        private void CloseModal() => isModalOpen = false;

    }
}
