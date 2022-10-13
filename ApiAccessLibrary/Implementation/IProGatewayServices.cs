using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ApiAccessLibrary.Implementation
{
    public class IProGatewayServices : IiProGatewayServices
    {
        public Task<string> PostIProGateway()
        {
            //setup some variables

            String security_key = "6457Thfj624V5r7WUwc5v6a68Zsd6YEm";
            String firstname = "John";
            String lastname = "Smith";
            String address1 = "1234 Main St.";
            String city = "Chicago";
            String state = "IL";
            String zip = "60193";

            String strPost = "security_key=" + security_key
               + "&firstname=" + firstname + "&lastname=" + lastname
               + "&address1=" + address1 + "&city=" + city + "&state=" + state
               + "&zip=" + zip + "&payment=creditcard&type=sale"
               + "&amount=1.00&ccnumber=4111111111111111&ccexp=1015&cvv=123";
            return ReadHtmlPageAsync("https://secure.iprogateway.com/api/transact.php", strPost);
        }






        private static async Task<String> ReadHtmlPageAsync(string url, string post)
        {
            String result = "";
            String strPost = post;
            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
               new StreamReader(objResponse.GetResponseStream()))
            {
                result = await sr.ReadToEndAsync();

                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }


    }
}
