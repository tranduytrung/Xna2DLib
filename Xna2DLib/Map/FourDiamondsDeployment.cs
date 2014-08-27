using System;
using System.Collections.Generic;

namespace tranduytrung.Xna.Map
{
    public class FourDiamondsDeployment : IIsometricDeployable
    {
        public IsometricCoords Left { get; private set; }
        public IsometricCoords Right { get; private set; }
        public IsometricCoords Top { get; private set; }
        public IsometricCoords Bottom { get; private set; }
        public int Level { get; set; }
        public IEnumerable<IsometricCoords> Formation {
            get
            {
                yield return Top;
                yield return Left;
                yield return Right;
                yield return Bottom;
            }
        }
        public void Deploy(IsometricCoords position, double x = 0.5, double y = 0.5)
        {
            // Left or Right
            if (Math.Abs(0.5 - x) > Math.Abs(0.5 - y))
            {
                // if near left, then pivot is at right cell
                if (x < 0.5)
                {
                    Left = new IsometricCoords(position.X - 2, position.Y);
                    Right = position;
                    Top = new IsometricCoords(position.X - 1, position.Y - 1);
                    Bottom = new IsometricCoords(position.X - 1, position.Y + 1);
                }
                else // pivot is at left cell
                {
                    Left = position;
                    Right = new IsometricCoords(position.X + 2, position.Y);
                    Top = new IsometricCoords(position.X + 1, position.Y - 1);
                    Bottom = new IsometricCoords(position.X + 1, position.Y + 1);
                }
            }
            else // Top or Bottom
            {
                // if near top, then pivot is at lower cell
                if (y < 0.5)
                {
                    Top = new IsometricCoords(position.X, position.Y - 2);
                    Bottom = position;
                    Left = new IsometricCoords(position.X - 1, position.Y - 1);
                    Right = new IsometricCoords(position.X + 1, position.Y - 1);
                }
                else // pivot is at upper cell
                {
                    Top = position;
                    Bottom = new IsometricCoords(position.X, position.Y + 2);
                    Left = new IsometricCoords(position.X - 1, position.Y + 1);
                    Right = new IsometricCoords(position.X + 1, position.Y + 1);
                }
            }
        }
    }
}
