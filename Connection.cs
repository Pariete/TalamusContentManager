using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Talamus_ContentManager
{
    public class Connection : Shape
    {
        public Connection()
        {
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 3;
            this.MouseEnter += Connection_MouseEnter;
            this.MouseLeave += Connection_MouseLeave;
        }

        private LineGeometry line = new LineGeometry();
        public bool IsConnected { get; set; } = false;

        public BookPart? StartPage = null;
        public BookPart? EndPage = null;

        public static readonly DependencyProperty dpEnd = DependencyProperty.Register("End", typeof(Point), typeof(Connection),
        new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty dpStart = DependencyProperty.Register("Start", typeof(Point), typeof(Connection),
        new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public Point Start
        {
            get
            {
                return (Point)GetValue(dpStart);
            }
            set
            {
                SetValue(dpStart, value);
            }
        }
        public Point End
        {
            get
            {
                return (Point)GetValue(dpEnd);
            }
            set
            {
                SetValue(dpEnd, value);
            }
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                line.StartPoint = Start;
                line.EndPoint = End;
                return line;
            }
        }
        private void Connection_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Stroke = Brushes.Black;
        }
        private void Connection_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Stroke = Brushes.AliceBlue;
        }
    }
}
