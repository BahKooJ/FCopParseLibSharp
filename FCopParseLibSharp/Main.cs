using System.Collections;
using FCopParser;


void LogActors() {

    var parser = new IFFParser(File.ReadAllBytes("Mp"));

    var actors = parser.parsedData.files.Where(file => {

        return file.dataFourCC == "Cact" || file.dataFourCC == "Csac";

    });

    foreach (var file in actors) {

        var actor = new FCopActor(file);

        Console.Write(file.dataFourCC + " " + file.dataID + " " + actor.objectType + " ");

        foreach (var r in actor.rpnsReferences) {

            Console.Write(r + " ");

        }

        Console.WriteLine();

    }

}

void LogCfun() {

    var parser = new IFFParser(File.ReadAllBytes("Mp"));


    var foo = parser.parsedData.files.First(file => {

        return file.dataFourCC == "Cfun";

    });

    var fun = new FCopFunction(foo);

}

void LogRPNS() {

    var parser = new IFFParser(File.ReadAllBytes("Mp"));


    var foo = parser.parsedData.files.First(file => {

        return file.dataFourCC == "RPNS";

    });

    var rpns = new FCopRPNS(foo);

    foreach (var code in rpns.code) {
        
        foreach (var b in code) {
            Console.Write(b + " ");
        }

        Console.WriteLine();

    }

}

LogRPNS();



