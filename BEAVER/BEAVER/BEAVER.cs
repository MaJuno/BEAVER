using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

// <Custom using>

using System.Linq;

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
    private void RunScript(Point3d _p, List<int> _id, List<double> _l, List<double> _w, List<double> _t, ref object A, ref object B, ref object C, ref object D, ref object E)
    {
        // <Custom code>

        IList<int> ID = new List<int>(_id);
        IList<double> L = new List<double>(_l);
        IList<double> W = new List<double>(_w);
        IList<double> T = new List<double>(_t);
        int IDLength = ID.Count;

        List<element> bars = new List<element>();
        for (int i = 0; i < IDLength; i+=1)
        {
            element bar = new element(ID[i], L[i], W[i], T[i]);
            bars.Add(bar);
        }

        List<double> barsVolumes = new List<double>();
        for (int i = 0; i < IDLength; i++)
        {
            double volume = bars.ElementAt(i).Volume;
            barsVolumes.Add(volume);
        }


        IList<element> barList = bars;

        barList.OrderBy(i => barsVolumes);

        IList<int> orderedID = new List<int>();
            for (int i = 0; i<IDLength ;i++)
        {
            int id = bars.ElementAt(i).Id;
            orderedID.Add(id);
        }

        IList<aggregate> aggregateList = new List<aggregate>();
        
        
        for (int i = 0; i < IDLength-1; i+=3)
        {
            aggregate agg = new aggregate(barList, i);
            aggregateList.Add(agg);
        }

        IList<string> aggNameList = new List<string>();
        for (int i = 0; i<aggregateList.Count ; i++ )
        {
            aggNameList.Add(aggregateList.ElementAt(i).Name);
        }

        A = bars.Count;
        B = IDLength;
        C = aggregateList;
        D = barsVolumes;
        E = aggNameList;

        // </Custom code>
    }




}

    // <Custom additional code> 
    public class Assembly
    {
        //Fields

        private List<Point3d> _assFSpot;
        public List<Point3d> FSpot
        {
            get { return _assFSpot; }
            set { _assFSpot = value; }
        }

        private List<Point3d> _assNfSpot;
        public List<Point3d> NfSpot
        {
            get { return _assNfSpot; }
            set { _assNfSpot = value; }
        }

        private List<int> _assDof;
        public List<int> Dof
        {
            get { return _assDof; }
            set { _assDof = value; }
        }
        //List of freedom degrees for each points : int
        //List of aggregate

        //Constructor
        public Assembly()
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

    private element[] agg_Arrelements;
    public element[] ArrElements
    {
        get { return agg_Arrelements; }
        set { agg_Arrelements = value; }
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

    private int _aggXFreedom;
    public int XFreedom
    {
        get { return _aggXFreedom; }
        set { _aggXFreedom = 3; }
    }

    private int _aggYFreedom;
    public int YFreedom
    {
        get { return _aggYFreedom; }
        set { _aggYFreedom = 3; }
    }

    private int _aggZFreedom;
    public int ZFreedom
    {
        get { return _aggZFreedom; }
        set { _aggZFreedom = 3; }
    }

    private double _aggXFsPos;
    public double XFsPos
    {
        get { return _aggXFsPos; }
        set { _aggXFsPos = value; }
    }

    private double _aggYFsPos;
    public double YFsPos
    {
        get { return _aggYFsPos; }
        set { _aggYFsPos = value; }
    }

    private double _aggZFsPos;
    public double ZFsPos
    {
        get { return _aggZFsPos; }
        set { _aggZFsPos = value; }
    }


    //Constructor
    public aggregate()
    {

    }

    public aggregate(IList<element> elements, int i)
    {
        this.ArrElements = Aggregation(elements, i);
        this.Name = "agg_" + i/3;
        
    }


    //Methods

    public element[] Aggregation(IList<element> element, int i)
    {
        element[] arrelements = new element[3] { element[i], element[i + 1], element[i + 2] };
        return arrelements;
    }

    //A method to place element along x,y,z axis

    //A method to get information from the elements

    //A method to give building instruction of the aggregate




    
}
/// <summary> Nested Class elements
/// Nested Class elements
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

    private int ele_Orientation;
    public int Orientation
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
//public double Map(double val, double fromMin, double fromMax, double toMin, double toMax)
//{
//    return toMin + (val - fromMin) * (toMax - toMin) / (fromMax - fromMin);
//}

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