using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandCore.TestConsole.Library
{
    public static class CommandCore
    {
        public static int Parse(string[] args)
        {
            var verbTypes = GetVerbTypes();
            Console.WriteLine(verbTypes.Select(a => a.FullName).Aggregate((a, b) => $"{a}, {b}"));

            return 0;
        }

        private static IReadOnlyList<Type> GetVerbTypes() => Assembly.GetExecutingAssembly().GetTypes()
            .Where(a => a.BaseType != null & a.BaseType.IsGenericType &&
                        a.BaseType.GetGenericTypeDefinition() == typeof(Verb<>)).ToList();


        public static ParsedVerb GetDummyVerb => new ParsedVerb()
        {
            Options = new Dictionary<string, string>()
            {
                {"name", "Tarik"},
                {"lastname", "guney"}
            },
            VerbName = "person"
        };

        public class ParsedVerb
        {
            public string VerbName { get; set; }

            // ToDo: Ideally the value should be anything. I don't know how I should design this right now. 
            // The reason is simple: Some arguments are flag attributes.
            public IReadOnlyDictionary<string, string> Options { get; set; }
        }
    }
}