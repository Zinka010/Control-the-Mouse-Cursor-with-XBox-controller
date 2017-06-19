using System;
using System.Linq;
using System.Threading;

using System.Windows.Forms;
using Windows.Gaming.Input;
using System.Runtime.InteropServices;

namespace MoveCursorWithXboxController
{
    class Program
    {
        // Get the Gamepad
        public static Gamepad Controller;

        // The X and Y input for the Joystick
        public static double rawXInput;
        public static double rawYInput;

        public static Program program = new Program();

        static void Main(string[] args)
        {           
            // Create a Thread
            Thread controlMouseThread = new Thread(new ThreadStart(ControlMouseThread));

            // Start the thread
            controlMouseThread.Start();         
        }

        private static void ControlMouseThread()
        {
            {
                
                int moveX = 0;
                int moveY = 0;
               
                while((!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)))
                {
                    if (Gamepad.Gamepads.Count > 0)
                    {
                        // Define readings from controller
                        Controller = Gamepad.Gamepads.First();
                        var Reading = Controller.GetCurrentReading();

                        // Cast the input to doubles
                        rawXInput = Reading.LeftThumbstickX;
                        rawYInput = Reading.LeftThumbstickY;

                        switch (Reading.Buttons)
                        {
                            case GamepadButtons.A:
                                program.SimulateLeftClick();
                                break;
                            case GamepadButtons.B:
                                program.SimulateRightClick();
                                break;

                        }

                    }



                    #region Get XBox Input for the X Axis

                    if (rawXInput >= 0.3)
                    {
                        moveX = 3;
                    }

                    else if (rawXInput >= 0.8)
                    {
                        moveX = 6;
                    }

                    else if (rawXInput <= -0.3)
                    {
                        moveX = -3;
                    }

                    else if (rawXInput <= -0.8)
                    {
                        moveX = -6;
                    }

                    else
                    {
                        moveX = 0;
                    }
                    #endregion

                    #region Get XBox Input for the Y Axis

                    if (rawYInput <= -0.3)
                    {
                        moveY = 3;
                    }

                    else if (rawYInput <= -0.8)
                    {
                        moveY = 6;
                    }

                    else if (rawYInput >= 0.3)
                    {
                        moveY = -3;
                    }

                    else if (rawYInput >= 0.8)
                    {
                        moveY = -6;
                    }

                    else
                    {
                        moveY = 0;
                    }
                    #endregion


                    //Change mouse cursor position to new coordinates
                    Cursor.Position = new System.Drawing.Point(
                    Cursor.Position.X + moveX,
                    Cursor.Position.Y + moveY);



                    Console.WriteLine(rawXInput);

                    Thread.Sleep(10);
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
        }
     }


        #region Mouse Method
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData,
        int dwExtraInfo);


        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        

        public void SimulateLeftClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        public void SimulateRightClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
        }
#endregion
    }

}
    




