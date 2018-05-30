using System.ComponentModel;
using System.Diagnostics;

namespace CardGameEngine.Debugs
{
    public static class DebugExtensions
    {
        public static void TraceInfoIfFalse(this bool boolean, string msg, params object[] args)
        {
            if (!boolean) Trace.TraceInformation(msg, args);
        }

        public static void TraceInfoIfTrue(this bool boolean, string msg, params object[] args)
        {
            if (boolean) Trace.TraceInformation(msg, args);
        }

        public static void TraceWarningIfFalse(this bool boolean, string msg, params object[] args)
        {
            if (!boolean) Trace.TraceWarning(msg, args);
        }

        public static void TraceWarningIfTrue(this bool boolean, string msg, params object[] args)
        {
            if (boolean) Trace.TraceWarning(msg, args);
        }

        public static void TraceErrorIfFalse(this bool boolean, string msg, params object[] args)
        {
            if (!boolean) Trace.TraceError(msg, args);
        }

        public static void TraceErrorIfTrue(this bool boolean, string msg, params object[] args)
        {
            if (boolean) Trace.TraceError(msg, args);
        }
    }
}
