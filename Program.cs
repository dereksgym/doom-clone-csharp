using DoomClone.Core;
using DoomClone.Utilities;

namespace DoomClone;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var game = new Game(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
            game.Run();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            Environment.Exit(1);
        }
    }
}
