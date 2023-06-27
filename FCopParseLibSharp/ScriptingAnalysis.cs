
using FCopParser;

//void LogActors() {

//    var parser = new IFFParser(File.ReadAllBytes("Mp"));

//    var actors = parser.parsedData.files.Where(file => {

//        return file.dataFourCC == "Cact" || file.dataFourCC == "Csac";

//    });

//    foreach (var file in actors) {

//        var actor = new FCopActor(file);

//        Console.Write(file.dataFourCC + " " + file.dataID + " " + actor.objectType + " ");

//        foreach (var r in actor.rpnsReferences) {

//            Console.Write(r + " ");

//        }

//        Console.WriteLine();

//    }

//}

//void LogCfun() {

//    var parser = new IFFParser(File.ReadAllBytes("Mp"));


//    var foo = parser.parsedData.files.First(file => {

//        return file.dataFourCC == "Cfun";

//    });

//    var fun = new FCopFunction(foo);

//}

//void LogRPNS() {

//    var parser = new IFFParser(File.ReadAllBytes("Mp"));


//    var foo = parser.parsedData.files.First(file => {

//        return file.dataFourCC == "RPNS";

//    });

//    var rpns = new FCopRPNS(foo);

//    foreach (var code in rpns.code) {

//        foreach (var b in code) {
//            Console.Write(b + " ");
//        }

//        Console.WriteLine();

//    }

//}



//void CompareData() {

//    var parser = new IFFParser(File.ReadAllBytes("Mp"));

//    var actors = parser.parsedData.files.Where(file => {

//        return file.dataFourCC == "Cact" || file.dataFourCC == "Csac";

//    });

//    var rawRPNS = parser.parsedData.files.First(file => {

//        return file.dataFourCC == "RPNS";

//    });

//    var rpns = new FCopRPNS(rawRPNS);

//    var rawCFun = parser.parsedData.files.First(file => {

//        return file.dataFourCC == "Cfun";

//    });

//    var fun = new FCopFunction(rawCFun);

//    foreach (var file in actors) {

//        var actor = new FCopActor(file);

//        Console.Write(file.dataFourCC + " ID: " + file.dataID + " Type: " + actor.objectType + "\n");


//        foreach (var r in actor.rpnsReferences) {

//            Console.Write("RPNS Ref: " + r + "\nByte Code: ");

//            foreach (var i in Enumerable.Range(r, rpns.bytes.Count)) {

//                Console.Write(rpns.bytes[i] + " ");

//                if (rpns.bytes[i] == 0) {
//                    Console.WriteLine();
//                    break;
//                }

//            }

//        }

//        Console.Write("Header Code Data: ");

//        foreach (var hcd in actor.headerCodeData) {

//            Console.Write(hcd + " ");

//        }

//        Console.WriteLine();

//        Console.Write("Header Code: ");

//        foreach (var hc in actor.headerCode) {

//            Console.Write(hc + " ");

//        }

//        Console.WriteLine();
//        Console.WriteLine();

//    }

//}

//var foo = new ScriptAnalysis(new() { (new IFFParser(File.ReadAllBytes("Mp")), "Mp") });

//foo.CompareActorsRPNSRef();



class ScriptAnalysis {

    public List<IffFilesWithScripts> files = new();

    public ScriptAnalysis(List<(IFFParser file, string name)> files) {

        foreach (var file in files) {
            this.files.Add(new IffFilesWithScripts(file.file, file.name));
        }

    }

    public struct RPNSRef {
        public int ref1;
        public int ref2;
        public int ref3;

        public RPNSRef(int ref1, int ref2, int ref3) {
            this.ref1 = ref1;
            this.ref2 = ref2;
            this.ref3 = ref3;
        }

    }

    public class ActorsByRPNSRef {

        public RPNSRef rpnsRefs;
        public List<FCopActor> actors;
        public int sharedSameType;

        public IffFilesWithScripts fileOrigin;

        public ActorsByRPNSRef(RPNSRef rpnsRefs, List<FCopActor> actors, IffFilesWithScripts fileOrigin) {

            this.rpnsRefs = rpnsRefs;
            this.actors = actors;
            this.fileOrigin = fileOrigin;

            var type = -1;
            var shareSameType = true;
            foreach (var actor in actors) {

                if (type == -1) {
                    type = actor.objectType;
                }
                else if (type != actor.objectType) {
                    shareSameType = false;
                }

            }

            if (shareSameType) {
                sharedSameType = type;
            }
            else {
                sharedSameType = -1;
            }

            this.fileOrigin = fileOrigin;
        }

    }

