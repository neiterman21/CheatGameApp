using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheatGameApp
{
    public static class MathHelper
    {
        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(max, Math.Max(value, min));
        }
    }
}
