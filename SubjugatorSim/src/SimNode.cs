using System;
using Mogre;
using Math = Mogre.Math;

namespace SubjugatorSim.Code
{
    public class SimNode
    {
        public SceneNode SceneNode { get; protected set;  }

        public SimNode(SceneNode root, MovableObject movableObject)
        {
            SceneNode = root.CreateChildSceneNode();
            SceneNode.AttachObject(movableObject);
        }

        public SimNode(SceneNode root, SceneNode node)
        {
            SceneNode = root.CreateChildSceneNode();
            SceneNode.AddChild(node);
        }

        public SimNode(SceneNode simNode)
        {
            SceneNode = simNode;
        }

        public Vector3 Position
        {
            get { return SceneNode.Position; }
            set { SceneNode.Position = value; }
        }

        public Quaternion Orientation
        {
            get { return SceneNode.Orientation; }
            set { SceneNode.Orientation= value; }
        }

        public bool ShowBoundingBox
        {
            get { return SceneNode.ShowBoundingBox; }
            set { SceneNode.ShowBoundingBox = value; }
        }

        public virtual void MoveForward(Vector3 amount)
        {
            SceneNode.Translate(SceneNode.Orientation*amount, Node.TransformSpace.TS_PARENT);
        }

        public virtual void Rotate(Radian yawAngle, Radian pitchAngle)
        {
            var xAxis = new Vector3();
            var yAxis = new Vector3();
            var zAxis = new Vector3();

            SceneNode.Orientation.ToAxes(out xAxis, out yAxis, out zAxis);
            var yaw = new Quaternion(yawAngle, Vector3.UNIT_Y * (yAxis.y > 0 ? 1 : -1));
            var pitch = new Quaternion(pitchAngle, xAxis);

            SceneNode.Orientation = yaw * pitch * SceneNode.Orientation;
        }

        public void AddChild(SceneNode node)
        {
            SceneNode.AddChild(node);
        }

        public void RemoveChild(SceneNode node)
        {
            SceneNode.RemoveChild(node);
        }

        public void AddChild(SimNode node)
        {
            AddChild(node.SceneNode);
        }

        public void RemoveChild(SimNode node)
        {
            RemoveChild(node.SceneNode);
        }

        public void LookAt(Vector3 vector)
        {
            Vector3 direction = vector - Position;
            direction.Normalise();
            if(direction == Vector3.ZERO) return;

            var forward = direction;
            forward.Normalise();
            var right = direction.CrossProduct(Vector3.UNIT_Y);
            right.Normalise();
            var up = right.CrossProduct(forward);
            up.Normalise();
            var quaternion = new Quaternion(right, up, -forward);
            quaternion.Normalise();
            Orientation = quaternion;
        }

        public void Translate(Vector3 amount)
        {
            SceneNode.Translate(amount);
        }

        public float Scale
        {
            get { return SceneNode.GetScale().x; }
            set
            {
                SceneNode.Scale(value, value, value);
            }
        }

    }
}