using System;

namespace xsystem
{
    public struct VspHeader
    {
        // デフォルトの表示位置
        public int x0, y0;
        // 画像サイズ
        public int width, height;
        // 予約 (未使用)
        public int rsv;
        // パレットバンク
        public int palletBank;
        // パレットへのポインタ
        public int palletPtr;
        // ピクセルデータへのポインタ
        public int dataPtr;
    }

    public class VspFormat
    {
        public static bool CheckVspFormat(byte[] data, int offset)
        {
            int x0 = LEBitConverter.From2ToInt(data, 0 + offset);
            int y0 = LEBitConverter.From2ToInt(data, 2 + offset);
            int w  = LEBitConverter.From2ToInt(data, 4 + offset) - x0;
            int h  = LEBitConverter.From2ToInt(data, 6 + offset) - y0;

            if (x0 < 0 || x0 > 80 || y0 < 0 || y0 > 400) return false;

            // handling dalk broken cg (h = 401)
            if (w < 0 || w > 80 || h < 0 || h > 401) return false;

            return true;
        }

        private static VspHeader ExtractHeader(DriObject dri)
        {
            VspHeader header = new VspHeader();
            int offset = dri.realDataPtr;
            byte[] data = dri.dataRaw;

            header.x0 = LEBitConverter.From2ToInt(data, offset);
            header.y0 = LEBitConverter.From2ToInt(data, offset + 2);
            header.width = LEBitConverter.From2ToInt(data, offset + 4) - header.x0;
            header.height = LEBitConverter.From2ToInt(data, offset + 6) - header.y0;
            header.palletBank = (int)data[offset + 9];
            header.palletPtr = 0x0a;
            header.dataPtr = 0x3a;

            return header;
        }

        public static Pallet256 GetPallet(DriObject dri, VspHeader header)
        {
            Pallet256 pallet = new Pallet256();
            int vspPalletOffset = header.palletPtr;
            int offset = dri.realDataPtr + vspPalletOffset;
            byte[] data = dri.dataRaw;

            int red, green, blue;
            for (int i = 0; i < 16; ++i) {
                blue  = (int)data[offset + i * 3 + 0];
                red   = (int)data[offset + i * 3 + 1];
                green = (int)data[offset + i * 3 + 2];
                pallet.blue[i]  = (byte)(blue << 4);
                pallet.red[i]   = (byte)(red << 4);
                pallet.green[i] = (byte)(green << 4);
            }
            return pallet;
        }

        public static byte[] ExtractImageData(DriObject dri, VspHeader header)
        {
            // +10 Margin for broken CG
            int picSize = (header.width * 8 + 10) * (header.height + 10);
            byte[] data = new byte[picSize];

            return data;
        }

        public static CgData Extract(DriObject dri)
        {
            CgData cg = new CgData();
            VspHeader header = ExtractHeader(dri);

            // プレーン型のデータは 1 byte で 8 pixels を表現
            cg.type = CgType.VSP;
            cg.pallet = GetPallet(dri, header);
            cg.data = ExtractImageData(dri, header);
            cg.x = header.x0 * 8;
            cg.y = header.y0;
            cg.width = header.width * 8;
            cg.height = header.height;

            return cg;
        }
    }
}