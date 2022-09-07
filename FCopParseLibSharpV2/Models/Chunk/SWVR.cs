using System.Text;

namespace FCopParseLibSharpV2.Models.Chunk
{
  public class SWVR : Chunk
  {
    public string FileName { get; set; }

    public SWVR(int index, int size, string type, string fileName) : base(index, size, type)
    {
      this.FileName = fileName;
    }
  }
}
