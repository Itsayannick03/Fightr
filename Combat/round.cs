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

    private double GetRoundScore(Dictionary<StatType, int> roundStats, string fighterName)
    {
        double roundScore = 0;
        foreach (var stat in roundStats)
        {
            StatType type = stat.Key;
            int quantity = stat.Value;

            double weight = StatTypeHelper.ScoreWeights[type];

            double contribution = weight * quantity;
            roundScore += contribution;

            Debug.LogDetail($"  {fighterName} stat {type}: {quantity} × {weight:N1} = {contribution:N1}");
        }

        Debug.LogDetail($"  {fighterName} round score: {roundScore:N1}");

        return roundScore;
    }
   

   

    private void DetermainRoundWinner(double fighter1Score, double fighter2Score)
    {

        Debug.Log($"{this.Fighter1.LastName} round score: {fighter1Score:N1}, {this.Fighter2.LastName} round score: {fighter2Score:N1}");

        double diff = Math.Abs(fighter1Score - fighter2Score);
        if(diff <= 5)
        {
            this.Winner = null;
            Debug.Log("Round result: Draw");
        }
        else if(fighter1Score > fighter2Score)
        {
            this.Winner = this.Fighter1;
            Debug.Log($"Round result: {this.Fighter1.LastName} wins");
        }
        else
        {
            this.Winner = this.Fighter2;
            Debug.Log($"Round result: {this.Fighter2.LastName} wins");
        }

        
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
        Debug.Log($"Round {this.RoundNumber} starts — Timer: 5:00");
        
        while(!timer.IsTimeOut())
        {
            Console.Write($"(Round: {RoundNumber})");
            timer.PrintTime();
            Debug.LogDetail($"Timer before exchange: {timer.GetRemaining() / 60}:{(timer.GetRemaining() % 60):D2}");

            Exchange exchange = new Exchange(this.Fighter1, this.Fighter2);

            ExchangeSummary summary = exchange.Run();

            int time = summary.TimeTaken;
            ExchangeOutcome result = summary.Result;
            
            timer.ReduceTime(time);
            Debug.LogDetail($"Timer after exchange: {timer.GetRemaining() / 60}:{(timer.GetRemaining() % 60):D2} (elapsed: {time}s)");

            if(Debug.StepThrough)
            {
                while(true)
                {
                    Console.Write("  Press Enter to continue, s for stats...");
                    string input = Console.ReadLine() ?? "";
                    if(input == "") break;
                    else if(input.ToLower() == "s")
                    {
                        Fighter1.GetFighterInfo();
                        Fighter2.GetFighterInfo();
                    }
                }
            }

            if(result == ExchangeOutcome.Knockout)
            {
                //Console.WriteLine("#### KOCKOUT ####");
                Debug.Log($"KNOCKOUT! {this.Winner?.LastName ?? "Unknown"} wins by KO!");
                if(this.Fighter1.IsKnockedOut())
                    this.Winner = this.Fighter2;
                else
                    this.Winner = this.Fighter1;

                return RoundResult.Knockout;
            }
                    //Thread.Sleep(1000);
        }


        Debug.Log($"Round {this.RoundNumber} time expired");

        this.ComputeRoundDelta(this.Fighter1Stats, this.Fighter1);
      

        this.ComputeRoundDelta(this.Fighter2Stats, this.Fighter2);
      

        Debug.Log($"Computing round {this.RoundNumber} scores:");
        double fighter1Score = this.GetRoundScore(Fighter1Stats, this.Fighter1.LastName);
        double fighter2Score = this.GetRoundScore(Fighter2Stats, this.Fighter2.LastName);

        this.DetermainRoundWinner(fighter1Score, fighter2Score);

        Fighter1.GetFighterInfo();
        Fighter2.GetFighterInfo();

        const int bigWinNumber = 60;
        if((fighter1Score - fighter2Score > bigWinNumber && fighter1Score > fighter2Score * 3) || (fighter2Score - fighter1Score > bigWinNumber && fighter2Score > fighter1Score * 3) )
        {
            string winnerName = fighter1Score > fighter2Score ? Fighter1.LastName : Fighter2.LastName;
            string loserName = fighter1Score > fighter2Score ? Fighter2.LastName : Fighter1.LastName;
            Console.WriteLine($"  {winnerName} 10 - 8 {loserName}");
            Debug.Log("Dominant win");
            return RoundResult.DominantWin;
        }

        if(this.Winner == this.Fighter1)
        {
            Console.WriteLine($"  {Fighter1.LastName} 10 - 9 {Fighter2.LastName}");
            return RoundResult.Fighter1Win;
        }
        else if(this.Winner == this.Fighter2)
        {
            Console.WriteLine($"  {Fighter2.LastName} 10 - 9 {Fighter1.LastName}");
            return RoundResult.Fighter2Win;
        }
        else
        {
            Console.WriteLine("  10 - 10 (draw)");
            return RoundResult.Draw;
        }

        
    }
}