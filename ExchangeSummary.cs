public class ExchangeSummary
{
    public ExchangeOutcome Result;
    public Fighter Attacker;
    public Fighter Defender;

    public int Damage;

    public int TimeTaken;

    public ExchangeSummary(Fighter attacker, Fighter defender, int damage, ExchangeOutcome result, int timeTaken)
    {
      this.Attacker = attacker;
      this.Defender = defender;
      this.Damage = damage;
      this.TimeTaken = timeTaken;
      this.Result = result;
    }

     
}