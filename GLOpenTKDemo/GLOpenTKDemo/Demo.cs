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
        int modelLocation;
        int viewLocation;
        int projectionLocation;
        int timeLocation;
        int resoLocation;
        Vector2 resolution; 
        Matrix4 ModelMatrix;
        Matrix4 ViewMatrix;
        Matrix4 ProjectionMatrix;
        private int VaoId, VboId, ColorBufferId;
        private Shaders shader;
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Demo()
            : base(1280, 720, new GraphicsMode(32, 24, 8, 0), "The Demo Effect", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.Debug)
        {
            VSync = VSyncMode.On;
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Compose a string that consists of three lines.
            shader = new Shaders();


            ErrorCode ErrorCheckValue = GL.GetError();
            GL.ClearColor(0.33f, 0.2f, 0.5f, 0.25f);
            GL.Enable(EnableCap.DepthTest);
            ModelMatrix = Matrix4.Identity;
            ViewMatrix = Matrix4.Identity;
            ProjectionMatrix = Matrix4.Identity;
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);

            shader.Initialize();
            ErrorCheckValue = GL.GetError();
            if (ErrorCheckValue != ErrorCode.NoError)
                Trace.WriteLine("Error at Creating Shaders: " + ErrorCheckValue);

            // Creating a VBO object now, so that ugly GL.Begin() code can fly to space.
            float[] Vertices = {
           -.5f, -.5f,  .5f, 1 , 
           -.5f,  .5f,  .5f, 1,
            .5f,  .5f,  .5f, 1 , 
            .5f, -.5f,  .5f, 1 , 
           -.5f, -.5f, -.5f, 1 , 
           -.5f,  .5f, -.5f, 1 ,
            .5f,  .5f, -.5f, 1 , 
            .5f, -.5f, -.5f, 1 , 


          };

        float[] Colors = {
         0, 0, 1, 1 ,
         1, 0, 0, 1,
         0, 1, 0, 1,
         1, 1, 0, 1 ,
         1, 1, 1, 1,
         1, 0, 0, 1,
         1, 0, 1, 1,
         0, 0, 1, 1
    };

            uint[] indices = {
            0,2,1, 0,3,2,
            4,3,0, 4,7,3,
            4,1,5, 4,0,1,
            3,6,2, 3,7,6,
            1,6,5, 1,2,6,
            7,5,6, 7,4,5
        };
            
            modelLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ModelMatrix");
            viewLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ViewMatrix");
            projectionLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ProjectionMatrix");
            timeLocation = GL.GetUniformLocation(shader.ProgramIds[0], "time");
            resoLocation = GL.GetUniformLocation(shader.ProgramIds[0], "resolution");

            resolution = new Vector2(1280, 720);

            GL.GenVertexArrays(1, out VaoId);
            GL.BindVertexArray(VaoId);
            //derp cubeg
            GL.GenBuffers(1, out VboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * sizeof(float)), Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            //our pretty colours
            GL.GenBuffers(1, out ColorBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ColorBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(float)), indices, BufferUsageHint.StaticDraw);
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

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width/(float)Height, 1.0f, 90.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            ProjectionMatrix = projection;

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
            if (Keyboard[Key.F5])
                shader.Update();
            if (Keyboard[Key.H])//left
            {
                Matrix4 heippa = ModelMatrix;
                Matrix4 heippa2 = Matrix4.CreateRotationY(0.2f);
                Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
            }
            if (Keyboard[Key.L]) //right
            {
                Matrix4 heippa = ModelMatrix;
                Matrix4 heippa2 = Matrix4.CreateRotationY(-0.2f);
                Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
            }
            if (Keyboard[Key.J]) //up
            {
                Matrix4 heippa = ModelMatrix;
                Matrix4 heippa2 = Matrix4.CreateRotationX(0.2f);
                Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
            }
            if (Keyboard[Key.K]) //down
            {
                Matrix4 heippa = ModelMatrix;
                Matrix4 heippa2 = Matrix4.CreateRotationX(-0.2f);
                Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
            }
            if (Keyboard[Key.U]) //down
            {
                Matrix4 heippa = ModelMatrix;
                Matrix4 heippa2 = Matrix4.CreateRotationZ(0.2f);
                Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
            }
            if (Keyboard[Key.I]) //down
            {
                Matrix4 heippa = ModelMatrix;
                Matrix4 heippa2 = Matrix4.CreateRotationZ(-0.2f);
                Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
            }
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.UseProgram(shader.ProgramIds[0]);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 modelview = Matrix4.CreateTranslation(new Vector3(0, 0, -2));
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            ViewMatrix = modelview;
            float time = System.DateTime.Now.Millisecond * 0.01f;
            GL.Uniform1(timeLocation, 1, ref time);
            GL.Uniform2(resoLocation, ref resolution);
            GL.UniformMatrix4(modelLocation, false, ref ModelMatrix);
            GL.UniformMatrix4(viewLocation, false, ref ViewMatrix);
            GL.UniformMatrix4(projectionLocation, false, ref ProjectionMatrix);

           


            //System.Console.WriteLine(time);
            GL.DrawElements(BeginMode.Triangles, 36,DrawElementsType.UnsignedInt, 0);
            /*
            GL.Begin(BeginMode.Triangles);

            GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
            GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

            GL.End();
            */

            GL.UseProgram(0);
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
