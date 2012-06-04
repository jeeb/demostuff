using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLOpenTKDemo
{
    class Scene
    {
        private VBOCube[] objects;
        private VBOCube background;
        private int objectCount;
        public Scene(int objectCount)
        {
            objects = new VBOCube[objectCount];
        }

        public void setBackground(VBOCube background)
        {
            this.background = background;
            background.loadToGpu();
        }

        public void addNewVBOCube(VBOCube cube)
        {
            objects[objectCount++] = cube;
            cube.loadToGpu();
        }

        public int getCount()
        {
            return objectCount;
        }
        public VBOCube[] getObjects()
        {
            return objects;
        }

        public VBOCube getBackground()
        {
            return background;
        }
    }
}
