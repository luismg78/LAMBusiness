namespace LAMBusiness.Shared.Aplicacion
{
    public class Resultado<T>
    {
        public bool Error { get; set; }
        public string Mensaje { get; set; }
        public T Contenido { get; set; }
    }
}
