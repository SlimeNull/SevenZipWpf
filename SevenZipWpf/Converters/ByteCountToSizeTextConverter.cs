using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SevenZipWpf.Converters
{
    public class ByteCountToSizeTextConverter : IValueConverter
    {
        static readonly string[] units = new string[]
        {
            "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _value = System.Convert.ToInt64(value);

            double _current = _value;
            int unitIndex = 0;

            while (_current >= 1024 && unitIndex < units.Length - 1)
            {
                _current = _current / 1024;
                unitIndex++;
            }

            return $"{_current:0.0}{units[unitIndex]}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
