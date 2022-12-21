using System.Collections;
using FCopParser;

var parser = new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Un"));

parser.Compile();

Console.WriteLine("bonk");
