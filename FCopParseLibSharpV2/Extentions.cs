using System.Text;

namespace FCopParseLibSharpV2.Extentions
{
  public static class ByteExtentions
  {
    public static string ToStringReverse(this byte[] bytes, int offset, int lenght)
    {
      var result = new string(Encoding.Default.GetString(bytes, offset, lenght).Reverse().ToArray());
      return result;
    }

    public static byte[] CopyOfRange(this byte[] bytes, int start, int end)
    {
      var length = end - start;
      var total = new byte[length];
      Array.Copy(bytes, start, total, 0, length);

      return total;
    }
  }
}
