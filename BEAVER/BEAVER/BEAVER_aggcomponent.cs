using System;
using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

// <Custom using>

// </Custom using>


namespace BEAVER
{
    public class aggregate
    {
        //Fields 

        private string agg_name;
        public string Name
        {
            get { return agg_name; }
            set { agg_name = value; }
        }
        private int agg_Id;
        public int Id
        {
            get { return agg_Id; }
            set { agg_Id = value; }
        }
        private double agg_length;
        public double Length
        {
            get { return agg_length; }
            set { agg_length = value; }
        }
        private double agg_width;
        public double Width
        {
            get { return agg_width; }
            set { agg_width = value; }
        }
        private double agg_thickness;
        public double Thickness
        {
            get { return agg_thickness; }
            set { agg_thickness = value; }
        }
        private int agg_xFreedom;
        public int XFreedom
        {
            get { return agg_xFreedom; }
            set { agg_xFreedom = 2; }
        }
        private int agg_yFreedom;
        public int YFreedom
        {
            get { return agg_yFreedom; }
            set { agg_yFreedom = 2; }
        }
        private int agg_zFreedom;
        public int ZFreedom
        {
            get { return agg_zFreedom; }
            set { agg_zFreedom = 2; }
        }
        private double agg_xPos;
        public double XPos
        {
            get { return agg_xPos; }
            set { agg_xPos = value; }
        }
        private double agg_yPos;
        public double YPos
        {
            get { return agg_yPos; }
            set { agg_yPos = value; }
        }
        private double agg_zPos;
        public double ZPos
        {
            get { return agg_zPos; }
            set { agg_zPos = value; }
        }

        //Constructor

        public aggregate()
        {

        }

        //Methods

        string aggregateName(aggregate agg)
        {
            return agg.Name;
        }
    }
}