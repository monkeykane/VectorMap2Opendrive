using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMap2Opendrive
{
    public class PolynomCurve
    {
        public double start;
        public double a, b, c, d;
    }

    public class ODLane
    {
        public string id;
        public string type;
        public string level;
        public string predecessor;
        public string successor;
        public List<PolynomCurve> widths;
    }

    public class ODLaneSection
    {
        public double start;
        public List<ODLane> lanes;    
    }

    public class PlaneLine
    {
        public double start;
        public double length;
        public double xs;
        public double ys;
        public double hdg;
    }

    public class ODRoad
    {
        public string name;
        public double length;
        public string id;
        public string junctionId;
        public string predecessor;
        public string successor;
        public List<PlaneLine> lineGeometries;
        public List<PolynomCurve> elevations;
    }


    class OpenDrive
    {
    }
}
