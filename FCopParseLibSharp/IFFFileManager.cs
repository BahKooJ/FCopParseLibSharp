
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FCopParser {

    // This object will act as the in-between from the IFF file and any parsers of game data/files.
    // This object is planned to convert the game files back to the IFF file format for Future Cop to read.
    public class IFFFileManager {

        // Game data that are separated and turn into individual files.
        public List<IFFDataFile> files = new List<IFFDataFile>();

        // Sub folders/files inside the IFF file that are separated.
        public Dictionary<byte[], List<IFFDataFile>> subFiles = new Dictionary<byte[], List<IFFDataFile>>();

        // Nothing but the music, the key is the name of the song.
        public KeyValuePair<byte[], List<byte>>? music = null;

        public void ExportFile(string fourCC, int id, string dir) {

            IFFDataFile file = files.First(file => { 
                return file.dataID == id && file.dataFourCC == fourCC;
            });

            File.WriteAllBytes(dir, file.data.ToArray());

        }

        public void ExportAll(string fourCC, string dir = "") {

            var files = this.files.Where(file => {
                return file.dataFourCC == fourCC;
            });

            foreach (var file in files) {

                File.WriteAllBytes(dir + file.dataID, file.data.ToArray());

            }

        }

        public void ImportFile(string fourCC, int id, string dir) {

            IFFDataFile file = files.First(file => {
                return file.dataID == id && file.dataFourCC == fourCC;
            });

            file.data = File.ReadAllBytes(dir).ToList();
            file.modified = true;

        }

        public void RemoveFile(string fourCC, int id) {

            files.RemoveAll(file => {
                return file.dataID == id && file.dataFourCC == fourCC;
            });

        }

        public void CreateFileList(string path) {

            var total = "";

            foreach (var file in files) {

                total += file.dataFourCC + " " +
                    file.startNumber + " " +
                    file.dataID + " " + 
                    file.data.Count + " Additional Data: ";

                foreach (var data in file.additionalData) {
                    total += data + " ";
                }

                total += "\n";

            }

            foreach (var subFile in subFiles) {

                total += "Sub File: ";

                foreach (var c in subFile.Key) {

                    if (c != 0) {
                        total += Encoding.Default.GetString(new byte[] { c });
                    } else {
                        break;
                    }

                }

                total += "\n";

                foreach (var file in subFile.Value) {

                    total += file.dataFourCC + " " +
                        file.startNumber + " " +
                        file.data.Count + " " +
                        file.dataID + " Additional Data: ";

                    foreach (var data in file.additionalData) {
                        total += data + " ";
                    }

                    total += "\n";

                }

                total += "\n";

            }

            if (music == null) {
                File.WriteAllText(path, total);
                return;
            }

            total += "Music: ";

            foreach (var c in music.Value.Key) {

                if (c != 0) {
                    total += Encoding.Default.GetString(new byte[] { c });
                }
                else {
                    break;
                }

            }

            File.WriteAllText(path, total);

        }

        public IFFDataFile GetFile(string fourCC, int id) {
            return files.First(file => {
                return file.dataID == id && file.dataFourCC == fourCC;
            });
        }

    }

    // Object for storing important meta data to a game file.
    public class IFFDataFile {

        public List<byte> data;
        public int startNumber;
        public string dataFourCC;
        public int dataID;
        public List<byte> additionalData;
        public bool modified = false;
        public bool ignore = false;

        public IFFDataFile(int startNumber, List<byte> data, string dataFourCC, int dataID, List<byte> additionalData) {
            this.startNumber = startNumber;
            this.data = data;
            this.dataFourCC = dataFourCC;
            this.dataID = dataID;
            this.additionalData = additionalData;
        }

        public IFFDataFile Clone(int newID) {
            return new IFFDataFile(startNumber, data, dataFourCC, newID, additionalData);
        }

    }

}