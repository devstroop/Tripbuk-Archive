namespace TripBUK.Server.Enums;

/// <summary>
/// Master type
/// </summary>
public enum MasterType
{
    AccountGroupMaster = 1,
    AccountMaster = 2,
    CostCenterGroupMaster = 3,
    CostCenterMaster = 4,
    ItemGroupMaster = 5,
    ItemMaster = 6,
    CurrencyMaster = 7,
    UnitMaster = 8,
    BillSundryMaster= 9,
    MaterialCenterGroupMaster = 10,
    MaterialCenterMaster = 11,
    FormMaster = 12,
    SaleTypeMaster = 13,
    PurchaseTypeMaster= 14,
    BillOfMaterialMaster = 15,
    UnitConversionMaster = 16,
    CurrencyConversionMaster = 17,
    StandardNarrationMaster = 18,
    BrokerMaster = 19,
    AuthorMaster = 20,
    VoucherSeriesMaster = 21,
    TdsMaster = 22,
    BranchMaster = 24,
    TaxCategoryMaster = 25,
    MasterSeriesGroupMaster = 26,
    EmployeeMaster = 27,
    EmployeeGroupMaster = 28,
    SalaryComponentMaster = 29,
    DiscountStructureMaster = 30,
    MarkupStructureMaster = 31,
    SchemeMaster = 32,
    ExecutiveMaster = 33,
    ContactGroupMaster = 34,
    ContactMaster = 36
}

public static class MasterTypeExtensions
{
    /// <summary>
    /// Get the display name of the master type
    /// </summary>
    /// <param name="masterType"></param>
    /// <returns></returns>
    public static string ToDisplayName(this MasterType masterType)
    {
        return masterType switch
        {
            MasterType.AccountGroupMaster => "Account Group",
            MasterType.AccountMaster => "Account",
            MasterType.CostCenterGroupMaster => "Cost Center Group",
            MasterType.CostCenterMaster => "Cost Center",
            MasterType.ItemGroupMaster => "Item Group",
            MasterType.ItemMaster => "Item",
            MasterType.CurrencyMaster => "Currency",
            MasterType.UnitMaster => "Unit",
            MasterType.BillSundryMaster => "Bill Sundry",
            MasterType.MaterialCenterGroupMaster => "Material Center Group",
            MasterType.MaterialCenterMaster => "Material Center",
            MasterType.FormMaster => "Form",
            MasterType.SaleTypeMaster => "Sale Type",
            MasterType.PurchaseTypeMaster => "Purchase Type",
            MasterType.BillOfMaterialMaster => "Bill Of Material",
            MasterType.UnitConversionMaster => "Unit Conversion",
            MasterType.CurrencyConversionMaster => "Currency Conversion",
            MasterType.StandardNarrationMaster => "Standard Narration",
            MasterType.BrokerMaster => "Broker",
            MasterType.AuthorMaster => "Author",
            MasterType.VoucherSeriesMaster => "Voucher Series",
            MasterType.TdsMaster => "Tds",
            MasterType.BranchMaster => "Branch",
            MasterType.TaxCategoryMaster => "Tax Category",
            MasterType.MasterSeriesGroupMaster => "Master Series Group",
            MasterType.EmployeeMaster => "Employee",
            MasterType.EmployeeGroupMaster => "Employee Group",
            MasterType.SalaryComponentMaster => "Salary Component",
            MasterType.DiscountStructureMaster => "Discount Structure",
            MasterType.MarkupStructureMaster => "Markup Structure",
            MasterType.SchemeMaster => "Scheme",
            MasterType.ExecutiveMaster => "Executive",
            MasterType.ContactGroupMaster => "Contact Group",
            MasterType.ContactMaster => "Contact",
            _ => string.Empty
        };
    }
    
    /// <summary>
    /// Get the options for the master type
    /// </summary>
    /// <returns></returns>
    public static List<object> GetOptions()
    {
        return Enum.GetValues<MasterType>().Select(x => new { Value = (int)x, Text = x.ToDisplayName() }).ToList<object>();
    }
}