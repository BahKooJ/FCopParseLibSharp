
using System.Collections;

// =WIP=
class FCopLevel {

    public List<FCopLevelSectionParser> sections = new List<FCopLevelSectionParser> ();

    public FCopLevel(IFFFileManager fileManager) {

        var rawFiles = fileManager.files.Where( file => {

            return file.dataFourCC == "Ctil";

        }).ToList();

        foreach (var rawFile in rawFiles) {
            sections.Add(new FCopLevelSectionParser(rawFile));
        }

    }

}

class FCopLevelSectionParser {

    const int textureCordCountOffset = 10;

    const int heightMapOffset = 12;
    const int heightMapLength = 867;

    const int renderDistanceOffset = 880;
    const int rednerDistanceLength = 90;

    const int tileCountOffset = 970;

    const int thirdSectionOffset = 972;
    const int thirdSectionLength = 512;

    const int tileArrayOffset = 1488;

    static List<byte> fourCC = new List<byte> () { 116, 99, 101, 83 };


    IFFDataFile rawFile;

    public short textureCordCount;
    public short tileCount;


    public List<HeightPoint3> heightPoints = new List<HeightPoint3>();
    public List<ThirdSectionBitfield> thirdSectionBitfields = new List<ThirdSectionBitfield>();
    public List<Tile> tiles = new List<Tile>();
    public List<TextureCoordinate> textureCoordinates = new List<TextureCoordinate>();
    public List<TileGraphics> tileGraphics = new List<TileGraphics>();


    int offset = 0;

    public FCopLevelSectionParser(IFFDataFile rawFile) {
        this.rawFile = rawFile;

        textureCordCount = Utils.BytesToShort(rawFile.data.ToArray(), textureCordCountOffset);

        ParseHeightPoints();

        tileCount = Utils.BytesToShort(rawFile.data.ToArray(), tileCountOffset);

        ParseThirdSection();
        ParseTiles();
        ParseTextures();
        ParseTileGraphics();

    }

    public void Compile() {

        List<byte> compiledFile = new List<byte> ();

        foreach (HeightPoint3 heightPoint3 in heightPoints) {

            compiledFile.Add(heightPoint3.height1);
            compiledFile.Add(heightPoint3.height2);
            compiledFile.Add(heightPoint3.height3);

        }

        compiledFile.Add(0);

        compiledFile.AddRange(
            rawFile.data.GetRange(renderDistanceOffset, rednerDistanceLength)
            );

        compiledFile.AddRange(BitConverter.GetBytes(tileCount));

        foreach (ThirdSectionBitfield bitfield in thirdSectionBitfields) {

            var bits64 = new BitArray(new int[] { bitfield.number1, bitfield.number2 });

            var bits16 = new BitArray(16);

            bits16[0] = bits64[0];
            bits16[1] = bits64[1];
            bits16[2] = bits64[2];
            bits16[4] = bits64[3];
            bits16[4] = bits64[4];
            bits16[5] = bits64[5];
            bits16[6] = bits64[32];
            bits16[7] = bits64[33];
            bits16[8] = bits64[34];
            bits16[9] = bits64[35];
            bits16[10] = bits64[36];
            bits16[11] = bits64[37];
            bits16[12] = bits64[38];
            bits16[13] = bits64[39];
            bits16[14] = bits64[40];
            bits16[15] = bits64[41];

            compiledFile.AddRange(Utils.BitArrayToByteArray(bits16));

        }

        compiledFile.AddRange(rawFile.data.GetRange(1484,4));

        foreach (Tile tile in tiles) {
            compiledFile.AddRange(tile.data);
        }

        foreach (TextureCoordinate texture in textureCoordinates) {
            compiledFile.AddRange(BitConverter.GetBytes((short)texture.topLeftIndex));
            compiledFile.AddRange(BitConverter.GetBytes((short)texture.topRightIndex));
            compiledFile.AddRange(BitConverter.GetBytes((short)texture.bottomRightIndex));
            compiledFile.AddRange(BitConverter.GetBytes((short)texture.bottomLeftIndex));

        }

        foreach (TileGraphics graphic in tileGraphics) {
            compiledFile.AddRange(graphic.data);
        }

        var header = new List<byte>();

        header.AddRange(fourCC);

        header.AddRange(BitConverter.GetBytes(12 + compiledFile.Count()));

        header.AddRange(rawFile.data.GetRange(8,2));

        header.AddRange(BitConverter.GetBytes(textureCordCount));

        header.AddRange(compiledFile);

        rawFile.data = header;
        rawFile.modified = true;

    }



