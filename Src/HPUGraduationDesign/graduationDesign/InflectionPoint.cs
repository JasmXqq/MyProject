using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace graduationDesign
{
    class InflectionPoint : Point3D
    {
        //拐点代号
        private string _num;
        public string Num
        {
            get { return _num; }
            set { _num = value; }
        }
        public InflectionPoint(double x, double y, double z)
            : base(x, y, z)
        {

        }
        public void getXY(string GDName)
        {
            this._num = GDName;
            string constr = sqlDataManager.getConn();
            using (SqlConnection conn=new SqlConnection(constr))
            {
                string sql = "select X,Y from GDZB where DH=@GDName";
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.Add(new SqlParameter("@GDName",GDName));
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    this.X = Convert.ToDouble(reader[0]);
                    this.Y = Convert.ToDouble(reader[1]);
                }
                conn.Close();
            }
        }
    }
}
