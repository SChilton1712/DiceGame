namespace DiceGame
{
    internal class SevensOut : Game
    {
        /// <summary>
        /// Derives from Game baseclass and implements SevensOut game.
        /// </summary>
        List<Die> Dice = new List<Die>();
        public SevensOut()
        {
            Dice.Clear();
            for (int Index = 0; Index < 2; Index++)
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

            Console.Write("Press any key to play: ");
            Console.ReadKey();
            Console.WriteLine();

            int Score = 0;
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

            (int, int) Scores = (0, 0);

            do
            {
                Console.WriteLine("\nRolling your dice:\n");

                List<int> Results = RollDice();

                int Total = 0;
                for (int Index = 0; Index < Results.Count; Index++)
                {
                    Console.WriteLine("Die " + Index + " rolled a " + Results[Index] + "!");
                    Total += Results[Index];
                }

                Console.Write("\nYour total is " + Total + "! ");

                if (Total == 7)
                {
                    Console.WriteLine("Your turn is over!");
                    break;
                }
                else if (Results[0] == Results[1])
                {
                    Score += Total * 2;
                    Console.WriteLine("Double points! Your total score is " + Score + "!");
                }
                else
                {
                    Score += Total;
                    Console.WriteLine("Your total score is " + Score + "!");
                }
                bool ValidInput = false;
                do
                {
                    try
                    {
                        Console.Write("\n'r' to roll again, 'e' to exit: ");
                        Input = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        switch (Input)
                        {
                            case 'r':
                            case 'R':
                                Exit = false;
                                ValidInput = true;
                                break;
                            case 'e':
                            case 'E':
                                Exit = true;
                                ValidInput = true;
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
                while (!ValidInput);
            }
            while (!Exit);

            Scores.Item1 = Score;
            Console.WriteLine("\nPlayer 1's turn concluded with a score of " + Score);
            if (COM) { Console.WriteLine("Press any key to begin COM's turn: "); }
            else { Console.WriteLine("Press any key to begin Player 2's turn: "); }
            Console.ReadKey();

            Score = 0;

            do
            {
                Console.WriteLine("\nRolling your dice:\n");

                List<int> Results = RollDice();

                int Total = 0;
                for (int Index = 0; Index < Results.Count; Index++)
                {
                    Console.WriteLine("Die " + Index + " rolled a " + Results[Index] + "!");
                    Total += Results[Index];
                }

                Console.Write("\nYour total is " + Total + "! ");

                if (Total == 7)
                {
                    if (COM) { Console.WriteLine("COM's turn is over!"); }
                    else { Console.WriteLine("Your turn is over!"); }
                    break;
                }
                else if (Results[0] == Results[1])
                {
                    Score += Total * 2;
                    Console.WriteLine("Double points! Your total score is " + Score + "!");
                }
                else
                {
                    Score += Total;
                    Console.WriteLine("Your total score is " + Score + "!");
                }

                if (COM)
                {
                    Console.WriteLine("COM is rolling again...");
                }
                else
                {
                    bool ValidInput = false;
                    do
                    {
                        try
                        {
                            Console.Write("\n'r' to roll again, 'e' to exit: ");
                            Input = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            switch (Input)
                            {
                                case 'r':
                                case 'R':
                                    Exit = false;
                                    ValidInput = true;
                                    break;
                                case 'e':
                                case 'E':
                                    Exit = true;
                                    ValidInput = true;
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
                    while (!ValidInput);
                }
            }
            while (!Exit);

            Scores.Item2 = Score;

            Console.WriteLine("\nThe final scores are:");
            Console.WriteLine("Player 1: " + Scores.Item1);
            if (COM) { Console.WriteLine("COM: " + Scores.Item2); }
            else { Console.WriteLine("Player 2: " + Scores.Item2); }

            if (Scores.Item1 > Scores.Item2)
            {
                Console.WriteLine("Player 1 wins!");
                if (COM) { WonAgainstCOM(); }
            }
            else if (Scores.Item1 < Scores.Item2)
            {
                if (COM) { Console.WriteLine("COM wins!"); }
                else { Console.WriteLine("Player 2 wins!"); }
            }
            else { Console.WriteLine("It's a draw!"); }

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
        public void WonAgainstCOM()
        {
            Statistics.IncrementWinsAgainstCOM(GetType());
        }
    }
}