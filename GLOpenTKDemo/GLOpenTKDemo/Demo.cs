using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System.Diagnostics;

namespace GLOpenTKDemo
{
    class Demo : GameWindow
    {
        private int VaoId, VboId, ColorBufferId, VertexShaderId, FragmentShaderId, ProgramId;
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Demo()
            : base(1280, 720, new GraphicsMode(32, 24, 8, 0), "Rendering a Fabulous Triangle^WTorus", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.Debug)
        {
            VSync = VSyncMode.On;
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ErrorCode ErrorCheckValue = GL.GetError();
            GL.ClearColor(0.33f, 0.2f, 0.5f, 0.25f);
            GL.Enable(EnableCap.DepthTest);

            VertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            //String vertex = Properties.Resources.simplevertex;
            //System.Console.WriteLine(vertex);
            GL.ShaderSource(VertexShaderId, Properties.Resources.simplevertex);
            GL.CompileShader(VertexShaderId);

            FragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShaderId, Properties.Resources.simplefragment);
            GL.CompileShader(FragmentShaderId);

            ProgramId = GL.CreateProgram();
            GL.AttachShader(ProgramId, VertexShaderId);
            GL.AttachShader(ProgramId, FragmentShaderId);


            GL.LinkProgram(ProgramId);
            GL.UseProgram(ProgramId);
            ErrorCheckValue = GL.GetError();
            if (ErrorCheckValue != ErrorCode.NoError)
                Trace.WriteLine("Error at Creating Shaders: " + ErrorCheckValue);

            // Creating a VBO object now, so that ugly GL.Begin() code can fly to space.
            // Besides this needs to be done only once, those loops slowing my opengl lol. Unlimited FPS BOOST!
            // This is still awesome to create torus algorithmically :O! We will be doing more of this.
            // Koura, where is that genNiceBoat() algorithm .3
            // Unlimited commenting works.
            float[] Vertices = {
        -0.4f, -0.4f, 0.0f, 1.0f,
         0.0f, 0.4f, 0.0f, 1.0f,
         0.4f, -0.4f, 0.0f, 1.0f
    };
            float[] Colors = {
        1.0f, 0.0f, 0.0f, 1.0f,
        0.0f, 1.0f, 0.2f, 1.0f,
        0.2f, 0.0f, 1.0f, 1.0f
    };
            /*
            int lol = 0;
            int numc = 50;
            int numt = 50;

            int i, j, k;
            double s, t, x, y, z, twopi;
            float[] Vertices = new float[numc * (numt+1) * 4 * 2];
            float[] Colors = new float[numc * (numt+1) * 4 * 2];

            twopi = 2 * Math.PI;
            for (i = 0; i < numc; i++)
            {
                //GL.Begin(BeginMode.QuadStrip);
                for (j = 0; j <= numt; j++)
                {
                    for (k = 1; k >= 0; k--)
                    {
                        ++lol;
                        s = (i + k) % numc + 0.5;
                        t = j % numt;

                        x = (1 + .1 * Math.Cos(s * twopi / numc)) * Math.Cos(t * twopi / numt);
                        y = (1 + .1 * Math.Cos(s * twopi / numc)) * Math.Sin(t * twopi / numt);
                        z = .1 * Math.Sin(s * twopi / numc) + 4;

                        Colors[((i + 1) * (j + 1) * 4 * (k + 1)) - 4] = 0.2f;
                        Colors[((i + 1) * (j + 1) * 4 * (k + 1) + 1) - 4] = 0.9f;
                        Colors[((i + 1) * (j + 1) * 4 * (k + 1) + 2) - 4] = 1.0f;
                        Colors[((i + 1) * (j + 1) * 4 * (k + 1) + 3) - 4] = 1.0f;
                        //GL.Color3(0.2f, 0.9f, 1.0f);
                        Vertices[((i + 1) * (j + 1) * 4 * (k + 1)) - 4] = (float)x;
                        Vertices[((i + 1) * (j + 1) * 4 * (k + 1) + 1) - 4] = (float)y;
                        Vertices[((i + 1) * (j + 1) * 4 * (k + 1) + 2) - 4] = (float)z;
                        Vertices[((i + 1) * (j + 1) * 4 * (k + 1) + 3) - 4] = 1.0f;
                        //GL.Vertex3(x, y, z);
                    }
                }
                //GL.End();
            }*/
            //System.Console.WriteLine("Torus had " + (lol*4) + " vertices. Expected " + (200*201*4*2) + " vertices.");
            //System.Console.WriteLine("Initialized these: Vertices.length " + Vertices.Length + ".");
            

            GL.GenVertexArrays(1, out VaoId);
            GL.BindVertexArray(VaoId);

            GL.GenBuffers(1, out VboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * sizeof(float)), Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.GenBuffers(1, out ColorBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Colors.Length * sizeof(float)), Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            ErrorCheckValue = GL.GetError();
            if (ErrorCheckValue != ErrorCode.NoError)
                Trace.WriteLine("Error at Creating VBO: " + ErrorCheckValue);
        }

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard[Key.Escape])
                Exit();
            if (Keyboard[Key.Left])
                Exit();
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.DrawArrays(BeginMode.Triangles, 0, 3*4);
            /*
            GL.Begin(BeginMode.Triangles);

            GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
            GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

            GL.End();
            */


            SwapBuffers();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Demo game = new Demo())
            {
                game.Run(30.0);
            }
        }
    }
}
