using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StlConverter
{
    enum State
    {
        DEFAULT,
        OUTERLOOP,
        V1,
        V2,
        V3,
        ENDLOOP,
        ENDFACET,
    }

    struct Point
    {
        public float X, Y, Z;

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            var point = (Point)obj;
            return X == point.X &&
                   Y == point.Y &&
                   Z == point.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }
    }

    struct Triangle
    {
        public int Normal, V1, V2, V3;
    }

    public class Converter
    {
        public static void Convert(string inputPath, string outputPath)
        {
            byte[] fileAsBytes = System.IO.File.ReadAllBytes(inputPath);
            bool isTextFormat;
            if (fileAsBytes.Length >= 5 && fileAsBytes[0] == 's' && fileAsBytes[1] == 'o' && fileAsBytes[2] == 'l' && fileAsBytes[3] == 'i' && fileAsBytes[4] == 'd')
            {
                isTextFormat = true;
            } else
            {
                isTextFormat = false;
            }
            //string[] inputFile = System.IO.File.ReadAllText(inputPath).Split(new char[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<Point> vertices = new List<Point>();
            List<Point> normals = new List<Point>();
            List<Triangle> triangles = new List<Triangle>();
            string name = "";
            Triangle currentTriangle = new Triangle();
            Point currentPoint = new Point();
            if (isTextFormat)
            {
                string[] inputFile = System.Text.Encoding.ASCII.GetString(fileAsBytes).Split(new char[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                State state = State.DEFAULT;
                name = inputFile[1];
                for (int i = 1; i < inputFile.Count();)
                {
                    switch (state)
                    {
                        case State.DEFAULT:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (inputFile[i].Equals("endsolid"))
                            {
                                i = inputFile.Count();
                                break;
                            }
                            if (!inputFile[i].Equals("facet"))
                            {
                                throw new FormatException("Expected \"facet\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (!inputFile[i].Equals("normal"))
                            {
                                throw new FormatException("Expected \"normal\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.X = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Y = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Z = float.Parse(inputFile[i]);
                            currentTriangle.Normal = normals.IndexOf(currentPoint);
                            if (currentTriangle.Normal < 0)
                            {
                                currentTriangle.Normal = normals.Count();
                                normals.Add(currentPoint);
                            }
                            currentPoint = new Point();
                            state = State.OUTERLOOP;
                            break;
                        case State.OUTERLOOP:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of fFile");
                            }
                            if (!inputFile[i].Equals("outer"))
                            {
                                throw new FormatException("Expected \"outer\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (!inputFile[i].Equals("loop"))
                            {
                                throw new FormatException("Expected \"loop\" as word " + i);
                            }
                            state = State.V1;
                            break;
                        case State.V1:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (!inputFile[i].Equals("vertex"))
                            {
                                throw new FormatException("Expected \"vertex\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.X = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Y = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Z = float.Parse(inputFile[i]);
                            currentTriangle.V1 = vertices.IndexOf(currentPoint);
                            if (currentTriangle.V1 < 0)
                            {
                                currentTriangle.V1 = vertices.Count();
                                vertices.Add(currentPoint);
                            }
                            currentPoint = new Point();
                            state = State.V2;
                            break;
                        case State.V2:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (!inputFile[i].Equals("vertex"))
                            {
                                throw new FormatException("Expected \"vertex\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.X = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Y = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Z = float.Parse(inputFile[i]);
                            currentTriangle.V2 = vertices.IndexOf(currentPoint);
                            if (currentTriangle.V2 < 0)
                            {
                                currentTriangle.V2 = vertices.Count();
                                vertices.Add(currentPoint);
                            }
                            currentPoint = new Point();
                            state = State.V3;
                            break;
                        case State.V3:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (!inputFile[i].Equals("vertex"))
                            {
                                throw new FormatException("Expected \"vertex\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.X = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Y = float.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            currentPoint.Z = float.Parse(inputFile[i]);
                            currentTriangle.V3 = vertices.IndexOf(currentPoint);
                            if (currentTriangle.V3 < 0)
                            {
                                currentTriangle.V3 = vertices.Count();
                                vertices.Add(currentPoint);
                            }
                            currentPoint = new Point();
                            state = State.ENDLOOP;
                            break;
                        case State.ENDLOOP:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (!inputFile[i].Equals("endloop"))
                            {
                                throw new FormatException("Expected \"endloop\" as word " + i);
                            }
                            state = State.ENDFACET;
                            break;
                        case State.ENDFACET:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of file");
                            }
                            if (!inputFile[i].Equals("endfacet"))
                            {
                                throw new FormatException("Expected \"endfacet\" as word " + i);
                            }
                            state = State.DEFAULT;
                            triangles.Add(currentTriangle);
                            currentTriangle = new Triangle();
                            break;
                    }
                }
            }
            else
            {
                if (fileAsBytes.Length < 84)
                {
                    throw new FormatException("Unexpected end of file. A binary stl-File must be at least 84 bytes in size");
                }
                int numberOfTriangles = BitConverter.ToInt32(fileAsBytes, 80);
                if (fileAsBytes.Length < 82 + numberOfTriangles * 50)
                {
                    throw new FormatException("Expected size of File: " + (82 + numberOfTriangles * 50) + " bytes, but is: " + fileAsBytes.Length + " bytes");
                }
                for (int i = 0, currentByte = 84; i < numberOfTriangles; i++, currentByte += 50)
                {
                    Point n, v1, v2, v3;
                    Triangle t;
                    n.X = BitConverter.ToSingle(fileAsBytes, currentByte);
                    n.Y = BitConverter.ToSingle(fileAsBytes, currentByte + 4);
                    n.Z = BitConverter.ToSingle(fileAsBytes, currentByte + 8);
                    v1.X = BitConverter.ToSingle(fileAsBytes, currentByte + 12);
                    v1.Y = BitConverter.ToSingle(fileAsBytes, currentByte + 16);
                    v1.Z = BitConverter.ToSingle(fileAsBytes, currentByte + 20);
                    v2.X = BitConverter.ToSingle(fileAsBytes, currentByte + 24);
                    v2.Y = BitConverter.ToSingle(fileAsBytes, currentByte + 28);
                    v2.Z = BitConverter.ToSingle(fileAsBytes, currentByte + 32);
                    v3.X = BitConverter.ToSingle(fileAsBytes, currentByte + 36);
                    v3.Y = BitConverter.ToSingle(fileAsBytes, currentByte + 40);
                    v3.Z = BitConverter.ToSingle(fileAsBytes, currentByte + 44);
                    t.Normal = vertices.IndexOf(n);
                    if (t.Normal < 0)
                    {
                        t.Normal = vertices.Count;
                        vertices.Add(n);
                    }
                    t.V1 = vertices.IndexOf(v1);
                    if (t.Normal < 0)
                    {
                        t.Normal = vertices.Count;
                        vertices.Add(v1);
                    }
                    t.V2 = vertices.IndexOf(v2);
                    if (t.Normal < 0)
                    {
                        t.Normal = vertices.Count;
                        vertices.Add(v2);
                    }
                    t.V3 = vertices.IndexOf(v3);
                    if (t.Normal < 0)
                    {
                        t.Normal = vertices.Count;
                        vertices.Add(v3);
                    }
                    triangles.Add(t);
                }
            }
            List<string> outputFile = new List<string>(1 + vertices.Count() + normals.Count() + triangles.Count());
            outputFile.Add("o " + name);
            for (int i = 0; i < vertices.Count(); i++)
            {
                outputFile.Add(string.Format("v {0} {1} {2}", vertices[i].X, vertices[i].Y, vertices[i].Z));
            }
            for (int i = 0; i < normals.Count(); i++)
            {
                outputFile.Add(string.Format("vn {0} {1} {2}", normals[i].X, normals[i].Y, normals[i].Z));
            }
            for (int i = 0; i < triangles.Count(); i++)
            {
                outputFile.Add(string.Format("f {0}//{3} {1}//{3} {2}//{3}", triangles[i].V1 + 1, triangles[i].V2 + 1, triangles[i].V3 + 1, triangles[i].Normal + 1));
            }
            System.IO.File.WriteAllLines(outputPath, outputFile);
        }
    }
}
