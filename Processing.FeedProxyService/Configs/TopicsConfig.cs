using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.Configs
{
    public class TopicsConfig : List<string>
    {
        public TopicsConfig() : base()
        { }

        public TopicsConfig(IEnumerable<string> topics) : base(topics)
        { }
    }
}
