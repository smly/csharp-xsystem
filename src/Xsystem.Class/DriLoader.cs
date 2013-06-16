using System;
using System.IO;
using System.Collections.Generic;
// MemoryMappedFile
//using System.IO.MemoryMappedFiles;

namespace xsystem
{
    public enum DriSpec : int
    {
        DriDataMax = 65535,
        DriFileMax = 255,
    }
    public struct DriObject
    {
        public int size;
        public byte[] dataRaw;
        public int realDataPtr;
    }

    public class DriLoader
    {
        private string[] filenames;
        private int maxFileSize;
        private int[] mapDisk;
        private int[] mapPtr;
        private int[][] filePtr;

        public DriLoader(string[] filenames)
        {
            this.filenames = filenames;
            this.maxFileSize = 0;
            this.filePtr = new int[(int)DriSpec.DriFileMax][];
        }

        public Nullable<DriObject> GetDriObject(int objectNo)
        {
            int disk, ptr, dataPtr, dataPtr2;

            if (objectNo > this.maxFileSize) return null;

            disk = this.mapDisk[objectNo];
            ptr = this.mapPtr[objectNo];
            if (disk < 0 || ptr < 0) return null;
            if (filePtr[disk] == null) return null;
            dataPtr = this.filePtr[disk][ptr];
            dataPtr2 = this.filePtr[disk][ptr + 1];
            if (dataPtr == 0 || dataPtr2 == 0) return null;
            int readSize = dataPtr2 - dataPtr;

            DriObject dri;
            // if not mmapped
            dri.dataRaw = new byte[readSize];
            FileStream fileStream = new FileStream(this.filenames[disk],
                                                   FileMode.Open,
                                                   FileAccess.Read);
            fileStream.Seek(dataPtr, SeekOrigin.Begin);
            fileStream.Read(dri.dataRaw, 0, readSize);
            fileStream.Close();

            dri.realDataPtr = LittleEndianBitConverter.ToInt16(dri.dataRaw, 0);
            dri.size = LittleEndianBitConverter.ToInt16(dri.dataRaw, 0);

            return dri;
        }

        // initialize drifile object
        public void Initialize()
        {
            bool gotFileMap = false;

            for (int i = 0; i < filenames.Length; ++i) {
                FileStream fileStream = new FileStream(filenames[i],
                                                       FileMode.Open,
                                                       FileAccess.Read);
                if (!gotFileMap) {
                    GetFileMap(fileStream);
                    gotFileMap = true;
                }
                GetFilePtr(fileStream, i);

                fileStream.Close();
            }
        }

        /*
         * DriFileObject のマッピングをする
         */
        private void GetFileMap(FileStream fileStream)
        {
            // シナリオから使われる通し番号のマッピングを行う
            byte[] bytes = new byte[6];
            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(bytes, 0, 6);

            // header2 へのポインタ（単位は 2^8 bytes）
            int ptrSize = LittleEndianBitConverter.ToInt12(bytes, 0);
            // data へのポインタ - header へのポインタ＝mapdata サイズ
            int mapSize = LittleEndianBitConverter.ToInt12(bytes, 3) - ptrSize;

            // header2 へ seek して mapdata を読み込み
            byte[] mapBuffer = new byte[mapSize << 8];
            fileStream.Seek(ptrSize << 8, SeekOrigin.Begin);
            fileStream.Read(mapBuffer, 0, 256 * mapSize);

            // header2 のサイズから定義可能な最大ファイル数を逆算
            this.maxFileSize = (mapSize << 8) / 3 - 1;

            // マッピング用配列を確保
            this.mapDisk = new int[maxFileSize];
            this.mapPtr = new int[maxFileSize];

            for (int i = 0; i < this.maxFileSize; ++i) {
                int offset = i * 3 + 1;

                int fileMapDisk = mapBuffer[i * 3];
                int fileMapPtr = LittleEndianBitConverter.ToInt8(mapBuffer, offset);

                // zero-indexed values
                mapDisk[i] = fileMapDisk - 1;
                mapPtr[i] = fileMapPtr - 1;
            }

            bytes = null;
            mapBuffer = null;
        }

        private void GetFilePtr(FileStream fileStream, int diskNo)
        {
            byte[] bytes = new byte[6];
            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(bytes, 0, 6);

            // header2 へのポインタ（単位は 2^8 bytes）
            int ptrSize = LittleEndianBitConverter.ToInt12(bytes, 0);

            int fileCount = (ptrSize << 8) / 3 - 1;
            byte[] buffer = new byte[ptrSize << 8];

            // header1 (先頭から header2 まで) を読み込み
            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(buffer, 0, 256 * ptrSize);

            // ポインタの領域を確保
            this.filePtr[diskNo] = new int[fileCount];

            for (int i = 0; i < fileCount; ++i) {
                int pos = LittleEndianBitConverter.ToInt12(buffer, i * 3 +3);
                this.filePtr[diskNo][i] = pos << 8;
            }

            bytes = null;
            buffer = null;
        }
    }
}