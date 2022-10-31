﻿
using System.Collections.Generic;

namespace FCopParser {
    public abstract class MeshType {

        static public List<TileVertex> VerticiesFromID(int id) {

            // Tiles can be rendered with both sides, or just one side. Could be those unknown numbers.

            switch (id) {

                case 0: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 1: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 2: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 3: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 4: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 5: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft)
                        };

                    }

                case 6: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 7: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomRight),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 8: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 9: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 10: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 11: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomRight),
                            new TileVertex(3, VertexPosition.TopRight)
                        };

                    }

                case 12: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 13: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 14: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 15: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(3, VertexPosition.TopRight)
                        };

                    }

                case 16: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 17: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft)
                        };

                    }

                case 18: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 19: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft)
                        };

                    }

                case 20: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 21: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 22: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 23: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 24: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft)
                        };

                    }

                case 25: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 26: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft)
                        };

                    }

                case 27: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 28: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 29: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 30: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 31: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomRight),
                            new TileVertex(3, VertexPosition.TopRight)
                        };

                    }

                case 32: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft)
                        };

                    }

                case 33: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 34: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft)
                        };

                    }

                case 35: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 36: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 37: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft)
                        };

                    }

                case 38: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 39: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft)
                        };

                    }

                case 40: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomRight),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 41: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 42: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 43: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 44: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 45: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 46: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 47: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft)
                        };

                    }

                case 48: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 49: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 50: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 51: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 52: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft)
                        };

                    }

                case 53: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 54: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft)
                        };

                    }

                case 55: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 56: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 57: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 58: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 59: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(3, VertexPosition.TopRight)
                        };

                    }

                case 60: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 61: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 62: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 63: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 64: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 65: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 66: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 67: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 68: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 69: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 70: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 71: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft)
                        };

                    }

                case 72: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomLeft)
                        };

                    }

                case 73: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft)
                        };

                    }

                case 74: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.TopLeft)
                        };

                    }

                case 75: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.TopLeft)
                        };

                    }

                case 76: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft)
                        };

                    }

                case 77: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomLeft)
                        };

                    }

                case 78: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomLeft)
                        };

                    }

                case 79: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }
                //Missing 88, 94, 95, 101
                case 80: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 81: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 82: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.TopRight)
                        };

                    }

                case 83: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 84: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 85: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 86: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 87: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 89: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 90: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.TopRight)
                        };

                    }

                case 91: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 92: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.TopRight)
                        };

                    }

                case 93: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 96: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 97: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 98: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomRight)
                        };

                    }

                case 99: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 100: {

                        return new List<TileVertex>() {
                            new TileVertex(2, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomRight)
                        };

                    }

                case 102: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.BottomRight),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 103: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 104: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 105: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 106: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.BottomRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomRight)
                        };

                    }

                case 107: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                        };

                    }

                case 108: {

                        return new List<TileVertex>() {
                            new TileVertex(1, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(1, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                case 109: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.BottomLeft),
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.BottomLeft),
                            new TileVertex(2, VertexPosition.TopLeft)
                        };

                    }

                case 110: {

                        return new List<TileVertex>() {
                            new TileVertex(3, VertexPosition.TopLeft),
                            new TileVertex(3, VertexPosition.TopRight),
                            new TileVertex(2, VertexPosition.TopLeft),
                            new TileVertex(2, VertexPosition.TopRight)
                        };

                    }

                default: return new List<TileVertex>();

            };

        }

    }

}