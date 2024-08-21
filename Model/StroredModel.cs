using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoredManageHelper.Model
{
    public class StroredModel
    {
        public string Path { get; set; }

        public double size { get; set; }

        public string Size => GetSize(size);
        public bool OverLimitSize { get; set; }

        public static string GetSize(double s)
        {
            if (s < 1024)
            {
                return s.ToString() + " B";
            }

            if (s / 1024 < 1024)
            {
                return Math.Round(s / 1024, 2).ToString() + "KB";
            }

            if (s / (1024 * 1024) < 1024)
            {
                return Math.Round(s / (1024 * 1024), 2) + "MB";
            }

            if (s / (1024 * 1024 * 1024) < 1024)
            {
                return Math.Round(s / (1024 * 1024 * 1024), 2) + "GB";
            }

            return "";
        }

    }
}
