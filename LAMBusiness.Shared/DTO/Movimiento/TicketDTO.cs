﻿using System.Collections.Generic;
using System.Linq;

namespace LAMBusiness.Shared.DTO.Movimiento
{
    public class TicketDTO
    {
        //Datos de la razón social
        public string RFC { get; set; }
        public string NombreCorto { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Colonia { get; set; }
        public string Telefono { get; set; }
        public string Lugar { get; set; }
        public string Slogan { get; set; }
        //-----
        public string Sucursal { get; set; }
        public string AtendidoPor { get; set; }
        public string Folio { get; set; }
        public string Fecha { get; set; }
        public string SubtotalDeVenta { get; set; }
        public string PorcentajeTotalCobroExtra { get; set; }
        public string ImporteTotalDeVenta { get; set; }
        //corte de caja
        public string ImporteTotalDeSistema { get; set; }
        public string ImporteTotalDeRetiros { get; set; }
        public decimal Cambio { get; set; }
        public List<FormasDePagoDTO> FormasDePagoDetalle { get; set; } = null!;

        public List<TicketDetalleDTO> DetalleDeVenta { get; set; }
    }   
}
