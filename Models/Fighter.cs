public class Fighter
{
    public string FirstName;
    public string LastName;

    public int Striking;
    public int StrikingDefense;
    public int Kicking;
    public int KickingDefense;
    public int Wrestling;
    public int Grappling;
    public int Cardio;
    private int CardioMod;
    public int Power;
    public int Chin;

    public int Stamina;
    
    public int Momentum;

    public int Aggression;
    private int AggressionMod;

    public Bodypart Head;
    public Bodypart Torso;
    public Bodypart LeftArm;
    public Bodypart RightArm;
    public Bodypart LeftLeg;
    public Bodypart RightLeg;

    public List<Move> Moves;
    private List<Bodypart> Bodyparts;

    // stats
    private int Score;
    public Dictionary<StatType, int> Stats { get; private set; } = new Dictionary<StatType, int>();    


    public Fighter(string firstName, string lastName, int striking, int strikingDefense, int kicking, int kickingDefense, int wrestling, int grappling, int cardio, int power, int chin, int aggression)
    {

        this.FirstName = firstName;
        this.LastName = lastName;


        this.Striking = striking;
        this.StrikingDefense = strikingDefense;

        this.Kicking = kicking;
        this.KickingDefense = kickingDefense;

        this.Wrestling = wrestling;
        this.Grappling = grappling;

        this.Cardio = cardio;
        this.CardioMod = (cardio - 100) / 10;
        this.Power = power;
        this.Chin = chin;


        this.Stamina = cardio;
        
        this.Momentum = 0;
        this.Aggression = aggression;
        this.AggressionMod = (aggression - 50) / 10;

        this.Score = 0;

        this.Head = new Bodypart("Head", 100, 45);
        this.Torso = new Bodypart("Torso", 100, 20);
        this.LeftArm = new Bodypart("Left Arm", 100, 5);
        this.RightArm = new Bodypart("Right Arm", 100, 5);
        this.LeftLeg = new Bodypart("Left Leg", 100, 12);
        this.RightLeg = new Bodypart("Right Leg", 100, 12);

        this.Bodyparts = new List<Bodypart>();
        this.Bodyparts.Add(this.Head);
        this.Bodyparts.Add(this.Torso);
        this.Bodyparts.Add(this.LeftArm);
        this.Bodyparts.Add(this.RightArm);
        this.Bodyparts.Add(this.LeftLeg);
        this.Bodyparts.Add(this.RightLeg);

        this.Moves = new List<Move>();

        this.Moves.Add(MoveList.jab);
        this.Moves.Add(MoveList.cross);
        this.Moves.Add(MoveList.hook);
        this.Moves.Add(MoveList.haymaker);

        this.Moves.Add(MoveList.lowKick);
        this.Moves.Add(MoveList.bodyKick);
        this.Moves.Add(MoveList.headKick);
        this.Moves.Add(MoveList.frontKick);

        Stats.Add(StatType.Strikes, 0);
        Stats.Add(StatType.SignificantStrikes, 0);

        Stats.Add(StatType.DamageDealt, 0);

    }

    // Info
    public void GetFighterInfo()
    {
        Console.WriteLine("###############");
        Console.WriteLine($"{this.FirstName} {this.LastName}");
        Console.WriteLine("Head: " + this.Head.Health);
        Console.WriteLine("Torso: " + this.Torso.Health);
        Console.WriteLine("Left Arm: " + this.LeftArm.Health);
        Console.WriteLine("Right Arm: " + this.RightArm.Health);
        Console.WriteLine("Left Leg: " + this.LeftLeg.Health);
        Console.WriteLine("Right Leg: " + this.RightLeg.Health);

        Console.WriteLine("\n");
        Console.WriteLine("Stamina: " + this.Stamina+ "/" + this.Cardio);
        Console.WriteLine("Momentum: " + this.Momentum);
        Console.WriteLine("###############\n");

    }

    // Stat functions

    public void IncreaseStat(StatType statType, int ammount)
    {
        this.Stats[statType] += ammount;
    }

    // Health functions
    public void TakeDamage(Bodypart bodypart, int damage)
    {
        int before = bodypart.Health;
        bodypart.Health -= damage;

        if(bodypart.Health < 0)
            bodypart.Health = 0;

        Debug.LogDetail($"{this.LastName} takes {damage} to {bodypart.Name} ({before} → {bodypart.Health})");
    }

    public void IncreaseStamina(int ammount)
    {
        int before = this.Stamina;
        this.Stamina += ammount;

        if(this.Stamina > this.Cardio)
            this.Stamina = this.Cardio;

        Debug.LogDetail($"{this.LastName} stamina {before} → {this.Stamina} (+{ammount})");
    }

    public void DecreaseStamina(int ammount)
    {
        int before = this.Stamina;
        this.Stamina -= ammount;

        if(this.Stamina < 0)
            this.Stamina = 0;

        Debug.LogDetail($"{this.LastName} stamina {before} → {this.Stamina} (-{ammount})");
    }

    public void IncreaseMomentum(int ammount)
    {
        int before = this.Momentum;
        this.Momentum += ammount;

        if(this.Momentum > 10)
            this.Momentum = 10;

        Debug.LogDetail($"{this.LastName} momentum {before} → {this.Momentum} (+{ammount})");
    }

    public void DecreaseMomentum(int ammount)
    {
        int before = this.Momentum;
        this.Momentum -= ammount;

        if(this.Momentum < -10)
            this.Momentum = -10;

        Debug.LogDetail($"{this.LastName} momentum {before} → {this.Momentum} ({-ammount})");
    }

    private void DriftMomentum(int ammount)
    {
        if(this.Momentum > 0)
            this.DecreaseMomentum(ammount);
        else if(this.Momentum < 0)
            this.IncreaseMomentum(ammount);
    
    }

    public void Recover()
    {
        // Momentum drifts 25% towards 0
        int momemtumDrift = (int)Math.Round(this.Momentum * 0.25);
        this.DriftMomentum(momemtumDrift);

        // Recover 25% of max stamina
        int staminaRecovery = (int)Math.Round(this.Cardio * 0.10);
        this.IncreaseStamina(staminaRecovery);
    }

    public void Circle()
    {
        

        int staminaRecovery = Random.Shared.Next(1, 3);
        this.IncreaseStamina(staminaRecovery);
    }

    public bool IsKnockedOut()
    {
        if(this.Head.Health == 0)
            return true;
        else
            return false;
    }

    

    

    // Combat functions
    public int GetInitiativeScore()
    {
        Random random = new Random();
        int roll = random.Next(-10, 10);
        int staminaMod = (this.Stamina - (this.Cardio / 2)) / 10;

        int score = roll;
        score += this.Momentum;
        score += staminaMod;
        score += this.AggressionMod;

        Debug.LogDetail($"{this.LastName} initiative: roll={roll}, momentum={this.Momentum}, staminaMod={staminaMod}, aggrMod={this.AggressionMod} → total={score}");

        return score;
    }

    public int GetAttackMargin(Fighter opponent, Move move)
    {
        int attackScore = this.GetAttackStats(move);
        int defenseScore = opponent.GetDefenseStats(move);

        int attackMargin = attackScore - defenseScore;

        return attackMargin;
    }

    public Move GetMove()
    {
        Random random = new Random();
        int index = random.Next(this.Moves.Count);

        Move move = this.Moves[index];

        return move;
    }

    public Bodypart GetBodypart(Move move)
    {
        List<Bodypart> validBodyparts = this.GetValidTargetBodyparts(move);

        int totalWeight = 0;

        foreach (Bodypart bodypart in validBodyparts)
        {
            totalWeight += bodypart.Weight;
        }

        int roll = Random.Shared.Next(totalWeight);

        foreach (Bodypart bodypart in validBodyparts)
        {
            if (roll < bodypart.Weight)
            {
                return bodypart;
            }

            roll -= bodypart.Weight;
        }

        return validBodyparts[0];
    }

    private Bodypart GetBodypartFromType(BodypartType bodypartType)
    {
        switch (bodypartType)
        {
            case BodypartType.Head:
                return this.Head;
            case BodypartType.Torso:
                return this.Torso;
            case BodypartType.LeftArm:
                return this.LeftArm;
            case BodypartType.RightArm:
                return this.RightArm;
            case BodypartType.LeftLeg:
                return this.LeftLeg;
            case BodypartType.RightLeg:
                return this.RightLeg;
            default:
                return this.Head;
        }
    }

    public List<Bodypart> GetValidTargetBodyparts(Move move)
    {
        List<Bodypart> validBodyparts = new List<Bodypart>();

        foreach(BodypartType bodypartType in move.TargetBodyparts)
        {

            Bodypart bodypart = this.GetBodypartFromType(bodypartType);
            validBodyparts.Add(bodypart);
        }

        return validBodyparts;
    }

    public int CalculateDamage(Move move)
    {
        double staminaMultiplier = 0.5 + 0.5 * (double)this.Stamina / this.Cardio;
        int powerModifier = (this.Power - 100) / 10;

        int damage =(int)((move.Damage + powerModifier) * staminaMultiplier);

        return damage;
    }

    public bool ShouldCircle()
    {
        int attackChance = this.Aggression;

        attackChance += this.Momentum * 3;

        if (this.Stamina < this.Cardio / 3)
        {
            attackChance -= 25;
        }

        attackChance = Math.Clamp(attackChance, 5, 95);

        int roll = Random.Shared.Next(1, 101);

        bool willCircle = roll <= attackChance;

        Debug.LogDetail($"{this.LastName} ShouldCircle: aggression={this.Aggression}, momentum={this.Momentum}×3, stamina={this.Stamina}/{this.Cardio}");
        Debug.LogDetail($"  attackChance={attackChance}%, roll={roll}% → {(willCircle ? "circles" : "attacks")}");

        return willCircle;
    }

    // Fight functions
    public void SetRoundScore(int score)
    {
        this.Score += score;
    }

    public int GetScore()
    {
        return this.Score;
    }

    // Internal combat helper functions
    private int GetAttackStats(Move move)
    {
        Random random = new Random();
        int roll = random.Next(-5, 6);

        int modifier;

        switch(move.Type)
        {
            

            case MoveType.Punch:
                modifier = (this.Striking - 100) / 5;
                break;
            case MoveType.Kick:
                modifier = (this.Kicking - 100) / 5;
                break;
            default:
                modifier = 0;
                break;
        }

        int staminaMod = (int)((1.0 - (double)this.Stamina/this.Cardio) * -10);

        int total = roll + modifier + move.AccuracyModifier + staminaMod;

        Debug.LogDetail($"  {this.LastName} attack ({move.Name}): roll={roll}, statMod={modifier}, accuracy={move.AccuracyModifier} → {total}");

        return total;
    }

    private int GetDefenseStats(Move move)
    {
        Random random = new Random();
        int roll = random.Next(-5, 6);

        int modifier;

        switch(move.Type)
        {
            

            case MoveType.Punch:
                modifier = (this.StrikingDefense - 100) / 5;
                break;
            case MoveType.Kick:
                modifier = (this.KickingDefense - 100) / 5;
                break;
            default:
                modifier = 0;
                break;
        }
        int staminaMod = (int)((1.0 - (double)this.Stamina/this.Cardio) * -10);
        int total = roll + modifier + staminaMod;

        Debug.LogDetail($"  {this.LastName} defense ({move.Name}): roll={roll}, statMod={modifier} → {total}");

        return total;
    }
}