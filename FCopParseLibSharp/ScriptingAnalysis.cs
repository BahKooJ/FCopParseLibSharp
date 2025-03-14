
using FCopParser;
using System.Collections;
using System.Text;

class ScriptAnalysis {

    public List<string> fileNames = new() {
        "Mp", "M2C", "ConFt", "HK", "JOKE", "LAX1", "LAX2", "M1A1", "M3A",
        "M3B", "M4A1", "OV", "OVMP", "Slim", "Un"
    };

    public List<FCopLevel> levels = new();

    public ScriptAnalysis(List<IFFParser> files) {

        foreach (var file in files) {
            this.levels.Add(new FCopLevel(file.parsedData));
        }

    }

    public ScriptAnalysis(IFFParser file) {

        levels.Add(new FCopLevel(file.parsedData));

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

        public FCopLevel fileOrigin;

        public ActorsByRPNSRef(RPNSRef rpnsRefs, List<FCopActor> actors, FCopLevel fileOrigin) {

            this.rpnsRefs = rpnsRefs;
            this.actors = actors;
            this.fileOrigin = fileOrigin;

            var type = -1;
            var shareSameType = true;
            foreach (var actor in actors) {

                if (type == -1) {
                    type = (int)actor.behaviorType;
                }
                else if (type != (int)actor.behaviorType) {
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

    public void LogCshd() {

        var message = "";

        foreach (var file in levels) {

            message += file.ToString() + ": \n\n";

            var bytes = file.fileManager.GetFile("Cshd", 1).data;

            var i = 0;
            foreach (var b in bytes) {
                message += b + " ";
                i++;

                if (i == 12) {
                    message += "\n";
                    i = 0;
                }

            }
            message += "\n\n";

        }

        Console.WriteLine(message);
    
    }

    public void LogCdcs() {

        var message = "";

        foreach (var file in levels) {

            message += file.ToString() + ": \n\n";

            var files = file.fileManager.GetFiles("Cdcs");

            if (files.Count == 0) {
                message += "No Cdcs\n\n";
                continue;
            }

            var bytes = files[0].data;

            message += files[0].dataID + " ";

            var i = 0;
            foreach (var b in bytes) {
                message += b + " ";
                i++;

                //if (i == 12) {
                //    message += "\n";
                //    i = 0;
                //}

            }
            message += "\n\n";

        }

        Console.WriteLine(message);

    }

    public void LogCctr() {

        var message = "";

        foreach (var file in levels) {

            message += file.ToString() + ": \n\n";

            var files = file.fileManager.GetFiles("Cctr");

            if (files.Count == 0) {
                message += "No Cctr\n\n";
                continue;
            }
            else if (files.Count > 1) {
                Console.WriteLine("more than one ctr");
            }

            var bytes = files[0].data;

            var i = 0;
            foreach (var b in bytes) {
                message += b + " ";
                i++;

                //if (i == 12) {
                //    message += "\n";
                //    i = 0;
                //}

            }
            message += "\n\n";

        }

        Console.WriteLine(message);

    }

    static public void LogCtos(byte[] iffFileBytes, byte[] ctosBytes) {

        var message = "";

        var startI = BitConverter.ToInt32(iffFileBytes, 20);

        var i = 16;
        while (i < ctosBytes.Length) {

            var offset = BitConverter.ToInt32(ctosBytes, i);

            message += iffFileBytes[startI + offset] + "\n";

            i += 12;
        }

        Console.WriteLine(message);

    }

    public void LogAllActorTypes() {

        var total = new HashSet<int>();

        foreach (var file in levels) {

            foreach (var actor in file.sceneActors.actors) {
                total.Add((int)actor.behaviorType);
            }

        }

        foreach (var i in total) {


            Console.WriteLine(i);
        }

    }

    public void LogActorsAndSize() {

        var total = new HashSet<string>();

        foreach (var file in levels) {

            foreach (var actor in file.sceneActors.actors) {

                total.Add((int)actor.behaviorType + " " + actor.rawFile.data.Count);

            }

        }

        foreach (var i in total) {


            Console.WriteLine(i);
        }

    }

    public void LogActorsAndBlocks() {

        var total = new HashSet<string>();

        foreach (var file in levels) {

            foreach (var actor in file.sceneActors.actors) {

                var propertyCount = (Utils.BytesToInt(actor.rawFile.data.ToArray(), 4) - 28) / 2;


                total.Add((int)actor.behaviorType + " " + propertyCount);

            }

        }

        foreach (var i in total) {

            Console.WriteLine(i);

        }

    }

    public void LogUniqueActorRefs() {

        var total = new HashSet<string>();

        foreach (var file in levels) {

            foreach (var actor in file.sceneActors.actors) {

                foreach (var r in actor.resourceReferences) {
                    total.Add(r.fourCC);

                }

            }

        }

        foreach (var i in total) {


            Console.WriteLine(i);
        }

    }

    public void LogAllActorRefs() {

        var message = "";


        foreach (var file in levels) {

            message += file.ToString() + ": \n";

            foreach (var actor in file.sceneActors.actors) {

                message += (int)actor.behaviorType + ":\n";

                foreach (var r in actor.resourceReferences) {
                    message += r.fourCC + " " + r.id + "\n";
                }

            }

        }

        Console.WriteLine(message);

    }

    public void LogSomethingIDK() {

        var total = new HashSet<string>();

        foreach (var file in levels) {

            foreach (var actor in file.sceneActors.actors) {

                if (actor.behavior is FCopBehavior11 e) {

                    total.Add(e.rotation.value.compiledRotation.ToString());

                }


            }

        }

        foreach (var i in total) {

            Console.WriteLine(i);

        }

    }

    public void LogCobjChunk(string fourCC) {

        var total = new StringBuilder();

        foreach (var file in levels) {

            total.Append(file.ToString() + ": \n\n");

            foreach (var obj in file.objects) {

                var headers = obj.offsets.Where(offset => offset.fourCCDeclaration == fourCC).ToList();

                if (headers.Count == 0) {
                    total.Append("no chunk \n");
                    continue;
                }

                total.Append("Count: " + headers.Count + "\n");

                foreach (var header in headers) {

                    var bytes = obj.rawFile.data.GetRange(header.index, header.chunkSize);

                    foreach (var b in bytes) {
                        total.Append(b.ToString("X2") + " ");
                    }

                    total.Append("\n");

                }



            }

            total.Append("\n\n");

        }

        Console.WriteLine(total.ToString());

    }

    public void LogTopHeaderCshd() {

        var message = "";

        foreach (var file in levels) {

            message += file.ToString() + ": \n\n";

            var bytes = file.fileManager.GetFile("Cshd", 1).data;

            var i = 2;
            foreach (var b in bytes) {

                if (i == 4 || i == 5) {
                    message += b + " ";
                }
                i++;

                if (i == 12) {
                    message += "\n";
                    i = 0;
                }

            }
            message += "\n\n";

        }

        Console.WriteLine(message);

    }

    public void GetAllFourCC() {

        var totalFourCCs = new HashSet<string>();

        foreach (var file in levels) {

            foreach (var dataFile in file.fileManager.files) {

                totalFourCCs.Add(dataFile.dataFourCC);

            }

        }

        foreach (var s in totalFourCCs) {
            Console.WriteLine(s);
        }

    }

    public void BitCompareActors(int id, int index) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<bool>>();

        var fileI = 0;
        foreach (var file in levels) {

            message += fileNames[fileI] + ": \n";

            var actors = file.sceneActors.actors.Where(actor => {

                return (int)actor.behaviorType == id;

            });

            foreach (var actor in actors) {

                var offset = index;

                var bits = new BitArray(new byte[] { actor.rawFile.data[offset] });

                var bi = 0;
                foreach (bool bit in bits) {

                    if (!uniqueValueCounter.ContainsKey(bi)) {

                        uniqueValueCounter[bi] = new();
                    }

                    uniqueValueCounter[bi].Add(bit);

                    bi++;

                }


            }

            fileI++;

        }

        foreach (var pair in uniqueValueCounter) {
            message += pair.Key + ": ";

            foreach (var value in pair.Value) {
                message += (value ? "1" : "0") + " ";
            }
            message += "\n";
        }

        Console.WriteLine(message);

    }

    public void CompareActors(int id) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<byte>>();

        var fileI = 0;
        foreach (var file in levels) {
            
            message += fileNames[fileI] + ": \n";

            var actors = file.sceneActors.actors.Where(actor => {

                return (int)actor.behaviorType == id;

            });

            foreach (var actor in actors) {

                var offset = 28;

                foreach (var i in Enumerable.Range(offset, Utils.BytesToInt(actor.rawFile.data.ToArray(), 4) - 28)) {
                    message += actor.rawFile.data[i].ToString("X2") + " ";

                    if (uniqueValueCounter.ContainsKey(i)) {
                        uniqueValueCounter[i].Add(actor.rawFile.data[i]);
                    }
                    else {
                        uniqueValueCounter[i] = new();
                        uniqueValueCounter[i].Add(actor.rawFile.data[i]);
                    }

                }



                message += "\n";

            }

            fileI++;

        }

        foreach (var pair in uniqueValueCounter) {
            message += pair.Key + ": ";

            var sortedValues = pair.Value.OrderBy(v => v);
            foreach (var value in sortedValues) {

                message += value.ToString("X2") + " ";
            }
            message += "\n";
        }

        Console.WriteLine(message);

    }

    public void EndianCompareActors(int type, List<MacIFFLevelParser> macFiles) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<int>>();

        var fileI = 0;
        foreach (var file in levels) {


            var actors = file.sceneActors.actors.Where(actor => {

                return (int)actor.behaviorType == type;

            });

            foreach (var actor in actors) {

                var offset = 28;

                var macFile = macFiles[fileI];
                
                var length = Utils.BytesToInt(actor.rawFile.data.ToArray(), 4) - 28;

                var compareData = macFile.BitComparison(actor.rawFile.dataFourCC, actor.DataID, offset, length, actor.rawFile.data.GetRange(offset, length));

                var i = offset;
                foreach (var compare in compareData) {

                    if (uniqueValueCounter.ContainsKey(i)) {
                        uniqueValueCounter[i].Add(compare);
                    }
                    else {
                        uniqueValueCounter[i] = new();
                        uniqueValueCounter[i].Add(compare);
                    }

                    i++;
                }

            }

            fileI++;

        }

        foreach (var pair in uniqueValueCounter) {
            message += pair.Key + ": ";

            var sortedValues = pair.Value.OrderBy(v => v);
            foreach (var value in sortedValues) {

                message += value.ToString() + " ";
            }
            message += "\n";
        }

        Console.WriteLine(message);

    }

    public void BitCompareActorsRange(List<int> types, int index) {

        var message = "";
        var uniqueValueCounter = new HashSet<byte>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {

                uniqueValueCounter.Add(actor.rawFile.data[index]);

            }

            fileI++;

        }

        message += index + ": ";
        var sortedValues = uniqueValueCounter.OrderBy(v => v);

        var bitArrays = new List<BitArray>();

        foreach (var value in sortedValues) {

            message += value.ToString("X2") + " ";

            bitArrays.Add(new BitArray(new byte[] { value }));

        }
        message += "\n";

        foreach (var i in Enumerable.Range(0, 8)) {

            message += i.ToString() + ": ";

            foreach (var bitArray in bitArrays) {

                message += (bitArray[i] ? "11" : "00") + " ";

            }
            message += "\n";

        }


        Console.WriteLine(message);

    }

