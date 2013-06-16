using System;
using System.IO;

namespace xsystem
{
    class Core
    {
        static void Main(string[] args)
        {
            DriLoader loader = new DriLoader(args);
            loader.Initialize();
            DriObject? dri = loader.GetDriObject(0);
            if (dri != null) {
                Console.WriteLine("ok");
            } else {
                Console.WriteLine("ng");
            }
        }
    }
}