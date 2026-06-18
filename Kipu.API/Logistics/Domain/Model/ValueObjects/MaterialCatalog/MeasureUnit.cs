namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

/// <summary>
/// Unit of measurement for materials in the catalog.
/// When making POST/PUT requests, use the numeric value.
/// <br/>
/// 1 = Unit (und) | 2 = Piece (pz) | 11 = Ton (t) | 20 = Meter (m) | 21 = Linear Meter (ml) |
/// 30 = Square Meter (m2) | 40 = Cubic Meter (m3) | 41 = Liter (l) | 42 = Gallon (gal) |
/// 50 = Bag | 51 = Roll | 52 = Rod | 53 = Sheet | 54 = Bucket | 55 = Box
/// </summary>
public enum MeasureUnit
{
    Unit = 1,
    Piece = 2,
    Ton = 11,
    Meter = 20,
    LinearMeter = 21,
    SquareMeter = 30,
    CubicMeter = 40,
    Liter = 41,
    Gallon = 42,
    Bag = 50,
    Roll = 51,
    Rod = 52,
    Sheet = 53,
    Bucket = 54,
    Box = 55
};