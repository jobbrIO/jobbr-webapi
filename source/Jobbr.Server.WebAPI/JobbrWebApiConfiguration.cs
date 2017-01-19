namespace Jobbr.Server.WebAPI
{
    public class JobbrWebApiConfiguration
    {
        public JobbrWebApiConfiguration()
        {
            this.BackendAddress = "http://localhost:80/jobbr";

        }

        public string BackendAddress { get; set; }
    }
}