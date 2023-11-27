using MimeKit;
using Newtonsoft.Json;
using System.Configuration;
using MailKit.Net.Smtp;

namespace Canvas_Texting
{
    internal class Texting
    {
        static async Task Main(string[] args)
        {
            //For each class, the word "Fall" appears after the course name. This helps cut it off for spacing/text formatting reasons.
            string nameCutOff = "Fall";
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");
            string jsonData = await GetToDo();
            string message = "Assignments due today:\n";
            bool noAssignments = true;

            try
            {
                List<ToDoItem>? items = JsonConvert.DeserializeObject<List<ToDoItem>>(jsonData);

                if (items == null)
                    message = "Issue Deserializing Json.";
                else
                    foreach (ToDoItem item in items)
                    {
                        string dueDate = item.assignment.all_dates[0].due_at.Substring(0,10);
                        if (dueDate == currDate)
                        {
                            int cutoff = item.context_name.IndexOf(nameCutOff);

                            message += item.context_name.Substring(0, cutoff - 1) + ":\n" + item.assignment.name + "\n\n";                           
                            noAssignments = false;
                        }
                    }
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }

            if (noAssignments)
                message = "Nothing due today!";

            SendText(message);
            
        }

        /// <summary>
        /// Sends a text to the specified number with the specified message
        /// </summary>
        /// <param name="message"></param>
        public static void SendText(string message)
        {
            string? emailAdd = ConfigurationManager.AppSettings.Get("EmailAdd");

            var mimeMess = new MimeMessage();

            mimeMess.From.Add(new MailboxAddress("Canvas Texts", emailAdd));
            mimeMess.To.Add(new MailboxAddress("Zach", ConfigurationManager.AppSettings.Get("PhoneNum")));

            mimeMess.Body = new TextPart("plain")
            {
                Text = message
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();

            client.Connect("smtp.gmail.com");
            client.Authenticate(emailAdd, ConfigurationManager.AppSettings.Get("EmailPW"));
            client.Send(mimeMess);
            client.Disconnect(true);
        }

        /// <summary>
        /// Connects to the Canvas API using the specified Access Token to retrieve the "TODO" tasks
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetToDo()
        {
            var client = new HttpClient();
            string? token = ConfigurationManager.AppSettings.Get("AccessToken");

            var requestContent = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://utah.instructure.com/api/v1/users/self/todo?access_token=" + token)
            };

            using (var response = await client.SendAsync(requestContent))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }
}