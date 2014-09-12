using Dovahkiin.Repository;

namespace Dovahkiin.Constant
{
    public static class  Fonts
    {
        public static int ButtonFont;
        public static int DescriptionFont;
        public static void Initialize()
        {
            ButtonFont = Resouces.AddFont(@"fonts/button_font");
            DescriptionFont = Resouces.AddFont(@"fonts/description_font");
        }
    }
}
