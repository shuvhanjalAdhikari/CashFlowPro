using System;
using System.Collections.Generic;
using CashFlowPro.Model;
using CashFlowPro.Services;

namespace CashFlowPro.Components.Pages
{
    public partial class Debts
    {
        private DebtService debtService = new DebtService();  // Create an instance of DebtService
        private List<Debt> filteredDebts = new();
        private Debt newDebt = new Debt();
        private string filterDebtSource;
        private string filterDebtStatus;
        private bool isDebtModalOpen = false;

        // Initialize and load filtered debts
        protected override void OnInitialized()
        {
            filteredDebts = debtService.FilterDebts(filterDebtSource, filterDebtStatus);  // Use the instance to call the method
        }

        // Apply filters
        private void FilterDebts()
        {
            filteredDebts = debtService.FilterDebts(filterDebtSource, filterDebtStatus);  // Use the instance to call the method
        }

        // Clear all debt filters and show all debts
        private void ClearDebtFilters()
        {
            filterDebtSource = string.Empty;
            filterDebtStatus = string.Empty;
            filteredDebts = debtService.FilterDebts(filterDebtSource, filterDebtStatus);  // Use the instance to call the method
        }

        // Add a new debt
        private void AddDebt()
        {
            debtService.AddDebt(newDebt);  // Use the instance to call the method
            newDebt = new Debt();  // Reset the new debt form
            FilterDebts();  // Reapply filters after adding
            CloseDebtModal();  // Close the modal
        }

        // Mark a debt as cleared
        private void MarkAsCleared(Debt debt)
        {
            debtService.MarkAsCleared(debt);  // Use the instance to call the method
            FilterDebts();  // Reapply filters after status change
        }

        // Open the debt modal
        private void OpenDebtModal() => isDebtModalOpen = true;

        // Close the debt modal
        private void CloseDebtModal() => isDebtModalOpen = false;
    }
}
