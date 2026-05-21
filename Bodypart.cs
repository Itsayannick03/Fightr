public class Bodypart
{
    public string Name;
    public int Health;
    public int Weight;
    public bool IsInjured;

    public Bodypart(string name, int health, int weight)
    {
        this.Name = name;
        this.Health = health;
        this.Weight = weight;
        this.IsInjured = false;
    }
}