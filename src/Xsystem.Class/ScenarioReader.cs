using System;
using System.IO;
using System.Collections.Generic;
// MemoryMappedFile
//using System.IO.MemoryMappedFiles;

namespace XSystem.Class
{
    struct NactInfo
    {
        public bool isQuit;
        public bool isCursorAnimation;
        public int scenarioVersion;
        public string gameTitleName;
    }
    class Nact
    {
    }
    class ScenarioReader
    {
        private DriLoader scoLoader;

        private readonly int stackSize = 1024;
        private int scenarioIndex; // sl_index
        private int scenarioPage; // sl_page
        private int stackIndex; // sco_stackindex
        private int[] stackBuffer; // sco_stackbuf
        private DriObject scenarioData; // sl_sco

        public ScenarioReader(DriLoader loader)
        {
            this.scoLoader = loader;
            this.scenarioIndex = 0;
        }

        public void Initialize()
        {
            stackBuffer = new int[stackSize];
            stackIndex = 0;
            JumpFar(0);
        }

        public bool JumpFar(int page)
        {
            DriObject? dri = scoLoader.GetDriObject(page);
            if (!dri.HasValue) {
                return false;
            }

            scenarioData = dri.Value;
            scenarioPage = page;
            scenarioIndex = LEBitConverter.From4ToInt(scenarioData.dataRaw, 4);
            return true;
        }

        public byte GetCommand()
        {
            byte[] data = scenarioData.dataRaw;
            return data[scenarioIndex++];
        }

        public int GetIndex()
        {
            return scenarioIndex;
        }
    }
}