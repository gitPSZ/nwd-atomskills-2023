using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;

namespace SetupApp
{
    public class MainWindow : Window
    {
        public TextBlock errorBackend;
        public TextBlock errorFrontend;
        public TextBox backendTextBox;
        public TextBox frontendTextBox;

        public TextBlock textBlockGreenConnectionString;

        public TextBox serverTextBox;
        public TextBox portTextBox;
        public TextBox userIDTextBox;
        public TextBox passwordTextBox;
        public TextBox databaseTextBox;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            errorBackend = this.FindControl<TextBlock>("TXBL_ERROR_BACKEND");
            errorFrontend = this.FindControl<TextBlock>("TXBL_ERROR_FRONTEND");

            frontendTextBox = this.FindControl<TextBox>("TXB_FRONTEND_URL");
            backendTextBox = this.FindControl<TextBox>("TXB_BACKEND_URL");

            textBlockGreenConnectionString = this.FindControl<TextBlock>("TXBL_GREEN_CONNECTIONSTRING");
            
            serverTextBox = this.FindControl<TextBox>("TXB_Server");
            portTextBox = this.FindControl<TextBox>("TXB_Port");
            userIDTextBox = this.FindControl<TextBox>("TXB_UserID");
            passwordTextBox = this.FindControl<TextBox>("TXB_Password");
            databaseTextBox = this.FindControl<TextBox>("TXB_Database");

            var buttonApply = this.FindControl<Button>("BTN_APPLY");
            buttonApply.Click += ButtonApply_Click;

            var buttonApplyConnectionString = this.FindControl<Button>("BTN_APPLY_CONNNECTION_STRING");
            buttonApplyConnectionString.Click += ButtonApplyConnectionString_Click;
            
        }

        private void ButtonApplyConnectionString_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            textBlockGreenConnectionString.Text = "";
            var connectionString =
                $"Server={serverTextBox.Text}; Port={portTextBox.Text}; User Id={userIDTextBox.Text}; Password={passwordTextBox.Text}; Database={databaseTextBox.Text}";

            ReplaceKeyInBackendConfig("ConnectionString", connectionString);

            textBlockGreenConnectionString.Text = "Строка подключения успешно сохранена";

        }

        private void ReplaceKeyInBackendConfig(string key, string value)
        {
            var configFilePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "backend" + Path.DirectorySeparatorChar + "appsettings.json";

            var stringToWrite = "";

            using (var fileStream = new StreamReader(configFilePath))
            {
                var configString = fileStream.ReadToEnd();
                dynamic JSONObjects = JsonConvert.DeserializeObject(configString);
                foreach (var jsonObject in JSONObjects)
                {
                    if (jsonObject.Name.ToString() == key)
                    {
                        jsonObject.Value = value;
                    }
                }

                stringToWrite = JsonConvert.SerializeObject(JSONObjects);
            }
            using (var fileStream = new StreamWriter(configFilePath))
            {
                fileStream.Write(stringToWrite);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonApply_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (ValidateURLs() == false)
            {
                return;
            }

           
            var frontUrl = frontendTextBox.Text;
            var backUrl = backendTextBox.Text;

            var configFilePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;

            DeleteDirectory(configFilePath + "frontend",true);
            CopyDirectory(configFilePath + "frontendSource", configFilePath + "frontend", true);

            // Get information about the source directory
            var dir = new DirectoryInfo(configFilePath + "frontend");

            var mainFilePath = "";

            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name.StartsWith("main"))
                {
                    mainFilePath = Path.Combine(configFilePath + "frontend", file.Name);
                }
            }

            var mainFileContent = "";
            using (var reader = new StreamReader(mainFilePath))
            {
                mainFileContent = reader.ReadToEnd();
                mainFileContent = mainFileContent.Replace("localhost", backUrl);
            }
            using (var writer = new StreamWriter(mainFilePath))
            {
                writer.Write(mainFileContent);
            }


            ReplaceKeyInBackendConfig("FrontendURL", frontUrl);
            textBlockGreenConnectionString.Text = "Успешно прописаны URL адреса";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public static void DeleteDirectory(string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(destinationDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();



            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    DeleteDirectory(newDestinationDir, true);
                }
            }

            // Create the destination directory
            Directory.Delete(destinationDir);
        }
        private bool ValidateURLs()
        {
            var returnValue = true;
            errorFrontend.Text = "";
            errorBackend.Text = "";
            try
            {
                var frontURL = frontendTextBox.Text;
                var urlArray = frontURL.Split('.');
                if (urlArray.Length != 4)
                {
                    throw new Exception();
                }
                foreach (var number in urlArray)
                {
                    var numberInt = Convert.ToInt32(number);
                    if (numberInt < 0 || numberInt > 255)
                    {
                        errorFrontend.Text = "Каждое число дожно быть больше 0 и меньше 255 (либо равно)";
                        throw new Exception();
                    }
                }

            }
            catch (Exception e)
            {
                if (String.IsNullOrEmpty(errorFrontend.Text))
                {
                    errorFrontend.Text = "Неверный URL. Смотри пример";

                }
                returnValue = false;
            }
            // Backend validation
            try
            {
                var backendURL = backendTextBox.Text;

                var urlArray = backendURL.Split('.');
                if (urlArray.Length != 4)
                {
                    throw new Exception();
                }
                foreach (var number in urlArray)
                {
                    var numberInt = Convert.ToInt32(number);
                    if (numberInt < 0 || numberInt > 255)
                    {
                        errorBackend.Text = "Каждое число дожно быть больше 0 и меньше 255 (либо равно)";
                        throw new Exception();
                    }
                }
            }
            catch (Exception e)
            {
                if (String.IsNullOrEmpty(errorBackend.Text))
                {
                    errorBackend.Text = "Неверный URL. Смотри пример";

                }
                returnValue = false;
            }


            return returnValue;

        }
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