    public void CompareActorsRPNSRef() {

        string LogActorRPNSGroup(ActorsByRPNSRef actRef) {

            var message = "";

            message += "RPNS Ref " + actRef.rpnsRefs.ref1 + ": ";

            foreach (var i in Enumerable.Range(actRef.rpnsRefs.ref1, actRef.fileOrigin.rpns.bytes.Count)) {

                message += actRef.fileOrigin.rpns.bytes[i] + " ";

                if (actRef.fileOrigin.rpns.bytes[i] == 0) {
                    message += "\n";
                    break;
                }

            }

            message += "RPNS Ref " + actRef.rpnsRefs.ref2 + ": ";

            foreach (var i in Enumerable.Range(actRef.rpnsRefs.ref2, actRef.fileOrigin.rpns.bytes.Count)) {

                message += actRef.fileOrigin.rpns.bytes[i] + " ";

                if (actRef.fileOrigin.rpns.bytes[i] == 0) {
                    message += "\n";
                    break;
                }

            }

            message += "RPNS Ref " + actRef.rpnsRefs.ref3 + ": ";

            foreach (var i in Enumerable.Range(actRef.rpnsRefs.ref3, actRef.fileOrigin.rpns.bytes.Count)) {

                message += actRef.fileOrigin.rpns.bytes[i] + " ";

                if (actRef.fileOrigin.rpns.bytes[i] == 0) {
                    message += "\n";
                    break;
                }

            }

            message += "Actors With RPNS Refs: \n";

            foreach (var actor in actRef.actors) {

                message += "(Type: " + actor.objectType + ", ID: " + actor.id + ") ";

            }

            message += "\nActors Shared Type: ";

            if (actRef.sharedSameType != -1) {
                message += actRef.sharedSameType;
            }
            else {
                message += "Assorted";
            }

            message += "\n";

            return message;

        }

        var message = "";

        foreach (var file in files) {

            message += file.name + ": \n\n";

            var actRefs = CreateActorsRPNSRefFromFile(file);

            foreach (var groupedActRef in actRefs) {

                foreach (var actRef in groupedActRef.Value) {
                    message += LogActorRPNSGroup(actRef);

                    message += "\n";
                }

            }

        }

        Console.Write(message);

    }

    public Dictionary<int, List<ActorsByRPNSRef>> CreateActorsRPNSRefFromFile(IffFilesWithScripts mFile) {

        var total = new Dictionary<int, List<ActorsByRPNSRef>>();

        var actorsByRPNSRef = new Dictionary<RPNSRef, List<FCopActor>>();

        foreach (var actor in mFile.actors) {

            var list = actorsByRPNSRef.GetValueOrDefault(new RPNSRef(actor.rpnsReferences[0], actor.rpnsReferences[1], actor.rpnsReferences[2]));

            if (list != null) {
                list.Add(actor);
            }
            else {
                actorsByRPNSRef[new RPNSRef(actor.rpnsReferences[0], actor.rpnsReferences[1], actor.rpnsReferences[2])] = new List<FCopActor>() { actor };
            }

        }

        foreach (var actorsByRef in actorsByRPNSRef) {

            var groupedActors = new ActorsByRPNSRef(actorsByRef.Key, actorsByRef.Value, mFile);

            var list = total.GetValueOrDefault(groupedActors.sharedSameType);

            if (list != null) {
                list.Add(groupedActors);
            }
            else {
                total[groupedActors.sharedSameType] = new List<ActorsByRPNSRef>() { groupedActors };
            }

        }

        return total;

    }


    public class IffFilesWithScripts {

        public string name;

        public List<FCopActor> actors = new();
        public FCopRPNS rpns;
        public FCopFunction fun;

        public IffFilesWithScripts(IFFParser file, string name) {

            this.name = name;

            var actors = file.parsedData.files.Where(file => {

                return file.dataFourCC == "Cact" || file.dataFourCC == "Csac";

            });

            foreach (var a in actors) {

                this.actors.Add(new FCopActor(a));

            }

            var rawRPNS = file.parsedData.files.First(file => {

                return file.dataFourCC == "RPNS";

            });

            rpns = new FCopRPNS(rawRPNS);

            var rawCFun = file.parsedData.files.First(file => {

                return file.dataFourCC == "Cfun";

            });

            fun = new FCopFunction(rawCFun);

        }

    }

}