using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurCraft.Graphics.Meshes;
using OurCraft.Tools;
using System.Collections.Generic;
using System.Diagnostics;


namespace OurCraft.Graphics.WorldObjects
{
    public class Water : WorldObjectBase<VertexPositionTexture>
    {
        private int _waterSurfaceChunkCount = Globals.Instance.WaterSurfaceChunkCount;

        public Water(Effect shader) : base(shader, null)
        {
        }

        protected override void GenerateVerteciesAndIndicies()
        {
            Dictionary<VertexPositionTexture, int> vertexIndexHashMap = new();

            for (int i = 0; i < this._waterSurfaceChunkCount; i++)
            {
                for (int j = 0; j < this._waterSurfaceChunkCount; j++)
                {
                    Square square = new Square(i, j);

                    foreach (var v in square.Vertecies)
                    {
                        if (!vertexIndexHashMap.ContainsKey(v))
                        {
                            _vertecies.Add(v);
                            vertexIndexHashMap[v] = _vertecies.Count - 1;
                        }
                    }

                    foreach (var ind in square.Indicies)
                    {
                        var vertexValue = square.Vertecies[ind];
                        _indicies.Add((uint)vertexIndexHashMap[vertexValue]);
                    }
                }
            }
        }

        public override void Render(Matrix world, Matrix view, Matrix projection, float timeElapsed)
        {
            this._shader.Parameters["time"].SetValue(timeElapsed);
            base.Render(world, view, projection, timeElapsed);
        }
    }
}
