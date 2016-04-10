using System;
using System.IO;

namespace DeathCounter.Lib
{
    class DeathInputOutputFileHandler : IDeathInputOutHandler
    {
        private string applicationName;
        private string game;
        public string ApplicationName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.applicationName))
                {
                    return this.applicationName;
                }
                else
                {
                    return "DeathCounter";
                }
            }
        }

        public string Game
        {
            get
            {
                if (!string.IsNullOrEmpty(this.game))
                {
                    return game;
                }
                else
                {
                    return "deaths";
                }
            }

            set
            {
                game = value;
            }
        }

        public string DeathCounterFile
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), string.Format("{0}\\{1}.txt",this.ApplicationName,this.Game));
            }
        }

        /// <summary>
        /// Initializes the output handler
        /// </summary>
        /// <param name="applicationName">Contains the const APPLICATIONNAME</param>
        /// <param name="game">the game that is currently being played</param>
        public void Init(string applicationName, string game)
        {
            this.applicationName = applicationName;
            this.game = game;
        }

        /// <summary>
        /// reloads the previous amounts of deaths
        /// </summary>
        /// <returns>The amount of deaths or 0 if there aren't any previous deaths to be found</returns>
        public int loadPreviousDeaths()
        {
            int deaths = 0;
            if (File.Exists(this.DeathCounterFile))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(this.DeathCounterFile, false))
                {
                    string deathLine = file.ReadLine();
                    if (!Int32.TryParse(deathLine, out deaths))
                    {
                        deaths = 0;
                    }
                    file.Close();
                }
            }
            return deaths;
        }

        /// <summary>
        /// updates the amount of deaths
        /// </summary>
        /// <param name="numberOfDeaths">The amount of deaths</param>
        public void UpdateDeaths(int numberOfDeaths)
        {
            if (CreateAppdataFolder())
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.DeathCounterFile, false))
                {
                    file.WriteLine(numberOfDeaths.ToString());
                    file.Close();
                }
            }
        }

        /// <summary>
        /// creates the application folder that contains the death counter files
        /// </summary>
        /// <returns></returns>
        private bool CreateAppdataFolder()
        {
            bool result = false;
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.ApplicationName)))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.applicationName));
            }
            result = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.applicationName));
            return result;
        }
    }
}
