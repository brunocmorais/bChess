namespace bChess
{
    public struct TranspositionTableInfo
    {
        public ulong Hash { get; set; }
        public int Depth { get; set; }
        public int Value { get; set; }
        public Color Color { get; set; }
        public Transposition Type { get; set; }

        public TranspositionTableInfo(ulong hash, int depth, int value, Color color, Transposition type)
        {
            Hash = hash;
            Depth = depth;
            Value = value;
            Color = color;
            Type = type;
        }
    }
}