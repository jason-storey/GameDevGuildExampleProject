namespace JCore.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}