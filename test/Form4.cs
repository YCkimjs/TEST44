using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidEdgeFramework;
using YCC_COMMON;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Media.Media3D;

namespace test
{
    public partial class Form4 : Form
    {
        static SolidEdgeFramework.Application SEApp = null;
        static SolidEdgeFramework.SolidEdgeTCE objTCE = null;
        public Form4()
        {
            InitializeComponent();
        }

        static bool OpenFileFromTC(string strItemIn, string strRevision, string strExtension)
        {
            bool bSuccess = false;

            try
            {
                object outResult;
                int nPropCount;
                string strSEFilePath = "";
                string strCachePath = "";
                string FileRevisionRule = null;
                object[,] temp = new object[1, 1];

                // Connect to or start Solid Edge.
                YCC_solidedge.getEdgeApplication(ref SEApp, true);

                // Get SEEC handle
                objTCE = SEApp.SolidEdgeTCE;
                if (null == objTCE) return false;

                // Turn TC mode on
                objTCE.SetTeamCenterMode(true);

                // Obtain list of files.
                objTCE.GetListOfFilesFromTeamcenterServer(strItemIn, strRevision,
                                                          out outResult, out nPropCount);
                // get cache path
                objTCE.GetPDMCachePath(out strCachePath);

                object[] result = outResult as object[]; //conversion object to object array

                foreach (object file in result)
                {
                    MessageBox.Show(file.ToString());
                    string currfile = file.ToString();
                    objTCE.DownladDocumentsFromServerWithOptions(strItemIn,
                                                                strRevision,
                                                                currfile,
                                                                FileRevisionRule,
                                                                "",
                                                                true,
                                                                false,
                                                                (uint)SolidEdgeConstants.TCDownloadOptions.COImplicit,
                                                                temp);

                    // Download complete.
                    if (null != temp)
                    {
                        strSEFilePath = strCachePath + "\\" + currfile;

                        object objOutDoc = SEApp.Documents.Open(strSEFilePath);

                        if (objOutDoc != null)
                        {
                            return true;
                        }
                    }
                }

                return bSuccess;
            }
            catch (System.Exception ex)
            {
                return bSuccess;
            }
            finally
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileFromTC("000025", "A", ".asm");


            SolidEdgeDocument SEDoc = (SolidEdgeDocument)SEApp.ActiveDocument;
            SEDoc.Save();
            SEDoc.Close();
            //object[] q = new object[] {SEDoc.FullName};

            //objTCE.CheckInDocumentsToTeamCenterServer(q, true);
            //objTCE.
            //MessageBox.Show(SEDoc.FullName);
            //SEDoc.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 200; i++)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = @"C:\Users\Administrator\Desktop\Debug\SEEC_Open.exe";
                psi.Arguments = "000024 A";

                Process p = Process.Start(psi);
                p.WaitForExit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // Connect to or start Solid Edge.
            YCC_solidedge.getEdgeApplication(ref SEApp, true);


            SolidEdgeAssembly.AssemblyDocument asydoc = (SolidEdgeAssembly.AssemblyDocument)SEApp.ActiveDocument;

            SolidEdgeAssembly.Occurrences occs = (SolidEdgeAssembly.Occurrences)asydoc.Occurrences;
            SolidEdgeAssembly.Occurrence occ = (SolidEdgeAssembly.Occurrence)occs.Item(2);

            Array MinRangePoint = Array.CreateInstance(typeof(double), 0);
            Array MaxRangePoint = Array.CreateInstance(typeof(double), 0);
            //object[] w = new object[3];

            occ.GetRangeBox(ref MinRangePoint, ref MaxRangePoint);


            SolidEdgePart.PartDocument ps = (SolidEdgePart.PartDocument)occ.OccurrenceDocument;
            //SolidEdgePart.Models ms = (SolidEdgePart.Models)ps.Models.Item[1];
            SolidEdgePart.Model m = (SolidEdgePart.Model)ps.Models.Item(1);

            SolidEdgeGeometry.Body b = (SolidEdgeGeometry.Body)m.Body;

            SolidEdgeGeometry.Edges ees = (SolidEdgeGeometry.Edges)b.Edges[SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll];
            SolidEdgeGeometry.Edge ee = (SolidEdgeGeometry.Edge)ees.Item(1);

            //ee.GetRange(ref MinRangePoint, ref MaxRangePoint);


            b.GetRange(ref MinRangePoint, ref MaxRangePoint);


        }


        SolidEdgePart.PartDocument partDoc;
        SolidEdgePart.Sketch3D sk;

