﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPuzzleScorer.Domain
{
    public class StubWordChecker : IWordChecker
    {
        private SortedSet<string> validWords;

        public StubWordChecker()
        {
            string[] data = {
                "AEON",
                "AEONS",
                "AGE",
                "AGES",
                "AGLOW",
                "AGO",
                "ALE",
                "ALES",
                "ALOE",
                "ALOES",
                "ALONE",
                "ALONG",
                "ALSO",
                "AN",
                "ANEW",
                "ANGEL",
                "ANGLE",
                "AS",
                "AWE",
                "AWLS",
                "AWN",
                "AWOL",
                "EG",
                "EGO",
                "ELAN",
                "ELS",
                "EON",
                "EONS",
                "GAL",
                "GALE",
                "GALES",
                "GALS",
                "GAOL",
                "GAOLS",
                "GAS",
                "GEL",
                "GELS",
                "GEN",
                "GENOA",
                "GLANS",
                "GLEAN",
                "GLEN",
                "GLENS",
                "GLOW",
                "GLOWS",
                "GNAW",
                "GNAWS",
                "GO",
                "GOAL",
                "GOALS",
                "GOES",
                "GONE",
                "GOO",
                "GOON",
                "GOONS",
                "GOOSE",
                "GOWN",
                "GOWNS",
                "LAG",
                "LAGOS",
                "LAGS",
                "LANE",
                "LANES",
                "LAOS",
                "LAS",
                "LASE",
                "LAW",
                "LAWN",
                "LAWNS",
                "LAWS",
                "LEA",
                "LEAN",
                "LEANS",
                "LEG",
                "LEGS",
                "LENS",
                "LOAN",
                "LOANS",
                "LOG",
                "LOGO",
                "LOGOS",
                "LOGS",
                "LONE",
                "LONG",
                "LONGS",
                "LOO",
                "LOON",
                "LOOSE",
                "LOSE",
                "LOW",
                "LOWS",
                "NAG",
                "NAGS",
                "NE",
                "NEW",
                "NEWS",
                "NO",
                "NOEL",
                "NOOSE",
                "NOSE",
                "NOW",
                "OGLE",
                "OLE",
                "ON",
                "ONE",
                "ONES",
                "OSLO",
                "OW",
                "OWE",
                "OWES",
                "OWL",
                "OWLS",
                "OWN",
                "OWNS",
                "SAG",
                "SAGE",
                "SAGO",
                "SALE",
                "SALON",
                "SAN",
                "SANE",
                "SANG",
                "SAW",
                "SAWN",
                "SEA",
                "SEAL",
                "SEN",
                "SEW",
                "SEWN",
                "SLAG",
                "SLANG",
                "SLEW",
                "SLOG",
                "SLOW",
                "SNAG",
                "SNOW",
                "SO",
                "SOLE",
                "SOLO",
                "SON",
                "SONG",
                "SOON",
                "SOW",
                "SOWN",
                "SWAG",
                "SWAN",
                "SWOON",
                "WAG",
                "WAGE",
                "WAGES",
                "WAGON",
                "WAGS",
                "WALES",
                "WAN",
                "WANE",
                "WANES",
                "WAS",
                "WE",
                "WEAL",
                "WEAN",
                "WEANS",
                "WOE",
                "WOES",
                "WON",
                "WOO",
                "WOOL",
                "WOOLS",
                "WOOS",
                "WOW",
                "WOWS"
            };

            validWords = new SortedSet<string>(data);
        }


        public bool IsValid(string word)
        {
            return this.validWords.Contains(word);
        }
    }
}
