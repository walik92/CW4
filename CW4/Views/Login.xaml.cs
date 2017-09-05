using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using CW4.BusinessLogic;
using CW4.DatabaseObjects;

namespace CW4.Views
{
    /// <summary>
    ///     Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly UserManager _userManager;
        private readonly NoteManager _noteManager;
        private readonly BackgroundWorker _worker;

        public Login()
        {
            InitializeComponent();
            _userManager = new UserManager();
            _noteManager = new NoteManager();
            _worker = new BackgroundWorker();
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_worker.IsBusy)
            {
                ProgressBar.Visibility = Visibility.Visible;
                Button.IsEnabled = true;
                _worker.RunWorkerAsync();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var login = "";
            var password = "";
            Dispatcher.Invoke(() =>
            {
                login = LoginBox.Text;
                password = PasswordBox.Password;
            });

            var user = _userManager.Get(login, password);
            e.Result = user;
        }

        private void worker_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                ProgressBar.Visibility = Visibility.Collapsed;
                Button.IsEnabled = true;
            }
            else
            {
                var user = e.Result as User;
                var view = new NoteView(user);
                view.Show();
                Close();
            }
        }
    }
}