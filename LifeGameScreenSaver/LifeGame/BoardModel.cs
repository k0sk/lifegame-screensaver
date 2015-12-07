using System;
using System.Windows.Threading;

namespace LifeGameScreenSaver
{
	public class BoardModel
	{
        public delegate void OnUpdate(object sender);
        public event OnUpdate Update;
        private DispatcherTimer pulse;
        private byte[] current;
        private byte[] next;

        public byte[] Cells
        {
            get { return this.current; }
        }

		public BoardModel()
		{
            this.current = new byte[Constants.CELLS_X * Constants.CELLS_Y];
            this.next = new byte[Constants.CELLS_X * Constants.CELLS_Y];
            this.pulse = new DispatcherTimer();
            this.pulse.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * 100);
            this.pulse.Tick += new EventHandler(this.pulse_Elapsed);
		}

		private void pulse_Elapsed(object sender, EventArgs e)
		{
			this.Next();
		}

		public Boolean IsActive { get; private set; }

		public void Start()
		{
			this.IsActive = true;
			this.pulse.Start();
		}

		public void Stop()
		{
			this.IsActive = false;
			this.pulse.Stop();
		}

		public void Clear()
		{
			if (this.IsActive)
			{
				this.Stop();
			}

			for (int i = 0; i < this.current.Length; i++)
			{
				this.current[i] = 0;
            }

            if (null != this.Update)
            {
                this.Update(this);
            }
		}

		public void Randomize()
		{
			if (this.IsActive)
			{
				this.Stop();
			}

			Random r = new Random((int)DateTime.Now.Ticks);

			for (int i = 0; i < this.current.Length; i++)
			{
				this.current[i] = Convert.ToByte(r.Next(0, 2));
            }

            if (null != this.Update)
            {
                this.Update(this);
            }
		}

		public void Next()
		{
			bool living;
			int count;
			bool result;

			for (int i = 0; i < this.current.Length; i++)
			{
				living = this.current[i] > 0;
				count = this.getLivingNeighbours(i);
				result = false;

				if (living && count < 2)
				{
					result = false;
				}
				else if (living && (2 == count || 3 == count))
				{
					result = true;
				}
				else if (living && count > 3)
				{
					result = false;
				}
				else if (!living && count == 3)
				{
					result = true;
				}

				if (result)
				{
					this.next[i] = 1;
				}
				else
				{
					this.next[i] = 0;
				}
			}

            // Copy the next state over into current
            for (int i = 0; i < this.current.Length; i++)
            {
                this.current[i] = this.next[i];
            }

            if (null != this.Update)
            {
                this.Update(this);
            }
		}

		public void ToggleCell(int x, int y)
		{
			int i = y * Constants.CELLS_X + x;

			if (0 == this.current[i])
			{
				this.current[i] = 1;
			}
			else
			{
				this.current[i] = 0;
			}

			if (null != this.Update)
			{
				this.Update(this);
			}
		}

		private int getLivingNeighbours(int i)
		{
            int x = (i % Constants.CELLS_X);
            int y = (i / Constants.CELLS_X);

            int up = (y == 0) ? Constants.CELLS_X * (Constants.CELLS_Y - 1) : -Constants.CELLS_X;
            int down = (y == Constants.CELLS_Y - 1) ? -Constants.CELLS_X * (Constants.CELLS_Y - 1) : Constants.CELLS_X;
            int left = (x == 0) ? (Constants.CELLS_X - 1) : -1;
            int right = (x == Constants.CELLS_X - 1) ? (-Constants.CELLS_X + 1) : 1;

            int count = this.current[i + left + up] + this.current[i + up] + this.current[i + right + up]
                        + this.current[i + left] + this.current[i + right]
                        + this.current[i + left + down] + this.current[i + down] + this.current[i + right + down];

            return count;
        }
	}
}
