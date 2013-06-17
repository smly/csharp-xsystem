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
            int vspDataOffset = header.dataPtr;
            int offset = dri.realDataPtr + vspDataOffset;
            // +10 Margin for broken CG
            int picSize = (header.width * 8 + 10) * (header.height + 10);

            int w = header.width;
            int h = header.height;

            // raw の +offset から VSP 画像データを展開し data へコピー
            byte[] raw = dri.dataRaw;
            byte[] data = new byte[picSize];

            // extraction buffer (現在のプレーンと前のプレーン)
            byte[][] bc = new byte[4][];
            byte[][] bp = new byte[4][];
            for (int j = 0; j < 4; ++j) bc[j] = new byte[480];
            for (int j = 0; j < 4; ++j) bp[j] = new byte[480];

            // 圧縮コード, mask, copy length
            int c0, l;
            byte mask = 0x00;

            for (int x = 0; x < w; ++x) {
                // バッファに読み込む
                for (int pl = 0; pl < 4; ++pl) {
                    int y = 0;
                    while (y < h) {
                        c0 = raw[offset];
                        offset++;

                        if (c0 >= 0x08) {
                            bc[pl][y] = (byte)c0;
                            ++y;
                        } else if (c0 == 0x00) {
                            l = (int)raw[offset] + 1;
                            offset++;
                            for (int i = 0; i < l; ++i)
                                bc[pl][y + i] = bp[pl][y + i];
                            y += l;
                        } else if (c0 == 0x01) {
                            l = (int)raw[offset] + 1;
                            offset++;
                            byte b0 = raw[offset];
                            offset++;
                            for (int i = 0; i < l; ++i)
                                bc[pl][y + i] = b0;
                            y += l;
                        } else if (c0 == 0x02) {
                            l = (int)raw[offset] + 1;
                            offset++;
                            byte b0 = raw[offset];
                            offset++;
                            byte b1 = raw[offset];
                            offset++;
                            for (int i = 0; i < l; ++i) {
                                bc[pl][y] = b0; ++y;
                                bc[pl][y] = b1; ++y;
                            }
                        } else if (c0 == 0x03) {
                            l = (int)raw[offset] + 1;
                            offset++;
                            for (int i = 0; i < l; ++i) {
                                bc[pl][y] = (byte)(bc[0][y] ^ mask);
                                ++y;
                            }
                            mask = 0x00;
                        } else if (c0 == 0x04) {
                            l = (int)raw[offset] + 1;
                            offset++;
                            for (int i = 0; i < l; ++i) {
                                bc[pl][y] = (byte)(bc[1][y] ^ mask);
                                ++y;
                            }
                            mask = 0x00;
                        } else if (c0 == 0x05) {
                            l = (int)raw[offset] + 1;
                            offset++;
                            for (int i = 0; i < l; ++i) {
                                bc[pl][y] = (byte)(bc[2][y] ^ mask);
                                ++y;
                            }
                            mask = 0x00;
                        } else if (c0 == 0x06) {
                            mask = 0xff;
                        } else if (c0 == 0x07) {
                            bc[pl][y] = raw[offset];
                            offset += 1;
                            ++y;
                        }
                    }
                }

                // place から packed 展開
                for (int y = 0; y < h; ++y) {
                    int loc = (y * w + x) * 8;
                    byte b0 = bc[0][y];
                    byte b1 = bc[1][y];
                    byte b2 = bc[2][y];
                    byte b3 = bc[3][y];

                    data[loc + 0] = (byte)(((b0>>7)&0x01) |
                                           ((b1>>6)&0x02) |
                                           ((b2>>5)&0x04) |
                                           ((b3>>4)&0x08));
                    data[loc + 1] = (byte)(((b0>>6)&0x01) |
                                           ((b1>>5)&0x02) |
                                           ((b2>>4)&0x04) |
                                           ((b3>>3)&0x08));
                    data[loc + 2] = (byte)(((b0>>5)&0x01) |
                                           ((b1>>4)&0x02) |
                                           ((b2>>3)&0x04) |
                                           ((b3>>2)&0x08));
                    data[loc + 3] = (byte)(((b0>>4)&0x01) |
                                           ((b1>>3)&0x02) |
                                           ((b2>>2)&0x04) |
                                           ((b3>>1)&0x08));
                    data[loc + 4] = (byte)(((b0>>3)&0x01) |
                                           ((b1>>2)&0x02) |
                                           ((b2>>1)&0x04) |
                                           ((b3   )&0x08));
                    data[loc + 5] = (byte)(((b0>>2)&0x01) |
                                           ((b1>>1)&0x02) |
                                           ((b2   )&0x04) |
                                           ((b3<<1)&0x08));
                    data[loc + 6] = (byte)(((b0>>1)&0x01) |
                                           ((b1   )&0x02) |
                                           ((b2<<1)&0x04) |
                                           ((b3<<2)&0x08));
                    data[loc + 7] = (byte)(((b0   )&0x01) |
                                           ((b1<<1)&0x02) |
                                           ((b2<<2)&0x04) |
                                           ((b3<<3)&0x08));
                }
                // buffer の入れ替え用
                byte[] bt = new byte[480];
                bt = bp[0]; bp[0] = bc[0]; bc[0] = bt;
                bt = bp[1]; bp[1] = bc[1]; bc[1] = bt;
                bt = bp[2]; bp[2] = bc[2]; bc[2] = bt;
                bt = bp[3]; bp[3] = bc[3]; bc[3] = bt;
            }

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