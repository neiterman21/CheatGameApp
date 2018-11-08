using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Luxand;
using System.Drawing;
using System.Threading;
using CheatGameModel;
using System.Diagnostics;

namespace CheatGameApp.Video.Multithreading
{
    public sealed class FeatureExtractionTask //: ITask
    {
        public TimeSpan Time { get; private set; }
        public string ImagePath { get; private set; }
        public TaskResults Results { get; private set; }
    }
}
/*
        public FeatureExtractionTask(TimeSpan time, string imagePath, TaskResults results)
        {
            this.Time = time;
            this.ImagePath = imagePath;
            this.Results = results;
            Interlocked.Increment(ref results.Pending);
        }

        public void Process()
        {

            const string seperator = "\t";

            StringBuilder pointsSB = new StringBuilder();
            using (FSDK.CImage image = new FSDK.CImage(ImagePath))
            {
                //get face position
                FSDK.TFacePosition facePosition = image.DetectFace();

                //append to log file
                pointsSB.AppendFormat(this.Time.ToString("G"));
                pointsSB.Append(seperator);
                pointsSB.AppendFormat("{0}, {1}, {2}, {3}", facePosition.xc, facePosition.yc, facePosition.w, facePosition.angle);
                pointsSB.Append(seperator);

                // if a face is detected, we detect facial features
                if (facePosition.w != 0)
                {
                    //get features
                    FSDK.TPoint[] facialPoints = image.DetectFacialFeaturesInRegion(ref facePosition);

                    //draw points
                    for (int i = 0; i < facialPoints.Length; i++)
                    {
                        FSDK.TPoint point = facialPoints[i];

                        //log point
                        pointsSB.AppendFormat("{0}, {1}", point.x, point.y);
                        pointsSB.Append(seperator);
                    }
                }
                //using (Image jpg = image.ToCLRImage())
                //{
                //    jpg.Save(ImagePath, JpegEncoding.Codec, JpegEncoding.EncoderParams30);
                //}
            }

            //add to results list
            this.Results.Add(Time, pointsSB.ToString());
        }
    }
}
*/