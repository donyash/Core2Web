using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace WebApp.Controllers
{
    public class PaymentStationController : Controller
    {
        private readonly ApplicationSettings _applicationSettings;

        public PaymentStationController(IOptions<ApplicationSettings> applicationSettings)
        {
            _applicationSettings = applicationSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> PaymentStations()
        {
            ViewData["Message"] = "Pay Stations";
            var viewModel = new PayStationViewModel();

            //use this for MuleSoft static data
            //var baseUri = _applicationSettings.MockServiceStaticResponses.BaseUri.PayStations;
            //var endpoint = "/api/PayStation/GetAllPayStations";

            //use this for the WebApi solution
            //var baseUri = _applicationSettings.WebApi.BaseUri.PayStations;
            //var endpoint = "/v1/PayStation/GetAllPaymentStations";

            //use this for the Azure hosted WebApi solution
            var baseUri = _applicationSettings.AzureWebApi.BaseUri.PayStations;
            var endpoint = "/v1/PayStation/GetAllPaymentStations";

            using (var client = new HttpClient())
            {
                List<PaymentStationRecord> payStationList = await RunAsyncPayStations(client, baseUri, endpoint);
                viewModel.PayStations = payStationList;
            }

            return View(viewModel);
        }

        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PaymentStationById(string id)
        {
            ViewData["Message"] = "Pay Stations By Id";
            var viewModel = new PayStationViewModel();

            //use this for MuleSoft static data
            //var baseUri = _applicationSettings.MockServiceStaticResponses.BaseUri.PayStations;
            //var endpoint = "/api/PayStation/GetAllPayStations";

            //use this for the WebApi solution
            var baseUri = _applicationSettings.WebApi.BaseUri.PayStations;
            var endpoint = "/v1/PayStation/GetAllPaymentStations";

            using (var client = new HttpClient())
            {
                List<PaymentStationRecord> payStationList = await RunAsyncPayStations(client, baseUri, endpoint);

                PaymentStationRecord paystation = payStationList.FirstOrDefault(x => x.PayStationId.Equals(id));

                var payStationListById = new List<PaymentStationRecord>();
                payStationListById.Add(paystation);

                viewModel.PayStations = payStationListById;
            }


            return View("PaymentStations", viewModel);
        }


        [Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPaymentStationById(string id)
        {
            ViewData["Message"] = "Pay Stations By Id";
            var viewModel = new PayStationViewModel();

            var baseUri = _applicationSettings.WebApi.BaseUri.PayStations;
            var endpoint = "/v1/PayStation/GetPaymentStationById" + "?id=" + id;

            using (var client = new HttpClient())
            {
                var payStationList = await RunAsyncPayStations(client, baseUri, endpoint);

                PaymentStationRecord paystation = payStationList.FirstOrDefault(x => x.PayStationId.Equals(id));

                var payStationListById = new List<PaymentStationRecord>();
                payStationListById.Add(paystation);

                viewModel.PayStations = payStationListById;
            }


            return View("PaymentStations", viewModel);
        }


        #region test
        public async Task<IActionResult> Test()
        {
            ViewData["Message"] = "Test Pay Stations";
            var viewModel = new PayStationViewModel();

            var baseUri = _applicationSettings.AzureWebApi.BaseUri.PayStations;
            var endpoint = "/v1/PayStation/Test";

            using (var client = new HttpClient())
            {
                List<PaymentStationRecord> payStationList = await RunAsyncPayStations(client, baseUri, endpoint);
                viewModel.PayStations = payStationList;
            }

            return View(viewModel);
        }
        #endregion


        #region privates

        private async Task<List<PaymentStationRecord>> RunAsyncPayStations(HttpClient client, string baseUri, string endpoint)
        {
            SetUpUri(client, baseUri, endpoint);
            var stations = await GetPayStationAsync(client.BaseAddress.ToString(), client);

            return stations;
        }
        private async Task<List<PaymentStationRecord>> GetPayStationAsync(string path, HttpClient client)
        {
            List<PaymentStationRecord> stations = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                stations = await response.Content.ReadAsAsync<List<PaymentStationRecord>>();
            }

            return stations;
        }

        private void SetUpUri(HttpClient client, string baseUri, string endpoint)
        {
            client.BaseAddress = new Uri(baseUri + endpoint);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //from session
            var tokenString = HttpContext.Session.GetString("AuthorizationToken");

            //from token
            if (Request.Cookies["UserToken"] != null)
            {
                string userTokenString;
                if (Request.Cookies["UserToken"] != null)
                {
                    userTokenString = Request.Cookies["UserToken"];
                    client.DefaultRequestHeaders.Add("Authorization", userTokenString);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userTokenString);

                }
            }

            //session  - not used, prefer cookie value for persistence
            //client.DefaultRequestHeaders.Add("Authorization", tokenString);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

        }




        #endregion


    }
}