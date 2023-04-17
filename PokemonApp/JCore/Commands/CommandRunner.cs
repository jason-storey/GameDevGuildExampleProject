namespace JCore.Commands
{
    public class CommandRunner
    {
        readonly DropoutStack<ICommand> _undoStack;
        readonly DropoutStack<ICommand> _redoStack;

        public CommandRunner(int stackSize) => (_undoStack, _redoStack) = (new DropoutStack<ICommand>(stackSize), new DropoutStack<ICommand>(stackSize));

        public void Run(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            while (!_redoStack.IsEmpty()) _redoStack.Pop();
        }

        public void Undo()
        {
            if (!_undoStack.IsEmpty())
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (!_redoStack.IsEmpty())
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
            }
        }
    }
}