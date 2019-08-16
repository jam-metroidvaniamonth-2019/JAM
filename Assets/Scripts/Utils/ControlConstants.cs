using UnityEngine;

namespace Utils
{
    public static class ControlConstants
    {
        // Axis
        public const string HorizontalAxis = "Horizontal";
        public const string VerticalAxis = "Vertical";

        // Single KeyCodes
        public const KeyCode Jump = KeyCode.Space;
        public const string JumpGamePad = "joystick button 0";
        public const KeyCode Dash = KeyCode.J;
        public const string DashGamePad = "joystick button 1";

        // Buttons
        public const string JumpButton = "Jump";
        public const string DashButton = "Dash";
    }
}
