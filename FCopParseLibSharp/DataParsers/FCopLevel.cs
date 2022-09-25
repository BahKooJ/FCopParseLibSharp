
using System.Collections;

// =WIP=
class FCopLevel {

    List<FCopLevelSectionParser> sections = new List<FCopLevelSectionParser> ();

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

    short textureCordCount;
    short tileCount;


    List<HeightPoint3> heightPoints = new List<HeightPoint3>();
    List<ThirdSectionBitfield> thirdSectionBitfields = new List<ThirdSectionBitfield>();
    List<Tile> tiles = new List<Tile>();
    List<TextureCoordinate> textureCoordinates = new List<TextureCoordinate>();
    List<TileGraphics> tileGraphics = new List<TileGraphics>();


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

        Console.WriteLine("foo");

    }

    void Compile() {

        List<byte> compiledFile = new List<byte> ();

        foreach (HeightPoint3 heightPoint3 in heightPoints) {

            compiledFile.Add(heightPoint3.height1);
            compiledFile.Add(heightPoint3.height2);
            compiledFile.Add(heightPoint3.height3);

        }


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
    int number1;
    // 10 bit
    int number2;

    public ThirdSectionBitfield(int number1, int number2) {
        this.number1 = number1;
        this.number2 = number2;
    }

}

struct Tile {

    byte[] data;

    public Tile(byte[] data) {
        this.data = data;
    }

}

struct TextureCoordinate {

    int topLeftIndex;
    int topRightIndex;
    int bottomRightIndex;
    int bottomLeftIndex;

    public TextureCoordinate(int topLeftIndex, int topRightIndex, int bottomRightIndex, int bottomLeftIndex) {
        this.topLeftIndex = topLeftIndex;
        this.topRightIndex = topRightIndex;
        this.bottomRightIndex = bottomRightIndex;
        this.bottomLeftIndex = bottomLeftIndex;
    }

}

struct TileGraphics {

    byte[] data;

    public TileGraphics(byte[] data) {
        this.data = data;
    }

}