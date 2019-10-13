using Grasshopper.Kernel;
using Rhino.Geometry;
using System;

namespace BEAVER
{
    public class Element : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Element()
          : base("Elements", "Elem",
              "Create Beaver Elements",
              "BEAVER", "Build")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("_identification", "_id", "The Id Of the bar in the stock", GH_ParamAccess.item);
            pManager.AddNumberParameter("_Length", "_L", "The length of the bar in the stock", GH_ParamAccess.item);
            pManager.AddNumberParameter("_Width", "_W", "The width of the bar in the stock", GH_ParamAccess.item);
            pManager.AddNumberParameter("_Thickness", "_T", "The thickness of the bar in the stock", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBoxParameter("Bar", "Bar", "The 3D representation of the bar in stock", GH_ParamAccess.item);
            pManager.AddNumberParameter("Volume", "Vol", "The Volume of the bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Element", "Element", "Instance of element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Input Declaration

            int _id = 0;
            double _l = double.NaN;
            double _w = double.NaN;
            double _t = double.NaN;

            DA.GetData(0, ref _id);
            DA.GetData(1, ref _l);
            DA.GetData(2, ref _w);
            DA.GetData(3, ref _t);

            //What the code actually do
            element element = new element(_id, _l, _w, _t);

            //Output Declaration
            DA.SetData(0, element.Box);
            DA.SetData(1, element.Volume);
            DA.SetData(2, element);
        }

        ///<summary>Class
        ///Class
        /// </summary>
        public class element
        {
            //Fields

            private int ele_Id;

            public int Id
            {
                get { return ele_Id; }
                set { ele_Id = value; }
            }

            private double ele_Length;

            public double Length
            {
                get { return ele_Length; }
                set { ele_Length = value; }
            }

            private double ele_Width;

            public double Width
            {
                get { return ele_Width; }
                set { ele_Width = value; }
            }

            private double ele_Thickness;

            public double Thickness
            {
                get { return ele_Thickness; }
                set { ele_Thickness = value; }
            }

            private double ele_Volume;

            public double Volume
            {
                get { return ele_Volume; }
                set { ele_Volume = value; }
            }

            private char ele_Orientation;
            public char Orientation
            {
                get { return ele_Orientation; }
                set { ele_Orientation = value; }
            }

            private Box ele_Box;

            public Box Box
            {
                get { return ele_Box; }
                set { ele_Box = value; }
            }

            //Constructor
            public element()
            {
            }

            public element(int id, double l, double w, double t)
            {
                Id = id;
                Length = l;
                Width = w;
                Thickness = t;
                Box = Bars(Length, Width, Thickness);
                Volume = Box.Volume;
            }

            //Methods

            /// <summary>
            /// This function create 3D geometry representing the bar
            /// </summary>
            public Box Bars(double l, double w, double t)
            {
                var lInterval = new Interval(0, l);
                var wInterval = new Interval(0, w);
                var tInterval = new Interval(0, t);
                var pPlane = new Plane(1, 0, 0, 0);
                var box = new Box(pPlane, lInterval, wInterval, tInterval);
                return box;
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.logo;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("94997cd2-e40d-4529-8e43-b354c9ce07ef"); }
        }
    }
}