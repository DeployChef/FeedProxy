namespace Processing.FeedProxyService.Configs
{
    public class ProducerConfig
    {
        public string Environment { get; set; } = "Default";

        public string Urls { get; set; } = "http://localhost:4222/";

        public string ClusterId { get; set; } = "test-cluster";
    }
}
