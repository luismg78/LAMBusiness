﻿using LAMBusiness.Contextos;
using LAMBusiness.Shared.Aplicacion;
using LAMBusiness.Shared.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace LAMBusiness.Backend
{
    public class FormasDePagos
    {
        private readonly DataContext _contexto;

        public FormasDePagos(DataContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Resultado<FormaPago>> ObtenerRegistroPredeterminadoAsync()
        {
            Resultado<FormaPago> resultado = new();
            var formaDePago = await _contexto.FormasPago.FirstOrDefaultAsync(f => f.ValorPorDefault == true);
            if(formaDePago == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "La forma de pago predeterminada, no está configurada";
                return resultado;
            }

            formaDePago.PorcentajeDeCobroExtra ??= 0;
            formaDePago.TextoPorCobroExtra ??= "";

            resultado.Datos = formaDePago;
            return resultado;
        }
        public async Task<Resultado<FormaPago>> ObtenerRegistroAsync(byte? id)
        {
            Resultado<FormaPago> resultado = new();
            if (id == null || id == 0)
            {
                resultado.Error = true;
                resultado.Mensaje = "El identificador de la forma de pago es incorrecto.";
                return resultado;
            }

            var formaDePago = await _contexto.FormasPago.FindAsync(id);
            if (formaDePago == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "No hay registro de formas de pago, verifique catálogo en el sistema administrador.";
                return resultado;
            }

            resultado.Datos = formaDePago;
            return resultado;
        }
        public async Task<Resultado<List<FormaPago>>> ObtenerRegistrosAsync()
        {
            Resultado<List<FormaPago>> resultado = new();
            var formaDePago = await _contexto.FormasPago.ToListAsync();
            if (formaDePago == null)
            {
                resultado.Error = true;
                resultado.Mensaje = "No hay registro de formas de pago, verifique catálogo en el sistema administrador.";
                return resultado;
            }

            resultado.Datos = formaDePago;
            return resultado;
        }
    }
}
