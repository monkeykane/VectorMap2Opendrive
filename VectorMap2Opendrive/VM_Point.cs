using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMap2Opendrive
{
    public class VM_Point   :   BaseData
    {
        public double B;
        public double L;
        public double H;
        public double Bx;
        public double Ly;
        public int ReF;
        public int MCODE1;
        public int MCODE2;
        public int MCODE3;

        public override void LoadData(int nRowIndex, DataFile file)
        {
            int readIndex = 0;
            m_ID = int.Parse(file.getString(readIndex)); readIndex++;
            B = double.Parse(file.getString(readIndex)); readIndex++;
            L = double.Parse(file.getString(readIndex)); readIndex++;
            H = double.Parse(file.getString(readIndex)); readIndex++;
            Bx = double.Parse(file.getString(readIndex)); readIndex++;
            Ly = double.Parse(file.getString(readIndex)); readIndex++;
            ReF = int.Parse(file.getString(readIndex)); readIndex++;
            MCODE1 = int.Parse(file.getString(readIndex)); readIndex++;
            MCODE2 = int.Parse(file.getString(readIndex)); readIndex++;
            MCODE3 = int.Parse(file.getString(readIndex)); readIndex++;
        }
    }
}
