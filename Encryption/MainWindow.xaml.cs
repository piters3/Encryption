using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Encryption
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Encryptor Enc { get; set; }
        public string FileNameToEncrypt { get; private set; }
        public string SafeFileNameToEncrypt { get; private set; }
        public string DestinationFolderToEncrypt { get; private set; }
        public string FileNameToDecrypt { get; private set; }
        public string SafeFileNameToDecrypt { get; private set; }
        public string DestinationFolderToDecrypt { get; private set; }
        private PhotoContext _ctx { get; set; }
        public ObservableCollection<Photo> EncryptedPhotos { get; private set; }
        public ObservableCollection<Photo> DecryptedPhotos { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Enc = new Encryptor();
            DestinationFolderToEncrypt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
            DestinationFolderName.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DestinationFolderToDecrypt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
            DestinationFolderName2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _ctx = new PhotoContext();
            EncryptedPhotos = new ObservableCollection<Photo>(_ctx.Photos);
            DecryptedPhotos = new ObservableCollection<Photo>();

            foreach (var photo in EncryptedPhotos)
            {
                DecryptedPhotos.Add(new Photo()
                {
                    ID = photo.ID,
                    Name = Enc.DecryptString(photo.Name, "12345678"),
                    ImgData = Enc.DecryptByte(photo.ImgData, "12345678")
                });
            }

            DataContext = this;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string TextToEncrypt = InputText.Text;
            string FileName = "EncryptedText.txt";
            DES DESalg = DES.Create("DES");

            var path2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

            Enc.EncryptTextToFile(TextToEncrypt, FileName, DESalg.Key, DESalg.IV);
            MessageBox.Show($"Pomyślnie zaszyfrowano plik {FileName}\nZaszyfrowany plik znajdziesz w lokalizacji: {path2.Substring(6)} pod nazwą EncryptedText.txt", "Szyfrowanie zakończone", MessageBoxButton.OK, MessageBoxImage.Information);

            string DecryptedText = Enc.DecryptTextFromFile(FileName, DESalg.Key, DESalg.IV);
            DecryptedOutput.Content = DecryptedText;
        }


        private void BtnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                FileNameToEncrypt = ofd.FileName;
                LoadedFileName.Text = FileNameToEncrypt;
                SafeFileNameToEncrypt = "encrypted_" + ofd.SafeFileName;
            }
        }


        private void BtnOpenFiles_Click2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                FileNameToDecrypt = ofd.FileName;
                LoadedFileName2.Text = FileNameToDecrypt;
                SafeFileNameToDecrypt = "decrypted_" + ofd.SafeFileName;
            }
        }


        private void BtnChangeLocation_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            var result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                DestinationFolderName.Text = fbd.SelectedPath;
                DestinationFolderToEncrypt = fbd.SelectedPath + "\\";
            }
        }


        private void BtnChangeLocation_Click2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            var result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                DestinationFolderName2.Text = fbd.SelectedPath;
                DestinationFolderToDecrypt = fbd.SelectedPath + "\\";
            }
        }


        private void EncryptFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EncryptPassword.Password == "")
            {
                MessageBox.Show("Podaj hasło", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (EncryptPassword.Password != EncryptPasswordRepeat.Password)
            {
                MessageBox.Show("Podane hasła nie pasują do siebie", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Enc.EncryptFile(FileNameToEncrypt, DestinationFolderToEncrypt + SafeFileNameToEncrypt, EncryptPassword.Password);
                MessageBox.Show($"Pomyślnie zaszyfrowano plik {FileNameToEncrypt}\nZaszyfrowany plik znajdziesz w lokalizacji: {DestinationFolderToEncrypt} pod nazwą {SafeFileNameToEncrypt}", "Szyfrowanie zakończone", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void DecryptFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DecryptPassword.Password == "")
            {
                MessageBox.Show("Podaj hasło", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Enc.DecryptFile(FileNameToDecrypt, DestinationFolderToDecrypt + SafeFileNameToDecrypt, DecryptPassword.Password);
                MessageBox.Show($"Pomyślnie Odszyfrowano plik {FileNameToDecrypt}\nOdszyfrowany plik znajdziesz w lokalizacji: {DestinationFolderToDecrypt} pod nazwą {SafeFileNameToDecrypt}", "Odzyfrowanie zakończone", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void BrowseImages_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select A File";
            ofd.InitialDirectory = "";
            ofd.Filter = "Image Files (*.gif,*.jpg,*.jpeg,*.bmp,*.png)|*.gif;*.jpg;*.jpeg;*.bmp;*.png";
            ofd.FilterIndex = 1;

            if (ofd.ShowDialog() == true)
            {
                TextBox1.Text = ofd.FileName;
                BitmapImage bmp = new BitmapImage(new Uri(TextBox1.Text.Trim()));
                image1.Source = bmp;
            }
        }


        public string GetFileNameNoExt(string FilePathFileName)
        {
            return Path.GetFileNameWithoutExtension(FilePathFileName);
        }


        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text == "")
            {
                MessageBox.Show("Wskaż plik do wgrania", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                FileStream fs = new FileStream(TextBox1.Text, FileMode.Open, FileAccess.Read);
                BinaryReader rdr = new BinaryReader(fs);

                byte[] fileData = rdr.ReadBytes((int)fs.Length);
                byte[] EncData = Enc.EncryptByte(fileData, "12345678");

                rdr.Close();
                fs.Close();

                Photo p = _ctx.Photos.Create();
                p.Name = Enc.EncryptString(GetFileNameNoExt(TextBox1.Text), "12345678");
                p.ImgData = EncData;

                _ctx.Photos.Add(p);
                _ctx.SaveChanges();
                EncryptedPhotos.Add(p);

                DecryptedPhotos.Add(new Photo()
                {
                    ID = p.ID,
                    Name = Enc.DecryptString(p.Name, "12345678"),
                    ImgData = Enc.DecryptByte(p.ImgData, "12345678")
                });

                MessageBox.Show("Zdjęcie zostało dodane do bazy danych!", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }
    }
}
