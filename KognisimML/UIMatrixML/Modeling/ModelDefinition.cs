using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UIMatrixML.Application;

namespace UIMatrixML.Modeling
{
    public class ModelDefinition : IModelDefinition
    {
        public IList<string> Watchers { get; set; }
        public IList<IModelDefinitionState> Valid { get; set; }
        public IList<IModelDefinitionState> Invalid { get; set; }

        public static ModelDefinition New(string jsonFilePath = null)
        {
            if (string.IsNullOrWhiteSpace(jsonFilePath))
                return new ModelDefinition();

            string modelDefinerTemplate = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<ModelDefinition>(modelDefinerTemplate, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public void Train(IWebApplication webApplication, IMLModel mlModel)
        {
            foreach(string watcher in this.Watchers)
            {
                Core.UIMatrix.AddElement(webApplication, watcher);
            }

            foreach (ModelDefinitionState validModel in this.Valid)
            {
                var validation = validModel.RunValidation(webApplication, mlModel,
                    this, validModel, true);
#if TRACE
                Console.WriteLine($"Validation matrix: {validation}");
#endif
            }

            foreach (ModelDefinitionState validModel in this.Invalid)
            {
                var validation = validModel.RunValidation(webApplication, mlModel,
                    this, validModel, false);
#if TRACE
                Console.WriteLine($"Validation matrix: {validation}");
#endif
            }
        }

        public ModelDefinition()
        {
            this.Valid =
                new List<IModelDefinitionState>();

            this.Invalid =
                new List<IModelDefinitionState>();
        }
    }
}
