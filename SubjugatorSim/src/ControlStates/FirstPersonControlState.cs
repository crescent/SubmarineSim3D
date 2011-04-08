using Mogre;
using MOIS;
using SubjugatorSim.Code;
using Vector3 = Mogre.Vector3;

namespace SubjugatorSim.ControlStates
{
    public class FirstPersonControlState : ControlState
    {
        protected override void CreateState()
        {
            ResetCamera();
            State.SceneManager.RootSceneNode.RemoveChild(State.CameraManager.CameraNode.SceneNode);
            State.SubNode.AddChild(State.CameraManager.CameraNode);
        }

        protected override void Rotate(Radian yawAngle, Radian pitch)
        {
            if (State.InputManger.IsKeyDown(KeyCode.KC_LCONTROL))
                base.Rotate(yawAngle, pitch);
            else State.SubNode.Rotate(yawAngle, pitch);
        }
    
        protected override void Translate(Vector3 amount)
        {
            State.SubNode.MoveForward(amount);
        }

        public override ControlState Next()
        {
            //ResetCamera();

            return new ThirdPersonControlState().Create(State);
        }
    }
}