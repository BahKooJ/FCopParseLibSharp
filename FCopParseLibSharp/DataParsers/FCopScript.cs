﻿
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCopParser {

    public class FCopScriptingProject {

        public FCopRPNS rpns;
        public FCopFunctionParser functionParser;
        public int emptyOffset;

        public FCopScriptingProject(FCopRPNS rpns, FCopFunctionParser functionParser) {
            this.rpns = rpns;
            this.functionParser = functionParser;
            emptyOffset = rpns.code.Last().Key;
        }

        public List<IFFDataFile> Compile() {

            List<IFFDataFile> total = new() {
                rpns.Compile(),
                functionParser.Compile()
            };

            emptyOffset = rpns.code.Last().Value.offset;

            return total;
        }

        public void ResetIDAndOffsets() {
            rpns.ResetKeys();
        }

        // Added a debug code I forgot about and now mission files are messed up
        public void DebugScriptDupeFix() {

            var first = rpns.code.First().Value;
            var copy = new Dictionary<int, FCopScript>(rpns.code.Skip(1));
            foreach (var code in copy) {

                if (first.compiledBytes.SequenceEqual(code.Value.compiledBytes)) {
                    rpns.code.Remove(code.Key);
                }

            }

        }

    }

    public class FCopScript {

        public bool failed = false;

        public string name = "";
        public int id;

        public int offset;
        public int terminationOffset;
        public List<byte> compiledBytes = new();
        public List<ScriptNode> code = new();

        public FCopScript(int offset, List<byte> compiledBytes) {
            this.id = offset;
            this.offset = offset;
            code = Decompile(offset, compiledBytes, out terminationOffset);
            this.compiledBytes = compiledBytes.GetRange(offset, terminationOffset - offset);

        }

        public static Dictionary<ByteCode, int> maxArgumentsByCode = new() {

            { ByteCode.BYTE11, 1 },
            { ByteCode.BYTE12, 1 },
            { ByteCode.BYTE13, 1 },
            { ByteCode.BYTE14, 1 },
            { ByteCode.BYTE15, 1 },
            { ByteCode.GET_16, 1 },
            { ByteCode.GET_17, 1 },
            { ByteCode.GET_18, 1 },
            { ByteCode.GET_19, 1 },
            { ByteCode.CONDITIONAL_JUMP, 1 },
            { ByteCode.INCREMENT_16, 1 },
            { ByteCode.INCREMENT_19, 1 },
            { ByteCode.DECREMENT_16, 1 },
            { ByteCode.DECREMENT_19, 1 },
            { ByteCode.SET_16, 2 },
            { ByteCode.Sound, 2 },
            { ByteCode.BYTE31, 2 },
            { ByteCode.SET_19, 2 },
            { ByteCode.EQUAL, 2 },
            { ByteCode.NOT_EQUAL, 2 },
            { ByteCode.GREATER_THAN, 2 },
            { ByteCode.GREATER_THAN_OR_EQUAL, 2 },
            { ByteCode.LESS_THAN, 2 },
            { ByteCode.LESS_THAN_OR_EQUAL, 2 },
            { ByteCode.ADD, 2 },
            { ByteCode.SUBTRACT, 2 },
            { ByteCode.AND, 2 },
            { ByteCode.BYTE47, 2 },
            { ByteCode.ADD_16_SET, 2 },
            { ByteCode.BYTE51, 2 },
            { ByteCode.SUB_16_SET, 2 },
            { ByteCode.Destroy, 3 },
            { ByteCode.BYTE57, 3 },
            { ByteCode.BYTE58, 3 },
            { ByteCode.BYTE59, 3 },
            { ByteCode.Spawn, 3 },
            { ByteCode.SpawnAll, 3 },
            { ByteCode.BYTE62, 3 }

        };

        public static Dictionary<ScriptDataKey, ScriptOperationData> scriptNodeData = new() {

            { new ScriptDataKey(ByteCode.BYTE11, 1), 
                new ScriptOperationData("11", ScriptDataType.Int, false, new() { new ScriptParameter("Par0", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE12, 1), 
                new ScriptOperationData("12", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE13, 1), 
                new ScriptOperationData("13", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE14, 1), 
                new ScriptOperationData("14", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE15, 1), 
                new ScriptOperationData("Get 15", ScriptDataType.Int, false, new() { new ScriptParameter("ID", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.GET_16, 1), 
                new ScriptOperationData("Get 16", ScriptDataType.Int, false, new() { new ScriptParameter("ID", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.GET_17, 1), 
                new ScriptOperationData("Get 17", ScriptDataType.Int, false, new() { new ScriptParameter("ID", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.GET_18, 1), 
                new ScriptOperationData("Get 18", ScriptDataType.Int, false, new() { new ScriptParameter("ID", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.GET_19, 1),
                new ScriptOperationData("Get 19", ScriptDataType.Int, false, new() { new ScriptParameter("ID", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.CONDITIONAL_JUMP, 1), 
                new ScriptOperationData("If", ScriptDataType.Void, false, new() { new ScriptParameter("Condition", ScriptDataType.Bool) }) 
            },
            { new ScriptDataKey(ByteCode.CONDITIONAL_JUMP, 0),
                new ScriptOperationData("20(0)", ScriptDataType.Void, false, new())
            },
            { new ScriptDataKey(ByteCode.INCREMENT_16, 1), 
                new ScriptOperationData("++(16)", ScriptDataType.Void, true, new() { new ScriptParameter("Left", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.INCREMENT_19, 1), 
                new ScriptOperationData("++(19)", ScriptDataType.Void, true, new() { new ScriptParameter("Left", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.DECREMENT_16, 1), 
                new ScriptOperationData("--(16)", ScriptDataType.Void, true, new() { new ScriptParameter("Left", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.DECREMENT_19, 1), 
                new ScriptOperationData("--(19)", ScriptDataType.Void, true, new() { new ScriptParameter("Left", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.SET_16, 2), 
                new ScriptOperationData("=(16)", ScriptDataType.Void, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.Sound, 2), 
                new ScriptOperationData("30", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE31, 2), 
                new ScriptOperationData("31", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.SET_19, 2), 
                new ScriptOperationData("=(19)", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.EQUAL, 2), 
                new ScriptOperationData("==", ScriptDataType.Bool, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.EQUAL, 1), 
                new ScriptOperationData("==(1)", ScriptDataType.Bool, false, new() { new ScriptParameter("Par0", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.NOT_EQUAL, 2), 
                new ScriptOperationData("!=", ScriptDataType.Bool, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.GREATER_THAN, 2), 
                new ScriptOperationData(">", ScriptDataType.Bool, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.GREATER_THAN_OR_EQUAL, 2), 
                new ScriptOperationData(">=", ScriptDataType.Bool, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.LESS_THAN, 2), 
                new ScriptOperationData("<", ScriptDataType.Bool, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.LESS_THAN_OR_EQUAL, 2), 
                new ScriptOperationData("<=", ScriptDataType.Bool, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.ADD, 2), 
                new ScriptOperationData("+", ScriptDataType.Int, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.SUBTRACT, 2), 
                new ScriptOperationData("-", ScriptDataType.Int, true, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.AND, 2), 
                new ScriptOperationData("&&", ScriptDataType.Bool, true, new() { new ScriptParameter("Left", ScriptDataType.Bool), new ScriptParameter("Right", ScriptDataType.Bool) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE47, 2), 
                new ScriptOperationData("47", ScriptDataType.Int, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.ADD_16_SET, 2),
                new ScriptOperationData("+=(16)", ScriptDataType.Void, false, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE51, 2), 
                new ScriptOperationData("51", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.SUB_16_SET, 2), 
                new ScriptOperationData("-=(16)", ScriptDataType.Void, false, new() { new ScriptParameter("Left", ScriptDataType.Int), new ScriptParameter("Right", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.Destroy, 3), 
                new ScriptOperationData("56", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int), new ScriptParameter("Par2", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.Destroy, 2), 
                new ScriptOperationData("56(2)", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE57, 3), 
                new ScriptOperationData("57", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int), new ScriptParameter("Par2", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE58, 3), 
                new ScriptOperationData("58", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int), new ScriptParameter("Par2", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE59, 3), 
                new ScriptOperationData("59", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int), new ScriptParameter("Par2", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.Spawn, 3), 
                new ScriptOperationData("60", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int), new ScriptParameter("Par2", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.SpawnAll, 3), 
                new ScriptOperationData("61", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int), new ScriptParameter("Par2", ScriptDataType.Int) }) 
            },
            { new ScriptDataKey(ByteCode.BYTE62, 3), 
                new ScriptOperationData("62", ScriptDataType.Void, false, new() { new ScriptParameter("Par0", ScriptDataType.Int), new ScriptParameter("Par1", ScriptDataType.Int), new ScriptParameter("Par2", ScriptDataType.Int) }) 
            },

        };


        public List<ScriptNode> Decompile(int startingOffset, List<byte> code, out int terminationOffset) {

            var statements = new List<ScriptNode>();
            var floatingExpressions = new List<ScriptNode>();
            List<(ScriptNode node, int byteCount)> nodesToNest = new();

            void AddScriptingNode(ScriptNode node, int bytesProcessed, bool isExpression) {

                if (nodesToNest.Count != 0) {

                    var nestingNode = nodesToNest.Last();

                    nestingNode.byteCount -= bytesProcessed;

                    if (!isExpression) {

                        nestingNode.node.nestedNodes.Add(node);

                    }

                    if (nestingNode.byteCount == 0) {

                        // Expression wasn't used and now outside the jump statement.
                        if (isExpression) {

                            nestingNode.node.nestedNodes.Add(node);

                            // Todo: Ternary Check

                        }

                    }
                    else {

                        if (isExpression) {
                            floatingExpressions.Add(node);
                        }

                    }

                    foreach (var i in Enumerable.Range(0, nodesToNest.Count)) {

                        var n = nodesToNest[i];
                        n.byteCount -= bytesProcessed;
                        nodesToNest[i] = n;

                        // Data is misaligned
                        if (n.byteCount < 0) {
                            throw new Exception();
                        }

                    }

                    nodesToNest.RemoveAll(n => n.byteCount == 0);

                }
                else {

                    if (isExpression) {
                        floatingExpressions.Add(node);
                    }
                    else {
                        statements.Add(node);
                    }

                }

            }

            var i = startingOffset;
            while (i < code.Count) {

                var b = code[i];

                // Key Byte found!
                if (Enum.IsDefined(typeof(ByteCode), (int)b)) {

                    var byteCode = (ByteCode)b;

                    if (byteCode == ByteCode.BIT_FLIP) {

                        int value = code[i + 1];

                        value ^= -1;

                        AddScriptingNode(new LiteralNode(value), 2, true);

                        i += 2;
                        continue;

                    }
                    if (byteCode == ByteCode.BIT_SHIFT_RIGHT) {

                        int value = code[i + 1];

                        // I can't figure this out so adding 128 does the same thing I guess.
                        value += 128;

                        AddScriptingNode(new LiteralNode(value), 2, true);

                        i += 2;
                        continue;

                    }
                    if (byteCode == ByteCode.LITERAL_16) {

                        // Big Endian
                        int value = BitConverter.ToInt16(new byte[] { code[i + 2], code[i + 1] });

                        AddScriptingNode(new LiteralNode(value), 3, true);

                        i += 3;
                        continue;

                    }
                    if (byteCode == ByteCode.JUMP) {

                        foreach (var expression in floatingExpressions) {

                            // Why 0?
                            // The expression was already counted for, because nothing used it and we hit a jump statement.
                            // This is pretty much always going to be a ternary.
                            AddScriptingNode(expression, 0, false);

                        }

                        floatingExpressions.Clear();

                        var jumpNode = new ScriptNode(ByteCode.JUMP, "Else", ScriptDataType.Void, failed, new());

                        AddScriptingNode(jumpNode, 2, false);

                        nodesToNest.Add((jumpNode, code[i + 1] - 1));

                        i += 2;
                        continue;

                    }
                    if (byteCode == ByteCode.END) {

                        i++;
                        break;

                    }

                    var maxParaCount = maxArgumentsByCode[byteCode];
                    var paraCount = floatingExpressions.Count >= maxParaCount ? maxParaCount : floatingExpressions.Count;

                    var scriptData = scriptNodeData[new ScriptDataKey(byteCode, paraCount)];

                    var node = new ScriptNode(byteCode, scriptData.name, scriptData.defaultReturnType, scriptData.isOperator, new (scriptData.parameterData));

                    node.parameters = floatingExpressions.GetRange(floatingExpressions.Count - paraCount, paraCount);
                    floatingExpressions.RemoveRange(floatingExpressions.Count - paraCount, paraCount);

                    if (byteCode == ByteCode.CONDITIONAL_JUMP) {

                        AddScriptingNode(node, 2, false);

                        nodesToNest.Add((node, code[i + 1] - 1));

                        i += 2;
                        continue;

                    }

                    AddScriptingNode(node, 1, node.defaultReturnType != ScriptDataType.Void);

                }
                else {

                    if (b < 128) {
                        throw new Exception();
                    }

                    AddScriptingNode(new LiteralNode(b), 1, true);

                }

                i++;

            }

            if (floatingExpressions.Count > 0) {

                foreach (var floatingExpression in floatingExpressions) {

                    statements.Add(floatingExpression);

                }

            }
            terminationOffset = i;
            return statements;

        }



        public List<byte> Compile(int newOffset) {

            offset = newOffset;

            return compiledBytes;

        }

    }

    public enum ByteCode {
        NONE = -2,
        LITERAL = -1,
        END = 0,
        BIT_FLIP = 1,
        BIT_SHIFT_RIGHT = 2,
        LITERAL_16 = 3,
        JUMP = 8,
        BYTE11 = 11,
        BYTE12 = 12,
        BYTE13 = 13,
        BYTE14 = 14,
        BYTE15 = 15,
        GET_16 = 16,
        GET_17 = 17,
        GET_18 = 18,
        GET_19 = 19,
        CONDITIONAL_JUMP = 20,
        INCREMENT_16 = 21,
        INCREMENT_19 = 24,
        DECREMENT_16 = 25,
        DECREMENT_19 = 28,
        SET_16 = 29,
        Sound = 30,
        BYTE31 = 31,
        SET_19 = 32,
        EQUAL = 33,
        NOT_EQUAL = 34,
        GREATER_THAN = 35,
        GREATER_THAN_OR_EQUAL = 36,
        LESS_THAN = 37,
        LESS_THAN_OR_EQUAL = 38,
        ADD = 39,
        SUBTRACT = 40,
        AND = 44,
        BYTE47 = 47,
        ADD_16_SET = 48,
        BYTE51 = 51,
        SUB_16_SET = 52,
        Destroy = 56,
        BYTE57 = 57,
        BYTE58 = 58,
        BYTE59 = 59,
        Spawn = 60,
        SpawnAll = 61,
        BYTE62 = 62,
    }

    public enum ScriptDataType {
        Void,
        Any,
        Int,
        Bool,
        Asset
    }

    public struct ScriptDataKey {

        public ByteCode byteCode;
        public int parameterCount;

        public ScriptDataKey(ByteCode byteCode, int parameterCount) {
            this.byteCode = byteCode;
            this.parameterCount = parameterCount;
        }

    }

    public struct ScriptParameter {

        public string name;
        public ScriptDataType dataType;

        public ScriptParameter(string name, ScriptDataType dataType) {
            this.name = name;
            this.dataType = dataType;
        }

    }

    public struct ScriptOperationData {

        public string name;
        public ScriptDataType defaultReturnType;
        public bool isOperator;
        public List<ScriptParameter> parameterData;

        public ScriptOperationData(string name, ScriptDataType defaultReturnType, bool isOperator, List<ScriptParameter> parameterData) {
            this.name = name;
            this.defaultReturnType = defaultReturnType;
            this.isOperator = isOperator;
            this.parameterData = parameterData;
        }

    }

    public class ScriptNode {

        public ByteCode byteCode;
        public string name;
        public ScriptDataType defaultReturnType;
        public bool isOperator;
        public List<ScriptParameter> parameterData;

        public List<ScriptNode> parameters = new();
        public List<ScriptNode> nestedNodes = new();

        public ScriptNode(ByteCode byteCode, string name, ScriptDataType defaultReturnType, bool isOperator, List<ScriptParameter> parameterData) {
            this.byteCode = byteCode;
            this.name = name;
            this.defaultReturnType = defaultReturnType;
            this.isOperator = isOperator;
            this.parameterData = parameterData;
        }

        public ScriptNode(ByteCode byteCode, string name, ScriptDataType defaultReturnType, bool isOperator, List<ScriptParameter> parameterData, List<ScriptNode> parameters, List<ScriptNode> nestedNodes) : this(byteCode, name, defaultReturnType, isOperator, parameterData) {
            this.parameters = parameters;
            this.nestedNodes = nestedNodes;
        }

        public virtual ScriptDataType ReturnType() {
            return defaultReturnType;
        }

    }

    public class LiteralNode : ScriptNode {

        public int value;

        public LiteralNode(int value): base(ByteCode.LITERAL, "", ScriptDataType.Int, false, new()) {
            this.value = value;
        }

    }

}