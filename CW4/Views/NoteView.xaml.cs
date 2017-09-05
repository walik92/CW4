using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CW4.BusinessLogic;
using CW4.DatabaseObjects;

namespace CW4.Views
{
    /// <summary>
    ///     Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView : Window
    {
        private readonly NoteManager _noteManager;
        private readonly User _user;
        private readonly BackgroundWorker _workerAddNote;
        private readonly BackgroundWorker _workerDeleteNote;
        private readonly BackgroundWorker _workerEditNote;
        private readonly BackgroundWorker _workerInit;

        public NoteView(User user)
        {
            InitializeComponent();
            _user = user;
            _noteManager = new NoteManager();

            _workerInit = new BackgroundWorker();
            _workerInit.DoWork += workerInit_DoWork;
            _workerInit.RunWorkerCompleted += workerInit_RunWorkerCompleted;

            _workerAddNote = new BackgroundWorker();
            _workerAddNote.DoWork += workerAddNote_DoWork;
            _workerAddNote.RunWorkerCompleted += workerAddNote_RunWorkerCompleted;

            _workerEditNote = new BackgroundWorker();
            _workerEditNote.DoWork += workerEditNote_DoWork;
            _workerEditNote.RunWorkerCompleted += workerEditNote_RunWorkerCompleted;

            _workerDeleteNote = new BackgroundWorker();
            _workerDeleteNote.DoWork += workerDeleteNote_DoWork;
            _workerDeleteNote.RunWorkerCompleted += workerDeleteNote_RunWorkerCompleted;

            _workerInit.RunWorkerAsync();
        }

        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (!_workerAddNote.IsBusy)
            {
                ShowProgess();
                _workerAddNote.RunWorkerAsync();
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!_workerEditNote.IsBusy)
            {
                ShowProgess();
                var btn = sender as Button;
                var note = btn.DataContext as Note;
                _workerEditNote.RunWorkerAsync(note);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!_workerDeleteNote.IsBusy)
            {
                ShowProgess();
                var btn = sender as Button;
                var note = btn.DataContext as Note;
                _workerDeleteNote.RunWorkerAsync(note);
            }
        }

        private void workerInit_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = _noteManager.GetAllNotesOfUser(_user.Id);
        }

        private void workerInit_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                var notes = e.Result as List<Note>;
                if (notes != null)
                    foreach (var note in notes)
                    {
                        ListView.Items.Add(note);
                    }
            }

            HideProgess();
        }

        private void workerAddNote_DoWork(object sender, DoWorkEventArgs e)
        {
            var textNote = "";
            Dispatcher.Invoke(() => { textNote = TextBox.Text; });

            var note = new Note
            {
                User = _user,
                Value = textNote
            };
            _noteManager.Add(note);
            e.Result = note;
        }

        private void workerAddNote_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                var note = e.Result as Note;
                ListView.Items.Add(note);
                TextBox.Text = "";
            }

            HideProgess();
        }

        private void workerEditNote_DoWork(object sender, DoWorkEventArgs e)
        {
            var note = e.Argument as Note;
            _noteManager.Edit(note);
        }

        private void workerEditNote_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            HideProgess();
        }

        private void workerDeleteNote_DoWork(object sender, DoWorkEventArgs e)
        {
            var note = e.Argument as Note;
            _noteManager.Delete(note);
            e.Result = note;
        }

        private void workerDeleteNote_RunWorkerCompleted(object sender,
            RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                var note = e.Result as Note;
                ListView.Items.Remove(note);
            }

            HideProgess();
        }

        private void ShowProgess()
        {
            ProgressBar.Visibility = Visibility.Visible;
            ButtonAddNew.IsEnabled = false;
            ListView.IsEnabled = false;
        }

        private void HideProgess()
        {
            ProgressBar.Visibility = Visibility.Hidden;
            ButtonAddNew.IsEnabled = true;
            ListView.IsEnabled = true;
        }
    }
}