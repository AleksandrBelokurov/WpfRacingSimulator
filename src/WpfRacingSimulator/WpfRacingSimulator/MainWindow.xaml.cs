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
        private Race race_;
        private OpenFileDialog openFileDialog_;
        public MainWindow()
        {
            InitializeComponent();
            race_ = new Race(this);
            openFileDialog_ = new OpenFileDialog();
            Title = "The racing simulator";
            btn_Start.IsEnabled = false;

        }

        public string InfoString
        {
            get
            {
                return this.lbl_InfoString.Text;
            }
            set
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.lbl_InfoString.Text = value;
                });
            }
        }
        public string InfoBoard
        {
            get
            {
                return this.lbl_InfoBoard.Text;
            }
            set
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.lbl_InfoBoard.Text = value;
                });
            }
        }
        public double ProgressMaximum
        {
            get
            {
                return this.pb_TotalProgress.Maximum;
            }
            set
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.pb_TotalProgress.Maximum = value;
                });
            }
        }
        public double ProgressValue
        {
            get
            {
                return this.pb_TotalProgress.Value;
            }
            set
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.pb_TotalProgress.Value = value;
                });
            }
        }
        public string LeftToFinish
        {
            get
            {
                return this.lbl_LeftToFinish.Text;
            }
            set
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.lbl_LeftToFinish.Text = value;
                });
            }
        }
        public void Finish()
        {
            this.Dispatcher.Invoke(() =>
            {
                lbl_Status.Text = "Done";
                btn_Start.IsEnabled = true;
                btn_OpenConfig.IsEnabled = true;
            });
        }
        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            btn_Start.IsEnabled = false;
            btn_OpenConfig.IsEnabled = false;
            lbl_Status.Text = "Distance left to finish: ";
            lbl_InfoString.Text = "The Race.";
            race_.Start();
        }
        private void btn_OpenConfigFile_Click(object sender, RoutedEventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;
            string ?appPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(".."));
            if (!String.IsNullOrEmpty(appPath))
                openFileDialog_.InitialDirectory = appPath;
            openFileDialog_.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog_.FilterIndex = 1;
            openFileDialog_.RestoreDirectory = true;
            Nullable<bool> result = openFileDialog_.ShowDialog();
            if (result == true)
            {
                filePath = openFileDialog_.FileName;
                lbl_Config.Text = filePath.ToString();
                var fileStream = openFileDialog_.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                    try 
                    {
                        race_.ParseConfig(fileContent.ToString());
                        btn_Start.IsEnabled = true;
                    }
                    catch 
                    {
                        lbl_Config.Text = "Config file " + openFileDialog_.SafeFileName + " is not valid";
                    }
                }
            }
        }
        
    }
}
