using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace OurCraft.Graphics.Vertextypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionNormalCubeTexture : IVertexType, ICloneable
    {
        private Vector3 _position;
        private Vector3 _normal;
        private Vector3 _textureCoordinates;
        private float _type;

        public readonly static VertexDeclaration _vertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * 9, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
        );

        public Vector3 Position { get => _position; set => _position = value; }
        public Vector3 Normal { get => _normal; set => _normal = value; }
        public Vector3 TextureCoordinates { get => _textureCoordinates; set => _textureCoordinates = value; }
        public float Type { get => _type; set => _type = value; }

        VertexDeclaration IVertexType.VertexDeclaration => _vertexDeclaration;

        public VertexPositionNormalCubeTexture(Vector3 position, Vector3 normal, Vector3 textureCoordinates, float type) 
        {
            this._position = position;
            this._normal = normal;
            this._textureCoordinates = textureCoordinates;
            this._type = type;
        }

        public object Clone()
        {
            return new VertexPositionNormalCubeTexture(this._position, this._normal, this._textureCoordinates, this._type);
        }

        public override int GetHashCode()
        {
            return (((Position.GetHashCode() * 397) ^ Normal.GetHashCode()) * 397) ^ TextureCoordinates.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return this == (VertexPositionNormalCubeTexture)obj;
        }

        public static bool operator ==(VertexPositionNormalCubeTexture left, VertexPositionNormalCubeTexture right)
        {
            if (left.Position == right.Position && left.Normal == right.Normal && left.Type == right.Type)
            {
                return left.TextureCoordinates == right.TextureCoordinates;
            }

            return false;
        }

        public static bool operator !=(VertexPositionNormalCubeTexture left, VertexPositionNormalCubeTexture right)
        {
            return !(left == right);
        }
    }
}
