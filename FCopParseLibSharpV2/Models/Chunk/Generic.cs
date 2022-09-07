namespace FCopParseLibSharpV2.Models.Chunk
{
  /// <summary>
  /// Generic chunk without. Used for any other unspecificed FourCC
  /// </summary>
  public class GenericChunk : Chunk
  {
    public GenericChunk(int index, int size, string type) : base(index, size, type)
    {
    }
  }
}
