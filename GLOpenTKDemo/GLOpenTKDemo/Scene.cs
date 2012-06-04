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
        private bool initialized;
        public Scene(int objectCount)
        {
            objects = new VBOCube[objectCount];
            initialized = false;
        }

        public void initialize()
        {
            background.loadToGpu();
            //hullu foreach, ihme syntaxi
            for (int i = 0; i < objectCount;i++ )
                objects[i].loadToGpu();
            initialized = true;
        }

        public bool isInit()
        {
            return initialized;
        }

        public void setBackground(VBOCube background)
        {
            this.background = background;
            //background.loadToGpu();
        }

        public void addNewVBOCube(VBOCube cube)
        {
            objects[objectCount++] = cube;
            //cube.loadToGpu();
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
