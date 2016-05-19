using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCA.Binarys
{
    public class BinaryDevice
    {
        public BinaryFile.DeviceHeader DeviceHeader = new BinaryFile.DeviceHeader();
        public List<BinaryFile.DeviceData> DeviceDataList = new List<BinaryFile.DeviceData>();
        public BinaryDevice() { }
    }
}
