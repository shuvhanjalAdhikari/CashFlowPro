using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CashFlowPro.Model;

namespace CashFlowPro.Services
{
    public class DebtService
    {
        // Set the file path for debts
        private static readonly string FilePath = Path.Combine(@"C:\Users\ashuv\AppDevCW1\CashFlowPro\CashFlowPro\CashFlowPro\wwwroot", "debt.json");

        private List<Debt> debts = new();
        private List<Debt> filteredDebts = new();

        public DebtService()
        {
            LoadDebts();  // Load debts from the file
        }

        // Method to load debts from the JSON file
        private void LoadDebts()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    debts = JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
                    filteredDebts = debts;  // Start with all debts
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading debts: {ex.Message}");
            }
        }

        // Method to save debts back to the JSON file
        private void SaveDebts()
        {
            try
            {
                var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving debts: {ex.Message}");
            }
        }

        // Filter debts based on criteria
        public List<Debt> FilterDebts(string filterDebtSource, string filterDebtStatus)
        {
            return debts.Where(d =>
                (string.IsNullOrWhiteSpace(filterDebtSource) || d.DebtSource != null && d.DebtSource.Contains(filterDebtSource, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(filterDebtStatus) || d.Status.Equals(filterDebtStatus, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        // Add a new debt to the list and save
        public void AddDebt(Debt newDebt)
        {
            try
            {
                // Ensure status is set to "Pending" by default if not provided
                if (string.IsNullOrEmpty(newDebt.Status))
                {
                    newDebt.Status = "Pending"; // Default to Pending
                }

                debts.Add(newDebt);
                SaveDebts();  // Save the updated list to the file
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding debt: {ex.Message}");
            }
        }

        // Mark a debt as cleared
        public void MarkAsCleared(Debt debt)
        {
            debt.Status = "Cleared";  // Mark the status as Cleared
            SaveDebts();  // Save the updated list to the file
        }
    }
}
