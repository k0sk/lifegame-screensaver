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
using System.Windows.Threading;

namespace LifeGameScreenSaver
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        Point? lastMousePos;

        public MainWindow()
        {
            InitializeComponent();

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * 100);
            this.timer.Tick += new EventHandler(this.times_Up);

            Loaded += (s, e) => {
                this.gameViewModel.AdjustBoardSize.Execute(new Tuple<int, int>((int)this.Width, (int)this.Height));
                this.gameViewModel.RandomStart.Execute(null);
                this.timer.Start();
            };
        }

        private void times_Up(object sender, EventArgs e)
        {
            this.gameViewModel.Next.Execute(null);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.gameViewModel.RandomStart.Execute(null);
        }
        private void LifeGameView_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePos = e.GetPosition(this.gameViewModel);

            if (lastMousePos.HasValue)
            {
                if (Math.Abs(lastMousePos.Value.X - currentMousePos.X) > this.gameViewModel.CellSize * 2 ||
                    Math.Abs(lastMousePos.Value.Y - currentMousePos.Y) > this.gameViewModel.CellSize * 2)
                {
                    int x = (int)((currentMousePos.X) / LifeGame.Defaults.CELL_SIZE);
                    int y = (int)((currentMousePos.Y) / LifeGame.Defaults.CELL_SIZE);

                    this.gameViewModel.ToggleCell.Execute(new Tuple<int, int>(x, y));
                }
            }
            else
            {
                this.lastMousePos = currentMousePos;
            }            
        }
    }
}
