using System;

namespace JCore.Application
{
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