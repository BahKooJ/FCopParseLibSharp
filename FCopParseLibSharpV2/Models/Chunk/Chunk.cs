namespace FCopParseLibSharpV2.Models.Chunk
{
  /// <summary>
  /// Generic Chunk
  /// </summary>
  public abstract class Chunk
  {
    public int HeaderLenght { get; protected set; } = 20;

    public int Index { get; set; }
    //public string FourCC { get; set; }
    public int Size { get; set; }

    public string Type { get; set; }

    public SubChunk subChunk { get; set; }

    public Chunk(int index, int size, string type)
    {
      Index = index;
      //FourCC = fourCC;
      Size = size;
      Type = type;
    }

    //public string FourCCType { get; set; } = "";
    //public string SubFileName { get; set; } = "";
    //public FileHeader FileHeader { get; set; }
  }
}
