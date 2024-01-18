﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace FinalProject_Omer_Chen
{
    [Serializable]
    class Bucket: ComponentsOnScreen
    {
       public Stack<Point> dispersal;
        protected int color; 
        public Bucket(Point start, int color)
        {
            this.start = start;
            this.color = color;
            this.dispersal = new Stack<Point>();
            count++;
            this.id_ = count;
        }
        public Bucket(Point start, int color,Bitmap bm)
        {
            this.start = start;
            this.color = color; 
            dispersal = new Stack<Point>();
            count++;
            this.id_ = count;
            this.initiat(bm); 
        }
        public int COLOR
        {
            get { return color; }
            set { color = value; }
        }
        private  void validate(Bitmap bm, Stack<Point> this_dis, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y); 
            if (cx == old_color) 
            {
                this_dis.Push(new Point(x, y)); 
                dispersal.Push(this_dis.Peek());
                bm.SetPixel(x, y, new_color); 
            }
        }
        public  void initiat(Bitmap bm)
        {

           
            Color old_color = bm.GetPixel(this.start.X, this.start.Y);
            if (old_color.ToArgb() == this.color) return;
            Stack<Point> this_dispersal = new Stack<Point>();
            this_dispersal.Push(new Point(this.start.X, this.start.Y)); 
            dispersal.Push(this_dispersal.Peek());
            bm.SetPixel(this.start.X, this.start.Y, Color.FromArgb(this.color));


            while (this_dispersal.Count > 0)
            {
                Point pt = (Point)this_dispersal.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, this_dispersal, pt.X - 1, pt.Y, old_color, Color.FromArgb(this.color));
                    validate(bm, this_dispersal, pt.X, pt.Y - 1, old_color, Color.FromArgb(this.color));
                    validate(bm, this_dispersal, pt.X + 1, pt.Y, old_color, Color.FromArgb(this.color));
                    validate(bm, this_dispersal, pt.X, pt.Y + 1, old_color, Color.FromArgb(this.color));
                }
            }
        }
        

        public override void Draw( Bitmap bm, Graphics g) 
            {
            
            Point[] arr = dispersal.ToArray();
              try
             {
            for (int i = 0; i < arr.Length; i++)
            {
               
                bm.SetPixel(arr[i].X,arr[i].Y, Color.FromArgb(this.color));
            }
            }
            catch(Exception e)
            {
            }
           
         
            }

        public override bool Is_Inside(int x, int y)
        {
            Point[] arr = dispersal.ToArray();
            for (int i = 0; i < arr.Length; i++)
                if (x == arr[i].X && y == arr[i].Y)
                    return true;
            return false;


        }
    }

    }

