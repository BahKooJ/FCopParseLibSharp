
using System.Collections;
using System.IO;
using FCopParser;




var allFiles = new List<IFFParser>() {
    new IFFParser(File.ReadAllBytes("Mp")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/HK")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/JOKE")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/LAX1")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/LAX2")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M1A1")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M2C")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M3A")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M4A1")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Conft")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/OV")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/OVMP")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Slim")),
    new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Un"))
};

var foo = new ScriptAnalysis(allFiles[0]);

//foo.LogCfunCode();

//return;

//foo.levels[0].rpns.code[0].compiledBytes = new List<byte> {
//    3, 2, 88, 128, 31, 0
//};

//var foo2 = new CFuntFUNData(15, 60, 0, 0, 0) {
//    line1 = new FCopScript(0, new() { 128, 153, 16, 33, 0 }),
//    line2 = new FCopScript(0, new() { 153, 21, 0 })
//};


//foo.levels[0].functions.tFUNData.Insert(0, foo2);

Console.WriteLine(foo.AnalyseCodeButBetter(
    //foo.levels[0].rpns.code.ToList()[2].Value.compiledBytes
    new List<byte>() { 
        128, 129, 31, 128, 136, 31, 128, 143, 32, 128, 152, 32, 129, 145, 29, 128, 144, 29, 128, 155, 29, 128, 156, 29, 128, 157, 29, 128, 158, 29, 128, 159, 29, 128, 160, 29, 128, 161, 29, 128, 162, 29, 129, 168, 29, 130, 169, 29, 131, 170, 29, 132, 171, 29, 0
    }
    ));

return;

//foo.levels[0].actors.First(actor => { return actor.id == 118; }).rawFile.data[37] = 10;

foo.levels[0].Compile();
foo.levels[0].iffFile.Compile();

File.WriteAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Mp", foo.levels[0].iffFile.bytes);