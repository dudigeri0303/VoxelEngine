using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurCraft.Tools;

namespace OurCraft.Graphics.Meshes
{
    public struct Square
    {
        public VertexPositionTexture[] Vertecies { get; private set; }
        public short[] Indicies { get; private set; }
        private int SquareSize { get; } = Globals.Instance.SquareSize;
        

        public Square(int xOffset, int zOffset)
        {
            Vertecies = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-1f + xOffset, -1f / SquareSize, -1f + zOffset) * SquareSize, new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(1f + xOffset, -1f / SquareSize, -1f + zOffset) * SquareSize, new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(1f + xOffset, -1f / SquareSize, 1f + zOffset) * SquareSize, new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(-1f + xOffset, -1f / SquareSize, 1f + zOffset) * SquareSize, new Vector2(1, 1))
            };

            Indicies = new short[] 
            {
                3, 0, 1,
                1, 2, 3
            };
        }
    }
}
