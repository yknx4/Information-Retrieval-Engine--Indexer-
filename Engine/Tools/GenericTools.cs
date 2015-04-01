using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tools
{
    abstract class GenericTools
    {
        public static string NumberStatementParameter(string query, string parameter, int position)
        {
            return query.Replace(parameter,parameter + position); 
        }
        public static string NumberStatementParameter(string query, string[] parameters, int position)
        {
            foreach (var parameter in parameters)
            {
                query = query.Replace(parameter, parameter + position);
            }
            return query;
        }

        private static string BasicFillParameter(string query, string parameter, string value)
        {
            query = query.Replace(parameter, value );
            return query;
        }

        public static string FillParameter(string query, string parameter, string value)
        {
            return BasicFillParameter(query, parameter, "'" + value + "'");
        }

        public static string FillParameter(string query, string parameter, int value)
        {
            return BasicFillParameter(query, parameter, value.ToString());
        }
    }
}
