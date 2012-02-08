using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLOpenTKDemo
{

    class Shaders
    {
        private string SIMPLEVERTEX = "Resources\\shaders\\simple.vertex";
        private string SIMPLEFRAGMENT = "Resources\\shaders\\simple.fragment";
        private string[] ShaderSources;
        /*
         * Use the already initialized SIMPLEVERTEX or SIMPLEFRAGMENT (etc) string objects to load corresponding shader to program memory.
         * Returns empty string if fails.
         */
        private string LoadShaders(string SHADER)
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
            Path += SHADER;
            //System.Console.WriteLine( Path );

            string returnable = "";
            System.IO.FileStream stream = new System.IO.FileStream(@Path, System.IO.FileMode.Open);
            // I wonder what will happen if this fails :D
            try
            {

                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                while (reader.EndOfStream != true)
                {
                    returnable += reader.ReadLine() + "\r\n";
                }
                //System.Console.WriteLine(returnable);
            }
            finally
            {
                stream.Close();
            }
            
            return returnable;
        }

        public void test()
        {
            LoadShaders(SIMPLEVERTEX);
            LoadShaders(SIMPLEFRAGMENT);
        }

    }
}
