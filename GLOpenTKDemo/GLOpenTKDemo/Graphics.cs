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
        private int VaoId, VboId, ColorBufferId;
        private Shaders shader;


        public Graphics(){}

        public void Initialize()
        {
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


            Matrix4 modelview = Matrix4.CreateTranslation(new Vector3(0, 0, -2));
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            ViewMatrix = modelview;
        }


        public void onResize(int a, int b, int c, int d, int x, int y)
        {
            GL.Viewport(a, b, c, d);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, x / (float)y, 1.0f, 90.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            ProjectionMatrix = projection;
        }

        public void Render()
        {
            GL.UseProgram(shader.ProgramIds[0]);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            float time = System.DateTime.Now.Millisecond * 0.01f;
            GL.Uniform1(timeLocation, 1, ref time);
            GL.Uniform2(resoLocation, ref resolution);
            GL.UniformMatrix4(modelLocation, false, ref ModelMatrix);
            GL.UniformMatrix4(viewLocation, false, ref ViewMatrix);
            GL.UniformMatrix4(projectionLocation, false, ref ProjectionMatrix);




            //System.Console.WriteLine(time);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedInt, 0);
            /*
            GL.Begin(BeginMode.Triangles);

            GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
            GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

            GL.End();
            */

            GL.UseProgram(0);
        }

        public void rotateObjectByX(float x)
        {
            Matrix4 heippa = ModelMatrix;
            Matrix4 heippa2 = Matrix4.CreateRotationX(x);
            Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
        }

        public void rotateObjectByY(float y)
        {
            Matrix4 heippa = ModelMatrix;
            Matrix4 heippa2 = Matrix4.CreateRotationY(y);
            Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
        }

        public void rotateObjectByZ(float z)
        {
            Matrix4 heippa = ModelMatrix;
            Matrix4 heippa2 = Matrix4.CreateRotationZ(z);
            Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
        }

        public void translateObject(float x, float y, float z)
        {
            Matrix4 heippa = ModelMatrix;
            Matrix4 heippa2 = Matrix4.CreateTranslation(new Vector3(x, y, z));
            Matrix4.Mult(ref heippa, ref heippa2, out ModelMatrix);
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
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref ViewMatrix);
        }
    }
}
