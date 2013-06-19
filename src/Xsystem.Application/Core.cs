using System;
using System.IO;
using System.Text;
using XSystem.Class;

namespace XSystem.Application
{
    class PPMWriter
    {
        public static void WritePPMImage(CgData cg, string path)
        {
            if (File.Exists(path)) {
                File.Delete(path);
            }

            // パレット
            Pallet256 realPallet = new Pallet256();
            int i_st = 0, i_end = 256;
            // VSP のとき
            switch (cg.type)
            {
            case CgType.VSP:
                i_st  = (cg.palletBank << 4);
                i_end = i_st + 16;
                break;
            default:
                break;
            }
            for (int i = i_st; i < i_end; ++i) {
                realPallet.red[i]   = cg.pallet.red[i];
                realPallet.green[i] = cg.pallet.green[i];
                realPallet.blue[i]  = cg.pallet.blue[i];
            }

            using (FileStream fs = File.Create(path)) {
                WriteText(fs, "P3\n");
                WriteText(fs, cg.width + " " + cg.height + "\n");
                WriteText(fs, "255\n");

                for (int j = 0; j < cg.height; ++j) {
                    for (int i = 0; i < cg.width; ++i) {
                        if (i != 0)
                            WriteText(fs, " ");
                        int pixel = cg.data[j * cg.width + i];

                        WriteText(fs, cg.pallet.red[pixel] + " ");
                        WriteText(fs, cg.pallet.green[pixel] + " ");
                        WriteText(fs, cg.pallet.blue[pixel] + "");
                    }
                    WriteText(fs, "\n");
                }
            }

            realPallet = null;
        }

        private static void WriteText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }

    class Core
    {
        static void Main(string[] args)
        {
            DriLoader loader = new DriLoader(args);
            loader.Initialize();

            CgLoader cgLoader = new CgLoader();
            for (int i = 4; i < 5; ++i) {
                DriObject? dri = loader.GetDriObject(i);
                if (dri.HasValue) {
                    CgData d = cgLoader.LoadCg(dri.Value);
                    Console.Write("no: " + i + ", size: " + dri.Value.size);
                    Console.WriteLine(", (" + d.width + ", " + d.height + ")");

                    PPMWriter.WritePPMImage(d, "./test.ppm");
                } else {
                    Console.WriteLine("ng");
                }
            }
        }
    }
}