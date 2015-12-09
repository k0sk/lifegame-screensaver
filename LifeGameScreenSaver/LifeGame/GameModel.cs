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
            this.CountLives = 0;
            this.SizeX = Defaults.RES_X / Defaults.CELL_SIZE;
            this.SizeY = Defaults.RES_Y / Defaults.CELL_SIZE;

            this.current = new byte[this.SizeX * this.SizeY];
            this.next = new byte[this.SizeX * this.SizeY];
		}

        public byte[] Cells
        {
            get { return this.current; }
        }

        public uint CountLives { get; set; }
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

		public void Clear()
		{
            Array.Clear(this.current, 0, this.SizeX * this.SizeY);

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
            this.CountLives = lives;

            if (this.Update != null) this.Update(this);
		}

		public void ToggleCell(int x, int y)
		{
            int i = y * this.SizeX + x;
            this.current[i] = (this.current[i] == 0) ? (byte)1 : (byte)0;

            if (this.Update != null) this.Update(this);
		}

        public void ChangeBoardSize(int SizeX, int SizeY)
        {
            this.SizeX = SizeX;
            this.SizeY = SizeY;

            this.current = new byte[this.SizeX * this.SizeY];
            this.next = new byte[this.SizeX * this.SizeY];
        }

        private int countAliveNeighbours(int i)
		{
            int x = (i % this.SizeX);
            int y = (i / this.SizeX);

            int up = (y == 0) ? this.SizeX * (this.SizeY - 1) : -this.SizeX;
            int down = (y == this.SizeY - 1) ? -this.SizeX * (this.SizeY - 1) : this.SizeX;
            int left = (x == 0) ? (this.SizeX - 1) : -1;
            int right = (x == this.SizeX - 1) ? (-this.SizeX + 1) : 1;

            int count = this.current[i + up] + this.current[i + right + up] + this.current[i + right] + this.current[i + right + down]
                        + this.current[i + down] + this.current[i + left + down] + this.current[i + left] + this.current[i + left + up];

            return count;
        }
	}
}
