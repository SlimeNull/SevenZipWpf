using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SevenZip;

namespace SevenZipWpf.Models
{
    public partial class FsItem : ObservableObject
    {
        [ObservableProperty]
        private bool isArchiveEntry;

        [ObservableProperty]
        private string? archiveContainer;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string path = string.Empty;

        [ObservableProperty]
        private long size = 0;

        [ObservableProperty]
        private DateTime modifiedDateTime;

        [ObservableProperty]
        private DateTime creationDateTime;

        [ObservableProperty]
        private string? comment;

        [ObservableProperty]
        private bool isFolder = false;

        [ObservableProperty]
        private int folderCount;

        [ObservableProperty]
        private int fileCount;

        public static FsItem FromFile(FileInfo fileInfo)
        {
            var fsItem = new FsItem()
            {
                IsArchiveEntry = false,
                Name = fileInfo.Name,
                Path = fileInfo.FullName,
                Size = fileInfo.Length,
                ModifiedDateTime = fileInfo.LastWriteTime,
                CreationDateTime = fileInfo.CreationTime
            };

            return fsItem;
        }

        public static FsItem FromDirectory(DirectoryInfo directoryInfo)
        {
            var fsItem = new FsItem()
            {
                IsArchiveEntry = false,
                Name = directoryInfo.Name,
                Path = directoryInfo.FullName,
                ModifiedDateTime = directoryInfo.LastWriteTime,
                CreationDateTime = directoryInfo.CreationTime
            };


            int fileCount = 0;
            int folderCount = 0;

            try
            {
                foreach (var fsInfo in directoryInfo.EnumerateFileSystemInfos())
                {
                    if (fsInfo is FileInfo)
                        fileCount++;
                    else if (fsInfo is DirectoryInfo)
                        folderCount++;
                }
            }
            catch { }

            fsItem.FileCount = fileCount;
            fsItem.FolderCount = folderCount;

            return fsItem;
        }

        public static FsItem FromArchiveFileInfo(FileInfo containerFile, ArchiveFileInfo archiveFileInfo)
        {
            var fsItem = new FsItem()
            {
                IsArchiveEntry = true,
                ArchiveContainer = containerFile.FullName,
                Name = System.IO.Path.GetFileName(archiveFileInfo.FileName),
                Path = System.IO.Path.Combine(containerFile.FullName, archiveFileInfo.FileName),
                ModifiedDateTime = archiveFileInfo.LastWriteTime,
                CreationDateTime = archiveFileInfo.CreationTime,
            };

            return fsItem;
        }
    }
}
