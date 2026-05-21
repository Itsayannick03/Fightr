public class RoundFighterStats
{
    public int DamageDealt { get; private set; }
    public int LandedStrikes { get; private set; }
    public int MissedStrikes { get; private set; }
    public int CriticalHits { get; private set; }
    public int BigWhiffs { get; private set; }

    public double GetScoreValue()
    {
        return
            DamageDealt * 1.0 +
            LandedStrikes * 2.0 +
            CriticalHits * 8.0 +
            //Knockdowns * 15.0 -
            MissedStrikes * 0.5 -
            BigWhiffs * 2.0;
    }

    public void ApplyExchangeSummary(ExchangeSummary summary)
    {
        DamageDealt += summary.Damage;

        switch (summary.Result)
        {
            case ExchangeOutcome.Hit:
                LandedStrikes++;
                break;

            case ExchangeOutcome.Miss:
                MissedStrikes++;
                break;

            case ExchangeOutcome.Crit:
                LandedStrikes++;
                CriticalHits++;
                break;

            case ExchangeOutcome.BigWhiff:
                MissedStrikes++;
                BigWhiffs++;
                break;

            case ExchangeOutcome.Knockout:
                LandedStrikes++;
                break;

            case ExchangeOutcome.Circle:
                break;
        }
    }
}