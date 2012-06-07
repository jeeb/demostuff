using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLOpenTKDemo
{
    class Nya
    {
        public int[] z_coords;
        public int[] y_coords;
        public int count;

        public Nya()
        {
            z_coords = new int[10000];
            y_coords = new int[10000];
        }

        public void addCoord(int z, int y)
        {
            z_coords[count] = z;
            y_coords[count] = y;
            count++;
        }
    }
}
