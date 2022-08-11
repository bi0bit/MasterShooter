using System;

namespace Game
{
    public interface IActionTrigger
    {
        event Action OnEndAction;
    }
}