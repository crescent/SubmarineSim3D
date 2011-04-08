using System;
using System.Windows.Forms;
using Mogre;
using SubjugatorSim.Code;
using Math=Mogre.Math;

namespace SubjugatorSim
{
    public class SimCameraManager
    {
        public SimNode CameraNode;
        public SimNode CameraChildNode;
        public Camera Camera2 { get; set; }

        public Camera Camera { get; protected set; }
        public Viewport Viewport { get; protected set; }

        public SimCameraManager Init(State state)
        {
            state.CameraManager = this;
            var misc = new NameValuePairList();
            misc["externalWindowHandle"] = state.MainWindow.Handle.ToString();
            RenderWindow = state.Root.CreateRenderWindow("Autumn main RenderWindow", 0, 0, false, misc);

            CreateCamera(state);

            return this;
        }

        private void CreateCamera(State state)
        {
            Camera = state.SceneManager.CreateCamera("MainCamera");
            Camera.NearClipDistance = 0.1F;

            Viewport = RenderWindow.AddViewport(Camera, 0);

            Camera.AspectRatio = (float)Viewport.ActualWidth / Viewport.ActualHeight;
            //Viewport.SetDimensions(window.Left, window.Top, window.Width, window.Height);

            Camera2 = state.SceneManager.CreateCamera("MainCamera2");
            Camera2.NearClipDistance = 0.1F;
            //Camera2.Yaw(Math.HALF_PI);

            ResetCamera(state);
        }

        public void ResetCamera(State state)
        {
            Camera.Position = Vector3.ZERO;
            Camera.Orientation = Quaternion.IDENTITY;

            CameraNode = new SimNode(state.SceneManager.RootSceneNode.CreateChildSceneNode());
            CameraChildNode = new SimNode(CameraNode.SceneNode, Camera);
        }

        public void ToggleViewport()
        {
            if(Viewport2 == null)
            {
                Viewport2 = RenderWindow.AddViewport(Camera2,1);
                Viewport2.SetDimensions(0, 0, 0.25F, 0.25F * Viewport.ActualWidth / Viewport.ActualHeight);
                Viewport2.OverlaysEnabled = false;
                Camera2.AspectRatio = (float)Viewport2.ActualWidth / Viewport2.ActualHeight;
            }
            else
            {
                RenderWindow.RemoveViewport(1);
                Viewport2 = null;
            }
        }

        protected Viewport Viewport2 { get; set; }

        public RenderWindow RenderWindow { get; set; }
    }
}