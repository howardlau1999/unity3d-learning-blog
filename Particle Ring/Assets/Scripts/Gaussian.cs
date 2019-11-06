using System;
using Random = System.Random;

namespace ExtensionMethods
{
    public static class RandomExtension
    {
        public static float Gaussian(this Random r, float mu, float sigma)
        {
            float u, v, S;

            do
            {
                u = 2f * (float) r.NextDouble() - 1f;
                v = 2f * (float) r.NextDouble() - 1f;
                S = u * u + v * v;
            } while (S >= 1f);

            float fac = (float) Math.Sqrt(-2f * Math.Log(S) / S);
            return u * fac * sigma + mu;
        }
    }

    public static class FloatExtension
    {
        public static float Clip(this float f, float min, float max)
        {
            if (f < min) f = min;
            if (f > max) f = max;
            return f;
        }
    }
}