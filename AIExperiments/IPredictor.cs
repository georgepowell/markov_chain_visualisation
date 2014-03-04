using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExperiments
{
    interface IPredictor
    {
        int Prediction();
        void Read(int value);
    }
}
