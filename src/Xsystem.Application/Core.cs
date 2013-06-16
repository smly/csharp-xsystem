using System;

namespace xsystem
{
    class Core
    {
        static void Main(string[] args)
        {
            DriLoader loader = new DriLoader(args);
            loader.Initialize();

            CgLoader cg = new CgLoader();

            for (int i = 0; i < 10; ++i) {
                DriObject? dri = loader.GetDriObject(i);
                if (dri.HasValue) {
                    Console.WriteLine("no: " + i + ", size: " + dri.Value.size);
                    CgType t = cg.CheckCgFormat(dri.Value);
                    Console.WriteLine(t);
                } else {
                    Console.WriteLine("ng");
                }
            }
        }
    }
}