﻿
// This object will act as the in-between from the IFF file and any parsers of game data/files.
// This object is planned to convert the game files back to the IFF file format for Future Cop to read.
class IFFFileManager {

    // Game data that are separated and turn into individual files.
    public List<IFFDataFile> files = new List<IFFDataFile> ();

    // Sub folders/files inside the IFF file that are separated.
    public Dictionary<string, List<IFFDataFile>> subFiles = new Dictionary<string, List<IFFDataFile>>();

    // Nothing but the music, the key is the name of the song.
    public KeyValuePair<string, List<byte>>? music = null;

    public List<IFFDataFile> GrabAllFiles(Func<IFFDataFile, bool> where) {

        var total = new List<IFFDataFile>();

        foreach (IFFDataFile file in files) {

            if (where(file)) {
                total.Add(file);
            }

        }

        return total;

    }

}

// Object for storing important meta data to a game file.
class IFFDataFile {

    public List<byte> data;
    public string dataFourCC;
    public int dataID;
    public List<byte> additionalData;

    public IFFDataFile(List<byte> data, string dataFourCC, int dataID, List<byte> additionalData) {
        this.data = data;
        this.dataFourCC = dataFourCC;
        this.dataID = dataID;
        this.additionalData = additionalData;
    }


}