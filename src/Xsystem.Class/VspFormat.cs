using System;

namespace xsystem
{
    struct VspHeader
    {
        // デフォルトの表示位置
        public int x0, y0;
        // 画像サイズ
        public int width, height;
        // 予約 (未使用)
        public int rsv;
        // パレットバンク
        public int palletBank;
    }

    public class VspFormat
    {
        public static bool CheckVspFormat(byte[] data, int offset)
        {
            int x0 = LittleEndianBitConverter.ToInt8(data, 0 + offset);
            int y0 = LittleEndianBitConverter.ToInt8(data, 2 + offset);
            int w  = LittleEndianBitConverter.ToInt8(data, 4 + offset) - x0;
            int h  = LittleEndianBitConverter.ToInt8(data, 6 + offset) - y0;

            if (x0 < 0 || x0 > 80 || y0 < 0 || y0 > 400) return false;

            // handling dalk broken cg (h = 401)
            if (w < 0 || w > 80 || h < 0 || h > 401) return false;

            return true;
        }

        private static VspHeader ExtractHeader(DriObject dri)
        {
            int offset = dri.realDataPtr;
            byte[] data = dri.dataRaw;

            VspHeader header = new VspHeader();
            header.x0 = LittleEndianBitConverter.ToInt8(data, offset);
            header.y0 = LittleEndianBitConverter.ToInt8(data, offset + 2);
            header.width = LittleEndianBitConverter.ToInt8(data, offset + 4) - header.x0;
            header.height = LittleEndianBitConverter.ToInt8(data, offset + 6) - header.y0;
            return header;
        }

        public static CgData Extract(DriObject dri)
        {
            CgData data = new CgData();
            VspHeader header = ExtractHeader(dri);

            // プレーン型のデータは 1 byte で 8 pixels
            data.x = header.x0 * 8;
            data.y = header.y0;
            data.width = header.width * 8;
            data.height = header.height;

            return data;
        }
    }
}