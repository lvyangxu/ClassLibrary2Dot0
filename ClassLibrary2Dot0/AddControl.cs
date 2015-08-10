using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace ClassLibrary2Dot0
{
    public class AddControl
    {
        public delegate void buttonClickHandler(object sender, EventArgs e);

        /// <summary>
        /// 动态增加一个button,指定显示的文本和点击事件执行的委托
        /// </summary>
        /// <param name="Controls1">要增加的控件集合</param>
        /// <param name="text">button文本</param>
        /// <param name="buttonClickHandler1">绑定点击后执行的委托</param>
        /// <returns></returns>
        public Button addButton(Control.ControlCollection Controls1, string text,int buttonWidth,int buttonHeight, buttonClickHandler buttonClickHandler1)
        {
            Button Button1 = new Button();
            Button1.Text = text;
            Button1.Size = new Size(buttonWidth, buttonHeight);
            Button1.Click += (sender,e) =>
            {
                buttonClickHandler1(sender,e);
            };

            Controls1.Add(Button1);
            return Button1;
        }
    }
}
