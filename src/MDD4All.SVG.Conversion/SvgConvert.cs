using MDD4All.SVG.DataModels;
using System.Collections.Generic;
using System.Drawing;

namespace MDD4All.SVG.Conversion
{
    public class SvgConvert
    {

        public static Text CreateTextLabel(string text, int maxWidth, Font font, )

        public static List<string> CalculateTextLines(string text, int maxWidth, Font font)
        {
            List<string> result = new List<string>();

            char[] space = { ' ' };

            string[] textTokens = text.Split(space);

            List<string> tokens = new List<string>(textTokens);

            string textLine = "";

            foreach (string token in tokens)
            {
                string tmpText = "";
                if (textLine == "")
                {
                    tmpText = token;
                }
                else
                {
                    tmpText = textLine + " " + token;
                }

                float width = GetTextDimension(tmpText, font).Width;

                if (width / 1.3 > maxWidth)
                {
                    //logger.Debug("width = " + width + " maxWidth = " + maxWidth + " / " + tmpText);

                    result.Add(textLine);
                    textLine = token;
                }
                else
                {
                    textLine = tmpText;
                }

            }
            if (textLine != "") // add last textline
            {
                result.Add(textLine);
            }

            return result;
        }

        private static SizeF GetTextDimension(string text, Font font)
        {
            //create a bmp / graphic to use MeasureString on
            Bitmap b = new Bitmap(2200, 2200);
            Graphics g = Graphics.FromImage(b);

            //measure the string
            SizeF sizeOfString = new SizeF();
            sizeOfString = g.MeasureString(text, font);

            return sizeOfString;
        }
    }
}
