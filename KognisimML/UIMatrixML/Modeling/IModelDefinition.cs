using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UIMatrixML.Application;

namespace UIMatrixML.Modeling
{
    public interface IModelDefinition
    {
        [NotMapped]
        IWebApplication WebApplication { get; }

        [NotMapped]
        IList<string> Watchers { get; set; }
        [NotMapped]
        IList<IModelDefinitionState> Valid { get; set; }
        [NotMapped]
        IList<IModelDefinitionState> Invalid { get; set; }

        [Key]
        Guid ID { get; set; }

        [Required]
        string Name { get; set; }

        void BindWebApplication(IWebApplication webApplication);

        void Train(IWebApplication webApplication, IMLModel mlModel);
    }
}
