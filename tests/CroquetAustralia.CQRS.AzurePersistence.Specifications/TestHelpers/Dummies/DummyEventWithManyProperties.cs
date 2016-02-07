using System;
using OpenMagic;

namespace CroquetAustralia.CQRS.AzurePersistence.Specifications.TestHelpers.Dummies
{
    public class DummyEventWithManyProperties : IEvent
    {
        public DummyEventWithManyProperties(
            bool boolean,
            DateTime dateTime,
            decimal @decimal,
            double @double,
            int @int,
            long @long,
            string @string
            /* possible future use

                byte @byte, 
                char @char, 
                float @float, 
                decimal? nullableDecimal, 
                uint uInt, 
                ulong uLong, 
                ushort uShort, 
                sbyte sByte, 
                short @short
            */
            )
        {
            Boolean = boolean;
            Decimal = @decimal;
            DateTime = dateTime;
            Double = @double;
            Int = @int;
            Long = @long;
            String = @string;

            /*
            possible future use

                Byte = @byte;
                Char = @char;
                Float = @float;
                NullableDecimal = nullableDecimal;
                SByte = sByte;
                Short = @short;
                UInt = uInt;
                ULong = uLong;
                UShort = uShort;
            */
        }

        public bool Boolean { get; }
        public DateTime DateTime { get; }
        public decimal Decimal { get; }
        public double Double { get; }
        public int Int { get; }
        public long Long { get; }
        public string String { get; }

        /* possible future use

            public byte Byte { get; }
            public char Char { get; }
            public float Float { get; }
            public decimal? NullableDecimal { get; }
            public sbyte SByte { get; }
            public short Short { get; }
            public uint UInt { get; }
            public ulong ULong { get; }
            public ushort UShort { get; }
        */

        public static DummyEventWithManyProperties RandomValue()
        {
            return new DummyEventWithManyProperties(
                RandomBoolean.Next(),
                RandomDateTime.Next(),
                RandomNumber.NextDecimal(),
                RandomNumber.NextDouble(),
                RandomNumber.NextInt(),
                RandomNumber.NextLong(),
                RandomString.Next()
                /* possible future use

                    RandomNumber.NextByte(), 
                    RandomNumber.NextChar(), 
                    RandomNumber.NextFloat(), 
                    RandomNumber.NextNullableDecimal(), 
                    RandomNumber.NextUInt(), 
                    RandomNumber.NextULong(), 
                    RandomNumber.NextUShort(), 
                    RandomNumber.NextSByte(), 
                    RandomNumber.NextShort()
                */
                );
        }
    }
}