using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.WindowsClassic
{

    public class Cell
    {

        private string cellRealName;
        public string CellRealName
        {
            get
            {
                return cellRealName;
            }
            private set
            {
                cellRealName = value;
            }
        }

        private CellType type;
        public CellType Type
        {
            get
            {
                return type;
            }
            private set
            {
                type = value;
            }
        }

        public Checker Checker;

        public bool IsFreeCell()
        {
            if (Checker == null)
                return true;
            else
                return false;
        }

        public bool HasUserChecker()
        {
            if (IsFreeCell() || !Checker.UserChecker)
                return false;
            else
                return true;
        }

        public bool HasEnemyChecker()
        {
            if (IsFreeCell() || Checker.UserChecker)
                return false;
            else
                return true;
        }

        public Cell(string realName, CellType cellType, Checker checker)
        {
            CellRealName = realName;
            type = cellType;
            Checker = checker;
        }

    }

}
