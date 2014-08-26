using System;

namespace Dovahkiin
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Dovahkiin game = new Dovahkiin())
            {
                game.Run();
            }
        }
    }
#endif
}

