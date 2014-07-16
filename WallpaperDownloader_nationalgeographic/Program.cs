
using HtmlAgilityPack;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WallpaperDownloader_nationalgeographic
{
    class Program
    {
        const string app_identifier = "WallpaperDownloader_nationalgeographic";

        static void Main(string[] args)
        {

            #region Html Structure

            // Load HTML source into a string

            //   <div class="primary_photo">

            //    <a title="Go to the previous Photo of the Day" href="/photography/photo-of-the-day/elephant-pair-kadur/">


            //<img width="990" height="742" alt="Picture of a porcelain crab and an anemone" src="http://images.nationalgeographic.com/wpf/media-live/photos/000/692/cache/porcelain-crab-anemone_69224_990x742.jpg">	


            //    </a>
            //</div><!-- .primary_photo-->

            // Bing Image Result for Cat, First Page

            #endregion

            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["URL"];

                // For speed of dev, I use a WebClient
                WebClient client = new WebClient();
                string html = client.DownloadString(url);

                // Load the Html into the agility pack
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(html);

                // Now, using LINQ to get all Images
                List<HtmlNode> imageNodes = null;
                imageNodes = (from HtmlNode node in doc.DocumentNode.SelectNodes("//img")
                              where node.Name == "img"
                              && node.Attributes["src"] != null
                              && node.Attributes["src"].Value.StartsWith("http://images.nationalgeographic.com")
                              select node).ToList();


                if (imageNodes.Count > 0)
                {
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\temp_nationalgeographic";
                    client.DownloadFile(imageNodes[0].Attributes["src"].Value, tempPath);

                    string style = System.Configuration.ConfigurationManager.AppSettings["STYLE"];

                    switch (style)
                    {
                        case "FILLED":
                            Wallpaper.Set(tempPath, Wallpaper.Style.Filled);
                            break;
                        case "CENTERED":
                            Wallpaper.Set(tempPath, Wallpaper.Style.Centered);
                            break;
                        case "FITTED":
                            Wallpaper.Set(tempPath, Wallpaper.Style.Fitted);
                            break;
                        case "STRETCHED":
                            Wallpaper.Set(tempPath, Wallpaper.Style.Stretched);
                            break;
                        case "TILED":
                            Wallpaper.Set(tempPath, Wallpaper.Style.Tiled);
                            break;
                        default:
                            Wallpaper.Set(tempPath, Wallpaper.Style.Fitted);
                            break;
                    }


                }
                else
                {
                    System.Diagnostics.EventLog.WriteEntry(app_identifier, "No Identifiable Image Structure. Unable to idenify the image URL", System.Diagnostics.EventLogEntryType.Warning);
                }


            }
            catch (Exception e)
            {
                if (!System.Diagnostics.EventLog.SourceExists(app_identifier))
                    System.Diagnostics.EventLog.CreateEventSource(app_identifier, "Application");
                System.Diagnostics.EventLog.WriteEntry(app_identifier, e.Message, System.Diagnostics.EventLogEntryType.Warning);
            }


        }
    }
}
