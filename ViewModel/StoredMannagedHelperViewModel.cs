using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoredManageHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        public ObservableCollection<StroredModel> listView1Info;


        [ObservableProperty]
        public ObservableCollection<StroredModel> listView2Info;


        [ObservableProperty]
        public ObservableCollection<StroredModel> listView3Info;

        [ObservableProperty]
        public string info;

        [ObservableProperty]
        public bool isRed;



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
        public void startm()
        {
            init();
            Info = "开始检测";

            if (Path is null)
            {
                Info = "检测到空路径";
                return;
            }
            string path = Path;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var size = 0.0;
            GetFileSize(directoryInfo, ref size, true);
            ListView2Info = new ObservableCollection<StroredModel>(stroredModels2.OrderByDescending(it => it.size).Take(10));
            ListView3Info = new ObservableCollection<StroredModel>(stroredModels3.OrderByDescending(it => it.size).Take(10));


            Info = "检测完成";
            return;


        }

        public void GetFileSize(DirectoryInfo directoryInfo, ref double Size, bool first)
        {
            if (directoryInfo.Exists)
            {
                FileInfo[] files = directoryInfo.GetFiles();

                foreach (FileInfo file in files)
                {

                    stroredModels3.Add(new StroredModel() { Path = file.FullName, size = file.Length });

                    Size += file.Length;
                }
            }
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (var item in directoryInfos)
            {

                if (first)
                {
                    var Size2 = 0.0;
                    try
                    {
                        GetFileSize(item, ref Size2, false);
                    }
                    catch (UnauthorizedAccessException)
                    {

                        continue;
                    }
                    stroredModels2.Add(new StroredModel() { Path = item.FullName, size = Size2 });

                }
                else
                {
                    try
                    {
                        GetFileSize(item, ref Size, false);
                    }
                    catch (UnauthorizedAccessException)
                    {

                        continue;
                    }
                }
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
