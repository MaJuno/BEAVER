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
    private void RunScript(List<Point3d> _p, List<int> _id, List<double> _l, List<double> _w, List<double> _t, ref object A, ref object B, ref object C)
    {
        // <Custom code>
    

     
        A = 0;
        B = 0;
        C = 0;
        // </Custom code>
    }

    // <Custom additional code> 
    public class assembly
    {
        //Fields
        //Constructor
        public assembly()
        {

        }
        //Methods
    }

    public class aggregate
    {
        //Fields
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

        private List<elements> agg_elements;
        public List<elements> Elements
        {
            get { return agg_elements; }
            set { agg_elements = value; }
        }
        //Constructor

        public aggregate()
        {

        }

        public aggregate(List<elements> elements)
        {
            this.elements = elements;
        }
        //Methods

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
        private double ele_length;
        public double Length
        {
            get { return ele_length; }
            set { ele_length = value; }
        }
        private double ele_width;
        public double Width
        {
            get { return ele_width; }
            set { ele_width = value; }
        }
        private double ele_thickness;
        public double Thickness
        {
            get { return ele_thickness; }
            set { ele_thickness = value; }
        }
        
        private double ele_Pos;
        public double XPos
        {
            get { return ele_Pos; }
            set { ele_Pos = value; }
        }
        private double ele_yPos;
        public double YPos
        {
            get { return ele_yPos; }
            set { ele_yPos = value; }
        }
        private double ele_zPos;
        public double ZPos
        {
            get { return ele_zPos; }
            set { ele_zPos = value; }
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
        }

        
        //Methods


    }

    // utilities functions

    /// <summary>
    /// This Function maps a value from a source domain to a destination domain
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

    public double Volume(double l, double w, double t)
    {
        return l * w * t;
    }

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