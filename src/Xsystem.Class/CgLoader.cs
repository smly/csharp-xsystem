using System;

namespace xsystem
{
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