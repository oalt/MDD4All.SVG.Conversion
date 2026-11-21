using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using MDD4All.Text.DataModels;

namespace MDD4All.SVG.Conversion
{
    public class SvgConvert
    {

        public static DataModels.Group CreateTextLabel(string text,
                                                       int maxWidth,
                                                       FontDescription font,
                                                       VerticalTextAlignment verticalTextAlignment,
                                                       HorizontalTextAlignment horizontalTextAlignment,
                                                       Point position,
                                                       Size size)
        {
            DataModels.Group result = new DataModels.Group();

#if DEBUG
            DataModels.Rectangle rectangle = new DataModels.Rectangle
            {
                X = position.X.ToString(CultureInfo.InvariantCulture),
                Y = position.Y.ToString(CultureInfo.InvariantCulture),
                Height = size.Height.ToString(CultureInfo.InvariantCulture),
                Width = size.Width.ToString(CultureInfo.InvariantCulture),
                CssStyle = "stroke:lightgray;fill:transparent;"

            };

            result.Elements.Add(rectangle);
#endif
            List<string> lines = CalculateTextLines(text, maxWidth, font);

            int lineOffset = (int)font.FontSize;

            foreach (string textLine in lines)
            {

                string fontFamily = "sans-serif";

                switch (font.FontFamily)
                {
                    case Text.DataModels.FontFamily.Serif:
                        fontFamily = "serif";
                        break;
                    case Text.DataModels.FontFamily.SansSerif:
                        fontFamily = "sans-serif";
                        break;
                    case Text.DataModels.FontFamily.Monospace:
                        fontFamily = "monospace";
                        break;
                }

                DataModels.Text nameText = new DataModels.Text()
                {
                    X = position.X.ToString(CultureInfo.InvariantCulture),
                    Y = (position.Y + lineOffset).ToString(CultureInfo.InvariantCulture),
                    Fill = "black",
                    FontFamily = fontFamily,
                    FontSize = font.FontSize.ToString(),
                    InlineSize = size.Width.ToString(CultureInfo.InvariantCulture),
                    DisplayedText = textLine
                };

                if (horizontalTextAlignment == HorizontalTextAlignment.Center)
                {
                    nameText.TextAnchor = "middle";
                    nameText.X = (position.X + size.Width / 2).ToString(CultureInfo.InvariantCulture);

                }
                else if (horizontalTextAlignment == HorizontalTextAlignment.Right)
                {
                    nameText.TextAnchor = "end";
                    nameText.X = (position.X + size.Width).ToString(CultureInfo.InvariantCulture);
                }

                if (font.FontWeight == FontWeight.Bold)
                {
                    nameText.FontWeight = "bold";
                }

                result.Elements.Add(nameText);
                lineOffset += (int)font.FontSize;
            }

            return result;
        }

        public static List<string> CalculateTextLines(string text, int maxWidth, FontDescription font)
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

        private static SizeF GetTextDimension(string text, FontDescription fontDescription)
        {
            System.Drawing.Font font = CreateFontFromFontDescription(fontDescription);

            //create a bmp / graphic to use MeasureString on
            Bitmap b = new Bitmap(2200, 2200);
            Graphics g = Graphics.FromImage(b);

            //measure the string
            SizeF sizeOfString = new SizeF();
            sizeOfString = g.MeasureString(text, font);

            return sizeOfString;
        }

        private static System.Drawing.Font CreateFontFromFontDescription(FontDescription fontDescription)
        {
            System.Drawing.Font result = null;

            System.Drawing.FontFamily fontFamily = null;

            switch (fontDescription.FontFamily)
            {
                case MDD4All.Text.DataModels.FontFamily.Serif:
                    fontFamily = new System.Drawing.FontFamily("Times");
                    break;
                case MDD4All.Text.DataModels.FontFamily.SansSerif:
                    fontFamily = new System.Drawing.FontFamily("Verdana");
                    break;
                case MDD4All.Text.DataModels.FontFamily.Monospace:
                    fontFamily = new System.Drawing.FontFamily("Courier New");
                    break;
            }

            if (fontDescription.FontStyle == MDD4All.Text.DataModels.FontStyle.Italic)
            {
                if (fontDescription.FontWeight == MDD4All.Text.DataModels.FontWeight.Bold)
                {
                    result = new System.Drawing.Font(fontFamily, fontDescription.FontSize, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
                }
                else
                {
                    result = new System.Drawing.Font(fontFamily, fontDescription.FontSize, System.Drawing.FontStyle.Italic);
                }
            }
            else
            {
                if (fontDescription.FontWeight == MDD4All.Text.DataModels.FontWeight.Bold)
                {
                    result = new System.Drawing.Font(fontFamily, fontDescription.FontSize, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    result = new System.Drawing.Font(fontFamily, fontDescription.FontSize);
                }
            }

            return result;
        }
    }
}
