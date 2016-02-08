using System.Reflection;
using System.Runtime.CompilerServices;
using Anotar.Custom;
using CroquetAustralia.Logging;

[assembly: AssemblyTitle("CroquetAustralia.CQRS.AzurePersistence")]
[assembly: AssemblyDescription("Persists CQRS data to Azure Table Storage.")]
[assembly: InternalsVisibleTo("CroquetAustralia.CQRS.AzurePersistence.Specifications")]
[assembly: LoggerFactory(typeof(LoggerFactory))]
[assembly: LogMinimalMessage]