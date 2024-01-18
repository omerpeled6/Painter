using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_Omer_Chen
{
    [Serializable]
    public class Undo
    {
        int mission;
        int id;
        public Undo(int mission,int id)
        {
            this.mission = mission;
            this.id = id;
            
        }
        public int Mission
        {
            get { return mission; }
            set { mission = value; }

        }
        public int Id
        {
            get { return id; }
            set { id = value; }

        }



    }
}
