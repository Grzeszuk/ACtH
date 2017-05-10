using System;
using System.Collections.Generic;
using System.Text;

namespace Draw.Randomize_Controller
{
    public static class RandomController
    {
        public static int Next(int max,int min)
        {
            return new Random().Next(min,max);
        }
    }
}
