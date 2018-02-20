using System;

namespace VanArsdel.Inventory
{
    public class NavigationItem
    {
        public NavigationItem(Type page)
        {
            Page = page;
        }
        public NavigationItem(int glyph, string label, Type page) : this(page)
        {
            Label = label;
            Glyph = Char.ConvertFromUtf32(glyph).ToString();
        }

        public readonly string Glyph;
        public readonly string Label;
        public readonly Type Page;
    }
}
