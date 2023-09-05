namespace LAMBusiness.Web.Data.Entities
{
    using LAMBusiness.Shared.Aplicacion;
    using LAMBusiness.Shared.Contacto;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class ModuloConfiguracion : IEntityTypeConfiguration<Modulo>
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            builder.HasData(
                //aplicaciones
                new Modulo
                {
                    ModuloID = new("37A8C12A-254F-44FB-BE68-67AF358B0610"),
                    Descripcion = "Aplicaciones",
                    ModuloPadreID = Guid.Empty,
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("36a7c3fa-a7fe-4288-988f-adcdfef9ef63"),
                    Descripcion = "Módulos",
                    ModuloPadreID = new("37A8C12A-254F-44FB-BE68-67AF358B0610"),
                    Activo = true
                },
                //Catálogos
                new Modulo
                {
                    ModuloID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Descripcion = "Catálogos",
                    ModuloPadreID = Guid.Empty,
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("da183d55-101e-4a06-9ec3-a1ed5729f0cb"),
                    Descripcion = "Almacenes",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("10acc48b-ad98-4500-bda5-d6a76e3e9dc9"),
                    Descripcion = "Géneros",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("08f4e254-ebbe-46eb-b005-9c6815d7f803"),
                    Descripcion = "Estados Civiles",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("f2c4db86-8c15-46bd-b8de-fb64de3bfcff"),
                    Descripcion = "Estados",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("1f1a8a70-239b-432c-afcd-71dff20d042c"),
                    Descripcion = "Formas de pago",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("7fcf442e-5377-47e0-b5f5-1c6a5067609c"),
                    Descripcion = "Marcas",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("46fdcc81-6ac7-4be4-84f3-4abae3a40ebb"),
                    Descripcion = "Municipios",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("a549419c-89bd-49ce-ba93-4d73afba37ce"),
                    Descripcion = "Productos",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("81ac4f4f-1886-4edf-ba65-c711e96a6e74"),
                    Descripcion = "Puestos",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("1b7183e2-e51b-4091-a99a-9bb38d462d81"),
                    Descripcion = "Tasas de impuestos",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                }, 
                new Modulo
                {
                    ModuloID = new("06239203-aced-4600-9f70-784bf73281ec"),
                    Descripcion = "Tipos de salidas",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                }, 
                new Modulo
                {
                    ModuloID = new("a01ed3a4-101e-4d64-b57e-ddb8f94f8680"),
                    Descripcion = "Unidades",
                    ModuloPadreID = new("50B65B8C-1CBA-47E4-8327-5F1A34375394"),
                    Activo = true
                },                
                //Contactos
                new Modulo
                {
                    ModuloID = new("25C76712-5552-44C5-93A1-298590F337FA"),
                    Descripcion = "Contactos",
                    ModuloPadreID = Guid.Empty,
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("0C36B7F4-02DF-459A-9606-CAEEB137D9B1"),
                    Descripcion = "Usuarios",
                    ModuloPadreID = new("25C76712-5552-44C5-93A1-298590F337FA"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("9B4E6E0C-6F34-4513-AACF-4A9A516CEDF6"),
                    Descripcion = "Clientes",
                    ModuloPadreID = new("25C76712-5552-44C5-93A1-298590F337FA"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("76BE358D-AA92-48E2-AB1A-5EE557441067"),
                    Descripcion = "Proveedores",
                    ModuloPadreID = new("25C76712-5552-44C5-93A1-298590F337FA"),
                    Activo = true
                },
                //Dashboard
                new Modulo
                {
                    ModuloID = new("C803EECE-79A9-4B7F-955C-3A0CC70BFEDB"),
                    Descripcion = "Dashboard",
                    ModuloPadreID = Guid.Empty,
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("50DECAF3-BFA7-42BB-B544-960178A93342"),
                    Descripcion = "Dashboard de movimientos",
                    ModuloPadreID = new("C803EECE-79A9-4B7F-955C-3A0CC70BFEDB"),
                    Activo = true
                },
                //Movimientos
                new Modulo
                {
                    ModuloID = new("4BA5D993-8BEB-48AF-A45F-4813825A658F"),
                    Descripcion = "Movimientos",
                    ModuloPadreID = Guid.Empty,
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("a0ca4d51-b518-4a65-b1e3-f0a03b1caff8"),
                    Descripcion = "Ventas",
                    ModuloPadreID = new("4BA5D993-8BEB-48AF-A45F-4813825A658F"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("aa3e3482-0ec5-40b8-8c48-7e567da135f6"),
                    Descripcion = "Retiros de caja",
                    ModuloPadreID = new("4BA5D993-8BEB-48AF-A45F-4813825A658F"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("7b848778-36f0-4254-b2a1-b822c9ab87b3"),
                    Descripcion = "Cortes de caja",
                    ModuloPadreID = new("4BA5D993-8BEB-48AF-A45F-4813825A658F"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("b019ebf0-5a25-4cc3-bd72-34fda134e5c1"),
                    Descripcion = "Entradas",
                    ModuloPadreID = new("4BA5D993-8BEB-48AF-A45F-4813825A658F"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("d6dc97d9-c3de-4920-a0b1-b63d7685bb6a"),
                    Descripcion = "Salidas",
                    ModuloPadreID = new("4BA5D993-8BEB-48AF-A45F-4813825A658F"),
                    Activo = true
                },
                new Modulo
                {
                    ModuloID = new("1d1048a7-44fe-44b5-abc8-cf0a4dfc0aff"),
                    Descripcion = "Devoluciones",
                    ModuloPadreID = new("4BA5D993-8BEB-48AF-A45F-4813825A658F"),
                    Activo = true
                }
            );
        }
    }
}
