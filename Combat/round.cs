public class Round
{
    public int RoundNumber;
    Fighter Fighter1;
    Fighter Fighter2;
    public Fighter? Winner { get; private set; }
    private Dictionary<StatType, int> Fighter1Stats = new Dictionary<StatType, int>();
    private Dictionary<StatType, int> Fighter2Stats = new Dictionary<StatType, int>();


    
    public Round(int roundNumber, Fighter fighter1, Fighter fighter2)
    {
        this.RoundNumber = roundNumber;

        this.Fighter1 = fighter1;
        this.Fighter2 = fighter2;
        this.Winner = null;

        this.Fighter1Stats = new Dictionary<StatType, int>(fighter1.Stats);
        this.Fighter2Stats = new Dictionary<StatType, int>(fighter2.Stats);
  
    }

    private double GetRoundScore(Dictionary<StatType, int> roundStats)
    {
        double roundScore = 0;
        foreach (var stat in roundStats)
        {
            StatType type = stat.Key;
            int quantity = stat.Value;

            double weight = StatTypeHelper.ScoreWeights[type];

            roundScore += weight * quantity;

        }

        return roundScore;
    }
   

   

    private void DetermainRoundWinner(double fighter1Score, double fighter2Score)
    {
        // Console.WriteLine($"Fighter 1 round score: {fighter1Score}");
        // Console.WriteLine($"Fighter 2 round score: {fighter2Score}");
        // Console.ReadLine();

        if(fighter1Score == fighter2Score)
            this.Winner = null;
        else if(fighter1Score > fighter2Score)
            this.Winner = this.Fighter1;
        else
            this.Winner = this.Fighter2;
        
    }

    public void PrintCurrentTime(Timer timer)
    {
        timer.PrintTime();
    }

    


    // private void CommentRoundWinner(RoundWinner result)
    // {
    //     Console.Write("Commentary: ");

    //     switch(result)
    //     {
    //         case RoundWinner.fighter1:
    //             Console.WriteLine($"That was a nice round! I'd give that a 10-9 in favor of {this.Fighter1.LastName}");
    //             break;
    //         case RoundWinner.fighter2:
    //             Console.WriteLine($"That was a nice round! I'd give that a 10-9 in favor of {this.Fighter2.LastName}");
    //             break;
    //         case RoundWinner.draw:
    //             Console.WriteLine($"Thats a tough one! I'd Give that a 10-10");
    //             break;

    //     }
        
        
    // }

    private void ComputeRoundDelta(Dictionary<StatType, int> snapshot, Fighter fighter)
{
    foreach (var stat in snapshot)
    {
        snapshot[stat.Key] = fighter.Stats[stat.Key] - stat.Value;
    }

}

    public RoundResult run()
    {
        Timer timer = new Timer();
        

        Console.WriteLine($"Round: {this.RoundNumber}");
        
        while(!timer.IsTimeOut())
        {
            Console.Write($"(Round: {RoundNumber})");
            timer.PrintTime();
            Exchange exchange = new Exchange(this.Fighter1, this.Fighter2);

            ExchangeSummary summary = exchange.Run();

            int time = summary.TimeTaken;
            ExchangeOutcome result = summary.Result;
            
            timer.ReduceTime(time);

            if(result == ExchangeOutcome.Knockout)
            {
                //Console.WriteLine("#### KOCKOUT ####");
                if(this.Fighter1.IsKnockedOut())
                    this.Winner = this.Fighter2;
                else
                    this.Winner = this.Fighter1;

                return RoundResult.Knockout;
            }
                    //Thread.Sleep(1000);
        }


        

        this.ComputeRoundDelta(this.Fighter1Stats, this.Fighter1);
      

        this.ComputeRoundDelta(this.Fighter2Stats, this.Fighter2);
      

        double fighter1Score = this.GetRoundScore(Fighter1Stats);
        double fighter2Score = this.GetRoundScore(Fighter2Stats);

        this.DetermainRoundWinner(fighter1Score, fighter2Score);

        Fighter1.GetFighterInfo();
        Fighter2.GetFighterInfo();

        if(this.Winner == this.Fighter1)
            return RoundResult.Fighter1Win;
        else if(this.Winner == this.Fighter2)
            return RoundResult.Fighter2Win;
        else
            return RoundResult.Draw;

        
    }
}