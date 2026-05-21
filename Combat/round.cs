public class Round
{
    int RoundNumber;
    Fighter Fighter1;
    Fighter Fighter2;
    Fighter? Winner;

    private Dictionary<Fighter, RoundFighterStats> Stats;

    
    public Round(int roundNumber, Fighter fighter1, Fighter fighter2)
    {
        this.RoundNumber = roundNumber;

        this.Fighter1 = fighter1;
        this.Fighter2 = fighter2;

        this.Winner = null;

        this.Stats = new Dictionary<Fighter, RoundFighterStats>
        {
            { fighter1, new RoundFighterStats() },
            { fighter2, new RoundFighterStats() }
        };
    }

    private enum RoundWinner
    {
        fighter1,
        fighter2,
        draw
    }

    private void UpdateStats(Fighter attacker, ExchangeSummary summary)
    {
        Stats[attacker].ApplyExchangeSummary(summary);
    }

    private RoundWinner DetermainRoundWinner()
    {
        
        double fighter1Score = Stats[Fighter1].GetScoreValue();
        double fighter2Score = Stats[Fighter2].GetScoreValue();

        Console.WriteLine($"{this.Fighter1.LastName}: {fighter1Score}");
        Console.WriteLine($"{this.Fighter2.LastName}: {fighter2Score}");

        //Thread.Sleep(5000);


        if (fighter1Score > fighter2Score)
        {
            return RoundWinner.fighter1;
        }
        else if (fighter2Score > fighter1Score)
        {
            return RoundWinner.fighter2;
        }
        else
        {
            return RoundWinner.draw;
        }
    }

    


    private void CommentRoundWinner(RoundWinner result)
    {
        Console.Write("Commentary: ");

        switch(result)
        {
            case RoundWinner.fighter1:
                Console.WriteLine($"That was a nice round! I'd give that a 10-9 in favor of {this.Fighter1.LastName}");
                break;
            case RoundWinner.fighter2:
                Console.WriteLine($"That was a nice round! I'd give that a 10-9 in favor of {this.Fighter2.LastName}");
                break;
            case RoundWinner.draw:
                Console.WriteLine($"Thats a tough one! I'd Give that a 10-10");
                break;

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
                return RoundResult.Knockout;
            }
            
            this.UpdateStats(summary.Attacker, summary);
            //Thread.Sleep(1000);
        }

        RoundWinner winner = this.DetermainRoundWinner();

        Fighter1.GetFighterInfo();
        Fighter2.GetFighterInfo();

        if(winner == RoundWinner.fighter1)
            return RoundResult.Fighter1Win;
        else if(winner == RoundWinner.fighter2)
            return RoundResult.Fighter2Win;
        else
            return RoundResult.Draw;

        
    }
}