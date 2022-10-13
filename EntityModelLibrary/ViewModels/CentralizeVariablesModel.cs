namespace EntityModelLibrary.ViewModels
{
    public class CentralizeVariablesModel
    {
        public Outlet Outlet { get; set; } = new Outlet();
        public string DbEnvironment { get; set; }
        public InstaMedCredentials InstaMedCredentials { get; set; } = new InstaMedCredentials();

    }

    public class InstaMedCredentials
    {
        public string BaseAddress { get; set; }
        public string APIkey { get; set; }
        public string APIsecret { get; set; }
    }
    public class Outlet
    {
        public string MerchantID { get; set; }
        public string StoreID { get; set; }
        public string TerminalID { get; set; }
    }
}
