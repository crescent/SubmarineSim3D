using System;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.ControlStates;
using SubjugatorSim.Entities;
using SubjugatorSim.src;

namespace SubjugatorSim
{
    public class State
    {
        public SimNode SubNode
        {
            get { return SceneWorld.Sub.SimNode; }
        }

        public SimCameraManager CameraManager { get; set; }

        public ControlState ControlState { get; set; }
        
        public MainWindow MainWindow { get; set; }
        
        public SimInputManger InputManger { get; set; }

        public Root Root { get; set; }

        public SceneManager SceneManager { get; set; }

        public PhysicsWorld PhysicsWorld { get; set; }

        public SimHydrax Hydrax { get; set; }

        public World SceneWorld { get; set; }

        public SimNode SelectedNode { get; set; }
    }
}