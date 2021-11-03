using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Field_of_dots
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        const int N = 10;
        Button[,] map1 = new Button[N, N];
        Button[,] map2 = new Button[N, N];
        int[,] shots = new int[100, 3];      // поля для выстрела компьютера
        int MyLuck = 0;
        int NotMyLuck = 0;
        int col2 = 0;       //координаты для выстрела компьютера
        int col3 = 0;
        int col4 = 0;

        void Delet(Button[,] map)  // очистка поля
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    map[i, j].BackColor = Color.White;
        }
        // функция расставляет корабли на заданных координатах и
        //заданной длинны, возвращает false если нельзя расположить корабль
        bool Set(Button[,] map1, int x, int y, int len, int ori)
        {
            //проверяем хватит ли места расположить корабль
            if (ori == 0)     // вертикальное расположение корабля
            {
                if (y + len >= 11)
                    return false;
                for (int i = 0; i < len; i++)
                    if (map1[x, y + i].BackColor == Color.Green || map1[x, y + i].BackColor == Color.Aqua)
                        return false;
                for (int i = 0; i < len; i++)
                {
                    map1[x, y].BackColor = Color.Aqua;
                    if (x != 0)
                        if (map1[x - 1, y].BackColor != Color.Aqua)
                            map1[x - 1, y].BackColor = Color.Green;
                    if (x != 9)
                        if (map1[x + 1, y].BackColor != Color.Aqua)
                            map1[x + 1, y].BackColor = Color.Green;
                    if (y != 0)
                        if (map1[x, y - 1].BackColor != Color.Aqua)
                            map1[x, y - 1].BackColor = Color.Green;
                    if (y != 9)
                        if (map1[x, y + 1].BackColor != Color.Aqua)
                            map1[x, y + 1].BackColor = Color.Green;
                    // закрашиваем по диоганалям
                    if (x != 0 && y != 0)
                        map1[x - 1, y - 1].BackColor = Color.Green;
                    if (x != 9 && y != 0)
                        map1[x + 1, y - 1].BackColor = Color.Green;
                    if (x != 0 && y != 9)
                        map1[x - 1, y + 1].BackColor = Color.Green;
                    if (x != 9 && y != 9)
                        map1[x + 1, y + 1].BackColor = Color.Green;
                    y++;
                }
                return true;
            }
            if (ori == 1)     // горизонтальное расположение корабля
            {
                if (x + len > 10)
                    return false;
                for (int i = 0; i < len; i++)
                    if (map1[x + i, y].BackColor == Color.Green || map1[x + i, y].BackColor == Color.Aqua)
                        return false;
                for (int i = 0; i < len; i++)
                {
                    map1[x, y].BackColor = Color.Aqua;
                    if (x != 0)
                        if (map1[x - 1, y].BackColor != Color.Aqua)
                            map1[x - 1, y].BackColor = Color.Green;
                    if (x != 9)
                        if (map1[x + 1, y].BackColor != Color.Aqua)
                            map1[x + 1, y].BackColor = Color.Green;
                    if (y != 0)
                        if (map1[x, y - 1].BackColor != Color.Aqua)
                            map1[x, y - 1].BackColor = Color.Green;
                    if (y != 9)
                        if (map1[x, y + 1].BackColor != Color.Aqua)
                            map1[x, y + 1].BackColor = Color.Green;
                    // закрашиваем по диоганалям
                    if (x != 0 && y != 0)
                        map1[x - 1, y - 1].BackColor = Color.Green;
                    if (x != 9 && y != 0)
                        map1[x + 1, y - 1].BackColor = Color.Green;
                    if (x != 0 && y != 9)
                        map1[x - 1, y + 1].BackColor = Color.Green;
                    if (x != 9 && y != 9)
                        map1[x + 1, y + 1].BackColor = Color.Green;
                    x++;
                }
                return true;
            }
            return true;
        }
        // в tag расположена строка, первые два символа-координаты кнопки, третий- есть корабль или нет        
        private void Form1_Load(object sender, EventArgs e)
        {
            //  создаем поля 
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    map1[i, j] = new Button();
                    map1[i, j].Width = map1[i, j].Height = 20;
                    map1[i, j].Location = new Point(100 + map1[i, j].Width * i, 80 + map1[i, j].Height * j);
                    map1[i, j].Tag = i + j.ToString();
                    Controls.Add(map1[i, j]);
                }
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    map2[i, j] = new Button();
                    map2[i, j].Width = map2[i, j].Height = 20;
                    map2[i, j].Location = new Point(350 + map2[i, j].Width * i, 80 + map2[i, j].Height * j);
                    map2[i, j].Tag = i + j.ToString();
                    Controls.Add(map2[i, j]);
                }
            //заполним поле для выстрела компьютера
            for (int i = 0; i <4;i++)
            {
                shots[i, 0] = i;
                shots[i, 1] = 3-i;
                shots[i, 2] = 1;
                shots[20+i, 0] = 6+i;
                shots[20+i, 1] = 9 - i;
                shots[20 + i, 2] = 1;
            }
            for (int i = 0; i < 8; i++)
            {
                shots[4+i, 0] = i;
                shots[4+i, 1] = 7 - i;
                shots[4 + i, 2] = 1;
                shots[12 + i, 0] = 2 + i;
                shots[12 + i, 1] = 9 - i;
                shots[12 + i, 2] = 1;
            }
            for(int i=0; i< 2; i++)
            {
                shots[24 + i, 0] = i;
                shots[24 + i, 1] = 1 - i;
                shots[24 + i, 2] = 1 ;
                shots[48 + i, 0] = 8 + i;
                shots[48 + i, 1] = 9 - i;
                shots[48 + i, 2] = 1;
            }
            for (int i = 0; i < 6; i++)
            {
                shots[26 + i, 0] = i;
                shots[26 + i, 1] = 5 - i;
                shots[26 + i, 2] = 1;
                shots[42 + i, 0] = 4 + i;
                shots[42 + i, 1] = 9 - i;
                shots[42 + i, 2] = 1;
            }
            for (int i = 0; i < 10; i++)
            {
                shots[32 + i, 0] = i;
                shots[32 + i, 1] = 9 - i;
                shots[32 + i, 2] = 1;
            }
            for (int i = 0; i < 10; i++)
            {
                shots[32 + i, 0] = i;
                shots[32 + i, 1] = 9 - i;
                shots[32 + i, 2] = 1;
            }
            for (int i=0; i<10; i++)
                for (int j=i%2; j<10; j+=2)
                {                    
                    shots[50+i*5+j/2, 0] = i;
                    shots[50 + i * 5 + j / 2, 1] = j;
                    shots[50 + i * 5 + j / 2, 2] = 1;
                    //map1[shots[50 + i * 5 + j / 2, 0], shots[50 + i * 5 + j / 2, 1]].BackColor = Color.YellowGreen;
                }
        }
        // авторасстановка кораблей 
        private void Button1_Click(object sender, EventArgs e)
        {
            Delet(map1);
            int a = 1;
            Random rand = new Random();
            for (int i = 4; i > 0; i--)
            {
                for (int j = 0; j < a; j++)
                {
                    bool bl = false;
                    while (bl == false)
                    {
                        int ori = rand.Next(2);
                        int x = rand.Next(10);
                        int y = rand.Next(10);
                        bl = Set(map1, x, y, i, ori);
                        if (bl == true)         // зАписываем в Tag данные о корабле
                        {
                            for (int num = 0; num < i; num++)
                            {
                                if (ori == 0)
                                {
                                    map1[x, y + num].Tag = x.ToString() + (y + num).ToString() + '1' + ori.ToString() + i.ToString() + num.ToString();
                                }
                                if (ori == 1)
                                {
                                    map1[x + num, y].Tag = (x + num).ToString() + y.ToString() + '1' + ori.ToString() + i.ToString() + num.ToString();
                                }
                            }
                        }
                    }
                }
                a++;
            }
        }
        // кнопка сброса
        private void Button3_Click(object sender, EventArgs e)
        {
            Delet(map1);
        }

        // Выстрел компьютера 
        public void CompShot()
        {
            Random rand = new Random();
            if (col2 < 25)
            {
                col3 = 25;
            }
            if (col2 < 50 && col2>=25)
            {
                col3 = 50;
                col4 = 25;
            }
            if (col2>=50)
            {
                col3 = 100;
                col4 = 50;
            }
            int col = rand.Next(col4,col3);            
                while (map1[shots[col, 0], shots[col, 1]].BackColor == Color.Black ||
                    map1[shots[col, 0], shots[col, 1]].BackColor == Color.Red || shots[col, 2] == 0)
                {
                if (shots[col, 2] == 1)
                {
                    shots[col, 2] = 0;
                    col2++;
                }
                col = rand.Next(col4,col3);                    
                }
                if (map1[shots[col, 0], shots[col, 1]].BackColor == Color.White)
                {
                    map1[shots[col, 0], shots[col, 1]].BackColor = Color.Black;
                    if (shots[col, 2] == 1)
                    {
                        shots[col, 2] = 0;
                        col2++;
                    }
                    return;
                }
                if (map1[shots[col, 0], shots[col, 1]].BackColor == Color.Aqua)
                {
                    map1[shots[col, 0], shots[col, 1]].BackColor = Color.Red;
                    if (shots[col, 2] == 1)
                    {
                        shots[col, 2] = 0;
                        col2++;
                    }
                    string tag = map1[shots[col, 0], shots[col, 1]].Tag.ToString();
                    int ori = tag[3] - '0';
                    int len = tag[4] - '0';
                    int num = tag[5] - '0';
                    if (Check(map1, shots[col, 0], shots[col, 1], ori, len, num))
                        NotMyLuck++;
                    if (NotMyLuck == 10)
                    {
                        label5.Visible = true;
                        return;
                    }
                    CompShot();
                    return;
                }
        }
        public void Shot(object sender, EventArgs e)        //обработчик события выстрела
        {
            if (((Button)sender).BackColor == Color.Black || ((Button)sender).BackColor == Color.Red)
                return;
            string tag = ((Button)sender).Tag.ToString();
            if (tag[2] == '0')
            {
                ((Button)sender).BackColor = Color.Black;
                // если промах, стреляет компьютер                
                CompShot();
            }
            if (tag[2] == '1')
            {
                ((Button)sender).BackColor = Color.Red;
                //проверяем убит ли корабль
                int x=tag[0] - '0';
                int y = tag[1] - '0';
                int ori= tag[3] - '0';
                int len = tag[4] - '0';
                int num = tag[5] - '0';

                if (Check(map2, x, y, ori, len, num))
                    MyLuck++;
                if (MyLuck == 10)
                {
                    label4.Visible = true;
                    return;
                }
            }
        }        
        //проверка убит ли корабль (закрашивает поля вокруг)
        public bool Check (Button[,] map2, int x, int y, int ori, int len, int num)
        {
            bool kill = true;
            if (ori == 0)
            {
                for (int i = 0; i < len; i++)
                {
                    if (map2[x, y - num + i].BackColor != Color.Red)
                    {
                        kill = false;
                        break ;
                    }
                }
            }
            if (ori == 1)
            {
                for (int i = 0; i < len; i++)
                {
                    if (map2[x - num + i, y].BackColor != Color.Red)
                    {
                        kill = false;
                        break ;
                    }
                }
            }
            if (kill == true)     // закрашиваем поля вокруг корабля
            {
                if (ori == 0)
                {
                    if (x != 0)
                    {
                        for (int i = 0; i < len; i++)
                            map2[x - 1, y - num + i].BackColor = Color.Black;
                        if (y - num != 0)
                            map2[x - 1, y - num - 1].BackColor = Color.Black;
                        if (y - num + len - 1 != 9)
                            map2[x - 1, y - num + len].BackColor = Color.Black;
                    }
                    if (x != 9)
                    {
                        for (int i = 0; i < len; i++)
                            map2[x + 1, y - num + i].BackColor = Color.Black;
                        if (y - num != 0)
                            map2[x + 1, y - num - 1].BackColor = Color.Black;
                        if (y - num + len - 1 != 9)
                            map2[x + 1, y - num + len].BackColor = Color.Black;
                    }
                    if (y - num != 0)
                        map2[x, y - num - 1].BackColor = Color.Black;
                    if (y - num + len - 1 != 9)
                        map2[x, y - num + len].BackColor = Color.Black;
                }
                if (ori == 1)
                {
                    if (x - num != 0)
                    {
                        map2[x - num - 1, y].BackColor = Color.Black;
                        if (y != 0)
                            map2[x - num - 1, y - 1].BackColor = Color.Black;
                        if (y != 9)
                            map2[x - num - 1, y + 1].BackColor = Color.Black;
                    }
                    if (y != 0)
                        for (int i = 0; i < len; i++)
                            map2[x - num + i, y - 1].BackColor = Color.Black;
                    if (y != 9)
                        for (int i = 0; i < len; i++)
                            map2[x - num + i, y + 1].BackColor = Color.Black;
                    if (x - num + len - 1 != 9)
                    {
                        map2[x - num + len, y].BackColor = Color.Black;
                        if (y != 0)
                            map2[x - num + len, y - 1].BackColor = Color.Black;
                        if (y != 9)
                            map2[x - num + len, y + 1].BackColor = Color.Black;
                    }
                }
            }
            return kill;
        }
        // начало игры
        private void Button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Игра началась!";
            richTextBox1.Text = "Красный - Ранен/убит" + "\n" + "Черный - Мимо" + "\n" +
                "Если корабль убит, клетки вокруг становятся черными";
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    if (map1[i, j].BackColor != Color.Aqua)
                    {
                        map1[i, j].Tag = i.ToString() + j.ToString()+0.ToString();
                        map1[i, j].BackColor = Color.White;
                    }
                }
            //рандомно заполняем поле противника

            //  Tag[0]= x
            //  Tag[1]= y
            //  Tag[2]= 0, если нет корабля/ 1 -если есть корабль
            //  Tag[3]= ori - направление корабля если 0 - гориз., 1- вертикально
            //  Tag[4]= len - длинна корабля
            //  Tag[5]= num - номер палубы корабля
            int a = 1;
            Random rand = new Random();
            for (int i = 4; i > 0; i--)
            {
                for (int j = 0; j < a; j++)
                {
                    bool bl = false;
                    while (bl == false)
                    {
                        int ori = rand.Next(2);
                        int x = rand.Next(10);
                        int y = rand.Next(10);
                        bl = Set(map2, x, y, i, ori);
                        if (bl== true)         // зАписываем в Tag данные о корабле
                        {
                            for (int num=0; num<i; num++)
                            {
                                if (ori == 0)
                                {
                                    map2[x, y + num].Tag =x.ToString()+ (y + num).ToString()+ '1' + ori.ToString() + i.ToString() + num.ToString();
                                }
                                if (ori == 1)
                                {
                                    map2[x + num, y ].Tag = (x + num).ToString() + y.ToString()+'1' + ori.ToString() + i.ToString() + num.ToString();
                                }
                            }
                        }
                    }
                }
                a++;
            }
            for (int i = 0; i < N; i++)     
                for (int j = 0; j < N; j++)
                {
                    if (map2[i, j].BackColor != Color.Aqua)
                        map2[i, j].Tag += 0.ToString();
                    map2[i, j].BackColor = Color.White;
                }
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    map2[i, j].Click += Shot;
                }
            
        }
    }
}
