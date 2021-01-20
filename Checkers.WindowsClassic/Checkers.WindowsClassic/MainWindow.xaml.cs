using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Checkers;
using Checkers;

namespace Checkers.WindowsClassic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        System.Windows.Threading.DispatcherTimer painter = new System.Windows.Threading.DispatcherTimer();

        Game game;
        bool UndoReq;

        public MainWindow()
        {
            InitializeComponent();

            //this.SizeToContent = SizeToContent.WidthAndHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            game = new Game();
            UndoReq = false;

            painter.Interval = new TimeSpan(0, 0, 0, 0, 100);
            painter.Tick += Painter_Paint;
            painter.Start();

            game.FieldSize = 512;

        }

        private void Painter_Paint(object sender, EventArgs e)
        {
            //Canvas cv = sender as Canvas;
            Field.Children.Clear();
            sender = Field;

            if (!game.UserTurn)
            {
                //
                game.EnemyTurn();
                game.UserTurn = true;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cell c = game.Field[i, j];
                    Rect r = new Rect(i * game.CellSize, j * game.CellSize, game.CellSize, game.CellSize);

                    if (c.Type == CellType.Black)
                    {
                        DrawRectangle(sender as Canvas, i * game.CellSize, j * game.CellSize, Colors.Black);
                    }
                    else
                        DrawRectangle(sender as Canvas, i * game.CellSize, j * game.CellSize, Colors.White);

                    //args.DrawingSession.FillRectangle(r, cscb[(int)c.Type]);
                    //args.DrawingSession.DrawRectangle(r, cscb[(int)c.Type]);

                    if (c.Checker != null)
                    {
                        float x = i * (float)game.CellSize + (float)(game.CellSize / 2);
                        float y = j * (float)game.CellSize + (float)(game.CellSize / 2);
                        DrawChecker(sender, c.Checker.Color, x, y, c.Checker.IsSelected, c.Checker.IsQueen);
                    }
                }
            }

        }

        private void DrawRectangle(Canvas sender, float x, float y, Color fill, bool Stroke = false)
        {
            Rectangle rect = new Rectangle();

            if (Stroke)
            {
                rect.StrokeThickness = 2;
                rect.Stroke = new SolidColorBrush(fill);
            }
            else
                rect.Fill = new SolidColorBrush(fill);

            rect.Width = game.CellSize;
            rect.Height = game.CellSize;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            sender.Children.Add(rect);
        }

        private void DrawCircle(Canvas sender, float x, float y, Color fill, bool Stroke = false, double StrokeSize = 0)
        {

            SolidColorBrush scb = new SolidColorBrush();
            scb.Color = fill;

            Ellipse el = new Ellipse();
            el.Width = game.CheckerRadiuss * 2;
            el.Height = game.CheckerRadiuss * 2;

            el.StrokeThickness = StrokeSize;

            if (Stroke)
                el.Stroke = scb;
            else
                el.Fill = scb;

            sender.Children.Add(el);
            Canvas.SetTop(el, y - (game.CheckerRadiuss));
            Canvas.SetLeft(el, x - (game.CheckerRadiuss));

        }

        private void DrawChecker(object sender, Color color, float x, float y, bool isSelected, bool isQueen)
        {

            Canvas args = sender as Canvas;

            if (color == Colors.Black)
            {
                DrawCircle(args, x, y, Colors.Brown);
                //args.DrawCircle(x, y, game.CheckerRadiuss, Colors.Black);
                //args.FillCircle(x, y, game.CheckerRadiuss, Colors.Brown);
            }
            else
            {
                DrawCircle(args, x, y, Colors.DimGray);
                //args.DrawingSession.DrawCircle(x, y, game.CheckerRadiuss, Colors.Black);
                //args.DrawingSession.FillCircle(x, y, game.CheckerRadiuss, Colors.DimGray);
            }
            if (isQueen)
            {
                DrawCircle(args, x, y, Colors.Red, true, 5);
                //args.DrawingSession.DrawCircle(x, y, game.CheckerRadiuss / 2, Colors.Red);
            }
            if (isSelected)
            {

                DrawCircle(args, x, y, Colors.Green, true, 3);

                //args.DrawingSession.DrawCircle(x, y, game.CheckerRadiuss, Colors.Green, 3);
            }
        }

        private void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Field_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!game.UserTurn)
                return;
            game.SelectCell(e.GetPosition(sender as Canvas).X, e.GetPosition(sender as Canvas).Y);
            //log.Text = string.Format("Selected: x({0}):y({1})", game.SelectedChecker[0], game.SelectedChecker[1]);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            game.MakeUndo();
            UndoReq = false;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            game = new Game();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control)
                         == ModifierKeys.Control)

            {
                MenuItem_Click(null, null);
            }
        }
    }
}
