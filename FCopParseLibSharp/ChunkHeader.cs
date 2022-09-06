
//Objects for storing useful data regarding chunk headers.
record ChunkHeader(int index, string fourCCDeclaration, int chunkSize, string fourCCType = null, FileHeader fileHeader = null, string subFileName = null);

record FileHeader(int startNumber, string fourCCData, int dataID, int dataSize, byte[] actData);