        private void button4_Click(object sender, EventArgs e)
        {           

            YCC_solidedge.getEdgeApplication(ref SEApp, true);
            partDoc = (SolidEdgePart.PartDocument)SEApp.ActiveDocument;
            SolidEdgePart.Model md = partDoc.Models.Item(1);
            SolidEdgeGeometry.Body by = (SolidEdgeGeometry.Body)md.Body;
            SolidEdgeGeometry.Edges ed = (SolidEdgeGeometry.Edges)by.Edges[SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryStraight];

            List<SolidEdgeGeometry.Edge> ListExceptionEdge = new List<SolidEdgeGeometry.Edge>();

            int Count = 0;

            foreach (SolidEdgeGeometry.Edge _e in ed)
            {
                if (ListExceptionEdge.Contains(_e)) continue;

                SEEdge newpoint = new SEEdge(_e);

                if (newpoint.Len == 23.09 || newpoint.Len == 11.55)
                {
                    POP newPOP = new POP(newpoint);
                    Count = Count + 1;

                    if (newPOP.ListHexagonEdge.Count == 6)
                    {
                        foreach (SolidEdgeGeometry.Edge _edge in newPOP.ListHexagonEdge)
                        {
                            ListExceptionEdge.Add(_edge);
                        }
                        foreach (SolidEdgeGeometry.Edge _edge in newPOP.ListHoleEdge)
                        {
                            ListExceptionEdge.Add(_edge);
                        }
                        foreach (SolidEdgeGeometry.Edge _edge in newPOP.ListOtherHexagonEdge)
                        {
                            ListExceptionEdge.Add(_edge);
                        }
                    }
                }
            }
     

            //bool done = true;
            //int count = 0;
            //while (done)
            //{
            //    foreach (SEEdge _p in point23)
            //    {
            //        if (www(ref point23, _p))
            //        {
            //            count++;
            //            break;
            //        }

            //        if (_p == point23[point23.Count() - 1]) done = false;                    
            //    }

            //    if (point23.Count == 0) done = false;
            //}


        }

        private Double EdgeLen(double[] a, double[] b)
        {
            Double EdgeLength = Math.Sqrt(((a[0] - b[0]) * (a[0] - b[0]) + (a[1] - b[1]) * (a[1] - b[1]) + (a[2] - b[2]) * (a[2] - b[2])));
            return Math.Round(EdgeLength * 1000, 2);
        }


        private bool hhh(ref List<Hexagon> listHexa, Hexagon h)
        {
            return false;
        }

        public static double AngleBetweenThreePoints(Point3D[] points)//, Vector3D up)
        {
            double x1 = 0 - points[1].X;
            double y1 = 0 - points[1].Y;
            double z1 = 0 - points[1].Z;

            Vector3D vt1 = new Vector3D(points[0].X + x1, points[0].Y + y1, points[0].Z + z1);
            Vector3D vt3 = new Vector3D(points[2].X + x1, points[2].Y + y1, points[2].Z + z1);

            return Vector3D.AngleBetween(vt1, vt3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }
    }

    public class SEEdge
    {

        public double[] StartP;
        public double[] EndP;
        public Double Len;
        public SolidEdgeGeometry.Edge edge;
        public SolidEdgeGeometry.Vertex VertexStart;
        public SolidEdgeGeometry.Vertex VertexEnd;
        public SEEdge(SolidEdgeGeometry.Edge edge)
        {
            this.edge = edge;

            Array StartPoint = Array.CreateInstance(typeof(double), 0);
            Array EndPoint = Array.CreateInstance(typeof(double), 0);

            VertexStart= (SolidEdgeGeometry.Vertex)edge.StartVertex;
            VertexEnd = (SolidEdgeGeometry.Vertex)edge.EndVertex;

            VertexStart.GetPointData(ref StartPoint);
            VertexEnd.GetPointData(ref EndPoint);

            StartP = (double[])StartPoint;
            EndP = (double[])EndPoint;

            Len = EdgeLen(StartP, EndP);
        }

        private Double EdgeLen(double[] a, double[] b)
        {
            Double EdgeLength = Math.Sqrt(((a[0] - b[0]) * (a[0] - b[0]) + (a[1] - b[1]) * (a[1] - b[1]) + (a[2] - b[2]) * (a[2] - b[2])));
            return Math.Round(EdgeLength * 1000, 2);
        }
    }

    public class Hexagon
    {
        Vector3D[] Points = new Vector3D[6];

        public Hexagon(SEEdge[] HexagonPoints)
        {
            for (int i = 0; i < 6; i++)
            {
                Points[i] = new Vector3D(HexagonPoints[i].StartP[0], HexagonPoints[i].StartP[1], HexagonPoints[i].StartP[2]);
            }
        }

        public bool hasPoints(Vector3D points)
        {
            return Points.Contains(points);
        }
    }

    public class POP
    {
        SolidEdgePart.Sketch3D sk;
        SolidEdgePart.PartDocument partDoc;

        Vector3D[] Points = new Vector3D[6];
        SolidEdgeGeometry.Edge StandardEdge;
        public List<SolidEdgeGeometry.Edge> ListHexagonEdge = new List<SolidEdgeGeometry.Edge>();
        public List<SolidEdgeGeometry.Edge> ListHoleEdge = new List<SolidEdgeGeometry.Edge>();
        public List<SolidEdgeGeometry.Edge> ListOtherHexagonEdge = new List<SolidEdgeGeometry.Edge>();


