using System.Collections.Concurrent;

public class Move
{
   public string Name;

   public int Damage;
   public int StaminaCost;

   public MoveType Type;
   public List<BodypartType> TargetBodyparts;
   public int AccuracyModifier;
   public bool Significant;


   public Move(string name, int damage, int staminaCost, MoveType type, List<BodypartType> targetBodyparts, int accuracyModifier = 0, bool significant = true)
   {
      this.Name = name;
      this.Damage = damage;
      this.StaminaCost = staminaCost;
      
      this.Type = type;

      this.TargetBodyparts = targetBodyparts;
      this.AccuracyModifier = accuracyModifier;

      this.Significant = significant;
   }

   
}

