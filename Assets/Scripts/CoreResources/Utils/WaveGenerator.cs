using System;

namespace CoreResources.Utils
{
    public enum WaveType
    {
        None = 0,
        Sine = 1,
        Tri = 2,
        Saw = 3,
        Sqr = 4
    }
    
    public class WaveGenerator
    {
        private const float pi = 3.14159265f;
        private const float doublePi = 2f * pi;
        private const float halfPi = pi / 2f;
        private const float gradTri = 2f / pi;
        private const float gradSqrSaw = 20f; // for the steep transitions
        private const float slopeWidth = 0.05f; // for the steep transitions
        private const float gradSaw = 1f / pi;


        public static float Sin(float x)
        {
            x %= doublePi;
            
            if (x < -pi)
                x += doublePi;
            else
            if (x >  pi)
                x -= doublePi;
 
            if ( x < 0 )
                return x * ( 1.27323954f + 0.405284735f * x );
            else
                return x * ( 1.27323954f - 0.405284735f * x );
        }

        public static float Tri(float x)
        {
            x %= doublePi;
            
            if (x < -pi)
                x += doublePi;
            else
            if (x >  pi)
                x -= doublePi;

            if (x is >= -halfPi and < halfPi)
            {
                return (gradTri * x);
            }
            
            if (x is >= halfPi and < pi)
            {
                return (-gradTri * (x - pi));
            }

            if (x is >= -pi and < -halfPi)
            {
                return (-gradTri * (x + pi));
            }

            throw new ArgumentOutOfRangeException("I don't know how you did it chief but you bamboozled a wave");
        }

        // For the square and sawtooth we'll use a steep line instead of a vertical one
        public static float Sqr(float x)
        {
            x %= doublePi;
            
            if (x < -pi)
                x += doublePi;
            else
            if (x >  pi)
                x -= doublePi;

            if (x is > slopeWidth and < pi - slopeWidth)
            {
                return 1f;
            }

            if (x is > -pi + slopeWidth and < -slopeWidth)
            {
                return - 1f;
            }

            if (x is > pi - slopeWidth and < pi)
            {
                return (gradSqrSaw * (x - pi));
            }

            if (x is > -pi and < -pi + slopeWidth)
            {
                return (gradSqrSaw * (x + pi));
            }

            if (x is > -slopeWidth and < slopeWidth)
            {
                return (gradSqrSaw * x);
            }
            
            throw new ArgumentOutOfRangeException("I don't know how you did it chief but you bamboozled a wave");
        }

        public static float Saw(float x)
        {
            x %= doublePi;
            
            if (x < -pi)
                x += doublePi;
            else
            if (x >  pi)
                x -= doublePi;

            if (x is > slopeWidth and < pi)
            {
                return (gradSaw * (x - pi));
            }
            
            if (x is > -pi and < -slopeWidth)
            {
                return (gradSaw * (x + pi));
            }

            if (x is > -slopeWidth and < slopeWidth)
            {
                return (-gradSqrSaw * x);
            }
            
            throw new ArgumentOutOfRangeException("I don't know how you did it chief but you bamboozled a wave");
        }
    }
}