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
    class ComponentsList
    {
        private SortedList MyElements;
        
        public ComponentsList()
        {
            MyElements = new SortedList();
        }
        
        public int NextIndex
        {
            get
            {
                return MyElements.Count;
            }
        }
        public ComponentsOnScreen this[int index] 
        {
            get
            {
                if (index >= MyElements.Count||index<0)
                    return (ComponentsOnScreen)null;
                return (ComponentsOnScreen)MyElements.GetByIndex(index);
            }
            set
            {
                if (index <= MyElements.Count&&index>=0)
                    MyElements[index] = value;	
            }
        }
        public void remove_all()
        {
            MyElements.Clear();
        }
        public void ADD(ComponentsOnScreen e)
        {
            MyElements.Add(MyElements.Count, e); 

        }
        public void Remove(int element)
        {
            if (element >= 0 && element < MyElements.Count) 
            {
                for (int i = element; i < MyElements.Count - 1; i++)
                    MyElements[i] = MyElements[i + 1];
                MyElements.RemoveAt(MyElements.Count - 1);
            }
        }
        public void DrawAll( Bitmap bm,Graphics g)
        {
           
           
            for (int i = 0; i < MyElements.Count; i++)
                ((ComponentsOnScreen)MyElements[i]).Draw(bm,g);
        }

    }
}

