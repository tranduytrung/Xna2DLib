
using System;

namespace tranduytrung.Xna.Map
{
    public struct IsometricCoords : IEquatable<IsometricCoords>
    {
        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public int X;
        public int Y;

        public IsometricCoords(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(IsometricCoords lhs, IsometricCoords rhs)
        {
            return rhs.X == lhs.X && rhs.Y == lhs.Y;
        }

        public static bool operator !=(IsometricCoords lhs, IsometricCoords rhs)
        {
            return rhs.X != lhs.X || rhs.Y != lhs.Y;
        }

        public static IsometricCoords operator +(IsometricCoords lhs, IsometricCoords rhs)
        {
            return new IsometricCoords(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static IsometricCoords operator -(IsometricCoords lhs, IsometricCoords rhs)
        {
            return new IsometricCoords(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public bool Equals(IsometricCoords other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IsometricCoords && Equals((IsometricCoords) obj);
        }
    }
}
