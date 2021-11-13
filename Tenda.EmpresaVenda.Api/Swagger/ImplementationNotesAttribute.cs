using System;

namespace Tenda.EmpresaVenda.Api.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ImplementationNotesAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string ImplementationNotes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="implementationNotes"></param>
        public ImplementationNotesAttribute(string implementationNotes)
        {
            this.ImplementationNotes = implementationNotes;
        }
    }
}