using System;
using Mogre;
using SubjugatorSim.Code;

namespace SubjugatorSim.ControlStates
{
    public class ThirdPersonCameraViewControlState : ControlState
    {
        protected override void CreateState()
        {
        }

        protected override void Translate(Vector3 amount)
        {
            State.CameraManager.CameraChildNode.SceneNode.Translate(amount);
            State.CameraManager.CameraChildNode.LookAt(Vector3.ZERO);
        }

        public override ControlState Next()
        {

            var realOrientation = State.CameraManager.Camera.RealOrientation;
            var realPosition = State.CameraManager.Camera.RealPosition;

            State.SubNode.RemoveChild(State.CameraManager.CameraNode);
            State.SceneManager.RootSceneNode.AddChild(State.CameraManager.CameraNode.SceneNode);

            ResetCamera();

            State.CameraManager.CameraNode.Position = realPosition;
            State.CameraManager.CameraNode.Orientation = realOrientation;

            return new FreeControlState().Create(State);
        }
    }
}