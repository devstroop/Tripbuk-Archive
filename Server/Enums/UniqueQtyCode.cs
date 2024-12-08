namespace ERP.Server.Enums;

public enum UniqueQtyCode
{
    Bag, // BAGS
    Bal, // BALE
    Bdl, // BUNDLES
    Bkl, // BUCKLES
    Bou, // BILLIONS OF UNITS
    Box, // BOX
    Btl, // BOTTLES
    Bun, // BUNCHES
    Can, // CANS
    Cbm, // CUBIC METER
    Ccm, // CUBIC CENTIMETER
    Cms, // CENTIMETER
    Ctn, // CARTONS
    Doz, // DOZEN
    Drm, // DRUM
    Ggr, // GREAT GROSS
    Gms, // GRAMS
    Grs, // GROSS
    Gyd, // GROSS YARDS
    Kgs, // KILOGRAMS
    Klr, // KILOLITRE
    Kme, // KILOMETRE
    Mlt, // MILLILITRE
    Mtr, // METERS
    Mts, // METRIC TON
    Nos, // NUMBERS
    Pac, // PACKS
    Pcs, // PIECES
    Prs, // PAIRS
    Qtl, // QUINTAL
    Rol, // ROLLS
    Set, // SETS
    Sqf, // SQUARE FEET
    Sqm, // SQUARE METERS
    Sqy, // SQUARE YARDS
    Tbs, // TABLETS
    Tgm, // TEN GRAMS
    Thd, // THOUSANDS
    Ton, // TONNES
    Tub, // TUBES
    Ugs, // US GALLONS
    Unt, // UNITS
    Yds, // YARDS
    Oth  // OTHERS
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
            UniqueQtyCode.Bag => "BAGS",
            UniqueQtyCode.Bal => "BALE",
            UniqueQtyCode.Bdl => "BUNDLES",
            UniqueQtyCode.Bkl => "BUCKLES",
            UniqueQtyCode.Bou => "BILLIONS OF UNITS",
            UniqueQtyCode.Box => "BOX",
            UniqueQtyCode.Btl => "BOTTLES",
            UniqueQtyCode.Bun => "BUNCHES",
            UniqueQtyCode.Can => "CANS",
            UniqueQtyCode.Cbm => "CUBIC METER",
            UniqueQtyCode.Ccm => "CUBIC CENTIMETER",
            UniqueQtyCode.Cms => "CENTIMETER",
            UniqueQtyCode.Ctn => "CARTONS",
            UniqueQtyCode.Doz => "DOZEN",
            UniqueQtyCode.Drm => "DRUM",
            UniqueQtyCode.Ggr => "GREAT GROSS",
            UniqueQtyCode.Gms => "GRAMS",
            UniqueQtyCode.Grs => "GROSS",
            UniqueQtyCode.Gyd => "GROSS YARDS",
            UniqueQtyCode.Kgs => "KILOGRAMS",
            UniqueQtyCode.Klr => "KILOLITRE",
            UniqueQtyCode.Kme => "KILOMETRE",
            UniqueQtyCode.Mlt => "MILLILITRE",
            UniqueQtyCode.Mtr => "METERS",
            UniqueQtyCode.Mts => "METRIC TON",
            UniqueQtyCode.Nos => "NUMBERS",
            UniqueQtyCode.Pac => "PACKS",
            UniqueQtyCode.Pcs => "PIECES",
            UniqueQtyCode.Prs => "PAIRS",
            UniqueQtyCode.Qtl => "QUINTAL",
            UniqueQtyCode.Rol => "ROLLS",
            UniqueQtyCode.Set => "SETS",
            UniqueQtyCode.Sqf => "SQUARE FEET",
            UniqueQtyCode.Sqm => "SQUARE METERS",
            UniqueQtyCode.Sqy => "SQUARE YARDS",
            UniqueQtyCode.Tbs => "TABLETS",
            UniqueQtyCode.Tgm => "TEN GRAMS",
            UniqueQtyCode.Thd => "THOUSANDS",
            UniqueQtyCode.Ton => "TONNES",
            UniqueQtyCode.Tub => "TUBES",
            UniqueQtyCode.Ugs => "US GALLONS",
            UniqueQtyCode.Unt => "UNITS",
            UniqueQtyCode.Yds => "YARDS",
            UniqueQtyCode.Oth => "OTHERS",
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
            options.Add(new { Value = code.ToString(), Text = code.ToDisplayName() });
        }
        return options;
    }
}