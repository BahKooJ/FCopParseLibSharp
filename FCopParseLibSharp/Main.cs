using System.Collections;
using FCopParser;

var parser = new IFFParser(File.ReadAllBytes("Mp"));

var level = new FCopLevel(parser.parsedData);

level.sections[21].heightPoints = level.sections[10].heightPoints;
level.sections[21].thirdSectionBitfields = level.sections[10].thirdSectionBitfields;
level.sections[21].tiles = level.sections[10].tiles;
level.sections[21].textureCoordinates = level.sections[10].textureCoordinates;
level.sections[21].tileGraphics = level.sections[10].tileGraphics;

level.sections[21].Compile();

parser.Compile();

File.WriteAllBytes("Mp MOD", parser.bytes);

Console.WriteLine("bonk");
