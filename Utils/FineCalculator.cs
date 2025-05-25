using System;

namespace LibraryManagementSystem.Utils
{
    public class FineCalculator
    {
        public decimal DailyFineRate { get; set; } = 5;

        public decimal CalculateFine(DateTime dueDate, DateTime? returnDate)
        {
            var effectiveReturnDate = returnDate ?? DateTime.Now;
            var daysLate = (effectiveReturnDate.Date - dueDate.Date).Days;
            return daysLate > 0 ? daysLate * DailyFineRate : 0;
        }
    }
}
