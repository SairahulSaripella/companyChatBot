using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Xml;
using System;
using Controllers;
using Newtonsoft.Json;

namespace Bot_Application
{ 


    [BotAuthentication]
    public class MessagesController : ApiController
    {

        string url;
        string city, temperature, humidity, windType, windSpeed, weather, updatedTime;

        XmlDocument doc;


        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                // checks for weather command and processes accordingly
                if(activity.Text.ToLower().Contains("weather @"))
                {
                        string cityKey = grabKeyword(activity.Text);

                        url = "http://api.openweathermap.org/data/2.5/weather?q=" + cityKey + "&mode=xml&units=imperial&APPID=7e0cd5a8a634743a2556fcf37ad7c2f9";

                        WebClient wc = new WebClient();

                        var xml = await wc.DownloadStringTaskAsync(new Uri(url));
                        doc = new XmlDocument();
                        doc.LoadXml(xml);


                        // define the strings from XML elements pulled from API
                        city = doc.DocumentElement.SelectSingleNode("city").Attributes["name"].Value;
                        temperature = doc.DocumentElement.SelectSingleNode("temperature").Attributes["value"].Value.ToString();
                        humidity = doc.DocumentElement.SelectSingleNode("humidity").Attributes["value"].Value;
                        windSpeed = doc.DocumentElement.SelectSingleNode("wind").SelectSingleNode("speed").Attributes["value"].Value;
                        weather = doc.DocumentElement.SelectSingleNode("weather").Attributes["value"].Value;
                        updatedTime = doc.DocumentElement.SelectSingleNode("lastupdate").Attributes["value"].Value;
                        updatedTime = updatedTime.Remove(0, 11);

                        string finalWeather = $"[Weather Response] City: {city}. Temperature: {temperature}. Humidity: {humidity}. Windspeed: {windSpeed}. Weather: {weather}. Data was last updated at {updatedTime}. ";

                        activity.Text = finalWeather;
                   

                }
                else if (activity.Text.ToLower().Contains("stock @"))
                {
                    try
                    {


                        string symbol = grabKeyword(activity.Text);

                        WebClient wc = new WebClient();

                        string data = wc.DownloadString("http://finance.google.com/finance/info?client=ig&q=NASDAQ:" + symbol);

                        // formatting
                        data = data.Replace("//", "");
                        string semi = data.Replace("[", "");
                        string final = semi.Replace("]", "");

                        stockJSON stock = new stockJSON();
                        stock = JsonConvert.DeserializeObject<stockJSON>(final);


                        activity.Text = "[Stock] Ticker: " + stock.t + ". Price: " + stock.l + ". Change: " + stock.c + ". Change %: " + stock.cp + ". ";

                    }
                    catch
                    {
                        activity.Text = "[Stock] Invalid stock symbol. Ex. To get price of google type: 'stock @ goog'";
                    }


                }



                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public string grabKeyword(string text)
        {

            string[] tokens = text.Split(null as string[], StringSplitOptions.RemoveEmptyEntries);

            int count = 0;

            foreach (string t in tokens)
            {
                if (t.Contains("@"))
                {
                    return tokens[count + 1];
                }

                count++;
            }

            return null;

        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}