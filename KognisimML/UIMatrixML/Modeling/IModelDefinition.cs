using System;
using System.Collections.Generic;
using System.Text;
using UIMatrixML.Application;

namespace UIMatrixML.Modeling
{
    public interface IModelDefinition
    {
        IWebApplication WebApplication { get; }

        IList<string> Watchers { get; set; }
        IList<IModelDefinitionState> Valid { get; set; }
        IList<IModelDefinitionState> Invalid { get; set; }

        void BindWebApplication(IWebApplication webApplication);

        void Train(IWebApplication webApplication, IMLModel mlModel);
    }
}
