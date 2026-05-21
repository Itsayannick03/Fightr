public class Fight
{
    private int Rounds;
    private Fighter Fighter1;
    private Fighter Fighter2;

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

    private Fighter? GetWinnerByDecision()
    {
        int fighter1Score = this.Fighter1.GetScore();
        int fighter2Score = this.Fighter2.GetScore();

        if (fighter1Score > fighter2Score)
        {
            return Fighter1;
        }
        else if (fighter2Score > fighter1Score)
        {
            return Fighter2;
        }

        return null; // draw
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

    public void Run()
    {
        Fighter? winner = null;
        bool fightEndedEarly = false;

        int currentRound = 1;

        while (currentRound <= Rounds)
        {
            Round round = new Round(currentRound, this.Fighter1, this.Fighter2);
            RoundResult result = round.run();

            if (IsFightEndingResult(result))
            {
                Console.WriteLine("#### KNOCKOUT ####");

                winner = GetWinnerByKnockout();
                fightEndedEarly = true;

                break;
            }

            this.HandleRoundResult(result);

            if (currentRound < Rounds)
            {
                this.Fighter1.Recover();
                this.Fighter2.Recover();
            }

            currentRound++;
        }

        if (!fightEndedEarly)
        {
            winner = GetWinnerByDecision();
        }

        this.PrintScore();
        this.PrintWinner(winner);
    }
}