using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UIMatrixML.Modeling
{
    public class SimpleMLModel : BaseMLModel
    {
        [NotMapped]
        IModelDefinition Model { get; set; }

        /// <summary>
        /// Make a prediction.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="features">Feature values.</param>
        /// <returns>Probability matrix is valid.</returns>
        public override double Predict(IList<double> data) =>
            this.Predict<SimpleMLModel>(data);

        /// <summary>
        /// Make a prediction on current UI.
        /// </summary>
        /// <returns>Matrix.</returns>
        public double Predict() =>
            this.Predict<SimpleMLModel>(Core.UIMatrix.CalculateMatrices(this.Model.WebApplication));

        public SimpleMLModel()
        {

        }

        public SimpleMLModel(bool init = false)
        {
            if (init)
                this.Init();
        }


        public SimpleMLModel(IModelDefinition model, bool init = false)
        {
            this.Model = model;
            if (init)
            {
                // Initial ML Model Definition
                this.Init();

                // Load CSV Data
                this.LoadDataFromCsv(csvFilePath: Resources.ResourceManager.GetString("modelDataFilePath"), hasHeader: true);
            }
        }
    }
}
