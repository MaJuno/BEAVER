using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace BEAVER
{
    public class BEAVERInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Beaver";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Beaver is, with human, the only known mammal to transform deeply its environment with aim at enhancing and enasuring its life and survival ";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("21369d85-dca0-4fa6-b850-b1814f4aa6ad");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Matthieu Brossette";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "matthieu@atelierjuno.com";
            }
        }
    }
}
