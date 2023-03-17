using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using Shared.authorization;

namespace CypherApp
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var TextBoxPassword = this.FindControl<TextBox>("TextBoxPassword");
            var TextBoxCipher = this.FindControl<TextBox>("TextBoxCipher");
            var TextBoxSalt = this.FindControl<TextBox>("TextBoxSalt");
            var password = TextBoxPassword.Text;


            var saltString = Shared.authorization.AsymmetricCypherHelper.GenerateSalt();
            //var cipher = StringCipher.Encrypt(password, Constants.Salt);

            var cypherAsymmetrical = AsymmetricCypherHelper.Hash(password + saltString);


            TextBoxCipher.Text = cypherAsymmetrical;
            TextBoxSalt.Text = saltString;

        }
        private async void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            var TextBlockResult = this.FindControl<TextBlock>("TXBL_Result");
            try
            {
                var TextBoxToken = this.FindControl<TextBox>("TXB_Token");


                //var _client = new RestClient("http://172.23.48.61:3000");

                ////_client.UseSerializer(() => new JsonSerializer { RootElement = "Data" });
                //HttpWebRequest request = new HttpWebRequest();
                //_client.Authenticator = new JwtAuthenticator(TextBoxToken.Text);
                //var result = _client.Execute(request);
                //TextBlockResult.Text = result.Content;
                //TextBlockResult.Text = result.ErrorMessage;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", TextBoxToken.Text);
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                var result = await client.GetAsync("http://172.23.48.61:3000/refs/time_sla");

                
            }
            catch (Exception exception)
            {
                TextBlockResult.Text += exception;
            }
           
        }

        
    }
}
