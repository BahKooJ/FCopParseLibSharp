
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

//foo.CompareResourceRefs(35);

//File.WriteAllBytes("CdcsM2C", allFiles[1].parsedData.GetFile("Cdcs", 11).data.ToArray());

//foo.CompareCdcs();

//foo.CompareActors(99);
//foo.EndianCompareActors(99, allFilesMac);

//foo.CompareActorsRange(new() { 5, 20, 26, 28 });


//foo.DataBitRangeCompareActorsRangeByLevel(new List<int>() { 96 }, 37, 0x04);
//foo.DataBitRangeCompareActorsRangeByBehaviorType(new List<int>() { 6, 8, 36 }, 72, 0x80);

//foo.DataRangeCompareActorsRangeByLevel(new List<int>() { 14 }, 46, true);
//foo.DataRangeCompareActorsRangeByBehaviorType(new List<int>() { 14 }, 46, true);

//foo.Data16RangeCompareActorsRange(new() { 98 }, 32);
//foo.Data16RangeCompareActorsRange(new() { 10 }, 60);

//foo.LogFloatingExpressions();

foo.LogScript(foo.levels[9].scripting.rpns.code[0]);

//foo.BitCompareActorsRange(new() { 32 }, 52);

//foo.PropertyCompareActors(new() { 6, 8, 36 }, 56, true, 70, true);


//foo.BitCompareActorsRange(new List<int>() { 20 }, 84);

//foo.EntityExplosionActorAnalysis();

//foo.TestForExplosionIDRef(94);



return;
