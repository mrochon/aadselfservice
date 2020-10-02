using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AADSelfService.Controllers
{
    public class UserController : Controller
    {
        IConfidentialClientApplication _msal;
        private static readonly string[] _scopes = { "https://graph.microsoft.com/.default" };
        public UserController(IConfidentialClientApplication msal)
        {
            _msal = msal; 
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View(new Models.User());
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.User user)
        {
            try
            {
                var tokens = await _msal.AcquireTokenForClient(_scopes).ExecuteAsync().ConfigureAwait(false);
                var u = new
                {
                    accountEnabled = false,
                    displayName = user.DisplayName,
                    mailNickName = user.Id,
                    userPrincipalName = $"{user.Id}@meraridom.com",
                    passwordProfile = new
                    {
                        forceChangePasswordNextSignIn = true,
                        password = user.Password
                    }
                };
                var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
                var resp = await http.PostAsync("https://graph.microsoft.com/v1.0/users", new StringContent(JsonConvert.SerializeObject(u), Encoding.UTF8, "application/json"));
                if (!resp.IsSuccessStatusCode)
                {
                    var respBody = await resp.Content.ReadAsStringAsync();
                    var msg = JObject.Parse(respBody)["error"].Value<string>("message");
                    throw new Exception(msg);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
