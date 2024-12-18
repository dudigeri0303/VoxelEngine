using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurCraft.Graphics.DataTypes;
using OurCraft.Graphics.Vertextypes;
using OurCraft.Tools;
using System;
using System.Collections.Generic;

namespace OurCraft.Graphics.Meshes
{
    public struct Cube
    {
        public List<VertexPositionNormalCubeTexture> Vertecies { get; private set; }
        public List<short> Indicies { get; private set; }   
        private int CubeSize { get; } = Globals.Instance.CubeSize;

        private static readonly VertexPositionNormalCubeTexture[] AllVertecies = new VertexPositionNormalCubeTexture[]
        {
            //Front face
            new VertexPositionNormalCubeTexture(new Vector3(-1f, 1f, 1f), Vector3.Forward, new Vector3(-1f, 1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, -1f, 1f), Vector3.Forward, new Vector3(-1f, -1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, -1f, 1f), Vector3.Forward, new Vector3(1f, -1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, 1f, 1f), Vector3.Forward, new Vector3(1f, 1f, 1f), 0f),

            //Back face
            new VertexPositionNormalCubeTexture(new Vector3(1f, 1f, -1f), Vector3.Backward, new Vector3(1f, 1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, -1f, -1f), Vector3.Backward, new Vector3(1f, -1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, -1f, -1f), Vector3.Backward, new Vector3(-1f, -1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, 1f, -1f), Vector3.Backward, new Vector3(-1f, 1f, -1f), 0f),

            //Left face
            new VertexPositionNormalCubeTexture(new Vector3(-1f, 1f, -1f), Vector3.Left, new Vector3(-1f, 1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, -1f, -1f), Vector3.Left, new Vector3(-1f, -1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, -1f, 1f), Vector3.Left, new Vector3(-1f, -1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, 1f, 1f), Vector3.Left, new Vector3(-1f, 1f, 1f), 0f),

            //Right Face
            new VertexPositionNormalCubeTexture(new Vector3(1f, 1f, 1f), Vector3.Right, new Vector3(1f, 1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, -1f, 1f), Vector3.Right, new Vector3(1f, -1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, -1f, -1f), Vector3.Right, new Vector3(1f, -1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, 1f, -1f), Vector3.Right, new Vector3(1f, 1f, -1f), 0f),
            
            //Top Face
            new VertexPositionNormalCubeTexture(new Vector3(-1f, 1f, -1f), Vector3.Up, new Vector3(-1f, 1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, 1f, 1f), Vector3.Up, new Vector3(-1f, 1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, 1f, 1f), Vector3.Up, new Vector3(1f, 1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, 1f, -1f), Vector3.Up, new Vector3(1f, 1f, -1f), 0f),

            //Bottom face
            new VertexPositionNormalCubeTexture(new Vector3(-1f, -1f, -1f), Vector3.Down, new Vector3(-1f, -1f, -1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(-1f, -1f, 1f), Vector3.Down, new Vector3(-1f, -1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, -1f, 1f), Vector3.Down, new Vector3(1f, -1f, 1f), 0f),
            new VertexPositionNormalCubeTexture(new Vector3(1f, -1f, -1f), Vector3.Down, new Vector3(1f, -1f, -1f), 0f)
        };

        public Cube(int xOffset, int yOffset, int zOffset, List<Direction> sides, float cubeType)
        {
            
            Vertecies = new();
            Indicies = new();

            GenerateVisibleSides(sides, xOffset, yOffset, zOffset, CubeSize, cubeType);
        }

        private void GenerateVisibleSides(List<Direction> sides, int xOffset, int yOffset, int zOffset, int cubeSize, float cubeType) 
        {
            void AddVertecies(int fromIndex, int toIndex, List<VertexPositionNormalCubeTexture> vertecies, VertexPositionNormalCubeTexture[] allVertecies) 
            {
                for (int i = fromIndex; i < toIndex + 1; i++)
                {
                    VertexPositionNormalCubeTexture vertex = (VertexPositionNormalCubeTexture)allVertecies[i].Clone();
                    vertex.Position += new Vector3(xOffset, yOffset, zOffset) * cubeSize;
                    vertex.Type = cubeType;
                    vertecies.Add(vertex);
                }   
            }
            void AddIndicies(int nextIndex, List<short> indicies) 
            {
                indicies.Add((short)nextIndex);
                indicies.Add((short)(nextIndex + 3));
                indicies.Add((short)(nextIndex + 2));

                indicies.Add((short)nextIndex);
                indicies.Add((short)(nextIndex + 2));
                indicies.Add((short)((nextIndex + 1)));
            }

            int nextindex = 0;

            if (sides.Contains(Direction.FORWARD))
            {
                AddVertecies(0, 3, Vertecies, AllVertecies);
                AddIndicies(nextindex, Indicies);
                nextindex += 4;
            }
            if (sides.Contains(Direction.BACKWARD))
            {
                AddVertecies(4, 7, Vertecies, AllVertecies);
                AddIndicies(nextindex, Indicies);
                nextindex += 4;
            }
            if (sides.Contains(Direction.LEFT)) 
            {
                AddVertecies(8, 11, Vertecies, AllVertecies);
                AddIndicies(nextindex, Indicies);
                nextindex += 4;
            }
            if (sides.Contains(Direction.RIGHT)) 
            {
                AddVertecies(12, 15, Vertecies, AllVertecies);
                AddIndicies(nextindex, Indicies);
                nextindex += 4;
            }
            if (sides.Contains(Direction.UP)) 
            {
                AddVertecies(16, 19, Vertecies, AllVertecies);
                AddIndicies(nextindex, Indicies);
                nextindex += 4;
            }
            if (sides.Contains(Direction.DOWN)) 
            {
                AddVertecies(20, 23, Vertecies, AllVertecies);
                AddIndicies(nextindex, Indicies);
            }
        }
        #region OldVerteciesAndIndicies
            /*
            //Front bottom left
            0, 3, 2,
            //Front top right
            0, 2, 1,

            //Back bottom left
            4, 6, 7,
            //Back top right
            4, 5, 6,

            //Left bottom left
            8, 10, 11,
            //Left top right
            8, 9, 10,

            //Right bottom left
            12, 15, 14,
            //Right top right
            12, 14, 13,

            //Top bottom left
            16, 19, 18,
            //Top top right
            16, 18, 17,

            //Bottom bottom left
            20, 22, 23,
            //Bottom top right
            20, 21, 22
            */
        #endregion
    }
}
