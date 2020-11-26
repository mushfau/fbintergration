using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBIntergrationApi.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FBIntergrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly SocialmediaoriginalpostContext _socialmediaoriginalpostcontext;
        private readonly SocialmediacommentthreadContext _socialmediacommentthreadcontext;
        private readonly SocialmediasubcommentContext _socialmediasubcommentcontext;

        private readonly IHttpContextAccessor _accessor;
        private readonly IHttpClientFactory _clientFactory;
        public CommentsController(
            SocialmediaoriginalpostContext socialmediaoriginalpostcontext,
            SocialmediacommentthreadContext socialmediacommentthreadcontext,
            SocialmediasubcommentContext socialmediasubcommentcontext,
             IHttpContextAccessor accessor,
             IHttpClientFactory clientFactory)
        {
            _socialmediaoriginalpostcontext = socialmediaoriginalpostcontext;
            _socialmediacommentthreadcontext = socialmediacommentthreadcontext;
            _socialmediasubcommentcontext = socialmediasubcommentcontext;
            _accessor = accessor;
            _clientFactory = clientFactory;
        }

        // GET: api/Comments/Facebook
        [HttpGet]
        [Route("Facebook")]
        public string VerifyFbWebhook()
        {
            Console.WriteLine("Facebook");
            return _accessor.HttpContext.Request.Query["hub.challenge"];
        }

        // POST: api/Comments/Subscribe
        [HttpPost]
        [Route("Subscribe")]
        public async Task<HttpResponseMessage> PageSubscribe(WebhookSubscribeDTO webhookSubscribeDTO)
        {
            var url = String.Concat(
                "https://graph.facebook.com/v8.0/",
                 webhookSubscribeDTO.page,
                 "/subscribed_apps?subscribed_fields=feed&access_token=",
                 webhookSubscribeDTO.token
                 );
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);
        }

        // POST: api/Comments/Facebook
        [HttpPost]
        [Route("Facebook")]
        public async Task PostFacebookComment(WebhookFacebookDTO webhookFacebookDTO)
        {

            if (webhookFacebookDTO.entry[0].changes[0].value.item == "status" && webhookFacebookDTO.entry[0].changes[0].value.verb == "add")
            {
                var socialmediaoriginalpost = new Socialmediaoriginalpost();
                socialmediaoriginalpost.subject = webhookFacebookDTO.entry[0].changes[0].value.from.name;
                socialmediaoriginalpost.createdt = String.Concat(webhookFacebookDTO.entry[0].changes[0].value.created_time, "");
                socialmediaoriginalpost.body = webhookFacebookDTO.entry[0].changes[0].value.message;
                socialmediaoriginalpost.originator = webhookFacebookDTO.entry[0].changes[0].value.from.id; // call GetCustomerIDFromName()
                socialmediaoriginalpost.originatorname = webhookFacebookDTO.entry[0].changes[0].value.from.name;
                socialmediaoriginalpost.disabled = 0;
                socialmediaoriginalpost.status = 0;
                socialmediaoriginalpost.channelID = 0; // to be taken fron initialization data ;
                socialmediaoriginalpost.image = ""; // call GetGUIDFromImageURL()
                socialmediaoriginalpost.orgID = 0; // to be taken fron initialization data ;
                socialmediaoriginalpost.sourceSocialMediaPostingID = webhookFacebookDTO.entry[0].changes[0].value.post_id;
                socialmediaoriginalpost.url = String.Concat("https://facebook.com/", webhookFacebookDTO.entry[0].changes[0].value.post_id);
                _socialmediaoriginalpostcontext.Socialmediaoriginalposts.Add(socialmediaoriginalpost);
                await _socialmediaoriginalpostcontext.SaveChangesAsync();
                return;

            }

            if (webhookFacebookDTO.entry[0].changes[0].value.item == "comment" && webhookFacebookDTO.entry[0].changes[0].value.verb == "add")
            {

                if (ParentExists(webhookFacebookDTO.entry[0].changes[0].value.parent_id))
                {
                    var posts = await _socialmediaoriginalpostcontext.Socialmediaoriginalposts
                    .Where(x => x.sourceSocialMediaPostingID == webhookFacebookDTO.entry[0].changes[0].value.parent_id).ToListAsync();
                    Console.Write("parent: ");
                    Console.WriteLine(posts[0].ID);

                    var socialmediacommentthread = new Socialmediacommentthread();
                    socialmediacommentthread.postID = posts[0].ID;
                    socialmediacommentthread.subject = webhookFacebookDTO.entry[0].changes[0].value.from.name;
                    socialmediacommentthread.createdt = String.Concat(webhookFacebookDTO.entry[0].changes[0].value.created_time, "");
                    socialmediacommentthread.body = webhookFacebookDTO.entry[0].changes[0].value.message;
                    socialmediacommentthread.originator = webhookFacebookDTO.entry[0].changes[0].value.from.id; // call GetCustomerIDFromName()
                    socialmediacommentthread.originatorname = webhookFacebookDTO.entry[0].changes[0].value.from.name;
                    socialmediacommentthread.disabled = 0;
                    socialmediacommentthread.status = 0;
                    socialmediacommentthread.channelID = 0; // to be taken fron initialization data ;
                    socialmediacommentthread.modifydt = null;
                    socialmediacommentthread.image = ""; // call GetGUIDFromImageURL()
                    socialmediacommentthread.orgID = 0; // to be taken fron initialization data ;
                    socialmediacommentthread.mood = 0;
                    socialmediacommentthread.activeAgent = -1;
                    socialmediacommentthread.sourceSocialMediaMsgID = webhookFacebookDTO.entry[0].changes[0].value.comment_id;
                    socialmediacommentthread.url = String.Concat("https://facebook.com/", webhookFacebookDTO.entry[0].changes[0].value.comment_id);
                    _socialmediacommentthreadcontext.Socialmediacommentthreads.Add(socialmediacommentthread);
                    await _socialmediacommentthreadcontext.SaveChangesAsync();
                }
                else
                {
                    var parentTickets = await _socialmediacommentthreadcontext.Socialmediacommentthreads
                    .Where(x => x.sourceSocialMediaMsgID == webhookFacebookDTO.entry[0].changes[0].value.parent_id).ToListAsync();
                    Console.Write("parent: ");
                    Console.WriteLine(parentTickets[0].ID);

                    var socialmediasubcomment = new Socialmediasubcomment();
                    socialmediasubcomment.parentTicket = parentTickets[0].ID;
                    socialmediasubcomment.subject = webhookFacebookDTO.entry[0].changes[0].value.from.name;
                    socialmediasubcomment.createdt = String.Concat(webhookFacebookDTO.entry[0].changes[0].value.created_time, "");
                    socialmediasubcomment.body = webhookFacebookDTO.entry[0].changes[0].value.message;
                    socialmediasubcomment.originator = webhookFacebookDTO.entry[0].changes[0].value.from.id; // call GetCustomerIDFromName()
                    socialmediasubcomment.originatorname = webhookFacebookDTO.entry[0].changes[0].value.from.name;
                    socialmediasubcomment.disabled = 0;
                    socialmediasubcomment.status = 0;
                    socialmediasubcomment.channelID = 0; // to be taken fron initialization data ;
                    socialmediasubcomment.modifydt = null;
                    socialmediasubcomment.image = ""; // call GetGUIDFromImageURL()
                    socialmediasubcomment.orgID = 0; // to be taken fron initialization data ;
                    socialmediasubcomment.mood = 0;
                    socialmediasubcomment.sourceSocialMediaMsgID = webhookFacebookDTO.entry[0].changes[0].value.comment_id;
                    socialmediasubcomment.url = String.Concat("https://facebook.com/", webhookFacebookDTO.entry[0].changes[0].value.comment_id);
                    _socialmediasubcommentcontext.Socialmediasubcomments.Add(socialmediasubcomment);
                    await _socialmediasubcommentcontext.SaveChangesAsync();
                }
                return;

            }

        }

        private bool ParentExists(string id)
        {
            return _socialmediaoriginalpostcontext.Socialmediaoriginalposts.Any(e => e.sourceSocialMediaPostingID == id);
        }

        // POST: api/Comments/Reply
        [HttpPost]
        [Route("Reply")]
        public async Task<HttpResponseMessage> CommentReply(CommentReplyDTO commentReplyDTO)
        {
            var url = String.Concat(
                "https://graph.facebook.com/v8.0/",
                 commentReplyDTO.commentId,
                 "/comments?message=",
                 commentReplyDTO.message,
                 "&access_token=",
                 commentReplyDTO.token
                 );
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);
        }

        // POST: api/Comments/Instagram
        [HttpPost]
        [Route("Instagram")]
        public async Task PostInstagramComment(WebhookInstagramDTO webhookInstagramDTO)
        {
            var socialmediacommentthread = new Socialmediacommentthread();
            socialmediacommentthread.postID = 0;
            socialmediacommentthread.subject = "";
            socialmediacommentthread.createdt = String.Concat(webhookInstagramDTO.entry[0].time, "");
            socialmediacommentthread.body = webhookInstagramDTO.entry[0].changes[0].value.text;
            socialmediacommentthread.originator = "";
            socialmediacommentthread.originatorname = "";
            socialmediacommentthread.disabled = 0;
            socialmediacommentthread.status = 0;
            socialmediacommentthread.channelID = 0;
            socialmediacommentthread.modifydt = null;
            socialmediacommentthread.image = "";
            socialmediacommentthread.orgID = 0;
            socialmediacommentthread.mood = 0;
            socialmediacommentthread.activeAgent = -1;
            socialmediacommentthread.sourceSocialMediaMsgID = webhookInstagramDTO.entry[0].changes[0].value.id;
            socialmediacommentthread.url = "";
            _socialmediacommentthreadcontext.Socialmediacommentthreads.Add(socialmediacommentthread);
            await _socialmediacommentthreadcontext.SaveChangesAsync();
            return;
        }



    }
}
