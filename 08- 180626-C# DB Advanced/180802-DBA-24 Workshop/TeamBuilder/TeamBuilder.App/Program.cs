using TeamBuilder.App.Core;

namespace TeamBuilder.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new Engine(new CommandDispatcher());
            engine.Run();
        }
    }
}
