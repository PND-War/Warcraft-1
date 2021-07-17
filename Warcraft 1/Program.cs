using System;

namespace Warcraft_1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Warcraft())
                game.Run();
        }
    }
}
