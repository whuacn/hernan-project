﻿// Copyright 2005-2013 Giacomo Stelluti Scala & Contributors. All rights reserved. See doc/License.md in the project root for license information.

using System;

namespace CommandLine
{
    /// <summary>
    /// Discriminator enumeration of <see cref="CommandLine.Error"/> derivates.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Value of <see cref="CommandLine.BadFormatTokenError"/> type.
        /// </summary>
        BadFormatTokenError,
        /// <summary>
        /// Value of <see cref="CommandLine.MissingValueOptionError"/> type.
        /// </summary>
        MissingValueOptionError,
        /// <summary>
        /// Value of <see cref="CommandLine.UnknownOptionError"/> type.
        /// </summary>
        UnknownOptionError,
        /// <summary>
        /// Value of <see cref="CommandLine.MissingRequiredOptionError"/> type.
        /// </summary>
        MissingRequiredOptionError,
        /// <summary>
        /// Value of <see cref="CommandLine.MutuallyExclusiveSetError"/> type.
        /// </summary>
        MutuallyExclusiveSetError,
        /// <summary>
        /// Value of <see cref="CommandLine.BadFormatConversionError"/> type.
        /// </summary>
        BadFormatConversionError,
        /// <summary>
        /// Value of <see cref="CommandLine.SequenceOutOfRangeError"/> type.
        /// </summary>
        SequenceOutOfRangeError,
        /// <summary>
        /// Value of <see cref="CommandLine.NoVerbSelectedError"/> type.
        /// </summary>
        NoVerbSelectedError,
        /// <summary>
        /// Value of <see cref="CommandLine.BadVerbSelectedError"/> type.
        /// </summary>
        BadVerbSelectedError,
        /// <summary>
        /// Value of <see cref="CommandLine.HelpRequestedError"/> type.
        /// </summary>
        HelpRequestedError,
        /// <summary>
        /// Value of <see cref="CommandLine.HelpVerbRequestedError"/> type.
        /// </summary>
        HelpVerbRequestedError
    }

    /// <summary>
    /// Base type of all errors.
    /// </summary>
    /// <remarks>All errors are defined within the system. There's no reason to create custom derivate types.</remarks>
    public abstract class Error : IEquatable<Error>
    {
        private readonly ErrorType tag;

        internal Error(ErrorType tag)
        {
            this.tag = tag;
        }

        /// <summary>
        /// Error type discriminator, defined as <see cref="CommandLine.ErrorType"/> enumeration.
        /// </summary>
        public ErrorType Tag
        {
            get { return this.tag; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="System.Object"/>.</param>
        /// <returns><value>true</value> if the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>; otherwise, <value>false</value>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Error;
            if (other != null)
            {
                return this.Equals(other);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <remarks>A hash code for the current <see cref="System.Object"/>.</remarks>
        public override int GetHashCode()
        {
            return this.Tag.GetHashCode();
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance and a specified <see cref="CommandLine.Error"/> have the same value.
        /// </summary>
        /// <param name="other">The <see cref="CommandLine.Error"/> instance to compare.</param>
        /// <returns><value>true</value> if this instance of <see cref="CommandLine.Error"/> and <paramref name="other"/> have the same value; otherwise, <value>false</value>.</returns>
        public bool Equals(Error other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Tag.Equals(other.Tag);
        }
    }

    /// <summary>
    /// Base type of all errors related to bad token detection.
    /// </summary>
    public abstract class TokenError : Error, IEquatable<TokenError>
    {
        private readonly string token;

        internal TokenError(ErrorType tag, string token)
            : base(tag)
        {
            if (token == null) throw new ArgumentNullException("token");

            this.token = token;
        }

        /// <summary>
        /// The string containing the token text.
        /// </summary>
        public string Token
        {
            get { return this.token; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="System.Object"/>.</param>
        /// <returns><value>true</value> if the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>; otherwise, <value>false</value>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as TokenError;
            if (other != null)
            {
                return this.Equals(other);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <remarks>A hash code for the current <see cref="System.Object"/>.</remarks>
        public override int GetHashCode()
        {
            return this.Tag.GetHashCode() ^ this.Token.GetHashCode();
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance and a specified <see cref="CommandLine.TokenError"/> have the same value.
        /// </summary>
        /// <param name="other">The <see cref="CommandLine.TokenError"/> instance to compare.</param>
        /// <returns><value>true</value> if this instance of <see cref="CommandLine.TokenError"/> and <paramref name="other"/> have the same value; otherwise, <value>false</value>.</returns>
        public bool Equals(TokenError other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Tag.Equals(other.Tag) && this.Token.Equals(other.Token);
        }
    }

    /// <summary>
    /// Models an error generated when an invalid token is detected.
    /// </summary>
    public sealed class BadFormatTokenError : TokenError
    {
        internal BadFormatTokenError(string token)
            : base(ErrorType.BadFormatTokenError, token)
        {
        }
    }

    /// <summary>
    /// Base type of all erros with name information.
    /// </summary>
    public abstract class NamedError : Error, IEquatable<NamedError>
    {
        private readonly NameInfo nameInfo;

        internal NamedError(ErrorType tag, NameInfo nameInfo)
            : base(tag)
        {
            this.nameInfo = nameInfo;
        }

        /// <summary>
        /// Name information relative to this error instance.
        /// </summary>
        public NameInfo NameInfo
        {
            get { return this.nameInfo; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="System.Object"/>.</param>
        /// <returns><value>true</value> if the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>; otherwise, <value>false</value>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as NamedError;
            if (other != null)
            {
                return this.Equals(other);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <remarks>A hash code for the current <see cref="System.Object"/>.</remarks>
        public override int GetHashCode()
        {
            return this.Tag.GetHashCode() ^ this.NameInfo.GetHashCode();
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance and a specified <see cref="CommandLine.NamedError"/> have the same value.
        /// </summary>
        /// <param name="other">The <see cref="CommandLine.NamedError"/> instance to compare.</param>
        /// <returns><value>true</value> if this instance of <see cref="CommandLine.NamedError"/> and <paramref name="other"/> have the same value; otherwise, <value>false</value>.</returns>
        public bool Equals(NamedError other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Tag.Equals(other.Tag) && this.NameInfo.Equals(other.NameInfo);
        }
    }

    /// <summary>
    /// Models an error generated when an option lacks its value.
    /// </summary>
    public sealed class MissingValueOptionError : NamedError
    {
        internal MissingValueOptionError(NameInfo nameInfo)
            : base(ErrorType.MissingValueOptionError, nameInfo)
        {
        }
    }

    /// <summary>
    /// Models an error generated when an unknown option is detected.
    /// </summary>
    public sealed class UnknownOptionError : TokenError
    {
        internal UnknownOptionError(string token)
            : base(ErrorType.UnknownOptionError, token)
        {
        }
    }

    /// <summary>
    /// Models an error generated when a required option is required.
    /// </summary>
    public sealed class MissingRequiredOptionError : NamedError
    {
        internal MissingRequiredOptionError(NameInfo nameInfo)
            : base(ErrorType.MissingRequiredOptionError, nameInfo)
        {
        }
    }

    /// <summary>
    /// Models an error generated when a an option from another set is defined.
    /// </summary>
    public sealed class MutuallyExclusiveSetError : NamedError
    {
        internal MutuallyExclusiveSetError(NameInfo nameInfo)
            : base(ErrorType.MutuallyExclusiveSetError, nameInfo)
        {
        }
    }

    /// <summary>
    /// Models an error generated when a value conversion fails.
    /// </summary>
    public sealed class BadFormatConversionError : NamedError
    {
        internal BadFormatConversionError(NameInfo nameInfo)
            : base(ErrorType.BadFormatConversionError, nameInfo)
        {
        }
    }

    /// <summary>
    /// Models an error generated when a sequence value lacks elements.
    /// </summary>
    public sealed class SequenceOutOfRangeError : NamedError
    {
        internal SequenceOutOfRangeError(NameInfo nameInfo)
            : base(ErrorType.SequenceOutOfRangeError, nameInfo)
        {
        }
    }

    /// <summary>
    /// Models an error generated when an unknown verb is detected.
    /// </summary>
    public sealed class BadVerbSelectedError : TokenError
    {
        internal BadVerbSelectedError(string token)
            : base(ErrorType.BadVerbSelectedError, token)
        {
        }
    }

    /// <summary>
    /// Models an error generated when a user explicit requests help.
    /// </summary>
    public sealed class HelpRequestedError : Error
    {
        internal HelpRequestedError()
            : base(ErrorType.HelpRequestedError)
        {
        }
    }

    /// <summary>
    /// Models an error generated when a user explicit requests help in verb commands scenario.
    /// </summary>
    public sealed class HelpVerbRequestedError : Error
    {
        private readonly string verb;
        private readonly Type type;
        private readonly bool matched;

        internal HelpVerbRequestedError(string verb, Type type, bool matched)
            : base(ErrorType.HelpVerbRequestedError)
        {
            this.verb = verb;
            this.type = type;
            this.matched = matched;
        }

        /// <summary>
        /// Verb command string.
        /// </summary>
        public string Verb
        {
            get { return this.verb; }
        }

        /// <summary>
        /// <see cref="System.Type"/> of verb command.
        /// </summary>
        public Type Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// <value>true</value> if verb command is found; otherwise <value>false</value>.
        /// </summary>
        public bool Matched
        {
            get { return this.matched; }
        }
    }

    /// <summary>
    /// Models an error generated when no verb is selected.
    /// </summary>
    public sealed class NoVerbSelectedError : Error
    {
        internal NoVerbSelectedError()
            : base(ErrorType.NoVerbSelectedError)
        {
        }
    }
}