    public void Data16RangeCompareActorsRange(List<int> types, int index) {

        var message = "";
        var uniqueValueCounter = new HashSet<int>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {
                
                uniqueValueCounter.Add(BitConverter.ToInt16(actor.rawFile.data.ToArray(), index));

            }

            fileI++;

        }

        message += index + ": ";
        var sortedValues = uniqueValueCounter.OrderBy(v => v);

        foreach (var value in sortedValues) {

            message += value.ToString() + " ";

        }
        message += "\n";

        Console.WriteLine(message);

    }

    public void CompareActorsRange(List<int> types) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<byte>>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {

                var offset = 28;

                foreach (var i in Enumerable.Range(offset, Utils.BytesToInt(actor.rawFile.data.ToArray(), 4) - 28)) {

                    if (uniqueValueCounter.ContainsKey(i)) {
                        uniqueValueCounter[i].Add(actor.rawFile.data[i]);
                    }
                    else {
                        uniqueValueCounter[i] = new();
                        uniqueValueCounter[i].Add(actor.rawFile.data[i]);
                    }

                }

            }

            fileI++;

        }

        foreach (var pair in uniqueValueCounter) {
            message += pair.Key + ": ";

            var sortedValues = pair.Value.OrderBy(v => v);
            foreach (var value in sortedValues) {
                
                message += value.ToString("X2") + " ";
            }
            message += "\n";
        }

        Console.WriteLine(message);

    }

    public void LogAllCobjChunks() {

        var message = "";

        var fileI = 0;
        foreach (var file in levels) {

            message += fileNames[fileI] + ": \n";

            foreach (var obj in file.objects) {

                foreach (var header in obj.offsets) {
                    message += header.fourCCDeclaration + " ";
                }
                message += "\n";

            }

            fileI++;

        }

        Console.WriteLine(message);

    }

    //public void LogAllCnetNodeData() {

    //    var message = new StringBuilder();
    //    var uniqueValueCounter = new Dictionary<int, HashSet<byte>>();

    //    var fileI = 0;
    //    foreach (var file in levels) {

    //        message.Append(fileNames[fileI] + ": \n");

    //        foreach (var cnet in file.navMeshes) {

    //            foreach (var node in cnet.nodes) {

    //                var bi = 0;
    //                foreach (var b in node.data) {

    //                    message.Append(b.ToString("X2") + " ");

    //                    if (uniqueValueCounter.ContainsKey(bi)) {
    //                        uniqueValueCounter[bi].Add(b);
    //                    }
    //                    else {
    //                        uniqueValueCounter[bi] = new();
    //                    }

    //                    bi++;
    //                }

    //                message.Append('\n');

    //            }

    //        }

    //        fileI++;

    //    }

    //    foreach (var pair in uniqueValueCounter) {
    //        message.Append(pair.Key + ": ");

    //        foreach (var value in pair.Value) {
    //            message.Append(value.ToString("X2") + " ");
    //        }
    //        message.Append('\n');
    //    }

    //    Console.WriteLine(message.ToString());

    //}

}