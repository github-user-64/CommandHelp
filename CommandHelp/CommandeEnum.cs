using System.Collections.Generic;

namespace CommandHelp
{
    /// <summary>
    /// 指令:返回选中项的下标, 默认<see langword="-1"/>
    /// </summary>
    public class CommandeEnum : CommandValue<int>
    {
        /// <summary>
        /// 选项
        /// </summary>
        public readonly string[] Enums;
        /// <summary>
        /// 每个<see cref="Enums"/>的提示文本
        /// </summary>
        public virtual string[] TipTexts { get; set; } = null;
        private int _rValue;


        /// <param name="IsVariable">是否可以不写</param>
        /// <param name="enums">选项</param>
        public CommandeEnum(bool IsVariable, params string[] enums) : base(IsVariable)
        {
            Enums = enums;
        }


        /// <summary>
        /// 返回选择了第几个<see cref="Enums"/>
        /// </summary>
        /// <returns></returns>
        public override object Run(ref int index, List<CommandObject> commandList)
        {
            return _rValue;
        }

        /// <inheritdoc/>
        public override CommandObject Parse(string command)
        {
            if (command == "" && IsVariable)
            {
                IsDefault = true;
                _rValue = GetDefault();
                return this;
            }

            _rValue = ArgConvertThrow(command);

            return this;
        }

        /// <inheritdoc/>
        protected override int ArgConvertThrow(string arg)
        {
            for (int i = 0; i < Enums.Length; ++i)
            {
                if (arg == Enums[i]) return i;
            }

            throw new Exceptions.CommandException(exceptionmessage: $"未找到与参数[{arg}]对应的值");
        }

        /// <summary>
        /// 返回<see langword="-1"/>
        /// </summary>
        /// <returns><see langword="-1"/></returns>
        protected override int GetDefault()
        {
            return -1;
        }
    }
}
