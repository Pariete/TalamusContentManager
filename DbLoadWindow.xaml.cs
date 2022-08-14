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
    /// Логика взаимодействия для DbLoadWindow.xaml
    /// </summary>
    public partial class DbLoadWindow : Window
    {
        private static TalamusContext _db;
        public List<Part> Parts { get; set; }
        public List<Subsequent> Subsequents { get; set; }
        public DbLoadWindow()
        {
            Subsequents = new List<Subsequent>();
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _db = new TalamusContext(new DbContextOptionsBuilder<TalamusContext>().UseSqlServer(tbPath.Text).Options);

                Parts = _db.Parts.Include(p => p.Book).Where(i => i.Book.Id == Convert.ToInt32(tbBookId.Text)).ToList();

                foreach(Part p in Parts)
                {
                    Subsequents.AddRange(_db.Subsequents.Include(p=>p.Part).Include(p=>p.NextPart).Where(s=>s.Part.Id==p.Id));
                }
                this.DialogResult = true;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException, "Хуйня какая-то");
            }
           
        }
    }
}
