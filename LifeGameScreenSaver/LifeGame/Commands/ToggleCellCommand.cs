using System;
using System.Windows.Input;

namespace LifeGameScreenSaver.LifeGame
{
    class ToggleCellCommand : ICommand
    {
        private GameViewModel gameViewModel;

        public ToggleCellCommand(GameViewModel viewModel)
        {
            gameViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return (0 <= (int)parameter && (int)parameter < this.gameViewModel.SizeX * this.gameViewModel.SizeY);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Tuple<int, int> pos = (Tuple<int, int>)parameter;
            gameViewModel.gameModel.ToggleCell(pos.Item1, pos.Item2);
        }
    }
}
