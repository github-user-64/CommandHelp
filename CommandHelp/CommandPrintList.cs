using System;
using System.Collections.Generic;

namespace CommandHelp
{
    /// <summary>
    /// 指令:显示指令列表
    /// </summary>
    public class CommandPrintList : CommandMethod
    {
        /// <inheritdoc/>
        public override string Text => "?";
        /// <summary>
        /// 指令列表
        /// </summary>
        public List<CommandObject> Cos { get; protected set; } = null;
        /// <summary>
        /// 输出位置
        /// </summary>
        public Action<string> Print { get; protected set; } = null;

        /// <summary/>
        public CommandPrintList(List<CommandObject> cos, string tip = null, Action<string> print = null)
        {
            Cos = cos;
            Print = print;
            TipText = tip;
        }

        /// <inheritdoc/>
        public override object OnRuning(ref int index, List<CommandObject> commandList, object[] args)
        {
            base.OnRuning(ref index, commandList, args);

            string s = null;

            Cos?.ForEach(i =>
            {
                if (i == this) return;
                if (i == null) return;

                if (i is CommandeEnum enu)
                {
                    foreach (string enui in enu.Enums)
                    {
                        if (s == null)
                        {
                            s = $"{enui}";
                        }
                        else
                        {
                            s += $", {enui}";
                        }
                    }

                    return;
                }

                if (s == null)
                {
                    s = $"{i.Text}";
                }
                else
                {
                    s += $", {i.Text}";
                }
            });

            if (s == null) s = "no cmd";
            if (TipText != null) s = $"{s}//{TipText}";
            Print?.Invoke(s);

            return this;
        }
    }
}
