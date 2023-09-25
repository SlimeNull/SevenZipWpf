using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipWpf.Utilities
{
    public static class FileSystemUtils
    {
        /// <summary>
        /// 返回路径的上级路径 (不包含结尾的 '\' 或者 '/')
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string? GetPathParent(string path)
        {
            string trimmedPath = Path.TrimEndingDirectorySeparator(path);

            string? parentPath = Path.GetDirectoryName(path);
            if (parentPath != null)
                parentPath = Path.TrimEndingDirectorySeparator(parentPath);

            return parentPath;
        }

        /// <summary>
        /// 根据路径, 逐级往上查找, 直到找到一个存在的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string? GetExistFileFromPath(string path)
        {
            string? _path = path;

            while (_path != null)
            {
                if (File.Exists(_path))
                    return _path;

                _path = GetPathParent(_path);
            }

            return null;
        }

        /// <summary>
        /// 判断路径是否处于某路径下
        /// </summary>
        /// <param name="path">父路径</param>
        /// <param name="pathToCheck">要检查的路径</param>
        /// <returns><paramref name="pathToCheck"/> 是否在 <paramref name="path"/>下</returns>
        public static bool IsPathUnderPath(string path, string pathToCheck)
        {
            string trimmedPath = Path.TrimEndingDirectorySeparator(path);

            return GetPathParent(pathToCheck) == trimmedPath;
        }
    }
}
