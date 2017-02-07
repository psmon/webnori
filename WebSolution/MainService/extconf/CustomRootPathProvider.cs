using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;
using Nancy.Conventions;
using System.Configuration;

namespace MainService.extconf
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {            
            string rootPath = ConfigurationManager.AppSettings.Get(@"WebPublicDir");
            //rootPath = @"C:\project\webapp\webnori\src\WebSolution\MainService";
            return rootPath;
        }
    }
}
