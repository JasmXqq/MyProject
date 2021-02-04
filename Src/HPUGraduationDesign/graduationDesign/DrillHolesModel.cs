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
    class DrillHolesModel
    {
        List<DrillHoleManager> holes = new List<DrillHoleManager> { };
        public void getScreenCoordinates()
        {
            PaintBorder pb = new PaintBorder();
            double minX = pb.getMinX();
            double minY = pb.getMinY();
            double width=pb.getWidth();
            double height=pb.getHeight();
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            string constr = sqlDataManager.getConn();
            string sql = "select zkdczl.ZKDM,zkdczl.BZCDM,zkdczl.CHLJ,zkdczl.YSBM,ZKMC.KKZBX,ZKMC.KKZBY,ZKMC.KKZBZ,ZKDCZL.CH from ZKDCZL join ZKMC on ZKMC.ZKDM=ZKDCZL.ZKDM where len(bzcdm)>1 and CHLJ is not null and YSBM is not null order by ZKDM asc";           
            using (SqlConnection conn=new SqlConnection(constr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                conn.Open();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DrillHoleManager hole = new DrillHoleManager(0,0,0);
                    hole.HoleName = dt.Rows[i][0].ToString();
                    hole.SymbolLayer = dt.Rows[i][1].ToString();
                    hole.SumThickness = -Convert.ToDouble(dt.Rows[i][2]);
                    hole.RockID = dt.Rows[i][3].ToString();
                    hole.X = (Convert.ToDouble(dt.Rows[i][4])-minX)*screenWidth/width;
                    hole.Y = (Convert.ToDouble(dt.Rows[i][5]) - minY)*screenHeight/height;
                    hole.Z = Convert.ToDouble(dt.Rows[i][6]);
                    hole.Thickness = Convert.ToDouble(dt.Rows[i][7]);
                    holes.Add(hole);
                }
                conn.Close();
            }
        }
        public void drawHoles(uint listNum)
        {
            GL.glCallList(listNum);
        }
       public void showHoles()
        {
            GL.glNewList(201,GL.GL_COMPILE);
            GL.glBegin(GL.GL_POINTS);
            GL.glPointSize(3);            
            for (int i = 0; i < holes.Count; i++)
            {
                GL.glVertex3d(holes[i].X,holes[i].Y,holes[i].Z);
            }
            GL.glEnd();
            GL.glEndList();
        }

       unsafe public void showHolesCylider()
        {
            GL.glPushMatrix();
            GL.glNewList(2000, GL.GL_COMPILE);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glColor3d(0.5, 0.5, 1);
            GL.glBegin(GL.GL_POINTS);
//             GL.glPointSize(3);
            for (int i = 0; i < holes.Count; i++)
            {
                GL.glPushMatrix();
                IntPtr pObj;
                pObj =GLU.gluNewQuadric();
                GL.glTranslated(holes[i].X, holes[i].Y, holes[i].Z - 1000);
                GLU.gluCylinder(pObj, 5, 5, 600, 20, 20);
                GL.glPopMatrix();
//                 GL.glVertex3d(holes[i].X, holes[i].Y, holes[i].Z);
            }
             GL.glEnd();
            GL.glEndList();
            GL.glPopMatrix();
        }
    }
}
