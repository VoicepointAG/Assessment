namespace Voicepoint.CliTools.WebApiClientGenerator
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NJsonSchema.CodeGeneration.TypeScript;
    using NSwag.CodeGeneration.TypeScript;
    using NSwag.SwaggerGeneration.WebApi;

    public static class WebApiClientGenerator
    {
        public static async Task<string> GenerateWebApiClientFileContentAsync(string dllPath)
        {
            var swaggerSettings = new WebApiToSwaggerGeneratorSettings()
            {
                IsAspNetCore = true,
                SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
            var swaggerGenerator = new WebApiToSwaggerGenerator(swaggerSettings);

            var controllers = LoadTypes(dllPath);

            var swaggerDocument = await swaggerGenerator.GenerateForControllersAsync(controllers);

            var clientSettings = new SwaggerToTypeScriptClientGeneratorSettings
            {
                InjectionTokenType = InjectionTokenType.InjectionToken,
                Template = TypeScriptTemplate.Angular,
                GenerateClientInterfaces = true,
                TypeScriptGeneratorSettings =
                {
                    TypeScriptVersion = 3.2m,
                    DateTimeType = TypeScriptDateTimeType.Date,
                    NullValue = TypeScriptNullValue.Undefined
                }
            };

            var clientGenerator = new SwaggerToTypeScriptClientGenerator(swaggerDocument, clientSettings);


            var moduleCode = GenerateWebApiClientModule(controllers);
            var apiClient = "// @ts-nocheck" + Environment.NewLine + "//@formatter:off" + Environment.NewLine + clientGenerator.GenerateFile() + moduleCode;
            return apiClient;
        }

        private static string GenerateWebApiClientModule(Type[] controllers)
        {
            var exportTypes = controllers.Select(c => c.Name.Substring(0, c.Name.Length - "Controller".Length) + "Client");
            var exports = string.Join(",\r\n\t\t", exportTypes);

            var separator = string.IsNullOrEmpty(exports) ? "" : ",";

            return
@"

import { NgModule } from '@angular/core';

@NgModule({
    providers: [
        " + exports + separator + @"
        {
            provide: API_BASE_URL,
            useValue: ''
        }
    ]
})
export class WebApiModule { }
";
        }

        private static Type[] LoadTypes(string dllPath)
        {
            var webAssembly = Assembly.LoadFrom(dllPath);
            return webAssembly
                .GetTypes()
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();

        }
    }
}
