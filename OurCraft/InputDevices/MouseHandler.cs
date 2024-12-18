using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OurCraft.Graphics;

namespace OurCraft.InputDevices
{
    public class MouseHandler
    {
        private Point _mousePosition;
        private bool _leftClicked;
        private bool _previousLeftCilcked;

        public MouseHandler()
        {
            this._mousePosition = new Point(0, 0);
            this._leftClicked = false;
            this._previousLeftCilcked = false;
        }

        public void HandleMouse(Camera camera, bool isInputEnabled)
        {
            if (isInputEnabled) 
            {
                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed & !this._previousLeftCilcked)
                {
                    this._leftClicked = true;
                    this._previousLeftCilcked = true;
                }
                else
                {
                    this._leftClicked = false;
                }

                if (mouseState.LeftButton == ButtonState.Released)
                {
                    this._leftClicked = false;
                    this._previousLeftCilcked = false;
                }

                //Camera movement
                camera.CalculateHorizontalAngle(this._mousePosition.X - mouseState.X);
                camera.CalculateVerticalAngle(this._mousePosition.Y - mouseState.Y);
                
                //Updating the mouse position
                this._mousePosition.X = mouseState.X;
                this._mousePosition.Y = mouseState.Y;
            }
        }
    }
}
