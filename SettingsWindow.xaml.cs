using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Load();           
        }

        private async Task Load()
        {
            Settings s = new Settings()
            {
                BotToken = "",
                DBPath = ""
            };

            try
            {
                using (FileStream fs = new FileStream("settings", FileMode.Open))
                {
                    s = await JsonSerializer.DeserializeAsync<Settings>(fs);
                }
            }
            catch
            {

            }
            tbToken.Text = s.BotToken;
            tbDbPath.Text = s.DBPath;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings st = new Settings();
            st.DBPath = tbDbPath.Text;
            st.BotToken = tbToken.Text;

            using (FileStream fs = new FileStream("settings", FileMode.Create))
            {
                JsonSerializer.Serialize<Settings>(fs, st);
            }
        }
    }
}
