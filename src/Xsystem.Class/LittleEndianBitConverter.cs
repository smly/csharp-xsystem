using System;

namespace XSystem.Class
{
    // LittleEndian 用の BitConverter
    public class LEBitConverter
    {
        // 4 bytes (16 bits) -> int
        public static int From4ToInt(byte[] value, int startIndex)
        {
            int c0 = value[0 + startIndex];
            int c1 = value[1 + startIndex];
            int c2 = value[2 + startIndex];
            int c3 = value[3 + startIndex];
            int d0 = c0 + (c1 << 8);
            int d1 = c2 + (c3 << 8);
            return d0 + (d1 << 16);
        }
        // 2 bytes (8 bits) -> int
        public static int From2ToInt(byte[] value, int startIndex)
        {
            int c0 = value[0 + startIndex];
            int c1 = value[1 + startIndex];
            int c = c0 + (c1 << 8);
            return c;
        }
        // 3 bytes (12 bits) -> int
        public static int From3ToInt(byte[] value, int startIndex)
        {
            int c0 = value[0 + startIndex];
            int c1 = value[1 + startIndex];
            int c2 = value[2 + startIndex];
            int c = c0 + (c1 << 8) + (c2 << 16);
            return c;
        }
    }
}