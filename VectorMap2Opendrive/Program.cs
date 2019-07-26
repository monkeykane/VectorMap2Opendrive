using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace VectorMap2Opendrive
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadManager.LoadCSV();

            int roadCount = LoadManager.GetRoadCount();
            OpenDrive opDrive = new OpenDrive();
            opDrive.roads = new List<ODRoad>(roadCount);

            int gldir = -1; // -1 means running on left

            int DIDIndex = 0;
            for ( int i = 0; i < roadCount; ++i )
            {
                ODRoad road = new ODRoad();
                road.name = i.ToString();
                road.id = i.ToString();
                road.junctionId = "-1";
                road.predecessor = "-1";
                road.successor = "-1";
                road.length = 0;
                road.lineGeometries = new List<PlaneLine>();
                road.elevations = new List<PolynomCurve>();
                road.laneSections = new List<ODLaneSection>();

                int key = 0;
                VM_DtLane nextdtLane = CSVFileManager<VM_DtLane>.Instance().GetstItemByIndex(DIDIndex+1, out key);
                float start = 0;

                while ( nextdtLane != null && nextdtLane.Dist != 0 )
                {
                    VM_DtLane dtlane = CSVFileManager<VM_DtLane>.Instance().GetstItemByIndex(DIDIndex, out key);
                    VM_Point P = CSVFileManager<VM_Point>.Instance().GetstItemByID(dtlane.PID);
                    VM_Point nextP = CSVFileManager<VM_Point>.Instance().GetstItemByID(nextdtLane.PID);
                    Vector3 vP = new Vector3((float)P.Ly, 0, (float)P.Bx);
                    Vector3 vNextP = new Vector3((float)nextP.Ly, 0, (float)nextP.Bx);
                    Vector3 dir = (vNextP - vP).normalized;
                    Vector3 Zdir = Vector3.Cross(dir, Vector3.up) * gldir;
                    float offset = gldir == -1 ? dtlane.RW : dtlane.LW;
                    float nextOffset = gldir == -1 ? nextdtLane.RW : nextdtLane.LW;

                    Vector3 road_center_start = vP + Zdir * offset;
                    Vector3 road_center_end = vNextP + Zdir * nextOffset;

                    float secLength = Vector3.Distance(road_center_start, road_center_end);
                    float hdg = Mathf.Atan2((road_center_end.z - road_center_start.z) , (road_center_end.x - road_center_start.x));
                    road.length += secLength;

                    // plan geo
                    PlaneLine planView = new PlaneLine();
                    planView.start = start;
                    planView.xs = road_center_start.x;
                    planView.ys = road_center_start.z;
                    planView.hdg = hdg;
                    planView.length = secLength;
                    road.lineGeometries.Add(planView);

                    // elevation
                    PolynomCurve ele = new PolynomCurve();
                    ele.start = start;
                    ele.a = (float)P.H;
                    ele.b = (nextP.H - P.H) / secLength;
                    ele.c = 0;
                    ele.d = 0;
                    road.elevations.Add(ele);

                    // lanes
                    ODLaneSection laneSection = new ODLaneSection();
                    laneSection.start = start;
                    
                    ODLane center = new ODLane();
                    center.id = 0;

                    center.predecessor = "-1";
                    center.successor = "-1";
                    laneSection.center = center;

                    laneSection.rights = new List<ODLane>();
                    ODLane sideLane = new ODLane();
                    sideLane.id = -1; //right;
                    sideLane.type = "driving";
                    sideLane.level = "0";
                    sideLane.predecessor = "-1";
                    sideLane.successor = "-1";
                    sideLane.widths = new List<PolynomCurve>();
                    PolynomCurve width = new PolynomCurve();
                    width.start = 0;
                    width.a = dtlane.LW + dtlane.RW;
                    width.b = ((nextdtLane.LW + nextdtLane.RW) - width.a) / secLength;
                    width.c = 0;
                    width.d = 0;
                    sideLane.widths.Add(width);
                    laneSection.rights.Add(sideLane);

                    road.laneSections.Add(laneSection);

                    start += secLength;

                    ++DIDIndex;
                    nextdtLane = CSVFileManager<VM_DtLane>.Instance().GetstItemByIndex(DIDIndex + 1, out key);
                }

                opDrive.roads.Add(road);
                ++DIDIndex;
            }
            string path = Directory.GetCurrentDirectory();
            opDrive.SaveToXML(path + "/../../OpenDrive/output.xodr");
            LoadManager.CleanUp();
        }
    }


    public class LoadManager
    {
        public static string LoadDataFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

        public static void LoadCSV()
        {
            string path = Directory.GetCurrentDirectory();
            CSVFileManager<VM_DtLane>.Instance().LoadData(path + "/../../VectorMap/dtLane.csv", LoadDataFile);
            CSVFileManager<VM_Lane>.Instance().LoadData(path + "/../../VectorMap/Lane.csv", LoadDataFile);
            CSVFileManager<VM_Point>.Instance().LoadData(path + "/../../VectorMap/point.csv", LoadDataFile);
            CSVFileManager<VM_Node>.Instance().LoadData(path + "/../../VectorMap/node.csv", LoadDataFile);
        }

        public static void CleanUp()
        {
            CSVFileManager<VM_DtLane>.Instance().ClearUp();
            CSVFileManager<VM_Lane>.Instance().ClearUp();
            CSVFileManager<VM_Point>.Instance().ClearUp();
            CSVFileManager<VM_Node>.Instance().ClearUp();
        }

        public static int GetRoadCount()
        {
            int ret = 0;
            int dtLaneCount = CSVFileManager<VM_DtLane>.Instance().GetstItemCount();
            for ( int i = 0; i < dtLaneCount; ++i )
            {
                int key = -1;
                VM_DtLane dtlane = CSVFileManager<VM_DtLane>.Instance().GetstItemByIndex(i, out key);
                if (dtlane != null )
                {
                    if (dtlane.Dist == 0)
                        ++ret;
                }
            }
            return ret;
        }
    }
}
