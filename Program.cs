using System;
using System.Numerics;

namespace ascii_heart
{
    class Program
    {
        static int windowHeight = Console.WindowHeight;
        static int windowWidth = Console.WindowWidth;
        static int camDistance = 11;
        static int r = 10; //sphereRadius
        static int rSquare = r * r;
        static int alpha = windowWidth / 2;
        static int beta = windowHeight / 2;
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
                        Vector3 coord = new Vector3(x, y, z);
                        drawChar(coord);
                        
                    }
                }
            }
            //get the light vector
            //get the dot product of orientation and light vector
            //render each pixel with a character representing light intensity
        }

        static void Main(string[] args)
        {
            Console.Clear();
            renderFrame();
            Console.ReadKey();

        }

        public static int getSphere(int x, int y)
        {
            int xSquare = x * x;
            int ySquare = y * y;
            for(int k = camDistance; k >= 0; k--)
            {
                if (xSquare + ySquare + (k * k) <= rSquare)
                    return k;              
            }
            return -1;
        }

        public static void drawChar(Vector3 coord)
        {
            Console.SetCursorPosition((int)coord.X + alpha, (int)coord.Y + beta);
            double magOfPositionVector = Math.Sqrt((coord.X * coord.X) + (coord.Y * coord.Y)+ (coord.Z * coord.Z));
            double dotOfNormalAndLight = Vector3.Dot(coord, lightVector);
            double luminence = dotOfNormalAndLight/(magOfPositionVector * magOfLightVector);
            char x = ' ';
            if (luminence >= -0.083f)
                x = '.';
            else if (luminence >= -0.166f)
                x = ',';
            else if (luminence >= -0.25f)
                x = '-';
            else if (luminence >= -0.33f)
                x = '~';
            else if (luminence >= -0.416f)
                x = ':';
            else if (luminence >= -0.5f)
                x = ';';
            else if (luminence >= -0.583f)
                x = '=';
            else if (luminence >= -0.666f)
                x = '!';
            else if (luminence >= -0.75f)
                x = '*';
            else if (luminence >= -0.833f)
                x = '#';
            else if (luminence >= -0.916f)
                x = '$';
            else if (luminence >= -1)
                x = '@';

            //Console.WriteLine("Position Vector: " + coord + " Magnitude:" + magOfPositionVector + " Dot Product:" + dotOfNormalAndLight +  " Luminence: " + luminence + " Character:" + x);
            Console.Write(x);
        }


    }
}
