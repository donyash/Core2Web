using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
        public class ApplicationSettings
        {
            public MockServiceStaticResponses MockServiceStaticResponses { get; set; }
            public WebApi WebApi { get; set; }
            public AzureWebApi AzureWebApi { get; set; }
            public Messages Messages { get; set; }
            public AzureWebApiContainer AzureWebApiContainer { get; set; }

    }

    public class MockServiceStaticResponses
            {
                public BaseUri BaseUri { get; set; }
            }

        public class BaseUri
        {
            public string Crms { get; set; }
            public string PayStations { get; set; }
        }

        public class WebApi
        {
            public BaseUri BaseUri { get; set; }
        }

        public class AzureWebApi
        {
            public BaseUri BaseUri { get; set; }
        }

        public class Messages
        {
            public string Environment { get; set; }

        }
    public class AzureWebApiContainer
    {
        public BaseUri BaseUri { get; set; }
    }

}
