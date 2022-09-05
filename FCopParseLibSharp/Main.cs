

var parser = new IFFParser(File.ReadAllBytes("Mp"));

var foo = parser.Parse();

Console.WriteLine("bonk");
