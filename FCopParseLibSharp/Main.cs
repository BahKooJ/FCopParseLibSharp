using System.Collections;
using FCopParser;

var parser = new IFFParser(File.ReadAllBytes("Mp"));

var level = new FCopLevel(parser.parsedData);


parser.Compile();

File.WriteAllBytes("Mp MOD", parser.bytes);

Console.WriteLine("bonk");
