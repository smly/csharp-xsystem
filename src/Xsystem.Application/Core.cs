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
                    CgData d = cg.LoadCg(dri.Value);

                    Console.WriteLine(t);
                    Console.WriteLine("x: " + d.x);
                    Console.WriteLine("y: " + d.y);
                    Console.WriteLine("width: " + d.width);
                    Console.WriteLine("height: " + d.height);
                } else {
                    Console.WriteLine("ng");
                }
            }
        }
    }
}