using System;

namespace VanArsdel.Inventory.Controls
{
    public interface IInputControl
    {
        TextEditMode Mode { get; set; }

        void SetFocus();
    }
}
