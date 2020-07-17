using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using System.Text.RegularExpressions;

namespace IndianBank_ChatBOT.Utils
{
    public static class AmountUtils
    {
        static AmountUtils()
        {
            RegexPatterns = new[]
            {
                new RegexPattern(@"(\d+) rupees*\b", 1),	//Matches "50 rupee(s)"
			    new RegexPattern(@"rupees*\b (\d+)", 1),	//Matches "rupee(s) 50"
			    new RegexPattern(@"(\d+)", 1),				//Matches "50"
                new RegexPattern(@"my loan amount is (\d+) rupees*\b", 1),	    //Matches "my loan amount is 50 rupee(s)"
                new RegexPattern(@"my loan amount is rupees*\b (\d+)", 1),	//Matches "rupee(s) 50"
                new RegexPattern(@"loan amount is (\d+) rupees*\b", 1),	    //Matches "my loan amount is 50 rupee(s)"
                new RegexPattern(@"loan amount is rupees*\b (\d+)", 1),	//Matches "rupee(s) 50"
		    };
        }

        public static int Parse(string utterance)
        {
            int loanAmount = TryParse(utterance);

            if (loanAmount != 0)
                return loanAmount;

            return 0;
        }

        private static readonly RegexPattern[] RegexPatterns;

        private static int TryParse(string utterance)
        {
            utterance = ConvertNumbersInWords(utterance).ToLowerInvariant();

            int amountEntered = 0;

            foreach (var pattern in RegexPatterns)
            {
                var match = Regex.Match(utterance, pattern.Pattern);

                if (match.Success)
                {
                    if (int.TryParse(match.Groups[pattern.AmountIndex].Value, out var unitsVal))
                        amountEntered = unitsVal;
                }

                if (amountEntered != 0)
                {
                    return amountEntered;
                }
            }

            return 0;
        }

        private static string ConvertNumbersInWords(string utterance)
        {
            var parseResult = NumberRecognizer.RecognizeNumber(utterance, Culture.EnglishOthers);

            foreach (var result in parseResult)
            {
                utterance = utterance.Replace(result.Text, result.Resolution["value"].ToString());
            }

            utterance = utterance.ToLowerInvariant().Replace("hundred", "100");


            return utterance;
        }

        private struct RegexPattern
        {
            public RegexPattern(string pattern, int amountIndex)
            {
                Pattern = pattern;
                AmountIndex = amountIndex;
            }

            public string Pattern { get; set; }
            public int AmountIndex { get; set; }
        }
    }
}
