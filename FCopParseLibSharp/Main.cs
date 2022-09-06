

var parser = new IFFParser(File.ReadAllBytes("Mp"));

var fileManager = parser.Parse();

var level = new FCopLevel(fileManager);

Console.WriteLine("bonk");

