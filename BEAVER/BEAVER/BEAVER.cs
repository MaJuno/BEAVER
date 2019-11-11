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
    private void RunScript(Point3d _p, List<int> _id, List<double> _l, List<double> _w, List<double> _t, int _aggid, ref object A, ref object B, ref object C, ref object D, ref object E, ref object F)
    {
        // <Custom code>

        IList<int> ID = new List<int>(_id);
        IList<double> L = new List<double>(_l);
        IList<double> W = new List<double>(_w);
        IList<double> T = new List<double>(_t);
        int IDLength = ID.Count;

        //Create a list of wood bars
        List<element> bars = new List<element>();
        for (int i = 0; i < IDLength; i++)
        {
            element bar = new element(ID[i], L[i], W[i], T[i]);
            bars.Add(bar);
        }

        //Get Wood bars Volume
        List<double> barsVolumes = new List<double>();
        for (int i = 0; i < IDLength; i++)
        {
            double volume = bars.ElementAt(i).Volume;
            barsVolumes.Add(volume);
        }

        //Sort wood bars according to their volume
        List<element> sortedBars = bars.OrderBy(x => x.Volume).ToList();

        //Create a enumerable list of bar
        IList<element> sortedBarList = sortedBars;

        //Create the list of sorted Bars' ID
        IList<int> sortedBarId = new List<int>();
        for (int i = 0; i < IDLength; i++)
        {
            int barId = sortedBarList.ElementAt(i).Id;
            sortedBarId.Add(barId);
        }

        //Create the list of sorted volumes
        IList<double> sortedBarVolume = new List<double>();
        for (int i = 0; i < IDLength; i++)
        {
            double barVolume = sortedBarList.ElementAt(i).Volume;
            sortedBarVolume.Add(barVolume);
        }

        //Create the origin point for the first Aggregate
        var pOrigin = new Point3d(0, 0, 0);

        //Create a list of Aggregates
        IList<Aggregate> aggregateList = new List<Aggregate>();
        for (int i = 0; i < IDLength; i += 3)
        {
            Aggregate agg = new Aggregate(sortedBarList, i, ref pOrigin);
            aggregateList.Add(agg);
        }

        IList<char> OrientationList = new List<char>();
        for (int i = 0; i < aggregateList.Count; i++)
        {
            char xOr = aggregateList.ElementAt(i).ArrElements.ElementAt(0).Orientation;
            char yOr = aggregateList.ElementAt(i).ArrElements.ElementAt(1).Orientation;
            char zOr = aggregateList.ElementAt(i).ArrElements.ElementAt(2).Orientation;
            OrientationList.Add(xOr);
            OrientationList.Add(yOr);
            OrientationList.Add(zOr);
        }

        Assembly assembly = new Assembly();
        assembly.Orientation = assembly.OrientationList(aggregateList);
        assembly.AggL = aggregateList;
        assembly.AssAggL = assembly.ListAssembly(assembly.AggL, assembly.Orientation);

        IList<Point3d> p = new List<Point3d>();

        for (int i = 0; i < aggregateList.Count; i++)
        {
            Point3d p1 = assembly.NewOrigin(aggregateList, assembly.Orientation.ElementAt(i), i).ElementAt(0);
            Point3d p2 = assembly.NewOrigin(aggregateList, assembly.Orientation.ElementAt(i), i).ElementAt(1);
            p.Add(p1);
            p.Add(p2);
        }



        List<Mesh> assemblyMesh = new List<Mesh>();
        for (int i = 1; i < assembly.AggL.Count; i++)
        {
            Mesh movedMesh = assembly.AggL.ElementAt(i).AggMesh;
            movedMesh.Translate(p.ElementAt(i).X, p.ElementAt(i).Y, p.ElementAt(i).Z);
            assemblyMesh.Add(movedMesh);
        }

        //Give assembly Id
        List<string> assemblyId = new List<string>();
        for (int i = 0; i < assembly.AggL.Count; i++)
        {
            String AggId = assembly.AggL.ElementAt(i).AggName;
            assemblyId.Add(AggId);
        }

        //Create the instruction to assemble Aggregate
        //List<String> AggInstructionsList = new List<String>();
        //for (int i = 0; i < aggregateList.Count ; i++)
        //{
        //    string instruction = aggregateList.ElementAt(i).AggInstructions;
        //    AggInstructionsList.Add(instruction);
        //}



        A = assembly;
        B = assemblyMesh;
        C = aggregateList.Count;
        D = aggregateList;
        E = p;
        F = assemblyId;

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

    private IList<char> _orientation;
    public IList<char> Orientation
    {
        get { return _orientation; }
        set { _orientation = value; }
    }

    private IList<Aggregate> _AggL;
    public IList<Aggregate> AggL
    {
        get { return _AggL; }
        set { _AggL = value; }
    }

    private IList<Aggregate> _assAggL;
    public IList<Aggregate> AssAggL
    {
        get { return _assAggL; }
        set { _assAggL = value; }
    }


    //Constructor
    public Assembly()
        {
        
        }


    //Methods

    public IList<char> OrientationList(IList<Aggregate> aggregateList)
    {
        List<char> orientationList = new List<char>();
        for (int i = 0; i < aggregateList.Count; i++)
        {
            char xOr = aggregateList.ElementAt(i).ArrElements.ElementAt(0).Orientation;
            char yOr = aggregateList.ElementAt(i).ArrElements.ElementAt(1).Orientation;
            char zOr = aggregateList.ElementAt(i).ArrElements.ElementAt(2).Orientation;
            orientationList.Add(xOr);
            orientationList.Add(yOr);
            orientationList.Add(zOr);
        }
        return orientationList;
    }

    /// <summary>Displacment Vector :
    /// This method find the relative vector for each Aggregate to be moved to its correct position in the assembly
    /// </summary>
    /// <param name="agg1"></param>
    /// <param name="agg2"></param>
    /// <param name="agg3"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
    public IList<Point3d> NewOrigin(IList<Aggregate> listagg, char orientation, int index)
    {
        Point3d agg1Origin = listagg.ElementAt(index).Origin;
        Point3d agg2Origin = listagg.ElementAt(listagg.Count - 2).Origin;
        Point3d agg3Origin = listagg.ElementAt(listagg.Count - 1).Origin;
        Point3d origin = new Point3d(0, 0, 0);
        Point3d agg1Freespot = new Point3d();
        Point3d agg2Freespot = new Point3d();
        Point3d agg3Freespot = new Point3d();
        var pList = new List<Point3d>();

        if (orientation == 'X')
        {
            agg1Freespot = listagg.ElementAt(index).XFsPos;
            agg2Freespot = listagg.ElementAt(listagg.Count - 2).YFsPos;
            agg3Freespot = listagg.ElementAt(listagg.Count - 1).ZFsPos;
        }
        else if (orientation == 'Y')
        {
            agg1Freespot = listagg.ElementAt(index).YFsPos;
            agg2Freespot = listagg.ElementAt(listagg.Count - 2).ZFsPos;
            agg3Freespot = listagg.ElementAt(listagg.Count - 1).XFsPos;
        }
        else
        {
            agg1Freespot = listagg.ElementAt(index).ZFsPos;
            agg2Freespot = listagg.ElementAt(listagg.Count - 2).XFsPos;
            agg3Freespot = listagg.ElementAt(listagg.Count - 1).YFsPos;
        }

        Vector3d v1 = new Vector3d(agg1Origin.X - origin.X, agg1Origin.Y - origin.Y, agg1Origin.Z - origin.Z);
        Vector3d v2 = new Vector3d(agg1Freespot.X - agg1Origin.X, agg1Freespot.Y - agg1Origin.Y, agg1Freespot.Z - agg1Origin.Z);
        Vector3d v3 = new Vector3d(agg2Freespot.X - agg2Origin.X, agg2Freespot.Y - agg2Origin.Y, agg2Freespot.Z - agg2Origin.Z);
        Vector3d v4 = new Vector3d(agg3Freespot.X - agg3Origin.X, agg3Freespot.Y - agg3Origin.Y, agg3Freespot.Z - agg3Origin.Z);

        //Vector3d vector3D1 = new Vector3d(v1.X + v2.X + v3.X, v1.Y + v2.Y + v3.Y, v1.Z + v2.Z + v3.Z);
        agg1Origin = new Point3d(v1.X + v2.X + v3.X, v1.Y + v2.Y + v3.Y, v1.Z + v2.Z + v3.Z);
        agg2Origin = new Point3d(v1.X + v2.X + v4.X, v1.Y + v2.Y + v4.Y, v1. Z + v2.Z + v4.Z);
        //Vector3d vector3D2 = new Vector3d(v1.X + v2.X + v4.X, v1.Y + v2.Y + v4.Y, v1.Z + v2.Z + v4.Z);

        pList.Add(agg1Origin);
        pList.Add(agg2Origin);
        return pList;
    }

    public IList<Aggregate> ListAssembly (IList<Aggregate> AggregateList, IList<char> OrientationList)
    {
        IList<Aggregate> aggregates = AggregateList;
        IList<Aggregate> listAssembly = new List<Aggregate>
        {
            aggregates.ElementAt(0)
        };
        
        for (int i = 0; i<aggregates.Count; i+=2 )
        {
            listAssembly.Add(aggregates.ElementAt(i));
            listAssembly.Add(aggregates.ElementAt(i+1));
            IList<Point3d> p = NewOrigin(listAssembly, OrientationList.ElementAt(i), i);
                
            Point3d p1 = p.ElementAt(p.Count-2);
            Point3d p2 = p.ElementAt(p.Count-1);
                
            listAssembly.ElementAt(listAssembly.Count - 2).Origin = p1;
            listAssembly.ElementAt(listAssembly.Count - 1).Origin = p2;

        }
        return listAssembly;
    }
        
}


