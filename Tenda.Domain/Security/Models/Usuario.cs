using Europa.Data.Model;
using Europa.Resources;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class Usuario : BaseEntity
    {
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "EmailInvalido", ErrorMessageResourceType = typeof(GlobalMessages))]
        [StringLength(DatabaseStandardDefinitions.OneHundredTwentyEigthLength,
            ErrorMessageResourceName = "TamanhoEmailExcedido", ErrorMessageResourceType = typeof(GlobalMessages))]
        public virtual string Email { get; set; }
        [Required(ErrorMessageResourceName = "CampoNomeObrigatorio", ErrorMessageResourceType = typeof(GlobalMessages))]
        [StringLength(DatabaseStandardDefinitions.OneHundredTwentyEigthLength,
            ErrorMessageResourceName = "TamanhoNomeExcedido", ErrorMessageResourceType = typeof(GlobalMessages))]
        public virtual string Nome { get; set; }
        [Required(ErrorMessageResourceName = "CampoLoginObrigatorio", ErrorMessageResourceType = typeof(GlobalMessages))]
        [StringLength(DatabaseStandardDefinitions.OneHundredTwentyEigthLength,
            ErrorMessageResourceName = "TamanhoLoginExcedido", ErrorMessageResourceType = typeof(GlobalMessages))]
        public virtual string Login { get; set; }
        [JsonIgnore]
        public virtual string Senha { get; set; }
        [Required(ErrorMessageResourceName = "CampoSituacaoObrigatorio", ErrorMessageResourceType = typeof(GlobalMessages))]
        public virtual SituacaoUsuario Situacao { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }

        public virtual List<ValidationResult> Validate()
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(this, context, results, true);

            return results;
        }
    }
}
