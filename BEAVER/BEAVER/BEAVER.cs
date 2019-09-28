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




/// <summary>
/// This class will be instantiated on demand by the Script component.
/// </summary>
public class Script_Instance : GH_ScriptInstance
{
    #region Utility functions
    /// <summary>Print a String to the [Out] Parameter of the Script component.</summary>
    /// <param name="text">String to print.</param>
    private void Print(string text) { __out.Add(text); }
    /// <summary>Print a formatted String to the [Out] Parameter of the Script component.</summary>
    /// <param name="format">String format.</param>
    /// <param name="args">Formatting parameters.</param>
    private void Print(string format, params object[] args) { __out.Add(string.Format(format, args)); }
    /// <summary>Print useful information about an object instance to the [Out] Parameter of the Script component. </summary>
    /// <param name="obj">Object instance to parse.</param>
    private void Reflect(object obj) { __out.Add(GH_ScriptComponentUtilities.ReflectType_CS(obj)); }
    /// <summary>Print the signatures of all the overloads of a specific method to the [Out] Parameter of the Script component. </summary>
    /// <param name="obj">Object instance to parse.</param>
    private void Reflect(object obj, string method_name) { __out.Add(GH_ScriptComponentUtilities.ReflectType_CS(obj, method_name)); }
    #endregion

    #region Members
    /// <summary>Gets the current Rhino document.</summary>
    private RhinoDoc RhinoDocument;
    /// <summary>Gets the Grasshopper document that owns this script.</summary>
    private GH_Document GrasshopperDocument;
    /// <summary>Gets the Grasshopper script component that owns this script.</summary>
    private IGH_Component Component;
    /// <summary>
    /// Gets the current iteration count. The first call to RunScript() is associated with Iteration==0.
    /// Any subsequent call within the same solution will increment the Iteration count.
    /// </summary>
    private int Iteration;
    #endregion

    /// <summary>
    /// This procedure contains the user code. Input parameters are provided as regular arguments, 
    /// Output parameters as ref arguments. You don't have to assign output parameters, 
    /// they will have a default value.
    /// </summary>
    private void RunScript(Point3d _p, int _id, double _l, double _w, double _t, ref object A, ref object B, ref object C, ref object D)
    {
        // <Custom code>

        elements bars = new elements(_id, _l, _w, _t);

        double volume = bars.Volume;
        Box bar3d = bars.Box;
        aggregate agg = new aggregate(bars);
        int id = bars.Id;

        //Create all wood bars
        //Create all aggregate
        //for 3n elements in stock
        //Give instructions for aggregate building
        //Create assembly
        //Give instructions for aggregate assembly
     
        A = volume;
        B = bar3d;
        C = agg;
        D = bars;
        // </Custom code>
    }

    // <Custom additional code> 
    public class assembly
    {
        //Fields

        private List<Point3d> ass_fSpot;
        public List<Point3d> fSpot
        {
            get { return ass_fSpot; }
            set { ass_fSpot = value; }
        }

        private List<Point3d> ass_nfSpot;
        public List<Point3d> nfSpot
        {
            get { return ass_nfSpot; }
            set { ass_nfSpot = value; }
        }

        private List<int> ass_Dof;
        public List<int> Dof
        {
            get { return ass_Dof; }
            set { ass_Dof = value; }
        }
        //List of freedom degrees for each points : int
        //List of aggregate


        //Constructor
        public assembly()
        {

        }
        //public assembly(list<aggregate> aggregates)
        //{
        //this.ass_aggregate = aggregates;
        //}

        //Methods

        //A method to evaluate freedom degrees of existing aggregate
        //A method to add an aggregate to the the assembly 
    }

    public class aggregate
    {
        //Fields

        private List<List<elements>> agg_listelement;
        public List<List<elements>> ListElements
        {
            get { return agg_listelement; }
            set { agg_listelement = value; }
        }

        private Point3d agg_initPos;
        public Point3d InitPos
        {
            get { return agg_initPos; }
            set { agg_initPos = value; }
        }

        private string agg_name;
        public string Name
        {
            get { return agg_name; }
            set { agg_name = value; }
        }

        private int agg_xFreedom;
        public int XFreedom
        {
            get { return agg_xFreedom; }
            set { agg_xFreedom = value; }
        }

        private int agg_yFreedom;
        public int YFreedom
        {
            get { return agg_yFreedom; }
            set { agg_yFreedom = value; }
        }

        private int agg_zFreedom;
        public int ZFreedom
        {
            get { return agg_zFreedom; }
            set { agg_zFreedom = value; }
        }

        private double agg_xFsPos;
        public double XFsPos
        {
            get { return agg_xFsPos; }
            set { agg_xFsPos = value; }
        }

        private double agg_yFsPos;
        public double YFsPos
        {
            get { return agg_yFsPos; }
            set { agg_yFsPos = value; }
        }

        private double agg_zFsPos;
        public double ZFsPos
        {
            get { return agg_zFsPos; }
            set { agg_zFsPos = value; }
        }

        
        //Constructor
        public aggregate()
        {

        }

