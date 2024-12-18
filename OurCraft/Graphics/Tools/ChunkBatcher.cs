using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurCraft.Graphics.Vertextypes;
using OurCraft.Graphics.WorldObjects;
using System.Collections.Generic;

namespace OurCraft.Graphics.Tools
{
    public class ChunkBatcher
    {
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private int _indexCount;

        public ChunkBatcher(List<Chunk> chunks) 
        {
            this.BatchChunkData(chunks);    
        }

        private void BatchChunkData(List<Chunk> chunks) 
        {
            Dictionary<VertexPositionNormalCubeTexture, int> vertexIndexHashMap = new();
            List<VertexPositionNormalCubeTexture> vertecies = new();
            List<uint> indicies = new();

            foreach (var c in chunks)
            {
                foreach (var v in c.Vertecies)
                {
                    if (!vertexIndexHashMap.ContainsKey(v))
                    {
                        vertecies.Add(v);
                        vertexIndexHashMap[v] = vertecies.Count - 1;
                    }
                }

                foreach (var ind in c.Indicies)
                {
                    var vertexValue = c.Vertecies[ind];
                    indicies.Add((uint)vertexIndexHashMap[vertexValue]);
                }
            }

            this._indexCount = indicies.Count;
            this._vertexBuffer = new VertexBuffer(Main.Graphics.GraphicsDevice, typeof(VertexPositionNormalCubeTexture), indicies.Count, BufferUsage.WriteOnly);
            this._indexBuffer = new IndexBuffer(Main.Graphics.GraphicsDevice, IndexElementSize.ThirtyTwoBits, indicies.Count, BufferUsage.WriteOnly);
        }

        public void Render(Matrix world, Matrix view, Matrix projection, Effect shader) 
        {
            shader.Parameters["World"].SetValue(world);
            shader.Parameters["View"].SetValue(view);
            shader.Parameters["Projection"].SetValue(projection);

            shader.CurrentTechnique.Passes[0].Apply();

            Main.Graphics.GraphicsDevice.SetVertexBuffer(this._vertexBuffer);
            Main.Graphics.GraphicsDevice.Indices = this._indexBuffer;

            Main.Graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this._indexCount / 3);
        }
    }
}
