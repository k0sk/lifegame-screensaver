using System;
using System.Windows.Threading;

namespace LifeGameScreenSaver.LifeGame
{
	public class GameModel
	{
        public delegate void OnUpdate(object sender);
        public event OnUpdate Update;
        private byte[] current;
        private byte[] next;

		public GameModel()
		{
            this.current = new byte[Constants.CELLS_X * Constants.CELLS_Y];
            this.next = new byte[Constants.CELLS_X * Constants.CELLS_Y];
		}

        public byte[] Cells
        {
            get { return this.current; }
        }

		public void Clear()
		{
            Array.Clear(this.current, 0, Constants.CELLS_X * Constants.CELLS_Y);

            if (this.Update != null) this.Update(this);
		}

		public void Randomize()
		{
			Random r = new Random((int)DateTime.Now.Ticks);

			for (int i = 0; i < this.current.Length; i++)
			{
				this.current[i] = Convert.ToByte(r.Next(0, 2));
            }

            if (this.Update != null) this.Update(this);
		}

		public void Next()
		{
			for (int i = 0; i < this.current.Length; i++)
			{
                bool is_alive = this.current[i] > 0;
                int count = this.countAliveNeighbours(i);

                bool result = (is_alive && 2 <= count && count <= 3) || (!is_alive && count == 3);
                this.next[i] = (result) ? (byte)1 : (byte)0;
			}

            this.next.CopyTo(this.current, 0);

            if (this.Update != null) this.Update(this);
		}

		public void ToggleCell(int x, int y)
		{
            int i = y * LifeGame.Constants.CELLS_X + x;
            this.current[i] = (this.current[i] == 0) ? (byte)1 : (byte)0;

            if (this.Update != null) this.Update(this);
		}

        private int countAliveNeighbours(int i)
		{
            int x = (i % Constants.CELLS_X);
            int y = (i / Constants.CELLS_X);

            int up = (y == 0) ? Constants.CELLS_X * (Constants.CELLS_Y - 1) : -Constants.CELLS_X;
            int down = (y == Constants.CELLS_Y - 1) ? -Constants.CELLS_X * (Constants.CELLS_Y - 1) : Constants.CELLS_X;
            int left = (x == 0) ? (Constants.CELLS_X - 1) : -1;
            int right = (x == Constants.CELLS_X - 1) ? (-Constants.CELLS_X + 1) : 1;

            int count = this.current[i + up] + this.current[i + right + up] + this.current[i + right] + this.current[i + right + down]
                        + this.current[i + down] + this.current[i + left + down] + this.current[i + left] + this.current[i + left + up];

            return count;
        }
	}
}
