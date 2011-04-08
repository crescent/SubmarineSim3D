using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.src;

namespace SubjugatorSim.Entities
{
    public class WorldObject : BaseEntity, IPropertyGridEntity
    {
        private string mesh;
        public static Random Random = new Random();

        //Vector3 Origin { get; set; }

        [Editor(typeof (FileNameEditor), typeof (UITypeEditor))]
        public string Mesh
        {
            get { return mesh; }
            set
            {
                mesh = value;
                RecreateSceneNode();
            }
        }

        #region IPropertyGridEntity Members

        public string Name { get; set; }

        public WorldObject(State state1, string name) : base(state1)
        {
            Name = name;
        }

        public IPropertyGridEntity CreateInstance()
        {
            Position = state.CameraManager.CameraNode.Position + 
                state.CameraManager.CameraNode.Orientation * new Vector3(0, 0, -10);
            Orientation = Quaternion.IDENTITY;
            Scale = 1;

            RecreateSceneNode();
            return this;
        }

        public bool Enabled
        {
            get { return true; }
        }

        #endregion

        private void RecreateSceneNode()
        {
            if (SimNode != null)
            {
                state.SceneManager.RootSceneNode.RemoveChild(base.SimNode.SceneNode);
                SimNode.SceneNode.RemoveAndDestroyAllChildren();
            }

            mesh = string.IsNullOrEmpty(mesh) ? @"pathsegment.mesh" : new FileInfo(mesh).Name;
            var sceneEntity = state.SceneManager.CreateEntity(Name + Random.Next(), mesh);
            var sceneNode = state.SceneManager.RootSceneNode.CreateChildSceneNode();
            sceneNode.AttachObject(sceneEntity);

            sceneNode.Position = Position;
            sceneNode.Orientation = Orientation;
            sceneNode.Scale(Scale, Scale, Scale);

            SimNode = new SimNode(sceneNode);
        }

        public override void CreateSimNode()
        {
            RecreateSceneNode();
        }
    }
}