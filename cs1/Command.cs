using CommandHelp;
using CommandHelp.Exceptions;
using System.Windows;

namespace cs1
{
    public partial class MainWindow
    {
        public void CommandRun(string command)
        {
            CommandException ex = RunCommand.ParseRun(command, GetCO().SubCommand);

            if (ex != null)
            {
                if (ex as CommandLackException != null || ex.InnerException as CommandLackException != null)
                {
                    AddMsg($"\n指令缺失");
                    if (ex.Line > -1) AddMsg($"位于:{command.Substring(0, ex.Line)}<");
                }
                else
                if (ex as CommandParseException != null || ex.InnerException as CommandParseException != null)
                {
                    AddMsg($"\n指令错误");
                    if (ex.Line > -1) AddMsg($"位于: {command.Substring(0, ex.Line)}>{command.Substring(ex.Line)?.TrimStart()}<");
                }
                else
                {
                    AddMsg($"\n指令错误:{ex.ExceptionMessage}");
                    if (ex.Line > -1) AddMsg($"位于: {command.Substring(0, ex.Line)}>{command.Substring(ex.Line)?.TrimStart()}<");
                }
            }
        }

        public class Ctitle : CommandMethod
        {
            public Ctitle() : base("title", 1)
            {
                SubCommand.Add(new Cstring());
            }

            public override object? OnRuning(ref int index, List<CommandObject> commandList, object[] args)
            {
                MainWindow? mw = Application.Current.MainWindow as MainWindow;
                if (mw != null)
                {
                    mw.AddMsg($"设置标题[{args[0]}]");
                }
                
                return null;
            }
        }

        public class Cgamemode : CommandMethod
        {
            public Cgamemode() : base("gamemode", 3)
            {
                CommandeEnum mod = new CommandeEnum(false, "0", "1");
                CommandeEnum who = new CommandeEnum(true, "all", "me");
                CommandeEnum item1 = new CommandeEnum(true, "s1", "s2");
                mod.SubCommand.Add(who);
                who.SubCommand.Add(item1);
                SubCommand.Add(mod);
            }

            public override object? OnRuning(ref int index, List<CommandObject> commandList, object[] args)
            {
                int? arg1 = args[0] as int?;
                int? arg2 = args[1] as int?;
                int? arg3 = args[2] as int?;
                if (arg1 == null) throw new CommandException(exceptionmessage: "缺少参数");

                string text = $"模式设为{(arg1 == 0 ? "生存" : "创造")}";

                if (arg2 == 0) text = $"已将所有的{text}";
                if (arg2 == 1) text = $"已将自己的{text}";

                if (arg3 == 0) text += "选项1";
                if (arg3 == 1) text += "选项2";

                (Application.Current.MainWindow as MainWindow)?.AddMsg(text);

                return null;
            }
        }

        public class Csc : CommandMethod
        {
            public Csc() : base("sc", 2)
            {
                CintV3 point = new CintV3();
                point.SubCommand.Add(new CommandeEnum(false, "a1", "a2", "item3"));
                SubCommand.Add(point);
            }

            public override object? OnRuning(ref int index, List<CommandObject> commandList, object[] args)
            {
                int[]? arg1 = args[0] as int[];
                string? arg2 = args[1].ToString();

                if (arg1 == null) throw new CommandException(exceptionmessage: "缺少参数");
                if (arg2 == null) throw new CommandException(exceptionmessage: "缺少参数");

                (Application.Current.MainWindow as MainWindow)?.AddMsg($"在{arg1[0]} {arg1[1]} {arg1[2]}生成{arg2}");

                return null;
            }
        }

        public class Ctime : CommandObject
        {
            public Ctime() : base("time")
            {
                CommandMethod add = new CommandMethod("add", 1);
                CommandMethod set = new CommandMethod("set", 1);

                add.SubCommand.Add(new CommandInt());
                set.SubCommand.Add(new CommandKeyVal("a1", "t1"));
                set.SubCommand.Add(new CommandKeyVal("a2", "t2"));
                set.SubCommand.Add(new CommandInt());
                SubCommand.Add(add);
                SubCommand.Add(set);

                add.Runing += Add_Runing;
                set.Runing += Set_Runing;
            }

            private void Add_Runing(object[] args)
            {
                int? arg1 = args[0] as int?;
                if (arg1 == null) throw new CommandException(exceptionmessage: "缺少参数");

                (Application.Current.MainWindow as MainWindow)?.AddMsg($"时间增加{arg1.Value}");
            }

            private void Set_Runing(object[] args)
            {
                int? arg1 = args[0] as int?;
                string? arg1_2 = args[0] as string;
                if (arg1 == null)
                {
                    if (arg1_2 == null) throw new CommandException(exceptionmessage: "缺少参数");

                    int time = -1;
                    if (arg1_2 == "t1") time = 100;
                    if (arg1_2 == "t2") time = 1000;

                    if (time == -1) throw new CommandException(exceptionmessage: $"参数[{arg1_2}]异常");

                    (Application.Current.MainWindow as MainWindow)?.AddMsg($"时间设为{time}");

                }
                else
                {
                    (Application.Current.MainWindow as MainWindow)?.AddMsg($"时间设为{arg1}");
                }
            }
        }

        public static bool gamerule_keepInventory = false;
        public class Cgamerule : CommandObject
        {
            public Cgamerule() : base("gamerule")
            {
                CommandMethod keepInventory = new CommandMethod("keepInventory", 1);
                keepInventory.SubCommand.Add(new CommandeEnum(true, "true", "false", "reset"));

                SubCommand.Add(keepInventory);

                keepInventory.Runing += KeepInventory_Runing;
            }

            private void KeepInventory_Runing(object[] obj)
            {
                int? arg1 = obj[0] as int?;
                if (arg1 == 0)
                {
                    gamerule_keepInventory = true;
                    (Application.Current.MainWindow as MainWindow)?.AddMsg($"模式keepInventory改为{gamerule_keepInventory}");
                }
                else
                if (arg1 == 1)
                {
                    gamerule_keepInventory = false;
                    (Application.Current.MainWindow as MainWindow)?.AddMsg($"模式keepInventory改为{gamerule_keepInventory}");
                }
                else
                if (arg1 == 2)
                {
                    gamerule_keepInventory = false;
                    (Application.Current.MainWindow as MainWindow)?.AddMsg($"模式keepInventory改为{gamerule_keepInventory}");
                }
                else
                if (arg1 == -1)
                {
                    (Application.Current.MainWindow as MainWindow)?.AddMsg($"模式keepInventory为{gamerule_keepInventory}");
                }
            }
        }

        public class Cstring : CommandValue<string?>
        {
            public override string Text => "<string>";
            protected override string ArgConvertThrow(string arg) => arg;
            protected override string? GetDefault() => null;
        }

        public class CintV3 : CommandValues<int>
        {
            public override string Text => "<int><int><int>";

            public CintV3() : base(3) { }

            protected override int ArgConvertThrow(string arg) => int.Parse(arg);

            protected override int GetDefault()
            {
                throw new Exception();
            }
        }
    }
}
