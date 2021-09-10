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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;
using System.IO;

namespace WpfRacingSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog_;
        public MainWindow()
        {
            InitializeComponent();
            openFileDialog_ = new OpenFileDialog();
            Title = "The racing simulator";
            Display += () => Win();
        }

        public delegate void Message();
        public event Message Display;

        public const int Distance = 300;
        public const int Scale = Distance / 100;

        public String MyDisplayValue
        {
            get
            {
                return this.lbl_TaskStatus.Text;
            }
            set
            {
                this.lbl_TaskStatus.Text = value;
            }
        }
        public void Win()
        {
            this.Dispatcher.Invoke(() =>
            {
                int i = Distance;
                pb_LengthyTaskProgress.Value = i;
                lbl_CountDownTimer.Text = i.ToString();
                lbl_TaskStatus.Text = "Done";
                //gd_Main.RowDefinitions.Add(new RowDefinition());
                //System.Windows.Controls.TextBlock newTxtbl = new TextBlock();
                //newTxtbl.Text = "New Text Block";
                //gd_Main.Children.Add(newTxtbl);
                //Grid.SetRow(newTxtbl, gd_Main.RowDefinitions.Count - 1);
                //Grid.SetColumn(newTxtbl, 2);
                btn_StartLengthyTask.IsEnabled = true;
                btn_OpenConfigFile.IsEnabled = true;
            });
        }
        private void btn_StartLengthyTask_Click(object sender, RoutedEventArgs e)
        {
            btn_StartLengthyTask.IsEnabled = false;
            btn_OpenConfigFile.IsEnabled = false;
            pb_LengthyTaskProgress.Value = 0;

            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                {
                    lbl_TaskStatus.Text = "Starting long Task...";
                    lbl_CountDownTimer.Text = "0";
                });
                Thread.Sleep(1000);
                this.Dispatcher.Invoke(() =>
                {
                    lbl_TaskStatus.Text = "In Progress...";
                });
                for (int i = 0; i < Distance; i++)
                {
                    Thread.Sleep(50);
                    this.Dispatcher.Invoke(() =>
                    {
                        pb_LengthyTaskProgress.Value = i / Scale;
                        lbl_CountDownTimer.Text = i.ToString();
                    });
                }
                Display();
            });
        }
        private void btn_OpenConfigFile_Click(object sender, RoutedEventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;
            string appPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(".."));
            if (!String.IsNullOrEmpty(appPath))
                openFileDialog_.InitialDirectory = appPath;
            openFileDialog_.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog_.FilterIndex = 1;
            openFileDialog_.RestoreDirectory = true;
            Nullable<bool> result = openFileDialog_.ShowDialog();
            if (result == true)
            {
                //Get the path of specified file
                filePath = openFileDialog_.FileName;
                //Read the contents of the file into a stream
                var fileStream = openFileDialog_.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
            }
        }
        
    }
}
