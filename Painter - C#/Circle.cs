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
namespace FinalProject_Omer_Chen
{
    [Serializable]
    class Circle : BlockedShape
    {
        public Circle(int color, int size_pen, Point start, Point end, int width, int length)
        {
            this.color = color; this.size_pen = size_pen;
            this.start = start; this.end = end;
            this.width = width;
            this.Length = length;

            count++;
            this.id_ = count;
        }


   
        public override void Draw( Bitmap bm, Graphics g)
        {
           Pen p = new Pen(Color.FromArgb(COLOR), size_pen);
           g.DrawEllipse(p, this.Start.X, this.Start.Y, width, length);
        }

        public override bool Is_Inside(int x, int y)
        {

            int elipseCenter = (Start.X + end.X) / 2;
            int elipseCenterHeight = (Start.Y + end.Y) / 2;
            float sqaureX = (x - elipseCenter) * (x - elipseCenter);
            float sqaureY = (y - elipseCenterHeight) * (y - elipseCenterHeight);
            float a = width/2;
            float b = length/2;
            float sqaureA = a * a;
            float sqaureXB = b * b;
            if (Math.Abs((sqaureX / sqaureA) + (sqaureY / sqaureXB)) <=1)
                return true;
            else return false;
        }

    }
}
