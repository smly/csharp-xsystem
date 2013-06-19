using System;

namespace xsystem
{
    // 256色パレット
    public class Pallet256
    {
        public byte[] red;
        public byte[] green;
        public byte[] blue;

        public Pallet256()
        {
            this.red = new byte[256];
            this.green = new byte[256];
            this.blue = new byte[256];
        }
    }
    public enum CgType : int
    {
        UNKNOWN = 1,
        VSP,
        PMS8,
        PMS16,
        BMP8,
        BMP24,
        QNT,
    }
    public struct CgData
    {
        public CgType type;
        public int x, y;
        public int width, height;
        public Pallet256 pallet;
        public byte[] data;
        public int palletBank;
    }

    public class CgLoader
    {
        public CgType CheckCgFormat(DriObject dri)
        {
            if (VspFormat.CheckVspFormat(dri.dataRaw, dri.realDataPtr)) {
                return CgType.VSP;
            } else {
                return CgType.UNKNOWN;
            }
        }

        public CgData LoadCg(DriObject dri)
        {
            CgType type = this.CheckCgFormat(dri);

            CgData data;
            switch(type)
            {
              case CgType.VSP:
                  data = VspFormat.Extract(dri);
                  break;
              default:
                  // dummy
                  data = new CgData();
                  data.type = type;
                  data.x = 10;
                  data.y = 10;
                  break;
            }

            return data;
        }
    }
}