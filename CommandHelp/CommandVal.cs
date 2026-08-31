namespace CommandHelp
{
    /// <summary>
    /// 指令:返回<see langword="false"/>
    /// </summary>
    public class CommandFalse : CommandKeyVal
    {
        /// <inheritdoc/>
        public CommandFalse(bool isVariable = false) : base("false", false, isVariable) { }
    }

    /// <summary>
    /// 指令:返回<see langword="true"/>
    /// </summary>
    public class CommandTrue : CommandKeyVal
    {
        /// <inheritdoc/>
        public CommandTrue(bool isVariable = false) : base("true", true, isVariable) { }
    }

    /// <summary>
    /// 指令:返回<see langword="int"/>
    /// </summary>
    public class CommandInt : CommandValue<int>
    {
        /// <inheritdoc/>
        public override string Text => "<int>";

        /// <inheritdoc/>
        protected override int ArgConvertThrow(string arg) => int.Parse(arg);

        /// <inheritdoc/>
        protected override int GetDefault() => default;
    }

    /// <summary>
    /// 指令:返回<see langword="float"/>
    /// </summary>
    public class CommandFloat : CommandValue<float>
    {
        /// <inheritdoc/>
        public override string Text => "<float>";

        /// <inheritdoc/>
        protected override float ArgConvertThrow(string arg) => float.Parse(arg);

        /// <inheritdoc/>
        protected override float GetDefault() => default;
    }

    /// <summary>
    /// 指令:返回<see langword="double"/>
    /// </summary>
    public class CommandDouble : CommandValue<double>
    {
        /// <inheritdoc/>
        public override string Text => "<double>";

        /// <inheritdoc/>
        protected override double ArgConvertThrow(string arg) => double.Parse(arg);

        /// <inheritdoc/>
        protected override double GetDefault() => default;
    }
}
