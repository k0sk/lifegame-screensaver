using System;

namespace LifeGameScreenSaver
{
	public class ClickEventArgs : EventArgs
	{
		public ClickEventArgs(int x, int y)
			: base()
		{
			this.X = x;
			this.Y = y;
		}

		public int X
		{
			get;
			private set;
		}

		public int Y
		{
			get;
			private set;
		}
	}
}
