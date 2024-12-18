using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace OurCraft.Graphics.WorldObjects
{
    public abstract class WorldObjectBase<VT> : IDisposable
        where VT : struct, IVertexType
    {
        protected List<VT> _vertecies = new();
        protected List<uint> _indicies = new();

        protected VertexBuffer _vertexBuffer;
        protected IndexBuffer _indexBuffer;

        protected Effect _shader;

        public VT[] Vertecies => _vertecies.ToArray();
        public uint[] Indicies => _indicies.ToArray();

        public WorldObjectBase(Effect shader, object param) 
        {
            UniqeConstructorBeforeBase(param);

            this._shader = shader;
            
            this.GenerateVerteciesAndIndicies();

            this._vertexBuffer = new VertexBuffer(Main.Graphics.GraphicsDevice, typeof(VT), Vertecies.Length, BufferUsage.WriteOnly);
            this._indexBuffer = new IndexBuffer(Main.Graphics.GraphicsDevice, IndexElementSize.ThirtyTwoBits, Indicies.Length, BufferUsage.WriteOnly);

            this._vertexBuffer.SetData<VT>(Vertecies);
            this._indexBuffer.SetData(Indicies);
        }
        public void Dispose()
        {
            this._vertecies.Clear();
            this._indicies.Clear();

            this._vertecies = null;
            this._indicies = null;

            Main.Graphics.GraphicsDevice.SetVertexBuffer(null);
            Main.Graphics.GraphicsDevice.Indices = null;

            this._vertexBuffer.Dispose();
            this._indexBuffer.Dispose();

            GC.SuppressFinalize(this);
        }

        protected abstract void GenerateVerteciesAndIndicies();
        protected virtual void UniqeConstructorBeforeBase(object param) 
        {
            if (param == null)
                return;
        }
        
        protected virtual void ActivateShader(Matrix world, Matrix view, Matrix projection) 
        {
            this._shader.Parameters["World"].SetValue(world);
            this._shader.Parameters["View"].SetValue(view);
            this._shader.Parameters["Projection"].SetValue(projection);

            this._shader.CurrentTechnique.Passes[0].Apply();
        }

        public virtual void Render(Matrix world, Matrix view, Matrix projection, float timeElapsed) 
        {
            this.ActivateShader(world, view, projection);
        
            Main.Graphics.GraphicsDevice.SetVertexBuffer(this._vertexBuffer);
            Main.Graphics.GraphicsDevice.Indices = this._indexBuffer;

            //Debug.WriteLine(this.Indicies.Length/3);
            Main.Graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this.Indicies.Length / 3);
        }
    }
}
