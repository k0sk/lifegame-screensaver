using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace LifeGameScreenSaver
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                string mode = e.Args[0].ToLower();

                if (mode.StartsWith("/c"))
                {
                    ShowConfig();
                    return;
                }
                else if (mode.StartsWith("/p"))
                {
                    ShowPreview();
                    return;
                }
            }

            ShowScreenSaver();
        }

        private void ShowScreenSaver() {
            foreach (Screen s in Screen.AllScreens)
            {
                MainWindow window = new MainWindow();
                window.Left = s.WorkingArea.Left;
                window.Top = s.WorkingArea.Top;
                window.Width = s.WorkingArea.Width;
                window.Height = s.WorkingArea.Height;
                window.Show();
            }
        }

        private void ShowPreview() {
        }

        private void ShowConfig() {
        }
    }
}
