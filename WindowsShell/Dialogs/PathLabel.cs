using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsShell.Dialogs
{
    [ToolboxItem(true)]
    public class PathLabel : Label
    {
        public static string GetPathText(string text, Font font, Size size)
        {
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.PathEllipsis | TextFormatFlags.ModifyString;
            TextFormatFlags flags1 = TextFormatFlags.Left | TextFormatFlags.PathEllipsis;
            string sTemp = string.Copy(text);
            sTemp = sTemp.Replace("/", "\\");
            bool bChanged = !sTemp.Equals(text);
            TextRenderer.MeasureText(sTemp, font, size, flags);
            if (bChanged)
                sTemp = sTemp.Replace("\\", "/");
            int pos = sTemp.IndexOf('\0');
            if (pos > 0)
                sTemp = sTemp.Substring(0, pos);
            return sTemp;
        }

        public PathLabel(): base()
        {

        }

        [Browsable(false)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }        
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.PathEllipsis | TextFormatFlags.ModifyString;
                TextFormatFlags flags1 = TextFormatFlags.Left | TextFormatFlags.PathEllipsis;
                string sTemp = string.Copy(this.Text);
                sTemp = sTemp.Replace("/", "\\");
                bool bChanged = !sTemp.Equals(this.Text);
                TextRenderer.MeasureText(e.Graphics, sTemp, this.Font, this.ClientRectangle.Size, flags);
                if (bChanged)
                    sTemp = sTemp.Replace("\\", "/");
                int pos = sTemp.IndexOf('\0');
                if (pos > 0)
                    sTemp = sTemp.Substring(0, pos);
                TextRenderer.DrawText(e.Graphics, sTemp, this.Font, this.ClientRectangle, this.ForeColor, flags1);
            }
            catch (Exception)
            {
                
            }            
        }
    }
}
