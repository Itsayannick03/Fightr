
public class Exchange
{
    private Fighter Fighter1;
    private Fighter Fighter2;

    private Fighter Attacker;
    private Fighter Defender;


    public Exchange(Fighter fighter1, Fighter fighter2)
    {
        this.Fighter1 = fighter1;
        this.Fighter2 = fighter2;
    }

    private enum AttackOutcome
    {
        BigWhiff,
        Miss,
        Hit,
        Crit,
        Circle
        
    }

    // Helper functions
    private void ResolveInitiative()
    {
        int fighter1Initiative = this.Fighter1.GetInitiativeScore();
        int fighter2Initiative = this.Fighter2.GetInitiativeScore();

        Debug.LogDetail($"{Fighter1.LastName} initiative: {fighter1Initiative}");
        Debug.LogDetail($"{Fighter2.LastName} initiative: {fighter2Initiative}");

        if(fighter1Initiative > fighter2Initiative)
        {
            this.Attacker = this.Fighter1;
            this.Defender = this.Fighter2;
        }
        else
        {
            this.Attacker = this.Fighter2;
            this.Defender = this.Fighter1;
        }

        Debug.Log($"{this.Attacker.LastName} wins initiative ({fighter1Initiative} vs {fighter2Initiative}) → will attack {this.Defender.LastName}");
    }

    public int GetExchangeTime(ExchangeOutcome result)
    {
        switch (result)
        {
            case ExchangeOutcome.Miss:
                return Random.Shared.Next(4, 9); // 4-8 seconds

            case ExchangeOutcome.Hit:
                return Random.Shared.Next(4, 8); // 4-7 seconds

            case ExchangeOutcome.Knockout:
                return Random.Shared.Next(6, 12); // 6-11 seconds

            default:
                return Random.Shared.Next(4, 12); 
        }
    }


    private Attack resolveAttack(Move attackMove, Bodypart bodypart)
    {
        int damage;
        ExchangeOutcome outcome;
        if(this.Attacker.ShouldCircle())
        {
            Debug.Log($"{this.Attacker.LastName} circles {this.Defender.LastName} (aggression {this.Attacker.Aggression}, stamina {this.Attacker.Stamina}/{this.Attacker.Cardio}, momentum {this.Attacker.Momentum})");
            Debug.LogDetail($"  {this.Attacker.LastName} stamina: {this.Attacker.Stamina}, momentum: {this.Attacker.Momentum}");

            this.Attacker.Circle();
            this.Defender.Circle();

            Debug.LogDetail($"  After circle - {this.Attacker.LastName} stamina: {this.Attacker.Stamina}, momentum: {this.Attacker.Momentum}");

            damage = 0;
            outcome = ExchangeOutcome.Circle;

            Attack circle = new Attack(damage, outcome);

            return circle;
        }

        Random random = new Random();
        int margin = this.Attacker.GetAttackMargin(this.Defender, attackMove);

        Debug.LogDetail($"Margin: {margin} ({this.Attacker.LastName} vs {this.Defender.LastName})");

        double staminaMultiplier;
        if(margin <= -15)
        {
            // Get random double betwwen 2.0 and 2.5
            staminaMultiplier = 2.0 + random.NextDouble() * (2.5 - 2.0);
            double staminaCost = attackMove.StaminaCost * staminaMultiplier;

            int RoundedstaminaCost = (int)Math.Round(staminaCost);

            Debug.LogDetail($"{this.Attacker.LastName} stamina before: {this.Attacker.Stamina}, cost: {RoundedstaminaCost}");
            this.Attacker.DecreaseStamina(RoundedstaminaCost);
            this.Attacker.DecreaseMomentum(1);
            Debug.LogDetail($"{this.Attacker.LastName} stamina after: {this.Attacker.Stamina}, momentum: {this.Attacker.Momentum}");

            damage = 0;
            outcome = ExchangeOutcome.BigWhiff;

            Debug.Log($"{this.Attacker.LastName} BIG WHIFF on {attackMove.Name} (margin {margin} ≤ -15, costly stamina penalty)");
        }
        else if(margin < 0)
        {
            staminaMultiplier = 1.25 + random.NextDouble() * (1.5 - 1.25);
            double staminaCost = attackMove.StaminaCost * staminaMultiplier;

            int RoundedstaminaCost = (int)Math.Round(staminaCost);

            Debug.LogDetail($"{this.Attacker.LastName} stamina before: {this.Attacker.Stamina}, cost: {RoundedstaminaCost}");
            this.Attacker.DecreaseStamina(RoundedstaminaCost);
            Debug.LogDetail($"{this.Attacker.LastName} stamina after: {this.Attacker.Stamina}");

            damage = 0;
            outcome = ExchangeOutcome.Miss;

            Debug.Log($"{this.Attacker.LastName} misses {attackMove.Name} (margin {margin}, needed ≥ 0)");
        }
        else if(margin >= 15)
        {
            staminaMultiplier = 0.75 + random.NextDouble() * (1.0 - 0.75);
            double staminaCost = attackMove.StaminaCost * staminaMultiplier;
            int RoundedstaminaCost = (int)Math.Round(staminaCost);

            int baseDamage = this.Attacker.CalculateDamage(attackMove);
            int totalDamage = baseDamage  * 2;

            Debug.LogDetail($"{this.Attacker.LastName} stamina before: {this.Attacker.Stamina}, cost: {RoundedstaminaCost}");
            Debug.LogDetail($"Base damage: {baseDamage}, CRIT multiplier x2 → total: {totalDamage}");

            this.Attacker.DecreaseStamina(RoundedstaminaCost);
            this.Attacker.IncreaseMomentum(2);

            this.Defender.TakeDamage(bodypart, totalDamage);
            this.Defender.DecreaseMomentum(2);

            Debug.LogDetail($"{this.Attacker.LastName} stamina after: {this.Attacker.Stamina}, momentum: {this.Attacker.Momentum}");
            Debug.LogDetail($"{this.Defender.LastName} {bodypart.Name} HP after: {bodypart.Health}");

            damage = totalDamage;
            outcome = ExchangeOutcome.Crit;

            Debug.Log($"{this.Attacker.LastName} CRITS {this.Defender.LastName} with {attackMove.Name} for {totalDamage} ({baseDamage} base × 2 = {totalDamage}, margin {margin} ≥ 15) — {bodypart.Name} HP: {bodypart.Health}");
        }
        else
        {
            staminaMultiplier = 0.8 + random.NextDouble() * (1.2 - 0.8);
            double staminaCost = attackMove.StaminaCost * staminaMultiplier;
            int RoundedstaminaCost = (int)Math.Round(staminaCost);

            int totalDamage = this.Attacker.CalculateDamage(attackMove);

            Debug.LogDetail($"{this.Attacker.LastName} stamina before: {this.Attacker.Stamina}, cost: {RoundedstaminaCost}");
            Debug.LogDetail($"Damage: {totalDamage}");

            this.Attacker.DecreaseStamina(RoundedstaminaCost);
            this.Attacker.IncreaseMomentum(1);

            this.Defender.TakeDamage(bodypart, totalDamage);
            this.Defender.DecreaseMomentum(1);

            Debug.LogDetail($"{this.Attacker.LastName} stamina after: {this.Attacker.Stamina}, momentum: {this.Attacker.Momentum}");
            Debug.LogDetail($"{this.Defender.LastName} {bodypart.Name} HP after: {bodypart.Health}");

            damage = totalDamage;
            outcome = ExchangeOutcome.Hit;

            Debug.Log($"{this.Attacker.LastName} hits {this.Defender.LastName} with {attackMove.Name} for {totalDamage} ({attackMove.Damage} base + {(this.Attacker.Power - 100) / 10} power, margin {margin}) — {bodypart.Name} HP: {bodypart.Health}");
        }

        Attack attack = new Attack(damage, outcome);

        return attack;
    }

