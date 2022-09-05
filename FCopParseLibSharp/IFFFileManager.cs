
class IFFFileManager {

    public List<IFFDataFile> files = new List<IFFDataFile> ();

    public Dictionary<string, List<IFFDataFile>> subFiles = new Dictionary<string, List<IFFDataFile>>();

    public KeyValuePair<string, List<byte>>? music = null;

}

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
