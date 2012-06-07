using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace GLOpenTKDemo
{
    class Graphics
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
        int offset;
        float timah;
        private Scene[] scenes;
        private Shaders shader;
        private int currentSceneId;
        

        public Graphics(){}
      
        public void Initialize()
        {
            timah = 0;
            shader = new Shaders();
            currentSceneId = 0;

            ErrorCode ErrorCheckValue = GL.GetError();
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
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

            //Binding UniformLocations to variables found in shader files
            modelLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ModelMatrix");
            viewLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ViewMatrix");
            projectionLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ProjectionMatrix");
            timeLocation = GL.GetUniformLocation(shader.ProgramIds[0], "time");
            resoLocation = GL.GetUniformLocation(shader.ProgramIds[0], "resolution");

            resolution = new Vector2(1280, 720);

            scenes = new Scene[10];
            scenes[0] = new Scene(1000);

            // Creating a VBO object now, and after it Indexed vb. In otherwords ElementBuffer
            scenes[0].setBackground(new VBOCube(0, 0, 0, 10.0f, 0, true));
            //scenes[0].addNewVBOCube(new VBOCube(0, 0, 0, 10.0f, 0, true));
            //scenes[0].addNewVBOCube(new VBOCube(0, 0, 0, 0.5f, 0));
            //scenes[0].addNewVBOCube(new VBOCube(0.5f,  0.5f, 0, 0.5f, 0));
            //scenes[0].addNewVBOCube(new VBOCube(0, -0.5f, 0, 0.5f, 0));
            initializeScene(0);

            scenes[1] = new Scene(10);
            scenes[1].setBackground(new VBOCube(0, 0, 0, 10.0f, 1, true));
            initializeScene(1);


            //scenes[5] = new Scene(10000);
            //scenes[5].setBackground(new VBOCube(0, 0, 0, 10.0f, 2, true));

            scenes[2] = new Scene(10);
            scenes[2].setBackground(new VBOCube(0, 0, 0, 10.0f, 3, true));
            scenes[2].addNewVBOCube(new VBOCube(0.7f, 0, 0, 2.0f, 2));
            scenes[2].addNewVBOCube(new VBOCube(-0.7f, 0, 0, 2.0f, 2));
         
            initializeScene(2);

            //JEEB!
            scenes[3] = new Scene(0); //monta cubea sul on maksimissaan, ei kasva automaattisesti ku lisäät
            scenes[3].setBackground(new VBOCube(0, 0, 0, 10.0f, 5, true)); //"5" on sinun shader program
            initializeScene(3);


            UuNyaaBuilder builder = new UuNyaaBuilder();
            scenes[5] = builder.sceneBuilder();
            initializeScene(5);


            ErrorCheckValue = GL.GetError();
            if (ErrorCheckValue != ErrorCode.NoError)
                Trace.WriteLine("Error at Creating VBO: " + ErrorCheckValue);


            Matrix4 modelview = Matrix4.CreateTranslation(new Vector3(0, 0, -2));
            ViewMatrix = modelview;
        }


        public void onResize(int a, int b, int c, int d, int x, int y)
        {
            GL.Viewport(a, b, c, d);
            resolution = new Vector2(x/2, y/2);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, x / (float)y, 0.2f, 90.0f);
            ProjectionMatrix = projection;
        }

        public void Render(int sceneId)
        {
            currentSceneId = sceneId;

            //float time = System.DateTime.Now.Millisecond * 0.001f;
            timah += 0.02f;
            //System.Console.WriteLine(timah);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //Piirretään tausta
            GL.UseProgram(shader.ProgramIds[scenes[currentSceneId].getBackground().getShaderProgramId()]);
            updateShaderStuff(scenes[currentSceneId].getBackground().getShaderProgramId());

            GL.Uniform1(timeLocation, 1, ref timah);
            GL.Uniform2(resoLocation, ref resolution);
            GL.UniformMatrix4(viewLocation, false, ref ViewMatrix);
            GL.UniformMatrix4(projectionLocation, false, ref ProjectionMatrix);

            Matrix4 MVMatrix = scenes[currentSceneId].getBackground().getModelViewMatrix();
            GL.UniformMatrix4(modelLocation, false, ref MVMatrix);
            scenes[currentSceneId].getBackground().draw();

            //Piirretään kaikki scenen objectit
            for (int i = 0; i < scenes[currentSceneId].getCount(); i++)
            {
                GL.UseProgram(shader.ProgramIds[scenes[currentSceneId].getObjects()[i].getShaderProgramId()]);
                updateShaderStuff(scenes[currentSceneId].getObjects()[i].getShaderProgramId());

                GL.Uniform1(timeLocation, 1, ref timah);
                GL.Uniform2(resoLocation, ref resolution);
                GL.UniformMatrix4(viewLocation, false, ref ViewMatrix);
                GL.UniformMatrix4(projectionLocation, false, ref ProjectionMatrix);

                
                MVMatrix = scenes[currentSceneId].getObjects()[i].getModelViewMatrix();
                GL.UniformMatrix4(modelLocation, false, ref MVMatrix);
                scenes[currentSceneId].getObjects()[i].draw();
            }

            GL.UseProgram(0);
        }

        public void addCube(float x, float y, float z, float scale, int shaderProgramId)
        {
            scenes[currentSceneId].addNewVBOCube(new VBOCube(x, y, z, scale, shaderProgramId));
        }

        public void rotateObjectByX(float x)
        {
            //ModelMatrix = Matrix4.Mult(ModelMatrix, Matrix4.CreateRotationX(x));
            for (int i = 0; i < scenes[currentSceneId].getCount(); i++)
            {
                scenes[currentSceneId].getObjects()[i].setViewMatrix(Matrix4.Mult(scenes[currentSceneId].getObjects()[i].getViewMatrix(), Matrix4.CreateRotationX(x)));
            }
        }

        public void rotateObjectByY(float y)
        {
            for (int i = 0; i < scenes[currentSceneId].getCount(); i++)
            {
                scenes[currentSceneId].getObjects()[i].setViewMatrix(Matrix4.Mult(scenes[currentSceneId].getObjects()[i].getViewMatrix(), Matrix4.CreateRotationY(y)));
            }
        }

        public void rotateObjectByZ(float z)
        {
            for (int i = 0; i < scenes[currentSceneId].getCount(); i++)
            {
                scenes[currentSceneId].getObjects()[i].setViewMatrix(Matrix4.Mult(scenes[currentSceneId].getObjects()[i].getViewMatrix(), Matrix4.CreateRotationZ(z)));
            }
        }

        public void shaderUpdate()
        {
            shader.Update();
        }

        public void moveCameraBy(float x, float y, float z)
        {
            Matrix4 heh = Matrix4.CreateTranslation(new Vector3(x, y, z));
            Matrix4 asd = ViewMatrix;
            ViewMatrix = Matrix4.Mult(asd, heh);
        }

        public void rotateCameraByX(float x)
        {
            Matrix4 heh = Matrix4.CreateRotationX(x);
            Matrix4 asd = ViewMatrix;
            Matrix4.Mult(ref asd, ref heh, out ViewMatrix);
        }
        public void rotateCameraByY(float y)
        {
            ViewMatrix = Matrix4.Mult(ViewMatrix, Matrix4.CreateRotationY(y));
        }
        public void rotateCameraByZ(float z)
        {
            Matrix4 heh = Matrix4.CreateRotationZ(z);
            Matrix4 asd = ViewMatrix;
            ViewMatrix = Matrix4.Mult(asd, heh);
        }

        public void moveObjectOnX(float constant_x)
        {
            for (int i = 0; i < 1; i++)
            {
                scenes[currentSceneId].getObjects()[i].setModelMatrix(Matrix4.Mult(Matrix4.CreateTranslation(new Vector3(constant_x, 0, 0)), scenes[currentSceneId].getObjects()[i].getModelMatrix()));
            }
        }

        public void initializeScene(int ID)
        {
            if (!scenes[ID].isInit())
                scenes[ID].initialize();
        }

        public void updateShaderStuff(int ID)
        {
            modelLocation = GL.GetUniformLocation(shader.ProgramIds[ID], "ModelMatrix");
            viewLocation = GL.GetUniformLocation(shader.ProgramIds[ID], "ViewMatrix");
            projectionLocation = GL.GetUniformLocation(shader.ProgramIds[ID], "ProjectionMatrix");
            timeLocation = GL.GetUniformLocation(shader.ProgramIds[ID], "time");
            resoLocation = GL.GetUniformLocation(shader.ProgramIds[ID], "resolution");
        }

        public void createUu() 
        {
            for (int i = 0; i < scenes[currentSceneId].getObjects().Length; i++)
            {
                scenes[currentSceneId].getObjects()[i].moveTowards(0,0.1f);
            }
        }
        public void createNya()
        {
            for (int i = 0; i < scenes[currentSceneId].getObjects().Length; i++)
            {
                scenes[currentSceneId].getObjects()[i].moveTowards(1, 0.1f);
            }
        }
        public void createRand()
        {
            for (int i = 0; i < scenes[currentSceneId].getObjects().Length; i++)
            {
                scenes[currentSceneId].getObjects()[i].moveTowards(2, 0.01f);
            }
        }
        long tick = 0;
        public void animateUuNyaa()
        {
            tick++;
            if (tick > 60 * 8)
            {
                tick = 0;
            }
            if (tick > 60 * 6)
            {
                createNya();
            }
            else if (tick > 60 * 4)
            {
                createRand();
            }
            else if (tick > 60 * 2)
            {
                createUu();
                //graphics.Render(2);
            }
            else if (tick > 0)
            {
                createRand();
                //graphics.Render(5);
            }
        }
    }
}
