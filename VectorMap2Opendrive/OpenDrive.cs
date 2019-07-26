using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace VectorMap2Opendrive
{
    public class PolynomCurve
    {
        public double start;
        public double a, b, c, d;
    }

    public class ODLane
    {
        public int id;
        public string type;
        public string level;
        public string predecessor;
        public string successor;
        public List<PolynomCurve> widths;
    }

    public class ODLaneSection
    {
        public double start;
        public ODLane center;
        public List<ODLane> lefts;
        public List<ODLane> rights;
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
        public List<ODLaneSection> laneSections;
    }


    public class OpenDrive
    {
        public List<ODRoad> roads;

        public void SaveToXML(string path)
        {
            var data = new OpenDRIVE { };
            
            data.header = new OpenDRIVEHeader();
            data.header.revMajor = 1;
            data.header.revMajorSpecified = true;
            data.header.revMinor = 4;
            data.header.revMinorSpecified = true;

            data.road = new OpenDRIVERoad[roads.Count];
            for (int i = 0; i < roads.Count; ++i )
            {
                data.road[i] = new OpenDRIVERoad();
                data.road[i].id = roads[i].id.ToString();
                data.road[i].name = roads[i].name;
                data.road[i].length = roads[i].length;
                data.road[i].lengthSpecified = true;
                data.road[i].junction = roads[i].junctionId;
                data.road[i].link = new OpenDRIVERoadLink();
                data.road[i].type = new OpenDRIVERoadType[1];
                data.road[i].type[0] = new OpenDRIVERoadType();
                data.road[i].type[0].s = 0;
                data.road[i].type[0].sSpecified = true;
                data.road[i].type[0].type = roadType.unknown;
                data.road[i].type[0].typeSpecified = true;

                // plan view
                int planCount = roads[i].lineGeometries.Count;
                data.road[i].planView = new OpenDRIVERoadGeometry[planCount];
                for( int planIndex = 0; planIndex < planCount; ++planIndex )
                {
                    data.road[i].planView[planIndex] = new OpenDRIVERoadGeometry();
                    data.road[i].planView[planIndex].s = roads[i].lineGeometries[planIndex].start;
                    data.road[i].planView[planIndex].sSpecified = true;
                    data.road[i].planView[planIndex].x = roads[i].lineGeometries[planIndex].xs;
                    data.road[i].planView[planIndex].xSpecified = true;
                    data.road[i].planView[planIndex].y = roads[i].lineGeometries[planIndex].ys;
                    data.road[i].planView[planIndex].ySpecified = true;
                    data.road[i].planView[planIndex].hdg = roads[i].lineGeometries[planIndex].hdg;
                    data.road[i].planView[planIndex].hdgSpecified = true;
                    data.road[i].planView[planIndex].length = roads[i].lineGeometries[planIndex].length;
                    data.road[i].planView[planIndex].lengthSpecified = true;
                    data.road[i].planView[planIndex].Items = new OpenDRIVERoadGeometryLine[1];
                    data.road[i].planView[planIndex].Items[0] = new OpenDRIVERoadGeometryLine();
                }

                // elevation
                int eleCount = roads[i].elevations.Count;
                data.road[i].elevationProfile = new OpenDRIVERoadElevationProfile();
                data.road[i].elevationProfile.elevation = new OpenDRIVERoadElevationProfileElevation[eleCount];
                for ( int eleIndex = 0; eleIndex < eleCount; ++eleIndex )
                {
                    data.road[i].elevationProfile.elevation[eleIndex] = new OpenDRIVERoadElevationProfileElevation();
                    data.road[i].elevationProfile.elevation[eleIndex].s = roads[i].elevations[eleIndex].start;
                    data.road[i].elevationProfile.elevation[eleIndex].sSpecified = true;
                    data.road[i].elevationProfile.elevation[eleIndex].a = roads[i].elevations[eleIndex].a;
                    data.road[i].elevationProfile.elevation[eleIndex].aSpecified = true;
                    data.road[i].elevationProfile.elevation[eleIndex].b = roads[i].elevations[eleIndex].b;
                    data.road[i].elevationProfile.elevation[eleIndex].bSpecified = true;
                    data.road[i].elevationProfile.elevation[eleIndex].c = roads[i].elevations[eleIndex].c;
                    data.road[i].elevationProfile.elevation[eleIndex].cSpecified = true;
                    data.road[i].elevationProfile.elevation[eleIndex].d = roads[i].elevations[eleIndex].d;
                    data.road[i].elevationProfile.elevation[eleIndex].dSpecified = true;
                }


                // lane
                data.road[i].lanes = new OpenDRIVERoadLanes();
                int laneCount = roads[i].laneSections.Count;
                data.road[i].lanes.laneSection = new OpenDRIVERoadLanesLaneSection[laneCount];
                for ( int laneIndex = 0; laneIndex < laneCount; ++laneIndex)
                {
                    data.road[i].lanes.laneSection[laneIndex] = new OpenDRIVERoadLanesLaneSection();
                    data.road[i].lanes.laneSection[laneIndex].s = roads[i].laneSections[laneIndex].start;
                    data.road[i].lanes.laneSection[laneIndex].sSpecified = true;

                    // center 
                    data.road[i].lanes.laneSection[laneIndex].center = new OpenDRIVERoadLanesLaneSectionCenter();
                    data.road[i].lanes.laneSection[laneIndex].center.lane = new centerLane();
                    data.road[i].lanes.laneSection[laneIndex].center.lane.id = roads[i].laneSections[laneIndex].center.id;
                    data.road[i].lanes.laneSection[laneIndex].center.lane.idSpecified = true;
                    data.road[i].lanes.laneSection[laneIndex].center.lane.type = laneType.none;
                    data.road[i].lanes.laneSection[laneIndex].center.lane.typeSpecified = true;
                    data.road[i].lanes.laneSection[laneIndex].center.lane.level = singleSide.@false;
                    data.road[i].lanes.laneSection[laneIndex].center.lane.levelSpecified = true;
                    data.road[i].lanes.laneSection[laneIndex].center.lane.link = new centerLaneLink();
                    data.road[i].lanes.laneSection[laneIndex].center.lane.link.predecessor = new centerLaneLinkPredecessor();
                    data.road[i].lanes.laneSection[laneIndex].center.lane.link.predecessor.id = -1;
                    data.road[i].lanes.laneSection[laneIndex].center.lane.link.predecessor.idSpecified = true;

                    // right
                    data.road[i].lanes.laneSection[laneIndex].right = new OpenDRIVERoadLanesLaneSectionRight();
                    int rlaneCount = roads[i].laneSections[laneIndex].rights.Count;
                    data.road[i].lanes.laneSection[laneIndex].right.lane = new lane[rlaneCount];
                    for( int rIndex = 0; rIndex < rlaneCount; ++ rIndex )
                    {
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex] = new lane();
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].id = roads[i].laneSections[laneIndex].rights[rIndex].id;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].idSpecified = true;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].type = laneType.driving;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].typeSpecified = true;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].level = singleSide.@false;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].levelSpecified = true;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].link = new laneLink();
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].link.predecessor = new laneLinkPredecessor();
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].link.predecessor.id = -1;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].link.predecessor.idSpecified = true;
                        // width
                        int widthCount = roads[i].laneSections[laneIndex].rights[rIndex].widths.Count;
                        data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].Items = new laneWidth[widthCount];
                        for( int wIndex = 0; wIndex < widthCount; ++wIndex )
                        {
                            laneWidth lw = new laneWidth();
                            lw.sOffset = roads[i].laneSections[laneIndex].rights[rIndex].widths[wIndex].start;
                            lw.sOffsetSpecified = true;
                            lw.a = roads[i].laneSections[laneIndex].rights[rIndex].widths[wIndex].a;
                            lw.aSpecified = true;
                            lw.b = roads[i].laneSections[laneIndex].rights[rIndex].widths[wIndex].b;
                            lw.bSpecified = true;
                            lw.c = roads[i].laneSections[laneIndex].rights[rIndex].widths[wIndex].c;
                            lw.cSpecified = true;
                            lw.d = roads[i].laneSections[laneIndex].rights[rIndex].widths[wIndex].d;
                            lw.dSpecified = true;

                            data.road[i].lanes.laneSection[laneIndex].right.lane[rIndex].Items[wIndex] = lw;
                        }
                    }
                }
            }

            var serializer = new XmlSerializer(typeof(OpenDRIVE));

            using (var stream = new StreamWriter(path))
                serializer.Serialize(stream, data);
        }
    }
}
