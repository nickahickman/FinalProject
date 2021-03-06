﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;

namespace FinalProject.Models
{
    public class TextToSpeech
    {
        public static async Task<string> SynthesizeAudioAsync(string article, string title)
        {
            AmazonPollyClient apc = new AmazonPollyClient(Secret.AWSAccessKey, Secret.AWSSecretKey, Amazon.RegionEndpoint.USEast1);
            StartSpeechSynthesisTaskRequest req = new StartSpeechSynthesisTaskRequest();
            req.Engine = "neural";
            req.OutputFormat = "mp3";
            req.OutputS3BucketName = "lrnr-neural";
            req.Text = article;
            req.VoiceId = "Matthew";

            StartSpeechSynthesisTaskResponse response = await apc.StartSpeechSynthesisTaskAsync(req);

            return response.SynthesisTask.OutputUri;
        }

        public static string BuildSSMLRequest(string article)
        {
            string req = $"<speak>{article}</speak>";

            return req;
        }
    }
}
