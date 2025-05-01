
using FCopParser;
using System;
using System.Collections;
using System.Reflection;
using System.Text;

class ScriptAnalysis {

    public List<string> fileNames = new() {
        "Mp", "M2C", "ConFt", "HK", "JOKE", "LAX1", "LAX2", "M1A1", "M3A",
        "M3B", "M4A1", "OV", "OVMP", "Slim", "Un"
    };

    public List<FCopLevel> levels = new();

    public ScriptAnalysis(List<IFFParser> files) {

        var i = 0;
        foreach (var file in files) {
            var name = fileNames[i];
            this.levels.Add(new FCopLevel(file.parsedData));
            i++;
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

    public void DataRangeCompareActorsRangeByBehaviorType(List<int> types, int index, bool bit16) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<int>>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {

                if (uniqueValueCounter.ContainsKey((int)actor.behaviorType)) {

                    if (bit16) {
                        uniqueValueCounter[(int)actor.behaviorType].Add(BitConverter.ToInt16(actor.rawFile.data.ToArray(), index));
                    }
                    else {
                        uniqueValueCounter[(int)actor.behaviorType].Add(actor.rawFile.data[index]);
                    }

                }
                else {
                    uniqueValueCounter[(int)actor.behaviorType] = new();
                    if (bit16) {
                        uniqueValueCounter[(int)actor.behaviorType].Add(BitConverter.ToInt16(actor.rawFile.data.ToArray(), index));
                    }
                    else {
                        uniqueValueCounter[(int)actor.behaviorType].Add(actor.rawFile.data[index]);
                    }

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

    public void DataRangeCompareActorsRangeByLevel(List<int> types, int index, bool bit16) {

        var message = "";
        var uniqueValueCounter = new Dictionary<string, HashSet<int>>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {

                if (uniqueValueCounter.ContainsKey(fileNames[fileI])) {

                    if (bit16) {
                        uniqueValueCounter[fileNames[fileI]].Add(BitConverter.ToInt16(actor.rawFile.data.ToArray(), index));
                    }
                    else {
                        uniqueValueCounter[fileNames[fileI]].Add(actor.rawFile.data[index]);
                    }

                }
                else {
                    uniqueValueCounter[fileNames[fileI]] = new();
                    if (bit16) {
                        uniqueValueCounter[fileNames[fileI]].Add(BitConverter.ToInt16(actor.rawFile.data.ToArray(), index));
                    }
                    else {
                        uniqueValueCounter[fileNames[fileI]].Add(actor.rawFile.data[index]);
                    }

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

    public void EntityExplosionActorAnalysis() {

        List<int> entitiesIds = new() { 1, 5, 6, 8, 9, 10, 11, 12, 16, 20, 25, 26, 27, 28, 29, 30, 31, 32, 33, 36, 37, 38 };
        List<int> explosionIds = new() { 87, 88, 89, 90, 91, 92, 93, 94 };

        Dictionary<int, HashSet<int>> result = new();

        var fileI = 0;
        foreach (var file in levels) {

            var entities = file.sceneActors.actors.Where(actor => {

                return entitiesIds.Contains((int)actor.behaviorType);

            });

            var explosions = file.sceneActors.actors.Where(actor => {

                return explosionIds.Contains((int)actor.behaviorType);

            });

            foreach (var e in entities) {

                var explosionRef = e.rawFile.data[40];

                var potentialExplosion = explosions.Where(ex => ex.rawFile.data[28] == explosionRef).ToList();

                if (potentialExplosion.Count != 0) {

                    foreach (var explosion in potentialExplosion) {

                        if (result.ContainsKey((int)explosion.behaviorType)) {

                            result[(int)explosion.behaviorType].Add(explosionRef);

                        }
                        else {

                            result[(int)explosion.behaviorType] = new();
                            result[(int)explosion.behaviorType].Add(explosionRef);

                        }

                    }

                }
                else {

                    if (explosionRef != 0) {

                        Console.WriteLine(fileNames[fileI] + " " + explosionRef);

                    }

                    if (result.ContainsKey(-1)) {

                        result[-1].Add(explosionRef);

                    }
                    else {

                        result[-1] = new();
                        result[-1].Add(explosionRef);

                    }

                }


            }

            fileI++;
        }

        var message = "";
        foreach (var pair in result) {
            message += pair.Key + ": ";

            var sortedValues = pair.Value.OrderBy(v => v);
            foreach (var value in sortedValues) {

                message += value.ToString() + " ";
            }
            message += "\n";
        }
        Console.WriteLine(message);
    }

    public void TestForExplosionIDRef(int behavior) {

        List<int> explosionIds = new() { 87, 88, 89, 90, 91, 92, 93, 94 };

        Dictionary<int, int> result = new();

        foreach (var file in levels) {

            var explosions = file.sceneActors.actors.Where(actor => {

                return explosionIds.Contains((int)actor.behaviorType);

            });

            var actors = file.sceneActors.actors.Where(actor => {

                return (int)actor.behaviorType == behavior;

            });

            foreach (var actor in actors) {

                var offset = 28;

                foreach (var i in Enumerable.Range(offset, Utils.BytesToInt(actor.rawFile.data.ToArray(), 4) - 28)) {

                    var byt = actor.rawFile.data[i];

                    foreach (var explosion in explosions) {

                        if (byt == explosion.rawFile.data[28]) {

                            if (result.ContainsKey(i)) {
                                result[i]++;
                            }
                            else {
                                result[i] = 1;
                            }

                        }

                    }

                }

            }

        }

        var message = "";
        var sortedValues = result.OrderBy(v => v.Value);
        foreach (var value in sortedValues) {

            message += value.Key.ToString() + ": " + value.Value.ToString();
            message += "\n";

        }

        Console.WriteLine(message);

    }

    public void DataBitRangeCompareActorsRangeByBehaviorType(List<int> types, int index, int comparitor) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<bool>>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {

                if (uniqueValueCounter.ContainsKey((int)actor.behaviorType)) {
                    
                    uniqueValueCounter[(int)actor.behaviorType].Add((actor.rawFile.data[index] & comparitor) == comparitor);

                }
                else {
                    uniqueValueCounter[(int)actor.behaviorType] = new();
                    uniqueValueCounter[(int)actor.behaviorType].Add((actor.rawFile.data[index] & comparitor) == comparitor);

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

    public void DataBitRangeCompareActorsRangeByLevel(List<int> types, int index, int comparitor) {

        var message = "";
        var uniqueValueCounter = new Dictionary<string, HashSet<bool>>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {

                if (uniqueValueCounter.ContainsKey(fileNames[fileI])) {

                    uniqueValueCounter[fileNames[fileI]].Add((actor.rawFile.data[index] & comparitor) == comparitor);

                }
                else {
                    uniqueValueCounter[fileNames[fileI]] = new();
                    uniqueValueCounter[fileNames[fileI]].Add((actor.rawFile.data[index] & comparitor) == comparitor);

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

    public void PropertyCompareActors(List<int> types, int indexKey, bool bit16Key, int indexComp, bool bit16Comp, int value) {

        var message = "";
        var uniqueValueCounter = new HashSet<int>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                if (types.Contains((int)actor.behaviorType)) {

                    int keyValue;

                    if (bit16Key) {
                        keyValue = BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexKey);
                    }
                    else {
                        keyValue = actor.rawFile.data[indexKey];
                    }

                    return keyValue == value;

                }

                return false;

            });

            foreach (var actor in actors) {

                if (bit16Comp) {
                    uniqueValueCounter.Add(BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexComp));
                }
                else {
                    uniqueValueCounter.Add(actor.rawFile.data[indexComp]);
                }

            }

            fileI++;

        }


        var sortedValues = uniqueValueCounter.OrderBy(v => v);
        foreach (var val in sortedValues) {

            message += val.ToString() + " ";
        }
        message += "\n";


        Console.WriteLine(message);

    }

    public void PropertyCompareActors(List<int> types, int indexKey, bool bit16Key, int indexComp, bool bit16Comp) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<int>>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return types.Contains((int)actor.behaviorType);

            });

            foreach (var actor in actors) {

                if (uniqueValueCounter.ContainsKey(bit16Key ? BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexKey) : actor.rawFile.data[indexKey])) {
                    uniqueValueCounter[bit16Key ? BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexKey) : actor.rawFile.data[indexKey]].Add(
                        bit16Comp ? BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexComp) : actor.rawFile.data[indexComp]);
                }
                else {
                    uniqueValueCounter[bit16Key ? BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexKey) : actor.rawFile.data[indexKey]] = new();
                    uniqueValueCounter[bit16Key ? BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexKey) : actor.rawFile.data[indexKey]].Add(
                        bit16Comp ? BitConverter.ToInt16(actor.rawFile.data.ToArray(), indexComp) : actor.rawFile.data[indexComp]);
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

    public void CompareTSAC() {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<byte>>();

        var fileI = 0;
        foreach (var file in levels) {

            message += fileNames[fileI] + ": \n";

            var actors = file.sceneActors.actors.Where(actor => {

                return actor.tSACData != null;

            });

            foreach (var actor in actors) {

                foreach (var i in Enumerable.Range(0, actor.tSACData.Count)) {
                    message += actor.tSACData[i].ToString("X2") + " ";

                    if (uniqueValueCounter.ContainsKey(i)) {
                        uniqueValueCounter[i].Add(actor.tSACData[i]);
                    }
                    else {
                        uniqueValueCounter[i] = new();
                        uniqueValueCounter[i].Add(actor.tSACData[i]);
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

    public void CompareResourceRefs(int id) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<string>>();

        var fileI = 0;
        foreach (var file in levels) {

            var actors = file.sceneActors.actors.Where(actor => {

                return (int)actor.behaviorType == id;

            });

            foreach (var actor in actors) {

                foreach (var i in Enumerable.Range(0, actor.resourceReferences.Count)) {


                    if (uniqueValueCounter.ContainsKey(i)) {
                        uniqueValueCounter[i].Add(actor.resourceReferences[i].fourCC);
                    }
                    else {
                        uniqueValueCounter[i] = new();
                        uniqueValueCounter[i].Add(actor.resourceReferences[i].fourCC);
                    }

                }

            }

            fileI++;

        }

        foreach (var pair in uniqueValueCounter) {
            message += pair.Key + ": ";

            var sortedValues = pair.Value.OrderBy(v => v);
            foreach (var value in sortedValues) {

                message += value + " ";
            }
            message += "\n";
        }

        Console.WriteLine(message);

    }

    public void EndianCompareTSAC(List<MacIFFLevelParser> macFiles) {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<int>>();

        var fileI = 0;
        foreach (var file in levels) {


            var actors = file.sceneActors.actors.Where(actor => {

                return actor.tSACData != null;

            });

            foreach (var actor in actors) {

                var offset = actor.offsets.FirstOrDefault(h => h.fourCCDeclaration == FCopActor.FourCC.tSAC).index;

                var macFile = macFiles[fileI];

                var length = actor.tSACData.Count;

                var compareData = macFile.BitComparison(actor.rawFile.dataFourCC, actor.DataID, offset, length, actor.rawFile.data.GetRange(offset, length));

                var i = 0;
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

    public void CompareCdcs() {

        var message = "";
        var uniqueValueCounter = new Dictionary<int, HashSet<byte>>();

        var zeros = new HashSet<byte>();
        
        var fileI = 0;
        foreach (var file in levels) {

            message += fileNames[fileI] + ": \n";

            var dcs = file.fileManager.files.FirstOrDefault(file => {

                return file.dataFourCC == "Cdcs";

            });

            if (dcs == null) {
                fileI++;
                message += "\n";
                continue;
            }

            var zi = -12;
            foreach (var i in Enumerable.Range(0, dcs.data.Count)) {
                message += dcs.data[i].ToString("X2") + " ";

                if (uniqueValueCounter.ContainsKey(i)) {
                    uniqueValueCounter[i].Add(dcs.data[i]);
                }
                else {
                    uniqueValueCounter[i] = new();
                    uniqueValueCounter[i].Add(dcs.data[i]);
                }

                if (zi == 5 || zi == 6) {
                    zeros.Add(dcs.data[i]);
                }

                if (zi == 7) {
                    zi = -1;
                }
                

                zi++;
            }

            message += "\n";

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

        message += "\nZeros: ";

        foreach (var v in zeros) {

            message += v.ToString() + " ";

        }

        Console.WriteLine(message);

    }

    public void LogAllCdcsIDs() {

        var message = "";

        foreach (var file in levels) {

            var dcs = file.fileManager.files.FirstOrDefault(file => {

                return file.dataFourCC == "Cdcs";

            });

            if (dcs != null) {
                message += dcs.dataID.ToString() + "\n";
                continue;
            }

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

    public void DiscoverScriptingByte(byte b) {

        FCopScript foundByte = null;

        var fileI = 0;
        foreach (var file in levels) {

            foreach (var script in file.scripting.rpns.code) {

                if (script.Value.compiledBytes.Contains(b)) { 
                    foundByte = script.Value;
                    break; 
                }

            }

            foreach (var func in file.scripting.functionParser.functions) {

                if (func.runCondition.compiledBytes.Contains(b)) {
                    foundByte = func.runCondition;
                    break;
                }

                if (func.code.compiledBytes.Contains(b)) {
                    foundByte = func.code;
                    break;
                }

            }

            fileI++;

        }

        if (foundByte != null) {

            foreach (var b2 in foundByte.compiledBytes) {
                Console.Write(b2 + " ");

            }

        }

    }

    public void LogFloatingExpressions() {

        HashSet<ByteCode> result = new();

        var fileI = 0;
        foreach (var file in levels) {

            foreach (var script in file.scripting.rpns.code) {

                var instructions = script.Value.assembly.Where(it => it.byteCode == ByteCode.NONE);

                foreach (var instruction in instructions) {
                    result.Add(instruction.parameters[0].byteCode);
                }

            }

            foreach (var func in file.scripting.functionParser.functions) {

                var instructions = func.runCondition.assembly.Where(it => it.byteCode == ByteCode.NONE).ToList();
                instructions.AddRange(func.code.assembly.Where(it => it.byteCode == ByteCode.NONE));

                foreach (var instruction in instructions) {
                    result.Add(instruction.parameters[0].byteCode);

                }

            }

            fileI++;

        }

        foreach (var res in result) {
            Console.WriteLine(res);
        }

    }

    public void LogScript(FCopScript script) {

        var message = new StringBuilder();

        void LogExpression(StringBuilder stringBuilder, ByteInstruction instruction) {

            stringBuilder.Append("<" + instruction.byteCode + ", " + instruction.value + ">(");

            if (instruction.parameters.Count != 0) {

                foreach (var paramter in instruction.parameters) {
                    LogExpression(stringBuilder, paramter);
                    message.Append(", ");
                }

                message.Remove(message.Length - 2, 2);

            }

            stringBuilder.Append(')');

        }

        foreach (var instruction in script.assembly) {

            message.Append("[" + instruction.byteCode + " (");

            if (instruction.parameters.Count != 0) {

                foreach (var paramter in instruction.parameters) {
                    LogExpression(message, paramter);
                    message.Append(", ");
                }

                message.Remove(message.Length - 2, 2);

            }

            message.Append(")]\n");

        }

        Console.Write(message.ToString());

    }

}