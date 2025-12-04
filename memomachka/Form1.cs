using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace memomachka
{
    public partial class Form1 : Form
    {

        private Image[] slika;
        private Button[] dugmad;
        private int[] slikaDugme;

        private bool potezUToku;
        private int trenutni;
        private int prethodni;
        private int parovi;

        Random brojGen;

        // private Graphics GFX;
        public Form1()
        {
            InitializeComponent();
            // GFX = CreateGraphics();

            potezUToku = false;
            parovi = 0;
            prethodni = 1;

            slika = new Image[7];
            dugmad = new Button[6];
            slikaDugme = new int[6];
            brojGen = new Random();

            int pom1, pom2, pom3;
            bool[] iskoristene = { false, false, false, false, false, false };
            bool[] iskoristeneSlike = { false, false, false, false, false, false, false };

            dugmad[0] = button1;
            dugmad[1] = button2;
            dugmad[2] = button3;
            dugmad[3] = button4;
            dugmad[4] = button5;
            dugmad[5] = button6;

            // for(int i = 0; i<3; i++) for(int j = 0; j<3; j++)

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
            dugmad[trenutni - 1].BackgroundImage = slika[slikaDugme[trenutni - 1]];
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
                    for (int i = 0; i < 6; i++) dugmad[i].Enabled = false;
                    await Task.Delay(1000);
                    dugmad[trenutni - 1].BackgroundImage = null;
                    dugmad[prethodni - 1].BackgroundImage = null;
                    for (int i = 0; i < 6; i++) dugmad[i].Enabled = true;
                }

                potezUToku = false;
            }
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            int dugmeX = (MousePosition.X-Location.X) / 150;
            int dugmeXOst = (MousePosition.X-Location.X) % 150;

            int dugmeY = (MousePosition.Y-Location.Y) / 150;
            int dugmeYOst = (MousePosition.Y-Location.Y) % 150;

            Debug.WriteLine("{0} {1} {2} {3}", dugmeX, dugmeXOst, dugmeY, dugmeYOst);

            bool jeLiDugme = ((dugmeXOst > 50 && dugmeYOst > 50) && (dugmeX < 3 && dugmeY < 2) ? true : false);

            if (jeLiDugme)
            {
                trenutni = 3 * (dugmeY) + dugmeX + 1;
                klik();
            }
        }
    }


}
