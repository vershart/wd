using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Checkers.WindowsClassic
{

    class Game
    {

        public bool UserTurn;

        public short[] SelectedChecker = new short[2] { -1, -1};

        private string[] Letters = new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" };
        private string[] Digits = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };

        public float FieldSize = 0;
        public float CellSize { get { return FieldSize / 8; } }
        public float CheckerSize
        {
            get
            {
                return (float)CellSize - (float)(CellSize * 0.25);
            }
        }
        public float CheckerRadiuss
        {
            get
            {
                return CheckerSize / 2;
            }
        }

        public int FieldMatrixSize { get { return 8; } }

        public List<Cell[,]> UndoStates;

        public Cell[,] Field;

        public Color UserColor = Colors.White;
        public Color EnemyColor = Colors.Black;

        /// <summary>
        /// Определяем: клетка содержит белую или черную шашку, или не содержит шашку вообще.
        /// Действует только для создания нового поля
        /// </summary>
        /// <param name="i">Позиция i на игровом поле (столбец)</param>
        /// <param name="j">Позиция j на игровом поле (строка)</param>
        /// <returns></returns>
        private bool IsEmpty(int i, int j)
        {
            // Если строка, в которой могут быть размещены шашки (строки 1-3 и строки 6-8 соответственно)
            // Если строка четная (2, 6, 8) и столбец нечетный (B, D, F, H)
            // Если строка нечетная (1, 3, 7) и столбец четный (A, C, E, G)
            if ((j <= 2 || j >= 5) && ((i % 2 != 0 & j % 2 == 0) || (i % 2 == 0 & j % 2 != 0)))
                return false;
            else
                return true;
        }

        public Game()
        {
            SelectedChecker = new short[2] { -1, -1};
            Field = new Cell[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool isEmpty = IsEmpty(i, j);
                    CellType type = CellType.White;
                    Checker ch = null;
                    if (!isEmpty)
                        if (j <= 2)
                            ch = new Checker(false, EnemyColor);
                        else if (j >= 5)
                            ch = new Checker(true, UserColor);
                    string cellName = Letters[j] + Digits[i];
                    bool isBlack = ((j % 2 != 0 && i % 2 == 0) || (j % 2 == 0 && i % 2 != 0));
                    if (isBlack)
                        type = CellType.Black;

                    //if (i == 0 & j == 7)
                    //    Field[i, j] = new Cell(cellName, type, ch);
                    //else if (i == 1 & j == 0)
                    //    Field[i, j] = new Cell(cellName, type, ch);
                    //else
                    //    Field[i, j] = new Cell(cellName, type, null);

                    //if(j > 5)
                    //    Field[i, j] = new Cell(cellName, type, null);
                    //else
                    //    Field[i, j] = new Cell(cellName, type, ch);

                    Field[i, j] = new Cell(cellName, type, ch);


                }
            }

            UndoStates = new List<Cell[,]>();
            StateCurrentState();

            //Undo = Field;
            UserTurn = true;
        }

        void StateCurrentState()
        {
            Cell[,] curState = new Cell[8,8];
            for (int i = 0; i < FieldMatrixSize; i++)
            {
                for (int j = 0; j < FieldMatrixSize; j++)
                {
                    curState[i, j] = new Cell(Field[i, j].CellRealName, Field[i, j].Type, Field[i,j].Checker);
                }
            }
            UndoStates.Add(curState);
        }

        #region Общая логика (пользователь-враг)

        int[,] directions = new int[4, 2] { { 1, 1 }, { -1, 1 }, { 1, -1 }, { -1, -1 } };

        public List<Point> SearchNextEating_Queen(int x, int y, int lx = -1, int ly = -1, bool UserQueen = false)
        {
            List<Point> posways = new List<Point>();

            for (int n = 0; n < 4; n++)
            {
                int dirx = directions[n, 0];
                int diry = directions[n, 1];

                int dx = x + dirx;
                int dy = y + diry;

                if (dx == lx & dy == ly)
                    continue;

                while (Point.InBounds(dx, dy))
                {

                    if (Field[dx, dy].Checker == null)
                    {
                        dx += dirx;
                        dy += diry;
                        continue;

                    }

                    if (Field[dx, dy].HasEnemyChecker())
                        break;

                    if (Field[dx, dy].HasUserChecker())
                    {
                        int dxi = dx + dirx;
                        int dyi = dy + diry;
                        if (!Point.InBounds(dxi, dyi))
                            break;
                        if (!Field[dxi, dyi].IsFreeCell())
                            break;

                        
                        Point[] newPoints = new Point[] { new Point(x, y), new Point(dx, dy), new Point(dxi, dyi) };

                        posways.AddRange(newPoints);

                        List<Point> nextpoints = SearchNextEating_Queen(dx, dy, x, y);
                        if (nextpoints != null)
                            posways.AddRange(nextpoints);
                        else
                            return posways;

                    }



                    dx += dirx;
                    dy += diry;

                }

            }

            return posways;
        }

        #endregion

        #region Правила

        // В этом регионе будут описаны правила для настоящей игры

        public bool IsEatingRequired = true;
        public bool IsEatingBackAllowed = true;
        public int PossibleWaysToEat
        {
            get
            {
                if (IsEatingBackAllowed)
                    return 4;
                else
                    return 2;
            }
        }

        public bool CanUndo { get
            {
                return (UndoStates.Count > 0);
            }
        }

        #endregion

        #region Логика пользователя

        private void SelectChecker(short x, short y)
        {
            SelectedChecker[0] = x;
            SelectedChecker[1] = y;
        }

        internal void SelectCell(double x, double y)
        {

            byte X = (byte)(x / CellSize);
            byte Y = (byte)(y / CellSize);

            if (X > FieldMatrixSize || X < 0 || Y < 0 || Y > FieldMatrixSize)
                return;

            Cell cell = Field[X, Y];        // Выбранная клетка
            Checker checker = cell.Checker; // Шашка выбранной клетки

            // Не игровая клетка или не пользовательская шашка
            if (cell.Type == CellType.White || (checker != null && !checker.UserChecker))
                return;

            if (IsEatingRequired)
            {
                List<Point> eatways = User_FindWaysToEat();
                if (eatways.Count > 0 && SelectedChecker[0] == -1 && SelectedChecker[1] == -1)
                {
                    bool CorrectSelection = false;
                    for (int i = 0; i < eatways.Count; i++)
                    {
                        int dx = eatways[i].X;
                        int dy = eatways[i].Y;

                        if (dx == X && dy == Y)
                            CorrectSelection = true;

                    }

                    if (!CorrectSelection)
                    {
                        //Debug.WriteLine("Incorrect selection at {0}:{1}", X, Y);
                        return;
                    }
                    else
                    {
                        SelectedChecker[0] = X;
                        SelectedChecker[1] = Y;
                        Field[X, Y].Checker.Select();
                        return;
                    }


                }
                else if (eatways.Count > 0 && SelectedChecker[0] != -1 && SelectedChecker[1] != -1)
                {
                    // Есть надо, уже выбрана верная шашка, теперь выбрали конечную клетку после поедания

                    if (User_PerformEating(SelectedChecker[0], SelectedChecker[1], X, Y))
                    {


                        // Только одно поедание пока

                        if (User_HasToEat(X, Y))
                        {
                            return;
                        }

                        SelectedChecker[0] = -1;
                        SelectedChecker[1] = -1;
                        Field[X, Y].Checker.UnselectChecker();
                        UserTurn = false;
                        return;
                    }
                    else
                        return; // Не тот путь ты выбрал, пользователь

                }
            }

            // Выбрали шашку, определяем возможность хода
            if (SelectedChecker[0] == -1 & checker != null)
            {
                // Пользователь может ходить, выбираем шашку
                if (CanMove(X, Y))
                {
                    Field[X, Y].Checker.Select();
                    SelectChecker(X, Y);
                    return;
                }
                // Не может ходить
                return;
            }
            else if (SelectedChecker[0] != -1)
            {

                // Шашка уже выбрана, выбрали путь

                // Шашка уже выбрана, но пользователь опять её выбрал - передумал

                if (SelectedChecker[0] == X && SelectedChecker[1] == Y)
                {
                    SelectChecker(-1, -1);
                    Field[X, Y].Checker.UnselectChecker();
                    return;
                }

                int sx = SelectedChecker[0];
                int sy = SelectedChecker[1];

                bool isQueen = Field[sx, sy].Checker.IsQueen;
                if (CanMoveTo(X, Y, isQueen))
                {
                    //Undo.Add(Field);
                    Move(X, Y);
                    UserTurn = false;
                    return;
                }
            }

           }

        internal void MakeUndo()
        {

            if (!CanUndo)
                return;
            Cell[,] lastState = UndoStates[UndoStates.Count - 1];

            if (CanUndo)
            {
                //Cell[,] gav = Undo;
                for (int i = 0; i < FieldMatrixSize; i++)
                {
                    for (int j = 0; j < FieldMatrixSize; j++)
                    {
                        Field[i, j].Checker = lastState[i,j].Checker;
                    }
                }
            }
            UndoStates.RemoveAt(UndoStates.Count - 1);
        }

        private bool User_PerformEating(short x, short y, short dx, short dy)
        {

            int posdirscount = PossibleWaysToEat; // Может быть два, если в правилах указано, что шашка не может есть назад     

            for (int n = 0; n < posdirscount; n++)
            {
                // Ближайщая клетка

                int xi = x + directions[n, 0];
                int yi = y + directions[n, 1];

                if (!Point.InBounds(xi, yi))
                    continue;
                else if (!Field[xi, yi].HasEnemyChecker())
                    continue;

                int dxi = xi + directions[n, 0];
                int dyi = yi + directions[n, 1];

                if (!Point.InBounds(dxi, dyi))
                    continue;

                if (!Field[dxi, dyi].IsFreeCell())
                    continue;

                if (dxi != dx || dyi != dy)
                    continue;

                StateCurrentState();

                Field[dxi, dyi].Checker = Field[x, y].Checker;

                Field[x, y].Checker = null;
                Field[xi, yi].Checker = null;


                if (dyi == 7)
                    Field[dxi, dyi].Checker.MakeQueen();

                SelectedChecker[0] = (short)dxi;
                SelectedChecker[1] = (short)dyi;

                // Очки пользователю

                return true;

            }

            return false;

        }

        private bool User_HasToEat(short X = -1, short Y = -1)
        {
            List<Point> eatways = User_FindWaysToEat();
            if (eatways.Count != 0)
            {
                if (X != -1 && Y != -1)
                {
                    for (int i = 0; i < eatways.Count; i++)
                    {
                        if (eatways[i].X == X && eatways[i].Y == Y)
                            return true;
                    }
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        private List<Point> User_FindWaysToEat()
        {

            // Логика для дамок?

            List<Point> posways = new List<Point>();

            // Possible ways

            for (int i = 0; i < FieldMatrixSize; i++)
            {
                for (int j = 0; j < FieldMatrixSize; j++)
                {
                    if (Field[i, j].Checker != null && Field[i, j].Checker.Color == this.UserColor)
                    {

                        int posdirscount = PossibleWaysToEat; // Может быть два, если в правилах указано, что шашка не может есть назад     

                        for (int n = 0; n < posdirscount; n++)
                        {
                            // Ближайщая клетка

                            int x = i + directions[n, 0];
                            int y = j + directions[n, 1];

                            if (!Point.InBounds(x, y))
                                continue;
                            else if (!Field[x, y].HasEnemyChecker())
                                continue;

                            int dx = x + directions[n, 0];
                            int dy = y + directions[n, 1];

                            if (!Point.InBounds(dx, dy))
                                continue;

                            if (!Field[dx, dy].IsFreeCell())
                                continue;

                            Point pt = new Point(i, j);

                            posways.Add(pt);

                        }
                    }
                }
            }

            return posways;

        }

        private bool CanMoveTo(int x, int y, bool isQueen)
        {

            int X = SelectedChecker[0];
            int Y = SelectedChecker[1];

            // It's a QUEEN
            if (isQueen)
            {
                // X, Y = my Queen

                int j = Y;

                int mx, my;
                if (X < x)
                    mx = 1;
                else
                    mx = -1;

                if (Y < y)
                    my = 1;
                else
                    my = -1;

                for (int i = X; (i >= 0 && i < FieldMatrixSize); i += mx)
                {
                    if (i == x & j == y)
                        return true;
                    j += my;
                }

                return false;

            }

            if ((X - x == 1 || x - X == 1) && Y - y == 1)
            {
                Checker ch1 = Field[x, y].Checker;
                if (ch1 == null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Определяет, мешают ли собственные шашки передвижению,
        /// Выделяет клетки, по которым можно идти
        /// </summary>
        /// <param name="x">Координата Х на доске</param>
        /// <param name="y">Координата Y на доске</param>
        /// <returns>Мешают ли собственные шашки передвижению</returns>
        private bool CanMove(int x, int y)
        {

            // Я - королева? Могу ли я выйти в свет?!

            bool isQueen = Field[x, y].Checker.IsQueen;

            if (isQueen)
            {
                return true;
            }

            // Логика обычных шлюх-шашек

            // Есть ли шашки, которые мешают идти (наша шашка)?

            int x1 = x + 1;
            int x2 = x - 1;
            int y1 = y - 1;

            if (y1 < 0)
                return false;

            if (x1 < FieldMatrixSize)
            {
                Checker ch1 = Field[x1, y1].Checker;
                if (ch1 == null)
                    return true;
            }

            if (x2 >= 0)
            {
                Checker ch2 = Field[x2, y1].Checker;
                if (ch2 == null)
                    return true;
            }

            return false;

        }

        internal void Move(int x, int y)
        {

            StateCurrentState();

            int X = SelectedChecker[0];
            int Y = SelectedChecker[1];

            Field[X, Y].Checker.UnselectChecker();

            Field[x, y].Checker = Field[X, Y].Checker;

            // Если это 1 или 8 позиция, то шашка становится дамкой
            if (y == FieldMatrixSize - 1 | y == 0)
            {
                Field[x, y].Checker.MakeQueen();
            }

            Field[X, Y].Checker = null;

            SelectedChecker[0] = -1;
            SelectedChecker[1] = -1;


        }
        #endregion

        #region Логика врага

        public void EnemyTurn()
        {
            // Сначала определяем возможность сбить пользовательские шашки
            // На данный момент, по правилам это обязательно
            if (IsEatingRequired)
            {
                List<List<Point>> posWaysToEat = Enemy_SearchWaysToEat();

                if (posWaysToEat.Count != 0)
                {
                    //Undo.Add(Field);
                    List<Point> analyzedeating = Enemy_AnalyzedEating(posWaysToEat);
                    Enemy_PerformEating(analyzedeating);
                    UserTurn = true;
                    return;
                }

                //Undo.Add(Field);

                Enemy_MakeRandomMove();


            }
        }

        private void Enemy_MakeRandomMove()
        {

            Random rnd = new Random();
            bool d1 = ((rnd.Next(0, 100) % 2 == 0) ? true: false);
            bool d2 = ((rnd.Next(0, 100) % 2 == 0) ? true : false);

            int I_inc = (d1 ? 1 : -1);
            int J_inc = (d2 ? 1 : -1);

            bool done = false;
            for (int i = (d1 ? 0 : 7); (i < FieldMatrixSize && i >= 0); i += I_inc)
            {
                if (done)
                    break;
                for (int j = (d2 ? 0 : 7); (j < FieldMatrixSize && j >= 0); j += J_inc)
                {
                    if (done)
                        break;
                    if (Field[i, j].Checker != null && Field[i, j].Checker.Color == EnemyColor)
                    {
                        if (TryToMove(i, j))
                        {
                            done = true;
                        }
                    }
                }
            }
            // User won...
        }

        private bool TryToMove(int x, int y)
        {

            Checker ch = Field[x, y].Checker;

            // Это ДАМКА!
            if (ch.IsQueen)
            {
                return EnemyQueen_TryToMove(x, y);
            }

            int x1 = x + 1;
            int x2 = x - 1;
            int y1 = y + 1;

            if (y1 < 0)
                return false;
            if (y1 == 8)
                return false;

            if (x1 >= 0 & x1 < FieldMatrixSize)
            {
                Checker ch1 = Field[x1, y1].Checker;
                if (ch1 == null)
                {

                    Field[x1, y1].Checker = Field[x, y].Checker;

                    // Если это 1 позиция, то шашка становится дамкой
                    if (y1 == 7)
                    {
                        Field[x, y].Checker.MakeQueen();
                    }

                    Field[x, y].Checker = null;
                    return true;
                }
            }

            if (x2 < FieldMatrixSize & x2 > 0)
            {
                Checker ch2 = Field[x2, y1].Checker;
                if (ch2 == null)
                {
                    Field[x2, y1].Checker = Field[x, y].Checker;

                    // Если это 1 или 8 позиция, то шашка становится дамкой
                    if (y1 == 7)
                    {
                        Field[x, y].Checker.MakeQueen();
                    }

                    Field[x, y].Checker = null;
                    return true;
                }
            }

            return false;
        }

        Point[,] p = new Point[4,2];

        private Point EnemyQueen_PosWays(int x, int y, int k)
        {

            int dirx = directions[k, 0];
            int diry = directions[k, 1];

            int dx = x + dirx, dy = y + diry;
            int dxm = x, dym = y;

            while (dx < FieldMatrixSize && dx >= 0 && dy < FieldMatrixSize && dy >= 0)
            {

                // Предполагается, что шашки пользователя уже убиты
                // Предполагается, что остались только собственные шашки => они мешают двигаться
                if (Field[dx, dy].Checker != null)
                    break;

                dxm = dx;
                dym = dy;

                dx += dirx;
                dy += diry;
            }



            return new Point(dxm, dym);

        }

        private bool EnemyQueen_TryToMove(int x, int y)
        {
            bool Moved = false;

            Point[] p = new Point[4];

            for (int n = 0; n < 4; n++)
            {
                Point curp = EnemyQueen_PosWays(x, y, n);
                p[n] = curp;

            }

            int m = 0;
            while (m < 4 && !Moved)
            {
                Random rnd = new Random();
                if (rnd.Next(0,100) % 2 == 0 && p[m].X != x && p[m].Y != y)
                {
                    EnemyQueen_Move(x, y, p[m].X, p[m].Y);
                    Moved = true;
                    break;
                }
                m++;
            }


            return Moved;

        }

        private void EnemyQueen_Move(int x, int y, int dx, int dy)
        {
            if (!Point.InBounds(dx, dy))
                return;
            Field[dx, dy].Checker = Field[x, y].Checker;
            Field[x, y].Checker = null;
            return;
        }

        public void Enemy_PerformEating(List<Point> eatinglist)
        {

            if (eatinglist == null || eatinglist.Count <= 0)
                return;

            Checker ch = Field[eatinglist[0].X, eatinglist[0].Y].Checker;


            int i = 1;
            int x = 0, y = 0;

            for (i = 1; i < eatinglist.Count; i++)
            {
                x = eatinglist[i].X;
                y = eatinglist[i].Y;

                if (Field[x, y].Checker != null && Field[x, y].Checker.Color == UserColor)
                {
                    Field[x, y].Checker = null;
                    // Очки
                }

            }

            Field[x, y].Checker = ch;

            if (y == 7)
                Field[x, y].Checker.MakeQueen();

            Field[eatinglist[0].X, eatinglist[0].Y].Checker = null;

        }

        public List<Point> Enemy_AnalyzedEating(List<List<Point>> poseating)
        {

            int max = 0;
            int i = 0;
            int j = 0;
            foreach (List<Point> eatinglist in poseating)
            {
                if (max < eatinglist.Count)
                {
                    max = eatinglist.Count;
                    i = j;
                }

                j++;  
            }

            return poseating[i];

        }

        public List<Point> Enemy_SearchNextEating(int i, int j, int l = -1, int m = -1)
        {

            List<Point> nextposways = null;

            int posdirscount = PossibleWaysToEat; // Может быть два, если в правилах указано, что шашка не может есть назад     

            for (int n = 0; n < posdirscount; n++)
            {
                // Ближайщая клетка

                int x = i + directions[n, 0];
                int y = j + directions[n, 1];

                if (x == l & m == y)
                    continue;

                if (!Point.InBounds(x, y))
                    continue;
                else if (!Field[x, y].HasUserChecker())
                    continue;

                int dx = x + directions[n, 0];
                int dy = y + directions[n, 1];

                if (!Point.InBounds(dx, dy))
                    continue;

                if (!Field[dx, dy].IsFreeCell())
                    continue;

                nextposways = new List<Point>();
                Point[] newPoints = new Point[] { new Point(i, j), new Point(x, y), new Point(dx, dy) };

                nextposways.AddRange(newPoints);

                List<Point> nextpoints = Enemy_SearchNextEating(dx, dy, x, y);
                if (nextpoints != null)
                    nextposways.AddRange(nextpoints);
                else
                    return nextposways;
                

            }

            if (nextposways == null)
                return null;
            else
                return nextposways;

        }

        public List<List<Point>> Enemy_SearchWaysToEat()
        {
            List<List<Point>> posWays = new List<List<Point>>();

            // Possible ways

            for (int i = 0; i < FieldMatrixSize; i++)
            {
                for (int j = 0; j < FieldMatrixSize; j++)
                {
                    if (Field[i, j].Checker != null && Field[i, j].Checker.Color == this.EnemyColor)
                    {

                        List<Point> ls = null;

                        if (Field[i, j].Checker.IsQueen)
                            ls = SearchNextEating_Queen(i, j);
                        else
                            ls = Enemy_SearchNextEating(i, j);

                        if (ls != null)
                            posWays.Add(ls);

                    }
                }
            }

            return posWays;

        }

        #endregion

    }

    public enum CellType
    {
        Black = 0,
        White = 1
    }


}