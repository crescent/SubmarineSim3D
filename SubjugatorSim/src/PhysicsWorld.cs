using System;
using Mogre;
using MogreNewt;
using MOIS;
using SubjugatorSim.Code;
using Vector3 = Mogre.Vector3;
using World = MogreNewt.World;

namespace SubjugatorSim
{
    public class PhysicsWorld : IDisposable
    {
        public MogreNewt.World World;
        private State State;

        public void Init(State State)
        {
            this.State = State;
            State.PhysicsWorld = this;
            World = new MogreNewt.World();
            World.SetPlatformArchitecture(MogreNewt.World.PlatformArchitecture.PA_BEST_HARDWARE);
            World.SetWorldSize(new Vector3(-500, -500, -500), new Vector3(500, 500, 500));
            World.SetSolverModel(MogreNewt.World.SolverModelMode.SM_2_PASS);
            World.DebuggerInstance.Init(State.SceneManager);


            // using the new "SceneParser" TreeCollision primitive.  this will automatically parse an entire tree of
            // SceneNodes (parsing all children), and add collision for all meshes in the tree.
            var statCol = new MogreNewt.CollisionPrimitives.TreeCollisionSceneParser(World);
            statCol.ParseScene(State.SceneManager.RootSceneNode, true, 0); // was floornode
            var sceneBody = new Body(World, statCol);
            statCol.Dispose();

            sceneBody.AttachNode(State.SceneManager.RootSceneNode); // was floornode
            sceneBody.SetPositionOrientation(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.IDENTITY);


            var ent = State.SceneManager.CreateEntity("cylinder_body", "mocksub.mesh");
            var simNode = new SimNode(State.SceneManager.RootSceneNode, ent);

            // rigid body.
            var phyNode = new PhysicsNode(simNode, State);

            phyNode.Body.SetPositionOrientation(new Vector3(0, 10, 0), Quaternion.IDENTITY);
            phyNode.Body.SetMassMatrix(125, Vector3.ZERO);

            var physicsNode = new PhysicsNode(State.SubNode, State);
            physicsNode.Body.SetMassMatrix(125, Vector3.ZERO);
            // TODO: FIX THIS State.SubNode = physicsNode;

            // initial position
            State.Root.FrameStarted += NewtonUpdate;
        }

        public void ForceCallback(Body body, float timestep, int threadindex)
        {
            var data = body.UserData as PhysicsControlData;
            if (data != null)
            {
                body.AddForce(data.Force * body.Mass);
                data.Force = Vector3.ZERO;
                body.AddTorque(data.Torque * body.Mass);
                data.Torque = Vector3.ZERO;
            }

            if (body.Position.y < -10) body.AddBouyancyForce(1030, 0.0020F, 0.0020F, Vector3.NEGATIVE_UNIT_Y * 9.8f, Plane );
        }

        private static bool Plane(int collisionid, Body body, Quaternion orientation, Vector3 position, ref Plane retplane)
        {
            return true;
        }

        private float m_elapsed = 0;
        private float m_update = 1.0F / 60f;

        private bool NewtonUpdate(FrameEvent evt)
        {
            m_elapsed += evt.timeSinceLastFrame;

            if ((m_elapsed > m_update) && (m_elapsed < (1.0f)))
            {
                while (m_elapsed > m_update)
                {
                    World.Update(m_update);
                    m_elapsed -= m_update;
                }
            }
            else
            {
                if (m_elapsed > (m_update))
                {
                    World.Update(m_elapsed);
                    m_elapsed = 0.0f; // reset the elapsed time so we don't become "eternally behind".
                }
            }

            // For the debug lines
            if (State.InputManger.IsKeyDown(MOIS.KeyCode.KC_F12))
                World.DebuggerInstance.ShowDebugInformation(true);
            else
                World.DebuggerInstance.HideDebugInformation();
            return true;
        }


        public void Dispose()
        {
            World.DebuggerInstance.DeInit();
            World.Dispose();
        }
    }
}