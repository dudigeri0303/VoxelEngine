internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new OurCraft.Main();
        game.Run();
    }
}