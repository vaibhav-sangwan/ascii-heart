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
        const float theta_spacing = 0.02f;
        const int width = 211;
        const int height = 50;
        const int zDistance = 5;
        static Vector3 light = new Vector3(0, 0, -1);

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
            int xDash = 0, yDash = 0, zDash = 0;
            for (int y = -50; y < 50; y++)
            {
                for (int x = -50; x < 50; x++)
                {
                    for (int z = -50; z <= 50; z++)
                    {
                        xSq = x * x;
                        ySq = y * y;
                        zSq = z * z;
                        xCu = x * x * x;
                        yCu = y * y * y;
                        zCu = z * z * z;
                        if (Math.Pow((xSq + (2 * zSq) + ySq - 200), 3) + (40 * xSq * yCu) - (0.045 * zSq * yCu) <= 0)
                        {
                            zDash = z + zDistance;
                            xDash = width / 2 + x * (30 / (zDash + 30));  //30 is viewer distance from screen, i.e., k1
                            yDash = height / 2 + y * (30 / (zDash + 30));


                            //calculate normal vector
                            Vector3 normal = new Vector3();
                            normal.X = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 2 * x) + (80 * x * yCu));
                            normal.Y = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 2 * y) + (120 * xSq * ySq) - (3 * 0.045f * zSq * ySq));
                            normal.Z = (float)((3 * Math.Pow((xSq + (2 * zSq) + ySq - 200), 2) * 4 * z) - (2 * 0.045f * z * yCu));
                            Vector3 unitNormal = new Vector3();
                            unitNormal.X = normal.X / normal.Length();
                            unitNormal.Y = normal.Y / normal.Length();
                            unitNormal.Z = normal.Z / normal.Length();

                            //dot product normal with lighting vector
                            float dotProduct = Vector3.Dot(unitNormal, light / light.Length());

                            //print character according to lumination
                            if (zDash < zBuffer[xDash, yDash])
                            {
                                zBuffer[xDash, yDash] = zDash;
                                if (dotProduct >= 0)
                                {
                                    int luminenceIndex = (int)(dotProduct * 11);
                                    output[xDash, yDash] = ".,-~:;=!*#$@"[luminenceIndex];
                                }
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


        }

        static void Main(string[] args)
        {
            while(true)
            {
                Console.ReadKey();
                Console.Clear();

                render();
                
            }
            

            //draw heart

            //spin heart
            //project heart onto 2d screen
            //determine illumination by calculating surface normal(given a light source)


            //misc

        }
    }
}
