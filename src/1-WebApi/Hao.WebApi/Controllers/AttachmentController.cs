using Hao.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class AttachmentController : HController
    {
        public AttachmentController(IConfiguration config) : base( config)
        {
        }
    }
}
