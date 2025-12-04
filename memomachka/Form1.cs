using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace memomachka
{
    public partial class Form1 : Form
    {

        private Image[] slika;
        private int[] slikaDugme;
        private bool[] otvorenaPolja;

        private bool potezUToku;
        private int trenutni;
        private int prethodni;
        private int parovi;
        private bool flagWait;

        private const int SIRINA = 4;
        private const int VISINA = 3;
        private const int PAROVI = (SIRINA * VISINA) / 2;

        Random brojGen;
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Width = SIRINA * 150 + 50;
            Height = (VISINA + 1) * 150;

            potezUToku = false;
            parovi = 0;
            prethodni = -1;
            flagWait = false;

            slika = new Image[7];
            slikaDugme = new int[SIRINA * VISINA];
            brojGen = new Random();
            otvorenaPolja = new bool[SIRINA * VISINA];

            for (int i = 0; i < SIRINA * VISINA; i++) otvorenaPolja[i] = false;

            int pom1, pom2, pom3;
            bool[] iskoristene = new bool[SIRINA * VISINA];
            for (int i = 0; i < SIRINA * VISINA; i++) iskoristene[i] = false;
            bool[] iskoristeneSlike = { false, false, false, false, false, false, false };

            slika[0] = Properties.Resources.m1;
            slika[1] = Properties.Resources.m2;
            slika[2] = Properties.Resources.m3;
            slika[3] = Properties.Resources.m4;
            slika[4] = Properties.Resources.m5;
            slika[5] = Properties.Resources.m6;
            slika[6] = Properties.Resources.m7;

            for (int i = 0; i < SIRINA * VISINA / 2; i++)
            {
                do
                {
                    pom1 = brojGen.Next(1, SIRINA * VISINA + 1);
                } while (iskoristene[pom1 - 1] == true);

                iskoristene[pom1 - 1] = true;

                do
                {
                    pom2 = brojGen.Next(1, SIRINA * VISINA + 1);
                } while (iskoristene[pom2 - 1] == true);

                iskoristene[pom2 - 1] = true;

                do
                {
                    pom3 = brojGen.Next(1, 8);
                } while (iskoristeneSlike[pom3 - 1] == true);

                iskoristeneSlike[pom3 - 1] = true;

                slikaDugme[pom1 - 1] = pom3 - 1;
                slikaDugme[pom2 - 1] = pom3 - 1;
            }

            button2.Location = new Point(150 * SIRINA - 30, 150 * VISINA + 95);

        }

        async void klik()
        {
            otvorenaPolja[trenutni - 1] = true;
            Refresh();
            if (!potezUToku)
            {
                potezUToku = true;
                prethodni = trenutni;
            }
            else
            {
                if (slikaDugme[trenutni - 1] == slikaDugme[prethodni - 1]) { if (++parovi == PAROVI) MessageBox.Show("Игра је завршена :)"); }
                else
                {
                    flagWait = true;
                    await Task.Delay(1000);
                    otvorenaPolja[trenutni - 1] = false;
                    otvorenaPolja[prethodni - 1] = false;
                    Refresh();
                    flagWait = false;
                }

                potezUToku = false;
            }

        }
        private void Form1_Click(object sender, EventArgs e)
        {
            if (!flagWait)
            {
                int dugmeX = (MousePosition.X - Location.X) / 150;
                int dugmeXOst = (MousePosition.X - Location.X) % 150;

                int dugmeY = (MousePosition.Y - Location.Y) / 150;
                int dugmeYOst = (MousePosition.Y - Location.Y) % 150;

                bool jeLiDugme = ((dugmeXOst > 50 && dugmeYOst > 50) ? true : false);

                if (jeLiDugme)
                {
                    trenutni = SIRINA * (dugmeY) + dugmeX + 1;
                    if (trenutni != prethodni) klik();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics GFX = e.Graphics;

            for (int i = 0; i < SIRINA; i++) for (int j = 0; j < VISINA; j++)
                {
                    if (!otvorenaPolja[SIRINA * j + i])
                    {
                        GFX.DrawImage(Properties.Resources.b, new Rectangle((i * 150 + 50), (j * 150 + 50), 100, 100));
                    }
                    else
                    {
                        GFX.DrawImage(slika[slikaDugme[SIRINA * j + i]], new Rectangle((i * 150 + 50), (j * 150 + 50), 100, 100));
                    }

                    GFX.DrawImage(Image.FromStream(new MemoryStream(Properties.Resources.tb2)), new Rectangle(0, (VISINA * 150 + 50), Width, 150));
                    GFX.DrawImage(Properties.Resources.t, new Rectangle(10, (VISINA * 150 + 50), 330, 120));
                    //GFX.DrawImage(Properties.Resources.maca, new Point((SIRINA * 150 - 50), (VISINA * 150 + 50)));
                }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Point tacka = new Point();
            int dx = MousePosition.X - Location.X;
            int dy = MousePosition.Y - Location.Y;
            while ((Control.MouseButtons & MouseButtons.Left) != 0)
            {
                Application.DoEvents();
                tacka.X = MousePosition.X - dy;
                tacka.Y = MousePosition.Y - dx;
                Location = tacka;
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources.close2;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources.close1;
        }
    }


}
