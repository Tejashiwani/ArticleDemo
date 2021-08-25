using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogHelix.Feature.BlogPostCard.Models;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace ArticleDemo.Feature.BlogPostCard.Controllers
{
    public class DynamicBlogCardController : Controller
    {

        // GET: Blogs
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getBlogs()
        {

            List<Sitecore.Data.Items.Item> articleItems = new List<Sitecore.Data.Items.Item>();
            List<BlogCard> BlogCardsCollection = new List<BlogCard>();
            var rootitem = Sitecore.Context.Database.GetItem("{80E32AE0-8954-4AE7-B646-C880009FC135}");
            var Websitesettings = Sitecore.Context.Database.GetItem("{27642ABA-3E3A-469D-8408-4473342EDD0B}");

            articleItems = rootitem.Axes.GetDescendants().ToList();

            for (int i = 0; i < articleItems.Count; i++)
            {
                BlogCard BlogModel = new BlogCard();
                var imageUrl = string.Empty;

                Sitecore.Data.Fields.ImageField imageField = articleItems[i].Fields["BlogSmallImage"];
                if (imageField?.MediaItem != null)
                {
                    var image = new MediaItem(imageField.MediaItem);
                    imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(image));
                    BlogModel.BlogSImage = imageUrl;
                }
                BlogModel.Category = articleItems[i].Fields["Category"].Value;
                BlogModel.BlogTitle = articleItems[i].Fields["BlogCardTitle"].Value;

                Sitecore.Data.Fields.DateField dateTimeField = articleItems[i].Fields["PostedDate"];

                string dateTimeString = dateTimeField.Value;

                DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                BlogModel.date = dateTimeStruct.ToShortDateString();

                BlogModel.ShortDesc = articleItems[i].Fields["ShortDescription"].Value;
                BlogModel.Readonbtn = Websitesettings.Fields["BlogCardButtonText"].Value;
                BlogModel.sitecoreItem = articleItems[i];
                BlogModel.BlogURL = Sitecore.Links.LinkManager.GetItemUrl(articleItems[i]);


                BlogCardsCollection.Add(BlogModel);
            }

            ViewBag.BlogCards = BlogCardsCollection;
            return View("~/Views/DynamicBlogCard/Articles.cshtml", ViewBag.BlogCards);
        }

    }
}