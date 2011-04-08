using System;
using Mogre;
using MOIS;
using SubjugatorSim.Code;
using Vector3 = Mogre.Vector3;

namespace SubjugatorSim.ControlStates
{
    public class ControlState
    {
        protected const float TRANSLATE = 20f;
        protected const float ROTATE = 0.005f;
        protected const float JOYSTICK_BOOST = 3f;


        public virtual void Init(State state)
        {
            this.State = state;
            this.State.ControlState = new ControlState().Create(state);

            this.State.Root.FrameStarted += delegate(FrameEvent evt) { Update(evt); return true; };
        }

        protected virtual void CreateState()
        {
            
        }

        public ControlState Create(State state)
        {
            this.State = state;
            CreateState();
            return this;
        }

        protected State State { get; set;}

        protected void ResetCamera()
        {
            State.CameraManager.CameraNode.Position = new Vector3(0, 0, 0);
            State.CameraManager.CameraNode.Orientation = Quaternion.IDENTITY;

            State.CameraManager.Camera.Position = new Vector3(0, 0, 0); // not req
            State.CameraManager.Camera.Orientation = Quaternion.IDENTITY;
            
            State.CameraManager.CameraChildNode.Position = new Vector3(0,0,0);
            State.CameraManager.CameraChildNode.Orientation = Quaternion.IDENTITY;
        }

        public virtual void Update(FrameEvent frameEvent)
        {
            //if (State.MainWindow.RenderWindowFocused == false) return;

            State.InputManger.Capture();

            Rotate(YawAngle(), PitchAngle());

            Translate(GetTranslation() * frameEvent.timeSinceLastFrame);
        }


        protected Radian PitchAngle()
        {
            return new Radian(State.InputManger.MouseState.Y.rel * ROTATE);
        }

        protected Radian YawAngle()
        {
            return new Radian(State.InputManger.MouseState.X.rel * -ROTATE);
        }

        protected Vector3 GetTranslation()
        {
            var mTranslation = new Vector3(0, 0, 0);

            mTranslation.z = State.InputManger.IsKeyDown(KeyCode.KC_W)  ? -TRANSLATE : mTranslation.z;
            mTranslation.z = State.InputManger.IsKeyDown(KeyCode.KC_S) ? TRANSLATE : mTranslation.z;
            mTranslation.y = State.InputManger.IsKeyDown(KeyCode.KC_Q) ? -TRANSLATE : mTranslation.y;
            mTranslation.y = State.InputManger.IsKeyDown(KeyCode.KC_E) ? TRANSLATE : mTranslation.y;
            mTranslation.x = State.InputManger.IsKeyDown(KeyCode.KC_A) ? -TRANSLATE : mTranslation.x;
            mTranslation.x = State.InputManger.IsKeyDown(KeyCode.KC_D) ? TRANSLATE : mTranslation.x;

            if (State.InputManger.InputJoyStick != null && mTranslation.Equals(new Vector3()))
                    mTranslation = TranslateJoystick();
            
            return mTranslation;
        }

        protected Vector3 TranslateJoystick()
        {
            float fltX = TranslateJoystickAxis(1, false),
                  fltY = TranslateJoystickAxis(0, false),
                  fltZ = TranslateJoystickAxis(4, true);
            return new Vector3(fltX, fltZ, fltY);
        }

        protected float TranslateJoystickAxis(int intAxisIndex, bool bolInvert)
        {
            float fltAxisAbs = State.InputManger.InputJoyStick.JoyStickState.GetAxis(intAxisIndex).abs;
            fltAxisAbs = System.Math.Abs(fltAxisAbs) < 6000 ? 0 : fltAxisAbs;
            return (float)System.Math.Pow(fltAxisAbs / 32767, 5) * JOYSTICK_BOOST * TRANSLATE * (bolInvert ? -1 : 1);
        }

        protected Radian RotateJoystickAxis(int intAxisIndex, bool bolInvert)
        {
            float fltAxisAbs = State.InputManger.InputJoyStick.JoyStickState.GetAxis(intAxisIndex).abs;
            fltAxisAbs = System.Math.Abs(fltAxisAbs) < 6000 ? 0 : fltAxisAbs;
            return new Radian((float)System.Math.Pow(fltAxisAbs / 32767, 5) * JOYSTICK_BOOST * ROTATE * (bolInvert ? -1 : 1));
        }

        protected virtual void Translate(Vector3 amount)
        {
            State.CameraManager.CameraNode.MoveForward(amount);
        }

        protected virtual void Rotate(Radian yawAngle, Radian pitchAngle)
        {
            State.CameraManager.CameraNode.Rotate(yawAngle, pitchAngle);
        }

        public ControlState()
        {
            
        }

        public virtual ControlState Next()
        {
            return new FreeControlState().Create(State);
        }
    }
}