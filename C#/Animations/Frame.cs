using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCA.Animations
{
    public class Frame
    {
        public enum DeviceType
        {
            None = 0,
            Keyboard = 1,
            Keypad = 2,
            Mouse = 3,
            Mousepad = 4,
            Headset = 5,
        }
        
        public Dictionary<Binarys.BinaryFile.DeviceType, Corale.Colore.Core.Color[][]> FrameData { get; set; } = new Dictionary<Binarys.BinaryFile.DeviceType, Corale.Colore.Core.Color[][]>();

        public Frame()
        {
            Corale.Colore.Core.Color[][] arr;
            #region Add Devices to Dictionary
            #region Keyboard
            arr = new Corale.Colore.Core.Color[Corale.Colore.Razer.Keyboard.Constants.MaxRows][];
            for (int i = 0; i < Corale.Colore.Razer.Keyboard.Constants.MaxRows; i++)
                arr[i] = new Corale.Colore.Core.Color[Corale.Colore.Razer.Keyboard.Constants.MaxColumns];
            FrameData.Add(Binarys.BinaryFile.DeviceType.Keyboard, arr);
            #endregion
            #region Keypad
            arr = new Corale.Colore.Core.Color[Corale.Colore.Razer.Keypad.Constants.MaxRows][];
            for (int i = 0; i < Corale.Colore.Razer.Keypad.Constants.MaxRows; i++)
                arr[i] = new Corale.Colore.Core.Color[Corale.Colore.Razer.Keypad.Constants.MaxColumns];
            FrameData.Add(Binarys.BinaryFile.DeviceType.Keypad, arr);
            #endregion
            #region Mouse
            arr = new Corale.Colore.Core.Color[Corale.Colore.Razer.Mouse.Constants.MaxRows][];
            for (int i = 0; i < Corale.Colore.Razer.Mouse.Constants.MaxRows; i++)
                arr[i] = new Corale.Colore.Core.Color[Corale.Colore.Razer.Mouse.Constants.MaxColumns];
            FrameData.Add(Binarys.BinaryFile.DeviceType.Mouse, arr);
            #endregion
            #region Mousepad
            arr = new Corale.Colore.Core.Color[1][];
            arr[0] = new Corale.Colore.Core.Color[Corale.Colore.Razer.Mousepad.Constants.MaxLeds];
            FrameData.Add(Binarys.BinaryFile.DeviceType.Mousepad, arr);
            #endregion
            #region Headset
            arr = new Corale.Colore.Core.Color[1][];
            arr[0] = new Corale.Colore.Core.Color[1];
            #endregion
            #endregion
        }
    }
}
