using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    public class Graphic
    {
        public static Bitmap trimImage(Image image)
        {
            Rectangle rect;
            int petit = 0;
            int grand = 0;
            bool sens;
            Bitmap original = (Bitmap)image;
            Bitmap newImage = original;
            if (image.Width != image.Height)
            {
                if (image.Width < image.Height)
                {
                    petit = image.Width;
                    grand = image.Height;
                    sens = false;
                }
                else
                {
                    petit = image.Height;
                    grand = image.Width;
                    sens = true;
                }
                int difference = grand - petit;
                if (sens)
                {
                    rect = new Rectangle(difference / 2, 0, petit, petit);
                }
                else
                {
                    rect = new Rectangle(0, difference / 2, petit, petit);
                }
                newImage = new Bitmap(petit, petit);
                newImage = original.Clone(rect, original.PixelFormat);
            }
            return newImage;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        public static void fillRectangle(DrawTreeNodeEventArgs e, int soustractionX, int width, Color arrierePlan)
        {
            e.Graphics.FillRectangle(new SolidBrush(arrierePlan), new Rectangle(e.Node.Bounds.X - soustractionX, e.Node.Bounds.Y, width, e.Node.Bounds.Height));
        }

        public static void hideRectangleHighlight(DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                fillRectangle(e, 22, 19, Constantes.blue);
            }
            else if (e.Node.Level == 2)
            {
                fillRectangle(e, 41, 38, Constantes.blue);
            }
        }

        public static void fillRectangleBackColor(DrawTreeNodeEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                fillRectangle(e, 4, 282, Constantes.blue);
            }
            else if (e.Node.Level == 1)
            {
                fillRectangle(e, 22, 282, Constantes.blue);
            }
            else if (e.Node.Level == 2)
            {
                fillRectangle(e, 41, 282, Constantes.blue);
            }
        }

        /*
         * Modifie la couleur du titre ou de la description de la checklist
         */
        public static void textBoxChecklistWritingColor(TextBox titreChecklist, RichTextBox shortDesc)
        {
            if (titreChecklist.ForeColor == Color.LightGray)
            {
                titreChecklist.ForeColor = Constantes.blue;
            }
            if (shortDesc.ForeColor == Color.LightGray)
            {
                shortDesc.ForeColor = Constantes.deepBlue;
            }
        }

        private void testRond(Graphics g, Color couleurFond, Brush couleurLettre, string lettre)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawEllipse(new Pen(Color.Black, 2), 1, 1, 60, 60);
            g.FillEllipse(new SolidBrush(couleurFond), 2, 2, 18, 18);
            g.DrawString(lettre, new Font("Calibri", 13, FontStyle.Bold), couleurLettre, 4, 0);
        }

    }
}
