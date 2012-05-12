using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace GLOpenTKDemo
{

    class Shaders
    {

        private string SIMPLEVERTEX = "Resources\\shaders\\simple.vertex";
        private string SIMPLEFRAGMENT = "Resources\\shaders\\simple.fragment";
        private string[] ShaderSources;
        private int[] ShaderIds;
        public int[] ProgramIds;
        /*
         * Use the already initialized SIMPLEVERTEX or SIMPLEFRAGMENT (etc) string objects to load corresponding shader to program memory.
         * Returns empty string if fails.
         */

        public Shaders()
        {
            ShaderSources = new string[2];
            ShaderIds = new int[2];
            ProgramIds = new int[1];

        }

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
                //System.IO.FileStream stream = new System.IO.FileStream(@Path, System.IO.FileMode.Open);

                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                while (reader.EndOfStream != true)
                {
                    returnable += reader.ReadLine() + "\n";
                }
                //System.Console.WriteLine(returnable);
                stream.Close();
            }
            finally
            {
                stream.Close();
            }
            
            return returnable;
        }

        private void CreateProgram(int vertex, int fragment, int programid, bool UpdateProgram)
        {
            if (UpdateProgram == true)
                GL.DeleteProgram(ProgramIds[programid]);
            ErrorCode ErrorCheckValue = GL.GetError();
            ShaderIds[vertex] = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(ShaderIds[vertex], ShaderSources[vertex]);
            GL.CompileShader(ShaderIds[vertex]);

            ShaderIds[fragment] = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(ShaderIds[fragment], ShaderSources[fragment]);
            GL.CompileShader(ShaderIds[fragment]);

            ProgramIds[programid] = GL.CreateProgram();
            GL.AttachShader(ProgramIds[programid], ShaderIds[vertex]);
            GL.AttachShader(ProgramIds[programid], ShaderIds[fragment]);

            GL.LinkProgram(ProgramIds[programid]);
            GL.UseProgram(ProgramIds[programid]);

            ErrorCheckValue = GL.GetError();
            if (ErrorCheckValue != ErrorCode.NoError)
                System.Console.WriteLine("Error at Creating Shaders: " + ErrorCheckValue);
        }


        private void LoadFromFile()
        {
            ShaderSources[0] = LoadShaders(SIMPLEVERTEX);
            ShaderSources[1] = LoadShaders(SIMPLEFRAGMENT);
        }

        public void Initialize()
        {
            LoadFromFile();
            CreateProgram(0, 1, 0, false);

        }

        public void Update()
        {
            String[] uudet = new String[2];
            bool changed = false;
            uudet[0] = LoadShaders(SIMPLEVERTEX);
            uudet[1] = LoadShaders(SIMPLEFRAGMENT);
            if (uudet[0] != ShaderSources[0])
            {
                changed = true;
                ShaderSources[0] = uudet[0];
                GL.DeleteShader(ShaderIds[0]);
            }
            if (uudet[1] != ShaderSources[1]) {
                changed = true;
                ShaderSources[1] = uudet[1];
                GL.DeleteShader(ShaderIds[1]);
            }
            if (changed)
            {
                System.Console.WriteLine("Shaders: Updated program!");
                CreateProgram(0, 1, 0, true);
            }
            else
                System.Console.WriteLine("Shaders: No change in shader sources!");
        }
            
            

    }
}
