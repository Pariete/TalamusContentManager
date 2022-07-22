using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Talamus_ContentManager.Models;

namespace Talamus_ContentManager
{
    /// <summary>
    /// Логика взаимодействия для UploadWindow.xaml
    /// </summary>
    public partial class UploadWindow : Window
    {
        static TalamusContext _db;
        BookSave book;
        public UploadWindow(BookSave bs)
        {
            InitializeComponent();
            book = bs;
        }

        private void tbUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _db = new TalamusContext(new DbContextOptionsBuilder<TalamusContext>().UseSqlServer(tbConnectionString.Text).Options);

                Book b = new Book()
                {
                    Author = tbAuthor.Text,
                    Description = tbDescription.Text,
                    ImageUrl = tbImageURL.Text,
                    Title = tbTitle.Text,
                    Price = Convert.ToInt64(tbPrice.Text)
                };

                foreach(var item in book.Parts)
                {
                    Part p = new Part()
                    {
                        Guid = item.Guid,
                        Book = b,
                        Content = item.Content,
                        Title = item.Title,
                        Created = DateTime.Now,
                        PageNumber = (item.FirstPage) ? 1 : 2
                    };
                    b.Parts.Add(p);
                }

                _db.Books.Add(b);
                _db.SaveChanges();

                foreach(var item in book.Connections)
                {
                    Part one = _db.Parts.FirstOrDefault(p => p.Guid == item.StartGuid);
                    Part two = _db.Parts.FirstOrDefault(p => p.Guid == item.EndGuid);

                    Subsequent s = new Subsequent()
                    {
                        Part = one,
                        NextPart = two
                    };

                    _db.Subsequents.Add(s);
                }
                _db.SaveChanges();
                this.DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message+"\n"+ ex.StackTrace +"\n"+ex.ToString());
                this.DialogResult = false;
            }
        }
    }
}
