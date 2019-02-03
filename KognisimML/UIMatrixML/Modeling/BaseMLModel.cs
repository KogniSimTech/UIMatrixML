using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Collections;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Math;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UIMatrixML.Core;

namespace UIMatrixML.Modeling
{
    public abstract class BaseMLModel : IMLModel
    {
        /* PRIVATE */

        Variable<Vector> w;
        InferenceEngine engine;
        bool initialized = false;
        bool hasHeaders = false;

        /* PUBLIC */

        public static object mlModelCompilation = 1;

        public virtual IModelDefinition Definition { get; set; }

        public virtual double Noise { get; set; }

        public virtual IList<bool> Validity { get; set; }

        public virtual IList<double[]> Data { get; set; }

        public virtual VariableArray<bool> y { get; set; }

        public virtual bool IsInitialized =>
            initialized;

        public virtual int FeatureCount
        {
            get
            {
                if (Data == null || Data.Count < 1)
                    return 0;
                else
                    return Data[0].Length;
            }
        }

        /// <summary>
        /// Load data from CSV.
        /// </summary>
        /// <param name="csvFilePath">CSV file path.</param>
        /// <param name="hasHeader">CSV has headers.</param>
        public virtual void LoadDataFromCsv(string csvFilePath, bool hasHeader = false)
        {
            if (initialized == false)
                throw new Exception("ML Model has not been initialized yet.");

            lock (mlModelCompilation)
            {
                hasHeaders = hasHeader;

                IEnumerable<string> csvLines = File.ReadAllLines(csvFilePath);

                if (hasHeader)
                    csvLines = csvLines.Skip(1);

                foreach (string line in csvLines)
                {
                    IList<double> dict =
                        new List<double>();

                    string[] vals = line.Replace(" ", "").Split(",");

                    for (int i = 0; i < vals.Length - 1; i++)
                    {
                        dict.Add(Convert.ToDouble(vals[i]));
                    }

                    Validity.Add(Convert.ToBoolean(vals[vals.Length - 1]));

                    Data.Add(dict.ToArray());
                }

                // Create random variable w with VectorGuassian (of size 3)
                // Features => (Gold | Nickel | Copper) Concentration, IsGoodMaterial?
                w = Variable.Random(new VectorGaussian(Vector.Zero(FeatureCount), PositiveDefiniteMatrix.Identity(FeatureCount))).Named("w");

                // Create Observable variable based on valid_ui;
                y = Variable.Observed(Validity.ToArray());

                IMLModel that = this;
                VariableArray<bool> that_y = y;

                BayesPointMachine.Run(ref that, y, w);
            }

        }

        /// <summary>
        /// Write data entry to CSV file.
        /// </summary>
        /// <param name="csvFilePath">CSV file path.</param>
        /// <param name="features">Feature values.</param>
        /// <param name="validity">Validity state.</param>
        public virtual void WriteDataToCsv(string csvFilePath, IList<double> features, bool validity)
        {
            if (initialized == false)
                throw new Exception("ML Model has not been initialized yet.");

            lock (mlModelCompilation)
            {
                // Rebuild initial data template.
                IList<string> existingLines = File.ReadAllLines(csvFilePath).ToList();
                if (existingLines.Count() < 2 || existingLines.ElementAt(1).Split(",").Length - 1 != features.Count)
                {
                    existingLines = new List<string>();
                    existingLines.Add(features.Select(x => $"feature_{features.IndexOf(x)}").Aggregate((c, n) => c + "," + n) + ",feature_score");
                    existingLines.Add(features.Select(x => $"0").Aggregate((c, n) => c + "," + n) + $",False");

                    File.WriteAllLines(csvFilePath, existingLines);

                    LoadDataFromCsv(csvFilePath, true);
                }

                string featuresLine = features.Select(f => Convert.ToString(f))
                    .Aggregate((c, n) => c + "," + n) + $",{validity}";

                File.AppendAllLines(csvFilePath, new[] { featuresLine });

                LoadDataFromCsv(csvFilePath, hasHeaders);
            }

        }

        /// <summary>
        /// Initialize base ML model.
        /// </summary>
        public virtual void Init()
        {
            Data =
                new List<double[]>();

            Validity =
                new List<bool>();

            lock (mlModelCompilation)
            {
                engine = 
                    new InferenceEngine(new ExpectationPropagation());

                initialized = true;
            }
        }

        /// <summary>
        /// Make a prediction.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <param name="features">Feature values.</param>
        /// <returns>Probability matrix is valid.</returns>
        public virtual double Predict<TModel>(double[] features) where TModel : new()
        {
            if (initialized == false)
                throw new Exception("ML Model has not been initialized yet.");

            if (Data == null || Data.Count == 0)
                throw new Exception("Dataset has 0 instances.");

            if (features == null)
                throw new ArgumentNullException("data");

            lock (mlModelCompilation)
            {
                object model =
                    new TModel();

                ((BaseMLModel)model).Data = 
                    new List<double[]>() { features };

                VectorGaussian wPosterior = 
                    engine.Infer<VectorGaussian>(w);

                VariableArray<bool> ytest = 
                    Variable.Array<bool>(new Range(1));

                IMLModel that = 
                    model as IMLModel;

                BayesPointMachine.Run(ref that, ytest, Variable.Random(wPosterior).Named("w"));

                return (engine.Infer(ytest) as IEnumerable<Bernoulli>)
                    .First()
                    .GetProbTrue();
            }

        }
        
        public BaseMLModel()
        {
        }
    }
}
