using System.Text.RegularExpressions;

namespace DiceGame
{
    internal class ThreeOrMore : Game
    {
        /// <summary>
        /// Derives from Game baseclass and implements ThreeOrMore game.
        /// </summary>
        List<Die> Dice = new List<Die>();
        public ThreeOrMore()
        {
            Dice.Clear();
            for (int Index = 0; Index < 5; Index++)
            {
                Dice.Add(new Die());
            }
        }
        public List<Die> GetDice()
        {
            return Dice;
        }
        public int Play()
        {
            Console.WriteLine("\nStarting game: " + GetType().Name);

            char Input;
            bool Exit = false;
            bool COM = false;

            Console.WriteLine("This game can be played with one player or two players.");

            do
            {
                Console.Write("'1' for one player, '2' for two players: ");
                Input = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (Input == '1')
                {
                    COM = true;
                    break;
                }
                else if (Input == '2')
                {
                    break;
                }
                Console.WriteLine("Invalid input!");
            }
            while (true);
            
            Console.Write("Press any key to play: ");
            Console.ReadKey();
            Console.WriteLine();

            bool PlayerTurn = false; // Inverted at start of game loop, starts on true.
            (int, int) Scores = (0, 0);

            do
            {
                PlayerTurn = !PlayerTurn;
                Console.WriteLine("\nRolling the dice:\n");

                List<int> Results = RollDice();
                Results = SortDice(Results);
                for (int Index = 0; Index < Results.Count; Index++)
                {
                    Console.Write("[" + Results[Index] + "] ");
                }
                Console.WriteLine("\b");

                List<(int, int)> Matches = FindMatches(Results);

                int HighestMatch = 1;
                for (int Index = 0; Index < Matches.Count; Index++)
                {
                    if (!PlayerTurn && COM) { Console.WriteLine("COM has a " + Matches[Index].Item2 + "-of-a-kind of " + Matches[Index].Item1 + "!"); }
                    else { Console.WriteLine("You have a " + Matches[Index].Item2 + "-of-a-kind of " + Matches[Index].Item1 + "!"); }
                    
                    if (Matches[Index].Item2 > HighestMatch) { HighestMatch = Matches[Index].Item2; }
                }

                if (HighestMatch == 2)
                {
                    if (!COM || PlayerTurn) { Console.WriteLine("\nYou can re-roll your dice if you choose."); }

                    int MatchValue = 0;
                    bool Proceed = false;
                    do
                    {
                        if (!COM || PlayerTurn)
                        {
                            try
                            {
                                Console.Write("Press a number to hold both dice of that value, or 'a' to re-roll all:"); // Ensures two 2-of-a-kind matches allow either to be held but not both.
                                Input = Console.ReadKey().KeyChar;
                                Console.WriteLine();
                                switch (Input)
                                {
                                    case '1':
                                        for (int Index = 0; Index < Matches.Count; Index++)
                                        {
                                            if (Matches[Index].Item1 == 1)
                                            {
                                                Proceed = true;
                                                MatchValue = 1;
                                                break;
                                            }
                                        }
                                        if (!Proceed) { Console.WriteLine("You can only hold dice that match."); }
                                        break;
                                    case '2':
                                        for (int Index = 0; Index < Matches.Count; Index++)
                                        {
                                            if (Matches[Index].Item1 == 2)
                                            {
                                                Proceed = true;
                                                MatchValue = 2;
                                                break;
                                            }
                                        }
                                        if (!Proceed) { Console.WriteLine("You can only hold dice that match."); }
                                        break;
                                    case '3':
                                        for (int Index = 0; Index < Matches.Count; Index++)
                                        {
                                            if (Matches[Index].Item1 == 3)
                                            {
                                                Proceed = true;
                                                MatchValue = 3;
                                                break;
                                            }
                                        }
                                        if (!Proceed) { Console.WriteLine("You can only hold dice that match."); }
                                        break;
                                    case '4':
                                        for (int Index = 0; Index < Matches.Count; Index++)
                                        {
                                            if (Matches[Index].Item1 == 4)
                                            {
                                                Proceed = true;
                                                MatchValue = 4;
                                                break;
                                            }
                                        }
                                        if (!Proceed) { Console.WriteLine("You can only hold dice that match."); }
                                        break;
                                    case '5':
                                        for (int Index = 0; Index < Matches.Count; Index++)
                                        {
                                            if (Matches[Index].Item1 == 5)
                                            {
                                                Proceed = true;
                                                MatchValue = 5;
                                                break;
                                            }
                                        }
                                        if (!Proceed) { Console.WriteLine("You can only hold dice that match."); }
                                        break;
                                    case '6':
                                        for (int Index = 0; Index < Matches.Count; Index++)
                                        {
                                            if (Matches[Index].Item1 == 6)
                                            {
                                                Proceed = true;
                                                MatchValue = 6;
                                                break;
                                            }
                                        }
                                        if (!Proceed) { Console.WriteLine("You can only hold dice that match."); }
                                        break;
                                    case 'a':
                                    case 'A':
                                        Proceed = true;
                                        break;
                                    default:
                                        throw new Exception("Error: invalid input!");
                                }
                            }
                            catch (Exception NewException)
                            {
                                Console.WriteLine(NewException.Message);
                            }
                        }
                        else
                        {
                            MatchValue = Matches[Matches.Count - 1].Item1; // COM always prefers higher numbers. No significance on gameplay, but could be modified to matter.
                            Console.WriteLine("COM has chosen to hold the dice containing " + MatchValue + ".");
                            Proceed = true;
                            Input = MatchValue.ToString()[0];
                        }
                    }
                    while (!Proceed);

                    Console.WriteLine("\nRolling the dice:\n");
                    Results = RollDice();
                    switch (Input)
                    {
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                            for (int Index = 0; Index < 2; Index++)
                            {
                                Results[Index] = MatchValue;
                            }
                            break;
                        default:
                            break;
                    }
                    Results = SortDice(Results);

                    for (int Index = 0; Index < Results.Count; Index++)
                    {
                        Console.Write("[" + Results[Index] + "] ");
                    }
                    Console.WriteLine("\b");

                    Matches = FindMatches(Results);

                    HighestMatch = 1;
                    for (int Index = 0; Index < Matches.Count; Index++)
                    {
                        if (!PlayerTurn && COM) { Console.WriteLine("COM has a " + Matches[Index].Item2 + "-of-a-kind of " + Matches[Index].Item1 + "!"); }
                        else { Console.WriteLine("You have a " + Matches[Index].Item2 + "-of-a-kind of " + Matches[Index].Item1 + "!"); }
                        if (Matches[Index].Item2 > HighestMatch) { HighestMatch = Matches[Index].Item2; }
                    }
                }

                Scores = UpdateScores(Scores, Matches, PlayerTurn);

                Console.WriteLine("\nThe current scores are:");
                Console.WriteLine("Player 1: " + Scores.Item1);
                if (COM) { Console.WriteLine("COM: " + Scores.Item2); }
                else { Console.WriteLine("Player 2: " + Scores.Item2); }
                

                if (Scores.Item1 >= 20)
                {
                    Console.WriteLine("Player 1 wins!");
                    if (COM) { WonAgainstCOM(); }
                    break;
                }
                else if (Scores.Item2 >= 20)
                {
                    if (COM) { Console.WriteLine("COM wins!"); }
                    else { Console.WriteLine("Player 2 wins!"); }
                    break;
                }
                else if (PlayerTurn && COM)
                {
                    Console.WriteLine("COM is taking its turn...");
                }
                Console.Write("\nPress any key to keep playing: ");
                Input = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }
            while (!Exit);

            Console.WriteLine("\nGame concluded with a score of " + Scores.Item1);
            Console.Write("Press any key to exit: ");

            Console.ReadKey();
            Console.WriteLine();

            return Scores.Item1;
        }
        public List<int> RollDice()
        {
            List<Die> NewDice = GetDice();
            List<int> Rolls = new List<int>();

            foreach (Die NewDie in NewDice)
            {
                Rolls.Add(NewDie.Roll());
            }

            return Rolls;
        }
        public List<int> SortDice(List<int> OldList)
        {
            if (OldList.Count < 2) { return OldList; }

            bool HasSwapped;
            do
            {
                HasSwapped = false;
                for (int Index = 1; Index < OldList.Count; Index++)
                {
                    if (OldList[Index - 1] > OldList[Index])
                    {
                        HasSwapped = true;
                        (OldList[Index - 1], OldList[Index]) = (OldList[Index], OldList[Index - 1]);
                    }
                }
            }
            while (HasSwapped);

            return OldList;
        }
        public List<(int, int)> FindMatches(List<int> Rolls)
        {
            List<(int, int)> NewList = new List<(int, int)>();

            for (int CheckValue = 1; CheckValue < 7; CheckValue++)
            {
                int Matches = 0;
                for (int Index = 0; Index < Rolls.Count; Index++)
                {
                    if (Rolls[Index] == CheckValue) { Matches++; }
                }
                if (Matches > 1) { NewList.Add((CheckValue, Matches)); }
            }

            return NewList;
        }
        public (int, int) UpdateScores ((int, int) Scores, List<(int, int)> Matches, bool PlayerTurn)
        {
            int ScoreFromResults = 0;
            foreach ((int, int) Match in Matches)
            {
                switch (Match.Item2)
                {
                    case 3:
                        ScoreFromResults += 3;
                        break;
                    case 4:
                        ScoreFromResults += 6;
                        break;
                    case 5:
                        ScoreFromResults += 12;
                        break;
                }
            }

            if (PlayerTurn) { Scores = (Scores.Item1 + ScoreFromResults, Scores.Item2); }
            else { Scores = (Scores.Item1, Scores.Item2 + ScoreFromResults); }
            return Scores;
        }
        public void WonAgainstCOM()
        {
            Statistics.IncrementWinsAgainstCOM(GetType());
        }
    }
}