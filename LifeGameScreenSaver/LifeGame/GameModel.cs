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
        private uint countLives = 0;
        private int sizeX = Defaults.RES_X / Defaults.CELL_SIZE;
        private int sizeY = Defaults.RES_Y / Defaults.CELL_SIZE;

		public GameModel()
		{
            this.current = new byte[this.sizeX * this.sizeY];
            this.next = new byte[this.sizeX * this.sizeY];
		}

        public byte[] Cells
        {
            get { return this.current; }
        }

		public void Clear()
		{
            Array.Clear(this.current, 0, this.sizeX * this.sizeY);

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
            uint lives = 0;

			for (int i = 0; i < this.current.Length; i++)
			{
                bool is_alive = this.current[i] > 0;
                int count = this.countAliveNeighbours(i);
                bool is_survive = (is_alive && 2 <= count && count <= 3) || (!is_alive && count == 3);

                if (is_survive)
                {
                    this.next[i] = (byte)1;
                    lives++;
                }
                else
                {
                    this.next[i] = (byte)0;
                }
			}

            this.next.CopyTo(this.current, 0);
            this.countLives = lives;

            if (this.Update != null) this.Update(this);
		}

		public void ToggleCell(int x, int y)
		{
            int i = y * this.sizeX + x;
            this.current[i] = (this.current[i] == 0) ? (byte)1 : (byte)0;

            if (this.Update != null) this.Update(this);
		}

        public void ChangeBoardSize(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.current = new byte[this.sizeX * this.sizeY];
            this.next = new byte[this.sizeX * this.sizeY];
        }

        private int countAliveNeighbours(int i)
		{
            int x = (i % this.sizeX);
            int y = (i / this.sizeX);

            int up = (y == 0) ? this.sizeX * (this.sizeY - 1) : -this.sizeX;
            int down = (y == this.sizeY - 1) ? -this.sizeX * (this.sizeY - 1) : this.sizeX;
            int left = (x == 0) ? (this.sizeX - 1) : -1;
            int right = (x == this.sizeX - 1) ? (-this.sizeX + 1) : 1;

            int count = this.current[i + up] + this.current[i + right + up] + this.current[i + right] + this.current[i + right + down]
                        + this.current[i + down] + this.current[i + left + down] + this.current[i + left] + this.current[i + left + up];

            return count;
        }
	}
}
