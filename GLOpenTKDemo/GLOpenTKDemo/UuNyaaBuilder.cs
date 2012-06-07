using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLOpenTKDemo
{
    class UuNyaaBuilder
    {
        private string filePath = "Resources\\UuNyaRef";
        private string[] source;
        private Uu uu;
        private Nya nya;
        private Random rand;

        public UuNyaaBuilder()
        {
            source = new string[61];
            uu = new Uu();
            nya = new Nya();
            rand = new Random(0);
        }

        private void loadFile()
        {
            string Path = "woot";
            Path = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            // Getting the directory where we ran the program so that
            // we can remove unnecessary directories and find the correct one
            if (Path.EndsWith("\\bin\\Debug"))
            {
                Path = Path.Replace("bin\\Debug", "");
            }
            else if (Path.EndsWith("\\bin\\Release"))
            {
                Path = Path.Replace("bin\\Release", "");
            }
            // Need to also remove shit from start of the string
            if (Path.StartsWith("file:\\"))
            {
                Path = Path.Replace("file:\\", "");
            }
            Path += filePath;

            System.IO.FileStream stream = new System.IO.FileStream(@Path, System.IO.FileMode.Open);
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                int x = 0;
                while (reader.EndOfStream != true)
                {
                    source[x++] = reader.ReadLine();
                }
                stream.Close();
            }
            finally
            {
                stream.Close();
            }
        }

        private void loadCoordsFromSource()
        {
            for (int i = 0; i < 25; i++)
            {
                for (int z = 0; z < source[i].Length; z++)
                {
                    if (source[i][z] == 'x')
                        nya.addCoord(z, i);
                }
            }
            for (int i = 32; i < source.Length - 1; i++)
            {
                for (int x = 0; x < source[i].Length; x++)
                {
                    if (source[i][x] == 'x')
                        uu.addCoord(x, i - 32);
                }
            }
        }

        private float getCoordX(float x)
        {
            float ret = 0.0f;
            ret = (x * 1.0f) * (0.0033f) - 0.7f;
            return ret;
        }

        private float getCoordY(float y)
        {
            float ret = 0.0f;

            ret = (y * 1.0f) * (-0.0033f) + 0.2f;

            return ret;
        }
        public Scene sceneBuilder()
        {
            loadFile();
            loadCoordsFromSource();
            UuNya[] lista = new UuNya[Math.Max(uu.count, nya.count) * 2];
            int count = 0;

            for (int i = 0; i < uu.count; i++)
            {
                for (int k = 0; k < nya.count; k++)
                {
                    if (nya.z_coords[k] != -1 && uu.y_coords[i] == nya.y_coords[k] && uu.x_coords[i] != -1)
                    {
                        lista[count++] = new UuNya(uu.x_coords[i], uu.y_coords[i], nya.z_coords[k]);
                        uu.x_coords[i] = -1;
                        uu.y_coords[i] = -1;
                        nya.z_coords[k] = -1;
                        nya.y_coords[k] = -1;
                    }
                }
            }
            for (int i = 0; i < uu.count; i++)
            {
                if (uu.x_coords[i] != -1)
                {
                    lista[count++] = new UuNya(uu.x_coords[i], uu.y_coords[i], 1);
                    uu.x_coords[i] = -1;
                    uu.y_coords[i] = -1;
                }
            }
            for (int i = 0; i < nya.count; i++)
            {
                if (nya.z_coords[i] != -1)
                {
                    lista[count++] = new UuNya(1, nya.y_coords[i], nya.z_coords[i]);
                    nya.z_coords[i] = -1;
                    nya.y_coords[i] = -1;
                }
            }
            //for (int i = 0; i < uu.count; i++)
            Scene scene = new Scene(count);
            scene.setBackground(new VBOCube(0, -10.0f, 0, 100.0f, 1, true));
            scene.getBackground().altColors();

            for (int i = 0; i < count; i++)
                scene.addNewVBOCube(new VBOCube(getCoordX(lista[i].x), getCoordY(lista[i].y), getCoordX(lista[i].z) + 1.5f, 0.0035f, 4, new UuNya(getCoordX(lista[i].x), getCoordY(lista[i].y), 2.0f), new UuNya(getCoordX(lista[i].z), getCoordY(lista[i].y), 2.0f) ,genRandCoords()));


            return scene;
        }

        public UuNya genRandCoords()
        {
            return new UuNya((((float)rand.NextDouble()) - 0.5f) * 2.0f, (((float)rand.NextDouble()) - 0.5f) * 2.0f, (((float)rand.NextDouble()) + 2.5f) * 2.0f);
        }
    }
}
