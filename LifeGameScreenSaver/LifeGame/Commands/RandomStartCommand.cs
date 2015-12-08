using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LifeGameScreenSaver.LifeGame
{
    class RandomStartCommand : ICommand
    {
        private GameViewModel gameViewModel;

        public RandomStartCommand(GameViewModel viewModel)
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
            gameViewModel.IsActive = true;
            gameViewModel.gameModel.Randomize();
        }
    }
}
