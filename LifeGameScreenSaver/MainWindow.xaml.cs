using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LifeGameScreenSaver
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private uint countLives;
        private int countPause;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * 100);
            this.timer.Tick += new EventHandler(this.times_Up);

            this.countLives = 0;
            this.countPause = 0;

            Loaded += (s, e) => {
                this.gameViewModel.AdjustBoardSize.Execute(new Tuple<int, int>((int)this.Width, (int)this.Height));
                this.gameViewModel.RandomStart.Execute(null);
                this.timer.Start();
            };
        }

        private void times_Up(object sender, EventArgs e)
        {
            this.gameViewModel.Next.Execute(null);

            if (this.countLives == this.gameViewModel.CountLives)
            {
                countPause++;
            }
            else
            {
                this.countLives = this.gameViewModel.CountLives;
            }

            if (countPause > 1000)
            {
                this.countPause = 0;
                this.gameViewModel.RandomStart.Execute(null);
            }
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

        Point? lastMousePos;
        private void LifeGameView_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePos = e.GetPosition(this.gameViewModel);

            if (lastMousePos.HasValue)
            {
                if (Math.Abs(lastMousePos.Value.X - currentMousePos.X) > this.gameViewModel.CellSize * 2 ||
                    Math.Abs(lastMousePos.Value.Y - currentMousePos.Y) > this.gameViewModel.CellSize * 2)
                {
                    int x = (int)((currentMousePos.X) / this.gameViewModel.CellSize);
                    int y = (int)((currentMousePos.Y) / this.gameViewModel.CellSize);

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
