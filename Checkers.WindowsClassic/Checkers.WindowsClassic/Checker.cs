using System.Windows.Media;

namespace Checkers.WindowsClassic
{
    public class Checker
    {

        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            private set
            {
                color = value;
            }
        }

        private bool isQueen;
        public bool IsQueen
        {
            get
            {
                return isQueen;
            }
            private set
            {
                isQueen = value;
            }
        }

        private bool userChecker;
        public bool UserChecker
        {
            get
            {
                return userChecker;
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            private set
            {
                isSelected = value;
            }
        }

        public void MakeQueen()
        {
            IsQueen = true;
        }
        public void Select()
        {
            IsSelected = true;
        }
        internal void UnselectChecker()
        {
            IsSelected = false;
        }

        public Checker(bool UserChecker, Color CheckerColor)
        {
            this.userChecker = UserChecker;
            Color = CheckerColor;
        }

    }
}
