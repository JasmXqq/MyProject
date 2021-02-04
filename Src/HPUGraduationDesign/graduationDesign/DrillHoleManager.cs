using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace graduationDesign
{
    
    //钻孔类
    class DrillHoleManager : Point3D
    {
        //钻孔名称
        private string _holeName;
        public string HoleName
        {
            get { return _holeName; }
            set { _holeName = value; }
        }
        private double _depth;  //钻孔深度
        public double Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private int _geoNum;//钻孔穿过地层的数量

        public int GeoNum
        {
            get { return _geoNum; }
            set { _geoNum = value; }
        }
        private string _symbolLayer; //标志层代码

        public string SymbolLayer
        {
            get { return _symbolLayer; }
            set { _symbolLayer = value; }
        }
        private double _sumThickness; //累计层厚

        public double SumThickness
        {
            get { return _sumThickness; }
            set { _sumThickness = value; }
        }
        private double _thickness;

        public double Thickness
        {
            get { return _thickness; }
            set { _thickness = value; }
        }
        private string _rockID; //岩石编码

        public string RockID
        {
            get { return _rockID; }
            set { _rockID = value; }
        }
        /// <summary>
        /// 构造函数  
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>   
        public DrillHoleManager(double x, double y, double z) : base(x, y, z)
        {

        }
    }
}
