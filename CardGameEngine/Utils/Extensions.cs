using System;

namespace CardGameEngine.Utils
{
    public static class Extensions
    {
        public static void InvokeWhenException<T>(this EventHandler<T> e, object sender, T args, Action<Exception> callback)
        {
            try
            {
                e(sender, args);
            }
            catch (Exception exception)
            {
                callback(exception);
            }
        }
    }
}
