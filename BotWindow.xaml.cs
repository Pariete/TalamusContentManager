using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Talamus_ContentManager.Models;
using Telegram.Bot;

namespace Talamus_ContentManager
{
    /// <summary>
    /// Логика взаимодействия для BotWindow.xaml
    /// </summary>
    public partial class BotWindow : Window
    {
        public BotWindow()
        {
            InitializeComponent();
            LoadSettings();
        }
        private Settings _settings;
        private int _total;
        private int _success;
        private int _failure;
        private int _delay;

        private bool _running;
        private string _token;

        private List<User> _users;

        private static TalamusContext _db;

        private TelegramBotClient _client;
        private Task _spam;

        private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        public int Success
        {
            get { return _success; }
            set
            {
                _success = value;
                lblSuccess.Content = "Отправлено:" + _success.ToString();
            }
        }
        public int Failure
        {
            get { return _failure; }
            set
            {
                _failure = value;
                lblError.Content = "Ошибка:" + _failure.ToString();
            }
        }
       

        public bool Running
        {
            get
            {
                return _running;
            }
            set
            {
                _running = value;
                if (value)
                {
                    lblError.Visibility = Visibility.Visible;
                    lblSuccess.Visibility = Visibility.Visible;
                    lblTotal.Visibility = Visibility.Visible;
                    lblTotal.Content = "Всего:" + _users.Count.ToString();
                    tbDBPath.IsEnabled = false;
                    tbDelay.IsEnabled = false;
                    tbMessage.IsEnabled = false;
                    tbToken.IsEnabled = false;
                    btnStart.IsEnabled = false;
                    btnStop.IsEnabled = true;
                }
                else
                {
                    lblError.Visibility = Visibility.Hidden;
                    lblSuccess.Visibility = Visibility.Hidden;
                    lblTotal.Visibility = Visibility.Hidden;
                    tbDBPath.IsEnabled = true;
                    tbDelay.IsEnabled = true;
                    tbMessage.IsEnabled = true;
                    tbToken.IsEnabled = true;
                    btnStart.IsEnabled = true;
                    btnStop.IsEnabled = false;
                }
            }
        }

        private async Task LoadSettings()
        {
            _settings = new Settings()
            {
                BotToken = "",
                DBPath = ""
            };

            try
            {
                using (FileStream fs = new FileStream("settings", FileMode.Open))
                {
                    _settings = await JsonSerializer.DeserializeAsync<Settings>(fs);
                }
            }
            catch
            {

            }
            tbDBPath.Text = _settings.DBPath;
            tbToken.Text = _settings.BotToken;
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancelTokenSource.Token;

            _delay = Convert.ToInt32(tbDelay.Text);
            _token = tbToken.Text;
            _client = new TelegramBotClient(_token);
            _db = new TalamusContext(new DbContextOptionsBuilder<TalamusContext>().UseSqlServer(tbDBPath.Text).Options);
            _users = _db.Users.ToList();
            _success = 0;
            _failure = 0;
            Running = true;

            string Message = tbMessage.Text;

            _spam = new Task(() =>
            {
                foreach(var user in _users)
                {
                    Thread.Sleep(1000 * _delay);
                    if (!token.IsCancellationRequested)
                    {
                        try
                        {
                            _client.SendTextMessageAsync(user.UserId, Message);
                            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => {
                                Success++;
                            }));
                        }
                        catch
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => {
                                Failure++;
                            }));
                        }
                    }
                    else break;
                }
                MessageBox.Show(String.Format("Пользователей:{0}\nОтправлено:{1}\nОшибка:{2}\n",_total,_success,_failure), "Done");
            },token);

            _spam.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Running = false;
            _cancelTokenSource.Cancel();
        }

    }
}
