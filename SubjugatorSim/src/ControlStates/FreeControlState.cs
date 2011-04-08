using System;
using Mogre;
using SubjugatorSim.Code;

namespace SubjugatorSim.ControlStates
{
    public class FreeControlState : ControlState
    {
        public override ControlState Next()
        {
            ResetCamera();
            return new FirstPersonControlState().Create(State);
        }

        protected override void CreateState()
        {
        }

        protected override void Rotate(Radian yawAngle, Radian pitchAngle)
        {
            base.Rotate(yawAngle,pitchAngle);

            var debugOverlay = OverlayManager.Singleton.GetByName("Core/DebugOverlay");
            debugOverlay.Show();

            var myCurr = OverlayManager.Singleton.GetOverlayElement("Core/CurrFps");

            myCurr.Caption = State.CameraManager.RenderWindow.LastFPS.ToString();

        }
    }
}