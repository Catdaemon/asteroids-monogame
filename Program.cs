using System;

namespace monogame_test
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new AsteroidsGame())
                game.Run();
        }
    }
}
