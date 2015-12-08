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
		private const double OUTLINE_WIDTH = 1;
		private DrawingVisual[] visuals;
		private DrawingVisual grid = new DrawingVisual();
		private DrawingVisual cells = new DrawingVisual();
		private byte[] values;
		private Pen outline = new Pen(Brushes.DimGray, OUTLINE_WIDTH);

        public GameViewModel() : base()
        {
			this.AddVisualChild(this.grid);
			this.AddLogicalChild(this.grid);

			this.AddVisualChild(this.cells);
			this.AddLogicalChild(this.cells);

			this.visuals = new DrawingVisual[] { this.grid, this.cells };

			this.drawGrid();
			this.drawCells();
        }

        public void Update(byte[] values)
        {
			this.values = values;
			this.drawGrid();
			this.drawCells();
        }

		public void Clear()
		{
			this.drawGrid();
		}

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
			return new Size(Constants.CELLS_X * Constants.CELL_SIZE, Constants.CELLS_Y * Constants.CELL_SIZE);
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
				Rect background = new Rect(0, 0, Constants.CELL_SIZE * Constants.CELLS_X, Constants.CELL_SIZE * Constants.CELLS_Y);
				dc.DrawRectangle(Brushes.Black, null, background);

				Point start = new Point(0, 0);
				Point end = new Point(0, background.Bottom);
				for (int i = 0; i < Constants.CELLS_X; i++)
				{
					dc.DrawLine(outline, start, end);
					start.Offset(Constants.CELL_SIZE, 0);
					end.Offset(Constants.CELL_SIZE, 0);
				}

				start = new Point(0, 0);
				end = new Point(background.Right, 0);
				for (int i = 0; i < Constants.CELLS_X; i++)
				{
					dc.DrawLine(outline, start, end);
					start.Offset(0, Constants.CELL_SIZE);
					end.Offset(0, Constants.CELL_SIZE);
				}
			}
		}

		private void drawCells()
		{
			if (null == this.values)
			{
				return;
			}

			using (DrawingContext dc = this.cells.RenderOpen())
			{
				int x = 0;
				int y = 0;
				Rect rect = new Rect(OUTLINE_WIDTH, OUTLINE_WIDTH, Constants.CELL_SIZE - OUTLINE_WIDTH, Constants.CELL_SIZE - OUTLINE_WIDTH);
				
				for (int i = 0; i < values.Length; i++)
				{
					x = (i % Constants.CELLS_X);
					y = (i / Constants.CELLS_X);
					rect.Location = new Point((x * Constants.CELL_SIZE) + OUTLINE_WIDTH, (y * Constants.CELL_SIZE) + OUTLINE_WIDTH);
					
					if (1 == values[i])
					{
						dc.DrawRectangle(Brushes.Lime, null, rect);
					}
				}
			}
		}
    }
}
