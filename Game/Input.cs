using Microsoft.Xna.Framework.Input;

namespace GameProject {
    static class Input {
        static MouseState _newMouse, _oldMouse;
        static KeyboardState _newKeyboard, _oldKeyboard;

        public static void Update() {
            _oldMouse = _newMouse;
            _newMouse = Mouse.GetState();
            _oldKeyboard = _newKeyboard;
            _newKeyboard = Keyboard.GetState();
        }

        public static int MouseX => _newMouse.X;
        public static int MouseY => _newMouse.Y;

        public static bool LMBHeld => _newMouse.LeftButton == ButtonState.Pressed;
        public static bool LMBPressed => _newMouse.LeftButton == ButtonState.Pressed && _oldMouse.LeftButton != ButtonState.Pressed;
        public static bool LMBReleased => _newMouse.LeftButton != ButtonState.Pressed && _oldMouse.LeftButton == ButtonState.Pressed;
        public static bool MMBHeld => _newMouse.MiddleButton == ButtonState.Pressed;
        public static bool MMBPressed => _newMouse.MiddleButton == ButtonState.Pressed && _oldMouse.MiddleButton != ButtonState.Pressed;
        public static bool MMBReleased => _newMouse.MiddleButton != ButtonState.Pressed && _oldMouse.MiddleButton == ButtonState.Pressed;
        public static bool RMBHeld => _newMouse.RightButton == ButtonState.Pressed;
        public static bool RMBPressed => _newMouse.RightButton == ButtonState.Pressed && _oldMouse.RightButton != ButtonState.Pressed;
        public static bool RMBReleased => _newMouse.RightButton != ButtonState.Pressed && _oldMouse.RightButton == ButtonState.Pressed;
        public static bool MMBScrolledUp => _newMouse.ScrollWheelValue > _oldMouse.ScrollWheelValue;
        public static bool MMBScrolledDown => _newMouse.ScrollWheelValue < _oldMouse.ScrollWheelValue;

        public static bool KeyHeld(Keys key) => _newKeyboard.IsKeyDown(key);
        public static bool KeyPressed(Keys key) => _newKeyboard.IsKeyDown(key) && !_oldKeyboard.IsKeyDown(key);
        public static bool KeyReleased(Keys key) => !_newKeyboard.IsKeyDown(key) && _oldKeyboard.IsKeyDown(key);
        public static bool AnyKeyHeld(params Keys[] keys) {
            foreach (var key in keys)
                if (_newKeyboard.IsKeyDown(key))
                    return true;
            return false;
        }
        public static bool AnyKeyPressed(params Keys[] keys) {
            foreach (var key in keys)
                if (_newKeyboard.IsKeyDown(key) && !_oldKeyboard.IsKeyDown(key))
                    return true;
            return false;
        }
        public static bool AnyKeyReleased(params Keys[] keys) {
            foreach (var key in keys)
                if (!_newKeyboard.IsKeyDown(key) && _oldKeyboard.IsKeyDown(key))
                    return true;
            return false;
        }
    }
}