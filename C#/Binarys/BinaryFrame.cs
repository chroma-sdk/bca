using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCA.Binarys
{
    public class BinaryFrame
    {
        public BinaryFile.FrameHeader FrameHeader = new BinaryFile.FrameHeader();
        public List<BinaryDevice> DeviceList = new List<BinaryDevice>();
        public BinaryFrame() { }
    }
}
