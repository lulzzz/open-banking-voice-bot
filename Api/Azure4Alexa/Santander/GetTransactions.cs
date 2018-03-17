﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AlexaSkillsKit.Speechlet;
using Azure4Alexa.Alexa;
using Azure4Alexa.Helper;
using Azure4Alexa.Models;
using Newtonsoft.Json.Linq;
using Session = AlexaSkillsKit.Speechlet.Session;

namespace Azure4Alexa.Santander
{
    public class GetTransactions
    {
        public static async Task<SpeechletResponse> GetResults(Session session, string date)
        {
            var api = new ApiClient();
            var datetimeFrom = DateTime.Parse(date);
            var result = await api.GetAsync<Transactions>(string.Format(Constants.ApiEndpoints.MyTransactions, datetimeFrom, datetimeFrom.AddDays(1)));
            var simpleIntentResponse = ParseResults(result);
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);
        }

        private static AlexaUtils.SimpleIntentResponse ParseResults(Transactions result)
        {
            string stringToRead = $"<speak>You have made {result.transactions.Length} transactions today.<break time=\"2s\" />";
            string stringForCard = String.Empty;

            for (var i = 0; i < result.transactions.Length; i++)
            {
                stringToRead += $"Transaction {i + 1}";
                stringToRead += $"<break time=\"2s\" />{result.transactions[i].details.description}" +
                                $"<break time=\"2s\" />total of {result.transactions[i].details.value.amount} pounds." +
                                "<break time=\"3s\" />";
            }

            stringToRead += "</speak>";

            return new AlexaUtils.SimpleIntentResponse
            {
                cardText = stringForCard,
                ssmlString = stringToRead,
            };
        }
    }
}