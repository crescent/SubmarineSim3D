using System;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.src;

namespace SubjugatorSim.Entities
{
    public class Water : BaseEntity, IPropertyGridEntity
    {
        public Water(State state) : base(state)
        {
        }

        private bool enabled = false;
        public Vector3 Dimentions { get; set; }
        
        public bool Hydrax { get; set; }
        public string HydraxConfig { get; set; }
        public bool Fog { get; set; }
        public float FogDepth { get; set; }
        public float FogDensity { get; set; }
        public string Texture { get; set; }

        #region IPropertyGridEntity Members

        public string Name
        {
            get { return "Water"; }
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

            new SimHydrax().Init(state);
            SimNode = new SimNode(state.SceneManager.RootSceneNode);
        }
    }
}