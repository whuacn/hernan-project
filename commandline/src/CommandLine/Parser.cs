﻿// Copyright 2005-2013 Giacomo Stelluti Scala & Contributors. All rights reserved. See doc/License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine.Core;
using CommandLine.Infrastructure;
using CommandLine.Text;

namespace CommandLine
{
    /// <summary>
    /// Provides methods to parse command line arguments.
    /// </summary>
    public class Parser : IDisposable
    {
        private bool disposed;
        private readonly ParserSettings settings;
        private static readonly Lazy<Parser> @default = new Lazy<Parser>(
            () => new Parser(new ParserSettings{ HelpWriter = Console.Error }));

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.Parser"/> class.
        /// </summary>
        public Parser()
        {
            this.settings = new ParserSettings { Consumed = true };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class,
        /// configurable with <see cref="ParserSettings"/> using a delegate.
        /// </summary>
        /// <param name="configuration">The <see cref="Action&lt;ParserSettings&gt;"/> delegate used to configure
        /// aspects and behaviors of the parser.</param>
        public Parser(Action<ParserSettings> configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.settings = new ParserSettings();
            configuration(this.settings);
            this.settings.Consumed = true;
        }

        internal Parser(ParserSettings settings)
        {
            this.settings = settings;
            this.settings.Consumed = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CommandLine.Parser"/> class.
        /// </summary>
        ~Parser()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the singleton instance created with basic defaults.
        /// </summary>
        public static Parser Default
        {
            get { return @default.Value; }
        }

        /// <summary>
        /// Gets the instance that implements <see cref="CommandLine.ParserSettings"/> in use.
        /// </summary>
        public ParserSettings Settings
        {
            get { return this.settings; }
        }

        /// <summary>
        /// Parses a string array of command line arguments constructing values in an instance of type <typeparamref name="T"/>.
        /// Grammar rules are defined decorating public properties with appropriate attributes.
        /// </summary>
        /// <typeparam name="T">Type of the target instance built with parsed value.</typeparam>
        /// <param name="args">A <see cref="System.String"/> array of command line arguments, normally supplied by application entry point.</param>
        /// <returns>A <see cref="CommandLine.ParserResult{T}"/> containing an instance of type <typeparamref name="T"/> with parsed values
        /// and a sequence of <see cref="CommandLine.Error"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if one or more arguments are null.</exception>
        public ParserResult<T> ParseArguments<T>(string[] args)
            where T : new()
        {
            if (args == null) throw new ArgumentNullException("args");

            return ParseArguments(() => new T(), args);
        }

        /// <summary>
        /// Parses a string array of command line arguments constructing values in an instance of type <typeparamref name="T"/>.
        /// Grammar rules are defined decorating public properties with appropriate attributes.
        /// </summary>
        /// <typeparam name="T">Type of the target instance built with parsed value.</typeparam>
        /// <param name="factory">A <see cref="System.Func{T}"/> delegate used to intitalize the target instance.</param>
        /// <param name="args">A <see cref="System.String"/> array of command line arguments, normally supplied by application entry point.</param>
        /// <returns>A <see cref="CommandLine.ParserResult{T}"/> containing an instance of type <typeparamref name="T"/> with parsed values
        /// and a sequence of <see cref="CommandLine.Error"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if one or more arguments are null.</exception>
        public ParserResult<T> ParseArguments<T>(Func<T> factory, string[] args)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (args == null) throw new ArgumentNullException("args");

            return MakeParserResult(
                () => InstanceBuilder.Build(
                    factory,
                    (arguments, optionSpecs) => Tokenize(arguments, optionSpecs, this.settings),
                    args,
                    this.settings.NameComparer,
                    this.settings.ParsingCulture),
                settings);
        }

        /// <summary>
        /// Parses a string array of command line arguments for verb commands scenario, constructing the proper instance from the array of types supplied by <paramref name="types"/>.
        /// Grammar rules are defined decorating public properties with appropriate attributes.
        /// The <see cref="CommandLine.VerbAttribute"/> must be applied to types in the array.
        /// </summary>
        /// <param name="args">A <see cref="System.String"/> array of command line arguments, normally supplied by application entry point.</param>
        /// <param name="types">A <see cref="System.Type"/> array used to supply verb alternatives.</param>
        /// <returns>A <see cref="CommandLine.ParserResult{T}"/> containing the appropriate instance with parsed values as a <see cref="System.Object"/>
        /// and a sequence of <see cref="CommandLine.Error"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if one or more arguments are null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="types"/> array is empty.</exception>
        /// <remarks>All types must expose a parameterless constructor. It's stronly recommended to use a generic overload.</remarks>
        public ParserResult<object> ParseArguments(string[] args, params Type[] types)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (types == null) throw new ArgumentNullException("types");
            if (types.Length == 0) throw new ArgumentOutOfRangeException("types");

            return MakeParserResult(
                () => InstanceChooser.Choose(
                    (arguments, optionSpecs) => Tokenize(arguments, optionSpecs, this.settings),
                    types,
                    args,
                    this.settings.NameComparer,
                    this.settings.ParsingCulture),
                settings);
        }

        /// <summary>
        /// Frees resources owned by the instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private static StatePair<IEnumerable<Token>> Tokenize(
                IEnumerable<string> arguments,
                IEnumerable<OptionSpecification> optionSpecs,
                ParserSettings settings)
        {
            return settings.EnableDashDash
                ? Tokenizer.PreprocessDashDash(
                        arguments,
                        args =>
                            Tokenizer.Tokenize(args, name => NameLookup.Contains(name, optionSpecs, settings.NameComparer)))
                : Tokenizer.Tokenize(arguments, name => NameLookup.Contains(name, optionSpecs, settings.NameComparer));
        }

        private static ParserResult<T> MakeParserResult<T>(Func<ParserResult<T>> parseFunc, ParserSettings settings)
        {
            return DisplayHelp(
                HandleUnknownArguments(
                    parseFunc(),
                    settings.IgnoreUnknownArguments),
                settings.HelpWriter);
        }

        private static ParserResult<T> HandleUnknownArguments<T>(ParserResult<T> parserResult, bool ignoreUnknownArguments)
        {
            return ignoreUnknownArguments
                       ? parserResult.MapErrors(errs => errs.Where(e => e.Tag != ErrorType.UnknownOptionError))
                       : parserResult;
        }

        private static ParserResult<T> DisplayHelp<T>(ParserResult<T> parserResult, TextWriter helpWriter)
        {
            if (parserResult.Errors.Any())
            {
                helpWriter.ToMaybe().Do(writer => writer.Write(HelpText.AutoBuild(parserResult)));
            }

            return parserResult;
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (settings != null)
                {
                    settings.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}