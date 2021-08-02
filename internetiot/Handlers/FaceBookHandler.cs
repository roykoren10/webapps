using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using internetiot.Models;
using Newtonsoft.Json;
using Facebook;

namespace internetiot.Handlers
{
    public class FaceBookHandler
    {
        private const string FacebookApiID = "295415171250806";
        private const string FacebookApiSecret = "f03621af30b003c7e0a30339ca2d6aaa";

        private const string PageID = "259264181584450";                                      
        private const string fb_exchange_token = "EAAEMrbRXZBnYBALzMHGJetydY2GLIXbWTFtGfQvJljz2ID5J8NNaZCz2mym5dhP3gcHbF76zErEZCbuJepThVUxw7axjawbVZBgAL6M3h5bOXuqQoUWkBG9o8o81X1davrkwZBuA0mvzhU3G10pfloiVNSZBeh0VmLJAouzcttRgZDZD";

        private const string AuthenticationUrlFormat =
            "https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id={0}&client_secret={1}&fb_exchange_token={2}";


        static string GetAccessToken(string apiID, string apiSecret, string pageID)
        {
            string accessToken = string.Empty;
            string url = string.Format(AuthenticationUrlFormat, apiID, apiSecret, fb_exchange_token);

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            using (System.IO.Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.Default);
                String responseString = reader.ReadToEnd();

                dynamic stuff = JsonConvert.DeserializeObject(responseString);

                accessToken = stuff["access_token"];
            }

            if (accessToken.Trim().Length == 0)
                throw new Exception("There is no Access Token");

            return accessToken;
        }

        public static void PostMessage(RageRoom room)
        {
            try
            {
                string accessToken = fb_exchange_token;
                FacebookClient facebookClient = new FacebookClient(accessToken);

                dynamic messagePost = new ExpandoObject();
                messagePost.access_token = accessToken;
                messagePost.message = "Check Out The New Rage Room: " + room.Name + "\n\n Now Only In: " + room.PricePerParticipant + "₪ for participant.";

                string url = string.Format("/{0}/feed", PageID);
                var result = facebookClient.Post(url, messagePost);
            }
            catch
            {
            }
        }
    }
}