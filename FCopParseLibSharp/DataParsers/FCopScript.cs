
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
        public List<ByteInstruction> assembly = new();

        public FCopScript(int offset, List<byte> compiledBytes) {
            this.id = offset;
            this.offset = offset;
            assembly = Decompile(offset, compiledBytes, out terminationOffset);
            this.compiledBytes = compiledBytes.GetRange(offset, terminationOffset - offset);
        }

        public Dictionary<int, ByteCodeOperationData> byteCodeData = new() {
            { 0, new ByteCodeOperationData(ByteCode.END, 0, 0, false) },
            { 1, new ByteCodeOperationData(ByteCode.BIT_FLIP, 0, 1, true) },
            { 2, new ByteCodeOperationData(ByteCode.BIT_SHIFT_RIGHT, 0, 1, true) },
            { 3, new ByteCodeOperationData(ByteCode.LITERAL_16, 0, 2, true) },
            { 8, new ByteCodeOperationData(ByteCode.JUMP, 0, 1, false) },
            { 11, new ByteCodeOperationData(ByteCode.BYTE11, 1, 0, true) },
            { 12, new ByteCodeOperationData(ByteCode.BYTE12, 1, 0, false) },
            { 13, new ByteCodeOperationData(ByteCode.BYTE13, 1, 0, false) },
            { 14, new ByteCodeOperationData(ByteCode.BYTE14, 1, 0, false) },
            { 15, new ByteCodeOperationData(ByteCode.BYTE15, 1, 0, true) },
            { 16, new ByteCodeOperationData(ByteCode.GET_16, 1, 0, true) },
            { 17, new ByteCodeOperationData(ByteCode.GET_17, 1, 0, true) },
            { 18, new ByteCodeOperationData(ByteCode.GET_18, 1, 0, true) },
            { 19, new ByteCodeOperationData(ByteCode.GET_19, 1, 0, true) },
            { 20, new ByteCodeOperationData(ByteCode.CONDITIONAL_JUMP, 1, 1, false) },
            { 21, new ByteCodeOperationData(ByteCode.INCREMENT_16, 1, 0, false) },
            { 24, new ByteCodeOperationData(ByteCode.INCREMENT_19, 1, 0, false) }, 
            { 25, new ByteCodeOperationData(ByteCode.DECREMENT_16, 1, 0, false) },
            { 28, new ByteCodeOperationData(ByteCode.DECREMENT_19, 1, 0, false) },
            { 29, new ByteCodeOperationData(ByteCode.SET_16, 2, 0, false) },
            { 30, new ByteCodeOperationData(ByteCode.Sound, 2, 0, false) },
            { 31, new ByteCodeOperationData(ByteCode.BYTE31, 2, 0, false) },
            { 32, new ByteCodeOperationData(ByteCode.SET_19, 2, 0, false) },
            { 33, new ByteCodeOperationData(ByteCode.EQUAL, 2, 0, true) },
            { 34, new ByteCodeOperationData(ByteCode.NOT_EQUAL, 2, 0, true) },
            { 35, new ByteCodeOperationData(ByteCode.GREATER_THAN, 2, 0, true) },
            { 36, new ByteCodeOperationData(ByteCode.GREATER_THAN_OR_EQUAL, 2, 0, true) },
            { 37, new ByteCodeOperationData(ByteCode.LESS_THAN, 2, 0, true) },
            { 38, new ByteCodeOperationData(ByteCode.LESS_THAN_OR_EQUAL, 2, 0, true) },
            { 39, new ByteCodeOperationData(ByteCode.ADD, 2, 0, true) },
            { 40, new ByteCodeOperationData(ByteCode.SUBTRACT, 2, 0, true) },
            { 44, new ByteCodeOperationData(ByteCode.AND, 2, 0, true) },
            { 47, new ByteCodeOperationData(ByteCode.BYTE47, 2, 0, true) },
            { 48, new ByteCodeOperationData(ByteCode.ADD_16_SET, 2, 0, false) },
            { 51, new ByteCodeOperationData(ByteCode.BYTE51, 2, 0, false) },
            { 52, new ByteCodeOperationData(ByteCode.SUB_16_SET, 2, 0, false) },
            { 56, new ByteCodeOperationData(ByteCode.Destroy, 3, 0, false) },
            { 57, new ByteCodeOperationData(ByteCode.BYTE57, 3, 0, false) },
            { 58, new ByteCodeOperationData(ByteCode.BYTE58, 3, 0, false) },
            { 59, new ByteCodeOperationData(ByteCode.BYTE59, 3, 0, false) },
            { 60, new ByteCodeOperationData(ByteCode.Spawn, 3, 0, false) },
            { 61, new ByteCodeOperationData(ByteCode.SpawnAll, 3, 0, false) },
            { 62, new ByteCodeOperationData(ByteCode.BYTE62, 3, 0, false) },

        };

        // Takes the bytes and puts them into a very basic struct.
        // This struct contains the parameters that the instruction needs.
        // It has no idea the types, it does keep track of expressions.
        public List<ByteInstruction> Decompile(int startingOffset, List<byte> code, out int terminationOffset) {

            var instructions = new List<ByteInstruction>();
            var floatingExpressions = new List<ByteInstruction>();

            var i = startingOffset;
            while (i < code.Count) {

                var b = code[i];

                // Key Byte found!
                if (Enum.IsDefined(typeof(ByteCode), (int)b)) {

                    var instructionData = byteCodeData[b];

                    var instruction = new ByteInstruction((ByteCode)b, b, new());

                    // Some overloads have less than the expected expressions.
                    // If it has less than expect it'll just give the instruction the floating expressions.
                    if (instructionData.parameterCount <= floatingExpressions.Count && instructionData.parameterCount != 0) {
                        var expressions = floatingExpressions.GetRange(floatingExpressions.Count - instructionData.parameterCount, instructionData.parameterCount);
                        instruction.parameters = expressions;
                        floatingExpressions.RemoveRange(floatingExpressions.Count - instructionData.parameterCount, instructionData.parameterCount);
                    }
                    else if (instructionData.parameterCount != 0) {
                        instruction.parameters = new(floatingExpressions);
                        floatingExpressions.Clear();
                    }

                    // If it is an expression, instruction needs to be stored for the next statement.
                    if (instructionData.isExpression) {

                        floatingExpressions.Add(instruction);

                    }
                    else {

                        // The expressions would've already been used if they were needed.
                        if (floatingExpressions.Count > 0) {

                            foreach (var floatingExpression in floatingExpressions) {

                                instructions.Add(new ByteInstruction(ByteCode.NONE, 0, new() { floatingExpression }));

                            }

                        }

                        floatingExpressions.Clear();

                        instructions.Add(instruction);

                    }

                    // Some instructions, like 16 bit numbers or jump statements, uses the next byte in the byte array (or more).
                    // + 1 becuse it has to move over for the current byte to the next one.
                    if (instructionData.rightParameterCount != 0) {

                        foreach (var rightI in Enumerable.Range(0, instructionData.rightParameterCount)) {
                            instruction.parameters.Add(new ByteInstruction(ByteCode.LITERAL, code[i + 1 + rightI], new()));
                        }

                        i += instructionData.rightParameterCount + 1;
                        continue;

                    }

                    if (instruction.byteCode == ByteCode.END) {
                        break;
                    }

                }
                else {

                    if (b < 128) {
                        throw new Exception();
                    }

                    floatingExpressions.Add(new ByteInstruction(ByteCode.LITERAL, b, new()));

                }

                i++;

            }

            if (floatingExpressions.Count > 0) {

                foreach (var floatingExpression in floatingExpressions) {

                    instructions.Add(new ByteInstruction(ByteCode.NONE, 0, new() { floatingExpression }));

                }

            }
            terminationOffset = i + 1;
            return instructions;

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

    public struct ByteCodeOperationData {

        public ByteCode byteCode;
        public int parameterCount;
        public int rightParameterCount;
        public bool isExpression;

        public ByteCodeOperationData(ByteCode byteCode, int parameterCount, int rightParameterCount, bool isExpression) {
            this.byteCode = byteCode;
            this.parameterCount = parameterCount;
            this.rightParameterCount = rightParameterCount;
            this.isExpression = isExpression;
        }

    }

    public struct ByteInstruction {

        public ByteCode byteCode;
        public byte value;
        public List<ByteInstruction> parameters;

        public ByteInstruction(ByteCode byteCode, byte value, List<ByteInstruction> parameters) {
            this.byteCode = byteCode;
            this.value = value;
            this.parameters = parameters;
        }

    }



}