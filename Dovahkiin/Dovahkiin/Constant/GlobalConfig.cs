using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovahkiin.Constant
{
    public static class GlobalConfig
    {
        public static bool SoundEnabled;
        public static bool MusicEnabled;

        public static void Initialize()
        {
            SoundEnabled = true;
            MusicEnabled = true;
        }
    }
}
