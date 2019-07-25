using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VectorMap2Opendrive
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadManager.LoadCSV();

            int roadCount = LoadManager.GetRoadCount();


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
