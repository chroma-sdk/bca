using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCA.Binarys
{
    public class Utils
    {
        public static BCA.Binarys.BinaryFile ReadBCA(System.IO.Stream s)
        {
            BCA.Binarys.BinaryFile file = new BinaryFile();
            System.IO.BinaryReader br = new System.IO.BinaryReader(s);

            #region File Header
            file.FHeader.fType = (ushort)br.ReadInt16();
            file.FHeader.fSize = (uint)br.ReadInt32();
            file.FHeader.fReserved = (uint)br.ReadInt32();
            file.FHeader.fBcaOffset = (uint)br.ReadInt32();
            #endregion
            #region BCA Header
            file.BHeader.hSize = (uint)br.ReadInt32();
            file.BHeader.hVersion = (ushort)br.ReadInt16();
            file.BHeader.hFrameOffset = (uint)br.ReadInt32();
            file.BHeader.hFPS = (ushort)br.ReadInt16();
            file.BHeader.hFrameCount = (uint)br.ReadInt32();
            file.BHeader.hReserved = (ushort)br.ReadInt16();
            #endregion
            #region Frames
            for (int f = 0; f < file.BHeader.hFrameCount; f++)
                file.FrameList.Add(new BinaryFrame());
            for (int f = 0; f < file.FrameList.Count; f++)
            {
                #region Frame Header
                file.FrameList[f].FrameHeader.fhSize = (ushort)br.ReadInt16();
                file.FrameList[f].FrameHeader.fhDeviceCount = (ushort)br.ReadInt16();
                
                for (int i = 0; i < file.FrameList[f].FrameHeader.fhDeviceCount; i++)
                    file.FrameList[f].DeviceList.Add(new BinaryDevice());
                file.FrameList[f].FrameHeader.fhDataSize = (ushort)br.ReadInt16();
                #endregion
                #region Frame Data
                for (int i = 0; i < file.FrameList[f].DeviceList.Count; i++)
                {
                    #region Device Header
                    file.FrameList[f].DeviceList[i].DeviceHeader.dhSize = br.ReadByte();
                    file.FrameList[f].DeviceList[i].DeviceHeader.dhDataType = (BCA.Binarys.BinaryFile.DataType)br.ReadByte();
                    file.FrameList[f].DeviceList[i].DeviceHeader.dhDevice = (BCA.Binarys.BinaryFile.DeviceType)br.ReadInt16();
                    file.FrameList[f].DeviceList[i].DeviceHeader.dhDataSize = (ushort)br.ReadInt16();
                    ushort dataCount = (ushort)(file.FrameList[f].DeviceList[i].DeviceHeader.dhDataSize / 6);
                    for (int d = 0; d < dataCount; d++)
                        file.FrameList[f].DeviceList[i].DeviceDataList.Add(new BinaryFile.DeviceData());
                    #endregion
                        #region Device Data
                    for (int d = 0; d < file.FrameList[f].DeviceList[i].DeviceDataList.Count; d++)
                    {
                        BCA.Binarys.BinaryFile.DeviceData data = new BinaryFile.DeviceData();
                        data.dRow = br.ReadByte();
                        data.dCol = br.ReadByte();
                        data.dABGR = (uint)br.ReadInt32();
                        file.FrameList[f].DeviceList[i].DeviceDataList[d] = data;
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            return file;
        }
        public static System.IO.MemoryStream WriteBCA(BCA.Binarys.BinaryFile file)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
            
                #region File Header
                bw.Write(file.FHeader.fType);
                bw.Write(file.FHeader.fSize);
                bw.Write(file.FHeader.fReserved);
                bw.Write(file.FHeader.fBcaOffset);
                #endregion
                #region BCA Header
                bw.Write(file.BHeader.hSize);
                bw.Write(file.BHeader.hVersion);
                bw.Write(file.BHeader.hFrameOffset);
                bw.Write(file.BHeader.hFPS);
                bw.Write(file.BHeader.hFrameCount);
                bw.Write(file.BHeader.hReserved);
                #endregion
                #region Frames
                for (int f = 0; f < file.FrameList.Count; f++)
                {
                    #region Frame Header
                    bw.Write(file.FrameList[f].FrameHeader.fhSize);
                    bw.Write((ushort)file.FrameList[f].FrameHeader.fhDeviceCount);
                    bw.Write(file.FrameList[f].FrameHeader.fhDataSize);
                    #endregion
                    #region Frame Data
                    for (int i = 0; i < file.FrameList[f].DeviceList.Count; i++)
                    {
                        #region Device Header
                        bw.Write(file.FrameList[f].DeviceList[i].DeviceHeader.dhSize);
                        bw.Write((byte)file.FrameList[f].DeviceList[i].DeviceHeader.dhDataType);
                        bw.Write((ushort)file.FrameList[f].DeviceList[i].DeviceHeader.dhDevice);
                        bw.Write(file.FrameList[f].DeviceList[i].DeviceHeader.dhDataSize);
                        #endregion
                        #region Device Data
                        for (int d = 0; d < file.FrameList[f].DeviceList[i].DeviceDataList.Count; d++)
                        {
                            bw.Write(file.FrameList[f].DeviceList[i].DeviceDataList[d].dRow);
                            bw.Write(file.FrameList[f].DeviceList[i].DeviceDataList[d].dCol);
                            bw.Write(file.FrameList[f].DeviceList[i].DeviceDataList[d].dABGR);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            
            return ms;
        }

    }
}
