using Mogre;
using SubjugatorSim.Code;

namespace SubjugatorSim.src
{
    public class NullEntity : IPropertyGridEntity
    {
        public string Name
        {
            get { return null; }
        }

        public IPropertyGridEntity CreateInstance()
        {
            return new NullEntity();
        }

        public bool Enabled
        {
            get { return false; }
        }

        public SimNode SimNode
        {
            get { return new NullSimNode(); }
        }
    }

    public class NullSimNode : SimNode
    {
        public NullSimNode() : base(new SceneNode(null))
        {
        }
    }
}