using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace OurCraft.Graphics
{
    public enum Direction 
    {
        LEFT, RIGHT, 
        UP, DOWN,
        FORWARD, BACKWARD
    }

    public class Camera
    {    
        private Vector3 _cameraPosition, _cameraDirection;

        private Matrix _worldMatrix;
        private Matrix _viewMatrix, _projectionMatrix;

        private BoundingFrustum _boundingFrustum;
        
        private float _horizontalAngle, _verticalAngle, _sensitivity, _speed;
    

        public Matrix WorldMatrix => _worldMatrix;
        public Matrix ViewMatrix { get => _viewMatrix; }
        public Matrix ProjectionMatrix { get => _projectionMatrix; }

        public Camera() 
        {
            this._cameraPosition = new Vector3(0f, 1f, -1f) * 50;
            this._cameraDirection = Vector3.Zero;
            
            this._worldMatrix = Matrix.Identity;
            this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), Main.Graphics.GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            this._viewMatrix = Matrix.CreateLookAt(this._cameraPosition * 100, this._cameraPosition + this._cameraDirection, Vector3.Up);

            this._boundingFrustum = new BoundingFrustum(this._viewMatrix * this._projectionMatrix);
        
            this._horizontalAngle = 0f;
            this._verticalAngle = 0f;
            this._sensitivity = 0.0018f;
            this._speed = 2.0f;
        }

        public void Move(Direction dir) 
        {
            Vector3 facing = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(this._horizontalAngle));
            Vector3 right = Vector3.Cross(Vector3.Up, facing);
            Vector3 up = Vector3.Cross(facing, right);

            switch (dir)
            {
                case Direction.LEFT:
                    this._cameraPosition += right * this._speed;
                    break;
                case Direction.RIGHT:
                    this._cameraPosition -= right * this._speed;
                    break;
                case Direction.UP:
                    this._cameraPosition += up * this._speed;
                    break;
                case Direction.DOWN:
                    this._cameraPosition -= up * this._speed;
                    break;
                case Direction.FORWARD:
                    this._cameraPosition += facing * this._speed;
                    break;
                case Direction.BACKWARD:
                    this._cameraPosition -= facing * this._speed;
                    break;
            }
        }
        
        public void UpdateViewMatrix() 
        {
            this._cameraDirection = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(this._verticalAngle) * Matrix.CreateRotationY(this._horizontalAngle));
            this._viewMatrix = Matrix.CreateLookAt(this._cameraPosition, this._cameraPosition + this._cameraDirection, Vector3.Up);
        }

        public void CalculateHorizontalAngle(int distance) 
        {
            this._horizontalAngle += this._sensitivity * distance;
        }

        public void CalculateVerticalAngle(int distance) 
        {
            this._verticalAngle += this._sensitivity * distance;
            this._verticalAngle = Math.Clamp(this._verticalAngle, -1, 1);
        }
    }
}
