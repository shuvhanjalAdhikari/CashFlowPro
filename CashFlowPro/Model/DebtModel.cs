using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowPro.Model
{
    public class Debt
    {
        public string DebtSource { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
    }
}
