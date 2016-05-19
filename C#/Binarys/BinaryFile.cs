using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCA.Binarys
{
    public class BinaryFile
    {
        #region Binary BCA-File interpretation, structs
        public struct FileHeader
        {
            public const int HEADERSIZE = 14;
            /// <summary>
            /// Should contain ASCII-String 'BA', 0x4142
            /// </summary>
            public ushort fType;
            /// <summary>
            /// Whole file size
            /// </summary>
            public uint fSize;
            /// <summary>
            /// Reserved
            /// </summary>
            public uint fReserved;
            /// <summary>
            /// Byte-Address to BCA Header
            /// </summary>
            public uint fBcaOffset;
        }
        public struct BcaHeader
        {
            /// <summary>
            /// Size of BCA header, 18 bytes
            /// </summary>
            public uint hSize;
            /// <summary>
            /// Version of BCA-File, v1
            /// </summary>
            public ushort hVersion;
            /// <summary>
            /// Byte-Address to first Frame Header
            /// </summary>
            public uint hFrameOffset;
            /// <summary>
            /// Frames per second
            /// </summary>
            public ushort hFPS;
            /// <summary>
            /// Total number of frames in this BCA
            /// </summary>
            public uint hFrameCount;
            /// <summary>
            /// Reserved
            /// </summary>
            public ushort hReserved;
        }
        public struct FrameHeader
        {
            /// <summary>
            /// Frame header size, 6 bytes
            /// </summary>
            public ushort fhSize;
            /// <summary>
            /// Number of used devices in this frame
            /// </summary>
            public ushort fhDeviceCount;
            /// <summary>
            /// Size of all device headers and their data in this frame
            /// </summary>
            public ushort fhDataSize;
        }
        public struct DeviceHeader
        {
            /// <summary>
            /// Device header size, 6 bytes
            /// </summary>
            public byte dhSize;
            /// <summary>
            /// Represents the kind of data, see <see cref="DataType"/>
            /// </summary>
            public DataType dhDataType;
            /// <summary>
            /// Device Type, see <see cref="DeviceType"/>
            /// </summary>
            public DeviceType dhDevice;
            /// <summary>
            /// Size of data, see <see cref="DeviceData"/>
            /// </summary>
            public ushort dhDataSize;
        }
        public struct DeviceData
        {
            /// <summary>
            /// Row
            /// </summary>
            public byte dRow;
            /// <summary>
            /// Column
            /// </summary>
            public byte dCol;
            /// <summary>
            /// ARGB
            /// </summary>
            public uint dABGR;
        }
        public enum DeviceType : ushort
        {
            None = 0x0,
            Keyboard = 0x1,
            Keypad = 0x2,
            Mouse = 0x3,
            Mousepad = 0x4,
            Headset = 0x5,
        }
        public enum DataType : byte
        {
            AssignAll = 0,
            AssignNamed = 1,
        }
        #endregion

        public FileHeader FHeader = new FileHeader();
        public BcaHeader BHeader = new BcaHeader();
        public List<BinaryFrame> FrameList = new List<BinaryFrame>();

        public BinaryFile() { }
    }
}
