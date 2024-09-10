using Nancy;

namespace WorkLearnProject3
{
    public class ByeModule:NancyModule
    {
        public ByeModule()
        {
            Get("/nancy-api/{name}/bye", parameters =>
            {
                return string.Concat("Bye ", parameters.Name);
            });
        }
    }
}