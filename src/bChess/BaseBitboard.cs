using System;
using System.Text;

namespace bChess
{
    public class BaseBitboard
    {
        public ulong Value { get; }

        public BaseBitboard(ulong bitboard)
        {
            this.Value = bitboard;
        }

        public BaseBitboard(BaseBitboard bitboard)
        {
            this.Value = bitboard.Value;
        }

        public static BaseBitboard operator |(BaseBitboard a, BaseBitboard b)
        {
            return new BaseBitboard(a.Value | b.Value);
        }

        public static BaseBitboard operator &(BaseBitboard a, BaseBitboard b)
        {
            return new BaseBitboard(a.Value & b.Value);
        }

        public static BaseBitboard operator |(BaseBitboard a, ulong b)
        {
            return new BaseBitboard(a.Value | b);
        }

        public static BaseBitboard operator &(BaseBitboard a, ulong b)
        {
            return new BaseBitboard(a.Value & b);
        }

        public static bool operator ==(BaseBitboard a, BaseBitboard b)
        {
            return a.Value == b.Value;
        }

        public static bool operator ==(BaseBitboard a, ulong b)
        {
            return a.Value == b;
        }

        public static bool operator !=(BaseBitboard a, ulong b)
        {
            return !(a.Value == b);
        }

        public static bool operator !=(BaseBitboard a, BaseBitboard b)
        {
            return !(a.Value == b.Value);
        }

        public static int GetIndex(byte position)
        {
            return ((position >> 4) * 8) + (7 - (position & 7));
        }

        public static ulong GetMask(byte position)
        {
            return 0x1ul << GetIndex(position);
        }

        public bool IsSet(byte position)
        {
            ulong mask = GetMask(position);
            return (Value & mask) == mask;
        }
        
        public BaseBitboard Set(byte position)
        {
            return new BaseBitboard(Value | GetMask(position));
        }

        public BaseBitboard Reset(byte position)
        {
            return new BaseBitboard(Value & ~(GetMask(position)));
        }

        public BaseBitboard Rotate()
        {
            return FlipVertical().MirrorHorizontal();
        }

        public BaseBitboard FlipVertical() 
        {
            const ulong k1 = 0x00FF00FF00FF00FF;
            const ulong k2 = 0x0000FFFF0000FFFF;
            ulong x = Value;
            x = ((x >>  8) & k1) | ((x & k1) <<  8);
            x = ((x >> 16) & k2) | ((x & k2) << 16);
            x = ( x >> 32)       | ( x       << 32);
            return new BaseBitboard(x);
        }

        public BaseBitboard MirrorHorizontal() 
        {
            const ulong k1 = 0x5555555555555555;
            const ulong k2 = 0x3333333333333333;
            const ulong k4 = 0x0f0f0f0f0f0f0f0f;
            ulong x = Value;
            x = ((x >> 1) & k1) | ((x & k1) << 1);
            x = ((x >> 2) & k2) | ((x & k2) << 2);
            x = ((x >> 4) & k4) | ((x & k4) << 4);
            return new BaseBitboard(x);
        }

        public static byte RotatePosition(byte position)
        {
            return Constants.Positions[63 - BaseBitboard.GetIndex(position)];
        }

        public override bool Equals(object obj)
        {
            return (obj as BaseBitboard)?.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = Constants.BoardSize - 1; i >= 0; i--)
            {
                if ((i + 1) % 8 == 0)
                    sb.AppendLine();

                int val = (Value & (0x1ul << i)) == 0x1ul << i ? 1 : 0;
                sb.Append(val + " ");
            }

            return sb.ToString();
        }

        public int Count
        {
            get
            {
                int count = 0;
                ulong value = Value;

                while (value != 0)
                {
                    count++;
                    value &= value - 1;
                }
                return count;
            }
        }
    }
}