using System.Windows.Forms;
using Mogre;
using MOIS;
using Vector3=Mogre.Vector3;

namespace SubjugatorSim.ControlStates
{
    public class EditorControlState : ControlState
    {
        public bool RenderWindowFocused { get; set; }

        public override void Init(State state)
        {
            base.Init(state);

            State.MainWindow.MouseDown += FocusOnRenderWindow;
            //State.MainWindow.MouseUp += UnfocusOnRenderWindow;
        }


        private void UnfocusOnRenderWindow(object sender, MouseEventArgs e)
        {
            RenderWindowFocused = false;
        }


        private void FocusOnRenderWindow(object sender, MouseEventArgs e)
        {
            State.MainWindow.FocusOnRenderWindow();
            RenderWindowFocused = true;
        }

        public override void Update(FrameEvent frameEvent)
        {
            if (!State.MainWindow.IsRenderWindowFocused) return;

            State.InputManger.Capture();

            bool leftButtonDown = State.InputManger.MouseState.ButtonDown(MouseButtonID.MB_Left);
            bool rightButtonDown = State.InputManger.MouseState.ButtonDown(MouseButtonID.MB_Right);

            bool ctrl = State.InputManger.InputKeyboard.IsKeyDown(KeyCode.KC_LCONTROL);
            bool shift = State.InputManger.InputKeyboard.IsKeyDown(KeyCode.KC_LSHIFT);
            bool alt = State.InputManger.InputKeyboard.IsKeyDown(KeyCode.KC_LMENU);

            Vector3 amount = GetTranslation()*frameEvent.timeSinceLastFrame;

            if (ctrl && shift)
            {
                State.SelectedNode.Rotate(amount.x/5.0f, amount.z/5.0f);
                State.SelectedNode.Rotate(5.0f*YawAngle(), 5.0f*PitchAngle());
                //State.SelectedNode.MoveForward(amount);
            }
            else if (ctrl)
            {
                State.SelectedNode.Translate(amount);
                State.SelectedNode.Translate(10.0f*new Vector3(-YawAngle().ValueRadians, 0, PitchAngle().ValueRadians));
            }
            else if (shift)
            {
                State.SelectedNode.Translate(new Vector3(amount.x, amount.z, amount.y));
                State.SelectedNode.Translate(10.0f*new Vector3(-YawAngle().ValueRadians, -PitchAngle().ValueRadians, 0));
            }
            else if (alt)
            {
                State.SelectedNode.Scale = 1 + amount.z/2;
                State.SelectedNode.Scale = 1 + 2*PitchAngle().ValueRadians;
            }
            else if (leftButtonDown)
            {
                Translate(amount);
                Rotate(YawAngle(), PitchAngle());
            }
            else
            {
                Translate(amount);
            }

            //Joystick rotation
            if (State.InputManger.InputJoyStick != null)
                Rotate(RotateJoystickAxis(3, true), RotateJoystickAxis(2, true));

            //base.Update(frameEvent);
            State.MainWindow.UpdatePropertyTreeGrid();
        }
    }
}