using System;
using Mogre;

namespace SubjugatorSim.Code
{
    internal class SimResourceManger
    {
        public void Init()
        {
            var manager = ResourceGroupManager.Singleton;

            manager.AddResourceLocation("resources/meshes", "FileSystem", "General");
            manager.AddResourceLocation("resources/textures", "FileSystem", "General");
            manager.AddResourceLocation("resources/materials", "FileSystem", "General");
            
            manager.AddResourceLocation("resources/hydrax", "FileSystem", "Hydrax");

            manager.AddResourceLocation("C:/Dev/MogreSDK/Media/packs/OgreCore.zip", "Zip", "Bootstrap");

            manager.InitialiseAllResourceGroups();
        }
    }
}