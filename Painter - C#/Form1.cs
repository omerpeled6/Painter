using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace FinalProject_Omer_Chen
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
            this.Width = 1250;
            this.Height = 800;
            bm = new Bitmap(pic.Width, pic.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic.Image = bm;
            

        }
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        Pen p = new Pen(Color.Black, 4);
        int index;
        int x, y, sX, sY, cX, cY;
        ColorDialog cd_ = new ColorDialog();
        Color new_color;
        int pen_color=Color.Black.ToArgb(); int pen_size=4;
       
        List<Point> mypen_p = new List<Point>();

        ComponentsOnScreen loked;
        bool is_press = false;
        int  ore_x_s=0, ore_y_s=0, ore_x_e=0, ore_y_e=0;
        Point[] prr; 
        Point[] p_bucket;
        ComponentsList myelements = new ComponentsList();
        ComponentsList deleted = new ComponentsList();
        ComponentsList moved = new ComponentsList();
        Stack<Undo> the_undo_orgnized = new Stack<Undo>();
        Stack<Undo> the_redo_orgnized = new Stack<Undo>();
        ComponentsList moved_redo = new ComponentsList();
        ComponentsList deleted_redo = new ComponentsList();

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            Point[] brr;
          
            if (index == 9) 
            {
                if (is_press) 
                {
                    px = e.Location;
                    if (loked.GetType() == typeof(Pen1))
                    {
                        for (int j = 0; j < ((Pen1)loked).arr.Length; j++)
                        {
                            ((Pen1)loked).arr[j].X = prr[j].X + sX;
                            ((Pen1)loked).arr[j].Y = prr[j].Y + sY;
                        }

                    }
                    else if (loked.GetType() == typeof(Bucket))
                    {
                        brr = ((Bucket)loked).dispersal.ToArray();
                        ((Bucket)loked).dispersal.Clear();
                        for (int j = brr.Length - 1; j >= 0; j--)
                        {
                            brr[j].X = p_bucket[j].X + sX;
                            brr[j].Y = p_bucket[j].Y + sY;
                            ((Bucket)loked).dispersal.Push(brr[j]);
                        }


                    }
                    else
                    {
                        ((Shape)loked).start.X = ore_x_s + sX;
                        ((Shape)loked).start.Y = ore_y_s + sY;

                        ((Shape)loked).end.X = ore_x_e + sX;

                        ((Shape)loked).end.Y = ore_y_e + sY;
                    }
                    g.Clear(Color.White);
                    pic.Image = bm;

                    myelements.DrawAll(bm, g);
                    loked.Draw(bm, g);
                    py = px;
                }

            }
            if (paint)
            {
                    if (index == 2) 
                    {
                        
                       for(int i=myelements.NextIndex-1;i>=0;i--)
                        {
                            
                            if (myelements[i].Is_Inside(e.X, e.Y)) 
                            {
                            deleted.ADD(myelements[i]);
                            myelements.Remove(i);
                            g.Clear(Color.White); 
                            pic.Image = bm;
                            myelements.DrawAll(bm, g);
                            the_undo_orgnized.Push(new Undo(1,deleted[deleted.NextIndex-1].Id_)); 
                            break;
                            }
                        }
                    }
                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(p, px, py);
                    mypen_p.Add(px);
                    py = px;
                }
                }
                pic.Refresh();
                x = e.X;
                y = e.Y;
                sX = e.X - cX;
                sY = e.Y - cY;
            textBox1.Text = myelements.NextIndex.ToString();
        }
        

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
           
            Point[] arr = new Point[mypen_p.Count];
            Stack<ComponentsOnScreen> brr = new Stack<ComponentsOnScreen>();
            mypen_p.CopyTo(arr);
            paint = false;

            sX = x - cX;
            sY = y - cY;
            int sx_abs= Math.Abs(x - cX); int sy_abs= Math.Abs(y - cY);
            int minX = x > cX ? cX : x;
            int minY = y > cY ? cY : y;

            int maxX = x < cX ? cX : x;
            int maxY = y < cY ? cY : y;

            if(index==1)
            {
                
                myelements.ADD((new Pen1(pen_color, pen_size, new Point(cX, cY), new Point(x, y), mypen_p.Count, arr)));
                mypen_p.Clear(); 
                the_undo_orgnized.Push(new Undo(2, myelements[myelements.NextIndex-1].Id_)); ; 
            }

            if (index == 3)
            {
                myelements.ADD((new Circle(pen_color, pen_size, new Point(cX, cY), new Point(x, y), sX, sY)));

                myelements[myelements.NextIndex-1].Draw( bm, g);
                the_undo_orgnized.Push(new Undo(2, myelements[myelements.NextIndex - 1].Id_));
            }
            if (index == 4)
            {
                myelements.ADD ((new Rectangle1(pen_color, pen_size, new Point(minX, minY), new Point(maxX, maxY), sx_abs, sy_abs)));

                myelements[myelements.NextIndex - 1].Draw( bm, g);
                the_undo_orgnized.Push(new Undo(2, myelements[myelements.NextIndex - 1].Id_));
            }
            if (index == 5)
            {
                myelements.ADD((new Line(pen_color, pen_size, new Point(cX, cY), new Point(x, y))));

                myelements[myelements.NextIndex - 1].Draw( bm, g);
                the_undo_orgnized.Push(new Undo(2, myelements[myelements.NextIndex - 1].Id_));
            }
            if(index==9)
            {
                if (is_press)
                {

                    if(loked.GetType()==typeof(Pen1))
                    {
                        ((Pen1)loked).start = ((Pen1)loked).arr[0];
                        ((Pen1)loked).end = ((Pen1)loked).arr[((Pen1)loked).arr.Length-1];
                    }
                   
                   myelements.ADD(loked);
                    g.Clear(Color.White);
                    pic.Image = bm;
                    the_undo_orgnized.Push(new Undo(3, myelements[myelements.NextIndex - 1].Id_));// move
                  

                    loked = null;
                  
                    is_press = false;

                    myelements.DrawAll(bm, g);
                    

                }

                textBox1.Text = myelements.NextIndex.ToString();
            }
        }

        private void btn_pencil_Click(object sender, EventArgs e)
        {
            index = 1;
            
        }

        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void btn_rect_Click(object sender, EventArgs e)
        {
            index = 4;
        }

        private void btn_line_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        static Point set_point(PictureBox pb, Point pt) 
        {
            float pX = 1f * pb.Image.Width / pb.Width; 
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }
        
        private void color_picker_MouseClick(object sender, MouseEventArgs e)
        {
           // Point point = set_point(color_picker, e.Location);
           // pic_color.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
           // new_color = pic_color.BackColor;
           // p.Color = pic_color.BackColor;
          //  pen_color = p.Color.ToArgb();
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            sX = x - cX;
            sY = y - cY;
            int sx_abs = Math.Abs(x - cX); int sy_abs = Math.Abs(y - cY);
            int minX = x > cX ? cX : x;
            int minY = y > cY ? cY : y;

            int maxX = x < cX ? cX : x;
            int maxY = y < cY ? cY : y;
            if (paint)
            {
              

                if (index == 3)
                {
                    g.DrawEllipse(p, cX, cY, sX, sY);
                }
                if (index == 4)
                {
                    g.DrawRectangle(p, minX, minY, sx_abs, sy_abs);
                }
                if (index == 5)
                {
                    g.DrawLine(p, cX, cY, x, y);
                }
              
            }
        }
        private void btn_fill_Click(object sender, EventArgs e)
        {
            index = 7;
        }
        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            Stack<Shape> brr = new Stack<Shape>();
            ComponentsOnScreen[]drr;
            bool erase = false;
            Stack<ComponentsOnScreen> crr;

            if (index == 7)
            {
                    Point point = set_point(pic, e.Location);
                    Bucket b = new Bucket(point, p.Color.ToArgb(),bm);
                    myelements.ADD(b);
                the_undo_orgnized.Push(new Undo(2, myelements[myelements.NextIndex - 1].Id_));
            }
            if (index == 2)  
            {
                for (int i = myelements.NextIndex - 1; i >= 0; i--)
                {

                    if (myelements[i].Is_Inside(e.X, e.Y))
                    {
                        deleted.ADD(myelements[i]);
                        myelements.Remove(i);
                        g.Clear(Color.White); 
                        pic.Image = bm;
                        myelements.DrawAll(bm, g);
                        the_undo_orgnized.Push(new Undo(1, deleted[deleted.NextIndex - 1].Id_));
                        break;
                    }


                }
            }
            textBox1.Text = myelements.NextIndex.ToString();

        }
        private void btn_save_Click(object sender, EventArgs e)
        {
           
                
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog1.Filter = "paints files(*.PNT)| *.PNT | All files(*.*) | *.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    //!!!!
                    formatter.Serialize(stream,myelements);
                    MessageBox.Show("the filed has been saved");
                }
            }
        }
     

        private void btn_exist_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pic_Click(object sender, EventArgs e)
        {

        }

        private void form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_load_Click(object sender, EventArgs e)
        {
           List<Shape> b;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "paints files(*.PNT)| *.PNT | All files(*.*) | *.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //!!!!
               myelements = (ComponentsList)binaryFormatter.Deserialize(stream);

                myelements.DrawAll(bm, g);


            }


        }


        private void btn_redo_Click(object sender, EventArgs e)
        {
            Undo task = null;
            if (the_redo_orgnized.Count != 0)
                task = the_redo_orgnized.Pop();

            if(task!=null)
            {
                if(task.Mission==1)
                {
                    for (int i= deleted_redo.NextIndex - 1; i >= 0;i--)
                    {

                        if(deleted_redo[i].Id_==task.Id)
                        {
                           myelements.ADD(deleted_redo[i]);
                            the_undo_orgnized.Push(new Undo(2, deleted_redo[i].Id_));
                            deleted_redo.Remove(i);
                            break;
                        }

                    }
                    g.Clear(Color.White);
                    pic.Image = bm;
                    myelements.DrawAll(bm, g);

                }

                if (task.Mission==2)
                {
                    for (int i = myelements.NextIndex - 1; i >= 0; i--)
                    {
                        if (myelements[i].Id_ == task.Id)
                        {
                           
                            deleted.ADD(myelements[i]);
                            the_undo_orgnized.Push(new Undo(1, myelements[i].Id_));
                            myelements.Remove(i);
                            break;
                        }
                    }

                    g.Clear(Color.White);
                    pic.Image = bm;
                    myelements.DrawAll(bm, g);


                }


                if (task.Mission==3)
                {
                    for (int i = myelements.NextIndex - 1; i >= 0; i--)
                    {
                        if (myelements[i].Id_ == task.Id)
                        {
                            
                            moved.ADD(myelements[i]);
                            myelements.Remove(i);
                            break;
                        }
                    }
                    for (int i = moved_redo.NextIndex - 1; i >= 0; i--)
                    {
                        if (moved_redo[i].Id_ == task.Id)
                        {
                            the_undo_orgnized.Push(new Undo(3, moved_redo[i].Id_));
                            myelements.ADD(moved_redo[i]);
                            moved_redo.Remove(i);
                            break;
                        }
                    }
                    g.Clear(Color.White);
                    pic.Image = bm;
                    myelements.DrawAll(bm, g);




                }



            }



        }


        private void btn_color_Click(object sender, EventArgs e)
        {
            cd_.ShowDialog();
            new_color = cd_.Color;
            pic_color.BackColor = cd_.Color;
            p.Color = cd_.Color;
            pen_color = p.Color.ToArgb();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            p.Width = trackBar1.Value;
            pen_size = ((int)p.Width);
        }


        private void btn_undo_Click(object sender, EventArgs e)
        {
            Undo task = null;
            
            if (the_undo_orgnized.Count != 0)
                task = the_undo_orgnized.Pop();
            if (task != null)
            {
                if (task.Mission == 1)
                {
                    for(int i=deleted.NextIndex-1;i>=0;i--)
                    {
                        if(deleted[i].Id_==task.Id)
                        {
                            the_redo_orgnized.Push(new Undo(2, deleted[i].Id_));
                            myelements.ADD(deleted[i]);
                            deleted.Remove(i);
                            break;
                        }
                    }
                    g.Clear(Color.White);
                    pic.Image = bm;
                    myelements.DrawAll(bm, g);

                }
                else if (task.Mission == 2)
                {
                    for(int i=myelements.NextIndex-1;i>=0;i--)
                    {
                        if(myelements[i].Id_==task.Id)
                        {
                            the_redo_orgnized.Push(new Undo(1, myelements[i].Id_));//add
                            deleted_redo.ADD(myelements[i]);
                            myelements.Remove(i);
                            break;
                        }
                    }
                    g.Clear(Color.White);
                    pic.Image = bm;
                    myelements.DrawAll(bm, g);


                }

                else if (task.Mission == 3)
                {
                    for (int i = myelements.NextIndex - 1; i >= 0; i--)
                    {
                        if (myelements[i].Id_ == task.Id)
                        {
                            
                            moved_redo.ADD(myelements[i]);
                            myelements.Remove(i);
                            break;
                        }
                    }
                    for (int i= moved.NextIndex-1; i>=0;i--)
                    {
                        if(moved[i].Id_==task.Id)
                        {
                            the_redo_orgnized.Push(new Undo(3, moved[i].Id_));
                            myelements.ADD(moved[i]);
                            moved.Remove(i);
                            break;
                        }
                    }
                    g.Clear(Color.White);
                    pic.Image = bm;
                    myelements.DrawAll(bm, g);


                }
            }
            textBox1.Text = myelements.NextIndex.ToString();

        }
       

        private void btn_move_Click(object sender, EventArgs e)
        {
            index = 9;
        }

       

        private void btn_clear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pic.Image = bm;
            index = 0;
            myelements.remove_all();
            deleted.remove_all();
            moved_redo.remove_all();
            deleted_redo.remove_all();
            moved.remove_all(); 
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            Bucket b;
            ComponentsOnScreen temp;
            py = e.Location;
            cX = e.X;
            cY = e.Y;
            paint = true;
            if (index == 1)
            {
                mypen_p.Add(py); 
            }

          
            
            if (index == 9) 
            {
                

               for(int i=myelements.NextIndex-1;i>=0;i--)
                {
                    temp = myelements[i];
                    
                    if (temp.Is_Inside(e.X, e.Y))
                    {
                        is_press = true;
                        loked = temp;
                        myelements.Remove(i);
                       
                       
                        if (loked.GetType() == typeof(Pen1))
                        {
                            prr = new Point[((Pen1)loked).arr.Length];
                            for (int j = 0; j < ((Pen1)loked).arr.Length; j++)
                            {
                                prr[j].X = ((Pen1)loked).arr[j].X; 
                                prr[j].Y = ((Pen1)loked).arr[j].Y;
                            }
                            moved.ADD(new Pen1(((Pen1)(loked)).COLOR, ((Pen1)(loked)).Size_pen, ((Pen1)(loked)).Start, ((Pen1)(loked)).End, ((Pen1)(loked)).arr.Length, ((Pen1)(loked)).arr));
                            ((Pen1)(moved[moved.NextIndex - 1])).Id_ = loked.Id_;
                        }
                        if(loked.GetType()==typeof(Bucket))
                        {
                            
                            p_bucket = ((Bucket)loked).dispersal.ToArray();
                            b=new Bucket(loked.Start, ((Bucket)(loked)).COLOR);
                            for (int j = 0; j < p_bucket.Length; j++)
                               b.dispersal.Push(p_bucket[j]);
                           moved.ADD(b);
                            moved[moved.NextIndex - 1].Id_ = loked.Id_;
                           
                        }
                        else
                        {
                            ore_x_s = ((Shape)loked).Start.X; ore_y_s = ((Shape)loked).Start.Y; 
                            ore_x_e = ((Shape)loked).End.X; ore_y_e = ((Shape)loked).End.Y;  
                           if(loked.GetType()==typeof(Line))
                            {
                               moved.ADD(new Line(((Line)(loked)).COLOR, ((Line)(loked)).Size_pen, ((Line)(loked)).Start, ((Line)(loked)).End));
                                ((Line)(moved[moved.NextIndex - 1])).Id_ = loked.Id_;
                            }
                            if (loked.GetType() == typeof(Rectangle1))
                           {
                                moved.ADD(new Rectangle1(((Rectangle1)(loked)).COLOR, ((Rectangle1)(loked)).Size_pen, ((Rectangle1)(loked)).Start, ((Rectangle1)(loked)).End, ((Rectangle1)(loked)).Width, ((Rectangle1)(loked)).Length));
                                ((Rectangle1)(moved[moved.NextIndex - 1])).Id_ = loked.Id_;
                            }
                            if (loked.GetType() == typeof(Circle))
                            {
                                moved.ADD(new Circle(((Circle)(loked)).COLOR, ((Circle)(loked)).Size_pen, ((Circle)(loked)).Start, ((Circle)(loked)).End, ((Circle)(loked)).Width, ((Circle)(loked)).Length));
                                ((Circle)(moved[moved.NextIndex - 1])).Id_ = loked.Id_;
                            }
                        }

                        break;
                    }
                    
                }
               
                
            }
            textBox1.Text = myelements.NextIndex.ToString();
        }
            private void btn_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        
       
    }
}
