public enum StatType
{
    Strikes,
    SignificantStrikes,
    // Rooked,
    DamageDealt,
    // DamageTaken,
    // KnockedDown,
    // ControllTime
}

public static class StatTypeHelper
{
    public static readonly Dictionary<StatType, double> ScoreWeights = new()
    {
        { StatType.DamageDealt,        1.0 },
        { StatType.SignificantStrikes, 2.0 },
        { StatType.Strikes,            1.0 },
    };
}