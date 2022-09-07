using FCopParseLibSharpV2.Extentions;

namespace FCopParseLibSharpV2.Models.Chunk
{
  public class SHDR : SubChunk
  {
    public int StartNumber { get; set; }
    public string FourCCData { get; set; }
    public int DataID { get; set; }
    public int DataSize { get; set; }
    public byte[] ActData { get; set; }

    public SHDR(byte[] bytes, int offset, int size)
    {
      var end = offset + size;

      StartNumber = BitConverter.ToInt32(bytes, offset);
      offset += 4; //read as int = 4bytes

      FourCCData = bytes.ToStringReverse(offset, 4);
      offset += 4; //read as 4 byte strings

      DataID = BitConverter.ToInt32(bytes, offset);
      offset += 4; //read as int = 4bytes

      DataSize = BitConverter.ToInt32(bytes, offset);
      offset += 4; //read as int = 4bytes

      ActData = bytes.Skip(offset).Take(end - offset).ToArray();
    }
  }
}
