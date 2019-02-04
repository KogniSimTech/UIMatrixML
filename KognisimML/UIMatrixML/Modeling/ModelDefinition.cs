using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UIMatrixML.Application;

namespace UIMatrixML.Modeling
{
    public class ModelDefinition : IModelDefinition
    {
        private IWebApplication webApplication { get; set; }

        public IWebApplication WebApplication =>
            this.webApplication;

        public IList<string> Watchers { get; set; }
        public IList<IModelDefinitionState> Valid { get; set; }
        public IList<IModelDefinitionState> Invalid { get; set; }

        public static ModelDefinition New(IWebApplication webApplication, string jsonFilePath = null)
        {
            ModelDefinition modelDef;
            if (string.IsNullOrWhiteSpace(jsonFilePath))
            {
                modelDef = new ModelDefinition();
                modelDef.BindWebApplication(webApplication);
            }
            else
            {
                string modelDefinerTemplate = File.ReadAllText(jsonFilePath);
                modelDef = JsonConvert.DeserializeObject<ModelDefinition>(modelDefinerTemplate, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                modelDef.BindWebApplication(webApplication);
            }

            return modelDef;

        }

        public void BindWebApplication(IWebApplication webApplication)
        {
            this.webApplication = webApplication;
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
