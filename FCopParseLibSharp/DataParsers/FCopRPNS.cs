

using System.Collections;

namespace FCopParser {

    public class FCopRPNS {

        public List<List<byte>> code = new();

        public FCopRPNS(IFFDataFile rawFile) {

            var currentLine = new List<byte>();

            foreach (var b in rawFile.data) {

                currentLine.Add(b);

                if (b == 0) {

                    code.Add(new(currentLine));

                    currentLine.Clear();

                }

            }

        }

    }

}