
using FCopParser;
using System.Collections;

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

    public ScriptAnalysis((IFFParser file, string name) file) {
        
        files.Add(new IffFilesWithScripts(file.file, file.name));

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

    public void LogRPNSCode() {

        var message = "";

        foreach (var file in files) {

            message += file.name + ": \n\n";

            foreach (var line in file.rpns.code) {
                message += "```\n";

                foreach (var b in line) {
                    message += b + " ";
                }

                message += "\n```\n\n";

            }

        }

        Console.WriteLine(message);

    }

    public void LogCfunCode() {

        var total = "";

        foreach (var file in files) {

            total += file.name + ": \n\n";

            foreach (var code in file.fun.tFUNData) {

                total += code.number1 + " " + code.number2 + " " + code.number3 + " " + code.startingOffset + " " + code.endingOffset + "\n";
                total += "Line1: ";
                foreach (var b in code.line1) {
                    total += b + " ";
                }
                total += "\nLine2: ";
                foreach (var b in code.line2) {
                    total += b + " ";
                }
                total += "\n\n";

            }

        }

        Console.WriteLine(total);


    }

    public void LogCfunCodeWithBits() {

        void Reverse(BitArray array) {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++) {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }

        var total = "";

        foreach (var file in files) {

            total += file.name + ": \n\n";

            foreach (var code in file.fun.tFUNData) {

                total += code.number1 + " " + code.number2 + " " + code.number3 + " " + code.startingOffset + " " + code.endingOffset + "\n";
                total += "Line1: ";
                foreach (var b in code.line1) {
                    total += b + " ";
                }
                total += "\n";
                foreach (var b in code.line1) {

                    var bits = new BitArray(new byte[] { b });
                    Reverse(bits);

                    foreach (bool bit in bits) {
                        total += bit ? 1 : 0;
                    }

                    total += " ";

                }
                total += "\n";
                foreach (var b in code.line1) {

                    var bits = new BitArray(new byte[] { b });
                    Reverse(bits);

                    foreach (bool bit in bits) {
                        total += bit ? 1 : 0;
                    }

                }


                total += "\nLine2: ";
                foreach (var b in code.line2) {
                    total += b + " ";
                }
                total += "\n";
                foreach (var b in code.line2) {

                    var bits = new BitArray(new byte[] { b });
                    Reverse(bits);

                    foreach (bool bit in bits) {
                        total += bit ? 1 : 0;
                    }

                    total += " ";

                }
                total += "\n";
                foreach (var b in code.line2) {

                    var bits = new BitArray(new byte[] { b });
                    Reverse(bits);

                    foreach (bool bit in bits) {
                        total += bit ? 1 : 0;
                    }

                }
                total += "\n\n";

            }

        }

        Console.WriteLine(total);

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

    string AnalyseCode(List<byte> code) {
        var message = "";

        var offset = 0;

        try {

            while (offset < code.Count) {

                if (code[offset + 1] == 131 && code[offset + 2] == 30) {

                    message += "Sound(" + code[offset] + " " + code[offset + 1] + " " + code[offset + 2] + ") ";
                    offset += 3;
                    continue;

                }

                if (code[offset + 1] == 199 && code[offset + 2] == 128 && code[offset + 3] == 60) {

                    message += "Spawn(" + code[offset] + " " + code[offset + 1] + " " + code[offset + 2] + " " + code[offset + 3] + ") ";
                    offset += 4;
                    continue;

                }

                if (code[offset + 1] == 24) {

                    message += "Function Call?(" + code[offset] + " " + code[offset + 1] + ") ";

                    offset += 2;

                    continue;

                }

                if (code[offset + 1] == 250 && code[offset + 2] == 129 && code[offset + 3] == 56) {

                    message += "Unknown(" + code[offset] + " " + code[offset + 1] + " " + code[offset + 2] + " " + code[offset + 3] + ") ";
                    offset += 4;
                    continue;

                }

                if (code[offset] == 8 && code[offset + 1] == 4) {

                    message += "Else?(" + code[offset] + " " + code[offset + 1] + ") ";
                    offset += 2;
                    continue;

                }

                if (code[offset] == 21) {
                    message += "PlusPlus(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 25) {
                    message += "MinusMinus(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 16) {
                    message += "Get?(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 37) {
                    message += "IsLessThan(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 35) {
                    message += "IsGreaterThan(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                message += code[offset] + " ";
                offset++;

            }

        }
        catch (Exception) {

            if (offset < code.Count - 1) {

                foreach (var b in code.GetRange(offset, code.Count - offset)) {
                    message += b + " ";
                }

            }
            else if (offset == code.Count - 1) {
                message += code[offset] + " ";
            }


            message += "\n\n";
            return message;

        }

        return message;

    }

    public void AnalyseRPNSCode() {

        var message = "";

        foreach (var file in files) {

            message += file.name + ": \n\n";

            foreach (var code in file.rpns.code) {

                message += AnalyseCode(code);

            }

        }

        Console.WriteLine(message);

    }

    public void AnalyseCfunCode() {

        var message = "";

        foreach (var file in files) {

            message += file.name + ": \n\n";

            foreach (var code in file.fun.tFUNData) {

                message += AnalyseCode(code.line1);
                message += AnalyseCode(code.line2);

                message += "\n";
            }

        }

        Console.WriteLine(message);

    }

    public void CompareActor14() {

        var message = "";

        foreach (var file in files) {

            message += file.name + ": \n";

            var actors = file.actors.Where(actor => {

                return actor.objectType == 14;

            });

            foreach (var actor in actors) {

                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 28) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 30) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 32) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 34) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 36) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 38) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 40) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 42) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 44) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 46) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 48) + " ";
                message += Utils.BytesToShort(actor.rawFile.data.ToArray(), 50) + " ";

                message += "\n";

            }

        }

        Console.WriteLine(message);

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
        public IFFParser file;

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

            try {
                var rawCFun = file.parsedData.files.First(file => {

                    return file.dataFourCC == "Cfun";

                });

                fun = new FCopFunction(rawCFun);
            } catch (Exception e) {
                fun = null;
            }

            this.file = file;

        }

    }

}