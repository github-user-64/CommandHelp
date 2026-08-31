using System;
using System.Collections.Generic;

namespace CommandHelp
{
    /// <summary>
    /// 指令:调用方法
    /// </summary>
    public class CommandMethod : CommandObject
    {
        /// <summary>
        /// 参数数量
        /// </summary>
        public readonly int ArgCount;
        /// <summary>
        /// <see cref="OnRuning"/>调用时调用
        /// </summary>
        public event Action<object[]> Runing = null;


        /// <exception cref="ArgumentException"></exception>
        public CommandMethod(string text = null, int argCount = 0) : base(text)
        {
            if (argCount < 0) throw new ArgumentException("参数不能小于0", nameof(argCount));
            ArgCount = argCount;
        }


        /// <inheritdoc/>
        public override object Run(ref int index, List<CommandObject> commandList)
        {
            if (HasPrintList(ref index, commandList)) return null;

            //

            if (commandList == null) throw new ArgumentNullException(nameof(commandList));
            int rightCount = commandList.Count - index - 1;
            if (ArgCount > rightCount) throw new Exceptions.CommandException(exceptionmessage: $"缺少{ArgCount - rightCount}个参数");

            object[] args = new object[ArgCount];

            for (int i = 0; i < ArgCount; ++i)
            {
                ++index;

                object arg = commandList[index].Run(ref index, commandList);
                args[i] = arg;
            }

            return OnRuning(ref index, commandList, args);
        }

        /// <summary>
        /// 调用<see cref="Runing"/>
        /// </summary>
        public virtual object OnRuning(ref int index, List<CommandObject> commandList, object[] args)
        {
            Runing?.Invoke(args);

            return null;
        }

        /// <summary>
        /// 参数中是否存在<see cref="CommandPrintList"/>
        /// </summary>
        public virtual bool HasPrintList(ref int index, List<CommandObject> commandList)
        {
            for (int i = 0;
                i < ArgCount &&
                i + index + 1 < commandList.Count;
                ++i)
            {
                if (commandList[index + i + 1] is CommandPrintList cpl == false) continue;

                index += i + 1;
                cpl.Run(ref index, commandList);
                return true;
            }

            return false;
        }
    }
}
