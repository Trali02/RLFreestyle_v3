using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RLFreestyle_v3.MVVM.Model
{
	class ConsoleOutput : TextWriter
	{
        TextBox textBox = null;
        public ConsoleOutput(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public override void Write(string value)
        {
            base.Write(value);
            textBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBox.AppendText(value.ToString());
            }));
        }
        public override void WriteLine(string value)
        {
            base.WriteLine(value);
            textBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBox.AppendText(value.ToString() + "\n");
            }));
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
