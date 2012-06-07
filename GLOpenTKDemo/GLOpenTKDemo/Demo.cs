using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

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
        SoundPlayer player = new SoundPlayer();
        string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        bool stupid = false;
        Random randNum;
        int fps;
        long time;
        long tick;
        float speed;
        Graphics graphics;
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Demo()
            : base(1280, 720, new GraphicsMode(32, 24, 8, 0), "The Demo Effect", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.Debug)
        {
            VSync = VSyncMode.On;
            fps = 0;
            tick = 0;
            speed = 0.0f;
            randNum = new Random(1);
            time = System.DateTime.Now.Millisecond + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Minute * 60000);
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            graphics = new Graphics();
            graphics.Initialize(); 
            player.SoundLocation = path + @"\demostep.wav";
            player.Load();
            player.Play();
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
            {
                switch (WindowState)
                {
                    case WindowState.Fullscreen:
                        WindowState = WindowState.Normal;
                        break;
                    default:
                        WindowState = WindowState.Fullscreen;
                        break;
                }
            }
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

            if (Keyboard[Key.Number4])//left
            {
                graphics.rotateCameraByY(0.2f);
            }
            if (Keyboard[Key.Number6]) //right
            {
                graphics.rotateCameraByY(-0.2f);
            }
            if (Keyboard[Key.Number8]) //up
            {
                graphics.rotateCameraByX(0.2f);
            }
            if (Keyboard[Key.Number2]) //down
            {
                graphics.rotateCameraByX(-0.2f);
            }
            if (Keyboard[Key.Number7])
            {
                graphics.rotateCameraByZ(0.2f);
            }
            if (Keyboard[Key.Number9])
            {
                graphics.rotateCameraByZ(-0.2f);
            }
            if (Keyboard[Key.Q])
            {
                graphics.addCube(0.7f, 0, 8,0.5f,1);
            }
            if (Keyboard[Key.R])
            {
                graphics.moveObjectOnX(-0.02f);
            }
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            
            base.OnRenderFrame(e);
            /*
            double fun = randNum.Next(100);
            
            if (tick == 60*6)
            {
                speed = 0.0011f;
                System.Console.WriteLine("changed speed to 0.002f");
            } 
            else if (tick == 60*20)
            {
                speed = 0.0003f;
                tick = 0;
                System.Console.WriteLine("changed speed to 0.001f");
            }

            graphics.rotateObjectByX((float)fun * (0.001f + speed));
            fun = randNum.Next(100);
            graphics.rotateObjectByX((float)fun * (0.001f + speed));
            fun = randNum.Next(100);
            graphics.rotateObjectByZ((float)fun * (0.001f + speed));*/


            if (tick > 60 * 32)
            {
                Exit();
            }
            else if (tick > 60 * 30)
            {
                graphics.Render(3);
            }

            else if (tick > 60 * 27)
            {
                graphics.Render(4);
                graphics.rotateObjectByY(-0.11f);
                graphics.rotateObjectByZ(0.01f);
                graphics.rotateObjectByX(-0.1f);
            }
            else if (tick > 60 * 25)
            {
                graphics.Render(4);
                graphics.rotateObjectByY(0.07f);
                graphics.rotateObjectByZ(0.01f);
            }
            else if (tick > 60 * 23)
            {
                graphics.Render(4);
                graphics.rotateObjectByY(-0.05f);
            }
            else if (tick == 60 * 22 && !stupid)
            {
                graphics.addCube(0, 0, -2, 0.5f, 0);
                stupid = true;
            }
            else if (tick == 60 * 21)
            {
                stupid = false;
            }
            else if (tick == 60 * 20.4 && !stupid)
            {
                graphics.destroyCube();
                graphics.destroyCube();
                stupid = true;
            }
            else if (tick > 60 * 16)
            {
                graphics.Render(2);
            }
            else if (tick > 60 * 4)
            {
                graphics.Render(6);
                graphics.animateUuNyaa();
            }
            else if (tick > 0)
            {
                graphics.Render(5);
            }
            //graphics.Render(2);
            SwapBuffers();
            tick++;

            fpsCounter();
        }

        protected void fpsCounter()
        {
            fps++;
            long now = System.DateTime.Now.Millisecond + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Minute * 60000);
            if (now - time > 1000)
            {
                base.Title = "DemoEffect  Fps: " + fps + " frames per second. Average draw time: " + (1000.0 / (double)fps) + " ms";
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
