using FTPApp.FTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace FTPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        FtpClient ftp;
        CoreApplicationView view;
        String ImagePath;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            view = CoreApplication.GetCurrentView();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            readConnectionsData();
        }

        private async void onConnectClick(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "Connect clicked";
            saveConnectionData();
            ftp = new FtpClient(textBoxAddress.Text, "21", textBoxUserName.Text, textBoxPassword.Text);

            FtpResponse response = await ftp.ConnectToServer();
            textBlock.Text += "\n" + response.Message;
            response = await ftp.AuthenticateOnServer();
            textBlock.Text += "\n" + response.Message;
            textBlock.Text += "\ngetting files list, wait...";
            response = await ftp.GetListDirectory();
            textBlock.Text += "\n" + response.Message;
            filloutCombo(response.Message);
            if (ftp.IsAuthenticated())
            {
                buttonDownload.IsEnabled = true;
                comboBox.IsEnabled = true;
            }
        }

        private void saveConnectionData()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // txtInput is a TextBox defined in XAML.
            settings.Values["server"] = textBoxAddress.Text;
            settings.Values["user"] = textBoxUserName.Text;
            settings.Values["password"] = textBoxPassword.Text;
        }
        private void readConnectionsData()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey("server")){
                textBoxAddress.Text = settings.Values["server"].ToString();
            }

            if (settings.Values.ContainsKey("user"))
            {
                textBoxUserName.Text = settings.Values["user"].ToString();
            }

            if (settings.Values.ContainsKey("password"))
            {
                textBoxPassword.Text = settings.Values["password"].ToString();
            }
        }
        private void filloutCombo(string pNamesString)
        {
            string[] lNames = pNamesString.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (String fileName in lNames) {
                if(fileName.Trim().Length>1)
                    comboBox.Items.Add(fileName);
            }
        }

        private async void onDownloadClick(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex < 0)
            {
                textBlock.Text += "\nChoose item to download first!";
                return;
            }

            String lFileName = comboBox.SelectedItem.ToString();
            textBlock.Text += "\nStart dowloading " + lFileName;
            FtpFileResponse lResponse = await ftp.GetFile(lFileName);
           
            textBlock.Text += "\nDownload completed: " + lFileName;

            StorageFolder lFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile lFileDownloaded = await lFolder.CreateFileAsync(lFileName.ToString(), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(lFileDownloaded, lResponse.mBytes);
            textBlock.Text += "\nFile saved to:+ " + lFileDownloaded.Path;

            try
            {
                //if image file was downloaded, try to display it in image widget
                BitmapImage img = new BitmapImage();
                img = await LoadImage(lFileDownloaded);
                image.Source = img;
            }
            catch
            {
                textBlock.Text += "\nnot image file";
            }
        }

        private static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;

        }


    }
}
