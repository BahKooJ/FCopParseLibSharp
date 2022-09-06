
// This object is what indexes the IFF files determines where chunks are located.
// It stores chunk information with the ChunkHeader object.
class IFFParser
{
    // consants for important fourCCs.
    // Other fourCCs can be ignored as they're just a description of what the item is.
    // However it is import to test for these specifically to know how to parse the chunk.
    static class FourCC {
        public const string CTRL = "CTRL";
        public const string SHOC = "SHOC";
        public const string FILL = "FILL";
        public const string SHDR = "SHDR";
        public const string SDAT = "SDAT";
        public const string SWVR = "SWVR";
        public const string MSIC = "MSIC";
    }

    // The normal length of a chunk header.
    const int chunkHeaderLength = 20;

    // Bytes of the file IFF file
    byte[] bytes;

    // This stores all the offsets or index of chunks as well as useful information regarding them with the ChunkHeader object.
    public List<ChunkHeader> offsets = new List<ChunkHeader>();

    public IFFParser(byte[] bytes) {
        this.bytes = bytes;
        FindStartChunkOffset();
    }

    // Grabs all the files/data and coverts them into their own files,
    // separating the data and chuncks allowing for other programs to parse the data freely.
    // Returns the IFFFileManage object store the individual files.
    public IFFFileManager Parse() {

        var fileMananger = new IFFFileManager();

        IFFDataFile? file = null;
        var dataChunksToAdd = 0;

        string? subFileName = null;

        foreach (ChunkHeader header in offsets) {

            if (header.fourCCDeclaration == FourCC.SWVR) {

                subFileName = header.subFileName;

            } else if (header.fourCCDeclaration == FourCC.MSIC) {

                if (fileMananger.music == null) {

                    fileMananger.music = new KeyValuePair<string, List<byte>>(subFileName!, new List<byte>());

                } else {

                    //todo: Magic number 28 is the size of the music header, there are two numbers after the header that are unknown

                    fileMananger.music.Value.Value.AddRange(CopyOfRange(header.index + 28, header.index + header.chunkSize).ToList());

                }

            }

            if (header.fourCCType == FourCC.SHDR) {

                if (file == null && header.fileHeader != null) {

                    file = new IFFDataFile(new List<byte>(), header.fileHeader.fourCCData, header.fileHeader.dataID, header.fileHeader.actData.ToList());
                    dataChunksToAdd = DataChunksBySize(header.fileHeader.dataSize);

                }

            } else if (header.fourCCType == FourCC.SDAT && file != null && dataChunksToAdd != 0) {

                file.data.AddRange(CopyOfRange(header.index + chunkHeaderLength, header.index + header.chunkSize).ToList());
                dataChunksToAdd--;

                if (dataChunksToAdd == 0) {

                    if (subFileName != null) {

                        if (!fileMananger.subFiles.ContainsKey(subFileName)) {
                            fileMananger.subFiles[subFileName] = new List<IFFDataFile> { file };
                        } else {
                            fileMananger.subFiles[subFileName].Add(file);
                        }

                        file = null;

                    } else {

                        fileMananger.files.Add(file);
                        file = null;

                    }

                }

            }

        }


        return fileMananger;

    }


    // ---Indexing---

    void FindStartChunkOffset() {

        int offset = 0;

        while (offset < bytes.Length) {

            var fourCC = BytesToStringReversed(offset, 4);
            var size = BytesToInt(offset + 4);

            if (fourCC == FourCC.SHOC) {

                var fourCCType = BytesToStringReversed(offset + 16, 4);

                if (fourCCType == FourCC.SDAT) {

                    offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType));

                } else if (fourCCType == FourCC.SHDR) {

                    var offsetOfHeader = offset + chunkHeaderLength;

                    var startNumber = BytesToInt(offsetOfHeader);
                    var fourCCData = BytesToStringReversed(offsetOfHeader + 4, 4);
                    var dataID = BytesToInt(offsetOfHeader + 8);
                    var dataSize = BytesToInt(offsetOfHeader + 12);
                    var remainingData = CopyOfRange(offsetOfHeader + 16, offset + size);

                    offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType,
                        new FileHeader(startNumber, fourCCData, dataID, dataSize, remainingData.ToArray()
                        )));

                } else {

                    offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType));

                }

            } else if (fourCC == FourCC.SWVR) {

                var fourCCType = BytesToStringReversed(offset + 16, 4);

                //todo: Sub file chunks length has not been proven to be constitant, and if it is refactor to a constant.

                var fileNameOffset = offset + 20;

                var fileName = "";

                foreach (int i in Enumerable.Range(0, 16)) {
                    var byteChar = bytes[fileNameOffset + i];

                    if (byteChar != 0) {
                        fileName += BytesToString(fileNameOffset + i, 1);
                    } else {
                        break;
                    }

                }

                offsets.Add(new ChunkHeader(offset, fourCC, size, fourCCType, null, fileName));


            } else {

                offsets.Add(new ChunkHeader(offset, fourCC, size));

            }

            offset += size;

        }
        
    }

    // ---Utils---

    public static string Reverse(string s) {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    int DataChunksBySize(int size, int chunkSize = 4096) {

        var total = size / (chunkSize - 20);
        if (size % (chunkSize - 20) != 0) {
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
        return BitConverter.ToInt32(bytes, offset);
    }

    string BytesToString(int offset, int length) {
        return System.Text.Encoding.Default.GetString(bytes, offset, length);
    }

    string BytesToStringReversed(int offset, int length)
    {
        Span<byte> data = bytes;
        data.Slice(offset, length).Reverse();
        return data.ToString();
    }
}