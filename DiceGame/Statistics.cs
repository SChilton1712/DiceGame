using System.Diagnostics;

namespace DiceGame
{
    internal static class Statistics
    {
        /// <summary>
        /// Stores statistics as encapsulated members and implements public methods to access them.
        /// </summary>
        // Encapsulated private lists store values as tuples.
        private static List<(Type, int)> Highscores = new List<(Type, int)>
        {
            (typeof(SevensOut), 0),
            (typeof(ThreeOrMore), 0)
        };
        private static List<(Type, int)> TimesPlayed = new List<(Type, int)>
        {
            (typeof(SevensOut), 0),
            (typeof(ThreeOrMore), 0)
        };
        private static List<(Type, int)> WinsAgainstCOM = new List<(Type, int)>
        {
            (typeof(SevensOut), 0),
            (typeof(ThreeOrMore), 0)
        };

        // Public getter methods for each list.
        public static List<(Type, int)> GetHighscores()
        {
            return Highscores;
        }
        public static List<(Type, int)> GetTimesPlayed()
        {
            return TimesPlayed;
        }
        public static List<(Type, int)> GetWinsAgainstCOM()
        {
            return WinsAgainstCOM;
        }

        // Public overload getter which gets a specific list entry.
        public static List<(Type, int)> GetHighscores(Type NewType)
        {
            IEnumerable<(Type, int)> Entries =
                from Highscore in Highscores
                where Highscore.Item1 == NewType
                select Highscore;

            return Entries.ToList<(Type, int)>();
        }

        // Public setter methods, two of which increment a value by one while the other sets a new value instead.
        public static void SetHighscore(Type NewType, int NewScore)
        {
            IEnumerable<(Type, int)> Entries =
                from Highscore in Highscores
                where Highscore.Item1 == NewType && Highscore.Item2 < NewScore
                select Highscore;

            foreach ((Type, int) Highscore in Entries)
            {
                Highscores.Remove(Highscore);
                Highscores.Add((NewType, NewScore));
                Console.WriteLine("New Highscore!");
                return;
            }
        }
        public static void IncrementTimesPlayed(Type NewType)
        {
            IEnumerable<(Type, int)> Entries =
                from Game in TimesPlayed
                where Game.Item1 == NewType
                select Game;

            foreach ((Type, int) Game in Entries)
            {
                TimesPlayed.Remove(Game);
                TimesPlayed.Add((NewType, Game.Item2 + 1));
                return;
            }
        }
        public static void IncrementWinsAgainstCOM(Type NewType)
        {
            IEnumerable<(Type, int)> Entries =
                from Wins in WinsAgainstCOM
                where Wins.Item1 == NewType
                select Wins;

            foreach ((Type, int) Wins in Entries)
            {
                WinsAgainstCOM.Remove(Wins);
                WinsAgainstCOM.Add((NewType, Wins.Item2 + 1));
                return;
            }
        }
    }
}