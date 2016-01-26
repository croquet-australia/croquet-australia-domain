using System;

namespace CroquetAustralia.Domain.App
{
    public class SetupCannotBeRepeatedException : Exception
    {
        public SetupCannotBeRepeatedException() : base("Setup cannot be repeated.")
        {
        }
    }
}