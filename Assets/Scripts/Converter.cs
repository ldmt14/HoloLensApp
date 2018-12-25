using System;
using System.Collections.Generic;
using System.Linq;

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
        public double X, Y, Z;

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
            string[] inputFile = System.IO.File.ReadAllText(inputPath).Split(new char[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<Point> vertices = new List<Point>();
            List<Point> normals = new List<Point>();
            List<Triangle> triangles = new List<Triangle>();
            string name = "";
            Triangle currentTriangle = new Triangle();
            Point currentPoint = new Point();
            if (inputFile[0].Equals("solid"))
            {
                State state = State.DEFAULT;
                name = inputFile[1];
                for (int i = 1; i < inputFile.Count();)
                {
                    switch (state)
                    {
                        case State.DEFAULT:
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
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
                                throw new FormatException("Unexpected end of File");
                            }
                            if (!inputFile[i].Equals("normal"))
                            {
                                throw new FormatException("Expected \"normal\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.X = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Y = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Z = double.Parse(inputFile[i]);
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
                                throw new FormatException("Unexpected end of File");
                            }
                            if (!inputFile[i].Equals("outer"))
                            {
                                throw new FormatException("Expected \"outer\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
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
                                throw new FormatException("Unexpected end of File");
                            }
                            if (!inputFile[i].Equals("vertex"))
                            {
                                throw new FormatException("Expected \"vertex\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.X = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Y = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Z = double.Parse(inputFile[i]);
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
                                throw new FormatException("Unexpected end of File");
                            }
                            if (!inputFile[i].Equals("vertex"))
                            {
                                throw new FormatException("Expected \"vertex\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.X = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Y = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Z = double.Parse(inputFile[i]);
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
                                throw new FormatException("Unexpected end of File");
                            }
                            if (!inputFile[i].Equals("vertex"))
                            {
                                throw new FormatException("Expected \"vertex\" as word " + i);
                            }
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.X = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Y = double.Parse(inputFile[i]);
                            if (++i >= inputFile.Count())
                            {
                                throw new FormatException("Unexpected end of File");
                            }
                            currentPoint.Z = double.Parse(inputFile[i]);
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
                                throw new FormatException("Unexpected end of File");
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
                                throw new FormatException("Unexpected end of File");
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
