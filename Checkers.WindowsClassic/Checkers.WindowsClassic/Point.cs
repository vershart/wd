
namespace Checkers.WindowsClassic
{
    class Point
    {

        private int _x;
        private int _y;

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public bool InBounds()
        {
            if (this.X < 8 && this.X >= 0 && this.Y < 8 && this.Y >= 0)
                return true;
            else
                return false;
        }

        public static bool InBounds(int X, int Y)
        {
            if (X < 8 && X >= 0 && Y < 8 && Y >= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// New nullable point {x = 0; y = 0}
        /// </summary>
        public Point()
        {
            _x = 0;
            _y = 0;
        }

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }


    }
}
