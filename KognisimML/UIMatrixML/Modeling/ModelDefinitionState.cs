using System.Collections.Generic;
using UIMatrixML.Application;
using UIMatrixML.Application.DOM.Elements;

namespace UIMatrixML.Modeling
{
    public class ModelDefinitionState : IModelDefinitionState
    {
        public IList<IModelDefinitionAction> Actions { get; set; }

        /// <summary>
        /// Run 'valid' and 'invalid' validations.
        /// </summary>
        /// <param name="webApplication">Web application handler.</param>
        /// <param name="mlModel">ML model context.</param>
        /// <param name="modelDefiner">Model definer context.</param>
        /// <param name="validModel">Valid or invalid model context.</param>
        /// <param name="valid">Validity.</param>
        /// <returns>Matrix.</returns>
        public IList<double> RunValidation(IWebApplication webApplication, IMLModel mlModel, IModelDefinition modelDefiner, IModelDefinitionState validModel, bool valid)
        {
            Core.UIMatrix.Init(webApplication);

            foreach (string watcher in modelDefiner.Watchers)
            {
                Core.UIMatrix.AddElement(webApplication, watcher);
            }

            foreach (ModelDefinitionAction action in validModel.Actions)
            {
                switch (action.Action)
                {
                    case "SetValue":
                        Application.DOM.IDomElement genericElement =
                            new GenericElement(webApplication.Driver)
                            {
                                Name = action.Selector
                            };

                        (genericElement as GenericElement).SetValue(action.Value?.ToString());
                        break;
                }
            }

            IList<double> matrix =
                Core.UIMatrix.CalculateMatrices(webApplication);

            mlModel.WriteDataToCsv(csvFilePath: "Data/modelData.csv",
                features: matrix, validity: valid);

            webApplication.Driver.Navigate().GoToUrl(webApplication.Url);

            return matrix;
        }

        public ModelDefinitionState()
        {
            this.Actions = 
                new List<IModelDefinitionAction>();
        }
    }
}
