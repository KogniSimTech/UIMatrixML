using System.Collections.Generic;
using UIMatrixML.Application;

namespace UIMatrixML.Modeling
{
    public interface IModelDefinitionState
    {
        IList<IModelDefinitionAction> Actions { get; set; }

        IList<double> RunValidation(IWebApplication webApplication, IMLModel mlModel, IModelDefinition modelDefiner, IModelDefinitionState validModel, bool valid);
    }
}
