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
        private string ADVFRAGMENT = "Resources\\shaders\\adv.fragment";
        private string ADV2FRAGMENT = "Resources\\shaders\\adv2.fragment";
        private string ADV2VERTEX = "Resources\\shaders\\adv2.vertex";
        private string ADVVertex = "Resources\\shaders\\adv.vertex";
        private string TunnelVertex = "Resources\\shaders\\tunnel.vertex";
        private string TunnelFragment = "Resources\\shaders\\tunnel.fragment";
        private string NoisyVertex = "Resources\\shaders\\noisy.vertex";
        private string NoisyFragment = "Resources\\shaders\\noisy.fragment";
        private string[] ShaderSources;
        private int[] ShaderIds;
        public int[] ProgramIds;
        private int shaderCount, programCount;
        /*
         * Use the already initialized SIMPLEVERTEX or SIMPLEFRAGMENT (etc) string objects to load corresponding shader to program memory.
         * Returns empty string if fails.
         */

        public Shaders()
        {

            shaderCount = 10;
            programCount = 7;
            ShaderSources = new string[shaderCount];
            ShaderIds = new int[shaderCount];
            ProgramIds = new int[programCount];
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

            //VertexShader
            ShaderIds[vertex] = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(ShaderIds[vertex], ShaderSources[vertex]);
            GL.CompileShader(ShaderIds[vertex]);

            //Fragment Shader
            ShaderIds[fragment] = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(ShaderIds[fragment], ShaderSources[fragment]);
            GL.CompileShader(ShaderIds[fragment]);

            //Putting them together as program
            ProgramIds[programid] = GL.CreateProgram();
            GL.AttachShader(ProgramIds[programid], ShaderIds[vertex]);
            GL.AttachShader(ProgramIds[programid], ShaderIds[fragment]);

            //Linking and telling we use it... actually no.
            GL.LinkProgram(ProgramIds[programid]);
            //GL.UseProgram(ProgramIds[programid]);

            ErrorCheckValue = GL.GetError();
            if (ErrorCheckValue != ErrorCode.NoError)
                System.Console.WriteLine("Error at Creating Shaders: " + ErrorCheckValue);
        }


        private void LoadFromFile()
        {
            ShaderSources[0] = LoadShaders(SIMPLEVERTEX);
            ShaderSources[1] = LoadShaders(SIMPLEFRAGMENT);
            ShaderSources[2] = LoadShaders(ADVVertex);
            ShaderSources[3] = LoadShaders(ADVFRAGMENT);
            ShaderSources[4] = LoadShaders(ADV2FRAGMENT);
            ShaderSources[5] = LoadShaders(TunnelVertex);
            ShaderSources[6] = LoadShaders(TunnelFragment);
            ShaderSources[7] = LoadShaders(NoisyVertex);
            ShaderSources[8] = LoadShaders(NoisyFragment);
            ShaderSources[9] = LoadShaders(ADV2VERTEX);
            shaderCount = 10;
        }

        public void Initialize()
        {
            LoadFromFile();
            CreateProgram(0, 1, 0, false);
            CreateProgram(0, 4, 1, false);
            CreateProgram(2, 4, 2, false);
            CreateProgram(5, 6, 3, false);
            CreateProgram(2, 1, 4, false); // elämuuta
            CreateProgram(7, 8, 5, false);
            CreateProgram(9, 1, 6, false);
        }

        public void Update()
        {
            String[] uudet = new String[shaderCount];
            bool changed = true;
            uudet[0] = LoadShaders(SIMPLEVERTEX);
            uudet[1] = LoadShaders(SIMPLEFRAGMENT);
            uudet[2] = LoadShaders(ADVVertex);
            uudet[3] = LoadShaders(ADVFRAGMENT);
            uudet[4] = LoadShaders(ADV2FRAGMENT);
            uudet[5] = LoadShaders(TunnelVertex);
            uudet[6] = LoadShaders(TunnelFragment);
            uudet[7] = LoadShaders(NoisyVertex);
            uudet[8] = LoadShaders(NoisyFragment);
            for (int i = 0; i < shaderCount; i++)
            {
                if (uudet[i] != ShaderSources[i])
                {
                    changed = true;
                    ShaderSources[i] = uudet[i];
                    GL.DeleteShader(ShaderIds[i]);
                }
            }
            /*if (uudet[1] != ShaderSources[1]) {
                changed = true;
                ShaderSources[1] = uudet[1];
                GL.DeleteShader(ShaderIds[1]);
            }*/
            if (changed)
            {
                System.Console.WriteLine("Shaders: Updated program!");
                CreateProgram(0, 1, 0, true);
                CreateProgram(0, 4, 1, true);
                CreateProgram(2, 4, 2, true);
                CreateProgram(5, 6, 3, true);
                CreateProgram(2, 1, 4, true);
                CreateProgram(7, 8, 5, true);
                CreateProgram(2, 1, 6, true);
            }
            else
                System.Console.WriteLine("Shaders: No change in shader sources!");
        }
    }
}
