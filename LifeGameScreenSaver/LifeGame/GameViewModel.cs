using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LifeGameScreenSaver.LifeGame
{
    public class GameViewModel : FrameworkElement
    {
        public GameModel gameModel = new GameModel();
		private const double OUTLINE_WIDTH = Defaults.OUTLINE_WIDTH;
        private Pen outline = new Pen(Brushes.DimGray, OUTLINE_WIDTH);
		private DrawingVisual[] visuals;
		private DrawingVisual grid = new DrawingVisual();
		private DrawingVisual cells = new DrawingVisual();
		private byte[] states;
        private long countLives;

        public GameViewModel() : base()
        {
            this.gameModel.Update += (sender) => this.Update(this.gameModel.Cells, this.countLives);

            this.CellSize = Defaults.CELL_SIZE;
            this.SizeX = Defaults.RES_X / Defaults.CELL_SIZE;
            this.SizeY = Defaults.RES_Y / Defaults.CELL_SIZE;

			this.AddVisualChild(this.grid);
			this.AddLogicalChild(this.grid);

			this.AddVisualChild(this.cells);
			this.AddLogicalChild(this.cells);

			this.visuals = new DrawingVisual[] { this.grid, this.cells };

			this.drawGrid();
			this.drawCells();
        }

        public void Update(byte[] states, long count)
        {
			this.states = states;
            this.countLives = count;
			this.drawGrid();
			this.drawCells();
        }

		public void Clear()
		{
			this.drawGrid();
		}

        public void ChangeRes(int width, int height)
        {

            for (int s = Defaults.CELL_SIZE; s <= width / 2; s += 8)
            {
                if ((width % s == 0) && (height % s == 0))
                {
                    this.CellSize = s;
                    break;
                }
            }

            this.SizeX = width / this.CellSize;
            this.SizeY = height / this.CellSize;

            this.gameModel.ChangeBoardSize(this.SizeX, this.SizeY);
        }

        public Boolean IsActive { get; set; }

        public int CellSize { get; private set; }
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

		protected override int VisualChildrenCount
		{
			get { return this.visuals.Length; }
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index > this.visuals.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}

			return this.visuals[index];
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			return new Size(this.SizeX * this.CellSize, this.SizeY * this.CellSize);
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			this.drawGrid();
			this.drawCells();
		}

		private void drawGrid()
		{
			using (DrawingContext dc = this.grid.RenderOpen())
			{
				Rect background = new Rect(0, 0, this.CellSize * this.SizeX, this.CellSize * this.SizeY);
				dc.DrawRectangle(Brushes.Black, null, background);

				Point start = new Point(0, 0);
				Point end = new Point(0, background.Bottom);
				for (int i = 0; i < this.SizeX; i++)
				{
					dc.DrawLine(outline, start, end);
					start.Offset(this.CellSize, 0);
					end.Offset(this.CellSize, 0);
				}

				start = new Point(0, 0);
				end = new Point(background.Right, 0);
				for (int i = 0; i < this.SizeX; i++)
				{
					dc.DrawLine(outline, start, end);
					start.Offset(0, this.CellSize);
					end.Offset(0, this.CellSize);
				}
			}
		}

		private void drawCells()
		{
			if (null == this.states) return;

			using (DrawingContext dc = this.cells.RenderOpen())
			{
				int x = 0;
				int y = 0;
				Rect rect = new Rect(OUTLINE_WIDTH, OUTLINE_WIDTH, this.CellSize - OUTLINE_WIDTH, this.CellSize - OUTLINE_WIDTH);
				
                for (int i = 0, l = states.Length; i < l; i++)
				{
					if (states[i] == 1)
				{
                        x = (i % this.SizeX);
                        y = (i / this.SizeX);
					
                        rect.Location = new Point((x * this.CellSize) + OUTLINE_WIDTH, (y * this.CellSize) + OUTLINE_WIDTH);
						dc.DrawRectangle(Brushes.Lime, null, rect);
					}
				}
			}
		}

        private ICommand adjustBoardSize;
        public ICommand AdjustBoardSize
        {
            get { return adjustBoardSize ?? (adjustBoardSize = new AdjustBoardSizeCommand(this)); }
        }

        private ICommand next;
                public ICommand Next
                {
                    get { return next ?? (next = new NextCommand(this)); }
                }

        private ICommand randomStart;
        public ICommand RandomStart
        {
            get { return randomStart ?? (randomStart = new RandomStartCommand(this)); }
        }

        private ICommand toggleCell;
        public ICommand ToggleCell
        {
            get { return toggleCell ?? (toggleCell = new ToggleCellCommand(this)); }
        }
    }
}
