using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMap2Opendrive
{
    public class VM_DtLane  :   BaseData
    {
        public float Dist;
        public int PID;
        public float Dir;
        public float Apara;
        public float r;
        public float slope;
        public float cant;
        public float LW;
        public float RW;

        public override void LoadData(int nRowIndex, DataFile file)
        {
            int readIndex = 0;
            m_ID = int.Parse(file.getString(readIndex)); readIndex++;
            Dist = float.Parse(file.getString(readIndex)); readIndex++;
            PID = int.Parse(file.getString(readIndex)); readIndex++;
            Dir = float.Parse(file.getString(readIndex)); readIndex++;
            Apara = float.Parse(file.getString(readIndex)); readIndex++;
            r = float.Parse(file.getString(readIndex)); readIndex++;
            slope = float.Parse(file.getString(readIndex)); readIndex++;
            cant = float.Parse(file.getString(readIndex)); readIndex++;
            LW = float.Parse(file.getString(readIndex)); readIndex++;
            RW = float.Parse(file.getString(readIndex)); readIndex++;
        }
    }
}
