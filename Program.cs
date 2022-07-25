using System;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;

namespace ascii_heart
{
    class Program
    {
        public class Point
        {
            public Vector3 coord = new Vector3();
            public Vector3 normalVector = new Vector3(0, 0, -1);
            public Point(Vector3 coordinates)
            {
                coord = coordinates;
                normalVector = calcNormalAt(coord);
            }

        }

        static int windowHeight = Console.WindowHeight;
        static int windowWidth = Console.WindowWidth;
        static int camDistance = 30;
        static int r = 10; //sphereRadius
        static int rSquare = r * r;
        static int alpha = windowWidth / 2;
        static int beta = windowHeight / 2;
        static float theta = 0; //angle of rotation of heart about y-axis
        static Vector3 lightVector = new Vector3(0, 0, 1);     //positive x is towards right, positive y is downwards, positive z is out of the screen
        static double magOfLightVector = lightVector.Length();


        static public void renderFrame(List<Point> points)
        {
            foreach(Point point in points)
            {
                Vector3 coord = getCoords(theta, point.coord);
                drawChar(coord, point);
            }
        }

        static Vector3 getCoords(float theta, Vector3 coordinates)
        {
            float x = coordinates.X;
            float y = coordinates.Y;
            float z = coordinates.Z;
            Vector3 coord = new Vector3(x, y, z);
            float sinTheta = MathF.Sin(theta);
            float cosTheta = MathF.Cos(theta);
            coord.X = (z *  sinTheta) + (cosTheta * x);
            coord.Z = (z * cosTheta) - (x * sinTheta); 
            return coord;
        }

        public static void drawChar(Vector3 coord, Point point)
        {
            Console.SetCursorPosition((int)coord.X + alpha, (int)coord.Y + beta);
            //double magOfPositionVector = Math.Sqrt((coord.X * coord.X) + (coord.Y * coord.Y)+ (coord.Z * coord.Z));
            //double dotOfNormalAndLight = Vector3.Dot(coord, lightVector);
            //point.normalVector = calcNormalAt(coord);
            float magOfNormalVector = point.normalVector.Length();
            Vector3 normal = point.coord;
            normal.X = (point.normalVector.Z * MathF.Sin(theta)) + (MathF.Cos(theta) * point.normalVector.X);
            normal.Z = (point.normalVector.Z * MathF.Cos(theta)) - (point.normalVector.X * MathF.Sin(theta));
            double dotOfNormalAndLight = Vector3.Dot(normal, lightVector);
            double luminence = dotOfNormalAndLight/(magOfLightVector * normal.Length());
            char x = 'Y';
            if (luminence <= 0)
                x = '.';
            else if (luminence <= 0.08f)
                x = ',';
            else if (luminence <= 0.16f)
                x = '-';
            else if (luminence <= 0.24f)
                x = '~';
            else if (luminence <= 0.32f)
                x = ':';
            else if (luminence <= 0.40f)
                x = ';';
            else if (luminence <= 0.50f)
                x = '=';
            else if (luminence <= 0.60f)
                x = '!';
            else if (luminence <= 0.70f)
                x = '*';
            else if (luminence <= 0.80f)
                x = '#';
            else if (luminence <= 0.90f)
                x = '$';
            else if (luminence <= 1f)
                x = '@';
            else
                x = 'x';

            //Console.WriteLine("Position Vector: " + coord + " Luminence: " + luminence + " Character:" + x);
            Console.Write(x);
        }

        static public Vector3 calcNormalAt(Vector3 coord)
        {
            Vector3 normalVector = new Vector3(0, 0, 1);
            float x = coord.X;
            float y = coord.Y;
            float z = coord.Z;
            normalVector.X = (6 * x * ((x * x) + (y * y) + (2 * z * z) - 100) * ((x * x) + (y * y) + (2 * z * z) - 100)) + (80 * x * y * y * y);
            normalVector.Y = (6 * y * ((x * x) + (y * y) + (2 * z * z) - 100) * ((x * x) + (y * y) + (2 * z * z) - 100)) + (120 * x * x * y * y) - (0.135f * y * y * z * z);
            normalVector.Z = (12 * z * ((x * x) + (y * y) + (2 * z * z) - 100) * ((x * x) + (y * y) + (2 * z * z) - 100)) - (0.09f * y * y * y * z);
            return normalVector;
        }

        //static void Main(string[] args)
        //{
        //    List<Point> points = new List<Point>();
        //    for (int x = -windowWidth / 2; x < windowWidth / 2; x++)
        //    {
        //        for (int y = -windowWidth / 2; y < windowWidth / 2; y++)
        //        {
        //            int xSquare = x * x;
        //            int xCube = x * x * x;
        //            int ySquare = y * y;
        //            int yCube = y * y * y;
        //            double prevValue = 1;
        //            for (int k = -camDistance; k <= camDistance; k++)
        //            {
        //                double currValue = (MathF.Pow((float)(xSquare + (2 * k * k) + ySquare - 100), 3) + (40 * xSquare * yCube) - (0.045 * k * k * yCube));
        //                //if ((MathF.Pow((float)(ySquare + (2 * k * k) + xSquare - 100), 3) - (40 *ySquare * xCube) - (0.045 * k * k * xCube)) <= 0)  //front horizontal
        //                if (prevValue * currValue <= 0)  //front vertical
        //                                                                                                                                             //if ((MathF.Pow((float)(xSquare + (2 * ySquare) + (k * k) - 100), 3) - (40 * xSquare * k * k * k) - (0.045 * ySquare * k * k * k)) <= 0) //top horizontal
        //                {
        //                    points.Add(new Point(new Vector3(x, y, k)));
        //                }
        //                prevValue = currValue;
        //            }
        //        }
        //    }
        //    renderFrame(points);
        //    Console.ReadKey();
        //    foreach (Point point in points)
        //        Console.WriteLine(point);
        //    Console.ReadKey();
        //    for (; ; )
        //    {
        //        Console.Clear();
        //        renderFrame(points);
        //        theta += 0.2f;
        //        if (theta >= 2 * Math.PI)
        //        {
        //            theta = 0;
        //        }
        //        Thread.Sleep(10);
        //    }
        //}

    }
}
