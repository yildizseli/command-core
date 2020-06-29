namespace CommandCore.TestConsole.Library
{
    public abstract class Verb<T>: IVerb where T: VerbOptions
    {
        public T? Options { get; set; }
        
        public abstract void Run();
    }
}