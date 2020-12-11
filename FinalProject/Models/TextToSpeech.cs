using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace FinalProject.Models
{
    public class TextToSpeech
    {
        public static async Task SynthesizeAudioAsync(string article)
        {
            var config = SpeechConfig.FromSubscription(Secret.azureSpeechKey, "eastus");
            using var audioConfig = AudioConfig.FromWavFileOutput("../../file.wav");
            using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            await synthesizer.SpeakTextAsync(article);
        }
    }
}
