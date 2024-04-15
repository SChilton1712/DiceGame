using System;

namespace DiceGame
{
    internal class Die
    {
        /// <summary>
        /// Simple 6-sided Die class.
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        } // Public access property for encapsulation.
        static Random _random = new Random(); // Instantiates a Random property to produce the random value.
        private int _value = 1; // Private encapsulated integer value.
        public int Roll()
        { // Public Roll() method that returns the result as an int. Result is also stored in the Value property.
            Value = _random.Next(1, 7); // Calls the method Next() to get a random int from 1 to 6 and assigns the it to the Value property.
            return Value; // Returns the value assigned to Value.
        }
    }
}