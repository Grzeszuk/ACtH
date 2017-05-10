using System;
using System.Collections.Generic;
using System.Text;

namespace AWP.Teams
{
    public class Teams
    {
        private readonly List<String> _teamsNames;

        public Teams()
        {
            _teamsNames = new List<string>()
            {
                "Impulse",
                "TADEUSZE",
                "magnotum",
                "Janusze",
                "Onion PowaH",
                "FEROX",
                "Thriller Makers",
                "Szybka Sklejka",
                "1stCav Junior 2",
                "Pszyńskie Dziki",
            };
        }
        public List<string> getTeams()
        {
            return _teamsNames;
        }

        public void RemoveTeam(string name)
        {
            _teamsNames.Remove(name);
        }
    }
}
