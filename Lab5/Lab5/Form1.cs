using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bug
{
    enum direction : byte { Up, Right, Down, Left };
    public partial class Form1 : Form
    {
        Cockroach workCockroach;//рабочий Таракан - активный Таракан, который будет выполнять алгоритм
        PictureBox workpb;//рабочее поле PictureBox - поле на которой будет рабочий Таракан
        List<Cockroach> LC;//Список для хранения созданных Тараканов
        List<PictureBox> PB;//Список для хранения созданных объектов PictureBox
        int AlgStep; //текущая комманда
        List<PictureBox> workfields;
        List<Cockroach> workbugs;  
        Cockroach cockroach;
        List<bool> newGroup;
        Random rnd = new Random();
        public Form1() 
        {
            InitializeComponent();
            LC = new List<Cockroach>();
            PB = new List<PictureBox>();
            workfields = new List<PictureBox>();
            workbugs = new List<Cockroach>();
            Random rnd = new Random();
        }
        public void RePaint(Cockroach c, PictureBox p)
        {
            p.Bounds = new Rectangle(c.X, c.Y, c.Image.Width, c.Image.Height);//создание новых границ изображения для PictureBox
            p.Image = c.Image;
        }
        public void Show(Cockroach c, PictureBox p, Panel owner)
        {
           
            //c.X = (owner.Width - c.Image.Width) / 2;
            //c.Y = (owner.Height - c.Image.Height) / 2;
           
            RePaint(c, p);
            owner.Controls.Add(p);// добавляем PictureBox к элементу Panel
        }
        private void IMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Form.ModifierKeys != Keys.Control)
                {
                    ClearWorkItems();
                }
                
                
                int k = PB.IndexOf(sender as PictureBox);//запоминаем номер нажатого компонента PictureBox
                workpb = sender as PictureBox;//объявляем его рабочим
                workCockroach = LC[k];//по найденному номеру находим Таракана в списке
                if(!workfields.Any())
                {
                    workbugs.Add(workCockroach);
                    workfields.Add(workpb);
                }
                else if(!workfields.Contains(workpb))
                {
                    
                    workbugs.Add(workCockroach);
                    workfields.Add(workpb);
                    


                }
            }
            else 
            if (e.Button == MouseButtons.Right) // смена образа на пкм
            {
                ClearWorkItems(); 
                int k = PB.IndexOf(sender as PictureBox);
                workpb = sender as PictureBox;
                if ((LC[k].Image.Tag).ToString() == "1")
                {
                    LC[k] = new Cockroach(new Bitmap("cockroach2.jpg"));
                    LC[k].Image.Tag = "2";
                    
                }
                else
                {
                    LC[k] = new Cockroach(new Bitmap("cockroach1.jpg"));
                    LC[k].Image.Tag = "1";
                }
                workfields.Add(workpb);
                workbugs.Add(LC[k]);
                RePaint(LC[k], PB[k]);
            }
        }
        private void IMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox picture = sender as PictureBox;
                picture.Tag = new Point(picture.Location.X+e.X+1, picture.Location.Y+e.Y+1);//запоминаем координаты мыши на момент начала перетаскивания
                picture.DoDragDrop(sender, DragDropEffects.Move);//начинаем перетаскивание ЧЕГО и с КАКИМ ЭФФЕКТОМ
            }
        }


        private void NewBtn_Click(object sender, EventArgs e)
        {
            
            int dx = rnd.Next(Field.Width);
            int dy = rnd.Next(Field.Height);
            cockroach = new Cockroach(new Bitmap("cockroach1.jpg"));
            cockroach.Image.Tag = "1";
            cockroach.newcoord(dx, dy);
            PictureBox p = new PictureBox();
            p.Location = new Point(dx, dy);
            Show(cockroach, p, Field);
            p.MouseMove += new MouseEventHandler(IMouseMove);
            p.MouseDown += new MouseEventHandler(IMouseDown);
            PB.Add(p);
            LC.Add(cockroach);
            workCockroach = cockroach;
            workpb = p;
            ClearWorkItems(); 
            workfields.Add(workpb);
            workbugs.Add(workCockroach);
        }

        private void Field_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Move;
            }

        }

        private void Field_DragDrop(object sender, DragEventArgs e)
        {
            if (Form.ModifierKeys == Keys.Control)
            {
                return;
            }
            //извлекаем PictureBox
            PictureBox picture = (PictureBox)e.Data.GetData(typeof(PictureBox));
            Panel panel = sender as Panel;
            //получаем клиентские координаты в момент отпускания кнопки
            Point pointDrop = panel.PointToClient(new Point(e.X, e.Y));  
            //извлекаем клиентские координаты мыши в момент начала перетаскивания
            Point pointDrag = (Point)picture.Tag;
            //вычисляем и устанавливаем Location для PictureBox в Panel
            picture.Location = new Point(pointDrop.X-pointDrag.X+picture.Location.X, pointDrop.Y - pointDrag.Y + picture.Location.Y);
            workbugs[0].X = picture.Location.X;
            workbugs[0].Y = picture.Location.Y;
            workfields[0].Location = picture.Location;
            picture.Parent = panel;
        }

        private void UpBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void RightBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void DownBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void LeftBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void StepBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void timerAlgorithm_Tick(object sender, EventArgs e)
        {
            if (AlgStep == Algorithm.Items.Count) //конец алгоритма
            {
                timerAlgorithm.Enabled = false; //выключение таймера
            }
            else//выполнение команды из списка
            {
                for (int i = 0; i < workbugs.Count; ++i)
                {
                    string s = (string)Algorithm.Items[AlgStep];
                    Algorithm.SetSelected(AlgStep, true);
                    if (s == "Step")
                        workbugs[i].Step();
                    if(s == "Up")
                        workbugs[i].ChangeTrend(s);
                    if (s == "Left")
                        workbugs[i].ChangeTrend(s);
                    if (s == "Right")
                        workbugs[i].ChangeTrend(s);
                    if (s == "Down")
                        workbugs[i].ChangeTrend(s);
                    RePaint(workbugs[i], workfields[i]);
                }
                AlgStep++;
            }
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            timerAlgorithm.Enabled = true;
            AlgStep = 0;
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Clear();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            while (workfields.Count > 0) 
            {
                workfields[0].Dispose(); 
                workfields.RemoveAt(0);
            }
            if (workpb!=null)
                workpb.Dispose();
            ClearWorkItems(); 
        }


        private void ClearWorkItems()
        {
            workbugs.Clear();
            workfields.Clear();
        }

    
    }
}

