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
    }
}