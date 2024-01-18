using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
namespace FinalProject_Omer_Chen
{
    [Serializable]
    abstract class BlockedShape : Shape
    {
        protected int width;
        protected int length;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
    }
}
