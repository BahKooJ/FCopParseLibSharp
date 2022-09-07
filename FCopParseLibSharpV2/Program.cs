namespace FCopParseLib
{
  class Program
  {
    static void Main(string[] args)
    {
      var levelName = "Mp"; //read this from input args

      var file = default(byte[]);
      try 
      {
        file = File.ReadAllBytes(levelName);
      }
      catch(Exception ex)
      {
        Console.Write($"Exception while trying to read level {levelName}.");
        throw;
      }

      var parser = new IFFParser(file);
    }
  }
}