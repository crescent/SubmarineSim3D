using System;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.src;
using Math = Mogre.Math;

namespace SubjugatorSim.Entities
{
    public class Sub : BaseEntity, IPropertyGridEntity
    {
        public Sub(State state) : base(state)
        {
        }

        private bool enabled;

        #region IPropertyGridEntity Members

        public string Name
        {
            get { return "Sub"; }
        }

        public IPropertyGridEntity CreateInstance()
        {
            CreateSimNode();
            return this;
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        #endregion

        public override void CreateSimNode()
        {
            enabled = true;

            state.SceneManager.DestroyEntity("SubNode");
            state.SceneManager.DestroyLight("subLight");

            Entity sub = state.SceneManager.CreateEntity("SubNode", @"MockSub.mesh");
            var sceneNode = new SceneNode(state.SceneManager);
            sceneNode.AttachObject(sub);
            sceneNode.Yaw(Math.HALF_PI);
            SimNode = new SimNode(state.SceneManager.RootSceneNode, sceneNode);

            Light subLight = state.SceneManager.CreateLight("subLight");
            subLight.Type = Light.LightTypes.LT_SPOTLIGHT;
            subLight.Direction = new Vector3(0, 0, -1);
            subLight.SpotlightOuterAngle = new Radian(0.4F);
            subLight.SpotlightInnerAngle = new Radian(0.2F);
            subLight.SpotlightFalloff = 100.0F;
            subLight.SetAttenuation(100, 0F, 0.0F, 0.01F);
            SimNode.SceneNode.AttachObject(subLight);
            SimNode.SceneNode.Position = new Vector3(0, 0, 20);
        }
    }
}