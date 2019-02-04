using System.Collections.Generic;
using UIMatrixML.Application;

namespace UIMatrixML.Modeling
{
    public interface IModelDefinitionState
    {
        IList<IModelDefinitionAction> Actions { get; set; }

        IList<double> RunValidation(IWebApplication webApplication, IMLModel mlModel, IModelDefinition modelDefinition, IModelDefinitionState definitionState, bool valid);
    }
}
