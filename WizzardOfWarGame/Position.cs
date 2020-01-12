using System;
using MessagePack;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    [MessagePackObject]
    public struct Position
    {
        [Key(0)]
        public int x;
        [Key(1)]
        public int y;

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   x == position.x &&
                   y == position.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

         public static bool operator ==(Position lhs, Position rhs)
        {
            return lhs.x == rhs.x && rhs.y == lhs.y;
        }

        public static bool operator !=(Position lhs, Position rhs)
        {
            return !(lhs == rhs);
        }
    }

   


}