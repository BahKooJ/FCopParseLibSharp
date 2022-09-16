using FCopParseLibSharpV2.Models;

namespace FCopParseLib
{
  class Program
  {
    static void Main(string[] args)
    {
      var levelName = "Mp"; //read this from input args

      byte[] file;
      try 
      {
        file = File.ReadAllBytes(levelName);
      }
      catch(Exception)
      {
        Console.Write($"Exception while trying to read level {levelName}.");
        throw;
      }

      var parser = new IFFParser(file);
      var fileManager = parser.Parse(file);
      var level = new FCopLevel(fileManager);
      Console.WriteLine("bonk");
    }
  }
}