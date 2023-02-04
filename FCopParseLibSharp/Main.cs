using System.Collections;
using FCopParser;

var parser = new IFFParser(File.ReadAllBytes("Mp"));


var foo = new FCopFunction(parser.parsedData.files.First(file => {

    return file.dataFourCC == "Cfun";

}));

//foo.tEXTData[27].line1 = new List<byte> { 0 };
//foo.tEXTData[27].line2.RemoveRange(60, 4);


//var indexOfDataChange = 692;
//var dataMin = 4;

//foreach (var item in foo.tFUNData) {

//    if (item.startingOffset > indexOfDataChange) {
//        item.startingOffset -= dataMin;
//    }

//    if (item.endingOffset > indexOfDataChange) {
//        item.endingOffset -= dataMin;
//    }

//}

var item = foo.tEXTData[5];

foreach (var i in Enumerable.Range(0, item.line1.Count)) {
    item.line1[i] = 0;
}

foo.Compile();

new FCopFunction(foo.rawFile);

parser.Compile();

File.WriteAllBytes("Mp MOD", parser.bytes);

Console.WriteLine("bonk");
