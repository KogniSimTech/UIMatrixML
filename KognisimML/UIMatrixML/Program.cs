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
            
            // Initialize Web Application at URL http://localhost/
            IWebApplication webApplication =
                new WebApplication("http://localhost/", init: true);

            // Create model definition.
            ModelDefinition modelDefinition = 
                ModelDefinition.New(webApplication, Resources.ResourceManager.GetString("modelDefinitionsFilePath"));

            // Load SimpleMLModel definition.
            IMLModel mlModel =
                new SimpleMLModel(modelDefinition, init: true);

            // Run validation.
            mlModel.Definition.Train(webApplication, mlModel);

            // Make a prediction based on how UI looks now.
            mlModel.Predict(Core.UIMatrix.CalculateMatrices(webApplication));
        }
    }
}