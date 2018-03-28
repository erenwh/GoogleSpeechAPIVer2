using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;

namespace WpfTutorialSamples.Dialogs
{
	public partial class OpenFileDialogSample : Window
	{
		string input;
		public OpenFileDialogSample()
		{
			InitializeComponent();
		}

		private void btnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				input = openFileDialog.FileName;
				//txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
			}
		}

		private void btnbtnConvert_Click(object sender, RoutedEventArgs e)
		{
			var phrases = new List<string>();
			phrases.Add("SOX");
			var speech = SpeechClient.Create();
			var response = speech.Recognize(new RecognitionConfig()
			{
				

				SpeechContexts = { new SpeechContext() { Phrases = { phrases } } },
				Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
				SampleRateHertz = 16000,
				LanguageCode = "en",
			}, RecognitionAudio.FromFile(input));


			txtEditor.Text = "";

			foreach (var result in response.Results)
			{
				foreach (var alternative in result.Alternatives)
				{
					txtEditor.Text = txtEditor.Text + " " + alternative.Transcript;
				}
			}

			if (txtEditor.Text.Length == 0)
				txtEditor.Text = "No Data ";
		}
	}
}