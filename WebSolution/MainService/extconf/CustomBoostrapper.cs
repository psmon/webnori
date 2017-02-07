using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;
using Nancy.Conventions;

namespace MainService.extconf
{
    public class CustomBoostrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);
            var rootPath = RootPathProvider.GetRootPath();
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("", rootPath + @"\public")
            );
        }
    }
}
