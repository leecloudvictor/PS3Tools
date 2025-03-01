﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
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

namespace PS4_PS2_Classics_Gui__WPF_
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        bool LoadCompleted = false;
        bool SkipSettings = false;

        int CurrentIdx = 0;
        MouseButtonEventArgs ButtonLick;


        public class SettingValue
        {
            public string SettingName { get; set; }

            public string SValue { get; set; }

            public Control type { get; set; }

        }

        List<SettingValue> Values = new List<SettingValue>();

        private readonly BackgroundWorker bgWorkerSS = new BackgroundWorker();
        public static bool bgwClose;


        public void AddEsttingValues(string SettingName,string DefaultValue, Control TypeOfContorl)
        {
            SettingValue set = new SettingValue();
            set.SettingName = SettingName;
            switch (SettingName)
            {
                case "Overwrite Temp Folder":
                    if(Properties.Settings.Default.OverwriteTemp == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;
                case "Temp Path":
                    if(Properties.Settings.Default.TempPath != "")
                    {
                        DefaultValue = Properties.Settings.Default.TempPath;
                    }
                    else
                    {
                        DefaultValue = "";
                    }
                    break;
                case "Use LibOrbisPkg":
                    if(Properties.Settings.Default.UseLibOrbisPkg == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;
                case "Replace NP Title ID With PS2 Title ID":
                    if (Properties.Settings.Default.EnablePS2IDReplace == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;
                case "Enable Boot Logo":
                    if (Properties.Settings.Default.EnableBootScreen == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;

                case "Enable PS4 Ambient Music":
                    if (Properties.Settings.Default.EnableGuiMusic == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;
                case "Enable kozarovv Patches":
                    if(Properties.Settings.Default.EnableCustomConfigFetching == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;

                case "Use Specific Emu":
                    if (Properties.Settings.Default.UseSpesifcEmu == "Default")
                    {
                        DefaultValue = "Default";
                    }
                    else
                    {
                        DefaultValue = Properties.Settings.Default.UseSpesifcEmu;
                    }
                    break;
            }
            set.SValue = DefaultValue;
            set.type = TypeOfContorl;

            Values.Add(set);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            InitializeComponent();
            AddEsttingValues("Overwrite Temp Folder", "Off", new ListView());
            AddEsttingValues("Temp Path", "", new Control());
            AddEsttingValues("Replace NP Title ID With PS2 Title ID", "On", new ListView());
            //AddEsttingValues("Replace NP Title ID With PS2 Title ID", "On", new ListView());
            AddEsttingValues("Enable Boot Logo", "On", new ListView());
            AddEsttingValues("Enable PS4 Ambient Music", "On", new ListView());

            AddEsttingValues("Enable kozarovv Patches", "Off", new ListView());
            AddEsttingValues("Use Specific Emu", "Default", new ListView());
            AddEsttingValues("Use LibOrbisPkg", "Off", new ListView());

            for (int i = 0; i < Values.Count; i++)
            {
                listBox.Items.Add(new { Name = Values[i].SettingName, ValueForNameValue = Values[i].SValue });
            }
            //Margin="0,200,0,10"
            //listView.Items.Add(new { SettingValue = "On" });
            //ListViewItem lvi = listView.Items[0] as ListViewItem;
            //lvi.Margin.Top = 200;


            //listView.Items.Add(new { SettingValue = "Off" });

            bgWorkerSS.DoWork += bgWorkerSS_DoWork;
            bgWorkerSS.WorkerReportsProgress = true;
            bgWorkerSS.WorkerSupportsCancellation = true;
            bgWorkerSS.RunWorkerCompleted += BgWorkerSS_RunWorkerCompleted;

            LoadCompleted = true;
        }


        public void ClearAndAddList()
        {
            try
            {
                listBox.Items.Clear();
            }
            catch(Exception ex)
            {

            }
            for (int i = 0; i < Values.Count; i++)
            {
                listBox.Items.Add(new { Name = Values[i].SettingName, ValueForNameValue = Values[i].SValue });
            }

            listBox.SelectedIndex = -1;
            SkipSettings = true;
            //i dont know why but the dam view keeps comming up

            OptionsView.Visibility = Visibility.Hidden;
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.Navigation);
            }
        }


        private void OptionsView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                MainWindowView.Opacity = 1.0f;
                OptionsView.Visibility = Visibility.Hidden;
                listBox.Background = System.Windows.Media.Brushes.Transparent;
            }
            if(e.Key == Key.Enter)
            {
                //save the current item
            }

           
        }

        private void listBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void listBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         

        }

        private void listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ButtonLick = e;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                if (OptionsView.Visibility == Visibility.Visible)
                {
                    //hide this 
                    OptionsView.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Close();
                }
            }
            if(OptionsView.Visibility == Visibility.Visible)
            {
                if(e.Key == Key.Up)
                {

                }
            }
        }

        private void ListView_SettingItemDown(object sender, MouseButtonEventArgs e)
        {
            //if(e.LeftButton == MouseButtonState.Pressed)
            //{
            //    DoSettings();
            //}
        }
        Stopwatch sw = new Stopwatch();

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {


            // Calculate download speed and output it to labelSpeed.
            string speed = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));



            // Show the percentage on our label.
            string percantage = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            string download = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            string Builder = string.Format(@"Downloading Addon {1}
Downloaded : {0}
Speed : {2}
                              ", download, percantage, speed);

            UpdateString(Builder);

        }

        private void bgWorkerSS_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    // your code

                    Busy Busy = new Busy(bgWorkerSS);
                    Busy.ShowDialog();
                    Busy.Focus();
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void BgWorkerSS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           // Properties.Settings.Default.EnablePMF = true;
        }


        private void UpdateString(string txt)
        {
            //UpdateInfo(txt);
            OpenCloseWaitScreen(true);
            Busy.INFO = txt;
            //lblTask.Invoke(new Action(() => lblTask.Text = txt));
        }

        public static string AppCommonPath()
        {
            return MainWindow.AppCommonPath();
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        private void OpenCloseWaitScreen(bool open)
        {
            if (open)
            {
                //Info Screen Wait screen
                Busy.INFO = "Loading Data";
                bgwClose = false;
                if (!bgWorkerSS.IsBusy)
                {
                    bgWorkerSS.RunWorkerAsync();
                }
            }
            else
            {
                //Wait screen/ info Screen
                bgwClose = true;
                bgWorkerSS.CancelAsync();
                bgWorkerSS.Dispose();
            }
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            OpenCloseWaitScreen(false);
            sw.Reset();
            //if directory exists just delete it 
            if (Directory.Exists(AppCommonPath() + "Jax And Daxter"))
            {
                DeleteDirectory(AppCommonPath() + "Jax And Daxter");
            }

            //create libvlc directory 
            if (!Directory.Exists(AppCommonPath() + "Jax And Daxter"))
            {
                Directory.CreateDirectory(AppCommonPath() + "Jax And Daxter");
            }





            ZipFile.ExtractToDirectory(AppCommonPath() + "Jax And Daxter.zip", AppCommonPath() + @"\Jax And Daxter\");

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                // your code

                MessageBox ps4mes = new MessageBox("Content Downloaded and Added\n\nPlease Restart The Application", "Done", PS4_MessageBoxButton.OK, SoundClass.Sound.Notification);
                ps4mes.ShowDialog();
            });
        }

        private void startDownload(string FileName)
        {
            Thread thread = new Thread(() =>
            {
                sw.Start();
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

               
                    client.DownloadFileAsync(new Uri("http://www.nigmacontractors.co.za/PSTools/PS2Classics/Downloads/"+ FileName), AppCommonPath() + "Jax and Daxter.zip");
               });
            thread.Start();
        }

        public void DoSettings()
        {
            //check control type
            if (Values[listBox.SelectedIndex].type != new Control())
            {
                ListViewItem lvi = listView.SelectedValue as ListViewItem;

                Values[listBox.SelectedIndex].SValue = lvi.Content.ToString();
                if (Values[listBox.SelectedIndex].SValue == "On")
                {
                    switch (Values[listBox.SelectedIndex].SettingName)
                    {
                        case "Overwrite Temp Folder":
                            Properties.Settings.Default.OverwriteTemp = true;
                            break;
                        case "Replace NP Title ID With PS2 Title ID":
                            Properties.Settings.Default.EnablePS2IDReplace = true;
                            break;
                        case "Enable Boot Logo":
                            Properties.Settings.Default.EnableBootScreen = true;
                            break;
                        case "Enable PS4 Ambient Music":
                            Properties.Settings.Default.EnableGuiMusic = true;

                            SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);

                            break;
                       
                        case "Enable kozarovv Patches":
                            Properties.Settings.Default.EnableCustomConfigFetching = true;
                            break;
                        case "Use Specific Emu":
                            Properties.Settings.Default.UseSpesifcEmu = listView.SelectedItem.ToString();
                           
                            //SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);

                            break;
                        case "Use LibOrbisPkg":
                            Properties.Settings.Default.UseLibOrbisPkg = true; 
                            break;

                    }
                    Properties.Settings.Default.Save();//save the settings
                }
                else
                {
                    switch (Values[listBox.SelectedIndex].SettingName)
                    {
                        case "Overwrite Temp Folder":
                            Properties.Settings.Default.OverwriteTemp = false;
                            break;
                        case "Replace NP Title ID With PS2 Title ID":
                            Properties.Settings.Default.EnablePS2IDReplace = false;
                            break;
                        case "Enable Boot Logo":
                            Properties.Settings.Default.EnableBootScreen = false;
                            
                            break;
                        case "Enable PS4 Ambient Music":
                            Properties.Settings.Default.EnableGuiMusic = false;
                            if (SoundClass.PS4BGMDevice != null)
                            {
                                SoundClass.PS4BGMDevice.Stop();
                            }
                            break;

                        case "Enable kozarovv Patches":
                            Properties.Settings.Default.EnableCustomConfigFetching = false;
                            break;
                        case "Use LibOrbisPkg":
                            Properties.Settings.Default.UseLibOrbisPkg = false;
                            break;
                        case "Use Specific Emu":
                            Properties.Settings.Default.UseSpesifcEmu = lvi.Content.ToString();
                            if (lvi.Content.ToString() == "Jax and Daxter")
                            {
                                //check if the item is found
                                MessageBox ps4mes = new MessageBox("To Allow this feature an external download is needed \nWould you like to continue ?", "Download", PS4_MessageBoxButton.YesNo, SoundClass.Sound.Notification);
                                ps4mes.ShowDialog();
                                if (PS4_MessageBoxResult.Yes == MessageBox.ReturnResult)
                                {
                                    bgWorkerSS.RunWorkerAsync();
                                    startDownload("Jax and Daxter.zip");
                                }
                            }
                            //SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);

                            break;

                    }
                    Properties.Settings.Default.Save();//save the settings
                }
            }
            else
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        Properties.Settings.Default.TempPath = dialog.SelectedPath.ToString();
                    }
                    Values[listBox.SelectedIndex].SValue = Properties.Settings.Default.TempPath;
                }
                Properties.Settings.Default.Save();//save the settings
            }

            //close the options pannel
            MainWindowView.Opacity = 1.0f;
            OptionsView.Visibility = Visibility.Hidden;
            listBox.Background = System.Windows.Media.Brushes.Transparent;

            ClearAndAddList();
        }

        private void ListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    DoSettings();
            //}
        }

        private void ListView_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    DoSettings();
            //}
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listBox.SelectedIndex != -1)
                {
                    DoSettings();
                }
            }
            catch(Exception ex)
            {
                MainWindowView.Opacity = 1.0f;
                OptionsView.Visibility = Visibility.Hidden;
                listBox.Background = System.Windows.Media.Brushes.Transparent;
            }
        }

        public ListViewItem CreateItem(string Value)
        {
            //FontSize="20" FontFamily="Yu Gothic" Foreground="#FFFFFFFF"
            ListViewItem itemadd = new ListViewItem();
            var mar = itemadd.Margin;
            if (listView.Items.Count == 0)
            {
                mar.Top = 200;
                mar.Bottom = 10;
                mar.Left = 0;
                mar.Right = 0;
            }
            else
            {
                mar.Top = 10;
                mar.Bottom = 10;
                mar.Left = 0;
                mar.Right = 0;
            }
            itemadd.FontSize = 20;
            itemadd.FontFamily = listView.FontFamily;
            itemadd.HorizontalAlignment = HorizontalAlignment.Center;
            itemadd.VerticalAlignment = VerticalAlignment.Center;
            itemadd.Foreground = new SolidColorBrush(Colors.White);
            itemadd.Content = (Value);


            return itemadd;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentIdx = listBox.SelectedIndex;

            var item = Values[CurrentIdx];

            if (ButtonLick.LeftButton == MouseButtonState.Pressed)
            {
                if (listBox.SelectedIndex != 1)
                {

                    if(item.SettingName == "Use Specific Emu")
                    {
                        listView.Items.Clear();
                        listView.Foreground = new SolidColorBrush(Colors.White);
                       
                        


                        //listView.ItemTemplate = (DataTemplate)Resources["SettingTemplate"];
                        //itemadd.Template = (ControlTemplate)Resources["SettingTemplate"];
                        listView.Items.Add(CreateItem("Default"));
                        listView.Items.Add(CreateItem("Jax and Daxter"));
                        //listView.Items.Add("Jax and Daxter");
                        //HorizontalAlignment="Center" VerticalAlignment="Center" Margin="0,200,0,10"
                        //Margin="0,200,0,10"
                        // listView.Items.Add("Default");
                        ListViewItem lvi = listView.Items[0] as ListViewItem;
                        if (lvi != null)
                        {
                            lvi.HorizontalAlignment = HorizontalAlignment.Center;
                            lvi.VerticalAlignment = VerticalAlignment.Center;
                        }
                        //var margin = lvi.Margin;
                        //margin.Top = 200;


                        //listView.Items.Add(new { SettingValue = "Off" });
                    }
                    MainWindowView.Opacity = 0.7f;
                    OptionsView.Visibility = Visibility.Hidden;
                    OptionsView.Visibility = Visibility.Visible;
                    listBox.Background = System.Windows.Media.Brushes.Transparent;
                }
                else
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            Properties.Settings.Default.TempPath = dialog.SelectedPath.ToString();
                        }
                        Values[listBox.SelectedIndex].SValue = Properties.Settings.Default.TempPath;
                    }
                    Properties.Settings.Default.Save();//save the settings
                    ClearAndAddList();
                }
            }
        }
    }
}
