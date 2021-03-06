﻿using Echo;
using System;
using System.IO;

namespace Samples.RecordToFile
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting recording");

            // create an actual instance of external dependency
            var externalPartner = new ExternalDependency();

            // setup recording
            using (var output = new StreamWriter(@"output.echo"))
            {
                var writer = new EchoWriter(output);
                var recorder = new Recorder(writer);
                var interceptedPartner = recorder.GetRecordingTarget<IExternalDependency>(externalPartner);

                // call external dependency
                interceptedPartner.Concat("Hello", " ", "World");
                interceptedPartner.Concat("This is", " a proof ", "of concept...");
                interceptedPartner.Concat("... that", " writing to a file ", "works!");
            }

            Console.ReadKey();
        }
    }
}
