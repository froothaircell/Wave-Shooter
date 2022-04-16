using System;

namespace CoreResources.Utils
{
    public enum WaveType
    {
        None = 0,
        Sine = 1,
        Tri = 2
    }
    
    public class WaveGenerator
    {
        private const float pi = 3.14159265f;
        private const float doublePi = 2f * pi;
        private const float halfPi = pi / 2f;
        
        public static float Sin( float x )
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
            
            float grad = 2f / pi;
            
            if (x < -pi)
                x += doublePi;
            else
            if (x >  pi)
                x -= doublePi;

            if (x is >= 0 and < halfPi)
            {
                return grad * x;
            }

            if (x is >= halfPi and < pi)
            {
                return (1f - grad * x);
            }

            if (x is >= -halfPi and < 0)
            {
                return (grad * x - 1f);
            }
            
            if (x is >= -pi and < -halfPi)
            {
                return (-grad * x);
            }

            throw new ArgumentOutOfRangeException("I don't know how you did it chief but you bamboozled a wave");
        }
    }
}