    void ParseHeightPoints() {

        var bytes = rawFile.data.GetRange(heightMapOffset, heightMapLength);

        var pointCount = 0;

        List<byte> heights = new List<byte>();

        foreach (byte b in bytes) {

            heights.Add(b);

            pointCount++;

            if (pointCount == 3) {
                heightPoints.Add(new HeightPoint3(
                    heights[0],
                    heights[1],
                    heights[2]
                    ));

                pointCount = 0;
                heights.Clear();

            }

        }

    }

    void ParseThirdSection() {

        var bytes = rawFile.data.GetRange(thirdSectionOffset, thirdSectionLength);

        foreach (int i in Enumerable.Range(0, thirdSectionLength / 2)) {

            var byteField = bytes.GetRange(i * 2, 2).ToArray();

            var bitField = new BitArray(byteField);

            var bitNumber6 = Utils.CopyBitsOfRange(bitField, 0, 6);
            var bitNumber10 = Utils.CopyBitsOfRange(bitField, 6, 16);

            thirdSectionBitfields.Add(new ThirdSectionBitfield(
                Utils.BitsToInt(bitNumber6),
                Utils.BitsToInt(bitNumber10)
                ));

        }

    }

    void ParseTiles() {

        var bytes = rawFile.data.GetRange(tileArrayOffset, tileCount * 4);

        foreach (int i in Enumerable.Range(0, tileCount)) {

            tiles.Add(new Tile(bytes.GetRange(i * 4, 4).ToArray()));

        }

        offset = tileArrayOffset + tileCount * 4;

    }

    void ParseTextures() {

        var bytes = rawFile.data.GetRange(offset, textureCordCount * 2);

        var texturesCords = new List<int>();

        foreach (int i in Enumerable.Range(0, textureCordCount)) {

            texturesCords.Add(Utils.BytesToShort(bytes.ToArray(), i * 2));

            if (texturesCords.Count() == 4) {
                textureCoordinates.Add(new TextureCoordinate(
                    texturesCords[0],
                    texturesCords[1],
                    texturesCords[2],
                    texturesCords[3]
                    ));

                texturesCords.Clear();

            }

        }

        offset += textureCordCount * 2;

    }

    void ParseTileGraphics() {

        var length = rawFile.data.Count() - offset;

        var bytes = rawFile.data.GetRange(offset, length);

        foreach (int i in Enumerable.Range(0, length / 2)) {
            tileGraphics.Add(new TileGraphics(bytes.GetRange(i * 2, 2).ToArray()));
        }

    }

}

struct HeightPoint3 {
    public byte height1;
    public byte height2;
    public byte height3;

    public HeightPoint3(byte height1, byte height2, byte height3) {
        this.height1 = height1;
        this.height2 = height2;
        this.height3 = height3;
    }

}

struct ThirdSectionBitfield {

    // 6 bit
    public int number1;
    // 10 bit
    public int number2;

    public ThirdSectionBitfield(int number1, int number2) {
        this.number1 = number1;
        this.number2 = number2;
    }

}

struct Tile {

    public byte[] data;

    public Tile(byte[] data) {
        this.data = data;
    }

}

struct TextureCoordinate {

    public int topLeftIndex;
    public int topRightIndex;
    public int bottomRightIndex;
    public int bottomLeftIndex;

    public TextureCoordinate(int topLeftIndex, int topRightIndex, int bottomRightIndex, int bottomLeftIndex) {
        this.topLeftIndex = topLeftIndex;
        this.topRightIndex = topRightIndex;
        this.bottomRightIndex = bottomRightIndex;
        this.bottomLeftIndex = bottomLeftIndex;
    }

}

struct TileGraphics {

    public byte[] data;

    public TileGraphics(byte[] data) {
        this.data = data;
    }

}