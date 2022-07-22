using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Talamus_ContentManager.Models;

namespace Talamus_ContentManager
{
  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        List<BookPart> BookParts { get; set; }
        List<Connection> Connections { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Connections = new List<Connection>();
            BookParts = new List<BookPart>();

            this.MouseMove += MainWindow_MouseMove;
            this.MouseDown += MainWindow_MouseDown;
            LoadSave();
        }

        private async Task LoadSave()
        {
            try
            {
                BookSave save = new BookSave();
                using (FileStream fs = new FileStream("save", FileMode.Open))
                {
                     save = await JsonSerializer.DeserializeAsync<BookSave>(fs);
                }

                mainCanvas.Width = save.CanvasWidth;
                mainCanvas.Height = save.CanvasHeight;

                foreach(PartSave bs in save.Parts)
                {
                    BookPart bp = new BookPart()
                    {
                        Content = bs.Content,
                        Title = bs.Title,
                        Position = bs.Position,
                       RenderTransform = new TranslateTransform(bs.Position.X, bs.Position.Y),
                       Guid = bs.Guid,
                       FirstPage = bs.FirstPage
                };
                    bp.MouseEnter += A_MouseEnter;
                    bp.MouseLeave += A_MouseLeave;
                    bp.MouseDown += A_MouseDown;
                    bp.rbClick += A_rbClick;
                    BookParts.Add(bp);
                    mainCanvas.Children.Add(bp);
                }

                foreach(ConnectionSave s in save.Connections)
                {
                    Connection con = new Connection()
                    {
                        IsConnected = true,
                        Start = s.Start,
                        End = s.End,
                        StartPage = BookParts.Find(b => b.Guid == s.StartGuid),
                        EndPage = BookParts.Find(b => b.Guid == s.EndGuid)
                    };
                    con.MouseRightButtonDown += A_MouseRightButtonDown;
                    con.EndPage.PositionChanged += (Point p) => { con.End = new Point(p.X, p.Y + 69.5); };
                    con.StartPage.PositionChanged += (Point p) => { con.Start = new Point(p.X + 287, p.Y + 75); };

                    Connections.Add(con);
                    mainCanvas.Children.Add(con);
                }
            }
            catch
            {

            }
        }
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            for(int i = 0; i < Connections.Count; i++)
            {
                if (!Connections[i].IsConnected)
                {
                    mainCanvas.Children.Remove(Connections[i]);
                    Connections.Remove(Connections[i]);
                }
            }

            foreach (var item in BookParts)
            {
                item.SelectedPermament = false;
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            foreach(var item in Connections)
            {
                if (!item.IsConnected)
                {
                    item.End = Mouse.GetPosition(mainCanvas);
                }
            }
        }

        BookPart GetPart()
        {
            BookPart a = new BookPart();
            a.Guid = Guid.NewGuid();
            a.MouseEnter += A_MouseEnter;
            a.MouseLeave += A_MouseLeave;
            a.MouseDown += A_MouseDown;
            a.rbClick += A_rbClick;
            a.RenderTransform = new TranslateTransform(svMainScroll.HorizontalOffset + 100, svMainScroll.VerticalOffset + 100);

            return a;
        }

        private void A_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BookPart bp = (BookPart)sender;

            for (int i = 0; i < Connections.Count; i++)
            {
                if (!Connections[i].IsConnected)
                {
                    if (Connections[i].StartPage != bp)
                    {
                        var c = Connections.FirstOrDefault(c => c.StartPage == Connections[i].StartPage && c.EndPage == bp);
                        if (c != null)
                        {
                            mainCanvas.Children.Remove(Connections[i]);
                            Connections.Remove(Connections[i]);
                            break;
                        }

                        var conn = Connections[i];
                        conn.End = new Point(bp.Position.X, bp.Position.Y+75);
                        conn.EndPage = bp;
                        conn.EndPage.PositionChanged+=(Point p) => { conn.End = new Point(p.X, p.Y+69.5); };
                        conn.IsConnected = true;

                        
                    }
                    else
                    {
                        mainCanvas.Children.Remove(Connections[i]);
                        Connections.Remove(Connections[i]);
                    }
                }
            }

