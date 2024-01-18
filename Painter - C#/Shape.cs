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
    abstract class Shape: ComponentsOnScreen 
    {

        protected int size_pen;
        protected int color;
        public Point  end;

        public Point End
        {
            get { return end; }
            set { end = value; }
        }
        public int Size_pen
        {
            get { return size_pen; }
            set { size_pen = value; }
        }
        public int COLOR
        {
            get { return color; }
            set { color = value; }
        }




        public abstract override void Draw( Bitmap bm, Graphics g);


        public abstract override bool  Is_Inside(int x, int y); 

        
       










    }
}
