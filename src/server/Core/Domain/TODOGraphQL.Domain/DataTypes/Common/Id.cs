﻿using System;

namespace TODOGraphQL.Domain.DataTypes.Common
{
    public class Id
    {
        private readonly Guid _value;

        public Id (Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new Exception($"{nameof(Id)} should not be empty");
            }
            _value = value;
        }

        public static implicit operator Guid(Id id)
        {
            return id._value;
        }

        public static implicit operator Id(Guid id)
        {
            return new Id(id);
        }

        public static implicit operator Guid?(Id id)
        {
            return id?._value;
        }

        public static implicit operator Id(Guid? id)
        {
            return id.HasValue 
                ? new Id(id.Value)
                : null;
        }

        public override string ToString() => _value.ToString();

        public static bool operator == (Id id1, Id id2)
        {
            return id1?._value == id2?._value;
        }

        public static bool operator != (Id id1, Id id2)
        {
            return !(id1 == id2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Id id)
            {
                return _value.Equals(id._value);
            }
            return _value.Equals(obj);
        }

        public override int GetHashCode() =>
            _value.GetHashCode();
    }
}
