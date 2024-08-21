using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCopParser {

    public class FCopFunction {

        public static List<byte> tFUNFourCC = new List<byte>() { 78, 85, 70, 116 };
        public static List<byte> tEXTFourCC = new List<byte>() { 84, 88, 69, 116 };


        public List<CFuntFUNData> tFUNData = new();

        public IFFDataFile rawFile;

        public FCopFunction(IFFDataFile rawFile) {

            this.rawFile = rawFile;

            var chunks = FindChunks(rawFile.data.ToArray());

            var tFUNDataCount = (chunks[0].chunkSize / 4) / 5;

            // Offset starts at 12 to move past the header data
            var offset = 12;

            var data = rawFile.data.ToArray();

            foreach (var i in Enumerable.Range(0, tFUNDataCount)) {

                tFUNData.Add(
                    new CFuntFUNData(
                            Utils.BytesToInt(data, offset),
                            Utils.BytesToInt(data, offset + 4),
                            Utils.BytesToInt(data, offset + 8),
                            Utils.BytesToInt(data, offset + 12),
                            Utils.BytesToInt(data, offset + 16)

                        )
                    );

                offset += 20;

            }

            // Add 12 to move past the tEXT header
            offset += 12;

            List<byte> GetLine(int offset) {

                var itoffset = offset;
                var total = new List<byte>();

                while (true) {

                    total.Add(rawFile.data[itoffset]);

                    if (rawFile.data[itoffset] == 0) {
                        break;
                    }

                    itoffset++;
                }

                return total;

            }


            foreach (var item in tFUNData) {

                item.line1 = new FCopScript(item.line1Offset, GetLine(offset + item.line1Offset));
                item.line2 = new FCopScript(item.line2Offset, GetLine(offset + item.line2Offset));

            }

        }

        public void Compile() {

            var total = new List<byte>();

            var tFUNSize = (tFUNData.Count * 5 * 4) + 12;

            total.AddRange(tFUNFourCC);
            total.AddRange(BitConverter.GetBytes(tFUNSize));
            total.AddRange(BitConverter.GetBytes(1));

            var tEXTTotal = new List<byte>();


            foreach (var item in tFUNData) {

                total.AddRange(BitConverter.GetBytes(item.number1));
                total.AddRange(BitConverter.GetBytes(item.number2));
                total.AddRange(BitConverter.GetBytes(item.number3));

                total.AddRange(BitConverter.GetBytes(tEXTTotal.Count));
                tEXTTotal.AddRange(item.line1.compiledBytes);

                total.AddRange(BitConverter.GetBytes(tEXTTotal.Count));
                tEXTTotal.AddRange(item.line2.compiledBytes);


            }


            total.AddRange(tEXTFourCC);
            total.AddRange(BitConverter.GetBytes(tEXTTotal.Count + 12));
            total.AddRange(BitConverter.GetBytes(1));

            total.AddRange(tEXTTotal);

            rawFile.data = total;
            rawFile.modified = true;

        }

        List<ChunkHeader> FindChunks(byte[] bytes) {

            var offsets = new List<ChunkHeader>();

            int offset = 0;

            while (offset < bytes.Length) {

                var fourCC = BytesToStringReversed(bytes, offset, 4);
                var size = Utils.BytesToInt(bytes, offset + 4);

                offsets.Add(new ChunkHeader(offset, fourCC, size));

                offset += size;

            }

            return offsets;

        }

        string BytesToStringReversed(byte[] bytes, int offset, int length) {
            var s = Encoding.Default.GetString(bytes, offset, length);
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

    }

    public class CFuntFUNData {

        public int number1, number2, number3, line1Offset, line2Offset;
        public FCopScript line1;
        public FCopScript line2;

        public CFuntFUNData(int number1, int number2, int number3, int line1Offset, int line2Offset) {
            this.number1 = number1;
            this.number2 = number2;
            this.number3 = number3;
            this.line1Offset = line1Offset;
            this.line2Offset = line2Offset;
        }
    }

}