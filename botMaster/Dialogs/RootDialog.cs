using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Dialogs;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Bot_Application.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if ((activity.Text.Contains("[Weather Response]")) || activity.Text.Contains("[Stock]"))
            {
                // pass along changes activity text which is now the stock or weather alert
                await context.PostAsync(activity.Text);


            }


            if (activity.Text.ToLower().Contains("@screen"))
            {
                try
                {

		
		    Thread.Sleep(4000);	
                    Bitmap capture = CaptureScreen.GetDesktopImage();
                    string file = Path.Combine("C:\\Users\\Sai\\Desktop", "botScreenShot.png");
                    ImageFormat format = ImageFormat.Png;
                    capture.Save(file, format);

                    await context.PostAsync("screenshot saved to desktop");


                }
                catch (Exception e)
                {
                    await context.PostAsync(e.ToString());
                }

            }

            

            context.Wait(MessageReceivedAsync);
        }
    }
}
