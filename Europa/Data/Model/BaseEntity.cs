using System;
using System.Collections.Generic;

namespace Europa.Data.Model
{
    public abstract class BaseEntity<TKeyType> : IBaseEntity
    {

        public virtual TKeyType Id { get; set; }

        public virtual long CriadoPor { get; set; }

        public virtual DateTime CriadoEm { get; set; } = DateTime.Now;

        public virtual long AtualizadoPor { get; set; }

        public virtual DateTime AtualizadoEm { get; set; } = DateTime.Now;

        public abstract string ChaveCandidata();

        public virtual Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("Id", Id);
            dictionary.Add("CriadoPor", CriadoPor);
            dictionary.Add("CriadoEm", CriadoEm);
            dictionary.Add("AtualizadoPor", AtualizadoPor);
            dictionary.Add("AtualizadoEm", AtualizadoEm);
            return dictionary;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is BaseEntity<TKeyType>)
            {
                return Id.Equals((obj as BaseEntity<TKeyType>).Id);
            }
            return false;
        }

        public static bool operator ==(BaseEntity<TKeyType> c1, BaseEntity<TKeyType> c2)
        {
            if (ReferenceEquals(c1, null))
            {
                return ReferenceEquals(c2, null);
            }
            if (ReferenceEquals(c2, null))
            {
                return false;

            }
            return c1.Id.Equals(c2.Id);
        }

        public static bool operator !=(BaseEntity<TKeyType> c1, BaseEntity<TKeyType> c2)
        {
            if (ReferenceEquals(c1, null))
            {
                return !ReferenceEquals(c2, null);
            }
            if (ReferenceEquals(c2, null))
            {
                return true;
            }
            return !c1.Id.Equals(c2.Id);
        }

    }

    public abstract class BaseEntity : BaseEntity<Int64>
    {

    }

    public class ControlBaseEntity : BaseEntity
    {
        public virtual string NomeCriadoPor { get; set; }
        public virtual string NomeAtualizadoPor { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }

    public interface IBaseEntity { }

}