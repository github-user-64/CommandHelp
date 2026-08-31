namespace CommandHelp
{
    /// <summary>
    /// 指令:指令匹配时返回值
    /// </summary>
    public class CommandKeyVal : CommandValue<object>
    {
        /// <summary/>
        public CommandKeyVal(string key, object val, bool isVariable = false) : base(isVariable, key)
        {
            RetVal = val;
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

            return Text == command ? this : null;
        }

        /// <inheritdoc/>
        protected override object ArgConvertThrow(string arg)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        protected override object GetDefault()
        {
            return RetVal;
        }
    }
}
