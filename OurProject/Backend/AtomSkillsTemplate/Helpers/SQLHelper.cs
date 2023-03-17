using System;
using System.Collections.Generic;

namespace AtomSkillsTemplate.Helpers
{
    public static class SQLHelper
    {
        public static SQLInQueryWithParameters TransformParameters(IEnumerable<long> values, char parameterName = 'p')
        {
            var returnObject = new SQLInQueryWithParameters()
            {
                InQuery = "",
                Parameters = new Dictionary<string, object>()
            };
            int i = 0;
            foreach(var value in values)
            {
                returnObject.InQuery += $":{parameterName}{i},";
                returnObject.Parameters.Add($"{parameterName}{i}", value);
                i++;
            }
         //   returnObject.InQuery = returnObject.InQuery.SubtractLast();

            if (String.IsNullOrEmpty(returnObject.InQuery))
            {
                return null;
            }

            return returnObject;
        }



    }
    public class SQLInQueryWithParameters
    {
        public string InQuery { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}