            PermamentSelect(sender);
        }

        private void PermamentSelect(object sender)
        {
            BookPart bp = (BookPart)sender;

            foreach(var item in BookParts)
            {
                item.SelectedPermament = false;
            }

            bp.SelectedPermament = true;
        }

        private void A_rbClick(Point rbLocation, object sender)
        {
            Connection a = new Connection()
            {
                StartPage = (BookPart)sender,
                Start = rbLocation,
                End = rbLocation,
            };
            //to delete connection rb click
            a.MouseRightButtonDown += A_MouseRightButtonDown;
            a.StartPage.PositionChanged += (Point position) => { a.Start = new Point(position.X+287,position.Y+75);};
            Connections.Add(a);
            mainCanvas.Children.Add(a);
        }

        private void A_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
                mainCanvas.Children.Remove(sender as Connection);
                Connections.Remove(sender as Connection);
        }

        private void A_MouseLeave(object sender, MouseEventArgs e)
        {
            BookPart a = (BookPart)sender;
            a.Selected = false;
        }

        private void A_MouseEnter(object sender, MouseEventArgs e)
        {
            BookPart a = (BookPart)sender;
            a.Selected = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tbCanvasWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbCanvasWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var d = Convert.ToDouble(tbCanvasWidth.Text);
                if(d<=100000 && d > 0)
                {
                    mainCanvas.Width = d;
                }
                else if(d>100000) { tbCanvasWidth.Text = "100000"; } else
                {
                    tbCanvasWidth.Text = "0";
                }
                
            }
            catch
            {

            }
        }
        private void tbCanvasHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var d = Convert.ToDouble(tbCanvasHeight.Text);
                if (d <= 100000 && d > 0)
                {
                    mainCanvas.Height = d;
                }
                else if (d > 100000) { tbCanvasHeight.Text = "100000"; }
                else
                {
                    tbCanvasHeight.Text = "0";
                }

            }
            catch
            {

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            BookPart bp = GetPart();
            BookParts.Add(bp);
            mainCanvas.Children.Add(bp);
        }

        private void btnMakeFirst_Click(object sender, RoutedEventArgs e)
        {
            bool ok = false;

            foreach (BookPart bp in BookParts)
            {
                    bp.FirstPage = false;
            }

            foreach (BookPart bp in BookParts)
            {
                if (bp.SelectedPermament)
                {
                    bp.FirstPage = true;
                    ok = true;
                    break;
                }
            }

            if (ok)
            {
                lblMakingFirstMsg.Content = "OK";
            }
            else
            {
                lblMakingFirstMsg.Content = "NOTHING IS SELECTED, FUCK YOU!";
            }
        }
        BookSave MakeSave()
        {
            BookSave save = new BookSave()
            {
                CanvasHeight = mainCanvas.ActualHeight,
                CanvasWidth = mainCanvas.ActualWidth,
                Parts = new List<PartSave>(),
                Connections = new List<ConnectionSave>()
            };

            foreach (BookPart bp in BookParts)
            {
                save.Parts.Add(new PartSave()
                {
                    Guid = bp.Guid,
                    Content = bp.Content,
                    Position = bp.Position,
                    Title = bp.Title,
                    FirstPage = bp.FirstPage
                });
            }

            foreach (Connection c in Connections)
            {
                if (c.IsConnected)
                {
                    save.Connections.Add(new ConnectionSave()
                    {
                        End = c.End,
                        Start = c.Start,
                        EndGuid = c.EndPage.Guid,
                        StartGuid = c.StartPage.Guid
                    });
                }
            }
            return save;
        }
        private void miSave_Click(object sender, RoutedEventArgs e)
        {
            BookSave save = MakeSave();

            using (FileStream fs = new FileStream("save", FileMode.Create))
            {
                
                 JsonSerializer.Serialize<BookSave>(fs, save);
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            BookPart bp = BookParts.Find(b => b.FirstPage);

            if(bp == null)
            {
                MessageBox.Show("You need to specify first page of the book", "Nu ti ebanutiy?");
            }
            else
            {
                UploadWindow uw = new UploadWindow(MakeSave());
                if (uw.ShowDialog() == true)
                {
                    File.WriteAllText("save", "");
                    mainCanvas.Children.Clear();
                    Connections.Clear();
                    BookParts.Clear();
                }
            }
           
        }

        private void miDel_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("save", "");
        }

        private void miReset_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Children.Clear();
            Connections.Clear();
            BookParts.Clear();
        }

        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            BookPart bp = BookParts.Find(b => b.SelectedPermament);

            if (bp == null)
            {
                MessageBox.Show("You Selected Nothing", "Eblan?");
            }
            else
            {
                mainCanvas.Children.Remove(bp);
                BookParts.Remove(bp);

                List<Connection> conToRemove = new List<Connection>();

                foreach(var item in Connections)
                {
                    if(item.StartPage==bp || item.EndPage == bp)
                    {
                        mainCanvas.Children.Remove(item);
                        conToRemove.Add(item);
                    }
                }
                foreach(var item in conToRemove)
                {
                    Connections.Remove(item);
                }
            }
        }
    }
}
