using System;

namespace xsystem
{
    public class LittleEndianBitConverter
    {
        public static int ToInt16(byte[] value, int startIndex)
        {
            int c0 = value[0 + startIndex];
            int c1 = value[1 + startIndex];
            int c2 = value[2 + startIndex];
            int c3 = value[3 + startIndex];
            int d0 = c0 + (c1 << 8);
            int d1 = c2 + (c3 << 8);
            return d0 + (d1 << 16);
        }

        public static int ToInt8(byte[] value, int startIndex)
        {
            int c0 = value[0 + startIndex];
            int c1 = value[1 + startIndex];
            int c = c0 + (c1 << 8);
            return c;
        }
        public static int ToInt12(byte[] value, int startIndex)
        {
            int c0 = value[0 + startIndex];
            int c1 = value[1 + startIndex];
            int c2 = value[2 + startIndex];
            int c = c0 + (c1 << 8) + (c2 << 16);
            return c;
        }
    }
}