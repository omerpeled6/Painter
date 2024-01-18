using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
namespace FinalProject_Omer_Chen
{
    [Serializable]
    abstract class ComponentsOnScreen 
    {
        
        public Point start; 
        static protected int count= 0;
        protected int id_;
        public Point Start
        {
            get { return start; }
            set { start = value; }
        }

        public int Id_
        {
            get { return id_; }
            set { id_ = value; }
        }
        public abstract void Draw(  Bitmap bm, Graphics g);
        public abstract  bool Is_Inside(int x, int y);
    }
}
