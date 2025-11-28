using System;
using System.Collections.Generic;

namespace CommandHelp
{
    /// <summary>
    /// 显示指令列表
    /// </summary>
    public class CommandPrintList : CommandMethod
    {
        public override string Text => "?";
        public List<CommandObject> Cos { get; protected set; } = null;
        public Action<string> Print { get; protected set; } = null;
        public string Tip { get; protected set; } = null;

        /// <summary>
        /// 显示指令列表
        /// </summary>
        /// <param name="cos"></param>
        /// <param name="tip"></param>
        /// <param name="print"></param>
        public CommandPrintList(List<CommandObject> cos, string tip = null, Action<string> print = null)
        {
            Cos = cos;
            Tip = tip;
            Print = print;
        }

        public override object OnRuning(ref int index, List<CommandObject> commandList, object[] args)
        {
            base.OnRuning(ref index, commandList, args);

            string s = null;

            Cos?.ForEach(i =>
            {
                if (i == this) return;
                if (i == null) return;

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
            if (Tip != null) s = $"{s}//{Tip}";
            Print?.Invoke(s);

            return this;
        }
    }
}
