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
    class Rectangle1 : BlockedShape
    {
        public Rectangle1(int color, int size_pen, Point start, Point end, int width, int length)
        {
            this.COLOR = color; this.Size_pen = size_pen;
            this.Start = start; this.End = end;
            this.width = width;
            this.length = length;
            count++;
            this.id_ = count;
        }


        public override void Draw( Bitmap bm, Graphics g)
        {
            int minX = this.Start.X < this.End.X ? this.Start.X : this.End.X;
            int minY = this.Start.Y < this.End.Y ? this.Start.Y : this.End.X;
            g.DrawRectangle(new Pen(Color.FromArgb(COLOR), size_pen), this.Start.X, this.Start.Y, this.Width, this.Length);
        }



        public override bool Is_Inside(int x, int y)
        {
            int mid_x, mid_y;
            mid_x = (Start.X + end.X) / 2;
            mid_y = (start.Y + end.Y) / 2;
            return (Math.Abs(mid_x - x) <= width / 2 && Math.Abs(mid_y - y) <= length / 2);


        }
    }
}
