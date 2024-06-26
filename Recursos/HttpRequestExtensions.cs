namespace ObligatorioProgram3.Recursos
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }

            return false;
        }
    }

    //EVENTO PARA PREGUNTAR SI HAY UNA solicitud AXAJ, se devolvería una vista parcial - Implementado en RESERVAS (index reservas y _ReservasTable)
}