public class Aggregate
{
    //Fields

    private double pi = Math.PI;
    private Vector3d unitVectorX = new Vector3d(1, 0, 0);
    private Vector3d unitVectorY = new Vector3d(0, 1, 0);
    private Vector3d unitVectorZ = new Vector3d(0, 0, 1);
    private Point3d origin = new Point3d(0, 0, 0);

    private Mesh _aggMesh;
    public Mesh AggMesh
    {
        get { return _aggMesh; }
        set { _aggMesh = value; }
    }

    private element[] _aggArrelements;
    public element[] ArrElements
    {
        get { return _aggArrelements; }
        set { _aggArrelements = value; }
    }

    private Point3d agg_Origin;
    public Point3d Origin
    {
        get { return agg_Origin; }
        set { agg_Origin = value; }
    }

    private string agg_name;
    public string AggName
    {
        get { return agg_name; }
        set { agg_name = value; }
    }

    private int _aggXFreedom;
    public int XFreedom
    {
        get { return _aggXFreedom; }
        set { _aggXFreedom = 2; }
    }

    private int _aggYFreedom;
    public int YFreedom
    {
        get { return _aggYFreedom; }
        set { _aggYFreedom = 2; }
    }

    private int _aggZFreedom;
    public int ZFreedom
    {
        get { return _aggZFreedom; }
        set { _aggZFreedom = 2; }
    }