    private void handleResult(ExchangeOutcome outcome, Bodypart targetBodypart, Move move)
    {
        switch (outcome)
        {
            case ExchangeOutcome.BigWhiff:
                Console.WriteLine(this.Attacker.LastName + " Tries to land a " + move.Name + " on " + this.Defender.LastName + "s " + targetBodypart.Name + " but misses brutally");
                break;
            case ExchangeOutcome.Miss:
                Console.WriteLine(this.Attacker.LastName + " Tries to land a " + move.Name + " on " + this.Defender.LastName + "s " + targetBodypart.Name + " but just misses");
                break;
            case ExchangeOutcome.Hit:
                Console.WriteLine(this.Attacker.LastName + " lands a " + move.Name + " on " + this.Defender.LastName + "s " + targetBodypart.Name);
                break;
            case ExchangeOutcome.Crit:
                Console.WriteLine(this.Attacker.LastName + " lands a brutal " + move.Name + " on " + this.Defender.LastName + "s " + targetBodypart.Name);
                break;
            case ExchangeOutcome.Circle:
                Console.WriteLine($"{this.Attacker.LastName} is circeling {this.Defender.LastName}");
                break;
            default:
                Console.WriteLine(this.Attacker.LastName + " lands a " + move.Name + " on " + this.Defender.LastName + "s " + targetBodypart.Name);
                break;
        }
    }

    private void HandleStats(int damage, Fighter attacker)
    {
        if(damage < 5)
            attacker.IncreaseStat(StatType.Strikes, 1);
        else
            attacker.IncreaseStat(StatType.SignificantStrikes, 1);
        
        attacker.IncreaseStat(StatType.DamageDealt, damage);
    }

    public ExchangeSummary Run()
    {
        

        this.ResolveInitiative();
        
        Move attackMove = this.Attacker.GetMove();
        Bodypart targetBodypart = this.Defender.GetBodypart(attackMove);

        Debug.Log($"{this.Attacker.LastName} chooses {attackMove.Name} → targets {this.Defender.LastName}'s {targetBodypart.Name}");
        Debug.LogDetail($"  {this.Attacker.LastName} stamina: {this.Attacker.Stamina}, momentum: {this.Attacker.Momentum}");
        Debug.LogDetail($"  {this.Defender.LastName} stamina: {this.Defender.Stamina}, momentum: {this.Defender.Momentum}");

        Attack attack = this.resolveAttack(attackMove, targetBodypart);

        this.Fighter1.CheckBodyparts();
        this.Fighter2.CheckBodyparts();

        this.handleResult(attack.Outcome, targetBodypart, attackMove);

        int damage = attack.Damage;
        
        ExchangeOutcome exchangeOutcome;
        if(Fighter1.IsKnockedOut() || Fighter2.IsKnockedOut())
        {
            exchangeOutcome = ExchangeOutcome.Knockout;
            this.HandleStats(damage, this.Attacker);
        }
            
        else if(attack.Outcome == ExchangeOutcome.Crit || attack.Outcome == ExchangeOutcome.Hit)
        {
            exchangeOutcome = ExchangeOutcome.Hit;
            this.HandleStats(damage, this.Attacker);
        }
            
        else
            exchangeOutcome = ExchangeOutcome.Miss;

        int timeTaken = this.GetExchangeTime(exchangeOutcome);

        Debug.LogDetail($"  Time taken: {timeTaken}s");

        return new ExchangeSummary(this.Attacker, this.Defender, damage, exchangeOutcome, timeTaken);

    }
}