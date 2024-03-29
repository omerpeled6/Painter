﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace FinalProject_Omer_Chen
{
    [Serializable]
    class Line:Shape 
    {
        public Line(int color,int size_pen, Point start,Point end)
        { this.color =color;this.size_pen = size_pen;
            this.start = start; this.end = end;
            count++;
            this.id_ = count;
        }
         public override void Draw( Bitmap bm, Graphics g)
         {

         g.DrawLine(new Pen(Color.FromArgb(color),size_pen), this.start.X,this.Start.Y, this.end.X, this.end.Y);

         }
       
        public override bool Is_Inside(int x, int y)
        {
            int Xmin, Xmax,Ymin,Ymax;
            Xmax = start.X > end.X ? start.X : end.X;
            Xmin = start.X < end.X ? start.X : end.X;
            Ymin= start.Y < end.Y ? start.Y : end.Y;
            Ymax = start.Y > end.Y ? start.Y : end.Y;
            if (x < Xmin || x > Xmax)
                return false;
            if (y < Ymin || y > Ymax)
                return false;
            if (Math.Abs(start.X - end.X) <= 10+size_pen )
                return true;
            if (Math.Abs(start.Y - end.Y) <= 10 + size_pen)
                return true;
            return Math.Abs((y - end.Y) + -1 * ((int)((((double)(end.Y - start.Y) / ((double)(end.X - start.X))) * ((double)(x - end.X)))))) <= 10 + size_pen; 
        }

    }

}

