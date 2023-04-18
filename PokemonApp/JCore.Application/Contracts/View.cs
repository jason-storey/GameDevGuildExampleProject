using System;

namespace JCore.Application
{
    public interface View
    {
        void SendMessage(Message message);
        
        bool ShowErrorDetails { get; set; }
        bool IsBusy { set; }
    }
    
    public class Message
    {
        public string Text;
        public object Context;

        public static Message Say(string text,object context = null) => new Message
            {
                Text = text,
                Context = null
            };

        public bool Is(string text) => text.Equals(Text, StringComparison.OrdinalIgnoreCase);
        public override string ToString() => Text;
    }
    
    public static class MessageUtilities
    {
        public static Message FromError(Exception ex) =>
            new Message
            {
                Text = ex.Message,
                Context = ex
            };
    }
}