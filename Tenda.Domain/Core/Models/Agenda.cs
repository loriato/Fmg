using Europa.Data.Model;
using System;
using System.Collections.Generic;
using Tenda.Domain.Core.Enums;


namespace Tenda.Domain.Core.Models
{
    public class Agenda : BaseEntity
    {
        public virtual UsuarioPortal Criador { get; set; }
        public virtual DateTime DataCriacao { get; set; }
        public virtual DateTime InicioAgenda { get; set; }
        public virtual DateTime FimAgenda { get; set; }
        public virtual TimeSpan HoraInicio { get; set; }
        public virtual TimeSpan HoraFim { get; set; }
        public virtual int Dias { get; set; }
        public virtual int TempoSlot { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }

        public virtual string FormatDiasAsString()
        {
            string result;
            var msg = new List<string>();
            var tempDias = Dias;
            switch (tempDias)
            {
                case (62):
                    result = "Dias da Semana";
                    break;
                case (65):
                    result = "Final de Semana";
                    break;
                case (127):
                    result = "Todos os Dias";
                    break;
                default:
                    if (tempDias >= 64)
                    {
                        msg.Add(" Sábado");
                        tempDias = tempDias - 64;
                    }

                    if (tempDias >= 32)
                    {
                        msg.Add(" Sexta");
                        tempDias = tempDias - 32;
                    }

                    if (tempDias >= 16)
                    {
                        msg.Add(" Quinta");
                        tempDias = tempDias - 16;
                    }

                    if (tempDias >= 8)
                    {
                        msg.Add(" Quarta");
                        tempDias = tempDias - 8;
                    }

                    if (tempDias >= 4)
                    {
                        msg.Add(" Terça");
                        tempDias = tempDias - 4;
                    }

                    if (tempDias >= 2)
                    {
                        msg.Add(" Segunda");
                        tempDias = tempDias - 2;
                    }

                    if (tempDias >= 1)
                    {
                        msg.Add(" Domingo");
                        tempDias = tempDias - 1;
                    }

                    msg.Reverse();
                    result = string.Join(",", msg.ToArray());
                    break;
            }

            return result;
        }
    }
}