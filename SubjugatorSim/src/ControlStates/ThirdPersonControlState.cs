using Mogre;
using MOIS;
using SubjugatorSim.Code;
using Vector3 = Mogre.Vector3;

namespace SubjugatorSim.ControlStates
{
    public class ThirdPersonControlState : ControlState
    {
        protected override void CreateState()
        {
            State.CameraManager.CameraChildNode.Position = new Vector3(0, 1, 5);

            State.CameraManager.CameraChildNode.LookAt(new Vector3(0, 0, 0));
        }

        protected override void Rotate(Radian yawAngle, Radian pitch)
        {
            if (State.InputManger.IsKeyDown(KeyCode.KC_LCONTROL))
                base.Rotate(yawAngle, pitch);
            else State.SubNode.Rotate(yawAngle, pitch);
        }


        protected override void Translate(Vector3 amount)
        {
            if (State.InputManger.IsKeyDown(KeyCode.KC_LCONTROL))
                State.CameraManager.CameraChildNode.MoveForward(amount);
            else State.SubNode.MoveForward(amount);
        }


        public override ControlState Next()
        {
            return new ThirdPersonCameraViewControlState().Create(State);
        }
    }
}