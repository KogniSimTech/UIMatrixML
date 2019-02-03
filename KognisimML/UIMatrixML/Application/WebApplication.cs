using System;
using System.Collections.Generic;
using System.Text;

namespace UIMatrixML.Application
{
    public sealed class WebApplication : BaseWebApplication
    {
        public WebApplication(string url, bool init = false) : base(url)
        {
            if (init)
                this.Init();
        }
    }
}
