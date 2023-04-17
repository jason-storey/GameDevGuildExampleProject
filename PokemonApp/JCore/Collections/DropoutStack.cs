namespace JCore
{
    public class DropoutStack<T>
    {
        readonly T[] _stack;
        int _top;

        public DropoutStack(int size) => (_stack, _top) = (new T[size], -1);
    
        public void Push(T item) => _stack[_top = (_top + 1) % _stack.Length] = item;

        public T Pop()
        {
            if (_top < 0) return default;
            T item = _stack[_top];
            _top = (_top - 1 + _stack.Length) % _stack.Length;
            return item;
        }
    
        public bool IsEmpty() => _top < 0;
    }
}