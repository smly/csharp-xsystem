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

            int no = 20;
            DriObject? dri = loader.GetDriObject(no);
            if (dri.HasValue) {
                CgData d = cgLoader.LoadCg(dri.Value);

                Console.WriteLine("P3");
                Console.WriteLine(d.width + " " + d.height);
                Console.WriteLine("255");

                for (int i = 0; i < d.height; ++i) {
                    for (int j = 0; j < d.width; ++j) {
                        if (j != 0)
                            Console.Write(" ");
                        int x = d.data[j + i * d.height] * 10;
                        Console.Write(x + " " + x + " " + x);
                    }
                    Console.WriteLine("");
                }
            } else {
                Console.WriteLine("ng");
            }
        }
    }
}