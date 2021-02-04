using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using WGLAV5;

namespace graduationDesign
{
    class LayerModel
    {
        myGrids[,] vgrid = new myGrids[12, 12];

        struct MarkerbedsInfo
        {
            //标志层代码
            public string Markerbeds;
            public double X;
            public double Y;
            //上表面高程
            public double ZUp;
            //下表面高程
            public double ZDown;

        }

        //插值后标志层的高程
        struct myGrids
        {
            public string MarkerbedsName;
            public double x;
            public double y;
            public double z_up;
            public double z_down;
        }

        //该列表存放了所有标志层的信息
        List<MarkerbedsInfo> markerbedsInfo = new List<MarkerbedsInfo> {};
        //该列表存放了所有的标志层名称
        List<string> SymbolLayerName = new List<string> {};
        //该列表存放了插值后标志层的值
        List<myGrids> grid = new List<myGrids> {};
        //获取列表中地层的数量
        public int getGeoNum()
        {

            this.getSymbolLayerName();
            return SymbolLayerName.Count;
        }

        //获取地层名称
        public void getSymbolLayerName()
        {
            string constr = sqlDataManager.getConn();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = "select distinct BZCDM from ZKDCZL where LEN(BZCDM)>1 and BZCDM like 'K[1-8]' ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SymbolLayerName.Add(reader[0].ToString());
                }
                conn.Close();
            }
        }

        //存储某标志层信息
        public void getSymbolLayerInfo(string layerName)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            PaintBorder pb = new PaintBorder();
            double minX = pb.getMinX();
            double minY = pb.getMinY();
            double width = pb.getWidth();
            double height = pb.getHeight();
            string constr = sqlDataManager.getConn();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql =
                    "select zkdczl.BZCDM,ZKMC.KKZBX,ZKMC.KKZBY,ZKMC.KKZBZ-ZKDCZL.CHLJ as Zup,KKZBZ-CHLJ-CH as Zdown  from ZKDCZL join ZKMC on ZKMC.ZKDM=ZKDCZL.ZKDM  where  zkdczl.YSBM is not null and zkdczl.BZCDM=@layerName and ZKDCZL.CH is not null";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlParameter para = new SqlParameter("@layerName", SqlDbType.NVarChar, 10);
                para.Value = layerName;
                cmd.Parameters.Add(para);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(ds);
                dt = ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MarkerbedsInfo s = new MarkerbedsInfo();
                    s.Markerbeds = dt.Rows[i][0].ToString();
                    s.X = (Convert.ToDouble(dt.Rows[i][1]) - minX)/width*screenWidth;
                    s.Y = (Convert.ToDouble(dt.Rows[i][2]) - minY)/height*screenHeight;
                    s.ZUp = Convert.ToDouble(dt.Rows[i][3]);
                    s.ZDown = Convert.ToDouble(dt.Rows[i][4]);
                    markerbedsInfo.Add(s);
                }
                conn.Close();
            }
        }

        public void draw(uint listNum)
        {
            GL.glCallList(listNum);
        }

        //绘制某层的格网点
        public void drawGridPoint(string layerName)
        {
            GL.glNewList(202, GL.GL_COMPILE);
            GL.glBegin(GL.GL_POINTS);
            GL.glPointSize(3);
            GL.glColor3d(0, 0, 1);
            foreach (var item in grid)
            {
                GL.glVertex3d(item.x, item.y, item.z_down);
                GL.glVertex3d(item.x, item.y, item.z_up);
            }
            GL.glEnd();
            GL.glEndList();
        }

        //反距离权重插值算法
        public void IDW(string layerName)
        {

            PaintBorder pb = new PaintBorder();
            double width = pb.getWidth();
            double height = pb.getHeight();
            double minX = pb.getMinX();
            double minY = pb.getMinY();
            double maxX = pb.getMaxX();
            double maxY = pb.getMaxY();
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            double deltaX = width/11;
            double deltaY = height/11;
            this.getSymbolLayerInfo(layerName);
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    myGrids g = new myGrids();
                    g.MarkerbedsName = layerName;
                    g.x = i*deltaX/width*screenWidth;
                    g.y = j*deltaY/height*screenHeight;
                    double fenmu = 0;
                    double fenzi1 = 0;
                    double fenzi2 = 0;
                    //求距离加权反比的分母
                    for (int r = 0; r < markerbedsInfo.Count; r++)
                    {
                        fenmu +=
                            Convert.ToDouble(1/
                                             Math.Sqrt((Math.Pow(g.x - markerbedsInfo[r].X, 2) +
                                                        Math.Pow(g.y - markerbedsInfo[r].Y, 2))));
                    }
                    //分别求出顶面和底面的分子
                    for (int l = 0; l < markerbedsInfo.Count; l++)
                    {
                        fenzi1 +=
                            Convert.ToDouble(1/
                                             Math.Sqrt(Math.Pow(g.x - markerbedsInfo[l].X, 2) +
                                                       Math.Pow(g.y - markerbedsInfo[l].Y, 2)))*markerbedsInfo[l].ZUp;
                        fenzi2 +=
                            Convert.ToDouble(1/
                                             Math.Sqrt(Math.Pow(g.x - markerbedsInfo[l].X, 2) +
                                                       Math.Pow(g.y - markerbedsInfo[l].Y, 2)))*markerbedsInfo[l].ZDown;
                    }
                    //分别求出顶面和底面的高程值
                    g.z_up = fenzi1/fenmu;
                    g.z_down = fenzi2/fenmu;
                    grid.Add(g);
                }
            }
            //将List转换为二维数组
            myGrids[] vertex = new myGrids[12*12];
            grid.CopyTo(vertex);
            int c = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    vgrid[i, j] = vertex[c];
                    c++;
                }

            }
        }

        public void drawK1Surface()
        {
            this.IDW("K1");
            GL.glNewList(203, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(1, 1, 0.5);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();
                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void drawK2Surface()
        {
            this.IDW("K2");
            GL.glNewList(204, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(1, 0.5, 0.5);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();
                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void drawK3Surface()
        {
            this.IDW("K3");
            GL.glNewList(205, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(1, 1, 0);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();
                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void drawK4Surface()
        {
            this.IDW("K4");
            GL.glNewList(206, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(1, 0.5, 0);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();
                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void drawK5Surface()
        {
            this.IDW("K5");
            GL.glNewList(207, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(1, 0, 0);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();
                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor3d(0.82, 0.41, 0.12);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void drawK6Surface()
        {
            this.IDW("K6");
            GL.glNewList(208, GL.GL_COMPILE);
            //绘制上表面
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(0.5, 0.5, 0);
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();
                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();

        }

        public void drawK7Surface()
        {
            this.IDW("K7");
            GL.glNewList(209, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(0.5, 0.5, 0.5);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();
                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void drawK8Surface()
        {
            this.IDW("K8");
            GL.glNewList(210, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(0, 0, 1);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();

                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();
            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void drawM3Coal()
        {
            this.IDW("M3");
            GL.glNewList(211, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(0.2, 0.5, 1);
            //绘制上表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_up - vgrid[i, j - 1].z_up);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_up - vgrid[i, j - 1].z_up);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_up);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_up - vgrid[i - 1, j].z_up);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_up - vgrid[i - 1, j].z_up);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_up);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_up);
                    GL.glEnd();

                }
            }
            //绘制下表面
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 11; j++)
                {
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p1 = new Point3D(vgrid[i, j].x - vgrid[i, j - 1].x, vgrid[i, j].y - vgrid[i, j - 1].y,
                        vgrid[i, j].z_down - vgrid[i, j - 1].z_down);
                    Point3D p2 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i, j - 1].x,
                        vgrid[i - 1, j - 1].y - vgrid[i, j - 1].y, vgrid[i - 1, j - 1].z_down - vgrid[i, j - 1].z_down);
                    Point3D pNormal = this.getNomalVector(p1, p2);
                    GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glVertex3d(vgrid[i, j - 1].x, vgrid[i, j - 1].y, vgrid[i, j - 1].z_down);
                    GL.glEnd();
                    GL.glBegin(GL.GL_TRIANGLES);
                    Point3D p3 = new Point3D(vgrid[i - 1, j - 1].x - vgrid[i - 1, j].x,
                        vgrid[i - 1, j - 1].y - vgrid[i - 1, j].y, vgrid[i - 1, j - 1].z_down - vgrid[i - 1, j].z_down);
                    Point3D p4 = new Point3D(vgrid[i, j].x - vgrid[i - 1, j].x, vgrid[i, j].y - vgrid[i - 1, j].y,
                        vgrid[i, j].z_down - vgrid[i - 1, j].z_down);
                    Point3D pNormal2 = this.getNomalVector(p3, p4);
                    GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                    GL.glVertex3d(vgrid[i, j].x, vgrid[i, j].y, vgrid[i, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j].x, vgrid[i - 1, j].y, vgrid[i - 1, j].z_down);
                    GL.glVertex3d(vgrid[i - 1, j - 1].x, vgrid[i - 1, j - 1].y, vgrid[i - 1, j - 1].z_down);
                    GL.glEnd();
                }
            }
            //绘制前面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 0].x - vgrid[i, 0].x, vgrid[i - 1, 0].y - vgrid[i, 0].y,
                    vgrid[i - 1, 0].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 0].z_down - vgrid[i, 0].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 0].z_down - vgrid[i - 1, 0].z_up);
                Point3D p4 = new Point3D(vgrid[i, 0].x - vgrid[i - 1, 0].x, vgrid[i, 0].y - vgrid[i - 1, 0].y,
                    vgrid[i, 0].z_down - vgrid[i - 1, 0].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 0].x, vgrid[i, 0].y, vgrid[i, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_up);
                GL.glVertex3d(vgrid[i - 1, 0].x, vgrid[i - 1, 0].y, vgrid[i - 1, 0].z_down);
                GL.glEnd();
            }
            //绘制后面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[i - 1, 11].x - vgrid[i, 11].x, vgrid[i - 1, 11].y - vgrid[i, 11].y,
                    vgrid[i - 1, 11].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[i, 11].z_down - vgrid[i, 11].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[i - 1, 11].z_down - vgrid[i - 1, 11].z_up);
                Point3D p4 = new Point3D(vgrid[i, 11].x - vgrid[i - 1, 11].x, vgrid[i, 11].y - vgrid[i - 1, 11].y,
                    vgrid[i, 11].z_down - vgrid[i - 1, 11].z_down);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[i, 11].x, vgrid[i, 11].y, vgrid[i, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_up);
                GL.glVertex3d(vgrid[i - 1, 11].x, vgrid[i - 1, 11].y, vgrid[i - 1, 11].z_down);
                GL.glEnd();

            }

            //绘制右面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[11, i - 1].x - vgrid[11, i].x, vgrid[11, i - 1].y - vgrid[11, i].y,
                    vgrid[11, i - 1].z_down - vgrid[i, 11].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(0, 0, vgrid[11, i - 1].z_down - vgrid[11, i - 1].z_up);
                Point3D p4 = new Point3D(vgrid[11, i].x - vgrid[11, i - 1].x, vgrid[11, i].y - vgrid[11, i - 1].y,
                    vgrid[11, i].z_up - vgrid[11, i - 1].z_up);
                Point3D pNormal2 = this.getNomalVector(p3, p4);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[11, i].x, vgrid[11, i].y, vgrid[11, i].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_up);
                GL.glVertex3d(vgrid[11, i - 1].x, vgrid[11, i - 1].y, vgrid[11, i - 1].z_down);
                GL.glEnd();
            }
            //绘制左面
            for (int i = 1; i <= 11; i++)
            {
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p1 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D p2 = new Point3D(0, 0, vgrid[0, i - 1].z_down - vgrid[0, i - 1].z_up);
                Point3D pNormal = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal.X, pNormal.Y, pNormal.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_down);
                GL.glEnd();
                GL.glBegin(GL.GL_TRIANGLES);
                Point3D p3 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y, 0);
                Point3D p4 = new Point3D(vgrid[0, i - 1].x - vgrid[0, i].x, vgrid[0, i - 1].y - vgrid[0, i].y,
                    vgrid[0, i - 1].z_down - vgrid[i, 0].z_up);
                Point3D pNormal2 = this.getNomalVector(p1, p2);
                GL.glNormal3d(pNormal2.X, pNormal2.Y, pNormal2.Z);
                GL.glVertex3d(vgrid[0, i].x, vgrid[0, i].y, vgrid[0, i].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_up);
                GL.glVertex3d(vgrid[0, i - 1].x, vgrid[0, i - 1].y, vgrid[0, i - 1].z_down);
                GL.glEnd();
            }
            GL.glDisable(GL.GL_COLOR_MATERIAL);
            GL.glEndList();
        }

        public void clearList()
        {
            grid.Clear();
            markerbedsInfo.Clear();
            SymbolLayerName.Clear();

        }

        //求平面法向量,遵守右手定则
        public Point3D getNomalVector(Point3D p1, Point3D p2)
        {
            Point3D result = new Point3D();
            result.X = p1.Y*p2.Z - p1.Z*p2.Y;
            result.Y = p1.Z*p2.X - p1.X*p2.Z;
            result.Z = p1.X*p2.Y - p1.Y*p2.X;
            return result;
        }
    }
}

