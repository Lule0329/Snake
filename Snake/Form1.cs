using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        const int SIZE_SNAKE = 32;
        const int SIZE_FOOD = 16;

        List<PictureBox> tailpieces = new List<PictureBox>(); // Håller koll på svansbitarna
        Random rng = new Random();
        int i = 0;
        string direction = "Right"; // Håller koll på vilken riktning ormen pekar
        int score; // Antal poäng

        public Form1()
        {
            InitializeComponent();
        }

        bool IsIntersectingWith()
        {
            for (int i = 0; i < tailpieces.Count; i++)
            {
                // Om den nuvarande delen av svansen krockar med ormens huvud
                if (tailpieces[i].Bounds.IntersectsWith(snake.Bounds))
                {
                    // Sätt funktionens värde till true och avsluta funktionen
                    return true;
                }
            }

            // sätter funktionens värde till false om ingen av delarna kolliderar
            return false;
        }

        void addTail()
        {
            // Skapa en ny pictureBox
            PictureBox box = new PictureBox();
            // Sätt alla egenskapers som behövs 
            box.Location = snake.Location;
            box.Size = snake.Size;
            box.BackColor = snake.BackColor;

            // Lägg in lådan i controls så att fönstret visar den 
            Controls.Add(box);

            // Lägg till lådan i tailPieces så vi kan manipulera dem
            tailpieces.Add(box);
        }

        void moveTail()
        {
            if (tailpieces.Count > 0)
            {
                tailpieces[i].Location = snake.Location;

                i = i + 1;

                if (i >= tailpieces.Count)
                {
                    i = 0;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(SIZE_SNAKE * 20, SIZE_SNAKE * 15);
            snake.Size = new Size(SIZE_SNAKE, SIZE_SNAKE);
            food.Size = new Size(SIZE_FOOD, SIZE_FOOD);

            // Gör så att mat inte kan spawna inuti spelaren
            do
            {
                food.Location = new Point(rng.Next(0, ClientSize.Width - SIZE_FOOD), rng.Next(0, ClientSize.Height - SIZE_FOOD));
            } while (food.Location == snake.Location);
            
            snake.Location = new Point(0, 0);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kollar vart spelaren vill och hindrar spelaren från att åka baklänges
            if (e.KeyChar == 'a' && direction != "right")
            {
                direction = "left";
            }
            else if (e.KeyChar == 'd' && direction != "left")
            {
                direction = "right";
            }
            else if (e.KeyChar == 'w' && direction != "down")
            {
                direction = "up";
            }
            else if (e.KeyChar == 's' && direction != "up")
            {
                direction = "down";
            }
            else if (e.KeyChar == ' ' && !timer1.Enabled)
            {
                // Startar om spelet om du trycker på space efter att ha förlorat
                Application.Restart();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            moveTail();

            if (direction == "left")
            {
                snake.Left -= SIZE_SNAKE;
            }
            else if (direction == "right")
            {
                snake.Left += SIZE_SNAKE;
            }
            else if (direction == "up")
            {
                snake.Top -= SIZE_SNAKE;
            }
            else if (direction == "down")
            {
                snake.Top += SIZE_SNAKE;
            }

            if (IsIntersectingWith() == true)
            {
                timer1.Stop();
                MessageBox.Show("You Lost!");
                label2.Visible = true;
            }

            if (snake.Bounds.IntersectsWith(food.Bounds))
            {
                food.Location = new Point(rng.Next(0, ClientSize.Width - SIZE_FOOD), rng.Next(0, ClientSize.Height - SIZE_FOOD));
                addTail();
                score += 1;
                label1.Text = "Score: " + score;
            }
            
            // ändrar hastighet beroende på hur många poäng man har
            if (score >= 10)
            {
                timer1.Interval = 85;
            }
            else if (score >= 20)
            {
                timer1.Interval = 70;
            }
            else if (score >= 30)
            {
                timer1.Interval = 50;
            }
            else if (score >= 40)
            {
                timer1.Interval = 25;
            }

            if (snake.Left < 0)
            {
                snake.Left = ClientSize.Width - snake.Width;
            }
            else if (snake.Left > ClientSize.Width - snake.Width)
            {
                snake.Left = 0;
            }
                

            if (snake.Top < 0)
            {
                snake.Top = ClientSize.Height - snake.Height;
            }   
            else if (snake.Top > ClientSize.Height - snake.Height)
            {
                snake.Top = 0;
            }
        }
    }
}
