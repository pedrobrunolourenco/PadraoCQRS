﻿using FluentValidation.Results;
using MediatR;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Lartech.Domain.Core.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {

        public DateTime Timestamp { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
