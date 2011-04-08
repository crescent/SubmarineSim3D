using System;
using System.Windows.Forms;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.ControlStates;
using SubjugatorSim.src;

namespace SubjugatorSim
{
    public class Sim
    {
        private State State { get; set; }

        public void Run()
        {
            InitializeOgre();

            State.MainWindow.Show();

            while (State.Root != null && State.Root.RenderOneFrame())
                Application.DoEvents();
        }

        /// <summary>
        /// Initializes ogre and shows the splash screen as it loads.
        /// </summary>
        public void InitializeOgre()
        {
            CreateCore();

            new SimCameraManager().Init(State);
            new SimResourceManger().Init();

            new SimInputManger().Init(State);
            new EditorControlState().Init(State);

            //new PhysicsWorld().Init(State);

            State.MainWindow.Disposed += Disposed;
        }

        private void CreateCore()
        {
            State = new State();
            State.Root = CreateRoot();

            State.MainWindow = new MainWindow(State);
            State.MainWindow.Init();

            State.SceneManager = CreateSceneManager();
        }

        private SceneManager CreateSceneManager()
        {
            return State.Root.CreateSceneManager(SceneType.ST_GENERIC, "Main State.SceneManager");
        }

        private Root CreateRoot()
        {
            var root = new Root();
            root.RenderSystem = root.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
            root.Initialise(false);
            return root;
        }

        private void Disposed(object sender, EventArgs e)
        {
            // Hack to call hydrax dispose before root dispose
            if(State.Hydrax != null) State.Hydrax.Disposed(null,null);

            State.Root.Dispose();
            State.Root = null;
        }
    }
}