using CommandHelp.Exceptions;
using System;
using System.Collections.Generic;

namespace CommandHelp
{
    /// <summary>
    /// 运行指令
    /// </summary>
    public static class RunCommand
    {
        /// <summary>
        /// 解析并运行指令, 解析格式>解析>运行
        /// </summary>
        /// <returns>如果有异常则返回<see cref="CommandException"/></returns>
        public static CommandException ParseRun(string command, List<CommandObject> cmdList)
        {
            (List<CommandObject> cos, CommandException cex) v = ParseCommand.Parse(command, cmdList);

            if (v.cex != null) return v.cex;
            if (v.cos == null || v.cos.Count < 1) return null;

            try
            {
                for (int i = 0; i < v.cos.Count; ++i)
                {
                    _ = v.cos[i].Run(ref i, v.cos);
                }

                return null;
            }
            catch (CommandException cex)
            {
                return cex;
            }
            catch (Exception ex)
            {
                return new CommandException(exceptionmessage: "未知异常", ex: ex);
            }
        }
    }
}
