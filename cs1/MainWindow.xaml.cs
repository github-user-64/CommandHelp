using CommandHelp;
using CommandHelp.Exceptions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace cs1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb.Focus())
            {
                int index = tb.SelectionStart;
                string text = tb.Text;

                text = text.Substring(0, index);

                tip(text, GetCO());
            }
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) CommandRun(tb.Text);
        }

        public void AddMsg(string text)
        {
            if (text == null) return;

            msg.Text += $"{(msg.Text == "" ? "" : '\n')}{text}";
        }

        public CommandObject GetCO()
        {
            CommandObject co = new();
            co.SubCommand.Add(new Ctitle());
            co.SubCommand.Add(new Cgamemode());
            co.SubCommand.Add(new Cgamerule());
            co.SubCommand.Add(new Csc());
            co.SubCommand.Add(new Ctime());

            return co;
        }

        public void tip(string command, CommandObject cos)
        {
            tb1.Text = "";
            tb2.Text = "";

            (List<CommandObject> cos, CommandException cex) v = ParseCommand.Parse(command, cos.SubCommand);
            List<CommandObject> cmdList = v.cos;
            CommandException ex = v.cex;
            bool isCmdLack = false;

            if (ex != null)
            {
                if (ex as CommandLackException != null || ex.InnerException as CommandLackException != null)
                {
                    isCmdLack = true;

                    tb1.Text = $"指令缺失";
                    if (ex.Line > -1) tb1.Text += $", 位于:{command.Substring(0, ex.Line)}<";
                }
                else
                if (ex as CommandParseException != null || ex.InnerException as CommandParseException != null)
                {
                    tb1.Text = $"指令错误";
                    if (ex.Line > -1) tb1.Text += $", 位于: {command.Substring(0, ex.Line)}>{command.Substring(ex.Line)?.TrimStart()}<\n错误的指令:>{ex.ExceptionCommand?.TrimStart()}<";
                }
                else
                {
                    tb1.Text = $"指令错误";
                    if (ex.Line > -1) tb1.Text += $", 位于: {command.Substring(0, ex.Line)}>{command.Substring(ex.Line)?.TrimStart()}<\n{ex.ExceptionMessage}";
                }
            }

            //显示c的子指令
            //c为指令对象列表的最后一个
            //如果指令缺失, 如果最后为空格且不为空文本则不变, 否者不显示
            //如果指令异常, 如果有错误部分的指令则不变, 否者不显示
            //如果没异常, 如果最后为空格, 则c为最后一个非可变参数
            CommandObject? c = cmdList.LastOrDefault();
            bool isEndSpace = command?.Length > 0 == true && command[command.Length - 1] == ' ';
            
            if (isCmdLack)
            {
                if (isEndSpace == false && command != "") return;
            }
            else
            if (ex != null)
            {
                int exCmdLen = ex?.ExceptionCommand?.TrimStart().Length ?? 0;
                if (exCmdLen < 1) return;
            }
            else
            if (isEndSpace)
            {
                for (int i = cmdList.Count - 1; i > -1 ; --i)
                {
                    CommandValue? cv = cmdList[i] as CommandValue;
                    c = cmdList[i];

                    if (cv == null) break;
                    if (cv.IsDefault == false) break;
                }
            }

            if (c == null) c = cos;
            string? text = null;

            for (int i = 0; i < c.SubCommand.Count; i++)
            {
                CommandObject subco = c.SubCommand[i];
                if (subco == null) continue;

                if (subco is CommandeEnum ce)
                {
                    for (int i2 = 0; i2 < ce.Enums.Length; i2++) text += $"{(text == null ? "" : '\n')}> {ce.Enums[i2]}";
                }
                else
                {
                    text += $"{(text == null ? "" : '\n')}> {subco.Text}";
                }
            }

            tb2.Text = text ?? "";
        }
    }
}