using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Talamus_ContentManager
{
    //Control element for bookparts 
    public class BookPart : Canvas
    {
        //
        //CONSTRUCTOR
        //
        public BookPart()
        {
            // Look of Canvas
            List<GradientStop> gradientStops = new List<GradientStop>();
            gradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFB, 0xFB, 0xFB), 0));
            gradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDB, 0x91, 0xF1), 1));
            this.Width = 297;
            this.Height = 139;
            this.Background = new LinearGradientBrush(endPoint: new Point(0.5, 1), startPoint: new Point(0.5, 0), gradientStopCollection: new GradientStopCollection(gradientStops));
            this.Cursor = Cursors.Hand;

            // Events on Canvas
            this.MouseMove += BookPart_MouseMove;
            this.MouseDown += BookPart_MouseDown;
            this.MouseUp += BookPart_MouseUp;
            this.Selection += BookPart_Selection;
            this.SelectionPermament += BookPart_SelectionPermament;
            this.FirstPageChanged += BookPart_FirstPageChanged;

            //Border
            border = new Border();
            border.Width = 297;
            border.Height = 139;
            border.Visibility = Visibility.Hidden;
            border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            border.BorderThickness = new Thickness(2);

            //Border2 Selection permament
            borderSP = new Border();
            borderSP.Width = 300;
            borderSP.Height = 141;
            borderSP.Visibility = Visibility.Hidden;
            borderSP.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 252, 00, 255));
            borderSP.BorderThickness = new Thickness(4);

            //Label Main Part
            lblFirstpage = new Label();
            lblFirstpage.Content = "\tFIRST PAGE";
            lblFirstpage.Visibility = Visibility.Hidden;
            lblFirstpage.RenderTransform = new TranslateTransform(28, 100);
            lblFirstpage.Foreground = new SolidColorBrush(Colors.Red);
            lblFirstpage.FontSize = 14;
            lblFirstpage.Width = 200;
            lblFirstpage.Height = 28;
            lblFirstpage.HorizontalAlignment = HorizontalAlignment.Center;
            lblFirstpage.VerticalAlignment = VerticalAlignment.Top;
            lblFirstpage.Background = new SolidColorBrush(Colors.Yellow);
            lblFirstpage.Margin = new Thickness(0);

            // Text box Title
            tbTitle = new TextBox();
            tbTitle.Text = "Default Title";
            tbTitle.RenderTransform = new TranslateTransform(28, 26);
            tbTitle.TextWrapping = TextWrapping.Wrap;
            tbTitle.Width = 200;
            tbTitle.Height = 33;
            tbTitle.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xC2, 0xD4, 0xFF));
            tbTitle.FontSize = 14;
            tbTitle.HorizontalAlignment = HorizontalAlignment.Left;
            tbTitle.VerticalAlignment = VerticalAlignment.Center;
            tbTitle.TextChanged += TbTitle_TextChanged;

            // Button `Edit Content`
            btnEditContent = new Button();
            btnEditContent.Content = "Edit Content";
            btnEditContent.RenderTransform = new TranslateTransform(28, 70);
            btnEditContent.HorizontalAlignment = HorizontalAlignment.Left;
            btnEditContent.VerticalAlignment = VerticalAlignment.Top;
            btnEditContent.Width = 200;
            btnEditContent.Height = 48;
            btnEditContent.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x6A, 0x4F, 0xF1));
            btnEditContent.FontSize = 14;
            btnEditContent.Click += BtnEditContent_Click;

            // Label            
            lblTitle = new Label();
            lblTitle.Content = "Title:";
            lblTitle.RenderTransform = new TranslateTransform(10, 0);
            lblTitle.HorizontalAlignment = HorizontalAlignment.Left;
            lblTitle.VerticalAlignment = VerticalAlignment.Center;

            //RadioButtons
            rbConnectionOne = new RadioButton();
            rbConnectionOne.Content = "";
            rbConnectionOne.RenderTransform = new TranslateTransform(282, 69);
            rbConnectionOne.Click += RbConnectionOne_Click;


            // Add Children
            this.Children.Add(tbTitle);
            this.Children.Add(btnEditContent);
            this.Children.Add(lblTitle);
            this.Children.Add(rbConnectionOne);
            this.Children.Add(border);
            this.Children.Add(borderSP);
            this.Children.Add(lblFirstpage);
        }

        //
        //PRIVATE FIELDS
        //
        private string _title = "Default Title";
        private Point _positionInBlock;
        private bool _firstPage = false;
        private bool _selectedPermament = false;
        private bool _selected = false;
        private Point _position = new Point(0, 0);
        private TextBox tbTitle;
        private Button btnEditContent;
        private Label lblTitle;
        private RadioButton rbConnectionOne;
        //border for selection
        private Border border;
        //border for permament selection
        private Border borderSP;
        private Label lblFirstpage;
        private bool _demoPart = false;

        //
        //Delegates and events
        //
        public delegate void FirstPageHandler(bool value);
        // Invoke when change _firstPage field
        public event FirstPageHandler FirstPageChanged;
        public delegate void PositionChangedHandler(Point position);
        //Invoke when position changed
        public event PositionChangedHandler PositionChanged;
        public delegate void rbClickHandler(Point rbLocation, object sender);
        //invoke when radiobutton pressed
        public event rbClickHandler rbClick;
        public delegate void SelectionPermamentHandler(bool selected);
        //Invoke when _selectedPermament changed
        public event SelectionHandler SelectionPermament;
        public delegate void SelectionHandler(bool selected);
        //Invoke when _selected changed
        public event SelectionHandler Selection;

        //
        //Properties
        //
        public bool Demo
        {
            get
            {
                return _demoPart;
            }
            set
            {
                _demoPart = value;
                if (value)
                {
                    PaintDemo();
                }
                else
                {
                    PaintNormal();
                }
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                tbTitle.Text = value;
            }
        }
        public string Content { get; set; } = String.Empty;
        public bool FirstPage
        {
            get
            {
                return _firstPage;
            }
            set
            {
                this._firstPage = value;
                this.FirstPageChanged?.Invoke(value);
            }
        }
        public Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                this.RenderTransform = new TranslateTransform(value.X, value.Y);
                PositionChanged?.Invoke(value);
            }
        }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public bool SelectedPermament
        {
            get
            {
                return _selectedPermament;
            }
            set
            {
                this._selectedPermament = value;
                this.SelectionPermament?.Invoke(value);
            }
        }
        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                Selection?.Invoke(value);
            }
        }
        //
        //PRIVATE METHODS
        //
        private void PaintNormal()
        {
            lblTitle.Content = "Title:";
            List<GradientStop> gradientStops = new List<GradientStop>();
            gradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFB, 0xFB, 0xFB), 0));
            gradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDB, 0x91, 0xF1), 1));
            this.Background = new LinearGradientBrush(endPoint: new Point(0.5, 1), startPoint: new Point(0.5, 0), gradientStopCollection: new GradientStopCollection(gradientStops));

        }
        private void PaintDemo()
        {
            lblTitle.Content = "(DEMO) Title:";
            List<GradientStop> gradientStops = new List<GradientStop>();
            gradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xA4, 0x23, 0x23), 0));
            gradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x9C, 0x23, 0xA4), 1));
            this.Background = new LinearGradientBrush(endPoint: new Point(0.5, 1), startPoint: new Point(0.5, 0), gradientStopCollection: new GradientStopCollection(gradientStops));
        }

        private void TbTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            Title = tbTitle.Text;
        }
        // opening new window  to edit content
        private void BtnEditContent_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow(Content);
            if (ew.ShowDialog() == true)
            {
                Content = ew.Content;
            }
        }

        private void BookPart_FirstPageChanged(bool value)
        {
            if (value)
            {
                lblFirstpage.Visibility = Visibility.Visible;
            }
            else
            {
                lblFirstpage.Visibility = Visibility.Hidden;
            }
        }

        private void BookPart_SelectionPermament(bool selected)
        {
            if (selected)
            {
                borderSP.Visibility = Visibility.Visible;
            }
            else
            {
                borderSP.Visibility = Visibility.Hidden;
            }
        }

        private void RbConnectionOne_Click(object sender, RoutedEventArgs e)
        {
            var container = VisualTreeHelper.GetParent(this) as UIElement;
            rbClick?.Invoke(Mouse.GetPosition(container), this);
        }
        private void BookPart_Selection(bool selected)
        {
            if (selected)
            {
                border.Visibility = Visibility.Visible;
                Panel.SetZIndex(this, 1);
            }
            else
            {
                border.Visibility = Visibility.Hidden;
                Panel.SetZIndex(this, 0);
            }
        }

        private void BookPart_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
        }

        private void BookPart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _positionInBlock = Mouse.GetPosition(this);
            this.CaptureMouse();
        }

        private void BookPart_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                var container = VisualTreeHelper.GetParent(this) as UIElement;

                if (container == null)
                    return;

                var mousePosition = e.GetPosition(container);

                double dX = mousePosition.X - _positionInBlock.X;
                double dY = mousePosition.Y - _positionInBlock.Y;

                double pW = ((Canvas)container).ActualWidth;
                double pH = ((Canvas)container).ActualHeight;

                // move the usercontrol
                if (dX + this.Width < pW && dY + this.Height < pH && dY > 0 && dX > 0)
                {
                    Position = new Point(mousePosition.X - _positionInBlock.X, mousePosition.Y - _positionInBlock.Y);
                }
                else if (dX + this.Width < pW && dX > 0)
                {
                    Position = new Point(mousePosition.X - _positionInBlock.X, this.RenderTransform.Value.OffsetY);

                }
                else if (dY + this.Height < pH && dY > 0)
                {
                    Position = new Point(this.RenderTransform.Value.OffsetX, mousePosition.Y - _positionInBlock.Y);
                }

            }
        }
    }
}
