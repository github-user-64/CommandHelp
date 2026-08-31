using CommandHelp.Exceptions;
using System;
using System.Collections.Generic;

namespace CommandHelp
{
    /// <summary>
    /// 指令:返回指定类型的值, 解析多个参数
    /// </summary>
    /// <typeparam name="T">指定类型</typeparam>
    public abstract class CommandValues<T> : CommandValue<T>
    {
        /// <summary>
        /// 参数数量
        /// </summary>
        public readonly int ArgCount;
        private string[] _args = null;
        private T[] _rValue = null;


        /// <exception cref="ArgumentException"></exception>
        public CommandValues(int argCount, bool isVariable = false, string text = null) : base(isVariable, text)
        {
            if (argCount < 1) throw new ArgumentException("参数数量不能小于1", nameof(argCount));
            ArgCount = argCount;
        }


        /// <inheritdoc/>
        public override object Run(ref int index, List<CommandObject> commandList)
        {
            return _rValue;
        }

        /// <inheritdoc/>
        public override CommandObject Parse(string command)
        {
            if (_args == null) ParseFormat(command);

            T[] value = new T[ArgCount];

            //参数正常时_args正常
            //异常时, 如果参数数量为0且该指令为可变参数时才会进来, 此时_args为null
            if (_args == null)
            {
                IsDefault = true;
                value = new T[ArgCount];
                for (int i = 0; i < ArgCount; ++i) value[i] = GetDefault();
                _rValue = value;
                return this;
            }

            for (int i = 0; i < ArgCount; ++i)
            {
                try
                {
                    value[i] = ArgConvertThrow(_args[i]);
                }
                catch (Exception ex)
                {
                    throw new CommandException(exceptionmessage: $"参数[{_args[i]}]类型不为<{typeof(T).Name}>", ex: ex);
                }
            }

            _rValue = value;

            return this;
        }

        /// <inheritdoc/>
        public override (string cmdParse, string cmd) ParseFormat(string command)
        {
            char key = ' ';

            //"v v v subCommamd", 获取"v v v"部分

            string cmdParse = command;
            string[] vs = new string[ArgCount];

            int i = 0;
            for (; i < vs.Length; ++i)
            {
                //清空开头空格
                command = command.TrimStart(key);

                if (command.Length == 0) break;

                //获取"v"之后空格位置
                int index = command.IndexOf(key);

                //获取"v"长度
                int vLen = index;
                if (vLen < 1) vLen = command.Length;

                //获取"v"
                vs[i] = command.Substring(0, vLen);

                //获取剩余指令
                command = command.Remove(0, vLen);
            }

            //缺少参数
            if (i < ArgCount)
            {
                //数量为0时
                if (i == 0)
                {
                    if (IsVariable) return ("", cmdParse);

                    throw new CommandLackException();
                }

                throw new CommandException(exceptionmessage: $"缺少{vs.Length - i}个参数");
            }

            cmdParse = cmdParse.Substring(0, cmdParse.Length - command.Length);

            _args = vs;

            return (cmdParse, command);
        }
    }
}
