using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLOpenTKDemo
{
    class Uu
    {
        public int[] x_coords;
        public int[] y_coords;
        public int count;

        public Uu()
        {
            x_coords = new int[10000];
            y_coords = new int[10000];
        }

        public void addCoord(int x, int y)
        {
            x_coords[count] = x;
            y_coords[count] = y;
            count++;
        }
    }
}
