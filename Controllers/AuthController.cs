using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FBIntergrationApi.Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FBIntergrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IHttpContextAccessor _accessor;
        private readonly IHttpClientFactory _clientFactory;
        public AuthController(IConfiguration configuration, IHttpContextAccessor accessor, IHttpClientFactory clientFactory)
        {
            Configuration = configuration;
            _accessor = accessor;
            _clientFactory = clientFactory;
        }




        // GET: api/auth/facebook/callback
        [HttpGet]
        [Route("facebook/callback")]
        public IActionResult VerifyFbWebhook()
        {
            return Redirect("/");
        }

        // GET: api/auth/facebook/token
        [HttpPost]
        [Route("facebook/token")]
        public async Task<PageTokenResponse> VerifyFbWebhook(PageTokenGenerateDTO pageTokenGenerateDTO)
        {

            try
            {
                var base_url = "https://graph.facebook.com/";
                var app_id = Configuration["clientID"];
                var app_secret = Configuration["clientSecret"];

                var url = String.Concat(base_url, "oauth/access_token?grant_type=fb_exchange_token&client_id=", app_id, "&client_secret=", app_secret, "&fb_exchange_token=",
                 pageTokenGenerateDTO.token);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var longLiveTokenResponse = await response.Content.ReadFromJsonAsync<LongLiveTokenResponse>();
                    url = String.Concat(base_url, pageTokenGenerateDTO.page, "?fields=access_token", "&access_token=", longLiveTokenResponse.access_token);
                    Console.WriteLine(url);
                    Console.WriteLine("");
                    request = new HttpRequestMessage(HttpMethod.Get, url);
                    client = _clientFactory.CreateClient();
                    var response2 = await client.SendAsync(request);
                    response2.EnsureSuccessStatusCode();
                    if (response2.IsSuccessStatusCode)
                    {
                        var pageTokenResponse = await response2.Content.ReadAsStringAsync();

                        return JsonSerializer.Deserialize<PageTokenResponse>(pageTokenResponse); ;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }


    }
}
