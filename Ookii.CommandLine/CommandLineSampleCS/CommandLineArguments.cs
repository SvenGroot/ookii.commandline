﻿// $Id: CommandLineArguments.cs 28 2011-06-26 06:42:21Z sgroot $
//
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Ookii.CommandLine;

namespace CommandLineSampleCS
{
    /// <summary>
    /// Class that defines the sample's command line arguments.
    /// </summary>
    [Description("Sample command line application. The application parses the command line and prints the results, but otherwise does nothing, and none of the arguments are actually used for anything.")]
    class CommandLineArguments
    {
        public CommandLineArguments([Description("The source data.")] string source,
                                    [Description("The destination data.")] string destination,
                                    [Description("The operation's index. This argument is optional, and the default value is 1."), ValueDescription("Number")] int index = 1)
        {
            // The parameters to the constructor become the positional arguments for the command line application.
            // Because the third parameter is an optional parameter, the corresponding command line argument is also optional, and its default value will be supplied if it is omitted.
            // The Description attribute for each parameter will be used when printing command line usage information.
            // The third parameter uses a custom value description so it will be shown as "[/index] <Number>" in the usage, rather than "[/index] <Int32>"
            if( source == null )
                throw new ArgumentNullException("source");
            if( destination == null )
                throw new ArgumentNullException("destination");

            Source = source;
            Destination = destination;
            Index = index;
        }

        // These properties are just so the Main function can retrieve the values of the positional arguments.
        // They are not used by the CommandLineParser because they have no NamedCommandLineArgument attribute.
        public string Source { get; private set; }
        public string Destination { get; private set; }
        public int Index { get; private set; }

        // This defines a named argument that can be set from the command line by using "/id value" or "/id:value" (on Unix, the named argument switch
        // is a dash (-) rather than a forward slash (/) by default; the named argument switch can be customized using the CommandLineParser.NamedArgumentSwitch property).
        // If the argument is not supplied, the CommandLineParser.Parse method will set it to the default value, which is "default" in this case.
        // This argument is a so-called named positional argument; it is a named argument, but can also be specified without the name, by its position on
        // the command line. Named positional arguments come after regular positional arguments, so this will be the fourth positional argument,
        // even though it says Position=0; that just means this is the first named positional argument (and indeed the only one in this case).
        // The Description attribute is used when printing command line usage information.
        [CommandLineArgument("id", DefaultValue = "default"), Description("Sets the operation ID. The default value is \"default\".")]
        public string Identifier { get; set; }

        // This defines a named argument whose name matches the property name ("Date" in this case). Note that the default comparer for
        // named arguments is case insensitive, so "/date" will work as well as "/Date" (or any other capitalization).
        // This named argument uses a nullable value type so you can easily tell when it has not been specified even when the type is not a reference type.
        // For types other than string, CommandLineParser will use the TypeConverter for the argument's type to try to convert the string to
        // the correct type. You can use your own custom classes or structures for command line arguments as long as you create a TypeConverter for
        // the type.
        // If you need more control over the conversion, it can be more beneficial to create a string argument and do the conversion
        // yourself. For example, DateTime's TypeConverter uses the current user's locale; if you want to force a specific format
        // you could use a string argument and parse it manually with DateTime.ParseExact.
        [CommandLineArgument(), Description("Provides a date to the application; the format to use depends on your regional settings.")]
        public DateTime? Date { get; set; }

        // Another named argument whose name matches the property name ("Count" in this case).
        // This argument uses a custom ValueDescription so it shows up as "/Count <Number>" in the usage rather than as "/Count <Int32>"
        [CommandLineArgument(ValueDescription = "Number"), Description("Provides the count for something to the application. This argument is required.")]
        public int Count { get; set; }

        // Named arguments whose type is "bool" act as a switch; if they are supplied on the command line, their value will be true, otherwise
        // it will be false. You don't need to specify a value, just specify /v to set it to true. You can explicitly set the value by
        // using /v:true or /v:false if you want, but it is not needed.
        [CommandLineArgument("v"), Description("Print verbose information.")]
        public bool Verbose { get; set; }

        // This is a named argument with an array type, which means it can be specified multiple times. Every time it is specified, the value will be added to the array.
        // To set multiple values, simply repeat the /val argument, e.g. /val:foo /val:bar /val:baz will set it to an array containing { "foo", "bar", "baz" }
        // Since no default value is specified, the property will be null if /val is not supplied at all.
        // Array arguments don't have to be string arrays, you can use an int[], DateTime[], YourClass[], anything works as long as a TypeConverter exists for the class
        // that can convert from a string to that type.
        [CommandLineArgument("val"), Description("This is an example of an array argument, which can be repeated multiple times to set more than one value.")]
        public string[] Values { get; set; }

        // This is another switch argument, like /v above. In Program.cs, we handle the CommandLineParser.ArgumentParsed event to cancel
        // command line processing when this argument is supplied. That way, we can print usage regardless of what other arguments are
        // present. For more details, see the CommandLineParser.ArgumentParser event handler in Program.cs
        [CommandLineArgument("?"), Description("Displays this help message.")]
        public bool Help { get; set; }
    }
}
