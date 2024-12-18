using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OurCraft.Graphics.Meshes
{
    public class Triangle
    {
        private VertexPositionColor[] _vertecis;

        public VertexPositionColor[] Vertecies => _vertecis;

        public Triangle()
        {
            this._vertecis = new VertexPositionColor[] 
            {
                new VertexPositionColor(new Vector3(0, 20, 0), Color.Red),
                new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green),
                new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue)
            };
        }
    }
}
