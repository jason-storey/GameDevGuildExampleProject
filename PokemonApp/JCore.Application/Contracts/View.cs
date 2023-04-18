using System;

namespace JCore.Application
{
    public interface View
    {
        void SendMessage(Message message);
        
        bool IsBusy { set; }
    }
    
    public class Message
    {
        public string Text;
        public object Context;
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