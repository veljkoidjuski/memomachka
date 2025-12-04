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

        Random brojGen;
        public Form1()
        {
            InitializeComponent();

            potezUToku = false;
            parovi = 0;
            prethodni = -1;
            flagWait = false;

            slika = new Image[7];
            slikaDugme = new int[6];
            brojGen = new Random();
            otvorenaPolja = new bool[6];

            for (int i = 0; i < 6; i++) otvorenaPolja[i] = false;

            int pom1, pom2, pom3;
            bool[] iskoristene = { false, false, false, false, false, false };
            bool[] iskoristeneSlike = { false, false, false, false, false, false, false };
            
            slika[0] = Properties.Resources.m1;
            slika[1] = Properties.Resources.m2;
            slika[2] = Properties.Resources.m3;
            slika[3] = Properties.Resources.m4;
            slika[4] = Properties.Resources.m5;
            slika[5] = Properties.Resources.m6;
            slika[6] = Properties.Resources.m7;

            for (int i = 0; i < 3; i++)
            {
                do
                {
                    pom1 = brojGen.Next(1, 7);
                } while (iskoristene[pom1 - 1] == true);

                iskoristene[pom1 - 1] = true;

                do
                {
                    pom2 = brojGen.Next(1, 7);
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
                if (slikaDugme[trenutni - 1] == slikaDugme[prethodni - 1]) { if (++parovi == 3) MessageBox.Show("Игра је завршена :)"); }
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

                bool jeLiDugme = ((dugmeXOst > 50 && dugmeYOst > 50) && (dugmeX < 3 && dugmeY < 2) ? true : false);

                if (jeLiDugme)
                {
                    trenutni = 3 * (dugmeY) + dugmeX + 1;
                    if (trenutni != prethodni) klik();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics GFX = e.Graphics;

            for (int i = 0; i < 3; i++) for (int j = 0; j < 2; j++)
                {
                    if (!otvorenaPolja[3*j + i])
                    {
                        GFX.DrawImage(Properties.Resources.b, new Point((i * 150 + 50), (j * 150 + 50)));
                    }
                    else
                    {
                        GFX.DrawImage(slika[slikaDugme[3*j + i]], new Rectangle((i * 150 + 50), (j * 150 + 50), 100, 100));
                    }
                }
        }
    }


}
