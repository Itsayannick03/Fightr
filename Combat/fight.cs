public class Fight
{
    private int Rounds;
    private Fighter Fighter1;
    private Fighter Fighter2;
    Fighter? winner = null;
    Fighter? looser = null;
    RoundResult result;
    int currentRound;



    public Fight(int rounds, Fighter fighter1, Fighter fighter2)
    {
        this.Rounds = rounds;
        this.Fighter1 = fighter1;
        this.Fighter2 = fighter2;
    }

    private void HandleRoundResult(RoundResult result)
    {
        if (result == RoundResult.Fighter1Win)
        {
            Fighter1.SetRoundScore(10);
            Fighter2.SetRoundScore(9);
        }
        else if (result == RoundResult.Fighter2Win)
        {
            Fighter1.SetRoundScore(9);
            Fighter2.SetRoundScore(10);
        }
        else
        {
            Fighter1.SetRoundScore(10);
            Fighter2.SetRoundScore(10);
        }
    }

    private bool IsFightEndingResult(RoundResult result)
    {
        return result == RoundResult.Knockout;
    }

    private Fighter GetWinnerByKnockout()
    {
        if (Fighter1.IsKnockedOut())
        {
            return Fighter2;
        }

        return Fighter1;
    }

    private void GetWinnerByDecision()
    {
        int fighter1Score = this.Fighter1.GetScore();
        int fighter2Score = this.Fighter2.GetScore();

        if (fighter1Score > fighter2Score)
        {
            this.winner = Fighter1;
            this.looser = Fighter2;
            return;
        }
        else if (fighter2Score > fighter1Score)
        {
            this.winner = Fighter2;
            this.looser = Fighter1;
            return;
        }
        
        this.winner = null;
        this.looser = null;
        return;
    }

    private void PrintScore()
    {
        Console.WriteLine($"{this.Fighter1.FirstName} {this.Fighter1.LastName}: {this.Fighter1.GetScore()}");
        Console.WriteLine($"{this.Fighter2.FirstName} {this.Fighter2.LastName}: {this.Fighter2.GetScore()}");
    }

    private void PrintWinner(Fighter? winner)
    {
        if (winner == null)
        {
            Console.WriteLine("The fight is a draw!");
        }
        else
        {
            Console.WriteLine($"{winner.FirstName} {winner.LastName} wins!");
        }
    }

    private void PrintStoppageResult(Fighter? winner, Fighter? looser, RoundResult result, int roundNr)
    {
        Console.WriteLine($"Ladies and gentlemen, referee Herb Dean has called a stop to this contest\n at round number {roundNr}\ndeclaring the winner by knockout:\n{winner!.FirstName} {winner.LastName}");
    }

    private void PrintDecicionResult(Fighter? winner, Fighter? looser, int totalRounds)
    {
        if (winner == null)
        {
            Console.WriteLine($"Ladies and gentlemen, after {totalRounds} rounds, we go to the judges' scorecards... The judges have scored this contest a draw!");
            return;
        }
        Console.WriteLine($"Ladies and gentlemen, after {totalRounds} rounds, we go to the judges' scorecards for a decision.\nAll three judges score the contest {winner.GetScore()} - {looser!.GetScore()}\n declaring the winner by unanimous decision:\n{winner.FirstName} {winner.LastName}");    
    }

    public void Run()
    {
        bool fightEndedEarly = false;
        this.currentRound = 1;

        while (this.currentRound <= Rounds)
        {
            Round round = new Round(this.currentRound, this.Fighter1, this.Fighter2);
            this.result = round.run();

            if (IsFightEndingResult(result))
            {
                Console.WriteLine("#### KNOCKOUT ####");

                winner = round.Winner;
                looser = winner == Fighter1 ? Fighter2 : Fighter1;
                fightEndedEarly = true;

                break;
            }

            this.HandleRoundResult(result);

            if (this.currentRound < Rounds)
            {
                this.Fighter1.Recover();
                this.Fighter2.Recover();
            }

            this.currentRound++;
        }

        if (fightEndedEarly)
        {
            this.PrintStoppageResult(this.winner, this.looser, result, this.currentRound);
        }
        else
        {
            this.GetWinnerByDecision();
            this.PrintDecicionResult(this.winner, this.looser, this.Rounds);
        }

        if (winner == null)
        {
            PrintWinner(null);
        }
    }
}