    private Point3d _aggXFsPos;
    public Point3d XFsPos
    {
        get { return _aggXFsPos; }
        set { _aggXFsPos = value; }
    }

    private Point3d _aggYFsPos;
    public Point3d YFsPos
    {
        get { return _aggYFsPos; }
        set { _aggYFsPos = value; }
    }

    private Point3d _aggZFsPos;
    public Point3d ZFsPos
    {
        get { return _aggZFsPos; }
        set { _aggZFsPos = value; }
    }

    private double _aggXPos;
    public double XPos
    {
        get { return _aggXPos; }
        set { _aggXPos = 0.3; }
    }

    private double _aggYPos;
    public double YPos
    {
        get { return _aggYPos; }
        set { _aggYPos = 0.3; }
    }

    private double _aggZPos;
    public double ZPos
    {
        get { return _aggZPos; }
        set { _aggZPos = 0.3; }
    }

    private string _aggInstructions;
    public string AggInstructions
    {
        get { return _aggInstructions; }
        set { _aggInstructions = value; }
    }

    //Constructor
    

    public Aggregate(IList<element> elements, int i, ref Point3d origin)
    {
        this.ArrElements = Aggregation(elements, i);
        this.AggName = "agg_" + i / 3;
        this.AggInstructions = aggregationInstruction(ArrElements, i);
        this.AggMesh = BuildAggregate(ArrElements, i, ref origin);
        this.Origin = origin;
        this.XPos = 0.3;
        this.YPos = 0.3;
        this.ZPos = 0.3;
        this.XFsPos = xFsPoint(ArrElements.ElementAt(0), Origin);
        this.YFsPos = yFsPoint(ArrElements.ElementAt(1), Origin);
        this.ZFsPos = zFsPoint(ArrElements.ElementAt(2), Origin);
    }


    //Methods

