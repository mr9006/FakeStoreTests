using System;
using System.Collections.Generic;
using System.Text;

namespace FakeStoreTests.Config
{
    public class Configuration
    {
        public string Browser { get; set; }
        public bool IsRemote { get; set; }
        public Uri RemoteAddress { get; set; }

        public string BaseUrl { get; set; }
    }
}
