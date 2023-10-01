using System.Collections;
using FCopParser;



var allFiles = new List<(IFFParser file, string name)>() {
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Mp")), "Mp"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/HK")), "HK"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/JOKE")), "JOKE"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/LAX1")), "LAX1"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/LAX2")), "LAX2"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M1A1")), "M1A1"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M2C")), "M2C"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M3A")), "M3A"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/M4A1")), "M4A1"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Conft")), "Conft"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/OV")), "OV"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/OVMP")), "OVMP"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Slim")), "Slim"),
    (new IFFParser(File.ReadAllBytes("C:/Program Files (x86)/Electronic Arts/Future Cop/missions/Un")), "Un")
};

var foo = new ScriptAnalysis(allFiles[0]);

foreach (var file in foo.files[0].file.parsedData.files) {

    if (file.dataFourCC == "Cact") {
        var act = new FCopActor(file);
        if (act.objectType == 1) {
            File.WriteAllBytes("actor1." + file.dataID, file.data.ToArray());
        }

    }

}

//foo.AnalyseRPNSCode();

return;


foo.files[0].file.parsedData.ImportFile("Cact", 106, "actor95106");

foo.files[0].file.Compile();

File.WriteAllBytes("Mod", foo.files[0].file.bytes);

return;

//var defaultRPNSRef = new List<byte>() { 0x0F, 0x07, 0x00, 0x00, 0x0F, 0x07, 0x00, 0x00, 0x0F, 0x07, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x43, 0x4F };

var code = new byte[] {
    153, 16, // Get Red Player Points Var?
    128, //Player Point Count Literal (0)
    35, // Is Greater Than
    156, 16, // Get Red Copper Count?
    148, //Chopper Count Literal
    37, // Is Less Than
    44, 20, 14, 
    162, 199, 128, 60, // Spawn Object
    191, 131, 30, // Play claim sound
    153, 25, // Remove player point
    156, 21, // Add Chopper count
    8, 4, // Else?
    201, 131, 30, // Play unable sound
    0 };

foo.files[0].rpns.bytes.RemoveRange(1595, 4);
foo.files[0].rpns.bytes.InsertRange(1595, code);

foo.files[0].rpns.Compile();

foreach (var act in foo.files[0].actors) {

    foreach (var i in Enumerable.Range(0, act.rpnsReferences.Count)) {

        if (act.rpnsReferences[i] > 1595) {
            act.rpnsReferences[i] += code.Count() - 4;
        }

    }

    act.Compile();

}

foo.files[0].file.Compile();

File.WriteAllBytes("Mod", foo.files[0].file.bytes);


