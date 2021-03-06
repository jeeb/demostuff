﻿using System;
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

        uint[] indices;

        uint[] background = {
            1,2,0, 2,3,0,
            0,3,4, 3,7,4,
            5,1,4, 1,0,4,
            2,6,3, 6,7,3,
            5,6,1, 6,2,1,
            6,5,7, 5,4,7
        };
        uint[] normal = {
            0,2,1, 0,3,2,
            4,3,0, 4,7,3,
            4,1,5, 4,0,1,
            3,6,2, 3,7,6,
            1,6,5, 1,2,6,
            7,5,6, 7,4,5
        };

        public UuNya uu;
        public UuNya nya;
        public UuNya rand;
        public float current_x, current_y, current_z, vx, vy,vz;

        public VBOCube(float x, float y, float z, float scale, int ShaderProgramId, bool isBackground)
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

            if (isBackground)
                indices = background;
            else
                indices = normal;

            ViewMatrix = Matrix4.Identity;
            ModelMatrix = Matrix4.CreateTranslation(new Vector3(x,y,z));
        }
        public void altColors()
        {
            Colors = new float[]{
            1, 1, 1, 1 ,
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1 ,
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1
          };
        }
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

            indices = normal;

            ViewMatrix = Matrix4.Identity;
            ModelMatrix = Matrix4.CreateTranslation(new Vector3(x, y, z));
        }

        public VBOCube(float x, float y, float z, float scale, int ShaderProgramId, UuNya uu, UuNya nya, UuNya rand)
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

            indices = normal;

            ViewMatrix = Matrix4.Identity;
            ModelMatrix = Matrix4.CreateTranslation(new Vector3(x, y, z));
            this.uu = uu;
            this.nya = nya;
            this.rand = rand;
            current_x = x;
            current_y = y;
            current_z = z;
            vx = vz = vy = 0.0f;
        }
        public void moveTowards(float x, float y, float z, float relative_velocity)
        {
            vx = (current_x - x) * relative_velocity;
            vy = (current_y - y) * relative_velocity;
            vz = (current_z - z) * relative_velocity;

            current_x -= vx;
            current_y -= vy;
            current_z -= vz;
            ModelMatrix = Matrix4.CreateTranslation(new Vector3(current_x, current_y, current_z));
        }

        public void moveTowards(int target, float relative_velocity)
        {
            UuNya obj = null;
            if (target == 0)
            {
                obj = uu;
            }
            else if (target == 1)
            {
                obj = nya;
            }
            else
            {
                obj = rand;
            }
            vx = (current_x - obj.x) * relative_velocity;
            vy = (current_y - obj.y) * relative_velocity;
            vz = (current_z - obj.z) * relative_velocity;

            current_x -= vx;
            current_y -= vy;
            current_z -= vz;
            ModelMatrix = Matrix4.CreateTranslation(new Vector3(current_x, current_y, current_z));
            //this.ModelMatrix = Matrix4.Mult(Matrix4.CreateTranslation(new Vector3(vx, vy, vz)), this.getModelMatrix());
        }
        //TODO: Jos tehdään vaa laatikoita nii annan samat vertice ja indice tiedot jokaselle eri laatikolle,
        //      sit vaa piirretään yks objecti monta kertaa. Optimisaatiot <3.
        //TODO: Ennen ylempää implementoi scale matriisi jotta voidaan sit saaha eri suurusia laatikoita.
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
            //GL.Disable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            //GL.FrontFace(FrontFaceDirection.Ccw);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.DepthMask(false);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VboId);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ColorBufferId);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawElements(BeginMode.Triangles, 36, DrawElementsType.UnsignedInt, 0);
            GL.DepthMask(true);

            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            //GL.FrontFace(FrontFaceDirection.Ccw);
        }
    }
}
