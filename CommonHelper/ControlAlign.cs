using System.Drawing;


namespace CommonHelper
{
    public class ControlAlign
    {
        public enum AlignType
        {
            TopLeft,
            TopCentre,
            TopRight,
            MiddleLeft,
            MiddleCentre,
            MiddleRight,
            BottomLeft,
            BottomCentre,
            BottomRight
        }

        /// <summary>
        /// 设置子控件相对父控件位置，最好只调用一次，否则会多次注册事件
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="parentCtrl1">父控件</param>
        /// <param name="childCtrl2">子控件</param>
        /// <param name="align">相对位置</param>
        public static void SetControlAlign<T1, T2>(T1 parentCtrl1, T2 childCtrl2, AlignType align = AlignType.MiddleCentre) where T1 : System.Windows.Forms.Control where T2 : System.Windows.Forms.Control
        {
            switch (align)
            {
                case AlignType.MiddleCentre:
                    childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    break;
                case AlignType.TopLeft:
                    childCtrl2.Location = new Point(0, 0);
                    break;
                case AlignType.TopCentre:
                    childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, 0);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, 0);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, 0);
                    break;
                case AlignType.TopRight:
                    childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, 0);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, 0);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, 0);
                    break;
                case AlignType.MiddleLeft:
                    childCtrl2.Location = new Point(0, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(0, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(0, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    break;
                case AlignType.MiddleRight:
                    childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, parentCtrl1.Size.Height / 2 - childCtrl2.Size.Height / 2);
                    break;
                case AlignType.BottomLeft:
                    childCtrl2.Location = new Point(0, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(0, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(0, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    break;
                case AlignType.BottomCentre:
                    childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width / 2 - childCtrl2.Size.Width / 2, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    break;
                case AlignType.BottomRight:
                    childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    parentCtrl1.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    childCtrl2.Resize += (a, b) => childCtrl2.Location = new Point(parentCtrl1.Size.Width - childCtrl2.Size.Width, parentCtrl1.Size.Height - childCtrl2.Size.Height);
                    break;
            }
        }
    }
}
