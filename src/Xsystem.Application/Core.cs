using System;

namespace xsystem
{
    class Core
    {
        static void Main(string[] args)
        {
            DriLoader loader = new DriLoader(args);
            loader.Initialize();

            CgLoader cgLoader = new CgLoader();

            for (int i = 0; i < 10; ++i) {
                DriObject? dri = loader.GetDriObject(i);
                if (dri.HasValue) {
                    CgData d = cgLoader.LoadCg(dri.Value);

                    Console.Write("no: " + i + ", size: " + dri.Value.size);
                    Console.Write(", type: " + d.type);
                    Console.WriteLine(", width: " + d.width + ", height: " + d.height);
                } else {
                    Console.WriteLine("ng");
                }
            }
        }
    }
}