using System.Collections;
using FCopParser;

var parser = new IFFParser(File.ReadAllBytes("Mp"));


var foo = parser.parsedData.files.First(file => {

    return file.dataFourCC == "Csac" && file.dataID == 28;

});

//foo.tEXTData[27].line1 = new List<byte> { 0 };
//foo.tEXTData[29].line2.RemoveRange(2, 2);


//var indexOfDataChange = 824;
//var dataMin = 2;

//foreach (var item in foo.tFUNData) {

//    if (item.startingOffset > indexOfDataChange) {
//        item.startingOffset -= dataMin;
//    }

//    if (item.endingOffset > indexOfDataChange) {
//        item.endingOffset -= dataMin;
//    }

//}

//var item = foo.tEXTData[29];

//item.line2[0] = 228;
//item.line2[3] = 60;

//item = foo.tEXTData[28];

//foreach (var i in Enumerable.Range(0, item.line2.Count)) {
//    item.line2[i] = 0;
//}

//foo.Compile();

//new FCopFunction(foo.rawFile);

//var index = parser.parsedData.files.FindIndex(file => {

//    return file.dataFourCC == "Csac";

//});

//parser.parsedData.files.Insert(index, FCopActor.AddNetrualTurretTempMethod(500, 786170, 1200000));

//foo.data = File.ReadAllBytes("C:/Users/Zewy/Desktop/source/fcopParser/output/8/Csac28.sac").ToList();

//foo.dataID = 500;

parser.Compile();

File.WriteAllBytes("Mp MOD", parser.bytes);

Console.WriteLine("bonk");
