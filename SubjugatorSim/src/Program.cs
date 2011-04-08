using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Mogre;

namespace SubjugatorSim.Code
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                var sim = new Sim();
                sim.Run();
            }
            catch (SEHException)
            {
                if (OgreException.IsThrown)
                    MessageBox.Show(OgreException.LastException.FullDescription, "An Ogre exception has occurred!");
                else
                    throw;
            }
        }
    }
}