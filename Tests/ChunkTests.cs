using FCopParseLibSharpV2.Service;

namespace ChunkTests
{
  public class ChunkTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SHOC()
    {
      var bytes = new byte[0];
      var offset = 0;

      var chunk = ChunkService.GetChunk(bytes, offset);
    }
  }
}