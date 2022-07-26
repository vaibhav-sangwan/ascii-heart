using System;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascii_heart
{
    class Program
    {
        const float theta_spacing = 0.3f;   //rotation speed
        static float theta = 0;             //initial rotation
        const int width = 211;              //width of terminal
        const int height = 50;              //height of terminal
        const int zDistance = 50;           //distance of 3D object from screen
        static Vector3 light = new Vector3(0, 0, 1);        //light vector - change here to change the direction of light
        static List<Point> points = new List<Point>();      //A list to hold all the points satisfying the condition of 3D object

        public class Point
        {
            public Vector3 pos = new Vector3();
            public Vector3 unitNormal = new Vector3();

            public Point(Vector3 position)
            {
                int x = (int)position.X, y = (int)position.Y, z = (int)position.Z;
                int xSq = x * x, zSq = z * z, ySq = y * y, yCu = y * y * y;

                Vector3 normal = new Vector3();
                normal.X = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 2 * x) + (80 * x * yCu));
                normal.Y = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 2 * y) + (120 * xSq * ySq) - (3 * 0.045f * zSq * ySq));
                normal.Z = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 4 * z) - (2 * 0.045f * z * yCu));

                this.unitNormal.X = normal.X / normal.Length();
                this.unitNormal.Y = normal.Y / normal.Length();
                this.unitNormal.Z = normal.Z / normal.Length();

                this.pos.X = x;
                this.pos.Y = y;
                this.pos.Z = z;
            }
        }

        static void getPoints()
        {
            int xSq = 0, ySq = 0, zSq = 0, xCu = 0, yCu = 0, zCu = 0;
            for (int y = -20; y < 20; y++)
            {
                for (int x = -20; x < 20; x++)
                {
                    for (int z = -20; z <= 20; z++)
                    {
                        xSq = x * x;
                        ySq = y * y;
                        zSq = z * z;
                        xCu = x * x * x;
                        yCu = y * y * y;
                        zCu = z * z * z;
                        if (Math.Pow((xSq + (2 * zSq) + ySq - 200), 3) + (40 * xSq * yCu) - (0.045 * zSq * yCu) <= 0)
                        {
                            Point point = new Point(new Vector3(x, y, z));
                            points.Add(point);
                        }
                    }
                }
            }

        }

        static void render()
        {
            float[,] zBuffer = new float[width, height];        //to keep track of the nearest point of object to the screen
            char[,] output = new char[width, height];           //to keep track of the character to show at each pixel on the screen
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    output[i, j] = ' ';
                    zBuffer[i, j] = 1000;
                }
            }
            int x = 0, y = 0, z = 0;
            int xDash = 0, yDash = 0;
            float zDash = 0;
            foreach (Point point in points)
            {
                x = (int)point.pos.X;
                y = (int)point.pos.Y;
                z = (int)point.pos.Z;

                //Plotting the 3D object onto 2D screen. xDash and yDash represent the pixel on which a specific point will be projected to
                //zDash is z after revolving + zDistance => the distance of that point from the screen
                double xAfterRevolving = 0, zAfterRevolving = 0;
                xAfterRevolving = z * Math.Sin(theta) + x * Math.Cos(theta);
                zAfterRevolving = z * Math.Cos(theta) - x * Math.Sin(theta);
                zDash = -(int)zAfterRevolving + zDistance;
                xDash = width / 2 + (int)(xAfterRevolving * (200 / (zDash + 200)));  //200 is viewer distance from screen, i.e., k1
                yDash = height / 2 + (int)(y * (200 / (zDash + 200)));

                //Calculating Unit Vector of a point on the object after it has rotated through an angle of Theta
                Vector3 unitNormalAfterRevolving = new Vector3();
                unitNormalAfterRevolving.Y = point.unitNormal.Y;
                unitNormalAfterRevolving.X = (float)(point.unitNormal.Z * (Math.Sin(theta)) + point.unitNormal.X * (Math.Cos(theta)));
                unitNormalAfterRevolving.Z = (float)(point.unitNormal.Z * (Math.Cos(theta)) - point.unitNormal.X * (Math.Sin(theta)));

                //dot product of unit normal vector with unit lighting vector
                float dotProduct = Vector3.Dot(unitNormalAfterRevolving, light / light.Length());

                //if zDash of current point is less than zBuffer, i.e, it is the nearest point on xDash, yDash to the screen, only then print it.
                //we don't need to show points which are behind other points
                if (zDash < zBuffer[xDash, yDash])
                {
                    zBuffer[xDash, yDash] = zDash;
                    if (dotProduct >= 0)    //show only those points which are facing towards light otherwise show them with a dot '.'
                    {
                        int luminenceIndex = (int)(dotProduct * 11.35f);            //dotProduct varies from 0 to 1 => luminence varies from 0 to 11
                        output[xDash, yDash] = ".,-~:;=!*#$@"[luminenceIndex];      //feeding characters to the output buffer according to lumination
                    }
                    else
                        output[xDash, yDash] = '.';
                }
            }
            for (int j = 0; j < height; j++)        //printing the output buffer
            {
                for (int i = 0; i < width; i++)
                {
                    Console.Write(output[i, j]);
                }
            }
        }

        static void Main(string[] args)
        {
            getPoints();
            Console.ReadKey();
            while (true)
            {
                render();
                theta += theta_spacing;
                Console.SetCursorPosition(0, 0);
            }
        }
    }
}
