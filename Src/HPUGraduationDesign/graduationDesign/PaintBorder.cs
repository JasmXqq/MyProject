using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WGLAV5;
using System.Timers;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace graduationDesign
{
     public  class PaintBorder
    {
        List<InflectionPoint> borderPoints = new List<InflectionPoint> { };

        public void Draw(uint listNumber)
        {
            GL.glCallList(listNumber);
        }
        public  void showBorder()
        {
            GL.glNewList(200, GL.GL_COMPILE);
            GL.glBegin(GL.GL_LINE_LOOP);
            GL.glColor3ub(100,50,200);
            for (int i = 0; i < borderPoints.Count; i++)
            {
                GL.glVertex3d(borderPoints[i].X,borderPoints[i].Y,borderPoints[i].Z);
            }
            GL.glEnd();
            GL.glBegin(GL.GL_LINE_LOOP);
            for (int i = 0; i < borderPoints.Count; i++)
            {
                GL.glVertex3d(borderPoints[i].X, borderPoints[i].Y, borderPoints[i].Z + 650);
            }
            GL.glEnd();
            GL.glBegin(GL.GL_LINES);
            for (int i = 0; i < borderPoints.Count; i++)
            {
                GL.glVertex3d(borderPoints[i].X,borderPoints[i].Y,borderPoints[i].Z);
                GL.glVertex3d(borderPoints[i].X, borderPoints[i].Y, borderPoints[i].Z + 650);
            }
            GL.glEnd();
            GL.glEndList();
        }
        public double getMinX()
        {
            string constr=sqlDataManager.getConn();
            double MinX;
            using (SqlConnection conn=new SqlConnection(constr))
            {
                string sql = "select top 1 X from GDZB order by X asc";
                SqlCommand cmd = new SqlCommand(sql,conn);
                conn.Open();
                MinX = Convert.ToDouble(cmd.ExecuteScalar());
                conn.Close();
            }
            return MinX;
        }
        public double getMinY()
        {
            string constr = sqlDataManager.getConn();
            double MinY;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = "select top 1 Y from GDZB order by Y asc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                MinY = Convert.ToDouble(cmd.ExecuteScalar());
                conn.Close();
            }
            return MinY;
        }
        public double getMaxX()
        {
            string constr = sqlDataManager.getConn();
            double Max;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = "select top 1 X from GDZB order by X desc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                Max = Convert.ToDouble(cmd.ExecuteScalar());
                conn.Close();
            }
            return Max;
        }
        public double getMaxY()
        {
            string constr = sqlDataManager.getConn();
            double Max;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = "select top 1 Y from GDZB order by Y desc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                Max = Convert.ToDouble(cmd.ExecuteScalar());
                conn.Close();
            }
            return Max;
        }
        public double getWidth()
        {
            return this.getMaxX() - this.getMinX();
        }
        public double getHeight()
        {
            return this.getMaxY() - this.getMinY();
        }
        public void getScreenCoordinates()
        {
            string constr = sqlDataManager.getConn();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = Screen.PrimaryScreen.Bounds.Height;
                string sql = "select ID,X,Y from GDZB";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                conn.Open();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    InflectionPoint pt = new InflectionPoint(0,0,0);
                    //得到相对坐标
                    double x = Convert.ToDouble(dt.Rows[i][1])-this.getMinX();
                    double y = Convert.ToDouble(dt.Rows[i][2]) - this.getMinY();
                   // double z = Convert.ToDouble(dt.Rows[i][2]);
                    double z = 0;
                    //获取屏幕坐标
                    x = x * screenWidth / this.getWidth();
                    y = y * screenHeight / this.getHeight();
                    pt.ID = Convert.ToInt32(dt.Rows[i][0]);
                    pt.X = x;
                    pt.Y = y;
                    pt.Z= z;
                    borderPoints.Add(pt);
                }
                conn.Close();
            }
        }
    }
}
