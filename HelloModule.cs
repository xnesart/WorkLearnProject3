using Nancy;

namespace WorkLearnProject3
{
    public class HelloModule:NancyModule
    {
        public HelloModule()
        {
            Get("/nancy-api/{name}", parameters =>
            {
                return string.Concat("Hello ", parameters.Name);
            });
        }
    }
}