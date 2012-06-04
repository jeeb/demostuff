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
        int count;
        private Scene[] scenes;
        private Shaders shader;
        private int currentSceneId;
        

        public Graphics(){}
      
        public void Initialize()
        {
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
            //GL.Enable(EnableCap.CullFace);
           // GL.CullFace(CullFaceMode.Back);
            //GL.FrontFace(FrontFaceDirection.Ccw);

            shader.Initialize();

            ErrorCheckValue = GL.GetError();
            if (ErrorCheckValue != ErrorCode.NoError)
                Trace.WriteLine("Error at Creating Shaders: " + ErrorCheckValue);

            // Creating a VBO object now, and after it Indexed vb. In otherwords ElementBuffer
            scenes = new Scene[10];
            scenes[0] = new Scene(1000);
            modelLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ModelMatrix");
            viewLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ViewMatrix");
            projectionLocation = GL.GetUniformLocation(shader.ProgramIds[0], "ProjectionMatrix");
            timeLocation = GL.GetUniformLocation(shader.ProgramIds[0], "time");
            resoLocation = GL.GetUniformLocation(shader.ProgramIds[0], "resolution");

            resolution = new Vector2(1280, 720);
            scenes[0].setBackground(new VBOCube(0, 0, 0, 20.0f, 0));
            scenes[0].addNewVBOCube(new VBOCube(0, 0, 0, 0.5f, 0));
            scenes[0].addNewVBOCube(new VBOCube(0.5f,  0.5f, 0, 0.5f, 0));
            scenes[0].addNewVBOCube(new VBOCube(0, -0.5f, 0, 0.5f, 0));
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
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, x / (float)y, 1.0f, 90.0f);
            ProjectionMatrix = projection;
        }

        public void Render(int sceneId)
        {
            currentSceneId = sceneId;
            GL.UseProgram(shader.ProgramIds[0]);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            scenes[sceneId].getBackground().drawFirst();
            float time = System.DateTime.Now.Millisecond * 0.01f;
            GL.Uniform1(timeLocation, 1, ref time);
            GL.Uniform2(resoLocation, ref resolution);
            GL.UniformMatrix4(viewLocation, false, ref ViewMatrix);
            GL.UniformMatrix4(projectionLocation, false, ref ProjectionMatrix);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1); 
            for (int i = 0; i < scenes[currentSceneId].getCount(); i++)
            {
                GL.UseProgram(shader.ProgramIds[scenes[sceneId].getObjects()[i].getShaderProgramId()]);
                Matrix4 ModelViewMatrix = scenes[sceneId].getObjects()[i].getModelViewMatrix();
                GL.UniformMatrix4(modelLocation, false, ref ModelViewMatrix);
                scenes[sceneId].getObjects()[i].draw();
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
    }
}
