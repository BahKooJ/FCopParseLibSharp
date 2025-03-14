
using System.Text;

namespace FCopParser {

    public class MacIFFLevelParser {

        // Bytes of the file IFF file
        public byte[] bytes = Array.Empty<byte>();

        // This stores all the offsets or index of chunks as well as useful information regarding them with the ChunkHeader object.
        public List<ChunkHeader> offsets = new();

        public IFFFileManager parsedData;

        public List<int> BitComparison(string fourCC, int id, int start, int count, List<byte> compareData) {

            var total = new List<int>();

            var asset = parsedData.GetFile(fourCC, id);

            var bigData = asset.data.GetRange(start, count);

            var offset = 0;

            while (offset < compareData.Count) {

                if (compareData[offset] == bigData[offset]) {
                    total.Add(8);
                    offset++;
                }
                else if (compareData.GetRange(offset, 2).SequenceEqual(bigData.GetRange(offset, 2).Reverse<byte>())) {

                    total.Add(16);
                    total.Add(0);
                    offset += 2;

                }
                else if (compareData.GetRange(offset, 3).SequenceEqual(bigData.GetRange(offset, 3).Reverse<byte>())) {

                    total.Add(24);
                    total.Add(0);
                    total.Add(0);
                    offset += 3;

                }
                else if (compareData.GetRange(offset, 4).SequenceEqual(bigData.GetRange(offset, 4).Reverse<byte>())) {

                    total.Add(32);
                    total.Add(0);
                    total.Add(0);
                    total.Add(0);
                    offset += 4;

                }

            }

            return total;

        }

        public MacIFFLevelParser(byte[] bytes) {
            this.bytes = bytes;
            FindStartChunkOffset();
            parsedData = Parse();
        }

        IFFFileManager Parse() {

            var fileMananger = new IFFFileManager();

            IFFDataFile file = null;
            var dataChunksToAdd = 0;

            SubFile openedSubFile = null;

            foreach (ChunkHeader header in offsets) {

                if (header.fourCCDeclaration == IFFParser.FourCC.SWVR) {

                    if (openedSubFile != null) {

                        if (openedSubFile.files.Count != 0) {
                            fileMananger.subFiles.Add(openedSubFile);
                        }

                    }

                    openedSubFile = new SubFile(header.subFileName);

                }
                else if (header.fourCCDeclaration == IFFParser.FourCC.MSIC) {

                    if (fileMananger.music == null) {

                        fileMananger.music = new MusicFile(openedSubFile.name);

                        fileMananger.music.data.AddRange(CopyOfRange(header.index + 28, header.index + header.chunkSize).ToList());

                    }
                    else {

                        //todo: Magic number 28 is the size of the music header, there are two numbers after the header that are unknown
                        //update: I know what they are now but I'm too lazy to refactor. They are 3 numbers, chunk count, chunk iteration, and the size divided by 2

                        fileMananger.music.data.AddRange(CopyOfRange(header.index + 28, header.index + header.chunkSize).ToList());

                    }

                }

                if (header.fourCCType == IFFParser.FourCC.SHDR) {

                    if (file == null && header.fileHeader != null) {

                        List<int> rpnsReferences = new();
                        List<int> headerCodeData = new();
                        List<byte> headerCode = new();

                        var offset = 0;

                        foreach (var i in Enumerable.Range(0, IFFDataFile.rpnsRefCount)) {

                            rpnsReferences.Add(Utils.BytesToInt(header.fileHeader.actData.ToArray(), offset));
                            offset += 4;

                        }

                        foreach (var i in Enumerable.Range(0, IFFDataFile.headerCodeDataCount)) {

                            headerCodeData.Add(Utils.BytesToInt(header.fileHeader.actData.ToArray(), offset));
                            offset += 4;
                        }

                        headerCode = header.fileHeader.actData.ToList().GetRange(offset, header.fileHeader.actData.Count() - offset);

                        file = new IFFDataFile(
                            header.fileHeader.startNumber,
                            new List<byte>(),
                            header.fileHeader.fourCCData,
                            header.fileHeader.dataID,
                            rpnsReferences, headerCodeData, headerCode
                        );

                        dataChunksToAdd = DataChunksBySize(header.fileHeader.dataSize);

                    }

                }
                else if (header.fourCCType == IFFParser.FourCC.SDAT && file != null && dataChunksToAdd != 0) {

                    file.data.AddRange(CopyOfRange(header.index + IFFParser.chunkHeaderLength, header.index + header.chunkSize).ToList());
                    dataChunksToAdd--;

                    if (dataChunksToAdd == 0) {

                        if (openedSubFile != null) {

                            openedSubFile.files.Add(file);

                            file = null;

                        }
                        else {

                            fileMananger.files.Add(file);
                            file = null;

                        }

                    }

                }

            }


            return fileMananger;

        }


