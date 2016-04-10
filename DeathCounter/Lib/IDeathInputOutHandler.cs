using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeathCounter.Lib
{
    /// <summary>
    /// the signature of input and output handlers
    /// </summary>
    interface IDeathInputOutHandler
    {
        /// <summary>
        /// Initializes the output handler
        /// </summary>
        /// <param name="applicationName">Contains the const APPLICATIONNAME</param>
        /// <param name="game">the game that is currently being played</param>
        void Init(string applicationName, string game);
        /// <summary>
        /// updates the amount of deaths
        /// </summary>
        /// <param name="numberOfDeaths">The amount of deaths</param>
        void UpdateDeaths(int numberOfDeaths);
        /// <summary>
        /// reloads the previous amounts of deaths
        /// </summary>
        /// <returns>The amount of deaths or 0 if there aren't any previous deaths to be found</returns>
        int loadPreviousDeaths();
    }
}
