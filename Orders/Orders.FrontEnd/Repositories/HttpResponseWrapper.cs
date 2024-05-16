using System.Net;

namespace Orders.FrontEnd.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public T? Response { get; }
        public bool Error { get; }
        public HttpResponseMessage HttpResponseMessage { get; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponseMessage.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return "Recurso no encontrado.";

                case HttpStatusCode.BadRequest:
                    return await HttpResponseMessage.Content.ReadAsStringAsync();

                case HttpStatusCode.Unauthorized:
                    return "Tienes que estar logueado para ejecutar esta operación.";

                case HttpStatusCode.Forbidden:
                    return "No tienes permisos para hacer esta operación.";
            }

            return "Ha ocurrido un error inesperado";
        }
    }
}