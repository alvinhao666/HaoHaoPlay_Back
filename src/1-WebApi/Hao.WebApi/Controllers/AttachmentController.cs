using Hao.Core;
using Hao.Core.AppController;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class AttachmentController : HController
    {
        public AttachmentController(IDistributedCache cache, IConfigurationRoot config, ICurrentUser currentUser) : base(cache, config, currentUser)
        {
        }
    }
}
