
// This object is what indexes the IFF files determines where chunks are located.
// It stores chunk information with the ChunkHeader object.
using FCopParseLibSharpV2.Extentions;
using FCopParseLibSharpV2.Models.Chunk;
using FCopParseLibSharpV2.Service;

class IFFParser
{
  // consants for important fourCCs.
  // Other fourCCs can be ignored as they're just a description of what the item is.
  // However it is import to test for these specifically to know how to parse the chunk.
  static class FourCC
  {
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

  // This stores all the offsets or index of chunks as well as useful information regarding them with the ChunkHeader object.
  public List<Chunk> chunks = new List<Chunk>();

  public IFFParser(byte[] bytes)
  {
    Init(bytes);
  }

  // Grabs all the files/data and coverts them into their own files,
  // separating the data and chuncks allowing for other programs to parse the data freely.
  // Returns the IFFFileManage object store the individual files.
  public IFFFileManager Parse()
  {
    var fileMananger = new IFFFileManager();

    IFFDataFile file = null;
    int dataChunksToAdd = 0;
    string subFileName;

    foreach (var chunk in chunks)
    {
      switch (chunk)
      {
        case SWVR: subFileName = ((SWVR)chunk).FileName; break;

        case MSIC:
          if (fileMananger.music == null)
          {
            fileMananger.music = new KeyValuePair<string, List<byte>>(subFileName!, new List<byte>());
          }
          else
          {
            //todo: Magic number 28 is the size of the music header, there are two numbers after the header that are unknown
            fileMananger.music.Value.Value.AddRange(CopyOfRange(chunk.Index + 28, chunk.Index + chunk.Size).ToList());
          }
          break;
      }

      switch (chunk.Type)
      {
        case FourCC.SHDR:
          if (file == null && chunk.subChunk != null)
          {
            var shdrFile = (SHDR)chunk.subChunk;

            file = new IFFDataFile(new List<byte>(), shdrFile.FourCCData, shdrFile.DataID, shdrFile.ActData.ToList());
            dataChunksToAdd = DataChunksBySize(shdrFile.DataSize);

          }
          break;

        case FourCC.SDAT:
          if (file != null && dataChunksToAdd != 0)
          {
            file.data.AddRange(CopyOfRange(chunk.Index + chunkHeaderLength, chunk.Index + chunk.Size).ToList());
            dataChunksToAdd--;

            if (dataChunksToAdd == 0)
            {

              if (subFileName != null)
              {

                if (!fileMananger.subFiles.ContainsKey(subFileName))
                {
                  fileMananger.subFiles[subFileName] = new List<IFFDataFile> { file };
                }
                else
                {
                  fileMananger.subFiles[subFileName].Add(file);
                }

                file = null;

              }
              else
              {

                fileMananger.files.Add(file);
                file = null;

              }
            }
          }
          break;
      }
    }

    return fileMananger;
  }

  void Init(byte[] bytes)
  {
    int offset = 0;
    while (offset < bytes.Length)
    {
      var fourCC = bytes.ToStringReverse(offset, 4);

      Chunk chunk;
      switch (fourCC)
      {
        case FourCC.SHOC: chunk = ChunkService.GetSHOC(bytes, offset); break;
        case FourCC.SWVR: chunk = ChunkService.GetSWVR(bytes, offset); break;
        default: chunk = ChunkService.GetGeneric(bytes, offset); break;
      }

      chunks.Add(chunk);
      offset += chunk.Size;
    }
  }

  // ---Utils---

  public static string Reverse(string s)
  {
    char[] charArray = s.ToCharArray();
    Array.Reverse(charArray);
    return new string(charArray);
  }

  int DataChunksBySize(int size, int chunkSize = 4096)
  {

    var total = size / (chunkSize - 20);
    if (size % (chunkSize - 20) != 0)
    {
      total++;
    }
    return total;

  }

  byte[] CopyOfRange(int start, int end)
  {

    var length = end - start;

    var total = new byte[length];

    Array.Copy(bytes, start, total, 0, length);

    return total;

  }
}