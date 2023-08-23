using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Threading;


namespace Snake_Game
{
    public partial class Form2 : Form
    {
        int x = 40;
        int y = 40;
        int i = 0;
        int j = 0;
        int d_x = 40;
        int d_y = 0;
        int score = -1;
        int pb_size = 600;
        Point cord_head = new Point(0, 0);
        
        PictureBox pb_bonus1 = new PictureBox();
        PictureBox[] snake = new PictureBox[225];
        int k = -1;
        
        int d = 0;
        Point[] point = new Point[225];
        int score_save = 0;
        SoundPlayer sp = new SoundPlayer("sample-15s.wav");
        SoundPlayer app = new SoundPlayer("appleeat2.wav");

        public Form2()
        {

         
            InitializeComponent();
            pictureBox1.Size = new Size(pb_size, pb_size);
            pictureBox2.Size = new Size(x, y);
            pictureBox2.BackColor = Color.Green;
            pictureBox2.Location = new Point(i, j); 
            pictureBox2.Parent = pictureBox1;
            pb_bonus1.Size = new Size(pictureBox2.Width, pictureBox2.Height);
            pb_bonus1.Image = Properties.Resources.apple2;
            pb_bonus1.SizeMode = PictureBoxSizeMode.StretchImage;
            timer1.Start();
            Bonus();
            ReadSave();
            timer2.Start();
            sp.Play();
            

        }    

        public void Movement()
        {
           
          // point[0] = new Point(pictureBox2.Location.X, pictureBox2.Location.Y);
          cord_head = new Point (pictureBox2.Location.X, pictureBox2.Location.Y);
            pictureBox2.Location = new Point(pictureBox2.Location.X + d_x, pictureBox2.Location.Y + d_y);
            
        }


        public void SnakeMovement(int k)
        {
            for(int i =0; i<=k; i++)
            {
                
                point[i + 1] = snake[i].Location;
                snake[i].Location = point[i];
               
            }


        }

        public void SnakeMovement_Dop(int k)
        {
            if (d < 0) d = k;
            snake[d].Location = cord_head;
            d--;

           

        }

            private void Press(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.P) timer1.Stop();
            
            if (e.KeyCode == Keys.S && !(d_x == 0) && !(d_y == -y)) { d_x = 0; d_y = y; } ;
            if (e.KeyCode == Keys.W && !(d_x == 0) && !(d_y == y)) { d_x = 0; d_y = -y; }; 
            if (e.KeyCode == Keys.A && !(d_x == x) && !(d_y == 0)) { d_x = -x; d_y = 0; }; 
            if (e.KeyCode == Keys.D && !(d_x == -x) && !(d_y == 0))  { d_x = x; d_y = 0; };
            if (e.KeyCode == Keys.Enter) timer1.Start();
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                Form f1 = new Form1();
                f1.Show();
                sp.Stop();
            }
                
        }

        public void Border()
        {
            if (pictureBox2.Left > pictureBox1.Width - pictureBox2.Width || pictureBox2.Top > pictureBox1.Height - pictureBox2.Height || pictureBox2.Top < 0 || pictureBox2.Left < 0)
            {

                timer1.Stop();
                MessageBox.Show("Вы врезались в стену \nНажмите Enter чтобы начать сначала");
                pictureBox2.Location = new Point(i, j);
                score = 0;
                d_x = x;
                d_y = 0;
                for (int j = 0; j <= k; j++)
                {
                    snake[j].Dispose();


                }
                k = -1;
                label1.Text = "Очки: 0" ;
                
            }
            Save();
            ReadSave();


        }

        

        
        public void Bonus()
        {
            score += 1;
            int m;
            int i_b, j_b;
            Random rn = new Random();

            do
            {
                i_b = rn.Next(0, pb_size/x);
                j_b = rn.Next(0, pb_size/y);
                m = 0;
                for (int j = 0; j <= k; j++)
                {
                    if (snake[j].Left / x == i_b && snake[j].Top / y == j_b)
                    {
                        m++;
                    }
                }

            } while (m > 0);



            app.Play();
           // System.Threading.Thread.Sleep(1000);
           // sp.Play();

            pb_bonus1.Location = new Point(i_b * x, j_b * y);
            label1.Text =  "Очки: " + score;
            
        }

        public void Crush()
        {
            for (int i = 0; i <= k; i++)
            {
                if(pictureBox2.Location == snake[i].Location)
                {
                    timer1.Stop();
                    MessageBox.Show("Вы врезались в себя ");
                    pictureBox2.Location = new Point(0, 0);
                    score = 0;
                    d_x = x;
                    d_y = 0;
                    for (int j = 0; j <= k; j++)
                    {
                        snake[j].Dispose();
                      
                        
                    }
                    k = -1;
                    label1.Text = "Очки: 0";
                }
                Save();
                ReadSave();

            }

        }


        public void CreateSnakes(int k)
        {
            Random zv = new Random();
            snake[k] = new PictureBox();
            snake[k].Size = new Size(pictureBox2.Width, pictureBox2.Height);
            //  snake[k].BackColor = Color.Green;
            snake[k].BackColor = Color.FromArgb(zv.Next(0,255), zv.Next(0, 255), zv.Next(0, 255));
            
            snake[k].Location = cord_head;
            snake[k].Parent = pictureBox1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            pb_bonus1.Parent = pictureBox1;
            Movement();
            Border();
            if (score > 0) SnakeMovement_Dop(k);
            if (pictureBox2.Top == pb_bonus1.Top && pictureBox2.Left == pb_bonus1.Left) 
            { 
                Bonus();
                k++;
                if (k >= 0) CreateSnakes(k);
                
            }
            
            

            Crush();
            
        }

        public void ReadSave()
        {
            StreamReader reader = new StreamReader("Save.txt");
            score_save = Convert.ToInt32(reader.ReadToEnd());
            reader.Close();
            label2.Text = "Рекорд: " + score_save;
        }


        public void Save()
        {


            if (score > score_save)
            {
                StreamWriter wruter = new StreamWriter("Save.txt");
                wruter.Write(score);
                wruter.Close();
            }
                
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            SoundPlayer music = new SoundPlayer("sample-15s.wav");
            music.Play();
        }

        
    }
}
