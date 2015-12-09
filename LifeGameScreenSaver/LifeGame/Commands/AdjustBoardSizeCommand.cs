using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LifeGameScreenSaver.LifeGame
{
    class AdjustBoardSizeCommand : ICommand
    {
        private GameViewModel gameViewModel;

        public AdjustBoardSizeCommand(GameViewModel viewModel)
        {
            gameViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return !gameViewModel.IsActive;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Tuple<int, int> res = (Tuple<int, int>)parameter;
            gameViewModel.ChangeRes(res.Item1, res.Item2);
        }
    }
}
