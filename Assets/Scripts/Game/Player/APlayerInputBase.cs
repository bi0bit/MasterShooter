using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Player
{
    public abstract class APlayerInputBase
    {
        public abstract bool CheckInput(out Vector2 position);
    }
    
    public class PlayerInputMobile : APlayerInputBase
    {
        public override bool CheckInput(out Vector2 position)
        {
            if (Input.touchCount > 0)
            {
                position = Input.GetTouch(0).position;
                return true;
            }
            position = default;
            return false;
        }
    }

    public class PlayerInputPC : APlayerInputBase
    {
        public override bool CheckInput(out Vector2 position)
        {
            position = Input.mousePosition;
            return Input.GetMouseButton((int) MouseButton.LeftMouse);
        }
    }
}