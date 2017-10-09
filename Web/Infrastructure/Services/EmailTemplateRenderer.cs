using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Mustache;

namespace Web.Infrastructure.Services
{
    public class EmailTemplateRenderer : IEmailTemplateRenderer
    {
        public string Render(string relativePathToTemplate, object values)
        {
            var template = GetFileContents(GetFullPathToTemplate(relativePathToTemplate));
            FormatCompiler compiler = new FormatCompiler();
            var generator = compiler.Compile(template);

            var body = generator.Render(values);
            return body;
        }

        private string GetFullPathToTemplate(string pathToTemplate)
        {
            if (string.IsNullOrWhiteSpace(pathToTemplate))
                return null;
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure", "EmailTemplates", pathToTemplate);
        }

        private String GetFileContents(String fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                return null;
            return File.ReadAllText(fileName);
        }
    }

    public interface IEmailTemplateRenderer
    {
        string Render(string relativePathToTemplate, object values);
    }
}