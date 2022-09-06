
class FCopLevel {

    List<IFFDataFile> rawFiles = new List<IFFDataFile> ();

    public FCopLevel(IFFFileManager fileManager) {

        rawFiles = fileManager.files.Where( file => {

            return file.dataFourCC == "Ctil";

        }).ToList();

    }


}

class FCopLevelSection {

    const int textureCordCountOffset = 10;
    const int heightMapOffset = 12;
    const int heightMapEndOffset = 879;

    IFFDataFile rawFile;

    public FCopLevelSection(IFFDataFile rawFile) {
        this.rawFile = rawFile;
    }

    public void ParseHeightPoints() {

        rawFile.data.GetRange(heightMapOffset, heightMapEndOffset);

    }

}

struct HeightPoint3 {
    public sbyte height1;
    public sbyte height2;
    public sbyte height3;

    public HeightPoint3(sbyte height1, sbyte height2, sbyte height3) {
        this.height1 = height1;
        this.height2 = height2;
        this.height3 = height3;
    }

}