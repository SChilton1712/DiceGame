using System.ComponentModel.DataAnnotations;

namespace DiceGame
{
    internal interface Game
    {
        /// <summary>
        /// Main class which runs when the program runs.
        /// Acts as the baseclass from which the other games derive.
        /// Converted to an interface with abstract methods.
        /// </summary>
        static void Main(string[] args)
        {
            Game CurrentGame; // Instantiates a new Game object.
            char Input;
            do
            {
                try
                {
                    int Score = -1;
                    Console.Write("'7' to play Seven's Out, '3' to play Three Or More, 'v' to view statistics, 't' to run tests, 'e' to exit: ");
                    Input = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    bool PlayAgain;
                    switch (Input)
                    {
                        case '7':
                            do
                            {
                                PlayAgain = false;
                                CurrentGame = new SevensOut();
                                Score = CurrentGame.Play();
                                if (Score > -1) { Statistics.SetHighscore(typeof(SevensOut), Score); }
                                Statistics.IncrementTimesPlayed(typeof(SevensOut));
                                bool ValidInput = false;
                                do
                                {
                                    try
                                    {
                                        Console.Write("'r' to replay, 'e' to exit: ");
                                        Input = Console.ReadKey().KeyChar;
                                        Console.WriteLine();
                                        switch (Input)
                                        {
                                            case 'r':
                                            case 'R':
                                                PlayAgain = true;
                                                ValidInput = true;
                                                break;
                                            case 'e':
                                            case 'E':
                                                PlayAgain = false;
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
                            while (PlayAgain);
                            break;
                        case '3':
                            do
                            {
                                PlayAgain = false;
                                CurrentGame = new ThreeOrMore();
                                Score = CurrentGame.Play();
                                if (Score > -1) { Statistics.SetHighscore(typeof(ThreeOrMore), Score); }
                                Statistics.IncrementTimesPlayed(typeof(ThreeOrMore));
                                bool ValidInput = false;
                                do
                                {
                                    try
                                    {
                                        Console.Write("'r' to replay, 'e' to exit: ");
                                        Input = Console.ReadKey().KeyChar;
                                        Console.WriteLine();
                                        switch (Input)
                                        {
                                            case 'r':
                                            case 'R':
                                                PlayAgain = true;
                                                ValidInput = true;
                                                break;
                                            case 'e':
                                            case 'E':
                                                PlayAgain = false;
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
                            while (PlayAgain);
                            break;
                        case 'v':
                        case 'V':
                            Console.WriteLine("\nHighscores:");

                            List<(Type, int)> NewScores = Statistics.GetHighscores();
                            foreach ((Type, int) NewScore in NewScores)
                            {
                                Console.WriteLine(NewScore.Item1.Name + ": " + NewScore.Item2);
                            }

                            Console.WriteLine("\nTimes Played:");

                            List<(Type, int)> NewTimesPlayed = Statistics.GetTimesPlayed();
                            foreach ((Type, int) NewGame in NewTimesPlayed)
                            {
                                Console.WriteLine(NewGame.Item1.Name + ": " + NewGame.Item2);
                            }

                            Console.WriteLine("\nWins Against COM:");

                            List<(Type, int)> NewWinsAgainstCOM = Statistics.GetWinsAgainstCOM();
                            foreach ((Type, int) NewWins in NewWinsAgainstCOM)
                            {
                                Console.WriteLine(NewWins.Item1.Name + ": " + NewWins.Item2);
                            }

                            break;
                        case 't':
                        case 'T':
                            Testing.StartTests();
                            break;
                        case 'e':
                        case 'E':
                            Environment.Exit(0);
                            break;
                        default:
                            throw new Exception("Error: invalid input!");
                    }
                    Console.WriteLine();
                }
                catch (Exception NewException)
                {
                    Console.WriteLine(NewException.Message);
                }
            }
            while (true); // Loops execution until the user inputs 'e' to exit the program.
        }
        public abstract List<Die> GetDice();
        protected abstract int Play();
        protected abstract List<int> RollDice();
        protected abstract List<int> SortDice(List<int> OldList);
        protected virtual void WonAgainstCOM()
        { Statistics.IncrementWinsAgainstCOM(GetType()); }
    }
}