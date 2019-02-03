using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UIMatrixML.Application;
using UIMatrixML.Modeling;

namespace UIMatrixML
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create model definition.
            var modelDefinition = 
                ModelDefinition.New(Resources.ResourceManager.GetString("modelDefinitionsFilePath"));

            // Load SimpleMLModel definition.
            IMLModel mlModel =
                new SimpleMLModel(modelDefinition, init: true);
            
            // Initialize Web Application at URL http://localhost/
            IWebApplication webApplication =
                new WebApplication("http://localhost/", init: true);

            // Run validation.
            mlModel.Definition.Train(webApplication, mlModel);
        }
    }
}