        void FindStartChunkOffset() {

            offsets.Clear();

            int offset = 0;
            int current24kSectionSize = 0;

            if (BytesToString(offset, 4) != IFFParser.FourCC.CTRL) {
                throw new InvalidFileException();
            }

            while (offset < bytes.Length) {

                var fourCC = BytesToString(offset, 4);
                var size = BytesToInt(offset + 4);

                if (fourCC == IFFParser.FourCC.FILL) {
                    var difference = IFFParser.iffFileSectionSize - current24kSectionSize;

                    offsets.Add(new ChunkHeader(offset, fourCC, difference));

                    offset += difference;

                    current24kSectionSize = 0;

                    continue;

                }
                else {

                    current24kSectionSize += size;

                    if (current24kSectionSize == IFFParser.iffFileSectionSize) {
                        current24kSectionSize = 0;
                    }

                }

                if (fourCC == IFFParser.FourCC.SHOC) {

                    var fourCCType = BytesToString(offset + 16, 4);

                    if (fourCCType == IFFParser.FourCC.SDAT) {

                        offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType));

                    }
                    else if (fourCCType == IFFParser.FourCC.SHDR) {

                        var offsetOfHeader = offset + IFFParser.chunkHeaderLength;

                        var startNumber = BytesToInt(offsetOfHeader);
                        var fourCCData = BytesToString(offsetOfHeader + 4, 4);
                        var dataID = BytesToInt(offsetOfHeader + 8);
                        var dataSize = BytesToInt(offsetOfHeader + 12);
                        var remainingData = CopyOfRange(offsetOfHeader + 16, offset + size);

                        var fileHeader = new FileHeader(startNumber, fourCCData, dataID, dataSize, remainingData.ToArray());

                        offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType, fileHeader));

                    }
                    //else if (fourCCType == IFFParser.FourCC.MSIC || fourCCType == IFFParser.FourCC.VAGM) {

                    //    var offsetOfHeader = offset + IFFParser.chunkHeaderLength;

                    //    var chunkCount = BitConverter.ToInt16(bytes, offsetOfHeader);
                    //    var chunkIteration = BitConverter.ToInt16(bytes, offsetOfHeader + 2);

                    //    var musicHeader = new MusicHeader(chunkCount, chunkIteration);

                    //    offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType, musicHeader));
                    //}
                    else {

                        offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType));

                    }

                }
                else if (fourCC == IFFParser.FourCC.SWVR) {

                    var fourCCType = BytesToString(offset + 16, 4);

                    //todo: Sub file chunks length has not been proven to be constitant, and if it is refactor to a constant.

                    var fileNameOffset = offset + 20;

                    offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType, CopyOfRange(fileNameOffset, fileNameOffset + 16)));


                }
                else {

                    offsets.Add(new ChunkHeader(offset, fourCC, size));

                }

                offset += size;


            }

        }

        // ---Utils---

        public static int DataChunksBySize(int size, int chunkSize = 4096, int headerLength = 20) {

            var total = size / (chunkSize - headerLength);
            if (size % (chunkSize - headerLength) != 0) {
                total++;
            }
            return total;

        }

        byte[] CopyOfRange(int start, int end) {

            var length = end - start;

            var total = new byte[length];

            Array.Copy(bytes, start, total, 0, length);

            return total;

        }

        int BytesToInt(int offset) {
            var data = CopyOfRange(offset, offset + 4).Reverse();
            return BitConverter.ToInt32(data.ToArray(), 0);
        }


        string BytesToString(int offset, int length) {
            return Encoding.Default.GetString(bytes, offset, length);
        }

    }

}