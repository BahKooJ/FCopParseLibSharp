using System.Collections;
using FCopParser;



//var level = new FCopLevel(new IFFParser(File.ReadAllBytes("Mp")).parsedData);

//var section = level.sections[10];

//foreach (var i in Enumerable.Range(0, section.tileGraphics.Count)) {

//    var uv = section.tileGraphics[i];

//    var count = 0;

//    foreach (var column in section.tileColumns) {

//        foreach (var tile in column.tiles) {

//            if (tile.graphicsIndex == i) {
//                count++;
//            }

//        }

//    }

//    Console.WriteLine(count + " " + uv);

//}

//var foo = new ScriptAnalysis(new() { (new IFFParser(File.ReadAllBytes("Mp")), "Mp") });

//foo.CompareActorsRPNSRef();

var foo = new IFFParser(File.ReadAllBytes("OV"));

foo.parsedData.ExportAll("Ctil", "OVCtil");

