using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMap2Opendrive
{
    public class VM_Node : BaseData
    {
        public int PID;

        public override void LoadData(int nRowIndex, DataFile file)
        {
            int readIndex = 0;
            m_ID = int.Parse(file.getString(readIndex)); readIndex++;
            PID = int.Parse(file.getString(readIndex)); readIndex++;
        }
    }
}
