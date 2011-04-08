using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.src;
using SubjugatorSim.src.Xml;

namespace SubjugatorSim.Entities
{
    public abstract class BaseEntity
    {
        protected readonly State state;

        protected BaseEntity(State state)
        {
            this.state = state;
            SimNode = new NullSimNode();
        }

        private SimNode simNode;

        public SimNode SimNode
        {
            get { return simNode; }
            protected set { simNode = value; }
        } // Make set protected!

        public BaseEntity(SimNode node)
        {
            this.SimNode = node;
        }

        public Quaternion Orientation
        {
            get { return SimNode.Orientation; }
            set { SimNode.Orientation = value; }
        }

        public Vector3 Position
        {
            get { return SimNode.Position; }
            set { SimNode.Position = value; }
        }

        public float Scale
        {
            get { return SimNode.Scale; }
            set { SimNode.Scale = value; }
        }


        public void Populate(BaseEntityXml baseEntity, State state)
        {
            CreateSimNode();
            Orientation = baseEntity.Orientation.ToEnitiy();
            Position = baseEntity.Position.ToEnitiy();
        }

        public abstract void CreateSimNode();
    }
}