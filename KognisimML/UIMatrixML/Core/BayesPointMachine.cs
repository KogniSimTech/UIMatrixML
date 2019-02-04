using Microsoft.ML.Probabilistic.Math;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIMatrixML.Modeling;

namespace UIMatrixML.Core
{
    public static class BayesPointMachine
    {
        /// <summary>
        /// Run Bayes Point Machine.
        /// </summary>
        /// <param name="mlModel">Machine learning model context.</param>
        /// <param name="y">Y feature.</param>
        /// <param name="w">Features vector.</param>
        public static void Run(IMLModel mlModel, VariableArray<bool> y, Variable<Vector> w)
        {
            // Get Y feature range.
            Range j = y.Range.Named("metal");

            // Vector data of incomes length.
            Vector[] xdata = new Vector[mlModel.Data.Count()];
            
            // Vector data.
            xdata = (from dblArr in (from data in mlModel.Data
                        select (from d in data select d))
                    select Vector.FromList(dblArr.ToList())).ToArray();

            // Create observed variable based on xdata with validity range.
            VariableArray<Vector> obs = Variable.Observed(xdata, j).Named("obs");

            // Create noise.
            double noise = mlModel.Noise;
            y[j] = Variable.GaussianFromMeanAndVariance(Variable.InnerProduct(w, obs[j]).Named("innerProduct"), noise) > 0;
        }
    }
}
