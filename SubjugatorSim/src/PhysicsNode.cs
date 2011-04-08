using System;
using Mogre;
using MogreNewt;
using SubjugatorSim.Code;
using Math = Mogre.Math;

namespace SubjugatorSim
{
    public class PhysicsNode : SimNode
    {
        public Body Body;

        public PhysicsNode(SimNode simNode, State State) : base(simNode.SceneNode)
        {
            SceneNode = simNode.SceneNode;
            SceneNode child = SceneNode;
            while (child.NumChildren() > 0) child = child.GetChild(0) as SceneNode;

            var hull = new MogreNewt.CollisionPrimitives.ConvexHull(State.PhysicsWorld.World, child, child.Orientation, 0.002f, SceneNode.Name.GetHashCode());
            Body = new Body(State.PhysicsWorld.World, hull);
            Body.AttachNode(SceneNode);
            Body.IsGravityEnabled = true;

            Body.SetPositionOrientation(SceneNode._getDerivedPosition(), SceneNode.Orientation); // REALPosition Orient
            Body.ForceCallback += State.PhysicsWorld.ForceCallback;
//            Body.AngularDamping = new Vector3(1,1,1);
//            Body.LinearDamping = 1;
            Body.UserData = new PhysicsControlData();
            hull.Dispose();

        }

        public override void MoveForward(Vector3 amount)
        {
            var data = Body.UserData as PhysicsControlData;
            data.Force += Body.Orientation * amount*50;
        }

        public override void Rotate(Radian yawAngle, Radian pitchAngle)
        {
            var xAxis = new Vector3();
            var yAxis = new Vector3();
            var zAxis = new Vector3();

            Body.Orientation.ToAxes(out xAxis, out yAxis, out zAxis);
//            var yaw = new Quaternion(yawAngle, Vector3.UNIT_Y);
//            var pitch = new Quaternion(pitchAngle, xAxis);

            var data = Body.UserData as PhysicsControlData;
            data.Torque += (yAxis * yawAngle.ValueRadians + xAxis * pitchAngle.ValueRadians)/5.0f;
        }
    }

    public class PhysicsControlData
    {
        public Vector3 Force;

        public Vector3 Torque { get; set; }
    }
}