        public POP(SEEdge initEdge)
        {
            StandardEdge = initEdge.edge;

            //partDoc = (SolidEdgePart.PartDocument)StandardEdge.Document;
            //sk = partDoc.Sketches3D.Add();
                      
            validpoint(initEdge.edge);
        }

        public void validpoint(SolidEdgeGeometry.Edge Edge)
        {
            if (ListHexagonEdge.Count() != 0 && Edge == StandardEdge) return;
            SolidEdgeGeometry.Vertex VertexStart;
            SolidEdgeGeometry.Vertex VertexEnd;

            VertexStart = (SolidEdgeGeometry.Vertex)Edge.StartVertex;
            VertexEnd = (SolidEdgeGeometry.Vertex)Edge.EndVertex;

            SolidEdgeGeometry.Edges EdgesStartPoint = (SolidEdgeGeometry.Edges)VertexStart.Edges;

            SolidEdgeGeometry.Edge HexagonEdge;
            SolidEdgeGeometry.Edge HoleEdge;

            //edge가 2개일 경
            if (EdgesStartPoint.Count != 3) return;

            foreach (SolidEdgeGeometry.Edge _edge in EdgesStartPoint)
            {
                if (_edge == Edge) continue;

                if (_edge.StartVertex != VertexStart)
                {
                    SolidEdgeGeometry.Vertex newVertex = (SolidEdgeGeometry.Vertex)_edge.StartVertex;
                    double angle = AngleBetweenThreePoints(VertexEnd, VertexStart, newVertex);

                    if (angle == 120)
                    {                        
                        HexagonEdge = _edge;
                        ListHexagonEdge.Add(_edge);

                        Array p1 = Array.CreateInstance(typeof(double), 0);
                        newVertex.GetPointData(ref p1);
                        //sk.Points3D.Add(3, p1);

                        validpoint(_edge);
                    }
                    else if (angle == 90)
                    {
                        HoleEdge = _edge;
                        ListHoleEdge.Add(_edge);
                        GetOtherEdge(newVertex, _edge);

                        //Array p1 = Array.CreateInstance(typeof(double), 0);
                        //newVertex.GetPointData(ref p1);
                        //sk.Points3D.Add(3, p1);
                    }
                    else
                    {
                        //
                        return;
                    }
                    
                }
                else
                {
                    SolidEdgeGeometry.Vertex newVertex = (SolidEdgeGeometry.Vertex)_edge.EndVertex;
                    double angle = AngleBetweenThreePoints(VertexEnd, VertexStart, newVertex);

                    if (angle == 120)
                    {
                        HexagonEdge = _edge;
                        ListHexagonEdge.Add(_edge);
                        validpoint(_edge);
                    }
                    else if (angle == 90)
                    {
                        HoleEdge = _edge;
                        ListHoleEdge.Add(_edge);
                        GetOtherEdge(newVertex, _edge);

                        //Array p1 = Array.CreateInstance(typeof(double), 0);
                        //newVertex.GetPointData(ref p1);
                        //sk.Points3D.Add(3, p1);
                    }
                    else
                    {
                        //
                        return;
                    }
                }
                
            }
        }

        private void GetOtherEdge(SolidEdgeGeometry.Vertex Vertex, SolidEdgeGeometry.Edge Edge)
        {
            SolidEdgeGeometry.Edges Edges = (SolidEdgeGeometry.Edges)Vertex.Edges;

            foreach (SolidEdgeGeometry.Edge _edge in Edges)
            {
                if (_edge == Edge) continue;

                if (!ListOtherHexagonEdge.Contains(_edge))
                {
                    ListOtherHexagonEdge.Add(_edge);                    
                }
            }
        }


        public static double AngleBetweenThreePoints(SolidEdgeGeometry.Vertex Start, SolidEdgeGeometry.Vertex Center, SolidEdgeGeometry.Vertex End)//, Vector3D up)
        {
            //Vertexs.GetPointData

            Array p1 = Array.CreateInstance(typeof(double), 0);
            Array p2 = Array.CreateInstance(typeof(double), 0);
            Array p3 = Array.CreateInstance(typeof(double), 0);
            double[] pointStart;
            double[] pointCenter;
            double[] pointEnd;

            Start.GetPointData(ref p1);
            Center.GetPointData(ref p2);
            End.GetPointData(ref p3);

            pointStart = (double[])p1;
            pointCenter = (double[])p2;
            pointEnd = (double[])p3;

            double x1 = 0 - pointCenter[0];
            double y1 = 0 - pointCenter[1];
            double z1 = 0 - pointCenter[2];

            Vector3D vt1 = new Vector3D(pointStart[0] + x1, pointStart[1] + y1, pointStart[2] + z1);
            Vector3D vt3 = new Vector3D(pointEnd[0] + x1, pointEnd[1] + y1, pointEnd[2] + z1);

            return Math.Round(Vector3D.AngleBetween(vt1, vt3));
        }
    }
}
