using FCopParseLibSharpV2.Extentions;
using FCopParseLibSharpV2.Models.Chunk;
using System.Text;

namespace FCopParseLibSharpV2.Service
{
  public static class ChunkService
  {
    public static (int size, string type) GetChunkHeader(byte[] bytes, int offset)
    {
      var size = BitConverter.ToInt32(bytes, offset + 4);
      var type = bytes.ToStringReverse(offset + 16, 4);

      return (size, type);
    }

    public static Chunk GetSHOC(byte[] bytes, int offset)
    {
      (var size, var type) = GetChunkHeader(bytes, offset);

      var result = new SHOC(offset, size, type);

      switch(type)
      {
        case "SHDR": result.subChunk = new SHDR(bytes, offset, size); break;
      }

      return result;
    }

    public static Chunk GetSWVR(byte[] bytes, int offset)
    {
      (var size, var type) = GetChunkHeader(bytes, offset);

      var nameOffset = offset + 20;
      var nameBytes = bytes[nameOffset..(nameOffset + 16)];
      var fileName = Encoding.Default.GetString(nameBytes.TakeWhile(x => x != 0).ToArray());

      var result = new SWVR(offset, size, type, fileName);
      return result;
    }

    internal static Chunk GetGeneric(byte[] bytes, int offset)
    {
      (var size, var type) = GetChunkHeader(bytes, offset);

      var result = new GenericChunk(offset, size, type);

      return result;
    }
  }
}
