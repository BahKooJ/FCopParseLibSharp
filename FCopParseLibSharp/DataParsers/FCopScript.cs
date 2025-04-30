
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
        public List<byte> compiledBytes = new();

        public FCopScript(int offset, List<byte> compiledBytes) {
            this.id = offset;
            this.offset = offset;
            this.compiledBytes.AddRange(compiledBytes);
            Decompile(compiledBytes);
        }

        public Dictionary<int, ByteCodeOperationData> byteCodeData = new() {

            { 8, new ByteCodeOperationData(ByteCode.Jump, 0, 1, false) },
            { 16, new ByteCodeOperationData(ByteCode.GET_16, 1, 0, true) },
            { 20, new ByteCodeOperationData(ByteCode.ConditionalJump, 1, 1, false) },
            { 21, new ByteCodeOperationData(ByteCode.INCREMENT_16, 1, 0, false) },
            { 24, new ByteCodeOperationData(ByteCode.INCREMENT_19, 1, 0, false) }, 
            { 25, new ByteCodeOperationData(ByteCode.DECREMENT_16, 1, 0, false) },
            { 30, new ByteCodeOperationData(ByteCode.Sound, 2, 0, false) },
            { 35, new ByteCodeOperationData(ByteCode.GreaterThan, 2, 0, true) },
            { 37, new ByteCodeOperationData(ByteCode.LessThan, 2, 0, true) },
            { 44, new ByteCodeOperationData(ByteCode.And, 2, 0, true) },
            { 60, new ByteCodeOperationData(ByteCode.Spawn, 3, 0, false) },

        };

        // Takes the bytes and puts them into a very basic struct.
        // This struct contains the parameters that the instruction needs.
        // It has no idea the types, it does keep track of expressions.
        public List<ByteInstruction> Decompile(List<byte> code) {

            var instructions = new List<ByteInstruction>();
            var floatingExpressions = new List<ByteInstruction>();

            var i = 0;
            while (i < code.Count) {

                var b = code[i];

                if (b == 0) {
                    i++;
                    continue;
                }

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

                        instructions.Add(instruction);

                    }

                    // Some instructions, like 16 bit numbers or jump statements, uses the next byte in the byte array (or more).
                    // + 1 becuse it has to move over for the current byte to the next one.
                    if (instructionData.rightParameterCount != 0) {

                        foreach (var rightI in Enumerable.Range(0, instructionData.rightParameterCount)) {
                            instruction.parameters.Add(new ByteInstruction(ByteCode.Literal, code[i + 1 + rightI], new()));
                        }

                        i += instructionData.rightParameterCount + 1;
                        continue;

                    }

                }
                else {

                    if (b < 128) {
                        throw new Exception();
                    }

                    floatingExpressions.Add(new ByteInstruction(ByteCode.Literal, b, new()));

                }

                i++;

            }

            if (floatingExpressions.Count > 0) {

                throw new Exception();

            }

            return instructions;

        }

        public List<byte> Compile(int newOffset) {

            offset = newOffset;

            return compiledBytes;

        }

    }

    public enum ByteCode {
        Literal = -1,
        End = 0,
        BitFlip = 1,
        ShiftRight = 2,
        Bit16Literal = 3,
        Jump = 8,
        Unknown11 = 11,
        Unknown12 = 12,
        Unknown13 = 13,
        GET_16 = 16,
        GET_18 = 18,
        GET_19 = 19,
        ConditionalJump = 20,
        INCREMENT_16 = 21,
        INCREMENT_19 = 24,
        DECREMENT_16 = 25,
        DECREMENT_19 = 28,
        Set = 29,
        Sound = 30,
        Unknown31 = 31,
        SET_19 = 32,
        Equal = 33,
        NotEqual = 34,
        GreaterThan = 35,
        GreaterThanOrEqual = 36,
        LessThan = 37,
        LessThanOrEqual = 38,
        Add = 39,
        Subtract = 40,
        And = 44,
        Unknown47 = 47,
        Unkown51 = 51,
        SubtractSet = 52,
        Destroy = 56,
        Unknown57 = 57,
        Unknown59 = 59,
        Spawn = 60,
        SpawnAll = 61
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