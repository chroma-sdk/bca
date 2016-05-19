using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCA.Animations
{
    public class Animation
    {
        /// <summary>
        /// Frames per second
        /// </summary>
        public int FPS { get; set; } = 100;
        /// <summary>
        /// List of all Frames
        /// </summary>
        public List<Frame> Frames { get; set; } = new List<Frame>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public Animation() { }

        /// <summary>
        /// Creates a playable animation from a binary file
        /// </summary>
        /// <param name="bca">BCA file</param>
        /// <returns></returns>
        public static Animation FromBCA(BCA.Binarys.BinaryFile bca)
        {
            Animation animation = new Animation();

            animation.FPS = bca.BHeader.hFPS;

            for (int i = 0; i < bca.BHeader.hFrameCount; i++)
            {
                Frame lastFrame = null;
                if (i != 0)
                    lastFrame = animation.Frames[i - 1];
                animation.Frames.Add(AFrameFromBFrame(bca.FrameList[i], lastFrame));
            }

            return animation;
        }
        /// <summary>
        /// Creates an animation frame from a binary frame
        /// </summary>
        /// <param name="frame">Frame contained in BCA file</param>
        /// <returns></returns>
        private static Frame AFrameFromBFrame(BCA.Binarys.BinaryFrame frame, Frame lastFrame)
        {
            Frame aFrame = new Frame();
            // clone lastframe
            if (lastFrame != null)
            {
                foreach (KeyValuePair<Binarys.BinaryFile.DeviceType, Corale.Colore.Core.Color[][]> kvp in lastFrame.FrameData)
                {
                    Corale.Colore.Core.Color[][] colArr = kvp.Value;
                    Corale.Colore.Core.Color[][] newColArr = aFrame.FrameData[kvp.Key];
                    for (int r = 0; r < colArr.Count(); r++)
                    {
                        for (int c = 0; c < colArr[r].Count(); c++)
                        {
                            newColArr[r][c] = colArr[r][c];
                        }
                    }
                }
            }
            
            for(int i = 0; i < frame.DeviceList.Count; i++)
            {
                Binarys.BinaryFile.DeviceType type = Binarys.BinaryFile.DeviceType.None;
                type = frame.DeviceList[i].DeviceHeader.dhDevice;

                for (int d = 0; d < frame.DeviceList[i].DeviceDataList.Count; d++)
                {
                    Corale.Colore.Core.Color[][] col = aFrame.FrameData[type];
                    col[frame.DeviceList[i].DeviceDataList[d].dRow][frame.DeviceList[i].DeviceDataList[d].dCol] =
                        new Corale.Colore.Core.Color(
                            frame.DeviceList[i].DeviceDataList[d].dABGR);
                }
            }


            return aFrame;
        }
    }
}
