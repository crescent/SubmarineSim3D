using Mogre;

namespace SubjugatorSim.Code
{
    class SceneCreator
    {
        public void Init(State state)
        {
            state.SceneManager.DestroyEntity("floor");
            state.SceneManager.DestroyEntity("Buoy");
            state.SceneManager.DestroyEntity("Path");
            state.SceneManager.DestroyLight("Light");

            state.SceneManager.AmbientLight = new ColourValue(0.25f, 0.25f, 0.25f);
            
            state.SceneManager.SetSkyDome(true, "Examples/CloudySky", 5, 8,100);

            // Define a floor plane mesh
            Plane p;
            p.normal = Vector3.UNIT_Y;
            p.d = 20;
            MeshManager.Singleton.CreatePlane("FloorPlane", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, p, 20000F, 20000F, 20, 20, true, 1, 2000F, 2000F, Vector3.UNIT_Z);

            Entity ent;
            // CreateFreeState an entity (the floor)
            ent = state.SceneManager.CreateEntity("floor", "FloorPlane");
            ent.SetMaterialName("Examples/RustySteel");

            // Attach to child of root node, better for culling (otherwise bounds are the combination of the 2)
            state.SceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(ent);


            state.SceneManager.SetFog(FogMode.FOG_EXP, ColourValue.White, 0.0002F); 

            LogManager.Singleton.SetDefaultLog(new Log("blah.log", true));
            LogManager.Singleton.SetLogDetail(LoggingLevel.LL_BOREME);


            Entity buoy = state.SceneManager.CreateEntity("Buoy", @"Buoy.mesh");
            Entity path = state.SceneManager.CreateEntity("Path", @"PathSegment.mesh");

            SceneNode buoyNode = state.SceneManager.RootSceneNode.CreateChildSceneNode();

            SceneNode pathNode = state.SceneManager.RootSceneNode.CreateChildSceneNode();
//            Entity transdec = state.SceneManager.CreateEntity("Transdec", @"Transdec.mesh");
//            SceneNode transdecNode = state.SceneManager.RootSceneNode.CreateChildSceneNode();

//            transdecNode.Position = new Vector3(0, -10, 0);
            pathNode.Position = new Vector3(0, 0, 10);

            buoyNode.Translate(0, -10, 0);
            buoyNode.AttachObject(buoy);
            
            pathNode.Pitch(Math.HALF_PI);
            pathNode.Translate(0,-10,0);
            pathNode.AttachObject(path);
//            transdecNode.AttachObject(transdec);
 
            Light light = state.SceneManager.CreateLight("Light");
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Vector3(250, 150, 250);
            light.DiffuseColour = ColourValue.White;
            light.SpecularColour = ColourValue.White;

            //state.SubNode.SceneNode.AttachObject(state.CameraManager.Camera2); 


        }
    }
}