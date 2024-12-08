namespace ERP.Support.Enums;

public enum UniqueQtyCode
{
    BAG, // BAGS
    BAL, // BALE
    BDL, // BUNDLES
    BKL, // BUCKLES
    BOU, // BILLIONS OF UNITS
    BOX, // BOX
    BTL, // BOTTLES
    BUN, // BUNCHES
    CAN, // CANS
    CBM, // CUBIC METER
    CCM, // CUBIC CENTIMETER
    CMS, // CENTIMETER
    CTN, // CARTONS
    DOZ, // DOZEN
    DRM, // DRUM
    GGR, // GREAT GROSS
    GMS, // GRAMS
    GRS, // GROSS
    GYD, // GROSS YARDS
    KGS, // KILOGRAMS
    KLR, // KILOLITRE
    KME, // KILOMETRE
    MLT, // MILLILITRE
    MTR, // METERS
    MTS, // METRIC TON
    NOS, // NUMBERS
    PAC, // PACKS
    PCS, // PIECES
    PRS, // PAIRS
    QTL, // QUINTAL
    ROL, // ROLLS
    SET, // SETS
    SQF, // SQUARE FEET
    SQM, // SQUARE METERS
    SQY, // SQUARE YARDS
    TBS, // TABLETS
    TGM, // TEN GRAMS
    THD, // THOUSANDS
    TON, // TONNES
    TUB, // TUBES
    UGS, // US GALLONS
    UNT, // UNITS
    YDS, // YARDS
    OTH  // OTHERS
}

public static class UniqueQtyCodeExtensions
{
    /// <summary>
    /// Get the display name of the UniqueQtyCode
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static string ToDisplayName(this UniqueQtyCode code)
    {
        return code switch
        {
            UniqueQtyCode.BAG => "BAGS",
            UniqueQtyCode.BAL => "BALE",
            UniqueQtyCode.BDL => "BUNDLES",
            UniqueQtyCode.BKL => "BUCKLES",
            UniqueQtyCode.BOU => "BILLIONS OF UNITS",
            UniqueQtyCode.BOX => "BOX",
            UniqueQtyCode.BTL => "BOTTLES",
            UniqueQtyCode.BUN => "BUNCHES",
            UniqueQtyCode.CAN => "CANS",
            UniqueQtyCode.CBM => "CUBIC METER",
            UniqueQtyCode.CCM => "CUBIC CENTIMETER",
            UniqueQtyCode.CMS => "CENTIMETER",
            UniqueQtyCode.CTN => "CARTONS",
            UniqueQtyCode.DOZ => "DOZEN",
            UniqueQtyCode.DRM => "DRUM",
            UniqueQtyCode.GGR => "GREAT GROSS",
            UniqueQtyCode.GMS => "GRAMS",
            UniqueQtyCode.GRS => "GROSS",
            UniqueQtyCode.GYD => "GROSS YARDS",
            UniqueQtyCode.KGS => "KILOGRAMS",
            UniqueQtyCode.KLR => "KILOLITRE",
            UniqueQtyCode.KME => "KILOMETRE",
            UniqueQtyCode.MLT => "MILLILITRE",
            UniqueQtyCode.MTR => "METERS",
            UniqueQtyCode.MTS => "METRIC TON",
            UniqueQtyCode.NOS => "NUMBERS",
            UniqueQtyCode.PAC => "PACKS",
            UniqueQtyCode.PCS => "PIECES",
            UniqueQtyCode.PRS => "PAIRS",
            UniqueQtyCode.QTL => "QUINTAL",
            UniqueQtyCode.ROL => "ROLLS",
            UniqueQtyCode.SET => "SETS",
            UniqueQtyCode.SQF => "SQUARE FEET",
            UniqueQtyCode.SQM => "SQUARE METERS",
            UniqueQtyCode.SQY => "SQUARE YARDS",
            UniqueQtyCode.TBS => "TABLETS",
            UniqueQtyCode.TGM => "TEN GRAMS",
            UniqueQtyCode.THD => "THOUSANDS",
            UniqueQtyCode.TON => "TONNES",
            UniqueQtyCode.TUB => "TUBES",
            UniqueQtyCode.UGS => "US GALLONS",
            UniqueQtyCode.UNT => "UNITS",
            UniqueQtyCode.YDS => "YARDS",
            UniqueQtyCode.OTH => "OTHERS",
            _ => string.Empty
        };
    }
    
    /// <summary>
    /// Get the options for the UniqueQtyCode
    /// </summary>
    /// <returns></returns>
    public static List<object> GetOptions()
    {
        var options = new List<object>();
        foreach (UniqueQtyCode code in Enum.GetValues(typeof(UniqueQtyCode)))
        {
            options.Add(new { Value = code, Text = code.ToDisplayName() });
        }
        return options;
    }
}