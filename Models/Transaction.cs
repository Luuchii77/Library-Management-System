public class Transaction
{
    public int TransactionId { get; set; }
    public int BookId { get; set; }
    public int StudentId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
