using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.knapp.CodingContest.data
{
    public class Position
    {
        public int X { get; private set; }

        public int Y { get; private set; }


        public Position( int x, int y )
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Calculate the distance between this position and another position
        /// </summary>
        /// <param name="other">Otehr position</param>
        /// <returns>distance between this instance of Position and other</returns>
        public double CalculateDistance(Position other) => Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2 ) );

        public override bool Equals(object other )
        {
            return other is Position otherPosition &&
                    X == otherPosition.X
                    && Y == otherPosition.Y;
        }

        public override int GetHashCode()
        {
            const int prime = 31;

            int result = 1;
            result = (prime * result) + X;
            result = (prime * result) + Y;

            return result;
        }

        public override string ToString()
        {
            return $"Position[x={X}, y={Y}]";
        }

    }
}
