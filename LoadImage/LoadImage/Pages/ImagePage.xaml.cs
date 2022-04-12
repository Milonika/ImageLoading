using LoadImage.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoadImage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        readonly ImageModel im;
        public ImagePage(ImageModel im)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.im = im;
            nameentry.Text = im.Name;
            imagebox.Source = im.Image;
        }
    }
}