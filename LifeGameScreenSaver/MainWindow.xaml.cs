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

namespace LifeGameScreenSaver
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private LifeGame.GameModel model = new LifeGame.GameModel();

        public MainWindow()
        {
            InitializeComponent();
            this.model.Update += new LifeGame.GameModel.OnUpdate(model_Update);
            this.model.Randomize();
            this.model.Start();
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
