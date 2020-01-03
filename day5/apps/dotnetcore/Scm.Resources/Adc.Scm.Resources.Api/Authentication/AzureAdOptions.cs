namespace Adc.Scm.Resources.Api.Authentication
{
    public class AzureAdOptions
    {
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientIdUri { get; set; }
    }
}