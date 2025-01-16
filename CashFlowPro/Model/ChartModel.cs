using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowPro.Model
{
    public class ChartData
    {
        public DateTime Date { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Debt { get; set; }
        public decimal Balance { get; set; }
    }
}
