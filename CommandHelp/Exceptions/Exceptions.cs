using System;

namespace CommandHelp.Exceptions
{
    /// <summary>
    /// 指令异常
    /// </summary>
    public class CommandException : Exception
    {
        /// <summary/>
        public CommandException(int line = -1, string exceptionCommand = null, string exceptionmessage = null, Exception ex = null) : base(ex?.Message, ex)
        {
            Line = line;
            ExceptionCommand = exceptionCommand;
            ExceptionMessage = exceptionmessage;
        }

        /// <summary>异常位置</summary>
        public int Line { get; protected set; } = -1;
        /// <summary>异常指令</summary>
        public string ExceptionCommand { get; protected set; } = null;
        /// <summary>异常信息</summary>
        public string ExceptionMessage { get; protected set; } = null;
    }

    /// <summary>
    /// 指令缺失
    /// </summary>
    public class CommandLackException : CommandException
    {
        /// <summary/>
        public CommandLackException(int line = -1, string exceptionCommand = null, string exceptionmessage = null, Exception ex = null)
            : base(line, exceptionCommand, exceptionmessage, ex)
        {

        }
    }

    /// <summary>
    /// 指令解析失败
    /// </summary>
    public class CommandParseException : CommandException
    {
        /// <summary/>
        public CommandParseException(int line = -1, string exceptionCommand = null, string exceptionmessage = null, Exception ex = null)
            : base(line, exceptionCommand, exceptionmessage, ex)
        {

        }
    }
}
