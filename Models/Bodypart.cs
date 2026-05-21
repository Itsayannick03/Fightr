public class Bodypart
{
    public string Name;
    public int Health;
    public int Weight;
    private int InjuryThreshold;
    private int PotentialPenalty;
    public int InjuryPenalty = 0;
    

    public Bodypart(string name, int health, int weight, int injuryThreshold, int potentialPenalty)
    {
        this.Name = name;
        this.Health = health;
        this.Weight = weight;
        this.InjuryThreshold = injuryThreshold;
        this.PotentialPenalty = potentialPenalty;
    }

    public bool isInjured()
    {
        return this.Health < this.InjuryThreshold;
    }

    public void applyInjury()
    {
        this.InjuryPenalty = this.PotentialPenalty;
    }
}