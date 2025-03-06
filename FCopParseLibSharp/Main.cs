
using System.Collections;
using System.Diagnostics;
using System.IO;
using FCopParser;

var allFiles = new List<IFFParser>() {
    new IFFParser(File.ReadAllBytes("Mp")),
    new IFFParser(File.ReadAllBytes("M2C")),
    new IFFParser(File.ReadAllBytes("ConFt")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/HK")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/JOKE")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/LAX1")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/LAX2")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M1A1")),
    new IFFParser(File.ReadAllBytes("M3A")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M3B")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M4A1")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/OV")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/OVMP")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Slim")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Un"))
};

var i = 0;


var foo = new ScriptAnalysis(allFiles);

foo.BitCompareActorsRange(new() { 5, 6, 8, 9, 20, 26, 27, 28, 36 }, 47);



return;

//foo.levels[i].fileManager.DeletesFiles("Cdcs");

foo.levels[i].fileManager.GetFile("Cdcs", 11).data = foo.levels[0].fileManager.GetFile("Cdcs", 11).data;

foo.levels[i].Compile();

var parser = new IFFParser(foo.levels[i].fileManager);

parser.Compile();

parser.parsedData.CreateFileList("debugList.txt");

File.WriteAllBytes("Mod", parser.bytes);