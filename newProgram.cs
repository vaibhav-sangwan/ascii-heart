using System;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ascii_heart
{
    class newProgram
    {
        const float theta_spacing = 0.7f;
        static float theta = 0;
        const int width = 211;
        const int height = 50;
        const int zDistance = 50;
        static Vector3 light = new Vector3(0, 0, 1);

        static void render()
        {
            float[,] zBuffer = new float[width, height];
            char[,] output = new char[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    output[x, y] = ' ';
                    zBuffer[x, y] = 1000;
                }
            }
            int xSq = 0, ySq = 0, zSq = 0, xCu = 0, yCu = 0, zCu = 0;
            int xDash = 0, yDash = 0;
            float zDash = 0;
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
                            double xAfterRevolving = 0, zAfterRevolving = 0;
                            xAfterRevolving = z * Math.Sin(theta) + x * Math.Cos(theta);
                            zAfterRevolving = z * Math.Cos(theta) - x * Math.Sin(theta);
                            zDash = -(int)zAfterRevolving + zDistance;
                            xDash = width / 2 + (int)(xAfterRevolving * (zDash / (zDash + 10)));  //30 is viewer distance from screen, i.e., k1
                            yDash = height / 2 + (int)(y * (zDash / (zDash + 10)));


                            //calculate normal vector
                            Vector3 normal = new Vector3();
                            normal.X = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 2 * x) + (80 * x * yCu));
                            normal.Y = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 2 * y) + (120 * xSq * ySq) - (3 * 0.045f * zSq * ySq));
                            normal.Z = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 4 * z) - (2 * 0.045f * z * yCu));
                            Vector3 unitNormal = new Vector3();
                            unitNormal.X = normal.X / normal.Length();
                            unitNormal.Y = normal.Y / normal.Length();
                            unitNormal.Z = normal.Z / normal.Length();

                            Vector3 unitNormalAfterRevolving = new Vector3();
                            unitNormalAfterRevolving.Y = unitNormal.Y;
                            unitNormalAfterRevolving.X = (float)(unitNormal.Z * (Math.Sin(theta)) + unitNormal.X * (Math.Cos(theta)));
                            unitNormalAfterRevolving.Z = (float)(unitNormal.Z * (Math.Cos(theta)) - unitNormal.X * (Math.Sin(theta)));

                            //dot product normal with lighting vector
                            float dotProduct = Vector3.Dot(unitNormalAfterRevolving, light / light.Length());

                            //print character according to lumination
                            if (zDash < zBuffer[xDash, yDash])
                            {
                                zBuffer[xDash, yDash] = zDash;
                                if (dotProduct >= 0)
                                {
                                    int luminenceIndex = (int)(dotProduct * 11);
                                    output[xDash, yDash] = ".,-~:;=!*#$@"[luminenceIndex];
                                }
                                else
                                    output[xDash, yDash] = '.';
                            }

                        }
                    }
                }
            }
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    Console.Write(output[x, y]);
                }
            }
            theta += theta_spacing;


        }

        static void Main(string[] args)
        {

            Console.ReadKey();
            while (true)
            {
                Console.Clear();

                render();

            }



        }
    }
}
