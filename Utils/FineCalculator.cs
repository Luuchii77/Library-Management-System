using System;

namespace LibraryManagementSystem.Utils
{
    public class FineCalculator
    {
        public decimal DailyFineRate { get; set; } = 5;
        public int GracePeriodDays { get; set; } = 7;

        public decimal CalculateFine(DateTime borrowDate, DateTime? returnDate)
        {
            var daysLate = (returnDate ?? DateTime.Now) - borrowDate;
            if (daysLate.TotalDays > GracePeriodDays)
                return (decimal)(daysLate.TotalDays - GracePeriodDays) * DailyFineRate;
            return 0;
        }
    }
}
