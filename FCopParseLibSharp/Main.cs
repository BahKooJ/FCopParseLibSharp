using System.Collections;
using FCopParser;

var parser = new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Un"));


var net = new FCopNavMesh(parser.parsedData.files.First(file => {

    return file.dataFourCC == "Cnet";

}));

parser.Compile();

Console.WriteLine("bonk");
