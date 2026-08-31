using System;
using System.Collections.Generic;

namespace CommandHelp
{
    /// <summary>
    /// 指令:返回值
    /// </summary>
    public abstract class CommandValue : CommandObject
    {
        /// <summary>
        /// 指示指令是否可以不写
        /// </summary>
        public readonly bool IsVariable;
        /// <summary>
        /// 指示该指令未写, 是默认值
        /// </summary>
        public bool IsDefault { get; protected set; } = false;

        /// <summary/>
        public CommandValue(bool isVariable = false, string text = null) : base(text)
        {
            IsVariable = isVariable;
        }
    }

    /// <summary>
    /// 指令:返回指定类型的值
    /// </summary>
    /// <typeparam name="T">指定类型</typeparam>
    public abstract class CommandValue<T> : CommandValue
    {
        /// <summary>
        /// 类型名
        /// </summary>
        public virtual string TypeName { get; } = null;
        /// <summary>
        /// <see cref="Run"/>返回的值
        /// </summary>
        protected T RetVal = default;

        /// <inheritdoc/>
        public CommandValue(bool isVariable = false, string text = null) : base(isVariable, text) { }


        /// <summary>
        /// 将参数转为值, 可以报异常
        /// </summary>
        protected abstract T ArgConvertThrow(string arg);
        /// <summary>
        /// 返回默认值
        /// </summary>
        /// <returns>默认值</returns>
        protected abstract T GetDefault();


        /// <summary>
        /// 返回指定类型的值
        /// </summary>
        /// <returns>指定类型的值</returns>
        public override object Run(ref int index, List<CommandObject> commandList)
        {
            return RetVal;
        }

        /// <inheritdoc/>
        public override CommandObject Parse(string command)
        {
            if (command == "" && IsVariable)
            {
                IsDefault = true;
                RetVal = GetDefault();
                return this;
            }

            //值类型解析不出来就直接报错

            try
            {
                RetVal = ArgConvertThrow(command);
            }
            catch (Exception ex)
            {
                throw new Exceptions.CommandException(exceptionmessage: $"参数[{command}]类型不为<{TypeName ?? typeof(T).Name}>:{ex.Message}", ex: ex);
            }

            return this;
        }

        /// <inheritdoc/>
        public override (string cmdParse, string cmd) ParseFormat(string command)
        {
            (string cmdParse, string cmd) v = base.ParseFormat(command);

            if (v.cmdParse == null && IsVariable) return ("", v.cmd);//如果返回null会被当成解析失败,所以返回""

            return v;
        }
    }
}
