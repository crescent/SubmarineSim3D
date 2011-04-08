using System;
using System.Collections.Generic;
using System.Text;
using MHydrax;
using Mogre;

namespace SubjugatorSim.src
{
    public class SimHydrax
    {
        private MHydrax.MHydrax hydrax;

        public void Init(State state)
        {
            if (state.Hydrax != null) return; // Hack for Opening

            state.Hydrax = this;

            ChangeSkyDomeToMesh(state);

            hydrax = new MHydrax.MHydrax(state.SceneManager, state.CameraManager.Camera, state.CameraManager.Viewport);

            CreateHydrax(state);

            // Must be called after frame ends
            state.Root.FrameEnded += Update;

            // Must be disposed before Root
            // state.MainWindow.Disposed+=Disposed;
        }

        private void CreateHydrax(State state)
        {
            var mSimpleGrid = new MSimpleGrid(hydrax, new MPerlin(), MMaterialManager.MNormalMode.NM_VERTEX);
            mSimpleGrid.Options = new MSimpleGrid.MOptions(10, new MSize(50,50),2,false,false,0);
            hydrax.SetModule(mSimpleGrid);

            state.Hydrax.hydrax.LoadCfg("HydraxDemo.hdx");
            state.Hydrax.hydrax.DepthLimit = 8;
            hydrax.Position = new Vector3(0, -9f, 0);

            hydrax.Create();
        }

        private void ChangeSkyDomeToMesh(State State)
        {
            State.SceneManager.SetSkyDome(false, "Examples/CloudySky");

            State.SceneManager.AmbientLight = new ColourValue(0.2F, 0.2F, 0.2F);
            var plane = new Plane {normal = Vector3.NEGATIVE_UNIT_Y, d = 200};
            MeshManager.Singleton.CreateCurvedPlane("HydraxSkyPlane", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 20000F, 20000F, 20000f, 20, 20, true, 1, 8, 8, Vector3.NEGATIVE_UNIT_Z);
            var skyEntity = State.SceneManager.CreateEntity("HydraxSky", "HydraxSkyPlane");
            skyEntity.SetMaterialName("SlowCloudySky");
            State.SceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(skyEntity);
        }

        public bool Update(FrameEvent e)
        {
            hydrax.Update(e.timeSinceLastFrame);
            return true;
        }

        public void Disposed(object sender, EventArgs e)
        {
            hydrax.Dispose();
            hydrax = null;
        }
    }
}
