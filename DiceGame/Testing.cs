using System.Diagnostics;

namespace DiceGame
{
    internal static class Testing
    {
        /// <summary>
        /// Static Testing class which instantiates other classes and tests them.
        /// Only class with file access. Can write to a file stored in the same directory with testing info.
        /// </summary>
        private static string FilePath = Directory.GetCurrentDirectory() + "\\Log.txt";
        public static void StartTests()
        {
            Console.WriteLine("\nStarting Tests...");

            StreamWriter SW;

            try
            {
                SW = new StreamWriter(FilePath, append : true);
                SW.WriteLine("Testing started at " + DateTime.UtcNow + "...\n");
            }
            catch (Exception NewException)
            {
                Console.WriteLine("Error: StreamWriter couldn't open the file at location " + FilePath + "!");
                Console.WriteLine(NewException.Message);
                return;
            }
            const int TestIterations = 1000;
            int TestFails;

            Console.WriteLine("\nTesting die class...");

            TestFails = TestDie(TestIterations, SW);

            Console.WriteLine("\nDie class failed " + TestFails + " out of " + TestIterations + " tests.");

            Console.WriteLine("\nTesting SevensOut class...");

            TestFails = TestSevensOut(TestIterations, SW);

            Console.WriteLine("\nSevensOut class failed " + TestFails + " out of " + TestIterations + " tests.");

            Console.WriteLine("\nTesting ThreeOrMore class...");

            (TestFails, int Doubles, int Triples, int Quadruples, int Quintuples) = TestThreeOrMore(TestIterations, SW);

            Console.WriteLine("\nThreeOrMore class failed " + TestFails + " out of " + TestIterations + " tests.");
            Console.WriteLine(TestIterations + " tests yielded " + Doubles + " 2-of-a-kinds.");
            Console.WriteLine(TestIterations + " tests yielded " + Triples + " 3-of-a-kinds.");
            Console.WriteLine(TestIterations + " tests yielded " + Quadruples + " 4-of-a-kinds.");
            Console.WriteLine(TestIterations + " tests yielded " + Quintuples + " 5-of-a-kinds.");

            Console.WriteLine("\nTesting concluded. See " + FilePath + " for details.");

            SW.WriteLine("Testing concluded successfully.\n");
            SW.WriteLine("--------------------------\n");
            SW.Close();
        }
        private static int TestDie(int TestIterations, StreamWriter SW)
        {
            int TestFails = 0;
            Die TestDie = new Die();

            for (int Index = 0; Index < TestIterations; Index++)
            {
                int RollValue = TestDie.Roll();
                bool TestResult = CheckRollValue(RollValue);
                Debug.Assert(TestResult, "Error: die rolled a " + RollValue + " during testing...");
                if (!TestResult)
                {
                    SW.WriteLine("Error: die rolled a " + RollValue + " during testing...");
                    SW.WriteLine("(Test " + Index + " of " + TestIterations + ")\n");
                    TestFails++;
                }
            }

            return TestFails;
        }
        private static int TestSevensOut(int TestIterations, StreamWriter SW)
        {
            int TestFails = 0;
            Game TestSevensOut = new SevensOut();

            for (int Index = 0; Index < TestIterations; Index++)
            {
                List<Die> NewDice = TestSevensOut.GetDice();
                foreach (Die NewDie in NewDice)
                {
                    int RollValue = NewDie.Roll();
                    bool TestResult = CheckRollValue(RollValue);
                    Debug.Assert(TestResult, "Error: die rolled a " + RollValue + " during testing...");
                    if (!TestResult)
                    {
                        SW.WriteLine("Error: die rolled a " + RollValue + " during testing...");
                        SW.WriteLine("(Test " + Index + " of " + TestIterations + ")\n");
                        TestFails++;
                    }
                }

                try
                {
                    int TotalValue = NewDice[0].Value + NewDice[1].Value;
                    bool TestResult = CheckTotalValue(NewDice[0].Value, NewDice[1].Value, TotalValue);

                    Debug.WriteLineIf(!TestResult, "\"Error: SevensOut class returned invalid total \" + TotalValue + \" during testing...\"");
                    Trace.Assert(TestResult);
                    if (!TestResult)
                    {
                        SW.WriteLine("Error: SevensOut class returned invalid total " + TotalValue + " during testing...");
                        SW.WriteLine("(Test " + Index + " of " + TestIterations + ")\n");
                    }
                }
                catch (Exception NewException) // Runs if two dice are not present in the Game object. Not possible currently, but there for if the code is repurposed in future (DRY Principle).
                {
                    Debug.Assert(false, NewException.Message);
                    SW.WriteLine("Error: encountered error during testing...");
                    SW.WriteLine(NewException.Message);
                    SW.WriteLine("(Test " + Index + " of " + TestIterations + ")\n");
                }
            }

            bool SevenFound = false;

            for (int Index = 0; Index < TestIterations; Index++)
            {
                List<Die> NewDice = TestSevensOut.GetDice();
                int TotalValue = 0;
                try
                {
                    TotalValue = NewDice[0].Roll() + NewDice[1].Roll();
                }
                catch (Exception NewException) // Used in case not enough dice or smthn.
                {
                    Debug.Assert(false, NewException.Message);
                    SW.WriteLine("Error: encountered error during testing...");
                    SW.WriteLine(NewException.Message);
                }

                if (TotalValue == 7) { SevenFound = true; }
                
                if (SevenFound)
                {
                    Console.WriteLine("SevensOut rolled a 7 after " + Index + " tests...");
                    SW.WriteLine("SevensOut rolled a 7 after " + Index + " tests...");
                    break;
                }
            }

            if (!SevenFound)
            {
                Console.WriteLine("SevensOut didn't roll a 7 after " + TestIterations + " tests...");
                SW.WriteLine("SevensOut didn't roll a 7 after " + TestIterations + " tests...");
            }

            return TestFails;
        }
        private static (int, int, int, int, int) TestThreeOrMore(int TestIterations, StreamWriter SW)
        {
            int TestFails = 0;

            ThreeOrMore TestThreeOrMore = new ThreeOrMore();
            int Doubles = 0, Triples = 0, Quadruples = 0, Quintuples = 0;

            for (int Index = 0; Index < TestIterations; Index++)
            {
                List<Die> NewDice = TestThreeOrMore.GetDice();
                foreach (Die NewDie in NewDice)
                {
                    int RollValue = NewDie.Roll();
                    bool TestResult = CheckRollValue(RollValue);
                    Debug.Assert(TestResult, "Error: die rolled a " + RollValue + " during testing...");
                    if (!TestResult)
                    {
                        SW.WriteLine("Error: die rolled a " + RollValue + " during testing...");
                        SW.WriteLine("(Test " + Index + " of " + TestIterations + ")\n");
                        TestFails++;
                    }
                }

                try
                {
                    int TotalValue = NewDice[0].Value + NewDice[1].Value + NewDice[2].Value + NewDice[3].Value + NewDice[4].Value;
                    bool TestResult = CheckTotalValue(NewDice[0].Value, NewDice[1].Value, NewDice[2].Value, NewDice[3].Value, NewDice[4].Value, TotalValue);
                    Debug.Assert(TestResult, "Error: ThreeOrMore class returned invalid total " + TotalValue + " during testing...");
                    if (!TestResult)
                    {
                        SW.WriteLine("Error: ThreeOrMore class returned invalid total " + TotalValue + " during testing...");
                        SW.WriteLine("(Test " + Index + " of " + TestIterations + ")\n");
                    }

                    List<int> RollValues = new List<int>();
                    foreach (Die NewDie in NewDice) { RollValues.Add(NewDie.Value); }
                    List<(int, int)> TestMatches = TestThreeOrMore.FindMatches(RollValues);

                    foreach ((int, int) TestMatch in TestMatches)
                    {
                        switch (TestMatch.Item2)
                        {
                            case 2:
                                Doubles++;
                                break;
                            case 3:
                                Triples++;
                                break;
                            case 4:
                                Quadruples++;
                                break;
                            case 5:
                                Quintuples++;
                                break;
                        }
                    }
                }
                catch (Exception NewException) // Runs if five dice are not present in the Game object. Not possible currently, but there for if the code is repurposed in future (DRY Principle).
                {
                    Debug.Assert(false, NewException.Message);
                    SW.WriteLine("Error: encountered error during testing...");
                    SW.WriteLine(NewException.Message);
                    SW.WriteLine("(Test " + Index + " of " + TestIterations + ")\n");
                }
            }

            (int, int) TestScores = (0, 0);
            bool TestTurn = true;
            bool HasWon = false;

            for (int Index = 0; Index < TestIterations; Index++)
            {
                List<(int, int)> TestMatches = TestThreeOrMore.FindMatches(TestThreeOrMore.RollDice());

                int HighestMatch = 1;

                foreach ((int, int) TestMatch in TestMatches)
                {
                    if (TestMatch.Item2 > HighestMatch) { HighestMatch = TestMatch.Item2; }
                }

                (int, int) NewTestScores = TestThreeOrMore.UpdateScores(TestScores, TestMatches, TestTurn);

                if (TestTurn)
                {
                    if (HighestMatch == 3)
                    {
                        if (NewTestScores.Item1 != TestScores.Item1 + 3)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                    else if (HighestMatch == 4)
                    {
                        if (NewTestScores.Item1 != TestScores.Item1 + 6)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                    else if (HighestMatch == 5)
                    {
                        if (NewTestScores.Item1 != TestScores.Item1 + 12)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                    else
                    {
                        if (NewTestScores.Item1 != TestScores.Item1)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                }
                else
                {
                    if (HighestMatch == 3)
                    {
                        if (NewTestScores.Item2 != TestScores.Item2 + 3)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                    else if (HighestMatch == 4)
                    {
                        if (NewTestScores.Item2 != TestScores.Item2 + 6)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                    else if (HighestMatch == 5)
                    {
                        if (NewTestScores.Item2 != TestScores.Item2 + 12)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                    else
                    {
                        if (NewTestScores.Item2 != TestScores.Item2)
                        {
                            Console.WriteLine("\nError: discrepancy in adding scores for ThreeOrMore during testing...");
                            SW.WriteLine("Error: discrepancy in adding scores for ThreeOrMore during testing...");
                        }
                    }
                }

                TestScores = NewTestScores;

                TestTurn = !TestTurn;

                if (TestScores.Item1 >= 20 || TestScores.Item2 >= 20)
                {
                    Console.WriteLine("ThreeOrMore reached 20 after " + Index + " tests...");
                    SW.WriteLine("ThreeOrMore reached 20 after " + Index + " tests...");
                    HasWon = true;
                    break;
                }
            }

            if (!HasWon)
            {
                Console.WriteLine("ThreeOrMore didn't reach 20 after " + TestIterations + " tests...");
                SW.WriteLine("ThreeOrMore didn't reach 20 after " + TestIterations + " tests...");
            }

            return (TestFails, Doubles, Triples, Quadruples, Quintuples);
        }
        static bool CheckRollValue(int rollValue)
        { // Boolean method that checks if the value from a dice roll is within the expected bounds.
            if (rollValue < 1 || rollValue > 6) { return false; } // Returns false if the value is higher than six or lower than one.
            else { return true; } // Returns true if the value is between six and one.
        }
        static bool CheckTotalValue(int Roll1, int Roll2, int Total)
        { // Boolean overload that checks if the total from two dice rolls is within the expected bounds.
            if (Total < 2 || Total > 12 || Total != Roll1 + Roll2) { return false; } // Returns false if the value is higher than twelve or lower than two or not the sum of the rolls.
            else { return true; } // Returns true if the value is valid.
        }
        static bool CheckTotalValue(int Roll1, int Roll2, int Roll3, int Roll4, int Roll5, int Total)
        { // Boolean overload that checks if the total from five dice rolls is within the expected bounds.
            if (Total < 5 || Total > 30 || Total != Roll1 + Roll2 + Roll3 + Roll4 + Roll5) { return false; } // Returns false if the value is higher than thirty or lower than five or not the sum of the rolls.
            else { return true; } // Returns true if the value is valid.
        }
    }
}