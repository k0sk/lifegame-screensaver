using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LifeGameScreenSaver.LifeGame
{
    class NextCommand : ICommand
    {
        private GameViewModel gameViewModel;

        public NextCommand(GameViewModel viewModel)
        {
            gameViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            gameViewModel.gameModel.Next();
        }
    }
}
