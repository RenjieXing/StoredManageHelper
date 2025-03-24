using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoredManageHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StoredManageHelper.ViewModel
{
    public partial class StoredMannagedHelperViewModel : ObservableObject
    {
        [ObservableProperty]
        public string path;
        /// <summary>
        /// 路径下文件夹信息
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<StroredModel> listView1Info;

        /// <summary>
        /// 路径下所有文件夹信息
        /// </summary>
        [ObservableProperty]
        public ObservableCollection<StroredModel> listView2Info;

        ///
        [ObservableProperty]
        public ObservableCollection<StroredModel> listView3Info;

        [ObservableProperty]
        public string info;

        [ObservableProperty]
        public bool isRed;

        public int BasicLevel = 2;

        [ObservableProperty]
        public bool isBusy = false;

        public object lock1= new object ();
        public object lock2 = new object();
        public object lock3 = new object();
        public bool IsNotBusy => !IsBusy;

        List<StroredModel> stroredModels1 = new List<StroredModel>();
        List<StroredModel> stroredModels2 = new List<StroredModel>();
        List<StroredModel> stroredModels3 = new List<StroredModel>();
        public StoredMannagedHelperViewModel()
        {
            Info = "init";
            listView1Info = new ObservableCollection<StroredModel>();
            listView2Info = new ObservableCollection<StroredModel>();
            listView3Info = new ObservableCollection<StroredModel>();

        }


        [RelayCommand]
        public async Task StartmAsync()
        {

            init();
            IsBusy = true;
            Info = "开始检测";

            if (Path is null)
            {
                Info = "检测到空路径";
                await Task.Delay(2000); IsBusy = false;
                return;
            }
            await Task.Run(() =>
            {
                string path = Path;
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                var size = 0.0;
                GetFileSize(directoryInfo, ref size, true);
            });
            ListView2Info = new ObservableCollection<StroredModel>(stroredModels2.OrderByDescending(it => it.size).Take(10));
            ListView3Info = new ObservableCollection<StroredModel>(stroredModels3.OrderByDescending(it => it.size).Take(10));
            ListView1Info = new ObservableCollection<StroredModel>(stroredModels1.OrderByDescending(it => it.size).Take(10));
            Info = "检测完成";
            IsBusy = false;
            return;
        }

        public void GetFileSize(DirectoryInfo directoryInfo, ref double Size, bool first)
        {
            var DirectorySize = 0.0;
            var FileSize = 0.0;
            if (directoryInfo.Exists)
            {

                FileInfo[] files = directoryInfo.GetFiles();

                foreach (FileInfo file in files)
                {

                    stroredModels3.Add(new StroredModel() { Path = file.FullName, size = file.Length });

                    FileSize += file.Length;
                }

                DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
                foreach (var item in directoryInfos)
                {

                    if (first)
                    {
                        try
                        {
                            var tempReg = DirectorySize;
                            GetFileSize(item, ref DirectorySize, false);
                            var Size1 = DirectorySize - tempReg;
                            stroredModels1.Add(new StroredModel() { Path = item.FullName.Split('\\').Last(), size = Size1 });
                        }
                        catch (UnauthorizedAccessException)
                        {

                            continue;
                        }
                    }
                    else
                    {
                        try
                        {
                            GetFileSize(item, ref DirectorySize, false);
                        }
                        catch (UnauthorizedAccessException)
                        {

                            continue;
                        }
                    }



                }
                Size += DirectorySize;
                Size += FileSize;
                stroredModels2.Add(new StroredModel() { Path = directoryInfo.FullName, size = DirectorySize });
            }

        }

        public void init()
        {
            stroredModels1.Clear();
            stroredModels2.Clear();
            stroredModels3.Clear();
            ListView3Info.Clear();
            ListView2Info.Clear();
            ListView1Info.Clear();
        }

    }
}
