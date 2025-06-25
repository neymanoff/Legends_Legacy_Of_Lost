namespace Grid
{
    public struct HexCoord
    {
        public int Q; 
        public int R;
        public HexCoord(int q, int r) {Q = q; R = r;}

        public static HexCoord[] Directions =
        {
            new HexCoord(+1, 0), new HexCoord(+1, -1),
            new HexCoord(0, -1), new HexCoord(-1, 0),
            new HexCoord(-1, +1), new HexCoord(0, +1)
        };

        public HexCoord Neighbor(int i) => new HexCoord(Q + Directions[i].Q, R + Directions[i].R);
    }
}