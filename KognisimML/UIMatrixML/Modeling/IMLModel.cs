using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIMatrixML.Modeling
{
    public interface IMLModel
    {
        IModelDefinition Definition { get; set; }

        double Noise { get; set; }

        bool IsInitialized { get; }

        int FeatureCount { get; }

        IList<bool> Validity { get; set; }

        IList<double[]> Data { get; set; }

        VariableArray<bool> y { get; set; }

        /// <summary>
        /// Load data from CSV.
        /// </summary>
        /// <param name="csvFilePath">CSV file path.</param>
        /// <param name="hasHeader">CSV has headers.</param>
        void LoadDataFromCsv(string csvFilePath, bool hasHeader = false);

        /// <summary>
        /// Write data entry to CSV file.
        /// </summary>
        /// <param name="csvFilePath">CSV file path.</param>
        /// <param name="features">Feature values.</param>
        /// <param name="validity">Validity state.</param>
        void WriteDataToCsv(string csvFilePath, IList<double> features, bool validity);

        /// <summary>
        /// Initialize base ML model.
        /// </summary>
        void Init();

        /// <summary>
        /// Make a prediction.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="features">Feature values.</param>
        /// <returns></returns>
        double Predict<TModel>(IList<double> data) where TModel : new();
        double Predict(IList<double> data);
    }
}
