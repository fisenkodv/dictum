using System;

namespace Dictum.Data
{
    internal class IdGenerator
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int Size = 10;

        private static readonly Lazy<IdGenerator> Lazy = new Lazy<IdGenerator>(() => new IdGenerator());

        public static IdGenerator Instance => Lazy.Value;

        private IdGenerator()
        {
        }

        public string Next()
        {
            return Nanoid.Nanoid.Generate(Alphabet, Size);
        }
    }
}