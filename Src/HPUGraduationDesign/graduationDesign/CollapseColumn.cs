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
    class CollapseColumn
    {
        /// <summary>
        /// 陷落柱的点信息
        /// </summary>
        struct CInfo
        {
            public double X;
            public double Y;
        }
        struct trigle
        {
            public CInfo p1;
            public CInfo p2;
            public CInfo p3;
        }
        //该列表存放了陷落柱的信息
        List<CInfo> Cinfo = new List<CInfo> { };
        /// <summary>
        /// 存储陷落柱信息
        /// </summary>
        public void getCInfo()
        {
            string constr = sqlDataManager.getConn();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string sql = "select X,Y from CollapseColumnPoint ";
                SqlCommand cmd = new SqlCommand(sql, conn);              
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(ds);
                dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CInfo cinfo= new CInfo();
                    cinfo.X = Convert.ToDouble(dt.Rows[i][4]);
                    cinfo.Y = Convert.ToDouble(dt.Rows[i][5]);
                    Cinfo.Add(cinfo);
                }
                conn.Close();
            }
        }
        //Delaunay三角网（TIN算法）
          public void delaunay(string cName)
        {
          
        }

        public void draw()
        {
            GL.glNewList(203, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(1, 0.1, 1);
        }
        public void drawCollapseColumn(uint listNum)
        {
            GL.glCallList(listNum);
        }
    }
}
