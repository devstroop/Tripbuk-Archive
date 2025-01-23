namespace Tripbuk.Server.Enums;

public enum VoucherType
{
    Purchase = 2,
    SaleReturn = 3,
    MaterialReceipt = 4,
    StockTransfer = 5,
    Production = 6,
    Unassemble = 7,
    StockJournal = 8,
    Sale = 9,
    PurchaseReturn = 10,
    MaterialIssue = 11,
    SaleOrder = 12,
    PurchaseOrder = 13,
    Receipt = 14,
    Contra = 15,
    Journal = 16,
    DebitNote = 17,
    CreditNote = 18,
    Payment = 19,
    FormsReceived = 21,
    FormsIssued = 22,
    SaleQuotation = 26,
    PurchaseQuotation = 27,
    SalaryCalculation = 28,
    CallReceipt = 29,
    CallAllocation = 30,
    PurchaseIndent = 31,
    CallReport = 32,
    PhysicalStock = 61
}

public static class VoucherTypeExtensions
{
    /// <summary>
    /// Get the display name of the voucher type
    /// </summary>
    /// <param name="voucherType"></param>
    /// <returns></returns>
    public static string ToDisplayName(this VoucherType voucherType)
    {
        return voucherType switch
        {
            VoucherType.Purchase => "Purchase",
            VoucherType.SaleReturn => "Sale Return",
            VoucherType.MaterialReceipt => "Material Receipt",
            VoucherType.StockTransfer => "Stock Transfer",
            VoucherType.Production => "Production",
            VoucherType.Unassemble => "Unassemble",
            VoucherType.StockJournal => "Stock Journal",
            VoucherType.Sale => "Sale",
            VoucherType.PurchaseReturn => "Purchase Return",
            VoucherType.MaterialIssue => "Material Issue",
            VoucherType.SaleOrder => "Sale Order",
            VoucherType.PurchaseOrder => "Purchase Order",
            VoucherType.Receipt => "Receipt",
            VoucherType.Contra => "Contra",
            VoucherType.Journal => "Journal",
            VoucherType.DebitNote => "Debit Note",
            VoucherType.CreditNote => "Credit Note",
            VoucherType.Payment => "Payment",
            VoucherType.FormsReceived => "Forms Received",
            VoucherType.FormsIssued => "Forms Issued",
            VoucherType.SaleQuotation => "Sale Quotation",
            VoucherType.PurchaseQuotation => "Purchase Quotation",
            VoucherType.SalaryCalculation => "Salary Calculation",
            VoucherType.CallReceipt => "Call Receipt",
            VoucherType.CallAllocation => "Call Allocation",
            VoucherType.PurchaseIndent => "Purchase Indent",
            VoucherType.CallReport => "Call Report",
            VoucherType.PhysicalStock => "Physical Stock",
            _ => voucherType.ToString()
        };
    }
    
    /// <summary>
    /// Get the options for the voucher type
    /// </summary>
    /// <returns></returns>
    public static List<object> GetOptions()
    {
        return Enum.GetValues<VoucherType>().Select(x => new { Value = (int)x, Text = x.ToDisplayName() }).ToList<object>();
    }
}