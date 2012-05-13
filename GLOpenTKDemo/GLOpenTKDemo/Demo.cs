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
        int fps;
        long time;
        Graphics graphics;
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Demo()
            : base(1280, 720, new GraphicsMode(32, 24, 8, 0), "The Demo Effect", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.Debug)
        {
            VSync = VSyncMode.Off;
            fps = 0;
            time = System.DateTime.Now.Millisecond + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Minute * 60000);
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            graphics = new Graphics();
            graphics.Initialize();
            // Compose a string that consists of three lines.

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

            graphics.onResize(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height, Width, Height);
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
            if (Keyboard[Key.F5])
                graphics.shaderUpdate();
            if (Keyboard[Key.F])
                WindowState = WindowState.Fullscreen;
            if (Keyboard[Key.W])
                WindowState = WindowState.Normal;
            if (Keyboard[Key.H])//left
            {
                graphics.rotateObjectByY(0.2f);
            }
            if (Keyboard[Key.L]) //right
            {
                graphics.rotateObjectByY(-0.2f);
            }
            if (Keyboard[Key.J]) //up
            {
                graphics.rotateObjectByX(0.2f);
            }
            if (Keyboard[Key.K]) //down
            {
                graphics.rotateObjectByX(-0.2f);
            }
            if (Keyboard[Key.U])
            {
                graphics.rotateObjectByZ(0.2f);
            }
            if (Keyboard[Key.I])
            {
                graphics.rotateObjectByZ(-0.2f);
            }


            if (Keyboard[Key.Up])//left
            {
                graphics.moveCameraBy(0.0f, 0.2f, 0.0f);
            }
            if (Keyboard[Key.Down]) //right
            {
                graphics.moveCameraBy(0.0f, -0.2f, 0.0f);
            }
            if (Keyboard[Key.Right]) //up
            {
                graphics.moveCameraBy(0.2f, 0.0f, 0.0f);
            }
            if (Keyboard[Key.Left]) //down
            {
                graphics.moveCameraBy(-0.2f, 0.0f, 0.0f);
            }
            if (Keyboard[Key.N])
            {
                graphics.moveCameraBy(0.0f, 0.0f, 0.2f);
            }
            if (Keyboard[Key.M])
            {
                graphics.moveCameraBy(0.0f, 0.0f, -0.2f);
            }
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
    
            base.OnRenderFrame(e);
            graphics.Render();
            SwapBuffers();
            fps++;
            long now = System.DateTime.Now.Millisecond + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Minute * 60000);
            if (now - time > 1000)
            {
                base.Title = "DemoEffect  Fps: " + fps + " frames per second.";
                fps = 0;
                time = now;
            }
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
