using System;

namespace xsystem
{
    public class VspFormat
    {
        public static bool CheckVspFormat(byte[] data, int offset)
        {
            int x0 = LittleEndianBitConverter.ToInt8(data, 0 + offset);
            int y0 = LittleEndianBitConverter.ToInt8(data, 2 + offset);
            int w  = LittleEndianBitConverter.ToInt8(data, 4 + offset) - x0;
            int h  = LittleEndianBitConverter.ToInt8(data, 6 + offset) - y0;

            if (x0 < 0 || x0 > 80 || y0 < 0 || y0 > 400) return false;

            // dalk broken cg
            if (w < 0 || w > 80 || h < 0 || h > 401) return false;

            return true;
        }
    }
}