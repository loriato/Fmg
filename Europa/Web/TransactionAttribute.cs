using System;

namespace Europa.Web
{
    public class TransactionAttribute : Attribute
    {
        public readonly TransactionAttributeType Type;

        /// <summary>
        /// Use TransactionAttributeType.None as Default
        /// </summary>
        public TransactionAttribute()
        {
            Type = TransactionAttributeType.None;
        }

        /// <summary>
        /// To define a custom Transaction Type
        /// </summary>
        /// <param name="transactionType"></param>
        public TransactionAttribute(TransactionAttributeType transactionType)
        {
            Type = transactionType;
        }

    }
}
