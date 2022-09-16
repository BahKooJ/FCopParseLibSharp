
// This object will act as the in-between from the IFF file and any parsers of game data/files.
// This object is planned to convert the game files back to the IFF file format for Future Cop to read.
public class IFFFileManager
{
  // Game data that are separated and turn into individual files.
  public List<IFFDataFile> files = new List<IFFDataFile>();

  // Sub folders/files inside the IFF file that are separated.
  public Dictionary<string, List<IFFDataFile>> subFiles = new Dictionary<string, List<IFFDataFile>>();

  // Nothing but the music, the key is the name of the song.
  // Every mission file can have only one music
  public Music music = new Music();
}

public class Music
{
  public byte[] Content;
  public string Name;

  public Music() { }

  public Music(byte[] content, string name)
  {
    Content = content;
    Name = name;
  }
}

// Object for storing important meta data to a game file.
public class IFFDataFile
{
  public byte[] data;
  public string dataFourCC;
  public int dataID;
  public byte[] additionalData;

  public IFFDataFile(string dataFourCC, int dataID, byte[] additionalData)
  {
    this.dataFourCC = dataFourCC;
    this.dataID = dataID;
    this.additionalData = additionalData;
  }
}
