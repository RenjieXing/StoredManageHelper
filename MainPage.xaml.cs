using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using StoredManageHelper.Model;
using StoredManageHelper.ViewModel;
namespace StoredManageHelper
{
    public partial class MainPage : ContentPage
    {

        string Rename = "";
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new StoredMannagedHelperViewModel();

        }

        private async void CopyTapped(object sender, EventArgs e)
        {
            var viewCell = (sender as Element).Parent as ViewCell;
            var viewCellLabel = viewCell.FindByName<Entry>("NameEntry");
            var path = GetFullPath(viewCellLabel.Text);
            await Clipboard.Default.SetTextAsync(path.Item1);
        }
        private async void DeleteTapped(object sender, EventArgs e)
        {
            var viewCell = (sender as Element).Parent as ViewCell;
            var viewCellLabel = viewCell.FindByName<Entry>("NameEntry");
            var path = GetFullPath(viewCellLabel.Text);

            if (Directory.Exists(path.Item1))
            {
                Directory.Delete(path.Item1, true);
            }
            else
            {
                if (File.Exists(path.Item1))
                {
                    File.Delete(path.Item1);
                }
            }
            var list = BindingContext as StoredMannagedHelperViewModel;
            var pr = viewCell.Parent as ListView;
            if (pr == ListView1Info)
            {
                list.ListView1Info.Remove(viewCell.BindingContext as StroredModel);

            }
            if (pr == ListView2Info)
            {
                list.ListView2Info.Remove(viewCell.BindingContext as StroredModel);

            }
            if (pr == ListView3Info)
            {
                list.ListView3Info.Remove(viewCell.BindingContext as StroredModel);
            }


        }
        private async void RenameTapped(object sender, EventArgs e)
        {
            var viewCell = (sender as Element).Parent as ViewCell;
            var viewCellLabel = viewCell.FindByName<Entry>("NameEntry");
            if (viewCellLabel != null)
            {
                Rename = viewCellLabel.Text;
                viewCellLabel.IsEnabled = true;
            }

        }
        private async void OpenTapped(object sender, EventArgs e)
        {
            var viewCell = (sender as Element).Parent as ViewCell;
            var viewCellLabel = viewCell.FindByName<Entry>("NameEntry");
            var path = GetFullPath(viewCellLabel.Text);
            if (path.Item2 == 0)
            {
                return;
            }
            OpenFolderInExplorer(path.Item1);
        }
        private void OpenFolderInExplorer(string folderPath)
        {
            // 使用Process启动资源管理器
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = folderPath, // 指定文件夹路径
                UseShellExecute = true
            });
        }



        //重命名更新完毕
        private void OnEntryCompleted(object sender, EventArgs e)
        {
            var entry = sender as Entry;
            entry.IsEnabled = false;
            var path = GetFullPath(entry.Text);
            if (path.Item2 == 0)
            {
                return;
            }
            if (Directory.Exists(path.Item1) || File.Exists(path.Item1))
            {
                entry.Text = Rename;
                return;
            }
            else
            {
                bool isDirectory = GetFullPath(Rename).Item2 == 1;

                if (isDirectory)
                {

                    Directory.Move(GetFullPath
                        (Rename).Item1, path.Item1);
                }
                else
                {
                    File.Move(GetFullPath
                        (Rename).Item1, path.Item1);

                }


            }
        }


        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            var grid = sender as Grid;
            var item = grid.BindingContext as StroredModel;
            grid.BackgroundColor = Colors.LightGray;
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            var grid = sender as Grid;
            var item = grid.BindingContext as StroredModel;
            grid.BackgroundColor = Colors.Transparent;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.Quit(); // Windows 平台下直接退出
        }


        private (string, int) GetFullPath(string path)
        {
            if (Directory.Exists(path))
            {
                return (path, 1);
            }
            if (File.Exists(path))
            {
                return (path, 2);
            }
            if (Directory.Exists(Path.Combine((BindingContext as StoredMannagedHelperViewModel).Path, path)))
            {
                return (Path.Combine((BindingContext as StoredMannagedHelperViewModel).Path, path), 3);
            }

            return (null, -1);
        }

    }
}
