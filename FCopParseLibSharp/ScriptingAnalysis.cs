
using FCopParser;
using System.Collections;


class ScriptAnalysis {

    public List<FCopLevel> levels = new();

    public ScriptAnalysis(List<IFFParser> files) {

        foreach (var file in files) {
            this.levels.Add(new FCopLevel(file));
        }

    }

    public ScriptAnalysis(IFFParser file) {

        levels.Add(new FCopLevel(file));

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
                    type = actor.actorType;
                }
                else if (type != actor.actorType) {
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

    public void LogCfunCode() {

        var message = "";

        foreach (var file in levels) {

            message += file.ToString() + ": \n\n";

            foreach (var code in file.functions.tFUNData) {

                message += "tFUN Struct: " + code.number1 + " " + code.number2 + " " + code.number3 + " " + code.line1Offset + " " + code.line2Offset + "\n";

                message += "Line 1: \n";
                message += "```\n";
                foreach (var b in code.line1.compiledBytes) {
                    message += b.ToString() + " ";
                }
                message += "\n```\n";

                message += "Line 2: \n";
                message += "```\n";
                foreach (var b in code.line2.compiledBytes) {
                    message += b.ToString() + " ";
                }
                message += "\n```\n";

                message += "\n";
            }

        }

        Console.WriteLine(message);


    }

    public void AnalyseCfunCode() {

        var message = "";

        foreach (var file in levels) {

            message += file.ToString() + ": \n\n";

            foreach (var code in file.functions.tFUNData) {

                message += "Line1: " + AnalyseCode(code.line1.compiledBytes);
                message += "\n";
                message += "Line2: " + AnalyseCode(code.line2.compiledBytes);

                message += "\n";
            }

        }

        Console.WriteLine(message);

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

                message += "(Type: " + actor.actorType + ", ID: " + actor.id + ") ";

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

        foreach (var file in levels) {

            message += file.ToString() + ": \n\n";

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

    class Expression {

        public Operator operationType;
        public List<Expression> nestedExpressions = new();
        public object value = null;

        public Expression(List<Expression> nestedExpressions, Operator operationType) {
            this.nestedExpressions = nestedExpressions;
            this.operationType = operationType;
        }

        public Expression(object value, Operator operationType) {
            this.value = value;
            this.operationType = operationType;
        }

    }

    class Statement {

        public Instruction instruction;
        public List<Expression> parametes = new();

        public Statement(Instruction instruction) {
            this.instruction = instruction;
        }

    }

    enum Operator {

        Literal = 256,
        Get16 = 16,
        Get18 = 18,
        Get19 = 19,
        Equal = 33,
        GreaterThan = 35,
        GreaterThanOrEqual = 36,
        LessThan = 37,
        Subtract = 40,
        And = 44

    }

    enum Instruction {

        None = 256,
        End = 0,
        Jump = 8,
        Unknown12 = 12,
        ConditionalJump = 20,
        Increment = 21,
        Unknown24 = 24,
        Decrement = 25,
        Set = 29,
        Sound = 30,
        Unknown31 = 31,
        Unknown32 = 32,
        Add = 48,
        Subtract = 52,
        Destroy = 56,
        Unknown57 = 57,
        Spawn = 60

    }

    List<Operator> doubleExpressionOperators = new() { 
        Operator.GreaterThan, Operator.LessThan, Operator.And, Operator.Equal, Operator.Subtract, Operator.GreaterThanOrEqual
    };

    public string AnalyseCode(List<byte> code) {
        var message = "";

        var offset = 0;

        try {

            while (offset < code.Count) {

                if (code[offset] == 8) {
                    message += "Else(" + code[offset] + ", Size: " + code[offset + 1] + ") ";
                    offset += 2;
                    continue;
                }

                if (code[offset] == 16) {
                    message += "Get(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 20) {
                    message += "If(" + code[offset] + ", Size: " + code[offset + 1] + ") ";
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

                if (code[offset] == 33) {
                    message += "IsEqual(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 35) {
                    message += "IsGreaterThan(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 37) {
                    message += "IsLessThan(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 48) {
                    message += "Add(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 56) {
                    message += "Destroy?(" + code[offset] + ") ";
                    offset++;
                    continue;
                }

                if (code[offset] == 60) {
                    message += "Spawn?(" + code[offset] + ") ";
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

    public string AnalyseCodeButBetter(List<byte> code) {

        List<Expression> floatingExpressions = new();
        List<Statement> statements = new List<Statement>();

        var i = 0;
        while (i < code.Count) {

            var b = code[i];

            if (Enum.IsDefined(typeof(Operator), (Int32)b)) {

                var opCase = (Operator)b;

                if (opCase == Operator.Get16 || opCase == Operator.Get18 || opCase == Operator.Get19) {

                    var lastExpression = floatingExpressions.Last();
                    floatingExpressions.RemoveAt(floatingExpressions.Count - 1);
                    floatingExpressions.Add(new Expression(new List<Expression>() { lastExpression }, opCase));

                }
                else if (doubleExpressionOperators.Contains(opCase)) {

                    var leftAndRight = new List<Expression> {
                            floatingExpressions[^2],
                            floatingExpressions.Last()
                        };

                    floatingExpressions.RemoveAt(floatingExpressions.Count - 1);
                    floatingExpressions.RemoveAt(floatingExpressions.Count - 1);

                    floatingExpressions.Add(new Expression(leftAndRight, opCase));

                }

            }
            else if (Enum.IsDefined(typeof(Instruction), (Int32)b)) {

                var instuctionCase = (Instruction)b;

                if (instuctionCase == Instruction.ConditionalJump) {

                    var state = new Statement(instuctionCase);

                    state.parametes.Add(floatingExpressions.Last());

                    floatingExpressions.RemoveAt(floatingExpressions.Count - 1);

                    state.parametes.Add(new Expression(code[i + 1], Operator.Literal));

                    statements.Add(state);

                    i += 2;
                    continue;

                }
                else if (instuctionCase == Instruction.Jump) {

                    var state = new Statement(instuctionCase);

                    state.parametes.Add(new Expression(code[i + 1], Operator.Literal));

                    statements.Add(state);

                    i += 2;
                    continue;

                }
                else if (instuctionCase == Instruction.Spawn || 
                    instuctionCase == Instruction.Destroy ||
                    instuctionCase == Instruction.Unknown57) {

                    var state = new Statement(instuctionCase);

                    state.parametes.Add(floatingExpressions[^3]);
                    state.parametes.Add(floatingExpressions[^2]);
                    state.parametes.Add(floatingExpressions[^1]);

                    statements.Add(state);

                    floatingExpressions.RemoveRange(floatingExpressions.Count - 3, 3);

                }
                else if (instuctionCase == Instruction.Sound ||
                    instuctionCase == Instruction.Unknown31 ||
                    instuctionCase == Instruction.Unknown32 ||
                    instuctionCase == Instruction.Add ||
                    instuctionCase == Instruction.Subtract ||
                    instuctionCase == Instruction.Set) {

                    var state = new Statement(instuctionCase);

                    state.parametes.Add(floatingExpressions[^2]);
                    state.parametes.Add(floatingExpressions[^1]);

                    statements.Add(state);

                    floatingExpressions.RemoveRange(floatingExpressions.Count - 2, 2);

                }
                else if (
                    instuctionCase == Instruction.Increment || 
                    instuctionCase == Instruction.Decrement ||
                    instuctionCase == Instruction.Unknown24 ||
                    instuctionCase == Instruction.Unknown12) {

                    var state = new Statement(instuctionCase);

                    state.parametes.Add(floatingExpressions[^1]);
                    floatingExpressions.RemoveAt(floatingExpressions.Count - 1);

                    statements.Add(state);

                }
                else if (instuctionCase == Instruction.End) {

                    var state = new Statement(instuctionCase);

                    statements.Add(state);

                }

            }
            else {

                if (b > 127) {
                    floatingExpressions.Add(new Expression(b - 128, Operator.Literal));
                }
                else if (b == 2) {
                    floatingExpressions.Add(new Expression(128 + (code[i + 1]), Operator.Literal));
                    i += 2;
                    continue;
                }
                else {
                    throw new Exception("Unknown byte: " + b.ToString());
                }

            }

            i++;
        }

        if (floatingExpressions.Count > 0) {
            Console.WriteLine("floatingExpressions still has count");

            var state = new Statement(Instruction.None);

            state.parametes.Add(floatingExpressions[^1]);
            floatingExpressions.RemoveAt(floatingExpressions.Count - 1);

            statements.Add(state);

        }

        string LogExpression(Expression expression) {

            var total = "";

            total += expression.operationType.ToString() + "(";

            if (expression.value != null) {
                total += expression.value.ToString();
                total += ")";
            }
            else {

                if (doubleExpressionOperators.Contains(expression.operationType)) {
                    total += LogExpression(expression.nestedExpressions[0]) + ", ";
                    total += LogExpression(expression.nestedExpressions[1]) + ")";

                } else {

                    foreach (var nestedExpression in expression.nestedExpressions) {
                        total += LogExpression(nestedExpression);
                    }
                    total += ")";
                }



            }

            return total;

        }

        var message = "";

        foreach (var statement in statements) {

            message += statement.instruction.ToString() + "(";

            foreach (var par in statement.parametes) {
                message += LogExpression(par) + ", ";
            }

            if (statement.parametes.Count > 0) {

                message = message.Remove(message.Length - 1);
                message = message.Remove(message.Length - 1);

            }

            message += ")\n";

        }

        return message;

    }

    public void CompareActors(int id) {

        var message = "";

        foreach (var file in levels) {

            message += file.ToString() + ": \n";

            var actors = file.actors.Where(actor => {

                return actor.actorType == id;

            });

            foreach (var actor in actors) {

                var propertyCount = (Utils.BytesToInt(actor.rawFile.data.ToArray(), 4) - 28) / 2;

                var offset = 28;

                foreach (var i in Enumerable.Range(0, propertyCount)) {
                    message += Utils.BytesToShort(actor.rawFile.data.ToArray(), offset) + " ";
                    offset += 2;
                }

                message += "\n";

            }

        }

        Console.WriteLine(message);

    }


    public Dictionary<int, List<ActorsByRPNSRef>> CreateActorsRPNSRefFromFile(FCopLevel mFile) {

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


}