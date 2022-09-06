
class FCopLevel {

    List<IFFDataFile> rawFiles = new List<IFFDataFile> ();

    public FCopLevel(IFFFileManager fileManager) {

        rawFiles = fileManager.GrabAllFiles(where: file => {

            return file.dataFourCC == "Ctil";

        });

    }


}

class FCopLevelSection {

    IFFDataFile rawFile;

    public FCopLevelSection(IFFDataFile rawFile) {
        this.rawFile = rawFile;
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