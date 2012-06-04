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
    class VBOCube
    {
        Matrix4 ModelMatrix,ViewMatrix;
        private int VaoId, VboId, ColorBufferId, ShaderProgramId;
        float[] Vertices;
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

        public VBOCube(float x, float y, float z, float scale, int ShaderProgramId)
        {
            this.ShaderProgramId = ShaderProgramId;
            Vertices = new float[]{
           -(scale*0.5f), -(scale*0.5f),  (scale*0.5f), 1 , 
           -(scale*0.5f),  (scale*0.5f),  (scale*0.5f), 1 ,
            (scale*0.5f),  (scale*0.5f),  (scale*0.5f), 1 , 
            (scale*0.5f), -(scale*0.5f),  (scale*0.5f), 1 , 
           -(scale*0.5f), -(scale*0.5f), -(scale*0.5f), 1 , 
           -(scale*0.5f),  (scale*0.5f), -(scale*0.5f), 1 ,
            (scale*0.5f),  (scale*0.5f), -(scale*0.5f), 1 , 
            (scale*0.5f), -(scale*0.5f), -(scale*0.5f), 1 , 
          };
            ViewMatrix = Matrix4.Identity;
            ModelMatrix = Matrix4.CreateTranslation(new Vector3(x,y,z));
        }

        public void loadToGpu() 
        {
            GL.GenVertexArrays(1, out VaoId);
            GL.BindVertexArray(VaoId);
            //derp cubeg
            GL.GenBuffers(1, out VboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * sizeof(float)), Vertices, BufferUsageHint.StaticDraw);
            //our pretty colours
            GL.GenBuffers(1, out ColorBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ColorBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(float)), indices, BufferUsageHint.StaticDraw);
        }

        public void draw()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            //GL.BindVertexArray(VaoId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboId);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ColorBufferId);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedInt, 0);
        }

        public Matrix4 getModelMatrix()
        {
            return ModelMatrix;
        }
        public Matrix4 getViewMatrix()
        {
            return ViewMatrix;
        }

        public void setModelMatrix(Matrix4 mat)
        {
            ModelMatrix = mat;
        }
        public void setViewMatrix(Matrix4 mat)
        {
            ViewMatrix = mat;
        }

        public Matrix4 getModelViewMatrix()
        {
            return Matrix4.Mult(ViewMatrix, ModelMatrix);
        }

        public int getShaderProgramId()
        {
            return ShaderProgramId;
        }

        public void drawFirst()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.DepthMask(false);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboId);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ColorBufferId);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedInt, 0);
            GL.DepthMask(true);
        }
    }
}
