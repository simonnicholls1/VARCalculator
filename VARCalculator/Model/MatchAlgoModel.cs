using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogicSearch.Model
{
    public class MatchAlgoModel
    {
        public string MatchAlgo { get; set; }

        public MatchAlgoModel(string matchAlgo)
        {            
            this.MatchAlgo = matchAlgo;
        }
    }
}
