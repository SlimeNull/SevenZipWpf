using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SevenZip;
using SevenZipWpf.Models;
using SevenZipWpf.Utilities;

namespace SevenZipWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? loadedPath;

        [ObservableProperty]
        private FsItem[]? loadedFsItems;

        [ObservableProperty]
        private SevenZipExtractor? loadedExtractor;

        [ObservableProperty]
        private string? currentPath;

        [ObservableProperty]
        private FsItem? selectedFsItem;

        [ObservableProperty]
        private FsItem[]? selectedFsItems;

        public FsItem[] FsItems
        {
            get
            {
                if (loadedFsItems == null || CurrentPath != loadedPath)
                {
                    LoadFsItemsCore();
                }

                return loadedFsItems;
            }
        }


        [MemberNotNull(nameof(loadedFsItems))]
        private void LoadFsItemsCore()
        {
            try
            {
                if (Directory.Exists(CurrentPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(CurrentPath);

                    List<FsItem> result = new List<FsItem>();

                    foreach (var dirInfo in directoryInfo.EnumerateDirectories())
                    {
                        if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                            continue;

                        result.Add(FsItem.FromDirectory(dirInfo));
                    }

                    foreach (var fileInfo in directoryInfo.EnumerateFiles())
                    {
                        result.Add(FsItem.FromFile(fileInfo));
                    }

                    CurrentPath = directoryInfo.FullName;
                    loadedFsItems = result.ToArray();
                    loadedPath = CurrentPath;
                }
                else if (File.Exists(CurrentPath))
                {
                    var fileinfo = new FileInfo(CurrentPath);
                    loadedExtractor = new SevenZipExtractor(CurrentPath);

                    List<FsItem> result = new List<FsItem>();

                    foreach (var file in loadedExtractor.ArchiveFileData)
                    {
                        if (FileSystemUtils.IsPathUnderPath(string.Empty, file.FileName))
                            result.Add(FsItem.FromArchiveFileInfo(fileinfo, file));
                    }

                    loadedFsItems = result.ToArray();
                    loadedPath = CurrentPath;
                }
                else if (CurrentPath != null)
                {
                    var filename = FileSystemUtils.GetExistFileFromPath(CurrentPath);
                    if (filename == null)
                        goto NotLoad;

                    var fileinfo = new FileInfo(filename);
                    loadedExtractor = new SevenZipExtractor(filename);

                    List<FsItem> result = new List<FsItem>();

                    string currentPathInArchive = Path.GetRelativePath(filename, CurrentPath);
                    foreach (var file in loadedExtractor.ArchiveFileData)
                    {
                        if (FileSystemUtils.IsPathUnderPath(currentPathInArchive, file.FileName))
                            result.Add(FsItem.FromArchiveFileInfo(fileinfo, file));
                    }

                    loadedFsItems = result.ToArray();
                    loadedPath = CurrentPath;
                }
            }
            catch { }

            NotLoad:
            if (loadedFsItems == null)
                loadedFsItems = Array.Empty<FsItem>();
        }


        #region Data Commands


        [RelayCommand]
        public void LoadFsItems()
        {
            OnPropertyChanged(nameof(FsItems));
        }

        [RelayCommand]
        public void ChangeSelectedItems(IEnumerable fsItems)
        {
            SelectedFsItems = fsItems.OfType<FsItem>().ToArray();
        }

        #endregion

        #region 



        #endregion

        #region Path Commands

        [RelayCommand]
        public void ResetCurrentPath()
        {
            if (!string.IsNullOrWhiteSpace(LoadedPath))
                CurrentPath = LoadedPath;
        }

        [RelayCommand]
        public void EnterPath()
        {
            if (SelectedFsItem == null)
                return;

            CurrentPath = SelectedFsItem.Path;
            LoadFsItems();
        }

        [RelayCommand]
        public void EnterAppCurrentPath()
        {
            CurrentPath = Environment.CurrentDirectory;
            LoadFsItems();
        }

        [RelayCommand]
        public void EnterPreviousLevelPath()
        {
            string? previousLevelPath = Path.GetDirectoryName(CurrentPath);

            if (previousLevelPath == null)
                return;

            CurrentPath = previousLevelPath;
            LoadFsItems();
        }

        #endregion
    }
}
