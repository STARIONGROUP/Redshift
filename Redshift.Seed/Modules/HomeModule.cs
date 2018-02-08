namespace Redshift.Seed.Modules
{
    using Nancy;

    /// <summary>
    /// Home module
    /// </summary>
    public class HomeModule : NancyModule
    {
        public HomeModule() : base("/")
        {
            this.Get(
                "/",
                x =>
                {
                    return "Hello World!";
                });
        }
    }
}
