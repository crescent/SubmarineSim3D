using System;
using System.Windows.Forms;
using MOIS;
using Type = MOIS.Type;

namespace SubjugatorSim
{
    public class SimInputManger
    {
        private State state { get; set; }
        private InputManager inputManager;

        public void Init(State state)
        {
            this.state = state;
            state.InputManger = this;
            var paramList = new ParamList();
            paramList.Insert("w32_mouse", "DISCL_FOREGROUND");
            paramList.Insert("w32_mouse", "DISCL_NONEXCLUSIVE");
            paramList.Insert("w32_keyboard", "DISCL_FOREGROUND");
            paramList.Insert("w32_keyboard", "DISCL_NONEXCLUSIVE");
            paramList.Insert("WINDOW", state.MainWindow.Handle.ToString());
            InputManager.CreateInputSystem(paramList);
            inputManager = InputManager.CreateInputSystem(paramList);
            //inputManager = InputManager.CreateInputSystem((uint)state.MainWindow.Handle.ToInt32());

            CreateInputDevices();
        }

        private void CreateInputDevices()
        {
            InputKeyboard = InputKeyboard  ?? (Keyboard)inputManager.CreateInputObject(Type.OISKeyboard, false);
            InputMouse = InputMouse  ?? (Mouse)inputManager.CreateInputObject(Type.OISMouse, false);

            InputJoyStick = InputJoyStick ?? (inputManager.GetNumberOfDevices(Type.OISJoyStick) > 0
                ? (JoyStick)inputManager.CreateInputObject(Type.OISJoyStick, false)
                : null);
        }

        public Keyboard InputKeyboard { get; private set; }
        public Mouse InputMouse { get; private set; }
        public JoyStick InputJoyStick { get; private set; }

        public MouseState_NativePtr MouseState
        {
            get { return InputMouse.MouseState; }
        }


        public void Capture()
        {
            CreateInputDevices();

            InputKeyboard.Capture();
            InputMouse.Capture();
            if (InputJoyStick != null) InputJoyStick.Capture();
        }

        public bool IsKeyDown(KeyCode keyCode)
        {
            return InputKeyboard == null ? false : InputKeyboard.IsKeyDown(keyCode);
        }

        public void Release()
        {
            inputManager.DestroyInputObject(InputKeyboard);
            InputKeyboard = null;

            inputManager.DestroyInputObject(InputMouse);
            InputMouse = null;

            inputManager.DestroyInputObject(InputJoyStick);
            InputJoyStick = null;
        }
    }
}