        public aggregate(elements elements)
        {
            this.ListElements = Aggregation (elements);
        }


        //Methods

        //A method to place element along x,y,z axis
        
        //A method to get information from the elements

        //A method to give building instruction of the aggregate

        public List<List<elements>> Aggregation (elements element)
        {
            elements[] elements = { element };
            List<elements> listA = new List<elements>(elements);
            List<elements> listB = new List<elements>();
            List<List<elements>> ListC = new List<List<elements>>();
            for (int i=0; i < listA.Count; i++)
            {
                if (listB.Count < 3)
                {
                    listB.Add(listA[0]);
                    listA.RemoveAt(0);
                }
                else
                {
                    ListC.Add(listB);
                    listB.Clear();
                }
            }
            return ListC;
        }

    }

    public class elements
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

        private Box ele_Box;
        public Box Box
        {
            get { return ele_Box; }
            set { ele_Box = value; }
        }

        
        //Constructor
        public elements()
        {

        }

        public elements(int id, double l, double w, double t)
        {
            this.Id = id;
            this.Length = l;
            this.Width = w;
            this.Thickness = t;
            this.Box = Bars(Length, Width, Thickness);
            this.Volume = Box.Volume;
        }


        //Methods

        /// <summary>
        /// This function create 3D geometry representing the bar
        /// </summary>
        public Box Bars(double l, double w, double t)
        {
            Interval lInterval = new Interval(0, l);
            Interval wInterval = new Interval(0, w);
            Interval tInterval = new Interval(0, t);
            Rhino.Geometry.Plane pPlane = new Plane(1, 0, 0, 0);
            Rhino.Geometry.Box box = new Box(pPlane, lInterval, wInterval, tInterval);
            return box;
        }

    }

    // utilities functions

    /// <summary>
    /// This Function maps a value from a source domain to a destination domain. Written by Alessio Erioli from Co-De-It
    /// </summary>
    /// <param name="val">the value</param>
    /// <param name="fromMin">low value of source domain</param>
    /// <param name="fromMax">high value of source domain</param>
    /// <param name="toMin">low value of destination domain</param>
    /// <param name="toMax">high value of destination domain</param>
    /// <returns>the remapped value</returns>
    public double Map(double val, double fromMin, double fromMax, double toMin, double toMax)
    {
        return toMin + (val - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

    /// <summary>
    /// This function intend to vizualise wood bars stock.
    /// </summary>

    ///<summary> This function intend to compare list and order one according to the other.
    ///
    /// </summary>


    // </Custom additional code>    

  private List<string> __err = new List<string>(); //Do not modify this list directly.
  private List<string> __out = new List<string>(); //Do not modify this list directly.
  private RhinoDoc doc = RhinoDoc.ActiveDoc;       //Legacy field.
  private IGH_ActiveObject owner;                  //Legacy field.
  private int runCount;                            //Legacy field.
  
  public override void InvokeRunScript(IGH_Component owner, object rhinoDocument, int iteration, List<object> inputs, IGH_DataAccess DA)
  {
    //Prepare for a new run...
    //1. Reset lists
    this.__out.Clear();
    this.__err.Clear();

    this.Component = owner;
    this.Iteration = iteration;
    this.GrasshopperDocument = owner.OnPingDocument();
    this.RhinoDocument = rhinoDocument as Rhino.RhinoDoc;

    this.owner = this.Component;
    this.runCount = this.Iteration;
    this. doc = this.RhinoDocument;

    //2. Assign input parameters
        object x = default(object);
    if (inputs[0] != null)
    {
      x = (object)(inputs[0]);
    }

    object y = default(object);
    if (inputs[1] != null)
    {
      y = (object)(inputs[1]);
    }



    //3. Declare output parameters
      object A = null;


    //4. Invoke RunScript
    RunScript(x, y, ref A);
      
    try
    {
      //5. Assign output parameters to component...
            if (A != null)
      {
        if (GH_Format.TreatAsCollection(A))
        {
          IEnumerable __enum_A = (IEnumerable)(A);
          DA.SetDataList(1, __enum_A);
        }
        else
        {
          if (A is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(1, (Grasshopper.Kernel.Data.IGH_DataTree)(A));
          }
          else
          {
            //assign direct
            DA.SetData(1, A);
          }
        }
      }
      else
      {
        DA.SetData(1, null);
      }

    }
    catch (Exception ex)
    {
      this.__err.Add(string.Format("Script exception: {0}", ex.Message));
    }
    finally
    {
      //Add errors and messages... 
      if (owner.Params.Output.Count > 0)
      {
        if (owner.Params.Output[0] is Grasshopper.Kernel.Parameters.Param_String)
        {
          List<string> __errors_plus_messages = new List<string>();
          if (this.__err != null) { __errors_plus_messages.AddRange(this.__err); }
          if (this.__out != null) { __errors_plus_messages.AddRange(this.__out); }
          if (__errors_plus_messages.Count > 0) 
            DA.SetDataList(0, __errors_plus_messages);
        }
      }
    }
  }
}