using LoadImage.Models;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LoadImage
{
    public partial class MainPage : ContentPage
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string impath;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        async void GetPhotoAsync(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                impath = photo.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateList();
        }

        async void TakePhotoAsync(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });
                var newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                Debug.WriteLine($"Путь фото {photo.FullPath}");
                impath = photo.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        public void UpdateList()
        {
            imageList.ItemsSource = null;
            imageList.ItemsSource = App.Db.GetObjects();
        }

        private void addbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.db.SaveItem(new ImageModel(nameentry.Text, impath));
                UpdateList();
            }
            catch
            {
                DisplayAlert("", "Ошибка", "Ok");
            }
        }

        private void imageList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new ImagePage((ImageModel)e.Item));
        }
    }
}
