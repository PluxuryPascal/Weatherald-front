using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Wetherald
{
    /// <summary>
    /// Логика взаимодействия для TrustPage_Change.xaml
    /// </summary>
    public partial class TrustPage_Change : Page
    {
        int count;
        public int Count => count;
        public TrustPage_Change()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Подписываемся на событие KeyDown для окна
            KeyDown += MainWindow_KeyDown;
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, была ли нажата кнопка F1
            if (e.Key == Key.F1)
            {
                // Вызываем метод для открытия справки
                ShowHelp();
            }
        }

        private void ShowHelp()
        {
            try
            {

                // Путь к файлу справки CHM
                string pathToHelpFile = @"SpravSluzhba.chm";
                // Название страницы в справке CHM
                string helpPage = "okno_mainwindow_vybor_doveriya.htm";

                // Строка с командой для открытия файла справки
                string command = $"hh.exe {pathToHelpFile}::{helpPage}";

                // Создаем новый процесс для выполнения команды
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
                {
                    // Скрываем окно командной строки
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия справки: {ex.Message}");
            }
        }
        private void ReturnMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
        private void WebClick1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int checker = count;
            Label lbl = (Label)sender;
            DragDrop.DoDragDrop(lbl, lbl.Content, DragDropEffects.Copy);
            if (count > checker)
            {
                lbl.IsEnabled = false;
            }

        }
        private void WebClick2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int checker = count;
            Label lbl = (Label)sender;
            DragDrop.DoDragDrop(lbl, lbl.Content, DragDropEffects.Copy);
            if (count > checker)
            {
                lbl.IsEnabled = false;
            }
        }




        private void NextPage_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new SettingsPage());
        }

        private void WebClick3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int checker = count;
            Label lbl = (Label)sender;
            DragDrop.DoDragDrop(lbl, lbl.Content, DragDropEffects.Copy);
            if (count > checker)
            {
                lbl.IsEnabled = false;
            }
        }

        private void WebClick4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int checker = count;
            Label lbl = (Label)sender;
            DragDrop.DoDragDrop(lbl, lbl.Content, DragDropEffects.Copy);
            if (count > checker)
            {
                lbl.IsEnabled = false;
            }
        }


        private void trustTarget1_Drop(object sender, DragEventArgs e)
        {
            ((TextBlock)sender).Text = (string)e.Data.GetData(DataFormats.Text);
            ((TextBlock)sender).AllowDrop = false;
            this.count++;
            App.trust.Add((string)e.Data.GetData(DataFormats.Text), 0.5);
            if (count == 3)
            {
                NextPageTrustButton.IsEnabled = true;
            }
        }

        private void trustTarget2_Drop(object sender, DragEventArgs e)
        {
            ((TextBlock)sender).Text = (string)e.Data.GetData(DataFormats.Text);
            ((TextBlock)sender).AllowDrop = false;
            this.count++;
            App.trust.Add((string)e.Data.GetData(DataFormats.Text), 0.3);
            if (count == 3)
            {
                NextPageTrustButton.IsEnabled = true;
            }
        }

        private void trustTarget3_Drop(object sender, DragEventArgs e)
        {
            ((TextBlock)sender).Text = (string)e.Data.GetData(DataFormats.Text);
            ((TextBlock)sender).AllowDrop = false;
            this.count++;
            App.trust.Add((string)e.Data.GetData(DataFormats.Text), 0.2);
            if (count == 3)
            {
                NextPageTrustButton.IsEnabled = true;
            }
        }



        private void testSpeavka1_Click(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo { FileName = @"SpravSluzhba.chm", UseShellExecute = true });

        }
        private void LangIco_Button_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).PageName = this.Title;
            NavigationService.Navigate(new ChangeTheLangPage());
        }
    }
}
