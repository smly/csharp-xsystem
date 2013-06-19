using System;
using System.IO;
using System.Text;
using XSystem.Class;

namespace XSystem.Application
{
    class PPMWriter
    {
        // *_GA?.ALD から dri object をロードする loader
        private DriLoader loader;

        public PPMWriter(DriLoader loader)
        {
            this.loader = loader;
        }

        public void Write(int objectId, string outputFilePath)
        {
            CgLoader cgLoader = new CgLoader();
            DriObject? dri = loader.GetDriObject(objectId);
            if (dri.HasValue) {
                CgData d = cgLoader.LoadCg(dri.Value);
                WritePPMImage(d, outputFilePath);

                Console.Write("- no: " + objectId);
                Console.Write(", size: " + dri.Value.size);
                Console.WriteLine(", (" + d.width + ", " + d.height + ")");
                Console.WriteLine("ok ");
            } else {
                Console.WriteLine("ng");
            }
        }

        private void WritePPMImage(CgData cg, string path)
        {
            if (File.Exists(path)) {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path)) {
                WriteText(fs, "P3\n");
                WriteText(fs, cg.width + " " + cg.height + "\n");
                WriteText(fs, "255\n");

                for (int j = 0; j < cg.height; ++j) {
                    for (int i = 0; i < cg.width; ++i) {
                        int pixel = cg.data[j * cg.width + i];
                        if (i != 0)
                            WriteText(fs, " ");

                        WriteText(fs, cg.pallet.red[pixel] + " ");
                        WriteText(fs, cg.pallet.green[pixel] + " ");
                        WriteText(fs, cg.pallet.blue[pixel] + "");
                    }
                    WriteText(fs, "\n");
                }
            }
        }

        private void WriteText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }

    class Core
    {
        static void Main(string[] args)
        {
            string outputFilePath = "output.ppm";
            string[] filePaths = args;
            int objectId = 4;

            DriLoader graphicsDriLoader = new DriLoader(filePaths);
            graphicsDriLoader.Initialize();

            PPMWriter p = new PPMWriter(graphicsDriLoader);
            p.Write(objectId, outputFilePath);
        }
    }
}