    public element[] Aggregation(IList<element> element, int i)
    {
        element[] arrelements = new element[3] { element[i], element[i + 1], element[i + 2] };
        element[i].Orientation = 'X';
        element[i+1].Orientation = 'Y';
        element[i+2].Orientation = 'Z';
        return arrelements;
    }


    ///<summary>A method to get information from the elements
    ///
    /// </summary>
    public string aggregationInstruction(element[] ArrElements, int i)
    {
       
        int xID = ArrElements.ElementAt(0).Id;
        int yID = ArrElements.ElementAt(1).Id;
        int zID = ArrElements.ElementAt(2).Id;
        char xOr = ArrElements.ElementAt(0).Orientation;
        char yOr = ArrElements.ElementAt(1).Orientation;
        char zOr = ArrElements.ElementAt(2).Orientation;
        string Name = AggName;
        string instructions = string.Format("The beaver kindly asks you to assemble elements {0}, {1} and {2} respectively as the {3}, {4} and {5} elements of the Aggregate named {6}", xID, yID, zID, xOr, yOr,zOr, Name);

        return instructions;
    }
    
    ///<summary>A method to place element along x,y,z axis
    ///
    ///</summary>
    public Mesh BuildAggregate(element[] ArrElements, int i, ref Point3d origin)
    {
        //Compute vector by global and local origin
        var vDirection =  new Vector3d(origin);
        
        //Get boxes representing each element in a Aggregate
        Mesh xMesh = ArrElements.ElementAt(0).Mesh;
        Mesh yMesh = ArrElements.ElementAt(1).Mesh;
        Mesh zMesh = ArrElements.ElementAt(2).Mesh;

        //Get translation vector for each bar
        double xv = ArrElements.ElementAt(0).Length;
        double yv = ArrElements.ElementAt(1).Length;
        double zv = ArrElements.ElementAt(2).Length;
        

        //Move bars to get them at the right position 
        xMesh.Translate(vDirection);
        yMesh.Translate(vDirection);
        zMesh.Translate(vDirection);

        //Move bars to obtain the right position along x
        xMesh.Translate(xv *-0.3, 0, 0);
        yMesh.Translate(yv *-0.3, 0, 0);
        zMesh.Translate(zv *-0.3, 0, 0);

        //Rotate bars to obtain the right position in space
        yMesh.Rotate(pi / 2, unitVectorZ, origin);
        yMesh.Rotate(pi / 2, unitVectorY, origin);
        zMesh.Rotate(-pi / 2, unitVectorX, origin);
        zMesh.Rotate(-pi / 2, unitVectorY, origin);

        //Join bars in one mesh
        List<Mesh> meshList = new List<Mesh> { xMesh, yMesh, zMesh };
        var joinedMesh = new Mesh();
        joinedMesh.Append(meshList);
        
        // return joined Mesh
        return joinedMesh;
    }

    public Point3d xFsPoint(element element, Point3d origin)
    {
        Point3d Fspot = new Point3d();
        Fspot.X = element.Length * 0.5+origin.X;
        Fspot.Y = origin.Y;
        Fspot.Z = origin.Z;

        return Fspot;
    }

    public Point3d yFsPoint(element element, Point3d origin)
    {
        Point3d Fspot = new Point3d();
        Fspot.X = origin.X;
        Fspot.Y = element.Length * 0.5+origin.Y;
        Fspot.Z = origin.Z;

        return Fspot;
    }

    public Point3d zFsPoint(element element, Point3d origin)
    {
        Point3d Fspot = new Point3d();
        Fspot.X = origin.X;
        Fspot.Y = origin.Y;
        Fspot.Z = element.Length * 0.5+origin.Z;


        return Fspot;
    }

}

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

    private Mesh ele_Mesh;
    public Mesh Mesh
    {
        get { return ele_Mesh; }
        set { ele_Mesh = value; }
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
        this.Mesh = Bars(Length, Width, Thickness);
        this.Volume = Mesh.Volume();
    }


    //Methods

    /// <summary>
    /// This function create 3D geometry representing the bar
    /// </summary>
    public Mesh Bars(double l, double w, double t)
    {
        Interval lInterval = new Interval(0, l);
        Interval wInterval = new Interval(-w,0);
        Interval tInterval = new Interval(0,t);
        Rhino.Geometry.Plane pPlane = new Plane(0, 0, 1, 0);
        Rhino.Geometry.Box box = new Box(pPlane, lInterval, wInterval, tInterval);
        var mesh = Mesh.CreateFromBox(box, 1, 1, 1);
        
        return mesh;
    }

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