﻿namespace UIMatrixML.Modeling
{
    public class SimpleMLModel : BaseMLModel
    {
        IModelDefinition Model { get; set; }

        /// <summary>
        /// Make a prediction.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="features">Feature values.</param>
        /// <returns>Probability matrix is valid.</returns>
        public override double Predict<TModel>(double[] data) =>
            base.Predict<SimpleMLModel>(data);

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