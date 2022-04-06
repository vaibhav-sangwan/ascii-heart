using System;
using System.Numerics;
using System.Threading;

namespace ascii_heart
{
    class Program
    {
        static int windowHeight = Console.WindowHeight;
        static int windowWidth = Console.WindowWidth;
        static int camDistance = 20;
        static int r = 10; //sphereRadius
        static int rSquare = r * r;
        static int alpha = windowWidth / 2;
        static int beta = windowHeight / 2;
        static float theta = 0; //angle of rotation of heart about y-axis
        static Vector3 lightVector = new Vector3(1, 1, -1);     //positive x is towards right, positive y is downwards, positive z is out of the screen
        static double magOfLightVector = Math.Sqrt((lightVector.X * lightVector.X) + (lightVector.Y * lightVector.Y) + (lightVector.Z * lightVector.Z));

        static public void renderFrame()
        {
            //get the current orientation of heart and the mathematical representation of heart
            for(int x = - windowWidth/2; x < windowWidth / 2; x++)
            {
                for(int y = - windowWidth / 2; y < windowWidth / 2; y++)
                {
                    int z = getSphere(x, y);
                    if(z != -1)
                    {
                        Vector3 coord = getCoords(theta, x, y, z);
                        drawChar(coord);
                        
                    }
                }
            }
            //get the light vector
            //get the dot product of orientation and light vector
            //render each pixel with a character representing light intensity
        }

        static Vector3 getCoords(float theta, int x, int y, int z)
        {
            Vector3 coord = new Vector3(x, y, z);
            float sinTheta = MathF.Sin(theta);
            float cosTheta = MathF.Cos(theta);
            coord.X = cosTheta * x;
            return coord;
        }

        static void Main(string[] args)
        {
            for (; ; )
            {
                Console.Clear();
                renderFrame();
                theta += 0.1f;
                Thread.Sleep(30);
            }
        }

        public static int getSphere(int x, int y)
        {
            int xSquare = x * x;
            int xCube = x * x * x;
            int ySquare = y * y;
            int yCube = y * y * y;
            for(int k = camDistance; k >= -camDistance; k--)
            {
                //if ((MathF.Pow((float)(ySquare + (2 * k * k) + xSquare - 100), 3) - (40 *ySquare * xCube) - (0.045 * k * k * xCube)) <= 0)  //front horizontal
                if ((MathF.Pow((float)(xSquare + (2 * k * k) + ySquare - 100), 3) + (40 * xSquare * yCube) - (0.045 * k * k * yCube)) <= 0)  //front vertical
                //if ((MathF.Pow((float)(xSquare + (2 * ySquare) + (k * k) - 100), 3) - (40 * xSquare * k * k * k) - (0.045 * ySquare * k * k * k)) <= 0) //top horizontal
                {
                    return k;
                }            
            }
            return -1;
        }

        public static void drawChar(Vector3 coord)
        {
            Console.SetCursorPosition((int)coord.X + alpha, (int)coord.Y + beta);
            //double magOfPositionVector = Math.Sqrt((coord.X * coord.X) + (coord.Y * coord.Y)+ (coord.Z * coord.Z));
            //double dotOfNormalAndLight = Vector3.Dot(coord, lightVector);
            double luminence = coord.Z;
            char x = ' ';
            if (luminence <= 0.5f)
                x = '.';
            else if (luminence <= 1)
                x = ',';
            else if (luminence <= 1.5f)
                x = '-';
            else if (luminence <= 2)
                x = '~';
            else if (luminence <= 2.5f)
                x = ':';
            else if (luminence <= 3)
                x = ';';
            else if (luminence <= 3.5f)
                x = '=';
            else if (luminence <= 4)
                x = '!';
            else if (luminence <= 4.5f)
                x = '*';
            else if (luminence <= 5)
                x = '#';
            else if (luminence <= 6)
                x = '$';
            else if (luminence <= 7)
                x = '@';

            //Console.WriteLine("Position Vector: " + coord + " Magnitude:" + magOfPositionVector + " Dot Product:" + dotOfNormalAndLight +  " Luminence: " + luminence + " Character:" + x);
            Console.Write(x);
        }


    }
}
