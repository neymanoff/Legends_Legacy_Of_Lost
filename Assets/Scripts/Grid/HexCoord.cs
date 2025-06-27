using System;

namespace Grid
{
    public readonly struct HexCoord : IEquatable<HexCoord>
    {
        public readonly int Q; 
        public readonly int R;
        public HexCoord(int q, int r) {Q = q; R = r;}

        private static readonly HexCoord[] Directions =
        {
            new HexCoord(+1, 0), new HexCoord(+1, -1),
            new HexCoord(0, -1), new HexCoord(-1, 0),
            new HexCoord(-1, +1), new HexCoord(0, +1)
        };

        public HexCoord Neighbor(int i) => new HexCoord(Q + Directions[i].Q, R + Directions[i].R);

        public bool Equals(HexCoord other)
        {
            return Q == other.Q && R == other.R;
        }

        public override bool Equals(object obj)
        {
            return obj is HexCoord other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Q, R);
        }
    }
}