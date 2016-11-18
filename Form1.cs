using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIChess
{
    public partial class Form1 : Form
    {
        int BoardBorder = 20;

        int BoardLineNum = 20;
        int BoardWidth = 25;
        
        int ChessRadious = 18;

        int turn = 1; //1为黑
        int start = 0;
        int[,] ChessArray;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void DrawBoard(int linenum,int gap)
        {

            //行数-1
            int horC = linenum-1;

            Pen pen = new Pen(Color.Black, 1);
            Image img = new Bitmap(horC * gap + BoardBorder * 2, horC * gap + BoardBorder*2);
            Graphics gra = Graphics.FromImage(img);
            gra = Graphics.FromImage(img);
            gra.Clear(Color.White);
            gra.DrawRectangle(pen, BoardBorder, BoardBorder, horC * gap, horC * gap);
            //画棋盘
            for (int i = 0; i < horC; i++)
            {
                gra.DrawLine(pen, BoardBorder, i * gap + BoardBorder, horC * gap + BoardBorder, i * gap + BoardBorder);
                gra.DrawLine(pen, i * gap + BoardBorder, BoardBorder, i * gap + BoardBorder, horC * gap + BoardBorder);
            }
            //gra.DrawLine(pen, 20, horC * gap - 1, horC * gap, horC * gap - 1);
            //gra.DrawLine(pen, horC * gap - 1, 20, horC * gap - 1, horC * gap);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Image = img;
        }

        public void DrawSignV(int linenum, int gap)
        {
            Pen pen = new Pen(Color.Black, 1);
            Image img = new Bitmap(21, linenum * gap + BoardBorder);
            Graphics gra = Graphics.FromImage(img);
            gra = Graphics.FromImage(img);
            gra.Clear(Color.FromArgb(0, 255, 255, 255));

            Font myFont = new Font("宋体", 10, FontStyle.Bold);             

            Brush bush = new SolidBrush(Color.Black);//填充的颜色

            //gra.DrawString("1", myFont, bush, new Point(7,3));
            for (int i = 0; i < linenum; i++)
            {
                gra.DrawString((i + 1).ToString(), myFont, bush, new Point((i < 9) ? 7 : 0, i * gap + BoardBorder));
            }
            pictureBox2.Image = img;
        }
        public void DrawSignH(int linenum, int gap)
        {
            Pen pen = new Pen(Color.Black, 1);
            Image img = new Bitmap(linenum * gap + BoardBorder, 21);
            Graphics gra = Graphics.FromImage(img);
            gra = Graphics.FromImage(img);
            gra.Clear(Color.FromArgb(0, 255, 255, 255));

            Font myFont = new Font("宋体", 10, FontStyle.Bold);

            Brush bush = new SolidBrush(Color.Black);//填充的颜色
            for (int i = 0; i < linenum; i++)
            {
                gra.DrawString((i + 1).ToString(), myFont, bush, new Point(i * gap + BoardBorder, 5));
            }
            pictureBox3.Image = img;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            DrawBoard(BoardLineNum, BoardWidth);
            DrawSignV(BoardLineNum, BoardWidth);
            DrawSignH(BoardLineNum, BoardWidth);
            ChessArray = new int[BoardLineNum, BoardLineNum];
            start = 1;
            turn = 1; //1为黑
        }

        public void DrawChess(int x,int y)
        {
            Graphics graphic = pictureBox1.CreateGraphics();
            Pen pen1 = new Pen(Color.Black, 2);
            Brush bru1 = new SolidBrush(Color.Black);
            Pen pen2 = new Pen(Color.Black, 2);
            Brush bru2 = new SolidBrush(Color.White);
            int newX = (int)BoardBorder + ((x * BoardWidth + BoardWidth / 2) / BoardWidth) * BoardWidth - ChessRadious / 2;
            int newY = (int)BoardBorder + ((y * BoardWidth + BoardWidth / 2) / BoardWidth) * BoardWidth - ChessRadious / 2;
            if (turn == 1)
            {
                graphic.DrawEllipse(pen1, newX, newY, ChessRadious, ChessRadious);
                graphic.FillEllipse(bru1, newX, newY, ChessRadious, ChessRadious);
                ChessArray[x, y] = turn;
            }
            else
            {
                graphic.DrawEllipse(pen2, newX, newY, ChessRadious, ChessRadious);
                graphic.FillEllipse(bru2, newX, newY, ChessRadious, ChessRadious);
                ChessArray[x, y] = turn;
            }
            graphic.Dispose();
            if (Victory(x, y))
            {
                if (turn == 1)
                {

                    start = 0;
                    MessageBox.Show("黑方胜利");
                }
                else
                {
                    start = 0;
                    MessageBox.Show("白方胜利");

                }

            }
            turn = -turn;

            if (start == 1)
            //if (turn == -1 && start == 1)
            {
                int[] loc = Decision(-1);
                DrawChess(loc[0], loc[1]);
            }
        }

        #region 判断棋子局势

        private int[] Decision(int turn)
        {
            int maxx=0, maxy=0;
            int[,] PointArray = new int[BoardLineNum, BoardLineNum];
            for (int i = 0; i < BoardLineNum; i++)
            {
                for (int j = 0; j < BoardLineNum; j++)
                {
                    if (ChessArray[i, j] == 0)
                    {
                        ChessArray[i, j] = turn;
                        PointArray[i, j] = ValueChess(i, j);
                        ChessArray[i, j] = -turn;
                        PointArray[i, j] = PointArray[i, j] + ValueChess(i, j);
                        ChessArray[i, j] = 0;
                    }
                    else
                        PointArray[i, j] = -999999;

                }
            }
            int k = PointArray[0, 0];
            for (int i = 0; i < BoardLineNum; i++)
            {
                for (int j = 0; j < BoardLineNum; j++)
                {
                    if (k < PointArray[i, j])
                    {
                        k = PointArray[i, j];
                        maxx = i;
                        maxy = j;
                    }
                }
            }
            MessageBox.Show(k.ToString());
            return new int[] { maxx, maxy };
        }

        private int ValueChess(int bx, int by)
        {
            return SValue(bx, by) + HorValue(bx, by) + VerValue(bx, by);
        }

        private int SValue(int bx, int by)
        {

            int b1 = 0, b2 = 0;
            if ((bx > by ? by : bx) - 5 < 0)
            {
                b1 = bx - (bx > by ? by : bx);
                b2 = by - (bx > by ? by : bx);
            }
            else
            {
                b1 = bx - 5;
                b2 = by - 5;
            }
            //int buttom = b1 > b2 ? b2 : b1;
            int val = ChessArray[bx, by];
            for (int i = b1, j = b2; i < BoardLineNum - 5 && j < BoardLineNum - 5; i++, j++)
            {
                if (ChessArray[i, j] == val && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == val)
                    return 100000;//左上到右下五连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == val && ChessArray[i + 5, j + 5] == 0)
                    return 10000;//左上到右下空四连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == 0
                    && ChessArray[i + 4, j + 4] == val)
                    return 1000;//左上到右下间隔五连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == 0 && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == val)
                    return 1000;//左上到右下间隔五连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j + 1] == 0 &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == val)
                    return 1000;//左上到右下间隔五连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == 0)
                    return 1000;//左上到右下四连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == val)
                    return 1000;//左上到右下四连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == 0
                    && ChessArray[i + 4, j + 4] == val && ChessArray[i + 5, j + 5] == 0)
                    return 700;//左上到右下间隔三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == 0 && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == val && ChessArray[i + 5, j + 5] == 0)
                    return 700;//左上到右下间隔三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == 0)
                    return 800;//左上到右下三连
                if (ChessArray[i, j] == -val && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == 0)
                    return 800;//左上到右下一侧三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == -val)
                    return 800;//左上到右下一侧三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == 0)
                    return 300;//左上到右下二连

            }

            b2 = (by + 5) < BoardLineNum - 1 ? by + 5 : BoardLineNum - 1;
            for (int i = b1, j = b2; i < BoardLineNum - 5 && j >= 5; i++, j--)
            {
                if (ChessArray[i, j] == val && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == val)
                    return 100000;//左下到右上五连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == val && ChessArray[i + 5, j - 5] == 0)
                    return 10000;//左下到右上空四连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == 0
                    && ChessArray[i + 4, j - 4] == val)
                    return 1000;//左下到右上间隔五连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == 0 && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == val)
                    return 1000;//左下到右上间隔五连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j - 1] == 0 &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == val)
                    return 1000;//左下到右上间隔五连
                if (ChessArray[i, j] == val && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == 0)
                    return 1000;//左下到右上四连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == val)
                    return 1000;//左下到右上四连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == 0
                    && ChessArray[i + 4, j - 4] == val && ChessArray[i + 5, j - 5] == 0)
                    return 700;//左下到右上间隔三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == 0 && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == val && ChessArray[i + 5, j - 5] == 0)
                    return 700;//左下到右上间隔三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == 0)
                    return 800;//左下到右上三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == -val)
                    return 800;//左下到右上一侧三连
                if (ChessArray[i, j] == -val && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == 0)
                    return 800;//左下到右上一侧三连
                if (ChessArray[i, j] == 0 && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == 0)
                    return 300;//左下到右上二连
            }
            return 0;
        }

        private int VerValue(int bx, int by)
        {
            int buttom = (by - 5) > 0 ? by - 5 : 0;
            int val = ChessArray[bx, by];
            for (int i = buttom; i < BoardLineNum - 5; i++)
            {
                if (ChessArray[bx, i] == val && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == val)
                    return 100000;//上到下五连
                if (ChessArray[bx, i] == 0 && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == val && ChessArray[bx, i + 5] == 0)
                    return 10000;//上到下空四连
                if (ChessArray[bx, i] == val && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == 0
                    && ChessArray[bx, i + 4] == val)
                    return 1000;//上到下间隔五连
                if (ChessArray[bx, i] == val && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == 0 && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == val)
                    return 1000;//上到下间隔五连
                if (ChessArray[bx, i] == val && ChessArray[bx, i + 1] == 0 &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == val)
                    return 1000;//上到下间隔五连
                if (ChessArray[bx, i] == val && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == 0)
                    return 1000;//上到下四连
                if (ChessArray[bx, i] == 0 && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == val)
                    return 1000;//上到下四连
                if (ChessArray[bx, i] == 0 && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == 0
                    && ChessArray[bx, i + 4] == val && ChessArray[bx, i + 5] == 0)
                    return 700;//上到下间隔三连
                if (ChessArray[bx, i] == 0 && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == 0 && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == val && ChessArray[bx, i + 5] == 0)
                    return 700;//上到下间隔三连
                if (ChessArray[bx, i] == 0 && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == 0)
                    return 800;//上到下三连
                if (ChessArray[bx, i] == 0 && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == -val)
                    return 800;//上到下一侧三连
                if (ChessArray[bx, i] == -val && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == 0)
                    return 800;//上到下一侧三连
                if (ChessArray[bx, i] == 0 && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == 0)
                    return 300;//上到下二连
            }
            return 0;
        }

        private int HorValue(int bx, int by)
        {
            int left = (bx - 5) > 0 ? bx - 5 : 0;
            int val = ChessArray[bx, by];
            for (int i = left; i < BoardLineNum - 5; i++)
            {
                if (ChessArray[i, by] == val && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == val)
                    return 100000;//左到右五连
                if (ChessArray[i, by] == 0 && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == val && ChessArray[i, by] == 0 )
                    return 10000;//左到右空四连
                if (ChessArray[i, by] == val && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == 0
                    && ChessArray[i + 4, by] == val)
                    return 1000;//左到右间隔五连
                if (ChessArray[i, by] == val && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == 0 && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == val)
                    return 1000;//左到右间隔五连
                if (ChessArray[i, by] == val && ChessArray[i + 1, by] == 0 &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == val)
                    return 1000;//左到右间隔五连
                if (ChessArray[i, by] == val && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == 0)
                    return 1000;//左到右四连
                if (ChessArray[i, by] == 0 && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == val)
                    return 1000;//左到右四连
                if (ChessArray[i, by] == 0 && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == 0
                    && ChessArray[i + 4, by] == val && ChessArray[i, by] == 0)
                    return 700;//左到右间隔三连
                if (ChessArray[i, by] == 0 && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == 0 && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == val && ChessArray[i, by] == 0)
                    return 700;//左到右间隔三连
                if (ChessArray[i, by] == 0 && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == 0)
                    return 800;//左到右三连
                if (ChessArray[i, by] == 0 && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == -val)
                    return 400;//左到右一侧三连
                if (ChessArray[i, by] == -val && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == 0)
                    return 400;//左到右一侧三连
                if (ChessArray[i, by] == 0 && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == 0)
                    return 300;//左到右二连
            }

            return 0;
        }
        #endregion


        #region 判断胜利
        private bool Victory(int bx, int by)
        {
            if (HorVic(bx, by))
                return true;
            if (VerVic(bx, by))
                return true;
            if (Vic45(bx, by))
                return true;
            else
                return false;
        }

        private bool Vic45(int bx, int by)
        {
            int b1=0,b2=0;
            if ((bx > by ? by : bx) - 4 < 0)
            {
                b1 = bx - (bx > by ? by : bx);
                b2 = by - (bx > by ? by : bx);
            }
            else
            {
                b1 = bx - 4 ;
                b2 = by - 4 ;
            }
            //int buttom = b1 > b2 ? b2 : b1;
            int val = ChessArray[bx, by];
            for (int i = b1, j = b2; i < BoardLineNum - 4 && j < BoardLineNum-4; i++, j++)
            {
                if (ChessArray[i, j] == val && ChessArray[i + 1, j + 1] == val &&
                    ChessArray[i + 2, j + 2] == val && ChessArray[i + 3, j + 3] == val
                    && ChessArray[i + 4, j + 4] == val)
                    return true;
            }
            b2 = (by + 4) < BoardLineNum-1 ? by + 4 : BoardLineNum-1;
            for (int i = b1, j = b2; i < BoardLineNum - 4 && j > 3; i++, j--)
            {
                if (ChessArray[i, j] == val && ChessArray[i + 1, j - 1] == val &&
                    ChessArray[i + 2, j - 2] == val && ChessArray[i + 3, j - 3] == val
                    && ChessArray[i + 4, j - 4] == val)
                    return true;
            }
            return false;
        }

        private bool VerVic(int bx, int by)
        {
            int buttom = (by - 4) > 0 ? by - 4 : 0;
            int val = ChessArray[bx, by];
            for (int i = buttom; i < BoardLineNum-4; i++)
            {
                if (ChessArray[bx, i] == val && ChessArray[bx, i + 1] == val &&
                    ChessArray[bx, i + 2] == val && ChessArray[bx, i + 3] == val
                    && ChessArray[bx, i + 4] == val)
                    return true;
            }
            return false;
        }

        private bool HorVic(int bx, int by)
        {
            int left = (bx - 4) > 0 ? bx - 4 : 0;
            int val = ChessArray[bx, by];
            for (int i = left; i < BoardLineNum-4; i++)
            {
                if (ChessArray[i, by] == val && ChessArray[i + 1, by] == val &&
                    ChessArray[i + 2, by] == val && ChessArray[i + 3, by] == val
                    && ChessArray[i + 4, by] == val)
                    return true;
            }
            return false;
        }
        #endregion




        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (start == 1)
            {
                if ((((e.X - BoardBorder) % BoardWidth) > (BoardWidth / 3) && ((e.X - BoardBorder) % BoardWidth) < (BoardWidth / 3 * 2)) || (((e.Y - BoardBorder) % BoardWidth) > (BoardWidth / 3) && ((e.Y - BoardBorder) % BoardWidth) < (BoardWidth / 3 * 2)))
                {
                    //MessageBox.Show("无效地点");
                }
                else
                {

                    int locx = (((e.X - BoardBorder) % BoardWidth) > (BoardWidth / 3)) ? ((e.X - BoardBorder) / BoardWidth) + 1 : (e.X - BoardBorder) / BoardWidth;
                    int locy = (((e.Y - BoardBorder) % BoardWidth) > (BoardWidth / 3)) ? ((e.Y - BoardBorder) / BoardWidth) + 1 : (e.Y - BoardBorder) / BoardWidth;
                    //Console.WriteLine(locx + " " + locy);
                    if (ChessArray[locx, locy] == 0)
                    {
                        DrawChess(locx, locy);
                        Console.WriteLine(ValueChess(locx, locy).ToString());

                    }
                    else
                    {
                        //MessageBox.Show("已有棋子！");
                    }
                }
            }
            else
            {
                int locx = (((e.X - BoardBorder) % BoardWidth) > (BoardWidth / 3)) ? ((e.X - BoardBorder) / BoardWidth) + 1 : (e.X - BoardBorder) / BoardWidth;
                int locy = (((e.Y - BoardBorder) % BoardWidth) > (BoardWidth / 3)) ? ((e.Y - BoardBorder) / BoardWidth) + 1 : (e.Y - BoardBorder) / BoardWidth;
                Console.WriteLine(ValueChess(locx, locy).ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
