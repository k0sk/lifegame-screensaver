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
        private LifeGame.GameModel model = new LifeGame.GameModel();
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * 100);
            this.timer.Tick += new EventHandler(this.times_Up);

            this.model.Update += new LifeGame.GameModel.OnUpdate(model_Update);
            this.model.Randomize();
            this.model.Start();

            this.timer.Start();
        }

        private void times_Up(object sender, EventArgs e)
        {
            this.model.Next();
        }

        private void model_Update(object sender)
        {
            this.LifeGameView.Update(this.model.Cells);
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
            this.model.Randomize();
            this.model.Start();
        }

        private void LifeGameView_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this.LifeGameView);
            int x = (int)((pt.X) / LifeGame.Constants.CELL_SIZE);
            int y = (int)((pt.Y) / LifeGame.Constants.CELL_SIZE);

            this.model.ToggleCell(x, y);
        }
    }
}
