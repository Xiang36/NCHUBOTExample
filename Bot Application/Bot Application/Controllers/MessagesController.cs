using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace Bot_Application
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //MakeRequest();
                //Console.WriteLine("Hit ENTER to exit...");
                //Console.ReadLine();

                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                Activity reply = activity.CreateReply($"您傳送了 {activity.Text} 訊息，共 {length} 個字");
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public static class Utilities
        {
            public static async Task<string> MakeRequest(string query)
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "02fe4037dc1b4f00920199fbdbc0eb22");

                // Request parameters
                queryString["returnFaceId"] = "true";
                queryString["returnFaceLandmarks"] = "false";
                queryString["returnFaceAttributes"] = "age";
                var uri = "https://api.projectoxford.ai/face/v1.0/detect?" + queryString;

                HttpResponseMessage response;

                // Request body
                //byte[] byteData = Encoding.UTF8.GetBytes("{ 'url':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRwuUQIWIGRWDLwOW7_rCdSM6wIesUuWWDv9SRS1pS5M_LaNeEv'}");
                byte[] byteData = Encoding.UTF8.GetBytes("{ 'url':'" + query +"'}");
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("< your content type, i.e. application/json >");
                    response = await client.PostAsync(uri, content);
                }
                return await response.Content.ReadAsStringAsync();
            }
        }

        static async void MakeRequest()
        {

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