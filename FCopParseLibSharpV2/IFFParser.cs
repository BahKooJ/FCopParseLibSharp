
// This object is what indexes the IFF files determines where chunks are located.
// It stores chunk information with the ChunkHeader object.
using FCopParseLibSharpV2.Extentions;
using FCopParseLibSharpV2.Models.Chunk;
using FCopParseLibSharpV2.Service;

class IFFParser
{
  // consants for important fourCCs.
  // Other fourCCs can be ignored as they're just a description of what the item is.
  // However it is import to test for these specifically to know how to parse the chunk.
  static class FourCC
  {
    public const string CTRL = "CTRL";
    public const string SHOC = "SHOC";
    public const string FILL = "FILL";
    public const string SHDR = "SHDR";
    public const string SDAT = "SDAT";
    public const string SWVR = "SWVR";
    public const string MSIC = "MSIC";
  }

  // This stores all the offsets or index of chunks as well as useful information regarding them with the ChunkHeader object.
  public List<Chunk> chunks = new List<Chunk>();

  public IFFParser(byte[] bytes)
  {
    Init(bytes);
  }

  // Grabs all the files/data and coverts them into their own files,
  // separating the data and chuncks allowing for other programs to parse the data freely.
  // Returns the IFFFileManage object store the individual files.
  public IFFFileManager Parse(byte[] bytes)
  {
    var fileMananger = new IFFFileManager();

    IFFDataFile lastFileFound = null;
    int dataChunksToAdd = 0;
    string lastFileNameFound = string.Empty;

    foreach (var chunk in chunks)
    {
      switch (chunk)
      {
        case SWVR:
          lastFileNameFound = ((SWVR)chunk).FileName;
          break;

        //What if lastFileNameFound is not about this MSIC chunk but some other file.
        //This logic works only if the chunks are ordered, what I'm saying is that this can cause problems.
        //Also you're not resetting lastFileNameFound like you do with SDAT. So this can cause you to apply the same lastFileNameFound
        //to multiple files found later.
        case MSIC:
          var content = bytes.CopyOfRange(chunk.Index + chunk.HeaderLenght, chunk.Index + chunk.Size);
          fileMananger.music = new Music(content, lastFileNameFound);
          break;
      }

      switch (chunk.Type)
      {
        case FourCC.SHDR:
          if (lastFileFound == null && chunk.subChunk != null)
          {
            var shdrFile = (SHDR)chunk.subChunk;

            //Prepare file for next sdat found.
            //Could be possible for SHDR chunks to have a SDAT subchunk?
            lastFileFound = new IFFDataFile(shdrFile.FourCCData, shdrFile.DataID, shdrFile.ActData);
            dataChunksToAdd = DataChunksBySize(shdrFile.DataSize);

          }
          break;

        //What if SDAT is not data about the last SHDR found but about some other chunk?
        //This logic works only if the chunks are ordered, what I'm saying is that this can cause problems.
        case FourCC.SDAT:
          if (lastFileFound != null && dataChunksToAdd != 0)
          {
            //dataChunksToAdd can be = 1? The logic is not clear at first sight. I suggest to explain with comments why this is the case.
            lastFileFound.data = bytes.CopyOfRange(chunk.Index + chunk.HeaderLenght, chunk.Index + chunk.Size);
            dataChunksToAdd--;

            if (dataChunksToAdd == 0)
            {
              if (lastFileNameFound != null) //lastFileNameFound is never set to null. As soon as you find a MSIC chunk, all other SDAT found after that will always enter this if.
              {
                if (!fileMananger.subFiles.ContainsKey(lastFileNameFound))
                {
                  fileMananger.subFiles.Add(lastFileNameFound, new List<IFFDataFile>()); //What if MSIC was found and lastFileFound was not reset
                }

                fileMananger.subFiles[lastFileNameFound].Add(lastFileFound);
                lastFileFound = null;
              }
              else
              {
                fileMananger.files.Add(lastFileFound);
                lastFileFound = null;
              }
            }
          }
          break;
      }
    }

    return fileMananger;
  }

  void Init(byte[] bytes)
  {
    int offset = 0;
    while (offset < bytes.Length)
    {
      var fourCC = bytes.ToStringReverse(offset, 4);

      Chunk chunk;
      switch (fourCC)
      {
        case FourCC.SHOC: chunk = ChunkService.GetSHOC(bytes, offset); break;
        case FourCC.SWVR: chunk = ChunkService.GetSWVR(bytes, offset); break;
        default: chunk = ChunkService.GetGeneric(bytes, offset); break;
      }

      chunks.Add(chunk);
      offset += chunk.Size;
    }
  }

  // ---Utils---
  //Is 20 the header size? If yes, you've got a problem when using DataChunksBySize with music files which have a lenght of 28.
  static int DataChunksBySize(int size)
  {
    var total = size / (4096 - 20);
    if (size % (4096 - 20) != 0)
    {
      total++;
    }
    return total;
  }
}