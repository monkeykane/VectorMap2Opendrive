using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMap2Opendrive
{
    public class VM_Lane    :   BaseData
    {
        public int DID;
        public int BLID;
        public int FLID;
        public int BNID;
        public int FNID;
        public int JCT;
        public int BLID2;
        public int BLID3;
        public int BLID4;
        public int FLID2;
        public int FLID3;
        public int FLID4;
        public int ClossID;
        public float Span;
        public int LCnt;
        public int Lno;
        public int LaneType;
        public float LimitVel;
        public float RefVel;
        public int RoadSecID;
        public int LaneChgFG;

        public override void LoadData(int nRowIndex, DataFile file)
        {
            int readIndex = 0;
            m_ID = int.Parse(file.getString(readIndex)); readIndex++;
            DID = int.Parse(file.getString(readIndex)); readIndex++;
            BLID = int.Parse(file.getString(readIndex)); readIndex++;
            FLID = int.Parse(file.getString(readIndex)); readIndex++;
            BNID = int.Parse(file.getString(readIndex)); readIndex++;
            FNID = int.Parse(file.getString(readIndex)); readIndex++;
            JCT = int.Parse(file.getString(readIndex)); readIndex++;
            BLID2 = int.Parse(file.getString(readIndex)); readIndex++;
            BLID3 = int.Parse(file.getString(readIndex)); readIndex++;
            BLID4 = int.Parse(file.getString(readIndex)); readIndex++;
            FLID2 = int.Parse(file.getString(readIndex)); readIndex++;
            FLID3 = int.Parse(file.getString(readIndex)); readIndex++;
            FLID4 = int.Parse(file.getString(readIndex)); readIndex++;
            ClossID = int.Parse(file.getString(readIndex)); readIndex++;
            Span = float.Parse(file.getString(readIndex)); readIndex++;
            LCnt = int.Parse(file.getString(readIndex)); readIndex++;
            Lno = int.Parse(file.getString(readIndex)); readIndex++;
            LaneType = int.Parse(file.getString(readIndex)); readIndex++;
            LimitVel = float.Parse(file.getString(readIndex)); readIndex++;
            RefVel = float.Parse(file.getString(readIndex)); readIndex++;
            RoadSecID = int.Parse(file.getString(readIndex)); readIndex++;
            LaneChgFG = int.Parse(file.getString(readIndex)); readIndex++;

        }
    }
}
