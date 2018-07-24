namespace My.App.Interfaces
{
    public interface ICommandInterpreter
    {
        IExecutable ProcessCommand(string cmdName);
    }
}
