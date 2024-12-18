using Microsoft.Xna.Framework.Input;
using OurCraft.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OurCraft.InputDevices
{
    public class KeyboardHandler
    {
        private KeyboardState _keybordState;
        private bool _wKeyBool, _sKeyBool, _aKeyBool, _dKeyBool, _leftControlKeyBool, _spaceKeyBool, _zeroKeyBool, _prevZeroKeyBool;

        public bool ZeroKeyBool => _zeroKeyBool;
      
        public KeyboardHandler()
        {
            this._wKeyBool = false;
            this._sKeyBool = false;
            this._aKeyBool = false;
            this._dKeyBool = false;
            this._leftControlKeyBool = false;
            this._spaceKeyBool = false;
            this._zeroKeyBool = true;
            this._prevZeroKeyBool = false;
        }
        private void IsKeysPressed()
        {
            if (this._keybordState.IsKeyDown(Keys.W))
                this._wKeyBool = true;
            
            if (this._keybordState.IsKeyDown(Keys.S))
                this._sKeyBool = true;

            if (this._keybordState.IsKeyDown(Keys.A))
                this._aKeyBool = true;

            if (this._keybordState.IsKeyDown(Keys.D))
                this._dKeyBool = true;
            
            if(this._keybordState.IsKeyDown(Keys.LeftControl)) 
                this._leftControlKeyBool= true;
            
            if(this._keybordState.IsKeyDown(Keys.Space)) 
                this._spaceKeyBool = true;

            if (this._keybordState.IsKeyDown(Keys.D0))
            {
                if (!this._prevZeroKeyBool)
                {
                    this._zeroKeyBool = !this._zeroKeyBool;
                    this._prevZeroKeyBool = true;
                }
            }
        }

        private void IsKeysReleased()
        {
            if (this._keybordState.IsKeyUp(Keys.W))
                this._wKeyBool = false;
            
            if (this._keybordState.IsKeyUp(Keys.S))
                this._sKeyBool = false;
            
            if (this._keybordState.IsKeyUp(Keys.A))
                this._aKeyBool = false;
            
            if (this._keybordState.IsKeyUp(Keys.D))
                this._dKeyBool = false;
            
            if(this._keybordState.IsKeyUp(Keys.LeftControl))
                this._leftControlKeyBool = false;

            if (this._keybordState.IsKeyUp(Keys.Space))
                this._spaceKeyBool = false;

            if (this._keybordState.IsKeyUp(Keys.D0))
                this._prevZeroKeyBool = false;
        }

        private void ActOnKeyEvent(Camera camera) 
        {
            if (this._zeroKeyBool) 
            {
                if (this._aKeyBool)
                    camera.Move(Direction.LEFT);

                if (this._dKeyBool)
                    camera.Move(Direction.RIGHT);

                if (this._wKeyBool)
                    camera.Move(Direction.FORWARD);

                if (this._sKeyBool)
                    camera.Move(Direction.BACKWARD);

                if (this._spaceKeyBool)
                    camera.Move(Direction.UP);

                if (this._leftControlKeyBool)
                    camera.Move(Direction.DOWN);
            }
        }

        public void HandleKey(Camera camera)
        {
            this._keybordState = Keyboard.GetState();
            this.IsKeysPressed();
            this.IsKeysReleased();
            this.ActOnKeyEvent(camera);
        }
    }
}
