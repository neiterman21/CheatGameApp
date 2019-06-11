using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace CheatGameModel
{
    public static class JpegEncoding
    {
        public static readonly ImageCodecInfo Codec;
        public static readonly EncoderParameters EncoderParams30;
        public static readonly EncoderParameters EncoderParams70;
        public static readonly EncoderParameters EncoderParams100;

        static JpegEncoding()
        {
            //init jpg format
            Codec = ImageCodecInfo.GetImageEncoders().Single(c => c.MimeType == "image/jpeg");
            EncoderParams30 = new EncoderParameters();
            EncoderParams30.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30L);

            EncoderParams70 = new EncoderParameters();
            EncoderParams70.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70L);

            EncoderParams100 = new EncoderParameters();
            EncoderParams100.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
        }
    }
}
