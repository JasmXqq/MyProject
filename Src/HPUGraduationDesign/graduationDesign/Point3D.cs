using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace graduationDesign
{
    class Point3D
    {
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private double _X;
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }
        private double _Y;
        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        private double _Z;
        public double Z
        {
            get { return _Z; }
            set { _Z = value; }
        }
        //无参构造函数
        public Point3D()
        { 
        
        }
        //三个参数X,Y,Z的构造函数
        public Point3D(double x, double y, double z)
        {
            this._X = x;
            this._Y = y;
            this._Z = z;
        }
    }

    
}
