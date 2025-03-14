
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

var allFilesMac = new List<MacIFFLevelParser>() {
    new MacIFFLevelParser(File.ReadAllBytes("Mac/Mp")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/M2C")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/ConFt")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/HK")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/JOKE")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/LAX1")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/LAX2")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/M1A1")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/M3A")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/M3B")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/M4A1")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/OV")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/OVMP")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/Slim")),
    new MacIFFLevelParser(File.ReadAllBytes("Mac/Un"))
};

var foo = new ScriptAnalysis(allFiles);

foo.EndianCompareActors(20, allFilesMac);

//foo.CompareActorsRange(new() { 1, 5, 6, 8, 9, 10, 11, 12, 16, 20, 25, 26, 27, 28, 30, 31, 32, 33, 36, 37, 38 });

//foo.CompareActors(20);

//foo.BitCompareActorsRange(new List<int>() { 16 }, 62);

//foo.Data16RangeCompareActorsRange(new() { 5 }, 68);



return;
