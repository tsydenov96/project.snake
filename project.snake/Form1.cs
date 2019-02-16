using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace project.snake
{
    public partial class Form1 : Form
    {
        Bitmap myBitmap;
        string message = "Game over";
        public bool show_rul;
        public int apple_x;
        public int apple_y;
        public int w = 20;
        public int h = 20;
        public int t_x = 0;
        public int t_y = 0;
        private Hashtable keys;
        struct Snake
        {
            public int x;
            public int y;
            public Snake(int x, int y) {
                this.x = x;
                this.y = y;
            }
        }
        List<Snake> snake = new List<Snake>();
        public void SpawnApple()
        {
            Random rand = new Random();
            apple_x = rand.Next(0, 10) * 20;
            apple_y = rand.Next(0, 10) * 20;
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            show_rul = true;
            myBitmap = new Bitmap(400, 400);
            keys = new Hashtable{
                { 38, false },//up
                { 40, false },//down
                { 37, false },//left
                { 39, false }//rigth
            };
            Random rand = new Random();
            Snake s = new Snake(rand.Next(0, 10) * 20, rand.Next(0, 10) * 20);
            snake.Add(s);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // Turn on WS_EX_COMPOSITED
                cp.ExStyle |= 0x00080000;   //WS_EX_LAYERED    
                return cp;
            }
        }
        public void Mowe_Snake()
        {
            Snake s;
            if (snake[0].x == apple_x && snake[0].y == apple_y) {
                s = snake[snake.Count - 1];
                snake.Add(s);
                SpawnApple();
            };
            for (int i = snake.Count - 1; i > 0; i--) {
                snake[i] = snake[i - 1];
            }
            s = snake[0];
            s.x += this.t_x;
            s.y += this.t_y;
            snake[0] = s;
            if (snake[0].x > 380) {
                s = snake[0];
                s.x = 0;
                snake[0] = s;
            }
            if (snake[0].y > 380) {
                s = snake[0];
                s.y = 0;
                snake[0] = s;
            }
            if (snake[0].x < 0) {
                s = snake[0];
                s.x = 380;
                snake[0] = s;
            }
            if (snake[0].y < 0) {
                s = snake[0];
                s.y = 380;
                snake[0] = s;
            }
        }
        private bool Rules(){
            Snake c, c0;
            c0 = snake[0];
            for (int i = 1; i < snake.Count(); i++){
                c = snake[i];
                if (c.x == c0.x && c.y == c0.y){
                    show_rul = false;
                }
            }
            return show_rul;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.DrawGrid(e, 20, 20);
            this.Render(e.Graphics);
        }
        public void MyUpdate()
        {
            if (this.Rules()){
                if ((bool)this.keys[38]) // up arrow
                {
                    this.t_x = 0; this.t_y = -20;
                    this.keys[38] = false;
                }
                if ((bool)this.keys[40]) // down arrow
                {
                    this.t_x = 0; this.t_y = 20;
                    this.keys[40] = false;
                }
                if ((bool)this.keys[37]) // left arrow
                {
                    this.t_x = -20; this.t_y = 0;
                    this.keys[37] = false;
                }
                if ((bool)this.keys[39]) // right arrow
                {
                    this.t_x = 20; this.t_y = 0;
                    this.keys[39] = false;
                }
                this.Mowe_Snake();                
            }
        }
        public void Render(Graphics e)
        {
            Snake c;
            for (int i=1; i<snake.Count(); i++){ 
                c = snake[i];
                e.DrawEllipse(new Pen(Color.Blue, 4f), c.x, c.y, this.w, this.h);
            }
            c = snake[0];
            e.DrawEllipse(new Pen(Color.Black, 4f), c.x, c.y, this.w, this.h);
            e.DrawEllipse(new Pen(Color.Red, 2f), apple_x, apple_y, this.w, this.h);
        }
        public void DrawGrid(PaintEventArgs e, int xCells, int yCells)
        {
            e.Graphics.Clear(Color.Gray);//Фон
            using (Pen pen = new Pen(Color.Black, 2)){
                //Горизонтальные линии
                for (int i = 1; i <= xCells; i++)
                    e.Graphics.DrawLine(pen, i * w, 0, i * w, h * yCells);
                //Вертикальные линии
                for (int i = 1; i <= yCells; i++)
                    e.Graphics.DrawLine(pen, 0, i * h, w * xCells, i * h);
            }
        }
        
        private void Timer1_Tick_1(object sender, EventArgs e)
        {
            if (this.Rules())
            {
                this.MyUpdate();
                this.Invalidate();
            }
            else
            {
                this.timer1.Enabled = false;
                MessageBox.Show(message + '\n' + "You score: " + (snake.Count()*10));
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            keys[(int)e.KeyCode] = true;
        }

    }
}
