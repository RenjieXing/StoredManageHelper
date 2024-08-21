using StoredManageHelper.ViewModel;
namespace StoredManageHelper
{
    public partial class MainPage : ContentPage
    {
     

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new StoredMannagedHelperViewModel();
        }


    }

}
