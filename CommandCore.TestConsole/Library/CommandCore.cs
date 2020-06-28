using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CommandCore.TestConsole.Library
{
    public static class CommandCore
    {
        public static int Parse(string[] args)
        {
            var verbTypes = GetVerbTypes();
            Console.WriteLine(verbTypes.Select(a => a.FullName).Aggregate((a, b) => $"{a}, {b}"));
            var parsedVerb = GetDummyVerb();
            var verbType = FindVerbTypeByName(parsedVerb.VerbName, verbTypes);
            var optionsType = GetAssociatedOptionsType(verbType);
            
            // TODO Create an instance of the Verb type and set the Options property with the new Options instance.
            var options = CreatePopulatedOptionsObject(optionsType, parsedVerb);

            SetOptionsOfVerb(verbType, options);

            return 0;
        }

        private static void SetOptionsOfVerb(Type verbType, VerbOptions options)
        {
            var verb = Activator.CreateInstance(verbType);
            var optionsPropertyInfo = verbType.GetProperty("Options");
            optionsPropertyInfo.SetValue(verb, options);
        }

        private static VerbOptions CreatePopulatedOptionsObject(Type optionsType, ParsedVerb parsedVerb)
        {
            var options = (VerbOptions) Activator.CreateInstance(optionsType);
            var optionProperties =
                optionsType.GetProperties(BindingFlags.SetProperty & BindingFlags.Public & BindingFlags.Instance);
            foreach (var propertyInfo in optionProperties)
            {
                // TODO: argument must be lower case for both CLI argument and the property name to match.
                propertyInfo.SetValue(options, parsedVerb.Options[propertyInfo.Name.ToLower()]);
            }

            return options;
        }

        private static Type GetAssociatedOptionsType(Type verb)
        {
            return verb.BaseType.GetGenericArguments()[0];;
        }

        private static Type? FindVerbTypeByName(string verbName, IReadOnlyList<Type> allTypes)
        {
            return allTypes.FirstOrDefault(t =>
                t.FullName.Equals(verbName, StringComparison.InvariantCultureIgnoreCase));
        }

        private static IReadOnlyList<Type> GetVerbTypes() => Assembly.GetExecutingAssembly().GetTypes()
            .Where(a => a.BaseType != null & a.BaseType.IsGenericType &&
                        a.BaseType.GetGenericTypeDefinition() == typeof(Verb<>)).ToList();

        private static ParsedVerb GetDummyVerb() => new ParsedVerb()
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